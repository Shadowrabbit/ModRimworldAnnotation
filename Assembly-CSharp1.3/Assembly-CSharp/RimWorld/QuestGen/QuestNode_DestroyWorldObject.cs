using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016BA RID: 5818
	public class QuestNode_DestroyWorldObject : QuestNode
	{
		// Token: 0x060086E1 RID: 34529 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060086E2 RID: 34530 RVA: 0x00304D34 File Offset: 0x00302F34
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_DestroyWorldObject questPart_DestroyWorldObject = new QuestPart_DestroyWorldObject();
			questPart_DestroyWorldObject.worldObject = this.worldObject.GetValue(slate);
			questPart_DestroyWorldObject.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			QuestGen.quest.AddPart(questPart_DestroyWorldObject);
		}

		// Token: 0x040054AD RID: 21677
		public SlateRef<WorldObject> worldObject;

		// Token: 0x040054AE RID: 21678
		[NoTranslate]
		public SlateRef<string> inSignal;
	}
}
