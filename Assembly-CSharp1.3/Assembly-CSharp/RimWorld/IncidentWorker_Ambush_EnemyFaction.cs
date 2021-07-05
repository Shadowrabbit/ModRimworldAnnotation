using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000BF4 RID: 3060
	public class IncidentWorker_Ambush_EnemyFaction : IncidentWorker_Ambush
	{
		// Token: 0x0600480C RID: 18444 RVA: 0x0017C6C4 File Offset: 0x0017A8C4
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Faction faction;
			return base.CanFireNowSub(parms) && PawnGroupMakerUtility.TryGetRandomFactionForCombatPawnGroup(parms.points, out faction, null, false, false, false, true);
		}

		// Token: 0x0600480D RID: 18445 RVA: 0x0017C6F0 File Offset: 0x0017A8F0
		protected override List<Pawn> GeneratePawns(IncidentParms parms)
		{
			if (!PawnGroupMakerUtility.TryGetRandomFactionForCombatPawnGroup(parms.points, out parms.faction, null, false, false, false, true))
			{
				Log.Error("Could not find any valid faction for " + this.def + " incident.");
				return new List<Pawn>();
			}
			PawnGroupMakerParms defaultPawnGroupMakerParms = IncidentParmsUtility.GetDefaultPawnGroupMakerParms(PawnGroupKindDefOf.Combat, parms, false);
			defaultPawnGroupMakerParms.generateFightersOnly = true;
			defaultPawnGroupMakerParms.dontUseSingleUseRocketLaunchers = true;
			return PawnGroupMakerUtility.GeneratePawns(defaultPawnGroupMakerParms, true).ToList<Pawn>();
		}

		// Token: 0x0600480E RID: 18446 RVA: 0x0017C75A File Offset: 0x0017A95A
		protected override LordJob CreateLordJob(List<Pawn> generatedPawns, IncidentParms parms)
		{
			return new LordJob_AssaultColony(parms.faction, true, false, false, false, true, false, false);
		}

		// Token: 0x0600480F RID: 18447 RVA: 0x0017C770 File Offset: 0x0017A970
		protected override string GetLetterText(Pawn anyPawn, IncidentParms parms)
		{
			Caravan caravan = parms.target as Caravan;
			return string.Format(this.def.letterText, (caravan != null) ? caravan.Name : "yourCaravan".TranslateSimple(), parms.faction.def.pawnsPlural, parms.faction.Name).CapitalizeFirst();
		}
	}
}
