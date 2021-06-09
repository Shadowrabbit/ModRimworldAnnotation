using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BBB RID: 7099
	public class ActiveLessonHandler : IExposable
	{
		// Token: 0x17001886 RID: 6278
		// (get) Token: 0x06009C48 RID: 40008 RVA: 0x00067D87 File Offset: 0x00065F87
		public Lesson Current
		{
			get
			{
				return this.activeLesson;
			}
		}

		// Token: 0x17001887 RID: 6279
		// (get) Token: 0x06009C49 RID: 40009 RVA: 0x00067D8F File Offset: 0x00065F8F
		public bool ActiveLessonVisible
		{
			get
			{
				return this.activeLesson != null && !Find.WindowStack.WindowsPreventDrawTutor;
			}
		}

		// Token: 0x06009C4A RID: 40010 RVA: 0x00067DA8 File Offset: 0x00065FA8
		public void ExposeData()
		{
			Scribe_Deep.Look<Lesson>(ref this.activeLesson, "activeLesson", Array.Empty<object>());
		}

		// Token: 0x06009C4B RID: 40011 RVA: 0x002DDBD0 File Offset: 0x002DBDD0
		public void Activate(InstructionDef id)
		{
			Lesson_Instruction lesson_Instruction = this.activeLesson as Lesson_Instruction;
			if (lesson_Instruction != null && id == lesson_Instruction.def)
			{
				return;
			}
			Lesson_Instruction lesson_Instruction2 = (Lesson_Instruction)Activator.CreateInstance(id.instructionClass);
			lesson_Instruction2.def = id;
			this.activeLesson = lesson_Instruction2;
			this.activeLesson.OnActivated();
		}

		// Token: 0x06009C4C RID: 40012 RVA: 0x002DDC20 File Offset: 0x002DBE20
		public void Activate(Lesson lesson)
		{
			Lesson_Note lesson_Note = lesson as Lesson_Note;
			if (lesson_Note != null && this.activeLesson != null)
			{
				lesson_Note.doFadeIn = false;
			}
			this.activeLesson = lesson;
			this.activeLesson.OnActivated();
		}

		// Token: 0x06009C4D RID: 40013 RVA: 0x002DDC58 File Offset: 0x002DBE58
		public void Deactivate()
		{
			Lesson lesson = this.activeLesson;
			this.activeLesson = null;
			if (lesson != null)
			{
				lesson.PostDeactivated();
			}
		}

		// Token: 0x06009C4E RID: 40014 RVA: 0x00067DBF File Offset: 0x00065FBF
		public void ActiveLessonOnGUI()
		{
			if (Time.timeSinceLevelLoad < 0.01f || !this.ActiveLessonVisible)
			{
				return;
			}
			this.activeLesson.LessonOnGUI();
		}

		// Token: 0x06009C4F RID: 40015 RVA: 0x00067DE1 File Offset: 0x00065FE1
		public void ActiveLessonUpdate()
		{
			if (Time.timeSinceLevelLoad < 0.01f || !this.ActiveLessonVisible)
			{
				return;
			}
			this.activeLesson.LessonUpdate();
		}

		// Token: 0x06009C50 RID: 40016 RVA: 0x00067E03 File Offset: 0x00066003
		public void Notify_KnowledgeDemonstrated(ConceptDef conc)
		{
			if (this.Current != null)
			{
				this.Current.Notify_KnowledgeDemonstrated(conc);
			}
		}

		// Token: 0x040063B6 RID: 25526
		private Lesson activeLesson;
	}
}
