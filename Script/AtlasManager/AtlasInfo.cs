using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

public class AtlasInfo 
{
	string		m_strName			= "";
	AtlasGroup	m_pAtlasGroup		= null;
	string		m_strFileName		= "";

	int			m_nWidth			= 0;
	int			m_fHeight			= 0;

	Rect		m_rcTexCoord		= new Rect(0, 0, 0, 0);

	public AtlasInfo(AtlasGroup atlasGroup)
	{
		m_pAtlasGroup = atlasGroup;
	}

	public AtlasInfo(AtlasGroup atlasGroup, string FileName)
	{
		m_pAtlasGroup = atlasGroup;
		m_strFileName = FileName;
	}

	public void			SetName(string Name)
	{
		m_strName = Name;
	}

	public string		GetName()
	{
		return m_strName;
	}

	public void			SetAtlasInfo(Rect TexCoord, int Width, int Height)
	{
		m_rcTexCoord	= TexCoord;
		m_nWidth		= Width;
		m_fHeight		= Height;
	}

	public AtlasGroup	GetAtlasGroup()
	{
		return m_pAtlasGroup;
	}

	public string		GetFileName()
	{
		return m_strFileName;
	}

	public int			GetWidth()
	{
		return m_nWidth;
	}

	public int			GetHeight()
	{
		return m_fHeight;
	}

	public Rect			GetTexCoord()
	{
		return m_rcTexCoord;
	}

	public void			SaveFile(BinaryWriter bw)
	{
		bw.Write(m_strFileName);
		bw.Write(m_nWidth);
		bw.Write(m_fHeight);
		bw.Write(m_rcTexCoord.x);
		bw.Write(m_rcTexCoord.y);
		bw.Write(m_rcTexCoord.width);
		bw.Write(m_rcTexCoord.height);
	}

	public void			LoadFile(BinaryReader br)
	{
		m_strFileName = br.ReadString();
		m_nWidth = br.ReadInt32();
		m_fHeight = br.ReadInt32();
		m_rcTexCoord.x = br.ReadSingle();
		m_rcTexCoord.y = br.ReadSingle();
		m_rcTexCoord.width = br.ReadSingle();
		m_rcTexCoord.height = br.ReadSingle();
	}
}
