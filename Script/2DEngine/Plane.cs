using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Plane2D
{
	public enum eTextureAddressMode
	{
		Stretch,        // Size 에 맞게 UV 가 셋팅됨
		Horizontal,     // 가로 방향으로 타일 방식
		Vertical,       // 세로 방향으로 타일 방식
		Both,           // 미구현, 가로 세로 방향으로 타일 방식
        UV,             // UV 에 따라 사이즈가 결정, m_vSize 는 의미 없어짐.
	}

	public enum ePlaneAnchorPosition
	{
		LeftTop,
		Center,
	}

	protected GameObject	m_pGameObject				= null;
	protected AtlasInfo		m_AtlasInfo					= null;
	protected bool			m_IsVisible					= true;
	protected string		m_strName					= "";
	protected Vector2		m_vPosition					= new Vector2(0, 0);
	protected Vector3		m_vScale					= new Vector3(-1, -1, -1);
	protected Vector3		m_vEulerAngle				= Vector3.zero;
	protected Vector2		m_vTextureSize				= new Vector2(1, 1);
	protected Vector2		m_vSize						= new Vector2(1, 1);
	protected Rect			m_rcTexCoord				= new Rect(0, 0, 1, 1);

	protected bool			m_IsReverse_X				= false;
	protected bool			m_IsReverse_Y				= false;

	protected Material		m_pMaterial					= null;
	protected float			m_fDepth					= 0;
	protected float			m_fDepth_Backup				= 0;

	protected bool			m_IsActiveColor				= false;
	protected Material		m_pMaterial_ForActiveColor	= null;

	protected object		m_pParameta					= null;

    protected Rect          m_rcTextureAddressMode_UV	= new Rect(0, 0, 1, 1);

	protected eTextureAddressMode   m_eTextureAddressMode		= eTextureAddressMode.Stretch;
	protected eScreenAnchorPosition m_eScreenAnchorPosition		= GameDefine.ms_eScreenAnchorPosition;
	protected ePlaneAnchorPosition	m_ePlaneAnchorPosition		= ePlaneAnchorPosition.Center;

	protected PickingComponent		m_pPickingComponent		    = null;
	protected SequenceAnimComponent m_pSequenceAnimComponent	= null;

	public Plane2D(AtlasInfo atlasInfo, float Depth)
	{
		m_AtlasInfo = atlasInfo;
		m_fDepth_Backup = Depth;
		AtlasGroup atlasGroup = atlasInfo.GetAtlasGroup();

		string FileType = "";
		switch (atlasGroup.GetAtlasFileType())
		{
			case eAtlasFileType.Atlas_2D:	FileType = "2D";				break;
			case eAtlasFileType.Atlas_GUI:	FileType = "GUI";				break;
			case eAtlasFileType.Atlas_2D_Tool: FileType = "Tool/2D_Tool";	break;
		}
		string MaterialPath = FileType + "/Materials/";

        Material material = AppInstance.Instance.m_pMaterialResourceManager.LoadMaterial(MaterialPath, atlasGroup.GetAtlasGroupName());

		Create(atlasInfo.GetFileName(), new Vector2(atlasInfo.GetWidth(), atlasInfo.GetHeight()), atlasInfo.GetTexCoord(), material, Depth);
	}

	public Plane2D(string Name, Vector2 Size, Rect TexCoord, Material Material, float Depth)
	{
		Create(Name, Size, TexCoord, Material, Depth);
	}

	private void		Create(string Name, Vector2 Size, Rect TexCoord, Material Material, float Depth)
	{
		m_strName		= Name;
		m_vTextureSize	= Size;
		m_vSize			= Size;
		m_rcTexCoord	= TexCoord;
		m_pMaterial		= Material;
		m_fDepth		= Depth;

		m_pGameObject = new GameObject(Name);
		m_pGameObject.layer = LayerMask.NameToLayer("2D");
		//m_pGameObject.isStatic = true;

		MeshFilter		meshFilter	= m_pGameObject.AddComponent<MeshFilter>();
		MeshRenderer	renderer	= m_pGameObject.AddComponent<MeshRenderer>();
		Mesh			mesh		= new Mesh();

		meshFilter.mesh		= mesh;
		renderer.material	= Material;

		UpdateUV();
		SetPosition(Vector2.zero);
        SetScale(new Vector3(1, 1, 1));

        m_pPickingComponent = m_pGameObject.AddComponent<PickingComponent>();
	}

	public SequenceAnimComponent SetAnimInfo(List<AtlasInfo> AtlasInfoList, float Time, bool IsLoop)
	{
		if (m_pGameObject != null && m_pSequenceAnimComponent == null)
		{
			m_pSequenceAnimComponent = m_pGameObject.AddComponent<SequenceAnimComponent>();
		}

		m_pSequenceAnimComponent.SetAnimInfo(this, AtlasInfoList, Time, IsLoop);

		return m_pSequenceAnimComponent;
	}

	public SequenceAnimComponent GetSequenceAnimComponent()
	{
		return m_pSequenceAnimComponent;
	}

	public void			OnDestroy()
	{
		GameObject.Destroy(m_pGameObject);
		m_pGameObject = null;

		if (m_pMaterial_ForActiveColor != null)
		{
			GameObject.Destroy(m_pMaterial_ForActiveColor);
			m_pMaterial_ForActiveColor = null;
		}
	}

	public AtlasInfo	GetAtlasInfo()
	{
		return m_AtlasInfo;
	}

	public void			SetDepth(float Depth)
	{
		m_fDepth = Depth;
		SetPosition(m_vPosition);
	}

	public float		GetDepth()
	{
		return m_fDepth;
	}

	public void			SetDepth_Backup()
	{
		m_fDepth_Backup = m_fDepth;
	}

	public void			ResetDepth_Backup()
	{
		m_fDepth = m_fDepth_Backup;
		SetPosition(m_vPosition);
	}

	public void			SetPosition(Vector2 vPos)
	{
		m_vPosition = vPos;

		if (m_pGameObject != null)
		{
			Vector2 vScreenZeroPosition = new Vector2(0, 0);
			Vector2 vPlaneZeroPosition = new Vector2(0, 0);

			switch (m_eScreenAnchorPosition)
			{
				case eScreenAnchorPosition.UpperLeft:		vScreenZeroPosition = new Vector2(AppInstance.Instance.m_vBaseResolution.x * -0.5f,	AppInstance.Instance.m_vBaseResolution.y);			break;
				case eScreenAnchorPosition.UpperCenter:		vScreenZeroPosition = new Vector2(0,												AppInstance.Instance.m_vBaseResolution.y);			break;
				case eScreenAnchorPosition.UpperRight:		vScreenZeroPosition = new Vector2(AppInstance.Instance.m_vBaseResolution.x *  0.5f,	AppInstance.Instance.m_vBaseResolution.y);			break;
				case eScreenAnchorPosition.MiddleLeft:		vScreenZeroPosition = new Vector2(AppInstance.Instance.m_vBaseResolution.x * -0.5f,	AppInstance.Instance.m_vBaseResolution.y * 0.5f);		break;
				case eScreenAnchorPosition.MiddleCenter:	vScreenZeroPosition = new Vector2(0,												AppInstance.Instance.m_vBaseResolution.y * 0.5f);		break;
				case eScreenAnchorPosition.MiddleRight:		vScreenZeroPosition = new Vector2(AppInstance.Instance.m_vBaseResolution.x *  0.5f,	AppInstance.Instance.m_vBaseResolution.y * 0.5f);		break;
				case eScreenAnchorPosition.LowerLeft:		vScreenZeroPosition = new Vector2(AppInstance.Instance.m_vBaseResolution.x * -0.5f,	0);													break;
				case eScreenAnchorPosition.LowerCenter:		vScreenZeroPosition = new Vector2(0,												0);													break;
				case eScreenAnchorPosition.LowerRight:		vScreenZeroPosition = new Vector2(AppInstance.Instance.m_vBaseResolution.x *  0.5f,	0);													break;
			}

			switch (m_ePlaneAnchorPosition)
			{
				case ePlaneAnchorPosition.LeftTop: vPlaneZeroPosition = new Vector2(m_vSize.x * 0.5f, m_vSize.y * -0.5f); break;
			}

			switch (m_eScreenAnchorPosition)
			{
				case eScreenAnchorPosition.UpperLeft:
				case eScreenAnchorPosition.UpperCenter:
				case eScreenAnchorPosition.UpperRight:
					{
						vPos.y *= -1;
						vPos += vScreenZeroPosition + vPlaneZeroPosition;
					}
					break;
				case eScreenAnchorPosition.MiddleLeft:
				case eScreenAnchorPosition.MiddleCenter:
				case eScreenAnchorPosition.MiddleRight:
				case eScreenAnchorPosition.LowerLeft:
				case eScreenAnchorPosition.LowerCenter:
				case eScreenAnchorPosition.LowerRight:
					{
						vPos += vScreenZeroPosition + new Vector2(vPlaneZeroPosition.x, -vPlaneZeroPosition.y);
					}
					break;
			}

            vPos *= AppInstance.Instance.m_fMainScale;

            m_pGameObject.transform.localPosition = new Vector3(vPos.x, vPos.y, -m_fDepth);
		}
	}

	public Vector2		GetPosition()
	{
		return m_vPosition;
	}

	public void			SetRotation(Vector3 vEulerAngle)
	{
		if (m_vEulerAngle != vEulerAngle)
		{
			m_vEulerAngle = vEulerAngle;

			if (m_pGameObject != null)
			{
				m_pGameObject.transform.localEulerAngles = vEulerAngle;
			}
		}
	}

	public Vector3		GetRotation()
	{
		return m_vEulerAngle;
	}

	public void			SetScale(Vector3 Scale)
	{
		if (m_vScale != Scale)
		{
			m_vScale = Scale;
			Vector3 vNewScale = Scale * AppInstance.Instance.m_fMainScale;
			vNewScale.z = 1;

			if (m_IsReverse_X == true)
			{
				vNewScale.x *= -1;
			}

			if (m_IsReverse_Y == true)
			{
				vNewScale.y *= -1;
			}

			if (m_pGameObject != null)
			{
				m_pGameObject.transform.localScale = vNewScale;
			}
		}
	}

	public void SetScale(float fScale)
	{
		Vector3 vScale = new Vector3(fScale, fScale, 1);

		SetScale(vScale);
	}

	public Vector3		GetScale()
	{
		return m_vScale;
	}

	public void SetReverse_X(bool IsReverse)
	{
		m_IsReverse_X = IsReverse;

		Vector3 vNewScale = m_vScale * AppInstance.Instance.m_fMainScale;
		vNewScale.z = 1;

		if (m_IsReverse_X == true)
		{
			vNewScale.x *= -1;
		}

		if (m_IsReverse_Y == true)
		{
			vNewScale.y *= -1;
		}

		if (m_pGameObject != null)
		{
			m_pGameObject.transform.localScale = vNewScale;
		}
	}

	public void SetReverse_Y(bool IsReverse)
	{
		m_IsReverse_Y = IsReverse;

		Vector3 vNewScale = m_vScale * AppInstance.Instance.m_fMainScale;
		vNewScale.z = 1;

		if (m_IsReverse_X == true)
		{
			vNewScale.x *= -1;
		}

		if (m_IsReverse_Y == true)
		{
			vNewScale.y *= -1;
		}

		if (m_pGameObject != null)
		{
			m_pGameObject.transform.localScale = vNewScale;
		}
	}

	public void			SetSize(Vector2 Size)
	{
        if (m_vSize != Size)
        {
            m_vSize = Size;
            UpdateUV();
			SetPosition(m_vPosition);

			BoxCollider meshCollider = m_pGameObject.GetComponent<BoxCollider>();
			if (meshCollider != null)
			{
				meshCollider.size = new Vector3(m_vSize.x, m_vSize.y, 0);
			}
		}
	}

	public Vector2		GetSize()
	{
		return m_vSize;
	}

	public void			SetActiveColor(bool IsActive)
	{
		if(m_IsActiveColor == IsActive)
			return;

		m_IsActiveColor = IsActive;

		if (m_IsActiveColor == true)
		{
			if (m_pMaterial_ForActiveColor != null)
			{
				MeshRenderer renderer = m_pGameObject.GetComponent<MeshRenderer>();
				renderer.material = m_pMaterial_ForActiveColor;
			}
			else
			{
				MeshRenderer renderer = m_pGameObject.GetComponent<MeshRenderer>();
				m_pMaterial_ForActiveColor = Material.Instantiate(m_pMaterial) as Material;
				renderer.material = m_pMaterial_ForActiveColor;
			}
		}
		else
		{
			MeshRenderer renderer = m_pGameObject.GetComponent<MeshRenderer>();
			renderer.material = m_pMaterial;
		}
	}

	public void			SetColor(Color color)
	{
		if (m_IsActiveColor == true)
		{
			MeshRenderer renderer = m_pGameObject.GetComponent<MeshRenderer>();
			renderer.material.SetColor("_Color", color);
		}
	}

	public void			SetVisible(bool IsVisible)
	{
		m_IsVisible = IsVisible;

		if (m_pGameObject != null)
		{
			m_pGameObject.SetActive(IsVisible);
		}
	}

	public bool			IsVisible()
	{
		return m_IsVisible;
	}

	public Vector2		GetTextureSize()
	{
		return m_vTextureSize;
	}

	public Material		GetMaterial()
	{
		return m_pMaterial;
	}

	public void			SetTextureInfo(AtlasInfo atlasInfo)
	{
        Rect rc = atlasInfo.GetTexCoord();

		if (m_rcTexCoord != rc )
        {
            m_rcTexCoord = rc;
            UpdateUV();
        }
	}

	public void			SetLayerName(string LayerName)
	{
		if (m_pGameObject != null)
		{
			m_pGameObject.layer = LayerMask.NameToLayer(LayerName);
		}
	}

	public void			SetParameta(object ob)
	{
		m_pParameta = ob;
	}

    public object       GetParameta()
    {
        return m_pParameta;
    }

	public PickingComponent GetPickingComponent()
	{
		return m_pPickingComponent;
	}

	public PickingComponent AddCallback_LButtonDown(PickingComponent.Callback_LButtonDown function)
	{
		if (m_pPickingComponent != null)
		{
			BoxCollider meshCollider = m_pGameObject.GetComponent<BoxCollider>();
			if (meshCollider == null)
			{
				meshCollider = m_pGameObject.AddComponent<BoxCollider>();
			}

			meshCollider.size = new Vector3(m_vSize.x, m_vSize.y, 0);
		}

		m_pPickingComponent.AddCallback_LButtonDown(function);

		return m_pPickingComponent;
	}

	public PickingComponent AddCallback_LButtonUp(PickingComponent.Callback_LButtonUp function)
	{
		if (m_pPickingComponent != null)
		{
			BoxCollider meshCollider = m_pGameObject.GetComponent<BoxCollider>();
			if (meshCollider == null)
			{
				meshCollider = m_pGameObject.AddComponent<BoxCollider>();
			}

			meshCollider.size = new Vector3(m_vSize.x, m_vSize.y, 0);
		}

		m_pPickingComponent.AddCallback_LButtonUp(function);

		return m_pPickingComponent;
	}

	public PickingComponent AddCallback_RButtonDown(PickingComponent.Callback_RButtonDown function)
	{
		if (m_pPickingComponent != null)
		{
			BoxCollider meshCollider = m_pGameObject.GetComponent<BoxCollider>();
			if (meshCollider == null)
			{
				meshCollider = m_pGameObject.AddComponent<BoxCollider>();
			}

			meshCollider.size = new Vector3(m_vSize.x, m_vSize.y, 0);
		}

		m_pPickingComponent.AddCallback_RButtonDown(function);

		return m_pPickingComponent;
	}

	public PickingComponent AddCallback_RButtonUp(PickingComponent.Callback_RButtonUp function)
	{
		if (m_pPickingComponent != null)
		{
			BoxCollider meshCollider = m_pGameObject.GetComponent<BoxCollider>();
			if (meshCollider == null)
			{
				meshCollider = m_pGameObject.AddComponent<BoxCollider>();
			}

			meshCollider.size = new Vector3(m_vSize.x, m_vSize.y, 0);
		}

		m_pPickingComponent.AddCallback_RButtonUp(function);

		return m_pPickingComponent;
	}

	public PickingComponent AddCallback_Move(PickingComponent.Callback_Move function)
	{
		if (m_pPickingComponent != null)
		{
			BoxCollider meshCollider = m_pGameObject.GetComponent<BoxCollider>();
			if (meshCollider == null)
			{
				meshCollider = m_pGameObject.AddComponent<BoxCollider>();
			}

			meshCollider.size = new Vector3(m_vSize.x, m_vSize.y, 0);
		}

		m_pPickingComponent.AddCallback_Move(function);

		return m_pPickingComponent;
	}

	public PickingComponent AddCallback_Touch(PickingComponent.Callback_Touch function)
	{
		if (m_pPickingComponent != null)
		{
			BoxCollider meshCollider = m_pGameObject.GetComponent<BoxCollider>();
			if (meshCollider == null)
			{
				meshCollider = m_pGameObject.AddComponent<BoxCollider>();
			}

			meshCollider.size = new Vector3(m_vSize.x, m_vSize.y, 0);
		}

		m_pPickingComponent.AddCallback_Touch(function);

		return m_pPickingComponent;
	}

	public void         SetTextureAddressMode(eTextureAddressMode Mode)
	{
        if (m_eTextureAddressMode != Mode)
        {
            m_eTextureAddressMode = Mode;
            UpdateUV();
        }
	}

    public void         SetTextureAddressMode_UV(Rect vUV)
    {
        if (m_rcTextureAddressMode_UV != vUV)
        {
            m_rcTextureAddressMode_UV = vUV;

            if (m_eTextureAddressMode == eTextureAddressMode.UV)
            {
                UpdateUV();
            }
        }
    }

	public eTextureAddressMode GetTextureAddresMode()
	{
		return m_eTextureAddressMode;
	}

	private void		UpdateUV()
	{
		switch (m_eTextureAddressMode)
		{
			case eTextureAddressMode.Stretch:	    UpdateUV_Stretch();		break;
			case eTextureAddressMode.Horizontal:    UpdateUV_Horizotal();	break;
			case eTextureAddressMode.Vertical:	    UpdateUV_Vertical();	break;
			case eTextureAddressMode.Both:		    UpdateUV_Both();		break;
            case eTextureAddressMode.UV:            UpdateUV_UV();          break;
		}
	}

	private void		UpdateUV_Stretch()
	{
		MeshFilter	meshFilter	= m_pGameObject.GetComponent<MeshFilter>();
		Mesh		mesh		= meshFilter.mesh;

		mesh.vertices = new Vector3[]
		{
			new Vector3(m_vSize.x * -0.5f, m_vSize.y *  0.5f, 0),
			new Vector3(m_vSize.x *  0.5f, m_vSize.y *  0.5f, 0),
			new Vector3(m_vSize.x * -0.5f, m_vSize.y * -0.5f, 0),
			new Vector3(m_vSize.x *  0.5f, m_vSize.y * -0.5f, 0)
		};

		mesh.uv = new Vector2[]
		{
			new Vector2(m_rcTexCoord.xMin, m_rcTexCoord.yMax),
			new Vector2(m_rcTexCoord.xMax, m_rcTexCoord.yMax),
			new Vector2(m_rcTexCoord.xMin, m_rcTexCoord.yMin),
			new Vector2(m_rcTexCoord.xMax, m_rcTexCoord.yMin)
		};

		mesh.triangles = new int[] { 0, 1, 2, 2, 1, 3 };
	}

	private void		UpdateUV_Horizotal()
	{
		MeshFilter	meshFilter	= m_pGameObject.GetComponent<MeshFilter>();
		Mesh		mesh		= meshFilter.mesh;
		mesh.Clear();

		float Ratio =  m_vSize.x / m_vTextureSize.x;
		Vector2 vLeftTop = new Vector2(m_vSize.x * -0.5f, m_vSize.y * -0.5f);

		int PitchCount = (int)Ratio;
		float TileRatio = 1.0f - (Ratio - PitchCount);
		int Split = PitchCount + (Ratio - PitchCount > 0 ? 1 : 0);

		Vector3[]	Vertices	= new Vector3[Split * 4];
		Vector2[]	UV			= new Vector2[Split * 4];
		int[]		Indices		= new int[Split * 6];

		for (int i = 0; i < PitchCount; ++i)
		{
			Vertices[i * 4] = new Vector3(vLeftTop.x + i * m_vTextureSize.x - 0.5f, m_vSize.y * 0.5f, 0);
			Vertices[i * 4 + 1] = new Vector3(vLeftTop.x + (i + 1) * m_vTextureSize.x + 0.5f, m_vSize.y * 0.5f, 0);
			Vertices[i * 4 + 2] = new Vector3(vLeftTop.x + i * m_vTextureSize.x - 0.5f, m_vSize.y * -0.5f, 0);
			Vertices[i * 4 + 3] = new Vector3(vLeftTop.x + (i + 1) * m_vTextureSize.x + 0.5f, m_vSize.y * -0.5f, 0);

			UV[i*4]				= new Vector2(m_rcTexCoord.xMin, m_rcTexCoord.yMax);
			UV[i*4+1]			= new Vector2(m_rcTexCoord.xMax, m_rcTexCoord.yMax);
			UV[i*4+2]			= new Vector2(m_rcTexCoord.xMin, m_rcTexCoord.yMin);
			UV[i*4+3]			= new Vector2(m_rcTexCoord.xMax, m_rcTexCoord.yMin);
		}

		if ((Ratio - PitchCount) > 0)
		{
			int Index = PitchCount * 4;
			Vertices[Index] = new Vector3(vLeftTop.x + PitchCount * m_vTextureSize.x - 0.5f, m_vSize.y * 0.5f, 0);
			Vertices[Index + 1] = new Vector3(vLeftTop.x + (PitchCount + 1) * m_vTextureSize.x - m_vTextureSize.x * TileRatio + 0.5f, m_vSize.y * 0.5f, 0);
			Vertices[Index + 2] = new Vector3(vLeftTop.x + PitchCount * m_vTextureSize.x - 0.5f, m_vSize.y * -0.5f, 0);
			Vertices[Index + 3] = new Vector3(vLeftTop.x + (PitchCount + 1) * m_vTextureSize.x - m_vTextureSize.x * TileRatio + 0.5f, m_vSize.y * -0.5f, 0);

			UV[Index]			= new Vector2(m_rcTexCoord.xMin,								m_rcTexCoord.yMax);
			UV[Index + 1]		= new Vector2(m_rcTexCoord.xMax - TileRatio / m_vTextureSize.x,	m_rcTexCoord.yMax);
			UV[Index + 2]		= new Vector2(m_rcTexCoord.xMin,								m_rcTexCoord.yMin);
			UV[Index + 3]		= new Vector2(m_rcTexCoord.xMax - TileRatio / m_vTextureSize.x,	m_rcTexCoord.yMin);
		}

		for (int i = 0; i < Split; ++i)
		{
			Indices[i*6]		= i*4;
			Indices[i*6+1]		= i*4+1;
			Indices[i*6+2]		= i*4+2;
			Indices[i*6+3]		= i*4+2;
			Indices[i*6+4]		= i*4+1;
			Indices[i*6+5]		= i*4+3;
		}

		mesh.vertices	= Vertices;
		mesh.uv			= UV;
		mesh.triangles	= Indices;
	}

	private void		UpdateUV_Vertical()
	{
		MeshFilter meshFilter = m_pGameObject.GetComponent<MeshFilter>();
		Mesh mesh = meshFilter.mesh;

		float Ratio = m_vSize.y / m_vTextureSize.y;
		Vector2 vLeftTop = new Vector2(m_vSize.x * -0.5f, m_vSize.y * 0.5f);

		int PitchCount = (int)Ratio;
		float TileRatio = 1.0f - (Ratio - PitchCount);
		int Split = PitchCount + (Ratio - PitchCount > 0 ? 1 : 0);

		Vector3[]	Vertices	= new Vector3[Split * 4];
		Vector2[]	UV			= new Vector2[Split * 4];
		int[]		Indices		= new int[Split * 6];

		for (int i = 0; i < PitchCount; ++i)
		{
			Vertices[i * 4] = new Vector3(m_vSize.x * -0.5f, vLeftTop.y + i * -m_vTextureSize.y + 0.5f, 0);
			Vertices[i * 4 + 1] = new Vector3(m_vSize.x * 0.5f, vLeftTop.y + i * -m_vTextureSize.y + 0.5f, 0);
			Vertices[i * 4 + 2] = new Vector3(m_vSize.x * -0.5f, vLeftTop.y + (i + 1) * -m_vTextureSize.y - 0.5f, 0);
			Vertices[i * 4 + 3] = new Vector3(m_vSize.x * 0.5f, vLeftTop.y + (i + 1) * -m_vTextureSize.y - 0.5f, 0);

			UV[i * 4]			= new Vector2(m_rcTexCoord.xMin, m_rcTexCoord.yMax);
			UV[i * 4 + 1]		= new Vector2(m_rcTexCoord.xMax, m_rcTexCoord.yMax);
			UV[i * 4 + 2]		= new Vector2(m_rcTexCoord.xMin, m_rcTexCoord.yMin);
			UV[i * 4 + 3]		= new Vector2(m_rcTexCoord.xMax, m_rcTexCoord.yMin);
		}

		if ((Ratio - PitchCount) > 0)
		{
			int Index = PitchCount * 4;
			Vertices[Index] = new Vector3(m_vSize.x * -0.5f, vLeftTop.y + PitchCount * -m_vTextureSize.y + 0.5f, 0);
			Vertices[Index + 1] = new Vector3(m_vSize.x * 0.5f, vLeftTop.y + PitchCount * -m_vTextureSize.y + 0.5f, 0);
			Vertices[Index + 2] = new Vector3(m_vSize.x * -0.5f, vLeftTop.y + (PitchCount + 1) * -m_vTextureSize.y + m_vTextureSize.x * TileRatio - 0.5f, 0);
			Vertices[Index + 3] = new Vector3(m_vSize.x * 0.5f, vLeftTop.y + (PitchCount + 1) * -m_vTextureSize.y + m_vTextureSize.x * TileRatio - 0.5f, 0);

			UV[Index]			= new Vector2(m_rcTexCoord.xMin, m_rcTexCoord.yMax);
			UV[Index+1]			= new Vector2(m_rcTexCoord.xMax, m_rcTexCoord.yMax - TileRatio / m_vTextureSize.y);
			UV[Index+2]			= new Vector2(m_rcTexCoord.xMin, m_rcTexCoord.yMin);
			UV[Index+3]			= new Vector2(m_rcTexCoord.xMax, m_rcTexCoord.yMin - TileRatio / m_vTextureSize.y);
		}

		for (int i = 0; i < Split; ++i)
		{
			Indices[i * 6] = i * 4 + 0;
			Indices[i * 6 + 1] = i * 4 + 1;
			Indices[i * 6 + 2] = i * 4 + 2;
			Indices[i * 6 + 3] = i * 4 + 2;
			Indices[i * 6 + 4] = i * 4 + 1;
			Indices[i * 6 + 5] = i * 4 + 3;
		}

		mesh.vertices	= Vertices;
		mesh.uv			= UV;
		mesh.triangles	= Indices;
	}

	private void		UpdateUV_Both()
	{
		MeshFilter	meshFilter	= m_pGameObject.GetComponent<MeshFilter>();
		Mesh		mesh		= meshFilter.mesh;
	}

    private void        UpdateUV_UV()
    {
        Vector2 vUV_Min = new Vector2(m_rcTextureAddressMode_UV.x, m_rcTextureAddressMode_UV.y);
        Vector2 vUV_Max = new Vector2(m_rcTextureAddressMode_UV.width, m_rcTextureAddressMode_UV.height);

        MeshFilter meshFilter = m_pGameObject.GetComponent<MeshFilter>();
        Mesh mesh = meshFilter.mesh;

        Vector2 vLeftTop = new Vector2(m_vTextureSize.x * -0.5f, m_vTextureSize.y * 0.5f);
        Vector2 vRightBottom = new Vector2(vLeftTop.x + m_vTextureSize.x, vLeftTop.y - m_vTextureSize.y);

        vLeftTop.x += m_vTextureSize.x * vUV_Min.x;
        vLeftTop.y -= m_vTextureSize.y * vUV_Min.y;

        vRightBottom.x -= m_vTextureSize.x * (1 - vUV_Max.x);
        vRightBottom.y += m_vTextureSize.y * (1 - vUV_Max.y);

        mesh.vertices = new Vector3[]
        {
			new Vector3(vRightBottom.x, vLeftTop.y, 0),
			new Vector3(vRightBottom.x, vRightBottom.y, 0),
			new Vector3(vLeftTop.x, vRightBottom.y, 0),
			new Vector3(vLeftTop.x, vLeftTop.y, 0)
		};

        float fUVInterval_X = m_rcTexCoord.xMax - m_rcTexCoord.xMin;
        float fUVInterval_Y = m_rcTexCoord.yMax - m_rcTexCoord.yMin;

        Rect rc = m_rcTexCoord;
        rc.xMin += vUV_Min.x * fUVInterval_X;
        rc.yMin += vUV_Min.y * fUVInterval_Y;
        rc.xMax -= (1 - vUV_Max.x) * fUVInterval_X;
        rc.yMax -= (1 - vUV_Max.y) * fUVInterval_Y;

        mesh.uv = new Vector2[]
        {
            new Vector2(rc.xMax, rc.yMax),
            new Vector2(rc.xMax, rc.yMin),
            new Vector2(rc.xMin, rc.yMin),
            new Vector2(rc.xMin, rc.yMax)
        };

        mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
    }

	public void			SetPlaneAnchorPosition(ePlaneAnchorPosition eAnchor)
	{
		if (m_ePlaneAnchorPosition != eAnchor)
		{
			m_ePlaneAnchorPosition = eAnchor;
			SetPosition(m_vPosition);
		}
	}

	public ePlaneAnchorPosition GetPlaneAnchorPosition()
	{
		return m_ePlaneAnchorPosition;
	}

	public void			SetScreenAnchorPosition(eScreenAnchorPosition eAnchor)
	{
		if (m_eScreenAnchorPosition != eAnchor)
		{
			m_eScreenAnchorPosition = eAnchor;
			SetPosition(m_vPosition);
		}
	}

	public eScreenAnchorPosition GetScreenAnchorPosition()
	{
		return m_eScreenAnchorPosition;
	}

	public GameObject GetGameObject()
	{
		return m_pGameObject;
	}
}
