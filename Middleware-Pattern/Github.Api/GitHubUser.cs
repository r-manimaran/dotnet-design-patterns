using System;

namespace Github.Api;

public class GitHubUser
{
    public string login { get; set; }
    public int id { get; set; }
    public string node_id { get; set; }
    public string avatar_url { get; set; }
    public string gravatar_id { get; set; }
    public string url { get; set; }
    public string html_url { get; set; }
    public string followers_url { get; set; }
    public string following_url { get; set; }
    public string gists_url { get; set; }
    public string starred_url { get; set; }
    public string subscriptions_url { get; set; }
    public string organizations_url { get; set; }
    public string repos_url { get; set; }
    public string events_url { get; set; }
    public string received_events_url { get; set; }
    public string type { get; set; }
    public bool site_admin { get; set; }
    public string name { get; set; }
    public string company { get; set; }
    public string blog { get; set; }
    public string location { get; set; }
    public string email { get; set; }
    public string hireable { get; set; }
    public string bio { get; set; }
    public string twitter_username { get; set; }
    public int public_repos { get; set; }
    public int public_gists { get; set; }
}
