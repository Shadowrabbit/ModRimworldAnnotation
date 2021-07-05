using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016ED RID: 5869
	public class QuestNode_TrackWhenExitMentalState : QuestNode
	{
		// Token: 0x06008788 RID: 34696 RVA: 0x00307ADD File Offset: 0x00305CDD
		protected override bool TestRunInt(Slate slate)
		{
			return slate.Get<Map>("map", null, false) != null;
		}

		// Token: 0x06008789 RID: 34697 RVA: 0x00307AF4 File Offset: 0x00305CF4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_TrackWhenExitMentalState questPart_TrackWhenExitMentalState = new QuestPart_TrackWhenExitMentalState();
			questPart_TrackWhenExitMentalState.mapParent = slate.Get<Map>("map", null, false).Parent;
			questPart_TrackWhenExitMentalState.tag = QuestGenUtility.HardcodedTargetQuestTagWithQuestID(this.tag.GetValue(slate));
			questPart_TrackWhenExitMentalState.mentalStateDef = this.mentalStateDef.GetValue(slate);
			questPart_TrackWhenExitMentalState.outSignal = QuestGenUtility.HardcodedSignalWithQuestID(this.outSignal.GetValue(slate));
			questPart_TrackWhenExitMentalState.inSignals = new List<string>();
			foreach (string signal in this.inSignals.GetValue(slate))
			{
				questPart_TrackWhenExitMentalState.inSignals.Add(QuestGenUtility.HardcodedSignalWithQuestID(signal));
			}
			QuestGen.quest.AddPart(questPart_TrackWhenExitMentalState);
		}

		// Token: 0x040055A7 RID: 21927
		[NoTranslate]
		public SlateRef<string> tag;

		// Token: 0x040055A8 RID: 21928
		public SlateRef<MentalStateDef> mentalStateDef;

		// Token: 0x040055A9 RID: 21929
		[NoTranslate]
		public SlateRef<IEnumerable<string>> inSignals;

		// Token: 0x040055AA RID: 21930
		[NoTranslate]
		public SlateRef<string> outSignal;
	}
}
