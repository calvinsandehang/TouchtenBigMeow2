using System;
using System.Collections.Generic;

public class ObservableList<T>
{
    private List<T> internalList = new List<T>();

    // Event that gets triggered when the list changes
    public event Action<List<T>> ListChanged;

    // Property to access the encapsulated list
    public List<T> List
    {
        get { return internalList; }
        set
        {
            internalList = value;
            OnListChanged();
        }
    }

    // Constructor to initialize the list
    public ObservableList(List<T> initialList)
    {
        internalList = initialList;
    }

    // Add an item to the list and trigger the event
    public void Add(T item)
    {
        internalList.Add(item);
        OnListChanged();
    }

    public void AddRange(IEnumerable<T> collection)
    {
        internalList.AddRange(collection);
        OnListChanged();
    }

    public void Sort(Comparison<T> comparison)
    {
        internalList.Sort(comparison);
        OnListChanged();
    }

    // Remove an item from the list and trigger the event
    public void Remove(T item)
    {
        internalList.Remove(item);
        OnListChanged();
    }

    // Clear the list and trigger the event
    public void Clear()
    {
        internalList.Clear();
        OnListChanged();
    }

    // Method to trigger the ListChanged event
    private void OnListChanged()
    {
        ListChanged?.Invoke(internalList);
    }
}
