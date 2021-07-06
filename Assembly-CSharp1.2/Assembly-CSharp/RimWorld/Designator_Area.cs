using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001988 RID: 6536
	public abstract class Designator_Area : Designator
	{
		// Token: 0x0600908E RID: 37006 RVA: 0x00060FB9 File Offset: 0x0005F1B9
		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableCells(this, dragCells);
		}
	}
}
