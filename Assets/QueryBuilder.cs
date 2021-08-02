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

    public string ToQueryString(List<string> regexp)
    {
        List<string> temp = dictionary
            .Where(kvp => !String.IsNullOrEmpty(kvp.Value))
            .Select(kvp => $"{kvp.Key} {(regexp.Contains(kvp.Key) ? $"REGEXP \"{kvp.Value}\"" : $"= \"{kvp.Value}\"")}")
            .ToList();

        return String.Join(" AND ", temp);
    }
}
