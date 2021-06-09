using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F8B RID: 8075
	public class QuestNode_DestroyOrPassToWorld : QuestNode
	{
		// Token: 0x0600ABDA RID: 43994 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600ABDB RID: 43995 RVA: 0x00320424 File Offset: 0x0031E624
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.things.GetValue(slate).EnumerableNullOrEmpty<Thing>())
			{
				return;
			}
			QuestPart_DestroyThingsOrPassToWorld questPart_DestroyThingsOrPassToWorld = new QuestPart_DestroyThingsOrPassToWorld();
			questPart_DestroyThingsOrPassToWorld.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_DestroyThingsOrPassToWorld.things.AddRange(this.things.GetValue(slate));
			QuestGen.quest.AddPart(questPart_DestroyThingsOrPassToWorld);
		}

		// Token: 0x04007528 RID: 29992
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04007529 RID: 29993
		public SlateRef<IEnumerable<Thing>> things;
	}
}
