using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

#if UNITY_EDITOR

public class Tool_CreateMap : MonoBehaviour
{
    private Dictionary<Tool_FolderItem,List<Tool_FileItem>>  m_FileTable = new Dictionary<Tool_FolderItem, List<Tool_FileItem>>();

	private List<string>		m_CurrFolderFileNameList		= new List<string>();

	private Scrollbar			m_pScrollbar_File				= null;
	private GameObject			m_pGameObject_Content_Folder	= null;
	private GameObject			m_pGameObject_Content_File		= null;

	private Tool_FolderItem		m_pCurrFolderItem				= null;
	private Tool_FileItem		m_pCurrFileItem					= null;

	private InputField			m_pInputField					= null;

	private string				m_strCurrFolderName				= "";
	private string				m_strCurrFileName				= "";

	void Start()
    {
		GameObject ob;
		ob = Helper.FindChildGameObject(gameObject, "Scroll View File");
		ob = Helper.FindChildGameObject(ob, "Scrollbar Vertical");
		m_pScrollbar_File = ob.GetComponent<Scrollbar>();

		ob = Helper.FindChildGameObject(gameObject, "Scroll View Folder");
		m_pGameObject_Content_Folder = Helper.FindChildGameObject(ob, "Content");

		ob = Helper.FindChildGameObject(gameObject, "Scroll View File");
		m_pGameObject_Content_File = Helper.FindChildGameObject(ob, "Content");

		ob = Helper.FindChildGameObject(gameObject, "InputField");
		m_pInputField = ob.GetComponent<InputField>();

		ConstructList_Folder();
	}

	private void OnDestroy()
	{
	}

	private void Update()
	{
		if (m_pCurrFolderItem != null && m_pCurrFileItem != null && m_strCurrFolderName != "" && m_strCurrFileName != "")
		{
			if (Input.GetKeyUp(KeyCode.DownArrow) == true)
			{
				if (m_FileTable.ContainsKey(m_pCurrFolderItem) == true)
				{
					int nNumData = m_FileTable[m_pCurrFolderItem].Count;

					for (int i = 0; i < nNumData; ++i)
					{
						if (m_FileTable[m_pCurrFolderItem][i].GetFileName() == m_strCurrFileName)
						{
							if (i < nNumData - 1)
							{
								OnFileSel(m_FileTable[m_pCurrFolderItem][i+1]);
								break;
							}
						}
					}
				}
			}
			else if (Input.GetKeyUp(KeyCode.UpArrow) == true)
			{
				if (m_FileTable.ContainsKey(m_pCurrFolderItem) == true)
				{
					int nNumData = m_FileTable[m_pCurrFolderItem].Count;

					for (int i = 0; i < nNumData; ++i)
					{
						if (m_FileTable[m_pCurrFolderItem][i].GetFileName() == m_strCurrFileName)
						{
							if (i > 0)
							{
								OnFileSel(m_FileTable[m_pCurrFolderItem][i - 1]);
								break;
							}
						}
					}
				}
			}
		}
	}

	private void ConstructList_Folder()
	{
		m_pCurrFileItem = null;

		string[] DirectionFolders = Directory.GetDirectories("Assets/Resources/Map", "*.*", SearchOption.TopDirectoryOnly);

		foreach (string strFolder in DirectionFolders)
		{
			if (strFolder.Contains(".meta"))
			{
				continue;
			}

			string folder = strFolder.Replace("Assets/Resources/Map\\", "");

			GameObject ob = Resources.Load("Tool/Prefabs/FolderItem") as GameObject;
			ob = GameObject.Instantiate(ob);
			Tool_FolderItem pFolderItem = ob.GetComponent<Tool_FolderItem>();
			pFolderItem.Init(this, folder);
			ob.transform.SetParent(m_pGameObject_Content_Folder.transform);
			ob.transform.localScale = Vector3.one;

			m_FileTable.Add(pFolderItem, new List<Tool_FileItem>());

			//Construct_File_ForDebug(folder);
		}
	}

	private void Construct_File_ForDebug(string strFolder)
	{
		string[] DirectionFiles = Directory.GetFiles("Assets/Resources/Map/" + strFolder + "/", "*.bytes", SearchOption.TopDirectoryOnly);

		foreach (string file in DirectionFiles)
		{
			if (file.Contains(".meta"))
			{
				continue;
			}

			FileInfo file_name = new FileInfo(file);

			string Name = file_name.Name.Replace(".bytes", "");

			MapDataInfo pMapDataInfo = new MapDataInfo(strFolder, Name);
			pMapDataInfo.LoadFile();
			pMapDataInfo.SaveFile();
		}
	}

	private void Construct_File(Tool_FolderItem pFolderItem, string strFolder)
	{
		Helper.RemoveChildAll(m_pGameObject_Content_File);

		m_FileTable[pFolderItem].Clear();
		m_CurrFolderFileNameList.Clear();

		string[] DirectionFiles = Directory.GetFiles("Assets/Resources/Map/" + strFolder + "/", "*.bytes", SearchOption.TopDirectoryOnly);

		foreach (string file in DirectionFiles)
		{
			if (file.Contains(".meta"))
			{
				continue;
			}

			FileInfo file_name = new FileInfo(file);

			string Name = file_name.Name.Replace(".bytes", "");
			GameObject ob = Resources.Load("Tool/Prefabs/FileItem") as GameObject;
			ob = GameObject.Instantiate(ob);
			Tool_FileItem pFileItem = ob.GetComponent<Tool_FileItem>();
			pFileItem.Init(this, Name);
			ob.transform.SetParent(m_pGameObject_Content_File.transform);
			ob.transform.localScale = Vector3.one;

			m_FileTable[pFolderItem].Add(pFileItem);
			m_CurrFolderFileNameList.Add(Name);
		}
	}

	public void OnFolderSel(Tool_FolderItem pFolderItem)
	{
		if(m_pCurrFolderItem != null)
			m_pCurrFolderItem.Disable_Sel();

		if (m_pCurrFolderItem != pFolderItem)
		{
			m_pCurrFolderItem = pFolderItem;
			Construct_File(m_pCurrFolderItem, m_pCurrFolderItem.GetFolderName());
		}
	}

	public void OnFileSel(Tool_FileItem pFileItem)
	{
		if (m_pCurrFileItem != null)
			m_pCurrFileItem.Disable_Sel();

		if (m_pCurrFileItem != pFileItem)
		{
			if (SavedGameDataInfo.Instance.m_pMapDataInfo != null)
			{
				SavedGameDataInfo.Instance.m_pMapDataInfo.SaveFile();
			}

			m_pCurrFileItem = pFileItem;

			m_pCurrFileItem.Able_Sel();

			// Load
			Tool_Info.Instance.m_IsEditing = false;
			SavedGameDataInfo.Instance.m_pMapDataInfo = new MapDataInfo(m_pCurrFolderItem.GetFolderName(), m_pCurrFileItem.GetFileName());
			SavedGameDataInfo.Instance.m_pMapDataInfo.LoadFile();

			EventDelegateManager_ForTool.Instance.OnUpdateMap(true);
			EventDelegateManager_ForTool.Instance.OnPostUpdateMap(true);

			m_strCurrFolderName = m_pCurrFolderItem.GetFolderName();
			m_strCurrFileName = m_pCurrFileItem.GetFileName();
		}
	}

	public void OnButtonClick_Create()
	{
		if (m_pCurrFolderItem == null)
		{
			OutputLog.Log("Select Folder Item");
		}
		else
		{
			if (m_pInputField.text == null || m_pInputField.text == "")
			{
				OutputLog.Log("Empty File Name");
			}
			else
			{
				if (m_CurrFolderFileNameList.Contains(m_pInputField.text) == true)
				{
					OutputLog.Log("Overlap File Name");
				}
				else
				{
					if (SavedGameDataInfo.Instance.m_pMapDataInfo != null)
					{
						SavedGameDataInfo.Instance.m_pMapDataInfo.SaveFile();
					}

					Tool_Info.Instance.m_IsEditing = false;
					SavedGameDataInfo.Instance.m_pMapDataInfo = new MapDataInfo(m_pCurrFolderItem.GetFolderName(), m_pInputField.text);
					SavedGameDataInfo.Instance.m_pMapDataInfo.SaveFile();

					GameObject ob = Resources.Load("Tool/Prefabs/FileItem") as GameObject;
					ob = GameObject.Instantiate(ob);
					Tool_FileItem pFileItem = ob.GetComponent<Tool_FileItem>();
					pFileItem.Init(this, m_pInputField.text);
					ob.transform.SetParent(m_pGameObject_Content_File.transform);
					ob.transform.localScale = Vector3.one;

					m_FileTable[m_pCurrFolderItem].Add(pFileItem);
					m_CurrFolderFileNameList.Add(m_pInputField.text);

					m_pScrollbar_File.value = 0;

					if (m_pCurrFileItem != null)
						m_pCurrFileItem.Disable_Sel();

					m_pCurrFileItem = pFileItem;

					pFileItem.Able_Sel();

					EventDelegateManager_ForTool.Instance.OnUpdateMap(true);
					EventDelegateManager_ForTool.Instance.OnPostUpdateMap(true);
				}
			}
		}
	}
}

#endif