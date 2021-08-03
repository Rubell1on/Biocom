using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Filter : MonoBehaviour
{
    public QueryBuilder query = new QueryBuilder(new Dictionary<string, string>());
    public QueryBuilder defaultQuery = new QueryBuilder(new Dictionary<string, string>());

    public FilterChangedEvent filterChanged = new FilterChangedEvent();

    protected virtual void SetFilter(Dictionary<string, string> filterParams)
    {
        query = new QueryBuilder(filterParams);
        filterChanged.Invoke(query);
    }

    protected virtual void ResetFilter()
    {
        query = new QueryBuilder(defaultQuery.dictionary);
        filterChanged.Invoke(query);
    }

    [Serializable]
    public class FilterChangedEvent : UnityEvent<QueryBuilder> { }
}
