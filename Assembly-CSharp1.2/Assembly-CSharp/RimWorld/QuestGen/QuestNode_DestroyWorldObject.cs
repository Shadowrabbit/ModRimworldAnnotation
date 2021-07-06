using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F8D RID: 8077
	public class QuestNode_DestroyWorldObject : QuestNode
	{
		// Token: 0x0600ABE0 RID: 44000 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600ABE1 RID: 44001 RVA: 0x003204F0 File Offset: 0x0031E6F0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_DestroyWorldObject questPart_DestroyWorldObject = new QuestPart_DestroyWorldObject();
			questPart_DestroyWorldObject.worldObject = this.worldObject.GetValue(slate);
			questPart_DestroyWorldObject.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			QuestGen.quest.AddPart(questPart_DestroyWorldObject);
		}

		// Token: 0x0400752B RID: 29995
		public SlateRef<WorldObject> worldObject;

		// Token: 0x0400752C RID: 29996
		[NoTranslate]
		public SlateRef<string> inSignal;
	}
}
