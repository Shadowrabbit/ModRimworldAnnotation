using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020013DA RID: 5082
	public class Tutor : IExposable
	{
		// Token: 0x06007B98 RID: 31640 RVA: 0x002B9038 File Offset: 0x002B7238
		public void ExposeData()
		{
			Scribe_Deep.Look<ActiveLessonHandler>(ref this.activeLesson, "activeLesson", Array.Empty<object>());
			Scribe_Deep.Look<LearningReadout>(ref this.learningReadout, "learningReadout", Array.Empty<object>());
			Scribe_Deep.Look<TutorialState>(ref this.tutorialState, "tutorialState", Array.Empty<object>());
		}

		// Token: 0x06007B99 RID: 31641 RVA: 0x002B9084 File Offset: 0x002B7284
		internal void TutorUpdate()
		{
			this.activeLesson.ActiveLessonUpdate();
			this.learningReadout.LearningReadoutUpdate();
		}

		// Token: 0x06007B9A RID: 31642 RVA: 0x002B909C File Offset: 0x002B729C
		internal void TutorOnGUI()
		{
			this.activeLesson.ActiveLessonOnGUI();
			this.learningReadout.LearningReadoutOnGUI();
		}

		// Token: 0x04004463 RID: 17507
		public ActiveLessonHandler activeLesson = new ActiveLessonHandler();

		// Token: 0x04004464 RID: 17508
		public LearningReadout learningReadout = new LearningReadout();

		// Token: 0x04004465 RID: 17509
		public TutorialState tutorialState = new TutorialState();
	}
}
