using System;
using System.Collections.Generic;
using UnityEngine;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x02000302 RID: 770
	public class StencilDrawerForCells : IExposable
	{
		// Token: 0x0600163F RID: 5695 RVA: 0x00081ADC File Offset: 0x0007FCDC
		public void Draw()
		{
			if (this.cells.NullOrEmpty<IntVec3>())
			{
				GenDraw.DrawStencilCell(this.center, GenDraw.RitualStencilMat, (float)this.dimensionsIfNoCells.x, (float)this.dimensionsIfNoCells.z);
				return;
			}
			foreach (IntVec3 intVec in this.cells)
			{
				GenDraw.DrawStencilCell(intVec.ToVector3Shifted(), GenDraw.RitualStencilMat, 1f, 1f);
			}
		}

		// Token: 0x06001640 RID: 5696 RVA: 0x00081B7C File Offset: 0x0007FD7C
		public void ExposeData()
		{
			Scribe_References.Look<Lord>(ref this.sourceLord, "sourceLord", false);
			Scribe_Collections.Look<IntVec3>(ref this.cells, "cells", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<Vector3>(ref this.center, "center", default(Vector3), false);
			Scribe_Values.Look<IntVec2>(ref this.dimensionsIfNoCells, "dimensionsIfNoCells", default(IntVec2), false);
			Scribe_Values.Look<int>(ref this.ticksLeftWithoutLord, "ticksLeftWithoutLord", 0, false);
		}

		// Token: 0x04000F7C RID: 3964
		public Lord sourceLord;

		// Token: 0x04000F7D RID: 3965
		public List<IntVec3> cells;

		// Token: 0x04000F7E RID: 3966
		public Vector3 center;

		// Token: 0x04000F7F RID: 3967
		public IntVec2 dimensionsIfNoCells;

		// Token: 0x04000F80 RID: 3968
		public int ticksLeftWithoutLord;
	}
}
