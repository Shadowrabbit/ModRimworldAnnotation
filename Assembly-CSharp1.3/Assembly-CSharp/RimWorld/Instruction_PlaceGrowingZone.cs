using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020013CF RID: 5071
	public class Instruction_PlaceGrowingZone : Lesson_Instruction
	{
		// Token: 0x06007B47 RID: 31559 RVA: 0x002B813C File Offset: 0x002B633C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<CellRect>(ref this.growingZoneRect, "growingZoneRect", default(CellRect), false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.RecacheCells();
			}
		}

		// Token: 0x06007B48 RID: 31560 RVA: 0x002B8177 File Offset: 0x002B6377
		private void RecacheCells()
		{
			this.cachedCells = this.growingZoneRect.Cells.ToList<IntVec3>();
		}

		// Token: 0x06007B49 RID: 31561 RVA: 0x002B818F File Offset: 0x002B638F
		public override void OnActivated()
		{
			base.OnActivated();
			this.growingZoneRect = TutorUtility.FindUsableRect(10, 8, base.Map, 0.5f, false);
			this.RecacheCells();
		}

		// Token: 0x06007B4A RID: 31562 RVA: 0x002B81B7 File Offset: 0x002B63B7
		public override void LessonOnGUI()
		{
			TutorUtility.DrawCellRectOnGUI(this.growingZoneRect, this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x06007B4B RID: 31563 RVA: 0x002B81D5 File Offset: 0x002B63D5
		public override void LessonUpdate()
		{
			GenDraw.DrawFieldEdges(this.cachedCells);
			GenDraw.DrawArrowPointingAt(this.growingZoneRect.CenterVector3, false);
		}

		// Token: 0x06007B4C RID: 31564 RVA: 0x002B81F3 File Offset: 0x002B63F3
		public override AcceptanceReport AllowAction(EventPack ep)
		{
			if (ep.Tag == "Designate-ZoneAdd_Growing")
			{
				return TutorUtility.EventCellsMatchExactly(ep, this.cachedCells);
			}
			return base.AllowAction(ep);
		}

		// Token: 0x04004442 RID: 17474
		private CellRect growingZoneRect;

		// Token: 0x04004443 RID: 17475
		private List<IntVec3> cachedCells;
	}
}
