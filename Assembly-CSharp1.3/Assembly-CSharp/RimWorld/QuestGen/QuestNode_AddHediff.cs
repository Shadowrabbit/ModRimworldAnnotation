using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016A9 RID: 5801
	public class QuestNode_AddHediff : QuestNode
	{
		// Token: 0x060086AD RID: 34477 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060086AE RID: 34478 RVA: 0x003042C0 File Offset: 0x003024C0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate) == null || this.hediffDef.GetValue(slate) == null)
			{
				return;
			}
			QuestPart_AddHediff questPart_AddHediff = new QuestPart_AddHediff();
			questPart_AddHediff.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_AddHediff.hediffDef = this.hediffDef.GetValue(slate);
			questPart_AddHediff.pawns.AddRange(this.pawns.GetValue(slate));
			questPart_AddHediff.checkDiseaseContractChance = this.checkDiseaseContractChance.GetValue(slate);
			if (this.partsToAffect.GetValue(slate) != null)
			{
				questPart_AddHediff.partsToAffect = new List<BodyPartDef>();
				questPart_AddHediff.partsToAffect.AddRange(this.partsToAffect.GetValue(slate));
			}
			questPart_AddHediff.addToHyperlinks = this.addToHyperlinks.GetValue(slate);
			QuestGen.quest.AddPart(questPart_AddHediff);
		}

		// Token: 0x04005474 RID: 21620
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04005475 RID: 21621
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x04005476 RID: 21622
		public SlateRef<HediffDef> hediffDef;

		// Token: 0x04005477 RID: 21623
		public SlateRef<IEnumerable<BodyPartDef>> partsToAffect;

		// Token: 0x04005478 RID: 21624
		public SlateRef<bool> checkDiseaseContractChance;

		// Token: 0x04005479 RID: 21625
		public SlateRef<bool> addToHyperlinks;
	}
}
