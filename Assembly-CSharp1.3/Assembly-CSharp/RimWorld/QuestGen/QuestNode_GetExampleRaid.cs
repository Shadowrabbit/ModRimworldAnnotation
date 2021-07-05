using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200171E RID: 5918
	public class QuestNode_GetExampleRaid : QuestNode
	{
		// Token: 0x06008883 RID: 34947 RVA: 0x00304E1D File Offset: 0x0030301D
		protected override bool TestRunInt(Slate slate)
		{
			return slate.Exists("map", false);
		}

		// Token: 0x06008884 RID: 34948 RVA: 0x00310ABC File Offset: 0x0030ECBC
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
				faction = (from x in Find.FactionManager.GetFactions(false, false, true, TechLevel.Industrial, false)
				where x.HostileTo(Faction.OfPlayer)
				select x).RandomElement<Faction>();
			}
			pawnGroupMakerParms2.faction = faction;
			pawnGroupMakerParms.points = IncidentWorker_Raid.AdjustedRaidPoints(this.points.GetValue(slate), PawnsArrivalModeDefOf.EdgeWalkIn, RaidStrategyDefOf.ImmediateAttack, pawnGroupMakerParms.faction, PawnGroupKindDefOf.Combat);
			IEnumerable<PawnKindDef> pawnKinds = PawnGroupMakerUtility.GeneratePawnKindsExample(pawnGroupMakerParms);
			slate.Set<string>(this.storeAs.GetValue(slate), PawnUtility.PawnKindsToLineList(pawnKinds, "  - ", ColoredText.ThreatColor), false);
		}

		// Token: 0x0400566D RID: 22125
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400566E RID: 22126
		public SlateRef<Faction> faction;

		// Token: 0x0400566F RID: 22127
		public SlateRef<float> points;
	}
}
