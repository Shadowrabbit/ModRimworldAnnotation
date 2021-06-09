using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BD5 RID: 7125
	public class Instruction_PlaceGrowingZone : Lesson_Instruction
	{
		// Token: 0x06009CD6 RID: 40150 RVA: 0x002DF030 File Offset: 0x002DD230
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<CellRect>(ref this.growingZoneRect, "growingZoneRect", default(CellRect), false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.RecacheCells();
			}
		}

		// Token: 0x06009CD7 RID: 40151 RVA: 0x0006849B File Offset: 0x0006669B
		private void RecacheCells()
		{
			this.cachedCells = this.growingZoneRect.Cells.ToList<IntVec3>();
		}

		// Token: 0x06009CD8 RID: 40152 RVA: 0x000684B3 File Offset: 0x000666B3
		public override void OnActivated()
		{
			base.OnActivated();
			this.growingZoneRect = TutorUtility.FindUsableRect(10, 8, base.Map, 0.5f, false);
			this.RecacheCells();
		}

		// Token: 0x06009CD9 RID: 40153 RVA: 0x000684DB File Offset: 0x000666DB
		public override void LessonOnGUI()
		{
			TutorUtility.DrawCellRectOnGUI(this.growingZoneRect, this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x06009CDA RID: 40154 RVA: 0x000684F9 File Offset: 0x000666F9
		public override void LessonUpdate()
		{
			GenDraw.DrawFieldEdges(this.cachedCells);
			GenDraw.DrawArrowPointingAt(this.growingZoneRect.CenterVector3, false);
		}

		// Token: 0x06009CDB RID: 40155 RVA: 0x00068517 File Offset: 0x00066717
		public override AcceptanceReport AllowAction(EventPack ep)
		{
			if (ep.Tag == "Designate-ZoneAdd_Growing")
			{
				return TutorUtility.EventCellsMatchExactly(ep, this.cachedCells);
			}
			return base.AllowAction(ep);
		}

		// Token: 0x040063DD RID: 25565
		private CellRect growingZoneRect;

		// Token: 0x040063DE RID: 25566
		private List<IntVec3> cachedCells;
	}
}
