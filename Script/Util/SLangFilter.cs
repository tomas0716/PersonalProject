using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SLangNode
{
    public bool m_IsLeatNode = false;
    public Dictionary<uint, SLangNode>   m_ChildNodeTable = new Dictionary<uint, SLangNode>();

    public SLangNode()
    {
    }

    public SLangNode AddChild(uint data)
    {
        if (m_ChildNodeTable.ContainsKey(data) == true)
        {
            return m_ChildNodeTable[data];
        }
    
        SLangNode pNode = new SLangNode();
        m_ChildNodeTable.Add(data, pNode);

        return pNode;
    }

    public SLangNode FindChild(uint data)
    {
        if (m_ChildNodeTable.ContainsKey(data) == true)
        {
            return m_ChildNodeTable[data];
        }

        return null;
    }
}

public class SLangFilter
{
    private string      m_Punctuations  = "`~!@#$%^&*()-_=+\\|[{]};:'\",<.>/?";
    private SLangNode   m_pRoot         = new SLangNode();

    public SLangFilter()
    {
    }

    public void AddString(string strSLang)
    {
        SLangNode pNode = m_pRoot;

        for (int i = 0; i < strSLang.Length; ++i)
        {
            pNode = pNode.AddChild((uint)strSLang[i]);
        }

        pNode.m_IsLeatNode = true;
    }

    public bool HasSLang(string strData)
    {
        for (int i = 0; i < strData.Length; ++i)
        {
            string strOrigin;
            if (Match(strData.Substring(i, strData.Length - i), out strOrigin) > 0)
            {
                return true;
            }
        }

        return false;
    }

    public string Filter(string strData)
    {
        string strCopy = string.Copy(strData);
        string strReplace = "";

        for (int i = 0; i < strData.Length;)
        {
            string strOrigin;
            int nSize = Match(strData.Substring(i, strData.Length - i), out strOrigin);

            if (nSize > 0)
            {
                strReplace = "";

                for (int k = 0; k < nSize; ++k)
                {
                    strReplace += "♡";
                }

                strCopy = strCopy.Replace(strOrigin, strReplace);
                i += nSize;
            }
            else
            {
                ++i;
            }
        }

        return strCopy;
    }

    private int Match(string strData, out string strOrigin)
    {
        strOrigin = "";

        if (strData.Length == 0)
            return 0;
        if(IsPunctutation(strData[0]))
            return 0;

        SLangNode pNode = m_pRoot;
        int nIndex = 0;

        while (nIndex < strData.Length)
        {
            if (IsPunctutation(strData[nIndex]) == true)
            {
                strOrigin += strData[nIndex].ToString();
                ++nIndex;
                continue;
            }

            uint data = (uint)(strData[nIndex]);
            pNode = pNode.FindChild(data);

            if( pNode == null) 
                return 0;

            if (pNode.m_IsLeatNode == true)
            {
                strOrigin += strData[nIndex].ToString();
                return nIndex + 1;
            }

            strOrigin += strData[nIndex].ToString();
            ++nIndex;
        }

        return 0;
    }

    private bool IsPunctutation(char strSet)
    {
        return m_Punctuations.Contains(strSet.ToString());
    }
}
