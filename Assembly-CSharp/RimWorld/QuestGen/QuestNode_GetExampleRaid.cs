using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FF3 RID: 8179
	public class QuestNode_GetExampleRaid : QuestNode
	{
		// Token: 0x0600AD5A RID: 44378 RVA: 0x00070719 File Offset: 0x0006E919
		protected override bool TestRunInt(Slate slate)
		{
			return slate.Exists("map", false);
		}

		// Token: 0x0600AD5B RID: 44379 RVA: 0x00327688 File Offset: 0x00325888
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			slate.Get<Map>("map", null, false);
			PawnGroupMakerParms pawnGroupMakerParms = new PawnGroupMakerParms();
			pawnGroupMakerParms.groupKind = PawnGroupKindDefOf.Combat;
			pawnGroupMakerParms.raidStrategy = RaidStrategyDefOf.ImmediateAttack;
			PawnGroupMakerParms pawnGroupMakerParms2 = pawnGroupMakerParms;
			Faction faction;
			if ((faction = this.faction.GetValue(slate)) == null)
			{
				faction = (from x in Find.FactionManager.GetFactions_NewTemp(false, false, true, TechLevel.Industrial, false)
				where x.HostileTo(Faction.OfPlayer)
				select x).RandomElement<Faction>();
			}
			pawnGroupMakerParms2.faction = faction;
			pawnGroupMakerParms.points = IncidentWorker_Raid.AdjustedRaidPoints(this.points.GetValue(slate), PawnsArrivalModeDefOf.EdgeWalkIn, RaidStrategyDefOf.ImmediateAttack, pawnGroupMakerParms.faction, PawnGroupKindDefOf.Combat);
			IEnumerable<PawnKindDef> pawnKinds = PawnGroupMakerUtility.GeneratePawnKindsExample(pawnGroupMakerParms);
			slate.Set<string>(this.storeAs.GetValue(slate), PawnUtility.PawnKindsToLineList(pawnKinds, "  - ", ColoredText.ThreatColor), false);
		}

		// Token: 0x040076DF RID: 30431
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x040076E0 RID: 30432
		public SlateRef<Faction> faction;

		// Token: 0x040076E1 RID: 30433
		public SlateRef<float> points;
	}
}
