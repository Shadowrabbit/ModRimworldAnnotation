using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F8F RID: 8079
	public class QuestNode_DropMonumentMarkerCopy : QuestNode
	{
		// Token: 0x0600ABE6 RID: 44006 RVA: 0x00070719 File Offset: 0x0006E919
		protected override bool TestRunInt(Slate slate)
		{
			return slate.Exists("map", false);
		}

		// Token: 0x0600ABE7 RID: 44007 RVA: 0x003205DC File Offset: 0x0031E7DC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_DropMonumentMarkerCopy questPart_DropMonumentMarkerCopy = new QuestPart_DropMonumentMarkerCopy();
			questPart_DropMonumentMarkerCopy.mapParent = slate.Get<Map>("map", null, false).Parent;
			questPart_DropMonumentMarkerCopy.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			questPart_DropMonumentMarkerCopy.outSignalResult = QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalResult.GetValue(slate));
			questPart_DropMonumentMarkerCopy.destroyOrPassToWorldOnCleanup = this.destroyOrPassToWorldOnCleanup.GetValue(slate);
			QuestGen.quest.AddPart(questPart_DropMonumentMarkerCopy);
		}

		// Token: 0x04007530 RID: 30000
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04007531 RID: 30001
		[NoTranslate]
		public SlateRef<string> outSignalResult;

		// Token: 0x04007532 RID: 30002
		public SlateRef<bool> destroyOrPassToWorldOnCleanup;
	}
}
