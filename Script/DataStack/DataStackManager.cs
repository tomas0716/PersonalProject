using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using UObject = UnityEngine.Object;

public class DataStackManager
{
    Dictionary<Type, object> m_DataStackTable = new Dictionary<Type, object>();

    public DataStackManager()
    {
    }

    public void Push(object dataStack)
    {
        if (m_DataStackTable.ContainsKey(dataStack.GetType()) == true)
        {
            m_DataStackTable.Remove(dataStack.GetType());
        }

        m_DataStackTable.Add(dataStack.GetType(), dataStack);
    }

    public T Pop<T>() where T : class, new()
    {
        Type type = typeof(T);
        if (m_DataStackTable.ContainsKey(type) == true)
        {
            T result = m_DataStackTable[type] as T;
            m_DataStackTable.Remove(type);
            return result;
        }
        return null;
    }

    public void Clear()
    {
        m_DataStackTable.Clear();
    }
}

