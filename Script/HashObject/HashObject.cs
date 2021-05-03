using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HashObject
{
	static private Dictionary<string,ulong>		m_HashTable = new Dictionary<string, ulong>();
	
	static public ulong			GetHashCode(string String)
	{
		ulong  HashCode = 0;
	
		if( m_HashTable.TryGetValue(String,out HashCode) == true )
		{
			return HashCode;
		}
	
		return AddHashCode(String);	
	}
	
	static public string 		GetHashString(ulong uHashCode)
	{
		foreach(KeyValuePair<string, ulong> item in m_HashTable)
		{
			if( item.Value == uHashCode )
			{
				return item.Key;
			}
		}
	
		return "";		
	}
	
	static private ulong 		AddHashCode(string String)
	{
	    ulong result 	= 0;  
	    ulong  XOR    	= 0;  
		
		int len 		= String.Length;  
	
	    for( int i = 0; i < len; ++i )  
	    {  
	        char ch = String[i];  
	
	        result = ( ( result << 8 ) | ch ) % 16777213UL; // 16777213UL  
	
	        XOR ^= ch;  
	    }
	
		result |= (XOR<<24);
	
		m_HashTable.Add (String, result);
	
	    return result;	
	}
}
