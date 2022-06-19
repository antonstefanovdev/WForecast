﻿using System.Collections;

public class ConcurrentList<T> : IEnumerable<T>, ICollection<T>
{
    private readonly List<T> _list;
    private readonly ReaderWriterLockSlim _lock;

    public int Count
    {
        get
        {
            try
            {
                _lock.EnterReadLock();
                return _list.Count;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }
    }

    public bool IsReadOnly => false;

    public ConcurrentList() : this(null) { }

    public ConcurrentList(IEnumerable<T> items)
    {
        _list = items is null ? new List<T>() : new List<T>(items);
        _lock = new ReaderWriterLockSlim();
    }

    public void Add(T item)
    {
        try
        {
            _lock.EnterWriteLock(); //4) Примитивы синхронизации
            _list.Add(item);
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    public void Clear()
    {
        try
        {
            _lock.EnterWriteLock();
            _list.Clear();
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    public bool Contains(T item)
    {
        try
        {
            _lock.EnterReadLock(); //4) Примитивы синхронизации
            return _list.Contains(item);
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        try
        {
            _lock.EnterReadLock();
            _list.CopyTo(array, arrayIndex);
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    public bool Remove(T item)
    {
        try
        {
            _lock.EnterWriteLock();
            return _list.Remove(item);
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    private IEnumerable<T> Enumerate()
    {
        try
        {
            _lock.EnterReadLock();
            foreach (T item in _list)
                yield return item;
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    public IEnumerator<T> GetEnumerator()
        => Enumerate().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => Enumerate().GetEnumerator();
}