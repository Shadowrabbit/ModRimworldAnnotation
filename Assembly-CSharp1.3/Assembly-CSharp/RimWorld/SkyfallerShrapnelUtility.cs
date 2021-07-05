using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020010D4 RID: 4308
	public static class SkyfallerShrapnelUtility
	{
		// Token: 0x06006726 RID: 26406 RVA: 0x0022DB29 File Offset: 0x0022BD29
		public static void MakeShrapnel(IntVec3 center, Map map, float angle, float distanceFactor, int metalShrapnelCount, int rubbleShrapnelCount, bool spawnMotes)
		{
			angle -= 90f;
			SkyfallerShrapnelUtility.SpawnShrapnel(ThingDefOf.ChunkSlagSteel, metalShrapnelCount, center, map, angle, distanceFactor);
			SkyfallerShrapnelUtility.SpawnShrapnel(ThingDefOf.Filth_RubbleBuilding, rubbleShrapnelCount, center, map, angle, distanceFactor);
			if (spawnMotes)
			{
				SkyfallerShrapnelUtility.ThrowShrapnelMotes((metalShrapnelCount + rubbleShrapnelCount) * 2, center, map, angle, distanceFactor);
			}
		}

		// Token: 0x06006727 RID: 26407 RVA: 0x0022DB68 File Offset: 0x0022BD68
		private static void SpawnShrapnel(ThingDef def, int quantity, IntVec3 center, Map map, float angle, float distanceFactor)
		{
			for (int i = 0; i < quantity; i++)
			{
				IntVec3 intVec = SkyfallerShrapnelUtility.GenerateShrapnelLocation(center, angle, distanceFactor);
				if (SkyfallerShrapnelUtility.IsGoodShrapnelCell(intVec, map) && (def.category != ThingCategory.Item || intVec.GetFirstItem(map) == null) && intVec.GetFirstThing(map, def) == null)
				{
					GenSpawn.Spawn(def, intVec, map, WipeMode.Vanish);
				}
			}
		}

		// Token: 0x06006728 RID: 26408 RVA: 0x0022DBBC File Offset: 0x0022BDBC
		private static void ThrowShrapnelMotes(int count, IntVec3 center, Map map, float angle, float distanceFactor)
		{
			for (int i = 0; i < count; i++)
			{
				IntVec3 c = SkyfallerShrapnelUtility.GenerateShrapnelLocation(center, angle, distanceFactor);
				if (SkyfallerShrapnelUtility.IsGoodShrapnelCell(c, map))
				{
					FleckMaker.ThrowDustPuff(c.ToVector3Shifted() + Gen.RandomHorizontalVector(0.5f), map, 2f);
				}
			}
		}

		// Token: 0x06006729 RID: 26409 RVA: 0x0022DC09 File Offset: 0x0022BE09
		private static bool IsGoodShrapnelCell(IntVec3 c, Map map)
		{
			return c.InBounds(map) && !c.Impassable(map) && !c.Filled(map) && map.roofGrid.RoofAt(c) == null;
		}

		// Token: 0x0600672A RID: 26410 RVA: 0x0022DC3C File Offset: 0x0022BE3C
		private static IntVec3 GenerateShrapnelLocation(IntVec3 center, float angleOffset, float distanceFactor)
		{
			float num = SkyfallerShrapnelUtility.ShrapnelAngleDistribution.Evaluate(Rand.Value);
			float d = SkyfallerShrapnelUtility.ShrapnelDistanceFromAngle.Evaluate(num) * Rand.Value * distanceFactor;
			return (Vector3Utility.HorizontalVectorFromAngle(num + angleOffset) * d).ToIntVec3() + center;
		}

		// Token: 0x04003A39 RID: 14905
		private const float ShrapnelDistanceFront = 6f;

		// Token: 0x04003A3A RID: 14906
		private const float ShrapnelDistanceSide = 4f;

		// Token: 0x04003A3B RID: 14907
		private const float ShrapnelDistanceBack = 30f;

		// Token: 0x04003A3C RID: 14908
		private const int MotesPerShrapnel = 2;

		// Token: 0x04003A3D RID: 14909
		private static readonly SimpleCurve ShrapnelDistanceFromAngle = new SimpleCurve
		{
			{
				new CurvePoint(0f, 6f),
				true
			},
			{
				new CurvePoint(90f, 4f),
				true
			},
			{
				new CurvePoint(135f, 4f),
				true
			},
			{
				new CurvePoint(180f, 30f),
				true
			},
			{
				new CurvePoint(225f, 4f),
				true
			},
			{
				new CurvePoint(270f, 4f),
				true
			},
			{
				new CurvePoint(360f, 6f),
				true
			}
		};

		// Token: 0x04003A3E RID: 14910
		private static readonly SimpleCurve ShrapnelAngleDistribution = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(0.1f, 90f),
				true
			},
			{
				new CurvePoint(0.25f, 135f),
				true
			},
			{
				new CurvePoint(0.5f, 180f),
				true
			},
			{
				new CurvePoint(0.75f, 225f),
				true
			},
			{
				new CurvePoint(0.9f, 270f),
				true
			},
			{
				new CurvePoint(1f, 360f),
				true
			}
		};
	}
}
