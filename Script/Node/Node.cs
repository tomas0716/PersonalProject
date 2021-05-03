using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node 
{
	protected int			m_nID				= 0;
	protected string      	m_strName			= "";
	protected Node			m_Parent			= null;
	protected List<Node>	m_ChildeNodeList 	= new List<Node>();
	protected object 		m_pParametaData 	= null;
	
	public Node()
	{
	}
	
	public void 	SetID(int ID)
	{
		m_nID = ID;	
	}
	
	public int 		GetID()
	{
		return m_nID;
	}	
	
	public void 	SetName(string Name)
	{
		m_strName = Name;
	}
	
	public string	GetName()
	{
		return m_strName;
	}	
	
	public void 	SetParametaData(object ob)
	{
		m_pParametaData = ob;
	}
	
	public object 	GetParametaData()
	{
		return m_pParametaData;
	}	
	
	public void 	SetParent(Node Parent)
	{
        m_Parent = Parent;	
	}
	
	public Node 	GetParent()
	{
		return m_Parent;
	}	
	
	public void 	AddChild(Node Child)
	{
		if( m_ChildeNodeList.Contains(Child) == true )
		{
			return;
		}
		
		m_ChildeNodeList.Add(Child);	
	}
	
	public int 		GetChildNodeCount()
	{
		return m_ChildeNodeList.Count;
	}
	
	public Node 	GetChildNode_byID(int ID)
	{
		foreach( Node child in m_ChildeNodeList )
		{
			if( child.GetID () == ID )
			{
				return child;
			}
		}
		
		return null;
	}
	
	public Node 	GetChildNode_byIndex(int Index)
	{
		if( Index < 0 || Index > m_ChildeNodeList.Count - 1 )
		{
			return null;
		}
		
		return m_ChildeNodeList[Index];	
	}
	
	public Node 	GetChildNode_byName(string Name)
	{
		foreach( Node child in m_ChildeNodeList )
		{
			if( child.GetName () == Name )
			{
				return child;
			}
		}
		
		return null;
	}
	
	public void 	RemoveChild_byID(int ID)
	{
		Node node = GetChildNode_byID(ID);
		
		if( node != null )
		{
			m_ChildeNodeList.Remove(node);
		}
	}
	
	public void 	RemoveChild_byIndex(int Index)
	{
		Node node = GetChildNode_byIndex(Index);
		
		if( node != null )
		{
			m_ChildeNodeList.Remove(node);
		}
	}
	
	public void 	RemoveChild_byName(string Name)
	{
		Node node = GetChildNode_byName(Name);
		
		if( node != null )
		{
			m_ChildeNodeList.Remove(node);
		}
	}
}
