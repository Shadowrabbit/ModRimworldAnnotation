using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200001F RID: 31
	public static class Vector2Utility
	{
		// Token: 0x06000170 RID: 368 RVA: 0x00008008 File Offset: 0x00006208
		public static Vector2 Rotated(this Vector2 v)
		{
			return new Vector2(v.y, v.x);
		}

		// Token: 0x06000171 RID: 369 RVA: 0x0000801B File Offset: 0x0000621B
		public static Vector2 RotatedBy(this Vector2 v, Rot4 rot)
		{
			return v.RotatedBy(rot.AsAngle);
		}

		// Token: 0x06000172 RID: 370 RVA: 0x0007CB5C File Offset: 0x0007AD5C
		public static Vector2 RotatedBy(this Vector2 v, float degrees)
		{
			float num = Mathf.Sin(degrees * 0.017453292f);
			float num2 = Mathf.Cos(degrees * 0.017453292f);
			return new Vector2(num2 * v.x - num * v.y, num * v.x + num2 * v.y);
		}

		// Token: 0x06000173 RID: 371 RVA: 0x0000802A File Offset: 0x0000622A
		public static float AngleTo(this Vector2 a, Vector2 b)
		{
			return Mathf.Atan2(-(b.y - a.y), b.x - a.x) * 57.29578f;
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00008052 File Offset: 0x00006252
		public static Vector2 Moved(this Vector2 v, float angle, float distance)
		{
			return new Vector2(v.x + Mathf.Cos(angle * 0.017453292f) * distance, v.y - Mathf.Sin(angle * 0.017453292f) * distance);
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00008083 File Offset: 0x00006283
		public static Vector2 FromAngle(float angle)
		{
			return new Vector2(Mathf.Cos(angle * 0.017453292f), -Mathf.Sin(angle * 0.017453292f));
		}

		// Token: 0x06000176 RID: 374 RVA: 0x000080A3 File Offset: 0x000062A3
		public static float ToAngle(this Vector2 v)
		{
			return Mathf.Atan2(-v.y, v.x) * 57.29578f;
		}

		// Token: 0x06000177 RID: 375 RVA: 0x000080BD File Offset: 0x000062BD
		public static float Cross(this Vector2 u, Vector2 v)
		{
			return u.x * v.y - u.y * v.x;
		}

		// Token: 0x06000178 RID: 376 RVA: 0x0007CBAC File Offset: 0x0007ADAC
		public static float DistanceToRect(this Vector2 u, Rect rect)
		{
			if (rect.Contains(u))
			{
				return 0f;
			}
			if (u.x < rect.xMin && u.y < rect.yMin)
			{
				return Vector2.Distance(u, new Vector2(rect.xMin, rect.yMin));
			}
			if (u.x > rect.xMax && u.y < rect.yMin)
			{
				return Vector2.Distance(u, new Vector2(rect.xMax, rect.yMin));
			}
			if (u.x < rect.xMin && u.y > rect.yMax)
			{
				return Vector2.Distance(u, new Vector2(rect.xMin, rect.yMax));
			}
			if (u.x > rect.xMax && u.y > rect.yMax)
			{
				return Vector2.Distance(u, new Vector2(rect.xMax, rect.yMax));
			}
			if (u.x < rect.xMin)
			{
				return rect.xMin - u.x;
			}
			if (u.x > rect.xMax)
			{
				return u.x - rect.xMax;
			}
			if (u.y < rect.yMin)
			{
				return rect.yMin - u.y;
			}
			return u.y - rect.yMax;
		}
	}
}
