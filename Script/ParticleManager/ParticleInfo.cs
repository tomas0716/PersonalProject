using UnityEngine;
using System.Collections;
using System;

public class ParticleInfo
{
    public delegate void OnDone_Effect(ParticleInfo particleInfo);
    private OnDone_Effect       m_Callback              = null;

	private GameObject			m_pGameObject_Particle	= null;

	private Transformer_Scalar	m_pTimer				= new Transformer_Scalar(0);

	private Vector3				m_vPosition				= Vector3.zero;
	private Quaternion			m_QuatRotation			= new Quaternion(0, 0, 0, 1);
	private float				m_fScale				= 1.0f;
	private float				m_fScale_StartSizeX		= 1.0f;
	private float				m_fScale_StartSizeY		= 1.0f;
	private float				m_fScale_StartSizeZ		= 1.0f;
	private Quaternion			m_QuatOrigin			= new Quaternion(0, 0, 0, 1);

    private SlotUnit            m_pAttachUnit           = null;

    public enum eParticleState
    {
        eParticleState_Stop,
        eParticleState_Pause,
        eParticleState_Play,
    }

	private eParticleState		_eParticleState			= eParticleState.eParticleState_Play;

	private PickingComponent	m_pPickingComponent		= null;

	public              ParticleInfo()
	{
	}

	public void Destroy()
	{
		if (m_pGameObject_Particle != null)
		{
			GameObject.Destroy(m_pGameObject_Particle);
			m_pGameObject_Particle = null;
		}
	}

	public void			Update()
	{
		m_pTimer.Update (Time.deltaTime);

        if (m_pAttachUnit != null && m_pGameObject_Particle != null)
        {
            Vector3 vPos = m_pAttachUnit.GetPosition();
            vPos *= AppInstance.Instance.m_fMainScale;
            m_pGameObject_Particle.transform.position = vPos;
        }
	}

    public void         LoadParticleSystem(string FileName, Vector3 vPos, SlotUnit pUnit = null)
	{
        LoadParticleSystem(FileName, vPos, new Quaternion(0, 0, 0, 1), pUnit);
	}

    public void         LoadParticleSystem(string FileName, Vector3 vPos, Quaternion QuatRot, SlotUnit pUnit = null)
	{
        m_vPosition = vPos;
        m_QuatRotation = QuatRot;
        m_pAttachUnit = pUnit;

        GameObject ob = Resources.Load("Effect/Prefabs/" + FileName) as GameObject;

		if (ob == null)
		{
			AppInstance.Instance.m_pParticleManager.RemoveParticleInfo(this);
			return;
		}

		m_pGameObject_Particle = (GameObject)GameObject.Instantiate(ob);
		m_QuatOrigin = m_pGameObject_Particle.transform.rotation;

		m_pPickingComponent = m_pGameObject_Particle.AddComponent<PickingComponent>();

		Helper.SetLayer(m_pGameObject_Particle, LayerMask.NameToLayer("2D"));

		ParticleSystem ps = m_pGameObject_Particle.GetComponent<ParticleSystem>();
        
		if (ps.main.loop == false)
		{
			float time = ps.main.duration + ps.main.startLifetime.constant + ps.main.startDelay.constant;

			TransformerEvent_Scalar eventValue = null;
			eventValue = new TransformerEvent_Scalar(time, 0);
			m_pTimer.AddEvent(eventValue);
			m_pTimer.SetCallback(null, OnDone_Timer);
		}

		SetPosition(m_vPosition);
		SetRotation(m_QuatRotation);
		//SetScale(m_fScale);

		switch (_eParticleState)
		{
			case eParticleState.eParticleState_Pause: OnPause(); break;
			case eParticleState.eParticleState_Play: OnPlay(); break;
			case eParticleState.eParticleState_Stop: OnStop(); break;
		}
	}

	public void			SetVisible(bool IsVisible)
	{
		if (m_pGameObject_Particle != null)
		{
			m_pGameObject_Particle.SetActive(IsVisible);
		}
	}

	public void 		OnPlay()
	{
		if( m_pGameObject_Particle != null )
		{
            ParticleSystem ps = m_pGameObject_Particle.GetComponent<ParticleSystem>();
            if (ps != null) ps.Play();

            m_pTimer.OnPlay();
		}

        _eParticleState = eParticleState.eParticleState_Play;
	}
	
	public void 		OnStop()
	{
		if( m_pGameObject_Particle != null )
		{
            ParticleSystem ps = m_pGameObject_Particle.GetComponent<ParticleSystem>();
            if (ps != null) ps.Stop();

            m_pTimer.OnStop();
		}

        _eParticleState = eParticleState.eParticleState_Stop;
	}
	
	public void 		OnPause()
	{
		if( m_pGameObject_Particle != null )
		{
            ParticleSystem ps = m_pGameObject_Particle.GetComponent<ParticleSystem>();
            if (ps != null) ps.Pause();

            m_pTimer.OnPause();
		}

        _eParticleState = eParticleState.eParticleState_Pause;
	}
	
	public void 		SetPosition(Vector3 vPos)
	{
		if( m_pGameObject_Particle != null )
		{
			m_pGameObject_Particle.transform.position = vPos;
		}

        m_vPosition = vPos;
	}
	
	public Vector3 		GetPosition()
	{
        return m_vPosition;
	}	
	
	public void 		SetRotation(Vector3 vRot)
	{
        SetRotation(Quaternion.Euler(vRot));	
	}
	
	public void 		SetRotation(Quaternion quatRot)
	{
		if( m_pGameObject_Particle != null )
		{
            m_pGameObject_Particle.transform.rotation = m_QuatOrigin * quatRot;
		}

        m_QuatRotation = quatRot;
	}
	
	public Quaternion 	GetRotation()
	{
        return m_QuatRotation;
	}
	
	public void 		SetScale(float scale)
	{
		if( m_pGameObject_Particle != null )
		{			
			ParticleSystem Parent = m_pGameObject_Particle.GetComponent<ParticleSystem>();
			
			foreach(ParticleSystem system in m_pGameObject_Particle.GetComponentsInChildren<ParticleSystem>())
			{			
				if( system != null && system.emission.enabled == true )
				{
					system.Clear(true);
                    ParticleSystem.MainModule main = system.main;

                    float size = system.main.startSize.constant / m_fScale;
                    float size_min = system.main.startSize.constantMin / m_fScale;
                    float size_max = system.main.startSize.constantMax / m_fScale;
                    ParticleSystem.MinMaxCurve startSizeCurve = system.main.startSize;
                    startSizeCurve.constant = size * scale;
                    startSizeCurve.constantMin = size_min * scale;
                    startSizeCurve.constantMax = size_max * scale;
                    main.startSize = startSizeCurve;

					if (system.main.startSize3D == true)
					{
						ParticleSystem.MinMaxCurve startSizeX = system.main.startSizeX;
						startSizeX.constant = size * scale;
						startSizeX.constantMin = size_min * scale;
						startSizeX.constantMax = size_max * scale;
						main.startSizeX = startSizeX;

						ParticleSystem.MinMaxCurve startSizeY = system.main.startSizeY;
						startSizeY.constant = size * scale;
						startSizeY.constantMin = size_min * scale;
						startSizeY.constantMax = size_max * scale;
						main.startSizeY = startSizeY;

						ParticleSystem.MinMaxCurve startSizeZ = system.main.startSizeZ;
						startSizeZ.constant = size * scale;
						startSizeZ.constantMin = size_min * scale;
						startSizeZ.constantMax = size_max * scale;
						main.startSizeZ = startSizeZ;
					}

					ParticleSystem.MinMaxCurve gravityCurve = system.main.gravityModifier;
					float fGravity = system.main.gravityModifier.constant / m_fScale;
					gravityCurve.constant = fGravity * scale;

					float speed = system.main.startSpeed.constant / m_fScale;
                    float speed_min = system.main.startSpeed.constantMin / m_fScale;
                    float speed_max = system.main.startSpeed.constantMax / m_fScale;
                    ParticleSystem.MinMaxCurve startSpeedCurve = system.main.startSpeed;
                    startSpeedCurve.constant = speed * scale;
                    startSpeedCurve.constantMin = speed_min * scale;
                    startSpeedCurve.constantMax = speed_max * scale;
                    main.startSpeed = startSpeedCurve;

					ParticleSystem.MinMaxCurve sizeOverLifetime = system.sizeOverLifetime.size;
					float sizeoverLifetime_min = sizeOverLifetime.constantMin / m_fScale;
					float sizeoverLifetime_max = sizeOverLifetime.constantMax / m_fScale;
					float fsizeOverLifetime = system.sizeOverLifetime.size.constant / m_fScale;
					sizeOverLifetime.constant = fsizeOverLifetime * scale;
					startSpeedCurve.constantMin = sizeoverLifetime_min * scale;
					startSpeedCurve.constantMax = sizeoverLifetime_max * scale;

					ParticleSystem.ShapeModule shapeModule = system.shape;
					float fShapeRadius = shapeModule.radius / m_fScale;
					shapeModule.radius = fShapeRadius * scale;

					Vector3 vShapePosition = shapeModule.position / m_fScale;
					shapeModule.position = vShapePosition * scale;

					Vector3 vShapeScale = shapeModule.scale / m_fScale;
					shapeModule.scale = vShapeScale * scale;

					ParticleSystem.LimitVelocityOverLifetimeModule limitVelocityOverLifetimeModule = system.limitVelocityOverLifetime;
					float limitMultiplier = limitVelocityOverLifetimeModule.limitMultiplier / m_fScale;
					limitVelocityOverLifetimeModule.limitMultiplier = limitMultiplier * scale;

					if (Parent != system)
					{
						Vector3 vPos = system.transform.localPosition / m_fScale;
						system.transform.localPosition = vPos * scale;
					}

					system.Play();
				}
			}
		}

        OnPlay();

        m_fScale = scale;
	}

	public void SetScale_ForElectrictyBolt(float scale, float fHeightScale)
	{
		if (m_pGameObject_Particle != null)
		{
			ParticleSystem Parent = m_pGameObject_Particle.GetComponent<ParticleSystem>();

			foreach (ParticleSystem system in m_pGameObject_Particle.GetComponentsInChildren<ParticleSystem>())
			{
				if (system != null && system.emission.enabled == true)
				{
					float fTempScale = scale;
					if (system.gameObject.name == "ElectricitySmall")
					{
						fTempScale = fHeightScale;
					}
					
					system.Clear(true);
					ParticleSystem.MainModule main = system.main;

					if (system.main.startSize3D == false)
					{
						float size = system.main.startSize.constant / m_fScale;
						float size_min = system.main.startSize.constantMin / m_fScale;
						float size_max = system.main.startSize.constantMax / m_fScale;
						ParticleSystem.MinMaxCurve startSizeCurve = system.main.startSize;
						startSizeCurve.constant = size * fTempScale;
						startSizeCurve.constantMin = size_min * fTempScale;
						startSizeCurve.constantMax = size_max * fTempScale;
						main.startSize = startSizeCurve;
					}
					else if (system.main.startSize3D == true)
					{
						float size = system.main.startSizeX.constant / m_fScale_StartSizeX;
						float size_min = system.main.startSizeX.constantMin / m_fScale_StartSizeX;
						float size_max = system.main.startSizeX.constantMax / m_fScale_StartSizeX;
						ParticleSystem.MinMaxCurve startSizeX = system.main.startSizeX;
						startSizeX.constant = size * fTempScale;
						startSizeX.constantMin = size_min * fTempScale;
						startSizeX.constantMax = size_max * fTempScale;
						main.startSizeX = startSizeX;

						size = system.main.startSizeY.constant / m_fScale_StartSizeY;
						size_min = system.main.startSizeY.constantMin / m_fScale_StartSizeY;
						size_max = system.main.startSizeY.constantMax / m_fScale_StartSizeY;
						ParticleSystem.MinMaxCurve startSizeY = system.main.startSizeY;
						startSizeY.constant = size * fHeightScale;
						startSizeY.constantMin = size_min * fHeightScale;
						startSizeY.constantMax = size_max * fHeightScale;
						main.startSizeY = startSizeY;

						size = system.main.startSizeZ.constant / m_fScale_StartSizeZ;
						size_min = system.main.startSizeZ.constantMin / m_fScale_StartSizeZ;
						size_max = system.main.startSizeZ.constantMax / m_fScale_StartSizeZ;
						ParticleSystem.MinMaxCurve startSizeZ = system.main.startSizeZ;
						startSizeZ.constant = size * fTempScale;
						startSizeZ.constantMin = size_min * fTempScale;
						startSizeZ.constantMax = size_max * fTempScale;
						main.startSizeZ = startSizeZ;
					}

					ParticleSystem.MinMaxCurve gravityCurve = system.main.gravityModifier;
					float fGravity = system.main.gravityModifier.constant / m_fScale;
					gravityCurve.constant = fGravity * fTempScale;

					float speed = system.main.startSpeed.constant / m_fScale;
					float speed_min = system.main.startSpeed.constantMin / m_fScale;
					float speed_max = system.main.startSpeed.constantMax / m_fScale;
					ParticleSystem.MinMaxCurve startSpeedCurve = system.main.startSpeed;
					startSpeedCurve.constant = speed * fTempScale;
					startSpeedCurve.constantMin = speed_min * fTempScale;
					startSpeedCurve.constantMax = speed_max * fTempScale;
					main.startSpeed = startSpeedCurve;

					ParticleSystem.MinMaxCurve sizeOverLifetime = system.sizeOverLifetime.size;
					float sizeoverLifetime_min = sizeOverLifetime.constantMin / m_fScale;
					float sizeoverLifetime_max = sizeOverLifetime.constantMax / m_fScale;
					float fsizeOverLifetime = system.sizeOverLifetime.size.constant / m_fScale;
					sizeOverLifetime.constant = fsizeOverLifetime * fTempScale;
					startSpeedCurve.constantMin = sizeoverLifetime_min * fTempScale;
					startSpeedCurve.constantMax = sizeoverLifetime_max * fTempScale;

					ParticleSystem.ShapeModule shapeModule = system.shape;
					float fShapeRadius = shapeModule.radius / m_fScale;
					shapeModule.radius = fShapeRadius * fTempScale;

					Vector3 vShapePosition = shapeModule.position / m_fScale;
					shapeModule.position = vShapePosition * fTempScale;

					if (system.gameObject.name == "ElectricitySmall")
					{
						Vector3 vShapeScale = shapeModule.scale / m_fScale;
						vShapeScale.z *= fHeightScale;
						vShapeScale.x *= fHeightScale * 0.08f;
						vShapeScale.y *= fHeightScale * 0.08f;
						shapeModule.scale = vShapeScale;

						if (Parent != system)
						{
							Vector3 vPos = system.transform.localPosition / m_fScale;
							system.transform.localPosition = vPos * fHeightScale;
						}
					}
					else if (Parent != system)
					{
						Vector3 vPos = system.transform.localPosition / m_fScale;
						system.transform.localPosition = vPos * fTempScale;
					}

					system.Play();
				}
			}
		}

		OnPlay();

		m_fScale = scale;
		//m_fScale_StartSizeX = fStartSize_Scale_X;
		//m_fScale_StartSizeY = fStartSize_Scale_Y;
		//m_fScale_StartSizeZ = fStartSize_Scale_Z;
	}
	
	public float 		GetScale()
	{
		return m_fScale;
	}

	public GameObject GetGameObject()
	{
		return m_pGameObject_Particle;
	}
	
	public void 		OnDone_Timer( TransformerEvent eventValue )
	{
		m_Callback?.Invoke(this);
		AppInstance.Instance.m_pParticleManager.RemoveParticleInfo(this);
	}

	public PickingComponent GetPickingComponent()
	{
		return m_pPickingComponent;
	}

	public PickingComponent AddCallback_LButtonDown(PickingComponent.Callback_LButtonDown function, Vector3 vSize)
	{
		if (m_pPickingComponent != null)
		{
			BoxCollider meshCollider = m_pGameObject_Particle.GetComponent<BoxCollider>();
			if (meshCollider == null)
			{
				meshCollider = m_pGameObject_Particle.AddComponent<BoxCollider>();
			}

			meshCollider.size = new Vector3(vSize.x, vSize.y, 0);

			m_pPickingComponent.AddCallback_LButtonDown(function);
		}

		return m_pPickingComponent;
	}

	public PickingComponent AddCallback_LButtonUp(PickingComponent.Callback_LButtonUp function, Vector3 vSize)
	{
		if (m_pPickingComponent != null)
		{
			BoxCollider meshCollider = m_pGameObject_Particle.GetComponent<BoxCollider>();
			if (meshCollider == null)
			{
				meshCollider = m_pGameObject_Particle.AddComponent<BoxCollider>();
			}

			meshCollider.size = new Vector3(vSize.x, vSize.y, 0);

			m_pPickingComponent.AddCallback_LButtonUp(function);
		}

		return m_pPickingComponent;
	}

	public PickingComponent AddCallback_RButtonDown(PickingComponent.Callback_RButtonDown function, Vector3 vSize)
	{
		if (m_pPickingComponent != null)
		{
			BoxCollider meshCollider = m_pGameObject_Particle.GetComponent<BoxCollider>();
			if (meshCollider == null)
			{
				meshCollider = m_pGameObject_Particle.AddComponent<BoxCollider>();
			}

			meshCollider.size = new Vector3(vSize.x, vSize.y, 0);

			m_pPickingComponent.AddCallback_RButtonDown(function);
		}

		return m_pPickingComponent;
	}

	public PickingComponent AddCallback_RButtonUp(PickingComponent.Callback_RButtonUp function, Vector3 vSize)
	{
		if (m_pPickingComponent != null)
		{
			BoxCollider meshCollider = m_pGameObject_Particle.GetComponent<BoxCollider>();
			if (meshCollider == null)
			{
				meshCollider = m_pGameObject_Particle.AddComponent<BoxCollider>();
			}

			meshCollider.size = new Vector3(vSize.x, vSize.y, 0);

			m_pPickingComponent.AddCallback_RButtonUp(function);
		}

		return m_pPickingComponent;
	}

	public PickingComponent AddCallback_Move(PickingComponent.Callback_Move function, Vector3 vSize)
	{
		if (m_pPickingComponent != null)
		{
			BoxCollider meshCollider = m_pGameObject_Particle.GetComponent<BoxCollider>();
			if (meshCollider == null)
			{
				meshCollider = m_pGameObject_Particle.AddComponent<BoxCollider>();
			}

			meshCollider.size = new Vector3(vSize.x, vSize.y, 0);

			m_pPickingComponent.AddCallback_Move(function);
		}

		return m_pPickingComponent;
	}

	public PickingComponent AddCallback_Touch(PickingComponent.Callback_Touch function, Vector3 vSize)
	{
		if (m_pPickingComponent != null)
		{
			BoxCollider meshCollider = m_pGameObject_Particle.GetComponent<BoxCollider>();
			if (meshCollider == null)
			{
				meshCollider = m_pGameObject_Particle.AddComponent<BoxCollider>();
			}

			meshCollider.size = new Vector3(vSize.x, vSize.y, 0);

			m_pPickingComponent.AddCallback_Touch(function);
		}

		return m_pPickingComponent;
	}

	public void         SetCallback(OnDone_Effect Callback)
	{
		m_Callback = Callback;
	}
}