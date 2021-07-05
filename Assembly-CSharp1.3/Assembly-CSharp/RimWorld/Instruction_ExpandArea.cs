using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020013C9 RID: 5065
	public abstract class Instruction_ExpandArea : Lesson_Instruction
	{
		// Token: 0x1700158E RID: 5518
		// (get) Token: 0x06007B2D RID: 31533
		protected abstract Area MyArea { get; }

		// Token: 0x1700158F RID: 5519
		// (get) Token: 0x06007B2E RID: 31534 RVA: 0x002B7D20 File Offset: 0x002B5F20
		protected override float ProgressPercent
		{
			get
			{
				return (float)(this.MyArea.TrueCount - this.startingAreaCount) / (float)this.def.targetCount;
			}
		}

		// Token: 0x06007B2F RID: 31535 RVA: 0x002B7D42 File Offset: 0x002B5F42
		public override void OnActivated()
		{
			base.OnActivated();
			this.startingAreaCount = this.MyArea.TrueCount;
		}

		// Token: 0x06007B30 RID: 31536 RVA: 0x002B7D5B File Offset: 0x002B5F5B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.startingAreaCount, "startingAreaCount", 0, false);
		}

		// Token: 0x06007B31 RID: 31537 RVA: 0x002B78B1 File Offset: 0x002B5AB1
		public override void LessonUpdate()
		{
			if (this.ProgressPercent > 0.999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}

		// Token: 0x0400443F RID: 17471
		private int startingAreaCount = -1;
	}
}
