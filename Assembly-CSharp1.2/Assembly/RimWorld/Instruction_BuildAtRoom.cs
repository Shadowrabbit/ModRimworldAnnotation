using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BC2 RID: 7106
	public abstract class Instruction_BuildAtRoom : Lesson_Instruction
	{
		// Token: 0x1700188D RID: 6285
		// (get) Token: 0x06009C77 RID: 40055
		protected abstract CellRect BuildableRect { get; }

		// Token: 0x1700188E RID: 6286
		// (get) Token: 0x06009C78 RID: 40056 RVA: 0x00067F6B File Offset: 0x0006616B
		protected override float ProgressPercent
		{
			get
			{
				if (this.def.targetCount <= 1)
				{
					return -1f;
				}
				return (float)this.NumPlaced() / (float)this.def.targetCount;
			}
		}

		// Token: 0x06009C79 RID: 40057 RVA: 0x002DE408 File Offset: 0x002DC608
		protected int NumPlaced()
		{
			int num = 0;
			using (CellRect.Enumerator enumerator = this.BuildableRect.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (TutorUtility.BuildingOrBlueprintOrFrameCenterExists(enumerator.Current, base.Map, this.def.thingDef))
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x06009C7A RID: 40058 RVA: 0x002DE478 File Offset: 0x002DC678
		public override void LessonOnGUI()
		{
			TutorUtility.DrawCellRectOnGUI(this.BuildableRect.ContractedBy(1), this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x06009C7B RID: 40059 RVA: 0x002DE4AC File Offset: 0x002DC6AC
		public override void LessonUpdate()
		{
			GenDraw.DrawArrowPointingAt(this.BuildableRect.CenterVector3, true);
		}

		// Token: 0x06009C7C RID: 40060 RVA: 0x002DE4D0 File Offset: 0x002DC6D0
		public override AcceptanceReport AllowAction(EventPack ep)
		{
			if (ep.Tag == "Designate-" + this.def.thingDef.defName)
			{
				return this.AllowBuildAt(ep.Cell);
			}
			return base.AllowAction(ep);
		}

		// Token: 0x06009C7D RID: 40061 RVA: 0x002DE520 File Offset: 0x002DC720
		protected virtual bool AllowBuildAt(IntVec3 c)
		{
			return this.BuildableRect.Contains(c);
		}

		// Token: 0x06009C7E RID: 40062 RVA: 0x00067F95 File Offset: 0x00066195
		public override void Notify_Event(EventPack ep)
		{
			if (this.NumPlaced() >= this.def.targetCount)
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}
