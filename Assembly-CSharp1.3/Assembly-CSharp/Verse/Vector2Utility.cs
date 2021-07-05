using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200001B RID: 27
	public static class Vector2Utility
	{
		// Token: 0x06000142 RID: 322 RVA: 0x00006E16 File Offset: 0x00005016
		public static Vector2 Rotated(this Vector2 v)
		{
			return new Vector2(v.y, v.x);
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00006E29 File Offset: 0x00005029
		public static Vector2 RotatedBy(this Vector2 v, Rot4 rot)
		{
			return v.RotatedBy(rot.AsAngle);
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00006E38 File Offset: 0x00005038
		public static Vector2 RotatedBy(this Vector2 v, float degrees)
		{
			float num = Mathf.Sin(degrees * 0.017453292f);
			float num2 = Mathf.Cos(degrees * 0.017453292f);
			return new Vector2(num2 * v.x - num * v.y, num * v.x + num2 * v.y);
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00006E86 File Offset: 0x00005086
		public static float AngleTo(this Vector2 a, Vector2 b)
		{
			return Mathf.Atan2(-(b.y - a.y), b.x - a.x) * 57.29578f;
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00006EAE File Offset: 0x000050AE
		public static Vector2 Moved(this Vector2 v, float angle, float distance)
		{
			return new Vector2(v.x + Mathf.Cos(angle * 0.017453292f) * distance, v.y - Mathf.Sin(angle * 0.017453292f) * distance);
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00006EDF File Offset: 0x000050DF
		public static Vector2 FromAngle(float angle)
		{
			return new Vector2(Mathf.Cos(angle * 0.017453292f), -Mathf.Sin(angle * 0.017453292f));
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00006EFF File Offset: 0x000050FF
		public static float ToAngle(this Vector2 v)
		{
			return Mathf.Atan2(-v.y, v.x) * 57.29578f;
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00006F19 File Offset: 0x00005119
		public static float Cross(this Vector2 u, Vector2 v)
		{
			return u.x * v.y - u.y * v.x;
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00006F38 File Offset: 0x00005138
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
