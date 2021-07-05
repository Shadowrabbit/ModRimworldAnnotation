using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000019 RID: 25
	public static class IntVec3Utility
	{
		// Token: 0x06000130 RID: 304 RVA: 0x00006A96 File Offset: 0x00004C96
		public static IntVec3 ToIntVec3(this Vector3 vect)
		{
			return new IntVec3(vect);
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00006AA0 File Offset: 0x00004CA0
		public static float DistanceTo(this IntVec3 a, IntVec3 b)
		{
			return (a - b).LengthHorizontal;
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00006ABC File Offset: 0x00004CBC
		public static int DistanceToSquared(this IntVec3 a, IntVec3 b)
		{
			return (a - b).LengthHorizontalSquared;
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00006AD8 File Offset: 0x00004CD8
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

		// Token: 0x06000134 RID: 308 RVA: 0x00006B54 File Offset: 0x00004D54
		public static int ManhattanDistanceFlat(IntVec3 a, IntVec3 b)
		{
			return Math.Abs(a.x - b.x) + Math.Abs(a.z - b.z);
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00006B7B File Offset: 0x00004D7B
		public static IntVec3 RandomHorizontalOffset(float maxDist)
		{
			return Vector3Utility.RandomHorizontalOffset(maxDist).ToIntVec3();
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00006B88 File Offset: 0x00004D88
		public static int DistanceToEdge(this IntVec3 v, Map map)
		{
			return Mathf.Max(Mathf.Min(Mathf.Min(Mathf.Min(v.x, v.z), map.Size.x - v.x - 1), map.Size.z - v.z - 1), 0);
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00006BE0 File Offset: 0x00004DE0
		public static int Determinant(this IntVec3 p, IntVec3 p1, IntVec3 p2)
		{
			int num = (p2.x - p1.x) * (p.z - p1.z) - (p.x - p1.x) * (p2.z - p1.z);
			if (num > 0)
			{
				return -1;
			}
			if (num < 0)
			{
				return 1;
			}
			return 0;
		}
	}
}
