using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TexturePacker
{
	public int  m_nWidth       = 0;
	public int  m_nHeight      = 0;
	public bool m_IsRotation   = false;

	public List<Rect> m_UsedRectangleList = new List<Rect>();
	public List<Rect> m_FreeRectangleList = new List<Rect>();

	public enum FreeRectChoiceHeuristic
	{
		RectBestShortSideFit,
		RectBestLongSideFit,
		RectBestAreaFit,
		RectBottomLeftRule,
		RectContactPointRule
	};

	private struct Storage
	{
		public Rect rect;
		public bool paddingX;
		public bool paddingY;
	}

	public TexturePacker(int width, int height, bool rotations)
	{
		Init(width, height, rotations);
	}

	public void Init(int width, int height, bool rotations)
	{
		m_nWidth = width;
		m_nHeight = height;
		m_IsRotation = rotations;

		Rect n = new Rect();
		n.x = 0;
		n.y = 0;
		n.width = width;
		n.height = height;

		m_UsedRectangleList.Clear();

		m_FreeRectangleList.Clear();
		m_FreeRectangleList.Add(n);
	}

	public static Rect[] PackTextures(Texture2D texture, Texture2D[] textures, int width, int height, int padding, int maxSize)
	{
		if (width > maxSize && height > maxSize)
			return null;

		if (width > maxSize || height > maxSize)
		{ 
			int temp = width;
			width = height; 
			height = temp; 
		}

		TexturePacker bp = new TexturePacker(width, height, false);
		Storage[] storage = new Storage[textures.Length];

		for (int i = 0; i < textures.Length; i++)
		{
			Texture2D tex = textures[i];

			Rect rect = new Rect();

			int xPadding = 1;
			int yPadding = 1;

			for (xPadding = 1; xPadding >= 0; --xPadding)
			{
				for (yPadding = 1; yPadding >= 0; --yPadding)
				{
					rect = bp.Insert(tex.width + (xPadding * padding), tex.height + (yPadding * padding), TexturePacker.FreeRectChoiceHeuristic.RectBestAreaFit);
					if (rect.width != 0 && rect.height != 0)
					{
						break;
					}
					else if (xPadding == 0 && yPadding == 0)
					{
						return PackTextures(texture, textures, width * (width <= height ? 2 : 1), height * (height < width ? 2 : 1), padding, maxSize);
					}
				}

				if (rect.width != 0 && rect.height != 0) 
					break;
			}

			storage[i] = new Storage();
			storage[i].rect = rect;
			storage[i].paddingX = (xPadding != 0);
			storage[i].paddingY = (yPadding != 0);
		}

		texture.Resize(width, height);
		texture.SetPixels(new Color[width * height]);

		Rect[] rects = new Rect[textures.Length];

		for (int i = 0; i < textures.Length; i++)
		{
			Texture2D tex = textures[i];
			Rect rect = storage[i].rect;
			int xPadding = (storage[i].paddingX ? padding : 0);
			int yPadding = (storage[i].paddingY ? padding : 0);
			Color[] colors = tex.GetPixels();

			if (rect.width != tex.width + xPadding)
			{
				Color[] newColors = tex.GetPixels();

				for (int x = 0; x < rect.width; x++)
				{
					for (int y = 0; y < rect.height; y++)
					{
						int prevIndex = ((int)rect.height - (y + 1)) + x * (int)tex.width;
						newColors[x + y * (int)rect.width] = colors[prevIndex];
					}
				}

				colors = newColors;
			}

			texture.SetPixels((int)rect.x, (int)rect.y, (int)rect.width - xPadding, (int)rect.height - yPadding, colors);
			rect.x /= width;
			rect.y /= height;
			rect.width = (rect.width - xPadding) / width;
			rect.height = (rect.height - yPadding) / height;
			rects[i] = rect;
		}
		return rects;
	}

	public Rect Insert(int width, int height, FreeRectChoiceHeuristic method)
	{
		Rect newNode = new Rect();
		int score1 = 0;
		int score2 = 0;
		switch (method)
		{
			case FreeRectChoiceHeuristic.RectBestShortSideFit:	newNode = FindPositionForNewNodeBestShortSideFit(width, height, ref score1, ref score2);	break;
			case FreeRectChoiceHeuristic.RectBottomLeftRule:	newNode = FindPositionForNewNodeBottomLeft(width, height, ref score1, ref score2);			break;
			case FreeRectChoiceHeuristic.RectContactPointRule:	newNode = FindPositionForNewNodeContactPoint(width, height, ref score1);					break;
			case FreeRectChoiceHeuristic.RectBestLongSideFit:	newNode = FindPositionForNewNodeBestLongSideFit(width, height, ref score2, ref score1);		break;
			case FreeRectChoiceHeuristic.RectBestAreaFit:		newNode = FindPositionForNewNodeBestAreaFit(width, height, ref score1, ref score2);			break;
		}

		if (newNode.height == 0)
			return newNode;

		int numRectanglesToProcess = m_FreeRectangleList.Count;
		for (int i = 0; i < numRectanglesToProcess; ++i)
		{
			if (SplitFreeNode(m_FreeRectangleList[i], ref newNode))
			{
				m_FreeRectangleList.RemoveAt(i);
				--i;
				--numRectanglesToProcess;
			}
		}

		PruneFreeList();

		m_UsedRectangleList.Add(newNode);
		return newNode;
	}

	public void Insert(List<Rect> rects, List<Rect> dst, FreeRectChoiceHeuristic method)
	{
		dst.Clear();

		while (rects.Count > 0)
		{
			int bestScore1 = int.MaxValue;
			int bestScore2 = int.MaxValue;
			int bestRectIndex = -1;
			Rect bestNode = new Rect();

			for (int i = 0; i < rects.Count; ++i)
			{
				int score1 = 0;
				int score2 = 0;
				Rect newNode = ScoreRect((int)rects[i].width, (int)rects[i].height, method, ref score1, ref score2);

				if (score1 < bestScore1 || (score1 == bestScore1 && score2 < bestScore2))
				{
					bestScore1 = score1;
					bestScore2 = score2;
					bestNode = newNode;
					bestRectIndex = i;
				}
			}

			if (bestRectIndex == -1)
				return;

			PlaceRect(bestNode);
			rects.RemoveAt(bestRectIndex);
		}
	}

	void PlaceRect(Rect node)
	{
		int numRectanglesToProcess = m_FreeRectangleList.Count;
		for (int i = 0; i < numRectanglesToProcess; ++i)
		{
			if (SplitFreeNode(m_FreeRectangleList[i], ref node))
			{
				m_FreeRectangleList.RemoveAt(i);
				--i;
				--numRectanglesToProcess;
			}
		}

		PruneFreeList();

		m_UsedRectangleList.Add(node);
	}

	Rect ScoreRect(int width, int height, FreeRectChoiceHeuristic method, ref int score1, ref int score2)
	{
		Rect newNode = new Rect();
		score1 = int.MaxValue;
		score2 = int.MaxValue;
		switch (method)
		{
			case FreeRectChoiceHeuristic.RectBestShortSideFit:	newNode = FindPositionForNewNodeBestShortSideFit(width, height, ref score1, ref score2);	break;
			case FreeRectChoiceHeuristic.RectBottomLeftRule:	newNode = FindPositionForNewNodeBottomLeft(width, height, ref score1, ref score2);			break;
			case FreeRectChoiceHeuristic.RectContactPointRule:	newNode = FindPositionForNewNodeContactPoint(width, height, ref score1);
				score1 = -score1;
				break;
			case FreeRectChoiceHeuristic.RectBestLongSideFit:	newNode = FindPositionForNewNodeBestLongSideFit(width, height, ref score2, ref score1);		break;
			case FreeRectChoiceHeuristic.RectBestAreaFit:		newNode = FindPositionForNewNodeBestAreaFit(width, height, ref score1, ref score2);			break;
		}

		if (newNode.height == 0)
		{
			score1 = int.MaxValue;
			score2 = int.MaxValue;
		}

		return newNode;
	}

	public float Occupancy()
	{
		ulong usedSurfaceArea = 0;
		for (int i = 0; i < m_UsedRectangleList.Count; ++i)
			usedSurfaceArea += (uint)m_UsedRectangleList[i].width * (uint)m_UsedRectangleList[i].height;

		return (float)usedSurfaceArea / (m_nWidth * m_nHeight);
	}

	Rect FindPositionForNewNodeBottomLeft(int width, int height, ref int bestY, ref int bestX)
	{
		Rect bestNode = new Rect();

		bestY = int.MaxValue;

		for (int i = 0; i < m_FreeRectangleList.Count; ++i)
		{
			if (m_FreeRectangleList[i].width >= width && m_FreeRectangleList[i].height >= height)
			{
				int topSideY = (int)m_FreeRectangleList[i].y + height;
				if (topSideY < bestY || (topSideY == bestY && m_FreeRectangleList[i].x < bestX))
				{
					bestNode.x = m_FreeRectangleList[i].x;
					bestNode.y = m_FreeRectangleList[i].y;
					bestNode.width = width;
					bestNode.height = height;
					bestY = topSideY;
					bestX = (int)m_FreeRectangleList[i].x;
				}
			}
			if (m_IsRotation && m_FreeRectangleList[i].width >= height && m_FreeRectangleList[i].height >= width)
			{
				int topSideY = (int)m_FreeRectangleList[i].y + width;
				if (topSideY < bestY || (topSideY == bestY && m_FreeRectangleList[i].x < bestX))
				{
					bestNode.x = m_FreeRectangleList[i].x;
					bestNode.y = m_FreeRectangleList[i].y;
					bestNode.width = height;
					bestNode.height = width;
					bestY = topSideY;
					bestX = (int)m_FreeRectangleList[i].x;
				}
			}
		}
		return bestNode;
	}

	Rect FindPositionForNewNodeBestShortSideFit(int width, int height, ref int bestShortSideFit, ref int bestLongSideFit)
	{
		Rect bestNode = new Rect();

		bestShortSideFit = int.MaxValue;

		for (int i = 0; i < m_FreeRectangleList.Count; ++i)
		{
			if (m_FreeRectangleList[i].width >= width && m_FreeRectangleList[i].height >= height)
			{
				int leftoverHoriz = Mathf.Abs((int)m_FreeRectangleList[i].width - width);
				int leftoverVert = Mathf.Abs((int)m_FreeRectangleList[i].height - height);
				int shortSideFit = Mathf.Min(leftoverHoriz, leftoverVert);
				int longSideFit = Mathf.Max(leftoverHoriz, leftoverVert);

				if (shortSideFit < bestShortSideFit || (shortSideFit == bestShortSideFit && longSideFit < bestLongSideFit))
				{
					bestNode.x = m_FreeRectangleList[i].x;
					bestNode.y = m_FreeRectangleList[i].y;
					bestNode.width = width;
					bestNode.height = height;
					bestShortSideFit = shortSideFit;
					bestLongSideFit = longSideFit;
				}
			}

			if (m_IsRotation && m_FreeRectangleList[i].width >= height && m_FreeRectangleList[i].height >= width)
			{
				int flippedLeftoverHoriz = Mathf.Abs((int)m_FreeRectangleList[i].width - height);
				int flippedLeftoverVert = Mathf.Abs((int)m_FreeRectangleList[i].height - width);
				int flippedShortSideFit = Mathf.Min(flippedLeftoverHoriz, flippedLeftoverVert);
				int flippedLongSideFit = Mathf.Max(flippedLeftoverHoriz, flippedLeftoverVert);

				if (flippedShortSideFit < bestShortSideFit || (flippedShortSideFit == bestShortSideFit && flippedLongSideFit < bestLongSideFit))
				{
					bestNode.x = m_FreeRectangleList[i].x;
					bestNode.y = m_FreeRectangleList[i].y;
					bestNode.width = height;
					bestNode.height = width;
					bestShortSideFit = flippedShortSideFit;
					bestLongSideFit = flippedLongSideFit;
				}
			}
		}
		return bestNode;
	}

	Rect FindPositionForNewNodeBestLongSideFit(int width, int height, ref int bestShortSideFit, ref int bestLongSideFit)
	{
		Rect bestNode = new Rect();

		bestLongSideFit = int.MaxValue;

		for (int i = 0; i < m_FreeRectangleList.Count; ++i)
		{
			if (m_FreeRectangleList[i].width >= width && m_FreeRectangleList[i].height >= height)
			{
				int leftoverHoriz = Mathf.Abs((int)m_FreeRectangleList[i].width - width);
				int leftoverVert = Mathf.Abs((int)m_FreeRectangleList[i].height - height);
				int shortSideFit = Mathf.Min(leftoverHoriz, leftoverVert);
				int longSideFit = Mathf.Max(leftoverHoriz, leftoverVert);

				if (longSideFit < bestLongSideFit || (longSideFit == bestLongSideFit && shortSideFit < bestShortSideFit))
				{
					bestNode.x = m_FreeRectangleList[i].x;
					bestNode.y = m_FreeRectangleList[i].y;
					bestNode.width = width;
					bestNode.height = height;
					bestShortSideFit = shortSideFit;
					bestLongSideFit = longSideFit;
				}
			}

			if (m_IsRotation && m_FreeRectangleList[i].width >= height && m_FreeRectangleList[i].height >= width)
			{
				int leftoverHoriz = Mathf.Abs((int)m_FreeRectangleList[i].width - height);
				int leftoverVert = Mathf.Abs((int)m_FreeRectangleList[i].height - width);
				int shortSideFit = Mathf.Min(leftoverHoriz, leftoverVert);
				int longSideFit = Mathf.Max(leftoverHoriz, leftoverVert);

				if (longSideFit < bestLongSideFit || (longSideFit == bestLongSideFit && shortSideFit < bestShortSideFit))
				{
					bestNode.x = m_FreeRectangleList[i].x;
					bestNode.y = m_FreeRectangleList[i].y;
					bestNode.width = height;
					bestNode.height = width;
					bestShortSideFit = shortSideFit;
					bestLongSideFit = longSideFit;
				}
			}
		}
		return bestNode;
	}

	Rect FindPositionForNewNodeBestAreaFit(int width, int height, ref int bestAreaFit, ref int bestShortSideFit)
	{
		Rect bestNode = new Rect();

		bestAreaFit = int.MaxValue;

		for (int i = 0; i < m_FreeRectangleList.Count; ++i)
		{
			int areaFit = (int)m_FreeRectangleList[i].width * (int)m_FreeRectangleList[i].height - width * height;

			if (m_FreeRectangleList[i].width >= width && m_FreeRectangleList[i].height >= height)
			{
				int leftoverHoriz = Mathf.Abs((int)m_FreeRectangleList[i].width - width);
				int leftoverVert = Mathf.Abs((int)m_FreeRectangleList[i].height - height);
				int shortSideFit = Mathf.Min(leftoverHoriz, leftoverVert);

				if (areaFit < bestAreaFit || (areaFit == bestAreaFit && shortSideFit < bestShortSideFit))
				{
					bestNode.x = m_FreeRectangleList[i].x;
					bestNode.y = m_FreeRectangleList[i].y;
					bestNode.width = width;
					bestNode.height = height;
					bestShortSideFit = shortSideFit;
					bestAreaFit = areaFit;
				}
			}

			if (m_IsRotation && m_FreeRectangleList[i].width >= height && m_FreeRectangleList[i].height >= width)
			{
				int leftoverHoriz = Mathf.Abs((int)m_FreeRectangleList[i].width - height);
				int leftoverVert = Mathf.Abs((int)m_FreeRectangleList[i].height - width);
				int shortSideFit = Mathf.Min(leftoverHoriz, leftoverVert);

				if (areaFit < bestAreaFit || (areaFit == bestAreaFit && shortSideFit < bestShortSideFit))
				{
					bestNode.x = m_FreeRectangleList[i].x;
					bestNode.y = m_FreeRectangleList[i].y;
					bestNode.width = height;
					bestNode.height = width;
					bestShortSideFit = shortSideFit;
					bestAreaFit = areaFit;
				}
			}
		}
		return bestNode;
	}

	int CommonIntervalLength(int i1start, int i1end, int i2start, int i2end)
	{
		if (i1end < i2start || i2end < i1start)
			return 0;
		return Mathf.Min(i1end, i2end) - Mathf.Max(i1start, i2start);
	}

	int ContactPointScoreNode(int x, int y, int width, int height)
	{
		int score = 0;

		if (x == 0 || x + width == m_nWidth)
			score += height;
		if (y == 0 || y + height == m_nHeight)
			score += width;

		for (int i = 0; i < m_UsedRectangleList.Count; ++i)
		{
			if (m_UsedRectangleList[i].x == x + width || m_UsedRectangleList[i].x + m_UsedRectangleList[i].width == x)
				score += CommonIntervalLength((int)m_UsedRectangleList[i].y, (int)m_UsedRectangleList[i].y + (int)m_UsedRectangleList[i].height, y, y + height);
			if (m_UsedRectangleList[i].y == y + height || m_UsedRectangleList[i].y + m_UsedRectangleList[i].height == y)
				score += CommonIntervalLength((int)m_UsedRectangleList[i].x, (int)m_UsedRectangleList[i].x + (int)m_UsedRectangleList[i].width, x, x + width);
		}
		return score;
	}

	Rect FindPositionForNewNodeContactPoint(int width, int height, ref int bestContactScore)
	{
		Rect bestNode = new Rect();

		bestContactScore = -1;

		for (int i = 0; i < m_FreeRectangleList.Count; ++i)
		{
			if (m_FreeRectangleList[i].width >= width && m_FreeRectangleList[i].height >= height)
			{
				int score = ContactPointScoreNode((int)m_FreeRectangleList[i].x, (int)m_FreeRectangleList[i].y, width, height);
				if (score > bestContactScore)
				{
					bestNode.x = (int)m_FreeRectangleList[i].x;
					bestNode.y = (int)m_FreeRectangleList[i].y;
					bestNode.width = width;
					bestNode.height = height;
					bestContactScore = score;
				}
			}
			if (m_IsRotation && m_FreeRectangleList[i].width >= height && m_FreeRectangleList[i].height >= width)
			{
				int score = ContactPointScoreNode((int)m_FreeRectangleList[i].x, (int)m_FreeRectangleList[i].y, height, width);
				if (score > bestContactScore)
				{
					bestNode.x = (int)m_FreeRectangleList[i].x;
					bestNode.y = (int)m_FreeRectangleList[i].y;
					bestNode.width = height;
					bestNode.height = width;
					bestContactScore = score;
				}
			}
		}
		return bestNode;
	}

	bool SplitFreeNode(Rect freeNode, ref Rect usedNode)
	{
		if (usedNode.x >= freeNode.x + freeNode.width || usedNode.x + usedNode.width <= freeNode.x ||
			usedNode.y >= freeNode.y + freeNode.height || usedNode.y + usedNode.height <= freeNode.y)
			return false;

		if (usedNode.x < freeNode.x + freeNode.width && usedNode.x + usedNode.width > freeNode.x)
		{
			if (usedNode.y > freeNode.y && usedNode.y < freeNode.y + freeNode.height)
			{
				Rect newNode = freeNode;
				newNode.height = usedNode.y - newNode.y;
				m_FreeRectangleList.Add(newNode);
			}

			if (usedNode.y + usedNode.height < freeNode.y + freeNode.height)
			{
				Rect newNode = freeNode;
				newNode.y = usedNode.y + usedNode.height;
				newNode.height = freeNode.y + freeNode.height - (usedNode.y + usedNode.height);
				m_FreeRectangleList.Add(newNode);
			}
		}

		if (usedNode.y < freeNode.y + freeNode.height && usedNode.y + usedNode.height > freeNode.y)
		{
			if (usedNode.x > freeNode.x && usedNode.x < freeNode.x + freeNode.width)
			{
				Rect newNode = freeNode;
				newNode.width = usedNode.x - newNode.x;
				m_FreeRectangleList.Add(newNode);
			}

			if (usedNode.x + usedNode.width < freeNode.x + freeNode.width)
			{
				Rect newNode = freeNode;
				newNode.x = usedNode.x + usedNode.width;
				newNode.width = freeNode.x + freeNode.width - (usedNode.x + usedNode.width);
				m_FreeRectangleList.Add(newNode);
			}
		}

		return true;
	}

	void PruneFreeList()
	{
		for (int i = 0; i < m_FreeRectangleList.Count; ++i)
			for (int j = i + 1; j < m_FreeRectangleList.Count; ++j)
			{
				if (IsContainedIn(m_FreeRectangleList[i], m_FreeRectangleList[j]))
				{
					m_FreeRectangleList.RemoveAt(i);
					--i;
					break;
				}
				if (IsContainedIn(m_FreeRectangleList[j], m_FreeRectangleList[i]))
				{
					m_FreeRectangleList.RemoveAt(j);
					--j;
				}
			}
	}

	bool IsContainedIn(Rect a, Rect b)
	{
		return a.x >= b.x && a.y >= b.y
			&& a.x + a.width <= b.x + b.width
			&& a.y + a.height <= b.y + b.height;
	}
}