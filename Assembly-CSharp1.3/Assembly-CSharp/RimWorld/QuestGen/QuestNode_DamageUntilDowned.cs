using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016B7 RID: 5815
	public class QuestNode_DamageUntilDowned : QuestNode
	{
		// Token: 0x060086D8 RID: 34520 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060086D9 RID: 34521 RVA: 0x00304BCC File Offset: 0x00302DCC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate) == null)
			{
				return;
			}
			QuestPart_DamageUntilDowned questPart_DamageUntilDowned = new QuestPart_DamageUntilDowned();
			questPart_DamageUntilDowned.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_DamageUntilDowned.pawns.AddRange(this.pawns.GetValue(slate));
			questPart_DamageUntilDowned.allowBleedingWounds = (this.allowBleedingWounds.GetValue(slate) ?? true);
			QuestGen.quest.AddPart(questPart_DamageUntilDowned);
		}

		// Token: 0x040054A7 RID: 21671
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x040054A8 RID: 21672
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x040054A9 RID: 21673
		public SlateRef<bool?> allowBleedingWounds;
	}
}
