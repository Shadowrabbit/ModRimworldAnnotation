using System;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x0200171F RID: 5919
	public class QuestNode_Incident : QuestNode
	{
		// Token: 0x06008886 RID: 34950 RVA: 0x00306634 File Offset: 0x00304834
		protected override bool TestRunInt(Slate slate)
		{
			return slate.Exists("map", false);
		}

		// Token: 0x06008887 RID: 34951 RVA: 0x00310BA0 File Offset: 0x0030EDA0
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

		// Token: 0x04005670 RID: 22128
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04005671 RID: 22129
		public SlateRef<IncidentDef> incidentDef;

		// Token: 0x04005672 RID: 22130
		public SlateRef<string> customLetterLabel;

		// Token: 0x04005673 RID: 22131
		public SlateRef<string> customLetterText;

		// Token: 0x04005674 RID: 22132
		public SlateRef<RulePack> customLetterLabelRules;

		// Token: 0x04005675 RID: 22133
		public SlateRef<RulePack> customLetterTextRules;

		// Token: 0x04005676 RID: 22134
		private const string RootSymbol = "root";
	}
}
