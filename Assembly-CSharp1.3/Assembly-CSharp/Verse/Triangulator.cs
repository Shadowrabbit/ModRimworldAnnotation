﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004F1 RID: 1265
	public class Triangulator
	{
		// Token: 0x06002636 RID: 9782 RVA: 0x000ED879 File Offset: 0x000EBA79
		public Triangulator(Vector2[] points)
		{
			this.m_points = new List<Vector2>(points);
		}

		// Token: 0x06002637 RID: 9783 RVA: 0x000ED898 File Offset: 0x000EBA98
		public int[] Triangulate()
		{
			List<int> list = new List<int>();
			int count = this.m_points.Count;
			if (count < 3)
			{
				return list.ToArray();
			}
			int[] array = new int[count];
			if (this.Area() > 0f)
			{
				for (int i = 0; i < count; i++)
				{
					array[i] = i;
				}
			}
			else
			{
				for (int j = 0; j < count; j++)
				{
					array[j] = count - 1 - j;
				}
			}
			int k = count;
			int num = 2 * k;
			int num2 = 0;
			int num3 = k - 1;
			while (k > 2)
			{
				if (num-- <= 0)
				{
					return list.ToArray();
				}
				int num4 = num3;
				if (k <= num4)
				{
					num4 = 0;
				}
				num3 = num4 + 1;
				if (k <= num3)
				{
					num3 = 0;
				}
				int num5 = num3 + 1;
				if (k <= num5)
				{
					num5 = 0;
				}
				if (this.Snip(num4, num3, num5, k, array))
				{
					int item = array[num4];
					int item2 = array[num3];
					int item3 = array[num5];
					list.Add(item);
					list.Add(item2);
					list.Add(item3);
					num2++;
					int num6 = num3;
					for (int l = num3 + 1; l < k; l++)
					{
						array[num6] = array[l];
						num6++;
					}
					k--;
					num = 2 * k;
				}
			}
			list.Reverse();
			return list.ToArray();
		}

		// Token: 0x06002638 RID: 9784 RVA: 0x000ED9D8 File Offset: 0x000EBBD8
		private float Area()
		{
			int count = this.m_points.Count;
			float num = 0f;
			int index = count - 1;
			int i = 0;
			while (i < count)
			{
				Vector2 vector = this.m_points[index];
				Vector2 vector2 = this.m_points[i];
				num += vector.x * vector2.y - vector2.x * vector.y;
				index = i++;
			}
			return num * 0.5f;
		}

		// Token: 0x06002639 RID: 9785 RVA: 0x000EDA50 File Offset: 0x000EBC50
		private bool Snip(int u, int v, int w, int n, int[] V)
		{
			Vector2 vector = this.m_points[V[u]];
			Vector2 vector2 = this.m_points[V[v]];
			Vector2 vector3 = this.m_points[V[w]];
			if (Mathf.Epsilon > (vector2.x - vector.x) * (vector3.y - vector.y) - (vector2.y - vector.y) * (vector3.x - vector.x))
			{
				return false;
			}
			for (int i = 0; i < n; i++)
			{
				if (i != u && i != v && i != w)
				{
					Vector2 p = this.m_points[V[i]];
					if (this.InsideTriangle(vector, vector2, vector3, p))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600263A RID: 9786 RVA: 0x000EDB08 File Offset: 0x000EBD08
		private bool InsideTriangle(Vector2 A, Vector2 B, Vector2 C, Vector2 P)
		{
			float num = C.x - B.x;
			float num2 = C.y - B.y;
			float num3 = A.x - C.x;
			float num4 = A.y - C.y;
			float num5 = B.x - A.x;
			float num6 = B.y - A.y;
			float num7 = P.x - A.x;
			float num8 = P.y - A.y;
			float num9 = P.x - B.x;
			float num10 = P.y - B.y;
			float num11 = P.x - C.x;
			float num12 = P.y - C.y;
			float num13 = num * num10 - num2 * num9;
			float num14 = num5 * num8 - num6 * num7;
			float num15 = num3 * num12 - num4 * num11;
			return num13 >= 0f && num15 >= 0f && num14 >= 0f;
		}

		// Token: 0x0400180C RID: 6156
		private List<Vector2> m_points = new List<Vector2>();
	}
}
