using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class AtlasGroup
{
	private string					m_strAtlasGroupName	= "";
	Dictionary<string, AtlasInfo>	m_AtlasInfoList		= new Dictionary<string, AtlasInfo>();
	bool							m_IsAtlasPack		= false;
	eAtlasFileType					m_eAtlasFileType	= eAtlasFileType.Atlas_2D;

	Texture2D						m_pAtlasTexture		= null;

	public					AtlasGroup(string AtlasGroupName, eAtlasFileType fileType)
	{
		m_strAtlasGroupName = AtlasGroupName;
		m_eAtlasFileType = fileType;
	}

	private void			InsertTexture(string FilePath, string FileName)
	{
		if (m_IsAtlasPack == false)
		{
			AtlasInfo atlasInfo = new AtlasInfo(this, FileName);
			string FullFilePath = FilePath + "/" + FileName;
			m_AtlasInfoList.Add(FullFilePath, atlasInfo);
		}
		else
		{
			Debug.Log("AtlasGroup : Already Pack Complete!!");
		}
	}

	private void			PackTextures()
	{
		if (m_AtlasInfoList.Count != 0)
		{
			m_IsAtlasPack = true;

			m_pAtlasTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false);

			Texture2D[] Textures = new Texture2D[m_AtlasInfoList.Count];

			int i = 0;
			foreach (KeyValuePair<string, AtlasInfo> item in m_AtlasInfoList)
			{
				Textures[i++] = Resources.Load(item.Key, typeof(Texture2D)) as Texture2D;
			}

			//Rect[] Rects = m_pAtlasTexture.PackTextures(Textures, 1, SystemInfo.maxTextureSize);
			Rect[] Rects = TexturePacker.PackTextures(m_pAtlasTexture, Textures, 4, 4, 2, SystemInfo.maxTextureSize);

			i = 0;
			foreach (KeyValuePair<string, AtlasInfo> item in m_AtlasInfoList)
			{
				Rect rect = ConvertToPixels(Rects[i], m_pAtlasTexture.width, m_pAtlasTexture.height, true);

				if (Mathf.RoundToInt(rect.width) != Textures[i].width)
				{
					Debug.Log("AtlasGroup : Texture Pack Fail!!");
				}

				item.Value.SetAtlasInfo(Rects[i], Textures[i].width, Textures[i].height);
				++i;
			}
		}
	}

	public void				SaveAtlasTexture(string FilePath)
	{
		string OutputPath = Application.dataPath + "/Resources/" + FilePath + "/" + m_strAtlasGroupName + ".png";
		byte[] bytes = m_pAtlasTexture.EncodeToPNG();
		System.IO.File.WriteAllBytes(OutputPath, bytes);

#if UNITY_EDITOR
		AssetDatabase.ImportAsset(OutputPath, ImportAssetOptions.ImportRecursive);
#endif

		SaveFile(FilePath);
	}

	private Rect			ConvertToPixels(Rect rect, int width, int height, bool round)
	{
		Rect final = rect;

		if (round)
		{
			final.xMin = Mathf.RoundToInt(rect.xMin * width);
			final.xMax = Mathf.RoundToInt(rect.xMax * width);
			final.yMin = Mathf.RoundToInt((1f - rect.yMax) * height);
			final.yMax = Mathf.RoundToInt((1f - rect.yMin) * height);
		}
		else
		{
			final.xMin = rect.xMin * width;
			final.xMax = rect.xMax * width;
			final.yMin = (1f - rect.yMax) * height;
			final.yMax = (1f - rect.yMin) * height;
		}
		return final;
	}

	public string			GetAtlasGroupName()
	{
		return m_strAtlasGroupName;
	}

	public eAtlasFileType	GetAtlasFileType()
	{
		return m_eAtlasFileType;
	}

	public Texture2D		GetTexture()
	{
		return m_pAtlasTexture;
	}

	public AtlasInfo		FindAtlasInfo(string FileName)
	{
		if (m_AtlasInfoList.ContainsKey(FileName) == true)
		{
			return m_AtlasInfoList[FileName];
		}

		return null;
	}

	public void				Remove(string FileName)
	{
		if (m_AtlasInfoList.ContainsKey(FileName) == true)
		{
			m_AtlasInfoList.Remove(FileName);
		}
	}

	public void				RemoveAll()
	{
		m_AtlasInfoList.Clear();
	}

	public void				SaveFile(string FilePath)
	{
		string FullPath = "Assets/Resources/" + FilePath + "/" + m_strAtlasGroupName + "_AtlasInfo.bytes";

		FileStream fs = new FileStream(FullPath, FileMode.Create, FileAccess.Write);

		if (fs != null)
		{
			BinaryWriter bw = new BinaryWriter(fs);

			bw.Write(m_strAtlasGroupName);
			bw.Write(m_AtlasInfoList.Count);

			foreach (KeyValuePair<string, AtlasInfo> item in m_AtlasInfoList)
			{
				item.Value.SaveFile(bw);
			}

			bw.Close();
		}

#if UNITY_EDITOR
		AssetDatabase.ImportAsset(FullPath, ImportAssetOptions.ImportRecursive);
#endif
	}

	public void				LoadFile(eAtlasFileType eFileType, string FileName)
	{
		string fileType = "";
		switch (eFileType)
		{
			case eAtlasFileType.Atlas_GUI:	fileType = "GUI";				break;
			case eAtlasFileType.Atlas_2D:	fileType = "2D";				break;
			case eAtlasFileType.Atlas_2D_Tool:	fileType = "Tool/2D_Tool";	break;
		}

		TextAsset textAsset = Resources.Load(fileType + "/Atlas/" + FileName + "_AtlasInfo") as TextAsset;

		MemoryStream ms = new MemoryStream(textAsset.bytes);

		if (ms != null)
		{
			BinaryReader br = new BinaryReader(ms);

			m_strAtlasGroupName		= br.ReadString();
			int NumAtlasInfo	= br.ReadInt32();

			for (int i = 0; i < NumAtlasInfo; ++i)
			{
				AtlasInfo atlasInfo = new AtlasInfo(this);
				atlasInfo.LoadFile(br);

				m_AtlasInfoList.Add(atlasInfo.GetFileName(), atlasInfo);
			}

			br.Close();
		}

		string FullPath = fileType + "/Atlas/" + FileName;
		m_pAtlasTexture = Resources.Load(FullPath, typeof(Texture2D)) as Texture2D;
	}
}