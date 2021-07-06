using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200001E RID: 30
	public static class Vector3Utility
	{
		// Token: 0x06000166 RID: 358 RVA: 0x00007F5D File Offset: 0x0000615D
		public static Vector3 HorizontalVectorFromAngle(float angle)
		{
			return Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward;
		}

		// Token: 0x06000167 RID: 359 RVA: 0x0007CA28 File Offset: 0x0007AC28
		public static float AngleFlat(this Vector3 v)
		{
			if (v.x == 0f && v.z == 0f)
			{
				return 0f;
			}
			return Quaternion.LookRotation(v).eulerAngles.y;
		}

		// Token: 0x06000168 RID: 360 RVA: 0x0007CA68 File Offset: 0x0007AC68
		public static Vector3 RandomHorizontalOffset(float maxDist)
		{
			float d = Rand.Range(0f, maxDist);
			float y = (float)Rand.Range(0, 360);
			return Quaternion.Euler(new Vector3(0f, y, 0f)) * Vector3.forward * d;
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00007F74 File Offset: 0x00006174
		public static Vector3 Yto0(this Vector3 v3)
		{
			return new Vector3(v3.x, 0f, v3.z);
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00007F8C File Offset: 0x0000618C
		public static Vector3 RotatedBy(this Vector3 v3, float angle)
		{
			return Quaternion.AngleAxis(angle, Vector3.up) * v3;
		}

		// Token: 0x0600016B RID: 363 RVA: 0x0007CAB4 File Offset: 0x0007ACB4
		public static Vector3 RotatedBy(this Vector3 orig, Rot4 rot)
		{
			switch (rot.AsInt)
			{
			case 0:
				return orig;
			case 1:
				return new Vector3(orig.z, orig.y, -orig.x);
			case 2:
				return new Vector3(-orig.x, orig.y, -orig.z);
			case 3:
				return new Vector3(-orig.z, orig.y, orig.x);
			default:
				return orig;
			}
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00007F9F File Offset: 0x0000619F
		public static float AngleToFlat(this Vector3 a, Vector3 b)
		{
			return new Vector2(a.x, a.z).AngleTo(new Vector2(b.x, b.z));
		}

		// Token: 0x0600016D RID: 365 RVA: 0x0007CB30 File Offset: 0x0007AD30
		public static Vector3 FromAngleFlat(float angle)
		{
			Vector2 vector = Vector2Utility.FromAngle(angle);
			return new Vector3(vector.x, 0f, vector.y);
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00007FC8 File Offset: 0x000061C8
		public static float ToAngleFlat(this Vector3 v)
		{
			return new Vector2(v.x, v.z).ToAngle();
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00007FE0 File Offset: 0x000061E0
		public static Vector3 Abs(this Vector3 v)
		{
			return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
		}
	}
}
