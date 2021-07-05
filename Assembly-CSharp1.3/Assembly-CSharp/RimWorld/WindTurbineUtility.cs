using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CD0 RID: 3280
	public static class WindTurbineUtility
	{
		// Token: 0x06004C82 RID: 19586 RVA: 0x00197F7A File Offset: 0x0019617A
		public static IEnumerable<IntVec3> CalculateWindCells(IntVec3 center, Rot4 rot, IntVec2 size)
		{
			CellRect rectA = default(CellRect);
			CellRect rectB = default(CellRect);
			int num = 0;
			int num2;
			int num3;
			if (rot == Rot4.North || rot == Rot4.East)
			{
				num2 = 9;
				num3 = 5;
			}
			else
			{
				num2 = 5;
				num3 = 9;
				num = -1;
			}
			if (rot.IsHorizontal)
			{
				rectA.minX = center.x + 2 + num;
				rectA.maxX = center.x + 2 + num2 + num;
				rectB.minX = center.x - 1 - num3 + num;
				rectB.maxX = center.x - 1 + num;
				rectB.minZ = (rectA.minZ = center.z - 3);
				rectB.maxZ = (rectA.maxZ = center.z + 3);
			}
			else
			{
				rectA.minZ = center.z + 2 + num;
				rectA.maxZ = center.z + 2 + num2 + num;
				rectB.minZ = center.z - 1 - num3 + num;
				rectB.maxZ = center.z - 1 + num;
				rectB.minX = (rectA.minX = center.x - 3);
				rectB.maxX = (rectA.maxX = center.x + 3);
			}
			int num4;
			for (int z = rectA.minZ; z <= rectA.maxZ; z = num4 + 1)
			{
				for (int x = rectA.minX; x <= rectA.maxX; x = num4 + 1)
				{
					yield return new IntVec3(x, 0, z);
					num4 = x;
				}
				num4 = z;
			}
			for (int z = rectB.minZ; z <= rectB.maxZ; z = num4 + 1)
			{
				for (int x = rectB.minX; x <= rectB.maxX; x = num4 + 1)
				{
					yield return new IntVec3(x, 0, z);
					num4 = x;
				}
				num4 = z;
			}
			yield break;
		}
	}
}
