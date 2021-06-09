using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F7C RID: 8060
	public class QuestNode_AddContentsToShuttle : QuestNode
	{
		// Token: 0x0600ABAC RID: 43948 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600ABAD RID: 43949 RVA: 0x0031FB44 File Offset: 0x0031DD44
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

		// Token: 0x040074F4 RID: 29940
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x040074F5 RID: 29941
		public SlateRef<Thing> shuttle;

		// Token: 0x040074F6 RID: 29942
		public SlateRef<IEnumerable<Thing>> contents;
	}
}
