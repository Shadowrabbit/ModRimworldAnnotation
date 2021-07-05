using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016BC RID: 5820
	public class QuestNode_DropMonumentMarkerCopy : QuestNode
	{
		// Token: 0x060086E7 RID: 34535 RVA: 0x00304E1D File Offset: 0x0030301D
		protected override bool TestRunInt(Slate slate)
		{
			return slate.Exists("map", false);
		}

		// Token: 0x060086E8 RID: 34536 RVA: 0x00304E2C File Offset: 0x0030302C
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

		// Token: 0x040054B2 RID: 21682
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x040054B3 RID: 21683
		[NoTranslate]
		public SlateRef<string> outSignalResult;

		// Token: 0x040054B4 RID: 21684
		public SlateRef<bool> destroyOrPassToWorldOnCleanup;
	}
}
