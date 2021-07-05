using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011E6 RID: 4582
	public class IncidentWorker_WildManWandersIn : IncidentWorker
	{
		// Token: 0x06006457 RID: 25687 RVA: 0x001F2904 File Offset: 0x001F0B04
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			Faction faction;
			if (!this.TryFindFormerFaction(out faction))
			{
				return false;
			}
			Map map = (Map)parms.target;
			IntVec3 intVec;
			return !map.GameConditionManager.ConditionIsActive(GameConditionDefOf.ToxicFallout) && map.mapTemperature.SeasonAcceptableFor(ThingDefOf.Human) && this.TryFindEntryCell(map, out intVec);
		}

		// Token: 0x06006458 RID: 25688 RVA: 0x001F2968 File Offset: 0x001F0B68
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 loc;
			if (!this.TryFindEntryCell(map, out loc))
			{
				return false;
			}
			Faction faction;
			if (!this.TryFindFormerFaction(out faction))
			{
				return false;
			}
			Pawn pawn = PawnGenerator.GeneratePawn(PawnKindDefOf.WildMan, faction);
			pawn.SetFaction(null, null);
			GenSpawn.Spawn(pawn, loc, map, WipeMode.Vanish);
			TaggedString baseLetterLabel = this.def.letterLabel.Formatted(pawn.LabelShort, pawn.Named("PAWN")).CapitalizeFirst();
			TaggedString baseLetterText = this.def.letterText.Formatted(pawn.NameShortColored, pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true).CapitalizeFirst();
			PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref baseLetterText, ref baseLetterLabel, pawn);
			base.SendStandardLetter(baseLetterLabel, baseLetterText, this.def.letterDef, parms, pawn, Array.Empty<NamedArgument>());
			return true;
		}

		// Token: 0x06006459 RID: 25689 RVA: 0x001F2A58 File Offset: 0x001F0C58
		private bool TryFindEntryCell(Map map, out IntVec3 cell)
		{
			return CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => map.reachability.CanReachColony(c), map, CellFinder.EdgeRoadChance_Ignore, out cell);
		}

		// Token: 0x0600645A RID: 25690 RVA: 0x00044C91 File Offset: 0x00042E91
		private bool TryFindFormerFaction(out Faction formerFaction)
		{
			return Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction_NewTemp(out formerFaction, false, true, TechLevel.Undefined, false);
		}
	}
}
