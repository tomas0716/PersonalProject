using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TypeManager 
{
	static public List<TypeObject>		m_TypeObjectList = new List<TypeObject>();
	
	static public void 			AddTypeObject(string TypeName, System.Type type)
	{
		TypeObject ob = new TypeObject();
		ob.m_strTypeName = TypeName;
		ob.m_Type = type;
		
		m_TypeObjectList.Add (ob);
	}
	
	static public TypeObject	FindTypeObject(string TypeName)
	{
		foreach(TypeObject ob in  m_TypeObjectList)
		{
			if( ob.m_strTypeName == TypeName )
			{
				return ob;
			}
		}
		
		return null;
	}
}
