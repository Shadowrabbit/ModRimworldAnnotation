using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200001D RID: 29
	public static class IntVec3Utility
	{
		// Token: 0x0600015F RID: 351 RVA: 0x00007F21 File Offset: 0x00006121
		public static IntVec3 ToIntVec3(this Vector3 vect)
		{
			return new IntVec3(vect);
		}

		// Token: 0x06000160 RID: 352 RVA: 0x0007C91C File Offset: 0x0007AB1C
		public static float DistanceTo(this IntVec3 a, IntVec3 b)
		{
			return (a - b).LengthHorizontal;
		}

		// Token: 0x06000161 RID: 353 RVA: 0x0007C938 File Offset: 0x0007AB38
		public static int DistanceToSquared(this IntVec3 a, IntVec3 b)
		{
			return (a - b).LengthHorizontalSquared;
		}

		// Token: 0x06000162 RID: 354 RVA: 0x0007C954 File Offset: 0x0007AB54
		public static IntVec3 RotatedBy(this IntVec3 orig, Rot4 rot)
		{
			switch (rot.AsInt)
			{
			case 0:
				return orig;
			case 1:
				return new IntVec3(orig.z, orig.y, -orig.x);
			case 2:
				return new IntVec3(-orig.x, orig.y, -orig.z);
			case 3:
				return new IntVec3(-orig.z, orig.y, orig.x);
			default:
				return orig;
			}
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00007F29 File Offset: 0x00006129
		public static int ManhattanDistanceFlat(IntVec3 a, IntVec3 b)
		{
			return Math.Abs(a.x - b.x) + Math.Abs(a.z - b.z);
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00007F50 File Offset: 0x00006150
		public static IntVec3 RandomHorizontalOffset(float maxDist)
		{
			return Vector3Utility.RandomHorizontalOffset(maxDist).ToIntVec3();
		}

		// Token: 0x06000165 RID: 357 RVA: 0x0007C9D0 File Offset: 0x0007ABD0
		public static int DistanceToEdge(this IntVec3 v, Map map)
		{
			return Mathf.Max(Mathf.Min(Mathf.Min(Mathf.Min(v.x, v.z), map.Size.x - v.x - 1), map.Size.z - v.z - 1), 0);
		}
	}
}
