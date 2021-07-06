using System;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F9C RID: 8092
	public class QuestNode_GiveTechprints : QuestNode
	{
		// Token: 0x0600AC13 RID: 44051 RVA: 0x00320E20 File Offset: 0x0031F020
		protected override bool TestRunInt(Slate slate)
		{
			ResearchProjectDef researchProjectDef = this.FindTargetProject(slate);
			if (researchProjectDef == null || researchProjectDef.TechprintRequirementMet)
			{
				return false;
			}
			if (this.storeProjectAs.GetValue(slate) != null)
			{
				slate.Set<ResearchProjectDef>(this.storeProjectAs.GetValue(slate), researchProjectDef, false);
			}
			return true;
		}

		// Token: 0x0600AC14 RID: 44052 RVA: 0x00320E68 File Offset: 0x0031F068
		private ResearchProjectDef FindTargetProject(Slate slate)
		{
			if (this.fixedProject.GetValue(slate) != null)
			{
				return this.fixedProject.GetValue(slate);
			}
			return (from p in DefDatabase<ResearchProjectDef>.AllDefsListForReading
			where !p.IsFinished && !p.TechprintRequirementMet
			select p).RandomElement<ResearchProjectDef>();
		}

		// Token: 0x0600AC15 RID: 44053 RVA: 0x00320EC0 File Offset: 0x0031F0C0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			ResearchProjectDef researchProjectDef = this.FindTargetProject(slate);
			QuestPart_GiveTechprints questPart_GiveTechprints = new QuestPart_GiveTechprints();
			questPart_GiveTechprints.amount = 1;
			questPart_GiveTechprints.project = researchProjectDef;
			questPart_GiveTechprints.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_GiveTechprints.outSignalWasGiven = QuestGenUtility.HardcodedSignalWithQuestID("AddedTechprints");
			QuestGen.quest.AddPart(questPart_GiveTechprints);
			if (this.storeProjectAs.GetValue(slate) != null)
			{
				QuestGen.slate.Set<ResearchProjectDef>(this.storeProjectAs.GetValue(slate), researchProjectDef, false);
			}
		}

		// Token: 0x04007572 RID: 30066
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04007573 RID: 30067
		public SlateRef<ResearchProjectDef> fixedProject;

		// Token: 0x04007574 RID: 30068
		[NoTranslate]
		public SlateRef<string> storeProjectAs;
	}
}
