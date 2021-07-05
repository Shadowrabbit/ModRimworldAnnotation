using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x02001591 RID: 5521
	public class SketchResolver_MechClusterWalls : SketchResolver
	{
		// Token: 0x06008264 RID: 33380 RVA: 0x002E33FC File Offset: 0x002E15FC
		protected override void ResolveInt(ResolveParams parms)
		{
			IntVec2 value = parms.mechClusterSize.Value;
			int num = GenMath.RoundRandom((float)GenMath.RoundRandom(SketchResolver_MechClusterWalls.WidthToMaxWallsCountCurve.Evaluate((float)Mathf.Min(value.x, value.z))) * SketchResolver_MechClusterWalls.WallCountRandomFactorRange.RandomInRange);
			num = Math.Max(1, num);
			for (int i = 0; i < num; i++)
			{
				this.TryAddWall(parms.sketch, value);
			}
			if (Rand.Bool)
			{
				ResolveParams parms2 = parms;
				parms2.symmetryVertical = new bool?(false);
				parms2.symmetryOrigin = new int?(value.x / 2);
				parms2.symmetryOriginIncluded = new bool?(value.x % 2 == 1);
				SketchResolverDefOf.Symmetry.Resolve(parms2);
				return;
			}
			if (Rand.Bool)
			{
				ResolveParams parms3 = parms;
				parms3.symmetryVertical = new bool?(true);
				parms3.symmetryOrigin = new int?(value.z / 2);
				parms3.symmetryOriginIncluded = new bool?(value.z % 2 == 1);
				SketchResolverDefOf.Symmetry.Resolve(parms3);
			}
		}

		// Token: 0x06008265 RID: 33381 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool CanResolveInt(ResolveParams parms)
		{
			return true;
		}

		// Token: 0x06008266 RID: 33382 RVA: 0x002E350C File Offset: 0x002E170C
		private void TryAddWall(Sketch sketch, IntVec2 size)
		{
			SketchResolver_MechClusterWalls.<>c__DisplayClass9_0 CS$<>8__locals1;
			CS$<>8__locals1.sketch = sketch;
			for (int i = 0; i < 50; i++)
			{
				if (Rand.Chance(0.8f))
				{
					bool @bool = Rand.Bool;
					int num = @bool ? size.x : size.z;
					CellRect rect;
					if (@bool)
					{
						IntVec2 intVec = new IntVec2(1, Rand.Bool ? (size.z - 1) : 0);
						rect = new CellRect(intVec.x, intVec.z, num - 1, 1);
					}
					else
					{
						IntVec2 intVec2 = new IntVec2(Rand.Bool ? (size.x - 1) : 0, 0);
						rect = new CellRect(intVec2.x, intVec2.z, 1, num);
					}
					rect.ClipInsideRect(new CellRect(0, 0, size.x, size.z));
					if (rect.Area >= 3 && SketchResolver_MechClusterWalls.<TryAddWall>g__WallRectIsUsable|9_0(rect, false, ref CS$<>8__locals1))
					{
						SketchResolver_MechClusterWalls.<TryAddWall>g__GenerateWallInRect|9_1(rect, Rand.Bool, ref CS$<>8__locals1);
						return;
					}
				}
				else
				{
					IntVec3 intVec3 = new IntVec3(Rand.RangeInclusive(0, size.x - 1), 0, Rand.RangeInclusive(0, size.z - 1));
					int num2 = GenMath.RoundRandom(Rand.Range((float)size.x * 0.4f, (float)size.x));
					CellRect rect2;
					if (Rand.Bool)
					{
						rect2 = new CellRect(intVec3.x, intVec3.z, num2, 1);
					}
					else
					{
						rect2 = new CellRect(intVec3.x - num2 + 1, intVec3.z, num2, 1);
					}
					rect2.ClipInsideRect(new CellRect(0, 0, size.x, size.z));
					if (rect2.Area >= 2)
					{
						int num3 = GenMath.RoundRandom(Rand.Range((float)size.z * 0.4f, (float)size.z));
						CellRect rect3;
						if (Rand.Bool)
						{
							rect3 = new CellRect(intVec3.x, intVec3.z, 1, num3);
						}
						else
						{
							rect3 = new CellRect(intVec3.x, intVec3.z - num3 + 1, 1, num3);
						}
						rect3.ClipInsideRect(new CellRect(0, 0, size.x, size.z));
						if (rect3.Area >= 2 && SketchResolver_MechClusterWalls.<TryAddWall>g__WallRectIsUsable|9_0(rect2, true, ref CS$<>8__locals1) && SketchResolver_MechClusterWalls.<TryAddWall>g__WallRectIsUsable|9_0(rect3, true, ref CS$<>8__locals1))
						{
							SketchResolver_MechClusterWalls.<TryAddWall>g__GenerateWallInRect|9_1(rect2, false, ref CS$<>8__locals1);
							SketchResolver_MechClusterWalls.<TryAddWall>g__GenerateWallInRect|9_1(rect3, false, ref CS$<>8__locals1);
							return;
						}
					}
				}
			}
		}

		// Token: 0x06008269 RID: 33385 RVA: 0x002E37E4 File Offset: 0x002E19E4
		[CompilerGenerated]
		internal static bool <TryAddWall>g__WallRectIsUsable|9_0(CellRect rect, bool checkAdjacentCells, ref SketchResolver_MechClusterWalls.<>c__DisplayClass9_0 A_2)
		{
			foreach (IntVec3 intVec in rect)
			{
				if (checkAdjacentCells)
				{
					for (int i = 0; i < 9; i++)
					{
						IntVec3 pos = intVec + GenAdj.AdjacentCellsAndInside[i];
						if (A_2.sketch.EdificeAt(pos) != null)
						{
							return false;
						}
					}
				}
				else if (A_2.sketch.EdificeAt(intVec) != null)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600826A RID: 33386 RVA: 0x002E387C File Offset: 0x002E1A7C
		[CompilerGenerated]
		internal static void <TryAddWall>g__GenerateWallInRect|9_1(CellRect rect, bool createRandomGap, ref SketchResolver_MechClusterWalls.<>c__DisplayClass9_0 A_2)
		{
			IntVec3 randomCell = rect.RandomCell;
			foreach (IntVec3 intVec in rect)
			{
				if (!createRandomGap || !(intVec == randomCell))
				{
					A_2.sketch.AddThing(ThingDefOf.Wall, intVec, Rot4.North, ThingDefOf.Steel, 1, null, null, true);
				}
			}
		}

		// Token: 0x04005125 RID: 20773
		private static readonly FloatRange WallCountRandomFactorRange = new FloatRange(0.5f, 1f);

		// Token: 0x04005126 RID: 20774
		private static readonly SimpleCurve WidthToMaxWallsCountCurve = new SimpleCurve
		{
			{
				new CurvePoint(3f, 1f),
				true
			},
			{
				new CurvePoint(6f, 2f),
				true
			},
			{
				new CurvePoint(9f, 3f),
				true
			},
			{
				new CurvePoint(14f, 4f),
				true
			}
		};

		// Token: 0x04005127 RID: 20775
		private const float Straight_LengthMinSizeFraction = 0.8f;

		// Token: 0x04005128 RID: 20776
		private const float Corner_LengthMinSizeFraction = 0.4f;

		// Token: 0x04005129 RID: 20777
		private const float EdgeWallChance = 0.8f;

		// Token: 0x0400512A RID: 20778
		private const int MinWallLengthStraight = 3;

		// Token: 0x0400512B RID: 20779
		private const int MinWallLengthCorner = 2;
	}
}
