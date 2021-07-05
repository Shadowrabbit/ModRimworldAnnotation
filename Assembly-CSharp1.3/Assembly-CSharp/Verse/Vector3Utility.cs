using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200001A RID: 26
	public static class Vector3Utility
	{
		// Token: 0x06000138 RID: 312 RVA: 0x00006C32 File Offset: 0x00004E32
		public static Vector3 HorizontalVectorFromAngle(float angle)
		{
			return Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward;
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00006C4C File Offset: 0x00004E4C
		public static float AngleFlat(this Vector3 v)
		{
			if (v.x == 0f && v.z == 0f)
			{
				return 0f;
			}
			return Quaternion.LookRotation(v).eulerAngles.y;
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00006C8C File Offset: 0x00004E8C
		public static Vector3 RandomHorizontalOffset(float maxDist)
		{
			float d = Rand.Range(0f, maxDist);
			float y = (float)Rand.Range(0, 360);
			return Quaternion.Euler(new Vector3(0f, y, 0f)) * Vector3.forward * d;
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00006CD7 File Offset: 0x00004ED7
		public static Vector3 Yto0(this Vector3 v3)
		{
			return new Vector3(v3.x, 0f, v3.z);
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00006CEF File Offset: 0x00004EEF
		public static Vector3 RotatedBy(this Vector3 v3, float angle)
		{
			return Quaternion.AngleAxis(angle, Vector3.up) * v3;
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00006D04 File Offset: 0x00004F04
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

		// Token: 0x0600013E RID: 318 RVA: 0x00006D80 File Offset: 0x00004F80
		public static float AngleToFlat(this Vector3 a, Vector3 b)
		{
			return new Vector2(a.x, a.z).AngleTo(new Vector2(b.x, b.z));
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00006DAC File Offset: 0x00004FAC
		public static Vector3 FromAngleFlat(float angle)
		{
			Vector2 vector = Vector2Utility.FromAngle(angle);
			return new Vector3(vector.x, 0f, vector.y);
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00006DD6 File Offset: 0x00004FD6
		public static float ToAngleFlat(this Vector3 v)
		{
			return new Vector2(v.x, v.z).ToAngle();
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00006DEE File Offset: 0x00004FEE
		public static Vector3 Abs(this Vector3 v)
		{
			return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
		}
	}
}
