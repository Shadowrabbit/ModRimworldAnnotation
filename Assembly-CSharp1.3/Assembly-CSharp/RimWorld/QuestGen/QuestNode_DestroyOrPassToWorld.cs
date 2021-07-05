using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016B8 RID: 5816
	public class QuestNode_DestroyOrPassToWorld : QuestNode
	{
		// Token: 0x060086DB RID: 34523 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060086DC RID: 34524 RVA: 0x00304C68 File Offset: 0x00302E68
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

		// Token: 0x040054AA RID: 21674
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x040054AB RID: 21675
		public SlateRef<IEnumerable<Thing>> things;
	}
}
