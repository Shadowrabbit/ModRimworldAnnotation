using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BCE RID: 7118
	public abstract class Instruction_ExpandArea : Lesson_Instruction
	{
		// Token: 0x17001898 RID: 6296
		// (get) Token: 0x06009CB9 RID: 40121
		protected abstract Area MyArea { get; }

		// Token: 0x17001899 RID: 6297
		// (get) Token: 0x06009CBA RID: 40122 RVA: 0x000682E2 File Offset: 0x000664E2
		protected override float ProgressPercent
		{
			get
			{
				return (float)(this.MyArea.TrueCount - this.startingAreaCount) / (float)this.def.targetCount;
			}
		}

		// Token: 0x06009CBB RID: 40123 RVA: 0x00068304 File Offset: 0x00066504
		public override void OnActivated()
		{
			base.OnActivated();
			this.startingAreaCount = this.MyArea.TrueCount;
		}

		// Token: 0x06009CBC RID: 40124 RVA: 0x0006831D File Offset: 0x0006651D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.startingAreaCount, "startingAreaCount", 0, false);
		}

		// Token: 0x06009CBD RID: 40125 RVA: 0x000681B2 File Offset: 0x000663B2
		public override void LessonUpdate()
		{
			if (this.ProgressPercent > 0.999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}

		// Token: 0x040063D8 RID: 25560
		private int startingAreaCount = -1;
	}
}
