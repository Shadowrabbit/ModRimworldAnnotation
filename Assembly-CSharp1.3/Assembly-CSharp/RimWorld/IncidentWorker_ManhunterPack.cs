using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C0E RID: 3086
	public class IncidentWorker_ManhunterPack : IncidentWorker
	{
		// Token: 0x06004888 RID: 18568 RVA: 0x0017FA90 File Offset: 0x0017DC90
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			Map map = (Map)parms.target;
			PawnKindDef pawnKindDef;
			IntVec3 intVec;
			return ManhunterPackIncidentUtility.TryFindManhunterAnimalKind(parms.points, map.Tile, out pawnKindDef) && RCellFinder.TryFindRandomPawnEntryCell(out intVec, map, CellFinder.EdgeRoadChance_Animal, false, null);
		}

		// Token: 0x06004889 RID: 18569 RVA: 0x0017FADC File Offset: 0x0017DCDC
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			PawnKindDef pawnKind = parms.pawnKind;
			if ((pawnKind == null && !ManhunterPackIncidentUtility.TryFindManhunterAnimalKind(parms.points, map.Tile, out pawnKind)) || ManhunterPackIncidentUtility.GetAnimalsCount(pawnKind, parms.points) == 0)
			{
				return false;
			}
			IntVec3 spawnCenter = parms.spawnCenter;
			if (!spawnCenter.IsValid && !RCellFinder.TryFindRandomPawnEntryCell(out spawnCenter, map, CellFinder.EdgeRoadChance_Animal, false, null))
			{
				return false;
			}
			List<Pawn> list = ManhunterPackIncidentUtility.GenerateAnimals(pawnKind, map.Tile, parms.points * 1f, parms.pawnCount);
			Rot4 rot = Rot4.FromAngleFlat((map.Center - spawnCenter).AngleFlat);
			for (int i = 0; i < list.Count; i++)
			{
				Pawn pawn = list[i];
				IntVec3 loc = CellFinder.RandomClosewalkCellNear(spawnCenter, map, 10, null);
				QuestUtility.AddQuestTag(GenSpawn.Spawn(pawn, loc, map, rot, WipeMode.Vanish, false), parms.questTag);
				pawn.health.AddHediff(HediffDefOf.Scaria, null, null, null);
				pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.ManhunterPermanent, null, false, false, null, false, false, false);
				pawn.mindState.exitMapAfterTick = Find.TickManager.TicksGame + Rand.Range(60000, 120000);
			}
			base.SendStandardLetter("LetterLabelManhunterPackArrived".Translate(), "ManhunterPackArrived".Translate(pawnKind.GetLabelPlural(-1)), LetterDefOf.ThreatBig, parms, list[0], Array.Empty<NamedArgument>());
			Find.TickManager.slower.SignalForceNormalSpeedShort();
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.ForbiddingDoors, OpportunityType.Critical);
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.AllowedAreas, OpportunityType.Important);
			return true;
		}

		// Token: 0x04002C65 RID: 11365
		private const float PointsFactor = 1f;

		// Token: 0x04002C66 RID: 11366
		private const int AnimalsStayDurationMin = 60000;

		// Token: 0x04002C67 RID: 11367
		private const int AnimalsStayDurationMax = 120000;
	}
}
