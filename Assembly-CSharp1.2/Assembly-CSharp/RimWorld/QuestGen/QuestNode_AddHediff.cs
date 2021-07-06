using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F7D RID: 8061
	public class QuestNode_AddHediff : QuestNode
	{
		// Token: 0x0600ABAF RID: 43951 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600ABB0 RID: 43952 RVA: 0x0031FBC4 File Offset: 0x0031DDC4
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

		// Token: 0x040074F7 RID: 29943
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x040074F8 RID: 29944
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x040074F9 RID: 29945
		public SlateRef<HediffDef> hediffDef;

		// Token: 0x040074FA RID: 29946
		public SlateRef<IEnumerable<BodyPartDef>> partsToAffect;

		// Token: 0x040074FB RID: 29947
		public SlateRef<bool> checkDiseaseContractChance;

		// Token: 0x040074FC RID: 29948
		public SlateRef<bool> addToHyperlinks;
	}
}
