using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02001722 RID: 5922
	public class QuestNode_Raid : QuestNode
	{
		// Token: 0x0600888F RID: 34959 RVA: 0x003111FA File Offset: 0x0030F3FA
		protected override bool TestRunInt(Slate slate)
		{
			return Find.Storyteller.difficulty.allowViolentQuests && slate.Exists("map", false) && slate.Exists("enemyFaction", false);
		}

		// Token: 0x06008890 RID: 34960 RVA: 0x00311230 File Offset: 0x0030F430
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Map target = QuestGen.slate.Get<Map>("map", null, false);
			float a = QuestGen.slate.Get<float>("points", 0f, false);
			Faction faction = QuestGen.slate.Get<Faction>("enemyFaction", null, false);
			QuestPart_Incident questPart_Incident = new QuestPart_Incident();
			questPart_Incident.debugLabel = "raid";
			questPart_Incident.incident = IncidentDefOf.RaidEnemy;
			IncidentParms incidentParms = new IncidentParms();
			incidentParms.forced = true;
			incidentParms.target = target;
			incidentParms.points = Mathf.Max(a, faction.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat, null));
			incidentParms.faction = faction;
			incidentParms.pawnGroupMakerSeed = new int?(Rand.Int);
			incidentParms.inSignalEnd = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalLeave.GetValue(slate));
			incidentParms.questTag = QuestGenUtility.HardcodedTargetQuestTagWithQuestID(this.tag.GetValue(slate));
			incidentParms.canTimeoutOrFlee = (this.canTimeoutOrFlee.GetValue(slate) ?? true);
			if (this.raidPawnKind.GetValue(slate) != null)
			{
				incidentParms.pawnKind = this.raidPawnKind.GetValue(slate);
				incidentParms.pawnCount = Mathf.Max(1, Mathf.RoundToInt(incidentParms.points / incidentParms.pawnKind.combatPower));
			}
			if (this.arrivalMode.GetValue(slate) != null)
			{
				incidentParms.raidArrivalMode = this.arrivalMode.GetValue(slate);
			}
			if (!this.customLetterLabel.GetValue(slate).NullOrEmpty() || this.customLetterLabelRules.GetValue(slate) != null)
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					incidentParms.customLetterLabel = x;
				}, QuestGenUtility.MergeRules(this.customLetterLabelRules.GetValue(slate), this.customLetterLabel.GetValue(slate), "root"));
			}
			if (!this.customLetterText.GetValue(slate).NullOrEmpty() || this.customLetterTextRules.GetValue(slate) != null)
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					incidentParms.customLetterText = x;
				}, QuestGenUtility.MergeRules(this.customLetterTextRules.GetValue(slate), this.customLetterText.GetValue(slate), "root"));
			}
			IncidentWorker_Raid incidentWorker_Raid = (IncidentWorker_Raid)questPart_Incident.incident.Worker;
			incidentWorker_Raid.ResolveRaidStrategy(incidentParms, PawnGroupKindDefOf.Combat);
			incidentWorker_Raid.ResolveRaidArriveMode(incidentParms);
			if (incidentParms.raidArrivalMode.walkIn)
			{
				incidentParms.spawnCenter = (this.walkInSpot.GetValue(slate) ?? (QuestGen.slate.Get<IntVec3?>("walkInSpot", null, false) ?? IntVec3.Invalid));
			}
			else
			{
				incidentParms.spawnCenter = (this.dropSpot.GetValue(slate) ?? (QuestGen.slate.Get<IntVec3?>("dropSpot", null, false) ?? IntVec3.Invalid));
			}
			PawnGroupMakerParms defaultPawnGroupMakerParms = IncidentParmsUtility.GetDefaultPawnGroupMakerParms(PawnGroupKindDefOf.Combat, incidentParms, false);
			defaultPawnGroupMakerParms.points = IncidentWorker_Raid.AdjustedRaidPoints(defaultPawnGroupMakerParms.points, incidentParms.raidArrivalMode, incidentParms.raidStrategy, defaultPawnGroupMakerParms.faction, PawnGroupKindDefOf.Combat);
			IEnumerable<PawnKindDef> pawnKinds = PawnGroupMakerUtility.GeneratePawnKindsExample(defaultPawnGroupMakerParms);
			questPart_Incident.SetIncidentParmsAndRemoveTarget(incidentParms);
			questPart_Incident.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			QuestGen.quest.AddPart(questPart_Incident);
			QuestGen.AddQuestDescriptionRules(new List<Rule>
			{
				new Rule_String("raidPawnKinds", PawnUtility.PawnKindsToLineList(pawnKinds, "  - ", ColoredText.ThreatColor)),
				new Rule_String("raidArrivalModeInfo", incidentParms.raidArrivalMode.textWillArrive.Formatted(faction))
			});
		}

		// Token: 0x04005688 RID: 22152
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04005689 RID: 22153
		public SlateRef<IntVec3?> walkInSpot;

		// Token: 0x0400568A RID: 22154
		public SlateRef<IntVec3?> dropSpot;

		// Token: 0x0400568B RID: 22155
		public SlateRef<string> customLetterLabel;

		// Token: 0x0400568C RID: 22156
		public SlateRef<string> customLetterText;

		// Token: 0x0400568D RID: 22157
		public SlateRef<RulePack> customLetterLabelRules;

		// Token: 0x0400568E RID: 22158
		public SlateRef<RulePack> customLetterTextRules;

		// Token: 0x0400568F RID: 22159
		public SlateRef<PawnsArrivalModeDef> arrivalMode;

		// Token: 0x04005690 RID: 22160
		public SlateRef<PawnKindDef> raidPawnKind;

		// Token: 0x04005691 RID: 22161
		public SlateRef<bool?> canTimeoutOrFlee;

		// Token: 0x04005692 RID: 22162
		[NoTranslate]
		public SlateRef<string> inSignalLeave;

		// Token: 0x04005693 RID: 22163
		[NoTranslate]
		public SlateRef<string> tag;

		// Token: 0x04005694 RID: 22164
		private const string RootSymbol = "root";
	}
}
