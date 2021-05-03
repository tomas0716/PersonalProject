using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextureResourceManager
{
	Dictionary<string, Texture> 	m_LoadingTexResourceList = new Dictionary<string, Texture>();

	public Texture LoadTexture(string filePath, string TextureFileName)
	{
		OutputLog.Log(filePath + TextureFileName);
		Texture tex = FindTexture(filePath, TextureFileName);
		
		if( tex == null )
		{
			tex = (Texture)Resources.Load(filePath + TextureFileName);	
			
			if( tex != null )
			{
				m_LoadingTexResourceList.Add (filePath + TextureFileName, tex);
			}
		}
		
		return tex;
	}
	
	Texture FindTexture(string filePath, string TextureFileName)
	{
		foreach(KeyValuePair<string,Texture> item in m_LoadingTexResourceList)
		{
			if( item.Key == filePath + TextureFileName )
				return item.Value;
		}
		
		return null;
	}
	
	public void RemoveTexture(string filePath, string TextureFileName)
	{
		foreach(KeyValuePair<string,Texture> item in m_LoadingTexResourceList)
		{
			if( item.Key == filePath + TextureFileName )
			{
				m_LoadingTexResourceList.Remove (item.Key);
				
				return;
			}
		}
	}
	
	public void RemoveTexture(Texture tex)
	{
		foreach(KeyValuePair<string,Texture> item in m_LoadingTexResourceList)
		{
			if( item.Value == tex )
			{
				m_LoadingTexResourceList.Remove (item.Key);
				
				return;
			}
		}		
	}
	
	public void RemoveTextureAll()
	{
		m_LoadingTexResourceList.Clear();
	}
}