﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CFF RID: 3327
	public static class WalkPathFinder
	{
		// Token: 0x06004C6C RID: 19564 RVA: 0x001A9DFC File Offset: 0x001A7FFC
		public static bool TryFindWalkPath(Pawn pawn, IntVec3 root, out List<IntVec3> result)
		{
			List<IntVec3> list = new List<IntVec3>();
			list.Add(root);
			IntVec3 intVec = root;
			for (int i = 0; i < 8; i++)
			{
				IntVec3 intVec2 = IntVec3.Invalid;
				float num = -1f;
				for (int j = WalkPathFinder.StartRadialIndex; j > WalkPathFinder.EndRadialIndex; j -= WalkPathFinder.RadialIndexStride)
				{
					IntVec3 intVec3 = intVec + GenRadial.RadialPattern[j];
					if (intVec3.InBounds(pawn.Map) && intVec3.Standable(pawn.Map) && !intVec3.IsForbidden(pawn) && !intVec3.GetTerrain(pawn.Map).avoidWander && GenSight.LineOfSight(intVec, intVec3, pawn.Map, false, null, 0, 0) && !intVec3.Roofed(pawn.Map) && !PawnUtility.KnownDangerAt(intVec3, pawn.Map, pawn))
					{
						float num2 = 10000f;
						for (int k = 0; k < list.Count; k++)
						{
							num2 += (float)(list[k] - intVec3).LengthManhattan;
						}
						float num3 = (float)(intVec3 - root).LengthManhattan;
						if (num3 > 40f)
						{
							num2 *= Mathf.InverseLerp(70f, 40f, num3);
						}
						if (list.Count >= 2)
						{
							float num4 = (list[list.Count - 1] - list[list.Count - 2]).AngleFlat;
							float angleFlat = (intVec3 - intVec).AngleFlat;
							float num5;
							if (angleFlat > num4)
							{
								num5 = angleFlat - num4;
							}
							else
							{
								num4 -= 360f;
								num5 = angleFlat - num4;
							}
							if (num5 > 110f)
							{
								num2 *= 0.01f;
							}
						}
						if (list.Count >= 4 && (intVec - root).LengthManhattan < (intVec3 - root).LengthManhattan)
						{
							num2 *= 1E-05f;
						}
						if (num2 > num)
						{
							intVec2 = intVec3;
							num = num2;
						}
					}
				}
				if (num < 0f)
				{
					result = null;
					return false;
				}
				list.Add(intVec2);
				intVec = intVec2;
			}
			list.Add(root);
			result = list;
			return true;
		}

		// Token: 0x06004C6D RID: 19565 RVA: 0x001AA040 File Offset: 0x001A8240
		public static void DebugFlashWalkPath(IntVec3 root, int numEntries = 8)
		{
			Map currentMap = Find.CurrentMap;
			List<IntVec3> list;
			if (!WalkPathFinder.TryFindWalkPath(currentMap.mapPawns.FreeColonistsSpawned.First<Pawn>(), root, out list))
			{
				currentMap.debugDrawer.FlashCell(root, 0.2f, "NOPATH", 50);
				return;
			}
			for (int i = 0; i < list.Count; i++)
			{
				currentMap.debugDrawer.FlashCell(list[i], (float)i / (float)numEntries, i.ToString(), 50);
				if (i > 0)
				{
					currentMap.debugDrawer.FlashLine(list[i], list[i - 1], 50, SimpleColor.White);
				}
			}
		}

		// Token: 0x0400324E RID: 12878
		private const int NumPathNodes = 8;

		// Token: 0x0400324F RID: 12879
		private const float StepDistMin = 2f;

		// Token: 0x04003250 RID: 12880
		private const float StepDistMax = 14f;

		// Token: 0x04003251 RID: 12881
		private static readonly int StartRadialIndex = GenRadial.NumCellsInRadius(14f);

		// Token: 0x04003252 RID: 12882
		private static readonly int EndRadialIndex = GenRadial.NumCellsInRadius(2f);

		// Token: 0x04003253 RID: 12883
		private static readonly int RadialIndexStride = 3;
	}
}
