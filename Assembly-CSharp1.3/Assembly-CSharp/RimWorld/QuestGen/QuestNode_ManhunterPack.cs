using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02001721 RID: 5921
	public class QuestNode_ManhunterPack : QuestNode
	{
		// Token: 0x0600888C RID: 34956 RVA: 0x00310EEC File Offset: 0x0030F0EC
		protected override bool TestRunInt(Slate slate)
		{
			PawnKindDef pawnKindDef;
			return Find.Storyteller.difficulty.allowViolentQuests && slate.Exists("map", false) && ManhunterPackIncidentUtility.TryFindManhunterAnimalKind(slate.Get<float>("points", 0f, false), slate.Get<Map>("map", null, false).Tile, out pawnKindDef);
		}

		// Token: 0x0600888D RID: 34957 RVA: 0x00310F4C File Offset: 0x0030F14C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Map map = QuestGen.slate.Get<Map>("map", null, false);
			float points = QuestGen.slate.Get<float>("points", 0f, false);
			QuestPart_Incident questPart_Incident = new QuestPart_Incident();
			questPart_Incident.incident = IncidentDefOf.ManhunterPack;
			IncidentParms incidentParms = new IncidentParms();
			incidentParms.forced = true;
			incidentParms.target = map;
			incidentParms.points = points;
			incidentParms.questTag = QuestGenUtility.HardcodedTargetQuestTagWithQuestID(this.tag.GetValue(slate));
			incidentParms.spawnCenter = (this.walkInSpot.GetValue(slate) ?? (QuestGen.slate.Get<IntVec3?>("walkInSpot", null, false) ?? IntVec3.Invalid));
			incidentParms.pawnCount = this.animalCount.GetValue(slate);
			PawnKindDef pawnKindDef;
			if (ManhunterPackIncidentUtility.TryFindManhunterAnimalKind(points, map.Tile, out pawnKindDef))
			{
				incidentParms.pawnKind = pawnKindDef;
			}
			slate.Set<PawnKindDef>("animalKindDef", pawnKindDef, false);
			int num = (incidentParms.pawnCount > 0) ? incidentParms.pawnCount : ManhunterPackIncidentUtility.GetAnimalsCount(pawnKindDef, points);
			QuestGen.slate.Set<int>("animalCount", num, false);
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
			questPart_Incident.SetIncidentParmsAndRemoveTarget(incidentParms);
			questPart_Incident.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			QuestGen.quest.AddPart(questPart_Incident);
			List<Rule> list = new List<Rule>();
			list.Add(new Rule_String("animalKind_label", pawnKindDef.label));
			list.Add(new Rule_String("animalKind_labelPlural", pawnKindDef.GetLabelPlural(num)));
			QuestGen.AddQuestDescriptionRules(list);
			QuestGen.AddQuestNameRules(list);
		}

		// Token: 0x0400567F RID: 22143
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04005680 RID: 22144
		public SlateRef<string> customLetterLabel;

		// Token: 0x04005681 RID: 22145
		public SlateRef<string> customLetterText;

		// Token: 0x04005682 RID: 22146
		public SlateRef<RulePack> customLetterLabelRules;

		// Token: 0x04005683 RID: 22147
		public SlateRef<RulePack> customLetterTextRules;

		// Token: 0x04005684 RID: 22148
		public SlateRef<IntVec3?> walkInSpot;

		// Token: 0x04005685 RID: 22149
		public SlateRef<int> animalCount;

		// Token: 0x04005686 RID: 22150
		[NoTranslate]
		public SlateRef<string> tag;

		// Token: 0x04005687 RID: 22151
		private const string RootSymbol = "root";
	}
}
