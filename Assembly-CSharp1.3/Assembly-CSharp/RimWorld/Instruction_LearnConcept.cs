using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020013CD RID: 5069
	public class Instruction_LearnConcept : Lesson_Instruction
	{
		// Token: 0x17001593 RID: 5523
		// (get) Token: 0x06007B3B RID: 31547 RVA: 0x002B7EE7 File Offset: 0x002B60E7
		protected override float ProgressPercent
		{
			get
			{
				return PlayerKnowledgeDatabase.GetKnowledge(this.def.concept);
			}
		}

		// Token: 0x06007B3C RID: 31548 RVA: 0x002B7EF9 File Offset: 0x002B60F9
		public override void OnActivated()
		{
			PlayerKnowledgeDatabase.SetKnowledge(this.def.concept, 0f);
			base.OnActivated();
		}

		// Token: 0x06007B3D RID: 31549 RVA: 0x002B7F16 File Offset: 0x002B6116
		public override void LessonUpdate()
		{
			base.LessonUpdate();
			if (PlayerKnowledgeDatabase.IsComplete(this.def.concept))
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}
