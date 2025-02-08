using AutoMapper;
using eCommerceApp.Application.DTOs;
using eCommerceApp.Application.DTOs.Identity;
using eCommerceApp.Application.Services.Interfaces.Authentication;
using eCommerceApp.Application.Services.Interfaces.Logging;
using eCommerceApp.Application.Validations;
using eCommerceApp.Domain.Entities.Identity;
using eCommerceApp.Domain.Interfaces.Authentication;
using FluentValidation;

namespace eCommerceApp.Application.Services.Implementations.Authentication;

public class AuthenticationService(ITokenManagement tokenManagement, 
                                   IUserManagement userManagement, 
                                   IRoleManagement roleManagement,
                                   IMapper mapper,
                                   IValidator<CreateUserRequest> createUserRequestValidator,
                                   IValidator<LoginRequest> loginRequestValidator,
                                   IValidationService validationService,
                                   IAppLogger<AuthenticationService> logger) : IAuthenticationService
{
    public async Task<ServiceResponse> CreateUser(CreateUserRequest user)
    {
        var validationResult = await validationService.ValidateAsync(user, createUserRequestValidator);
        if(!validationResult.Success) return validationResult;

        var mappedModel = mapper.Map<AppUser>(user);
        mappedModel.UserName = user.Email;
        mappedModel.PasswordHash = user.Password;
        
        // Create the user
        var result = await userManagement.CreateUser(mappedModel);
        if (!result) 
        return new ServiceResponse
            {
                Message = "Email Address might be already in use or unknown error occured."
            };
        
        // Assign Role to the user
        var dbUser = await userManagement.GetUserByEmail(user.Email);
        var users = await userManagement.GetAllUsers();
        bool assignedResult = await roleManagement.AddUserToRole(dbUser!, users!.Count() >1 ? "User" : "Admin");
        if(!assignedResult)
        {
            // remove already added user
            int removeUserResult = await userManagement.RemoveUserByEmail(dbUser!.Email!);
            if(removeUserResult < 0)
            {
                logger.LogError(
                    new Exception($"User with Email as {dbUser.Email} failed to be remove as a result of role assigning isssue."),
                    "User could not be assigned role");
                return new ServiceResponse { Message = "Error occured in creating account." };
            }
        }

        return new ServiceResponse { Success = true, Message = "Account Created" };

        // Send Email to verif the email
    }

    public async Task<LoginResponse> Login(LoginRequest user)
    {
        // Validate Request
        var validationResult = await validationService.ValidateAsync(user, loginRequestValidator);
        if(!validationResult.Success)
            return new LoginResponse(Message: validationResult.Message!);

        // Automap
        var mappedModel = mapper.Map<AppUser>(user);
        mappedModel.PasswordHash = user.Password;

        // Login user
        bool loginResult = await userManagement.LoginUser(mappedModel);
        if (!loginResult)
            return new LoginResponse(Message: "Email not found or invalid credentials");

        // Get Claims
        var dbUser = await userManagement.GetUserByEmail(user.Email);
        var claims = await userManagement.GetUserClaims(dbUser!.Email!);

        // Generate Token and Refresh token with the retrievd claims
        string jwtToken = tokenManagement.GenerateToken(claims);
        string refreshToken = tokenManagement.GetRefreshToken();

        int saveTokenResult = 0;
        bool userTokenCheck = await tokenManagement.ValidateRefreshToken(refreshToken);

        if (userTokenCheck)
            saveTokenResult = await tokenManagement.UpdateRefreshToken(dbUser.Id, refreshToken);
        else
            // Save the Refresh token in database
            saveTokenResult = await tokenManagement.AddRefreshToken(dbUser.Id, refreshToken);

        // If any issue in storing the refresh token return error response. Else return the Tokens
        return saveTokenResult <= 0 ? new LoginResponse(Message: "Internal error occured while authenticating") :
                                     new LoginResponse(Success: true, Token: jwtToken, RefreshToken: refreshToken);

       
    }


    public async Task<LoginResponse> ReviewToken(string refreshToken)
    {
        bool validateTokenResult = await tokenManagement.ValidateRefreshToken(refreshToken);
        if (!validateTokenResult)
            return new LoginResponse(Message: "Invalid token");

        string userId = await tokenManagement.GetUserIdByRefreshToken(refreshToken);
        AppUser? dbUser = await userManagement.GetUserById(userId);
        var claims = await userManagement.GetUserClaims(dbUser!.Email!);
        string newJwtToken = tokenManagement.GenerateToken(claims);
        string newRefreshToken = tokenManagement.GetRefreshToken();

        int result = await tokenManagement.UpdateRefreshToken(userId, newRefreshToken);
        return result <=0 ? new LoginResponse(Message: "Internal error occured while authenticating.") :
                            new LoginResponse(Success: true, Token: newJwtToken, RefreshToken: newRefreshToken);

    }
}
