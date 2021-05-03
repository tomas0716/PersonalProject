using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MaterialResourceManager 
{
	Dictionary<string, Material> 	m_LoadingMaterialResourceList = new Dictionary<string, Material>();
	
	public Material LoadMaterial(string filePath, string MaterialFileName)
	{
		Material mat = FindMaterial(filePath, MaterialFileName);
		
		if( mat == null )
		{
			mat = (Material)Resources.Load (filePath + MaterialFileName, typeof(Material));	
			
			if( mat != null )
			{
				m_LoadingMaterialResourceList.Add (filePath + MaterialFileName, mat);
			}
			
		}
		
		return mat;
	}
	
	Material FindMaterial(string filePath, string MaterialFileName)
	{
		foreach(KeyValuePair<string,Material> item in m_LoadingMaterialResourceList)
		{
			if( item.Key == filePath + MaterialFileName )
				return item.Value;
		}
		
		return null;
	}
	
	public void RemoveMaterial(string filePath, string MaterialFileName)
	{
		foreach(KeyValuePair<string,Material> item in m_LoadingMaterialResourceList)
		{
			if( item.Key == filePath + MaterialFileName )
			{
				m_LoadingMaterialResourceList.Remove (item.Key);
				
				return;
			}
		}
	}
	
	public void RemoveMaterial(Material ob)
	{
		foreach(KeyValuePair<string,Material> item in m_LoadingMaterialResourceList)
		{
			if( item.Value == ob )
			{
				m_LoadingMaterialResourceList.Remove (item.Key);
				
				return;
			}
		}		
	}
	
	public void RemoveMaterialAll()
	{
		m_LoadingMaterialResourceList.Clear();
	}
}
