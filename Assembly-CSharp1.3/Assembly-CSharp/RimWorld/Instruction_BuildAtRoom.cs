using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020013C0 RID: 5056
	public abstract class Instruction_BuildAtRoom : Lesson_Instruction
	{
		// Token: 0x17001583 RID: 5507
		// (get) Token: 0x06007AF6 RID: 31478
		protected abstract CellRect BuildableRect { get; }

		// Token: 0x17001584 RID: 5508
		// (get) Token: 0x06007AF7 RID: 31479 RVA: 0x002B7065 File Offset: 0x002B5265
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

		// Token: 0x06007AF8 RID: 31480 RVA: 0x002B7090 File Offset: 0x002B5290
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

		// Token: 0x06007AF9 RID: 31481 RVA: 0x002B7100 File Offset: 0x002B5300
		public override void LessonOnGUI()
		{
			TutorUtility.DrawCellRectOnGUI(this.BuildableRect.ContractedBy(1), this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x06007AFA RID: 31482 RVA: 0x002B7134 File Offset: 0x002B5334
		public override void LessonUpdate()
		{
			GenDraw.DrawArrowPointingAt(this.BuildableRect.CenterVector3, true);
		}

		// Token: 0x06007AFB RID: 31483 RVA: 0x002B7158 File Offset: 0x002B5358
		public override AcceptanceReport AllowAction(EventPack ep)
		{
			if (ep.Tag == "Designate-" + this.def.thingDef.defName)
			{
				return this.AllowBuildAt(ep.Cell);
			}
			return base.AllowAction(ep);
		}

		// Token: 0x06007AFC RID: 31484 RVA: 0x002B71A8 File Offset: 0x002B53A8
		protected virtual bool AllowBuildAt(IntVec3 c)
		{
			return this.BuildableRect.Contains(c);
		}

		// Token: 0x06007AFD RID: 31485 RVA: 0x002B71C4 File Offset: 0x002B53C4
		public override void Notify_Event(EventPack ep)
		{
			if (this.NumPlaced() >= this.def.targetCount)
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}
