using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F8A RID: 8074
	public class QuestNode_DamageUntilDowned : QuestNode
	{
		// Token: 0x0600ABD7 RID: 43991 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600ABD8 RID: 43992 RVA: 0x00320388 File Offset: 0x0031E588
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

		// Token: 0x04007525 RID: 29989
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04007526 RID: 29990
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x04007527 RID: 29991
		public SlateRef<bool?> allowBleedingWounds;
	}
}
