using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020011E1 RID: 4577
	public class IncidentWorker_ThrumboPasses : IncidentWorker
	{
		// Token: 0x06006445 RID: 25669 RVA: 0x001F2334 File Offset: 0x001F0534
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 intVec;
			return !map.gameConditionManager.ConditionIsActive(GameConditionDefOf.ToxicFallout) && map.mapTemperature.SeasonAndOutdoorTemperatureAcceptableFor(ThingDefOf.Thrumbo) && this.TryFindEntryCell(map, out intVec);
		}

		// Token: 0x06006446 RID: 25670 RVA: 0x001F2380 File Offset: 0x001F0580
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 intVec;
			if (!this.TryFindEntryCell(map, out intVec))
			{
				return false;
			}
			PawnKindDef thrumbo = PawnKindDefOf.Thrumbo;
			int num = GenMath.RoundRandom(StorytellerUtility.DefaultThreatPointsNow(map) / thrumbo.combatPower);
			int max = Rand.RangeInclusive(3, 6);
			num = Mathf.Clamp(num, 2, max);
			int num2 = Rand.RangeInclusive(90000, 150000);
			IntVec3 invalid = IntVec3.Invalid;
			if (!RCellFinder.TryFindRandomCellOutsideColonyNearTheCenterOfTheMap(intVec, map, 10f, out invalid))
			{
				invalid = IntVec3.Invalid;
			}
			Pawn pawn = null;
			for (int i = 0; i < num; i++)
			{
				IntVec3 loc = CellFinder.RandomClosewalkCellNear(intVec, map, 10, null);
				pawn = PawnGenerator.GeneratePawn(thrumbo, null);
				GenSpawn.Spawn(pawn, loc, map, Rot4.Random, WipeMode.Vanish, false);
				pawn.mindState.exitMapAfterTick = Find.TickManager.TicksGame + num2;
				if (invalid.IsValid)
				{
					pawn.mindState.forcedGotoPosition = CellFinder.RandomClosewalkCellNear(invalid, map, 10, null);
				}
			}
			base.SendStandardLetter("LetterLabelThrumboPasses".Translate(thrumbo.label).CapitalizeFirst(), "LetterThrumboPasses".Translate(thrumbo.label), LetterDefOf.PositiveEvent, parms, pawn, Array.Empty<NamedArgument>());
			return true;
		}

		// Token: 0x06006447 RID: 25671 RVA: 0x00044C4D File Offset: 0x00042E4D
		private bool TryFindEntryCell(Map map, out IntVec3 cell)
		{
			return RCellFinder.TryFindRandomPawnEntryCell(out cell, map, CellFinder.EdgeRoadChance_Animal + 0.2f, false, null);
		}
	}
}
