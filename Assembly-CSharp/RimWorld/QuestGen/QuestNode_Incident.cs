using System;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FF5 RID: 8181
	public class QuestNode_Incident : QuestNode
	{
		// Token: 0x0600AD60 RID: 44384 RVA: 0x0007086B File Offset: 0x0006EA6B
		protected override bool TestRunInt(Slate slate)
		{
			return slate.Exists("map", false);
		}

		// Token: 0x0600AD61 RID: 44385 RVA: 0x0032776C File Offset: 0x0032596C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Map target = QuestGen.slate.Get<Map>("map", null, false);
			float points = QuestGen.slate.Get<float>("points", 0f, false);
			QuestPart_Incident questPart_Incident = new QuestPart_Incident();
			questPart_Incident.incident = this.incidentDef.GetValue(slate);
			IncidentParms incidentParms = new IncidentParms();
			incidentParms.forced = true;
			incidentParms.target = target;
			incidentParms.points = points;
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
		}

		// Token: 0x040076E4 RID: 30436
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x040076E5 RID: 30437
		public SlateRef<IncidentDef> incidentDef;

		// Token: 0x040076E6 RID: 30438
		public SlateRef<string> customLetterLabel;

		// Token: 0x040076E7 RID: 30439
		public SlateRef<string> customLetterText;

		// Token: 0x040076E8 RID: 30440
		public SlateRef<RulePack> customLetterLabelRules;

		// Token: 0x040076E9 RID: 30441
		public SlateRef<RulePack> customLetterTextRules;

		// Token: 0x040076EA RID: 30442
		private const string RootSymbol = "root";
	}
}
