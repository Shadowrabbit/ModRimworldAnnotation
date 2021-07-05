using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200129A RID: 4762
	public abstract class Designator_Area : Designator
	{
		// Token: 0x060071CB RID: 29131 RVA: 0x00260D19 File Offset: 0x0025EF19
		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableCells(this, dragCells);
		}
	}
}
