using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013BB RID: 5051
	public class ActiveLessonHandler : IExposable
	{
		// Token: 0x1700157E RID: 5502
		// (get) Token: 0x06007AD4 RID: 31444 RVA: 0x002B680B File Offset: 0x002B4A0B
		public Lesson Current
		{
			get
			{
				return this.activeLesson;
			}
		}

		// Token: 0x1700157F RID: 5503
		// (get) Token: 0x06007AD5 RID: 31445 RVA: 0x002B6813 File Offset: 0x002B4A13
		public bool ActiveLessonVisible
		{
			get
			{
				return this.activeLesson != null && !Find.WindowStack.WindowsPreventDrawTutor;
			}
		}

		// Token: 0x06007AD6 RID: 31446 RVA: 0x002B682C File Offset: 0x002B4A2C
		public void ExposeData()
		{
			Scribe_Deep.Look<Lesson>(ref this.activeLesson, "activeLesson", Array.Empty<object>());
		}

		// Token: 0x06007AD7 RID: 31447 RVA: 0x002B6844 File Offset: 0x002B4A44
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

		// Token: 0x06007AD8 RID: 31448 RVA: 0x002B6894 File Offset: 0x002B4A94
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

		// Token: 0x06007AD9 RID: 31449 RVA: 0x002B68CC File Offset: 0x002B4ACC
		public void Deactivate()
		{
			Lesson lesson = this.activeLesson;
			this.activeLesson = null;
			if (lesson != null)
			{
				lesson.PostDeactivated();
			}
		}

		// Token: 0x06007ADA RID: 31450 RVA: 0x002B68F0 File Offset: 0x002B4AF0
		public void ActiveLessonOnGUI()
		{
			if (Time.timeSinceLevelLoad < 0.01f || !this.ActiveLessonVisible)
			{
				return;
			}
			this.activeLesson.LessonOnGUI();
		}

		// Token: 0x06007ADB RID: 31451 RVA: 0x002B6912 File Offset: 0x002B4B12
		public void ActiveLessonUpdate()
		{
			if (Time.timeSinceLevelLoad < 0.01f || !this.ActiveLessonVisible)
			{
				return;
			}
			this.activeLesson.LessonUpdate();
		}

		// Token: 0x06007ADC RID: 31452 RVA: 0x002B6934 File Offset: 0x002B4B34
		public void Notify_KnowledgeDemonstrated(ConceptDef conc)
		{
			if (this.Current != null)
			{
				this.Current.Notify_KnowledgeDemonstrated(conc);
			}
		}

		// Token: 0x0400442D RID: 17453
		private Lesson activeLesson;
	}
}
