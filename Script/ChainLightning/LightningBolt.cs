using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LightningBolt
{
	public LineRenderer[]		m_pLineRenderer		= null;
	public LineRenderer			m_pLightRenderer	= null;
	public float				m_fSegmentLength	= 0;

	public LightningBolt(float segmentLength)
	{
		m_fSegmentLength = segmentLength;
	}

	public void Init(int lineRendererCount, GameObject lineRendererPrefab, GameObject lightRendererPrefab)
	{
		m_pLineRenderer = new LineRenderer[lineRendererCount];
		for (int i = 0; i < lineRendererCount; i++)
		{
			m_pLineRenderer[i] = (GameObject.Instantiate(lineRendererPrefab) as GameObject).GetComponent<LineRenderer>();
			m_pLineRenderer[i].enabled = false;
		}
		m_pLightRenderer = (GameObject.Instantiate(lightRendererPrefab) as GameObject).GetComponent<LineRenderer>();
	}

	public void OnDestroy()
	{
		for (int i = 0; i < m_pLineRenderer.Length; i++)
		{
			GameObject.Destroy(m_pLineRenderer[i].gameObject);
		}
		GameObject.Destroy(m_pLightRenderer.gameObject);
	}

	public void Activate()
	{
		for (int i = 0; i < m_pLineRenderer.Length; i++)
		{
			m_pLineRenderer[i].enabled = true;
		}
		m_pLightRenderer.enabled = true;
	}

	public void DrawLightning(Vector3 source, Vector3 target)
	{
		float distance = Vector3.Distance(source, target);
		int segments;
		if (distance > m_fSegmentLength)
		{
			segments = Mathf.FloorToInt(distance / m_fSegmentLength) + 2;
		}
		else
		{
			segments = 4;
		}

		float fRandom = 10.0f * InGameInfo.Instance.m_fSlotScale;

		for (int i = 0; i < m_pLineRenderer.Length; i++)
		{
			m_pLineRenderer[i].positionCount = segments;
			m_pLineRenderer[i].SetPosition(0, source);
			Vector3 lastPosition;
			for (int j = 1; j < segments - 1; j++)
			{
				Vector3 tmp = Vector3.Lerp(source, target, (float)j / (float)segments);
				lastPosition = new Vector3(tmp.x + Random.Range(-fRandom, fRandom), tmp.y + Random.Range(-fRandom, fRandom));
				m_pLineRenderer[i].SetPosition(j, lastPosition);
			}

			float fRandom_Last = 5.0f *InGameInfo.Instance.m_fSlotScale;
			target = new Vector3(target.x + Random.Range(-fRandom_Last, fRandom_Last), target.y + Random.Range(-fRandom_Last, fRandom_Last));
			m_pLineRenderer[i].SetPosition(segments - 1, target);
		}

		m_pLightRenderer.SetPosition(0, source);
		m_pLightRenderer.SetPosition(1, target);
		Color lightColor = new Color(0.5647f, 0.58823f, 1f, Random.Range(0.2f, 1f));
		m_pLightRenderer.startColor = lightColor;
		m_pLightRenderer.endColor = lightColor;
	}
}
