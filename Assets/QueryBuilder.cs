using System;
using System.Collections.Generic;
using System.Linq;

public class QueryBuilder
{
    public Dictionary<string, string> dictionary;

    public QueryBuilder(Dictionary<string, string> dictionary)
    {
        this.dictionary = dictionary;
    }

    public string ToUpdateQueryString()
    {
        List<string> temp = dictionary
            .Where(kvp => !String.IsNullOrEmpty(kvp.Value))
            .Select(kvp => $"{kvp.Key} = \"{kvp.Value}\"")
            .ToList();

        return String.Join(", ", temp);
    }

    public string ToSearchQueryString(List<string> regexp)
    {
        List<string> temp = dictionary
            .Where(kvp => !String.IsNullOrEmpty(kvp.Value))
            .Select(kvp => $"{kvp.Key} {(regexp.Contains(kvp.Key) ? $"REGEXP \"{kvp.Value}\"" : $"= \"{kvp.Value}\"")}")
            .ToList();

        return String.Join(" AND ", temp);
    }
}
