using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C1D RID: 3101
	public class IncidentWorker_WildManWandersIn : IncidentWorker
	{
		// Token: 0x060048CA RID: 18634 RVA: 0x00181104 File Offset: 0x0017F304
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

		// Token: 0x060048CB RID: 18635 RVA: 0x00181168 File Offset: 0x0017F368
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

		// Token: 0x060048CC RID: 18636 RVA: 0x00181258 File Offset: 0x0017F458
		private bool TryFindEntryCell(Map map, out IntVec3 cell)
		{
			return CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => map.reachability.CanReachColony(c), map, CellFinder.EdgeRoadChance_Ignore, out cell);
		}

		// Token: 0x060048CD RID: 18637 RVA: 0x0018128F File Offset: 0x0017F48F
		private bool TryFindFormerFaction(out Faction formerFaction)
		{
			return Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction(out formerFaction, false, true, TechLevel.Undefined, false);
		}
	}
}
