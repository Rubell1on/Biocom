using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

public class CustomList<T> : List<T>
{
    public UnityEvent changed = new UnityEvent();
    public new void Add(T item)
    {
        base.Add(item);
        changed.Invoke();
    }

    public new void AddRange(IEnumerable<T> collection)
    {
        base.AddRange(collection);
        changed.Invoke();
    }

    public new void Remove(T item)
    {
        base.Remove(item);
        changed.Invoke();
    }

    public new void RemoveAt(int index)
    {
        base.RemoveAt(index);
        changed.Invoke();
    }

    public new void Clear()
    {
        base.Clear();
        changed.Invoke();
    }
}