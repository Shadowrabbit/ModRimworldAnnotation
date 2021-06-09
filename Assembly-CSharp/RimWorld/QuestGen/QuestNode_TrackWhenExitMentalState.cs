using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FC8 RID: 8136
	public class QuestNode_TrackWhenExitMentalState : QuestNode
	{
		// Token: 0x0600ACA5 RID: 44197 RVA: 0x00070AAE File Offset: 0x0006ECAE
		protected override bool TestRunInt(Slate slate)
		{
			return slate.Get<Map>("map", null, false) != null;
		}

		// Token: 0x0600ACA6 RID: 44198 RVA: 0x00322E00 File Offset: 0x00321000
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

		// Token: 0x04007624 RID: 30244
		[NoTranslate]
		public SlateRef<string> tag;

		// Token: 0x04007625 RID: 30245
		public SlateRef<MentalStateDef> mentalStateDef;

		// Token: 0x04007626 RID: 30246
		[NoTranslate]
		public SlateRef<IEnumerable<string>> inSignals;

		// Token: 0x04007627 RID: 30247
		[NoTranslate]
		public SlateRef<string> outSignal;
	}
}
