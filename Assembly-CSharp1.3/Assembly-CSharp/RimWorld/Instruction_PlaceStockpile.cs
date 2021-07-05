using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020013D0 RID: 5072
	public class Instruction_PlaceStockpile : Lesson_Instruction
	{
		// Token: 0x06007B4E RID: 31566 RVA: 0x002B8224 File Offset: 0x002B6424
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<CellRect>(ref this.stockpileRect, "stockpileRect", default(CellRect), false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.RecacheCells();
			}
		}

		// Token: 0x06007B4F RID: 31567 RVA: 0x002B825F File Offset: 0x002B645F
		private void RecacheCells()
		{
			this.cachedCells = this.stockpileRect.Cells.ToList<IntVec3>();
		}

		// Token: 0x06007B50 RID: 31568 RVA: 0x002B8277 File Offset: 0x002B6477
		public override void OnActivated()
		{
			base.OnActivated();
			this.stockpileRect = TutorUtility.FindUsableRect(6, 6, base.Map, 0f, false);
			this.RecacheCells();
		}

		// Token: 0x06007B51 RID: 31569 RVA: 0x002B829E File Offset: 0x002B649E
		public override void LessonOnGUI()
		{
			TutorUtility.DrawCellRectOnGUI(this.stockpileRect, this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x06007B52 RID: 31570 RVA: 0x002B82BC File Offset: 0x002B64BC
		public override void LessonUpdate()
		{
			GenDraw.DrawFieldEdges(this.cachedCells);
			GenDraw.DrawArrowPointingAt(this.stockpileRect.CenterVector3, false);
		}

		// Token: 0x06007B53 RID: 31571 RVA: 0x002B82DA File Offset: 0x002B64DA
		public override AcceptanceReport AllowAction(EventPack ep)
		{
			if (ep.Tag == "Designate-ZoneAddStockpile_Resources")
			{
				return TutorUtility.EventCellsMatchExactly(ep, this.cachedCells);
			}
			return base.AllowAction(ep);
		}

		// Token: 0x04004444 RID: 17476
		private CellRect stockpileRect;

		// Token: 0x04004445 RID: 17477
		private List<IntVec3> cachedCells;
	}
}
