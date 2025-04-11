using System;
using System.Text.RegularExpressions;

public class TopicParser
{
    /// <summary>
    /// Convert string template to get id from path
    /// </summary>
    /// <param name="template">Template topic has the target {id}, ie. "rsu/AccidentReport/{id}"</param>
    /// <param name="topic">Topic  desired to be extracted</param>
    /// <returns>Return {id} value if matched, otherwise return null</returns>
    public static string? ExtractId(string template, string topic)
    {
        var pattern = "^" + Regex.Escape(template).Replace("\\{id\\}", "(?<id>[^/]+)") + "$";
        var match = Regex.Match(topic, pattern);

        return match.Success ? match.Groups["id"].Value : null;
    }
}
