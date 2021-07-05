using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016A8 RID: 5800
	public class QuestNode_AddContentsToShuttle : QuestNode
	{
		// Token: 0x060086AA RID: 34474 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060086AB RID: 34475 RVA: 0x00304240 File Offset: 0x00302440
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.contents.GetValue(slate) == null)
			{
				return;
			}
			QuestPart_AddContentsToShuttle questPart_AddContentsToShuttle = new QuestPart_AddContentsToShuttle();
			questPart_AddContentsToShuttle.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			questPart_AddContentsToShuttle.shuttle = this.shuttle.GetValue(slate);
			questPart_AddContentsToShuttle.Things = this.contents.GetValue(slate);
			QuestGen.quest.AddPart(questPart_AddContentsToShuttle);
		}

		// Token: 0x04005471 RID: 21617
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04005472 RID: 21618
		public SlateRef<Thing> shuttle;

		// Token: 0x04005473 RID: 21619
		public SlateRef<IEnumerable<Thing>> contents;
	}
}
