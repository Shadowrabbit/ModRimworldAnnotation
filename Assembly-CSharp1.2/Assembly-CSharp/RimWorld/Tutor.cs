using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BE8 RID: 7144
	public class Tutor : IExposable
	{
		// Token: 0x06009D3A RID: 40250 RVA: 0x002DFCF4 File Offset: 0x002DDEF4
		public void ExposeData()
		{
			Scribe_Deep.Look<ActiveLessonHandler>(ref this.activeLesson, "activeLesson", Array.Empty<object>());
			Scribe_Deep.Look<LearningReadout>(ref this.learningReadout, "learningReadout", Array.Empty<object>());
			Scribe_Deep.Look<TutorialState>(ref this.tutorialState, "tutorialState", Array.Empty<object>());
		}

		// Token: 0x06009D3B RID: 40251 RVA: 0x00068A4F File Offset: 0x00066C4F
		internal void TutorUpdate()
		{
			this.activeLesson.ActiveLessonUpdate();
			this.learningReadout.LearningReadoutUpdate();
		}

		// Token: 0x06009D3C RID: 40252 RVA: 0x00068A67 File Offset: 0x00066C67
		internal void TutorOnGUI()
		{
			this.activeLesson.ActiveLessonOnGUI();
			this.learningReadout.LearningReadoutOnGUI();
		}

		// Token: 0x04006410 RID: 25616
		public ActiveLessonHandler activeLesson = new ActiveLessonHandler();

		// Token: 0x04006411 RID: 25617
		public LearningReadout learningReadout = new LearningReadout();

		// Token: 0x04006412 RID: 25618
		public TutorialState tutorialState = new TutorialState();
	}
}
