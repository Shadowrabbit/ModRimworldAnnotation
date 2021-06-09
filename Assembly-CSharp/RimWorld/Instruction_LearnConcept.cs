using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BD3 RID: 7123
	public class Instruction_LearnConcept : Lesson_Instruction
	{
		// Token: 0x1700189D RID: 6301
		// (get) Token: 0x06009CCA RID: 40138 RVA: 0x0006838D File Offset: 0x0006658D
		protected override float ProgressPercent
		{
			get
			{
				return PlayerKnowledgeDatabase.GetKnowledge(this.def.concept);
			}
		}

		// Token: 0x06009CCB RID: 40139 RVA: 0x0006839F File Offset: 0x0006659F
		public override void OnActivated()
		{
			PlayerKnowledgeDatabase.SetKnowledge(this.def.concept, 0f);
			base.OnActivated();
		}

		// Token: 0x06009CCC RID: 40140 RVA: 0x000683BC File Offset: 0x000665BC
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
