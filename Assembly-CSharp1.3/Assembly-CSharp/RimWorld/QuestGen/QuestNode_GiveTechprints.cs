using System;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016C6 RID: 5830
	public class QuestNode_GiveTechprints : QuestNode
	{
		// Token: 0x0600870D RID: 34573 RVA: 0x00305A00 File Offset: 0x00303C00
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

		// Token: 0x0600870E RID: 34574 RVA: 0x00305A48 File Offset: 0x00303C48
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

		// Token: 0x0600870F RID: 34575 RVA: 0x00305AA0 File Offset: 0x00303CA0
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

		// Token: 0x040054FF RID: 21759
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04005500 RID: 21760
		public SlateRef<ResearchProjectDef> fixedProject;

		// Token: 0x04005501 RID: 21761
		[NoTranslate]
		public SlateRef<string> storeProjectAs;
	}
}
