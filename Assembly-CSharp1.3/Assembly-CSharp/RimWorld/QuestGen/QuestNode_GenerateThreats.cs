using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200171D RID: 5917
	public class QuestNode_GenerateThreats : QuestNode
	{
		// Token: 0x06008880 RID: 34944 RVA: 0x003108BC File Offset: 0x0030EABC
		protected override bool TestRunInt(Slate slate)
		{
			return Find.Storyteller.difficulty.allowViolentQuests && slate.Get<Map>("map", null, false) != null;
		}

		// Token: 0x06008881 RID: 34945 RVA: 0x003108E4 File Offset: 0x0030EAE4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Map map = slate.Get<Map>("map", null, false);
			QuestPart_ThreatsGenerator questPart_ThreatsGenerator = new QuestPart_ThreatsGenerator();
			questPart_ThreatsGenerator.threatStartTicks = this.threatStartTicks.GetValue(slate);
			questPart_ThreatsGenerator.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			questPart_ThreatsGenerator.inSignalDisable = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalDisable.GetValue(slate));
			ThreatsGeneratorParams value = this.parms.GetValue(slate);
			value.faction = (this.faction.GetValue(slate) ?? value.faction);
			questPart_ThreatsGenerator.parms = value;
			questPart_ThreatsGenerator.mapParent = map.Parent;
			QuestGen.quest.AddPart(questPart_ThreatsGenerator);
			if (!this.storeThreatExampleAs.GetValue(slate).NullOrEmpty())
			{
				PawnGroupMakerParms pawnGroupMakerParms = new PawnGroupMakerParms();
				pawnGroupMakerParms.groupKind = PawnGroupKindDefOf.Combat;
				pawnGroupMakerParms.raidStrategy = RaidStrategyDefOf.ImmediateAttack;
				PawnGroupMakerParms pawnGroupMakerParms2 = pawnGroupMakerParms;
				Faction faction;
				if ((faction = value.faction) == null)
				{
					faction = (from x in Find.FactionManager.GetFactions(false, false, true, TechLevel.Industrial, false)
					where x.HostileTo(Faction.OfPlayer)
					select x).RandomElement<Faction>();
				}
				pawnGroupMakerParms2.faction = faction;
				float num = value.threatPoints ?? (StorytellerUtility.DefaultThreatPointsNow(map) * value.currentThreatPointsFactor);
				if (value.minThreatPoints != null)
				{
					num = Mathf.Max(num, value.minThreatPoints.Value);
				}
				pawnGroupMakerParms.points = IncidentWorker_Raid.AdjustedRaidPoints(num, PawnsArrivalModeDefOf.EdgeWalkIn, RaidStrategyDefOf.ImmediateAttack, pawnGroupMakerParms.faction, PawnGroupKindDefOf.Combat);
				IEnumerable<PawnKindDef> pawnKinds = PawnGroupMakerUtility.GeneratePawnKindsExample(pawnGroupMakerParms);
				slate.Set<string>(this.storeThreatExampleAs.GetValue(slate), PawnUtility.PawnKindsToLineList(pawnKinds, "  - ", ColoredText.ThreatColor), false);
			}
		}

		// Token: 0x04005667 RID: 22119
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x04005668 RID: 22120
		[NoTranslate]
		public SlateRef<string> inSignalDisable;

		// Token: 0x04005669 RID: 22121
		[NoTranslate]
		public SlateRef<string> storeThreatExampleAs;

		// Token: 0x0400566A RID: 22122
		[NoTranslate]
		public SlateRef<int> threatStartTicks;

		// Token: 0x0400566B RID: 22123
		public SlateRef<ThreatsGeneratorParams> parms;

		// Token: 0x0400566C RID: 22124
		public SlateRef<Faction> faction;
	}
}
