using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BD6 RID: 7126
	public class Instruction_PlaceStockpile : Lesson_Instruction
	{
		// Token: 0x06009CDD RID: 40157 RVA: 0x002DF06C File Offset: 0x002DD26C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<CellRect>(ref this.stockpileRect, "stockpileRect", default(CellRect), false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.RecacheCells();
			}
		}

		// Token: 0x06009CDE RID: 40158 RVA: 0x00068545 File Offset: 0x00066745
		private void RecacheCells()
		{
			this.cachedCells = this.stockpileRect.Cells.ToList<IntVec3>();
		}

		// Token: 0x06009CDF RID: 40159 RVA: 0x0006855D File Offset: 0x0006675D
		public override void OnActivated()
		{
			base.OnActivated();
			this.stockpileRect = TutorUtility.FindUsableRect(6, 6, base.Map, 0f, false);
			this.RecacheCells();
		}

		// Token: 0x06009CE0 RID: 40160 RVA: 0x00068584 File Offset: 0x00066784
		public override void LessonOnGUI()
		{
			TutorUtility.DrawCellRectOnGUI(this.stockpileRect, this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x06009CE1 RID: 40161 RVA: 0x000685A2 File Offset: 0x000667A2
		public override void LessonUpdate()
		{
			GenDraw.DrawFieldEdges(this.cachedCells);
			GenDraw.DrawArrowPointingAt(this.stockpileRect.CenterVector3, false);
		}

		// Token: 0x06009CE2 RID: 40162 RVA: 0x000685C0 File Offset: 0x000667C0
		public override AcceptanceReport AllowAction(EventPack ep)
		{
			if (ep.Tag == "Designate-ZoneAddStockpile_Resources")
			{
				return TutorUtility.EventCellsMatchExactly(ep, this.cachedCells);
			}
			return base.AllowAction(ep);
		}

		// Token: 0x040063DF RID: 25567
		private CellRect stockpileRect;

		// Token: 0x040063E0 RID: 25568
		private List<IntVec3> cachedCells;
	}
}
