using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000AB8 RID: 2744
	public static class CastPositionFinder
	{
		// Token: 0x060040D0 RID: 16592 RVA: 0x00184984 File Offset: 0x00182B84
		public static bool TryFindCastPosition(CastPositionRequest newReq, out IntVec3 dest)
		{
			req = newReq;
			casterLoc = req.caster.Position;
			targetLoc = req.target.Position;
			verb = req.verb;
			avoidGrid = newReq.caster.GetAvoidGrid(false);
			if (verb == null)
			{
				Log.Error(req.caster + " tried to find casting position without a verb.", false);
				dest = IntVec3.Invalid;
				return false;
			}
			if (req.maxRegions > 0)
			{
				Region region = casterLoc.GetRegion(req.caster.Map, RegionType.Set_Passable);
				if (region == null)
				{
					Log.Error("TryFindCastPosition requiring region traversal but root region is null.", false);
					dest = IntVec3.Invalid;
					return false;
				}
				inRadiusMark = Rand.Int;
				RegionTraverser.MarkRegionsBFS(region, null, newReq.maxRegions, inRadiusMark);
				if (req.maxRangeFromLocus > 0.01f)
				{
					Region locusReg = req.locus.GetRegion(req.caster.Map);
					if (locusReg == null)
					{
						Log.Error("locus " + req.locus + " has no region", false);
						dest = IntVec3.Invalid;
						return false;
					}
					if (locusReg.mark != inRadiusMark)
					{
						inRadiusMark = Rand.Int;
						RegionTraverser.BreadthFirstTraverse(region, null, delegate(Region r)
						{
							r.mark = inRadiusMark;
							req.maxRegions = req.maxRegions + 1;
							return r == locusReg;
						});
					}
				}
			}
			CellRect cellRect = CellRect.WholeMap(req.caster.Map);
			if (req.maxRangeFromCaster > 0.01f)
			{
				int num = Mathf.CeilToInt(req.maxRangeFromCaster);
				CellRect otherRect = new CellRect(casterLoc.x - num, casterLoc.z - num, num * 2 + 1, num * 2 + 1);
				cellRect.ClipInsideRect(otherRect);
			}
			int num2 = Mathf.CeilToInt(req.maxRangeFromTarget);
			CellRect otherRect2 = new CellRect(targetLoc.x - num2, targetLoc.z - num2, num2 * 2 + 1, num2 * 2 + 1);
			cellRect.ClipInsideRect(otherRect2);
			if (req.maxRangeFromLocus > 0.01f)
			{
				int num3 = Mathf.CeilToInt(req.maxRangeFromLocus);
				CellRect otherRect3 = new CellRect(targetLoc.x - num3, targetLoc.z - num3, num3 * 2 + 1, num3 * 2 + 1);
				cellRect.ClipInsideRect(otherRect3);
			}
			bestSpot = IntVec3.Invalid;
			bestSpotPref = 0.001f;
			maxRangeFromCasterSquared = req.maxRangeFromCaster * req.maxRangeFromCaster;
			maxRangeFromTargetSquared = req.maxRangeFromTarget * req.maxRangeFromTarget;
			maxRangeFromLocusSquared = req.maxRangeFromLocus * req.maxRangeFromLocus;
			rangeFromTarget = (req.caster.Position - req.target.Position).LengthHorizontal;
			rangeFromTargetSquared = (req.caster.Position - req.target.Position).LengthHorizontalSquared;
			optimalRangeSquared = verb.verbProps.range * 0.8f * (verb.verbProps.range * 0.8f);
			EvaluateCell(req.caster.Position);
			if (bestSpotPref >= 1.0)
			{
				dest = req.caster.Position;
				return true;
			}
			float slope = -1f / CellLine.Between(req.target.Position, req.caster.Position).Slope;
			CellLine cellLine = new CellLine(req.target.Position, slope);
			bool flag = cellLine.CellIsAbove(req.caster.Position);
			foreach (IntVec3 c in cellRect)
			{
				if (cellLine.CellIsAbove(c) == flag && cellRect.Contains(c))
				{
					EvaluateCell(c);
				}
			}
			if (bestSpot.IsValid && bestSpotPref > 0.33f)
			{
				dest = bestSpot;
				return true;
			}
			foreach (IntVec3 c2 in cellRect)
			{
				if (cellLine.CellIsAbove(c2) != flag && cellRect.Contains(c2))
				{
					EvaluateCell(c2);
				}
			}
			if (bestSpot.IsValid)
			{
				dest = bestSpot;
				return true;
			}
			dest = casterLoc;
			return false;
		}

		// Token: 0x060040D1 RID: 16593 RVA: 0x00184EC0 File Offset: 0x001830C0
		private static void EvaluateCell(IntVec3 c)
		{
			if (CastPositionFinder.maxRangeFromTargetSquared > 0.01f && CastPositionFinder.maxRangeFromTargetSquared < 250000f && (float)(c - CastPositionFinder.req.target.Position).LengthHorizontalSquared > CastPositionFinder.maxRangeFromTargetSquared)
			{
				if (DebugViewSettings.drawCastPositionSearch)
				{
					CastPositionFinder.req.caster.Map.debugDrawer.FlashCell(c, 0f, "range target", 50);
				}
				return;
			}
			if ((double)CastPositionFinder.maxRangeFromLocusSquared > 0.01 && (float)(c - CastPositionFinder.req.locus).LengthHorizontalSquared > CastPositionFinder.maxRangeFromLocusSquared)
			{
				if (DebugViewSettings.drawCastPositionSearch)
				{
					CastPositionFinder.req.caster.Map.debugDrawer.FlashCell(c, 0.1f, "range home", 50);
				}
				return;
			}
			if (CastPositionFinder.maxRangeFromCasterSquared > 0.01f)
			{
				CastPositionFinder.rangeFromCasterToCellSquared = (float)(c - CastPositionFinder.req.caster.Position).LengthHorizontalSquared;
				if (CastPositionFinder.rangeFromCasterToCellSquared > CastPositionFinder.maxRangeFromCasterSquared)
				{
					if (DebugViewSettings.drawCastPositionSearch)
					{
						CastPositionFinder.req.caster.Map.debugDrawer.FlashCell(c, 0.2f, "range caster", 50);
					}
					return;
				}
			}
			if (!c.Walkable(CastPositionFinder.req.caster.Map))
			{
				return;
			}
			if (CastPositionFinder.req.maxRegions > 0 && c.GetRegion(CastPositionFinder.req.caster.Map, RegionType.Set_Passable).mark != CastPositionFinder.inRadiusMark)
			{
				if (DebugViewSettings.drawCastPositionSearch)
				{
					CastPositionFinder.req.caster.Map.debugDrawer.FlashCell(c, 0.64f, "reg radius", 50);
				}
				return;
			}
			if (!CastPositionFinder.req.caster.Map.reachability.CanReach(CastPositionFinder.req.caster.Position, c, PathEndMode.OnCell, TraverseParms.For(CastPositionFinder.req.caster, Danger.Some, TraverseMode.ByPawn, false)))
			{
				if (DebugViewSettings.drawCastPositionSearch)
				{
					CastPositionFinder.req.caster.Map.debugDrawer.FlashCell(c, 0.4f, "can't reach", 50);
				}
				return;
			}
			float num = CastPositionFinder.CastPositionPreference(c);
			if (CastPositionFinder.avoidGrid != null)
			{
				byte b = CastPositionFinder.avoidGrid[c];
				num *= Mathf.Max(0.1f, (37.5f - (float)b) / 37.5f);
			}
			if (DebugViewSettings.drawCastPositionSearch)
			{
				CastPositionFinder.req.caster.Map.debugDrawer.FlashCell(c, num / 4f, num.ToString("F3"), 50);
			}
			if (num < CastPositionFinder.bestSpotPref)
			{
				return;
			}
			if (!CastPositionFinder.verb.CanHitTargetFrom(c, CastPositionFinder.req.target))
			{
				if (DebugViewSettings.drawCastPositionSearch)
				{
					CastPositionFinder.req.caster.Map.debugDrawer.FlashCell(c, 0.6f, "can't hit", 50);
				}
				return;
			}
			if (!CastPositionFinder.req.caster.Map.pawnDestinationReservationManager.CanReserve(c, CastPositionFinder.req.caster, false))
			{
				if (DebugViewSettings.drawCastPositionSearch)
				{
					CastPositionFinder.req.caster.Map.debugDrawer.FlashCell(c, num * 0.9f, "resvd", 50);
				}
				return;
			}
			if (PawnUtility.KnownDangerAt(c, CastPositionFinder.req.caster.Map, CastPositionFinder.req.caster))
			{
				if (DebugViewSettings.drawCastPositionSearch)
				{
					CastPositionFinder.req.caster.Map.debugDrawer.FlashCell(c, 0.9f, "danger", 50);
				}
				return;
			}
			CastPositionFinder.bestSpot = c;
			CastPositionFinder.bestSpotPref = num;
		}

		// Token: 0x060040D2 RID: 16594 RVA: 0x00185258 File Offset: 0x00183458
		private static float CastPositionPreference(IntVec3 c)
		{
			bool flag = true;
			List<Thing> list = CastPositionFinder.req.caster.Map.thingGrid.ThingsListAtFast(c);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				Fire fire = thing as Fire;
				if (fire != null && fire.parent == null)
				{
					return -1f;
				}
				if (thing.def.passability == Traversability.PassThroughOnly)
				{
					flag = false;
				}
			}
			float num = 0.3f;
			if (CastPositionFinder.req.caster.kindDef.aiAvoidCover)
			{
				num += 8f - CoverUtility.TotalSurroundingCoverScore(c, CastPositionFinder.req.caster.Map);
			}
			if (CastPositionFinder.req.wantCoverFromTarget)
			{
				num += CoverUtility.CalculateOverallBlockChance(c, CastPositionFinder.req.target.Position, CastPositionFinder.req.caster.Map) * 0.55f;
			}
			float num2 = (CastPositionFinder.req.caster.Position - c).LengthHorizontal;
			if (CastPositionFinder.rangeFromTarget > 100f)
			{
				num2 -= CastPositionFinder.rangeFromTarget - 100f;
				if (num2 < 0f)
				{
					num2 = 0f;
				}
			}
			num *= Mathf.Pow(0.967f, num2);
			float num3 = 1f;
			CastPositionFinder.rangeFromTargetToCellSquared = (float)(c - CastPositionFinder.req.target.Position).LengthHorizontalSquared;
			float num4 = Mathf.Abs(CastPositionFinder.rangeFromTargetToCellSquared - CastPositionFinder.optimalRangeSquared) / CastPositionFinder.optimalRangeSquared;
			num4 = 1f - num4;
			num4 = 0.7f + 0.3f * num4;
			num3 *= num4;
			if (CastPositionFinder.rangeFromTargetToCellSquared < 25f)
			{
				num3 *= 0.5f;
			}
			num *= num3;
			if (CastPositionFinder.rangeFromCasterToCellSquared > CastPositionFinder.rangeFromTargetSquared)
			{
				num *= 0.4f;
			}
			if (!flag)
			{
				num *= 0.2f;
			}
			return num;
		}

		// Token: 0x04002CB2 RID: 11442
		private static CastPositionRequest req;

		// Token: 0x04002CB3 RID: 11443
		private static IntVec3 casterLoc;

		// Token: 0x04002CB4 RID: 11444
		private static IntVec3 targetLoc;

		// Token: 0x04002CB5 RID: 11445
		private static Verb verb;

		// Token: 0x04002CB6 RID: 11446
		private static float rangeFromTarget;

		// Token: 0x04002CB7 RID: 11447
		private static float rangeFromTargetSquared;

		// Token: 0x04002CB8 RID: 11448
		private static float optimalRangeSquared;

		// Token: 0x04002CB9 RID: 11449
		private static float rangeFromCasterToCellSquared;

		// Token: 0x04002CBA RID: 11450
		private static float rangeFromTargetToCellSquared;

		// Token: 0x04002CBB RID: 11451
		private static int inRadiusMark;

		// Token: 0x04002CBC RID: 11452
		private static ByteGrid avoidGrid;

		// Token: 0x04002CBD RID: 11453
		private static float maxRangeFromCasterSquared;

		// Token: 0x04002CBE RID: 11454
		private static float maxRangeFromTargetSquared;

		// Token: 0x04002CBF RID: 11455
		private static float maxRangeFromLocusSquared;

		// Token: 0x04002CC0 RID: 11456
		private static IntVec3 bestSpot = IntVec3.Invalid;

		// Token: 0x04002CC1 RID: 11457
		private static float bestSpotPref = 0.001f;

		// Token: 0x04002CC2 RID: 11458
		private const float BaseAIPreference = 0.3f;

		// Token: 0x04002CC3 RID: 11459
		private const float MinimumPreferredRange = 5f;

		// Token: 0x04002CC4 RID: 11460
		private const float OptimalRangeFactor = 0.8f;

		// Token: 0x04002CC5 RID: 11461
		private const float OptimalRangeFactorImportance = 0.3f;

		// Token: 0x04002CC6 RID: 11462
		private const float CoverPreferenceFactor = 0.55f;
	}
}
