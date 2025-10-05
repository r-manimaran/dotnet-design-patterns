using System.Reflection;

namespace Messaging.Contracts;

public class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
