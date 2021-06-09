using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FF9 RID: 8185
	public class QuestNode_ManhunterPack : QuestNode
	{
		// Token: 0x0600AD6C RID: 44396 RVA: 0x00327AB0 File Offset: 0x00325CB0
		protected override bool TestRunInt(Slate slate)
		{
			PawnKindDef pawnKindDef;
			return Find.Storyteller.difficultyValues.allowViolentQuests && slate.Exists("map", false) && ManhunterPackIncidentUtility.TryFindManhunterAnimalKind(slate.Get<float>("points", 0f, false), slate.Get<Map>("map", null, false).Tile, out pawnKindDef);
		}

		// Token: 0x0600AD6D RID: 44397 RVA: 0x00327B10 File Offset: 0x00325D10
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

		// Token: 0x040076F5 RID: 30453
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x040076F6 RID: 30454
		public SlateRef<string> customLetterLabel;

		// Token: 0x040076F7 RID: 30455
		public SlateRef<string> customLetterText;

		// Token: 0x040076F8 RID: 30456
		public SlateRef<RulePack> customLetterLabelRules;

		// Token: 0x040076F9 RID: 30457
		public SlateRef<RulePack> customLetterTextRules;

		// Token: 0x040076FA RID: 30458
		public SlateRef<IntVec3?> walkInSpot;

		// Token: 0x040076FB RID: 30459
		public SlateRef<int> animalCount;

		// Token: 0x040076FC RID: 30460
		[NoTranslate]
		public SlateRef<string> tag;

		// Token: 0x040076FD RID: 30461
		private const string RootSymbol = "root";
	}
}
