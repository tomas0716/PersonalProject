using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

public enum eAtlasFileType
{
	Atlas_GUI,
	Atlas_2D,
	Atlas_2D_Tool,

	Max,
}

public class AtlasManager
{
	private Dictionary<string, AtlasGroup> [] m_AtlasGroupTable = new Dictionary<string,AtlasGroup>[(int)eAtlasFileType.Max];

	public AtlasManager()
	{
		for (int i = 0; i < (int)eAtlasFileType.Max; ++i)
		{
			m_AtlasGroupTable[i] = new Dictionary<string, AtlasGroup>();
		}
	}

	public AtlasGroup Begin(eAtlasFileType fileType, string AtlasGroupName)
	{
		AtlasGroup atlasGroup = FindAtlasGroup(fileType, AtlasGroupName);

		if (atlasGroup == null)
		{
			BindingFlags flags = BindingFlags.CreateInstance;
			Type type = typeof(AtlasGroup);
			atlasGroup = (AtlasGroup)type.InvokeMember("AtlasGroup", flags, null, null, new object[] { AtlasGroupName, fileType });
			m_AtlasGroupTable[(int)fileType].Add(AtlasGroupName, atlasGroup);
		}
		else
		{
			Debug.Log("Exist Same AtlasGroup!!");
		}

        return atlasGroup;
	}

	public void			InsertTexture(AtlasGroup atlasGroup, string FilePath, string FileName)
	{
		Type type = atlasGroup.GetType();
		BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
		MethodInfo Method = type.GetMethod("InsertTexture", flags);
		Method.Invoke(atlasGroup, new object[] { FilePath, FileName });
	}

	public void			End(AtlasGroup atlasGroup)
	{
		Type type = atlasGroup.GetType();
		BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
		MethodInfo Method = type.GetMethod("PackTextures", flags);
		Method.Invoke(atlasGroup, null);
	}

	private AtlasGroup	FindAtlasGroup(eAtlasFileType fileType, string AtlasGroupName)
    {
		if (m_AtlasGroupTable[(int)fileType].ContainsKey(AtlasGroupName) == true)
		{
			return m_AtlasGroupTable[(int)fileType][AtlasGroupName];
		}

		return null;
    }

	public AtlasGroup	LoadAtlasGroup(eAtlasFileType fileType, string AtlasGroupName)
	{
		AtlasGroup atlasGroup = FindAtlasGroup(fileType, AtlasGroupName);

		if (atlasGroup == null)
		{
            //BindingFlags flags = BindingFlags.NonPublic | BindingFlags.CreateInstance;
            BindingFlags flags = BindingFlags.CreateInstance;
            Type type = typeof(AtlasGroup);
			atlasGroup = (AtlasGroup)type.InvokeMember("AtlasGroup", flags, null, null, new object[] { AtlasGroupName, fileType });
			m_AtlasGroupTable[(int)fileType].Add(AtlasGroupName, atlasGroup);

			atlasGroup.LoadFile(fileType, AtlasGroupName);
		}

		return atlasGroup;
	}

	public void			Remove(eAtlasFileType fileType, string AtlasGroupName)
	{
		if (m_AtlasGroupTable[(int)fileType].ContainsKey(AtlasGroupName) == true)
		{
			m_AtlasGroupTable[(int)fileType].Remove(AtlasGroupName);
		}
	}

	public void			RemoveAll()
	{
		for (int i = 0; i < (int)eAtlasFileType.Max; ++i)
		{
			foreach (KeyValuePair<string, AtlasGroup> item in m_AtlasGroupTable[i])
			{
				item.Value.RemoveAll();
			}

			m_AtlasGroupTable[i].Clear();
		}
	}
}