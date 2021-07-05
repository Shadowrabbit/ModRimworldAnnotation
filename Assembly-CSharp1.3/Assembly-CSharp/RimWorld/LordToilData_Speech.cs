using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020008D5 RID: 2261
	public class LordToilData_Speech : LordToilData_Gathering
	{
		// Token: 0x06003B6A RID: 15210 RVA: 0x0014BE74 File Offset: 0x0014A074
		public override void ExposeData()
		{
			Scribe_Values.Look<CellRect>(ref this.spectateRect, "spectateRect", default(CellRect), false);
			Scribe_Values.Look<SpectateRectSide>(ref this.spectateRectAllowedSides, "spectateRectAllowedSides", SpectateRectSide.None, false);
			Scribe_Values.Look<SpectateRectSide>(ref this.spectateRectPreferredSide, "spectateRectPreferredSide", SpectateRectSide.None, false);
		}

		// Token: 0x04002060 RID: 8288
		public CellRect spectateRect;

		// Token: 0x04002061 RID: 8289
		public SpectateRectSide spectateRectAllowedSides = SpectateRectSide.All;

		// Token: 0x04002062 RID: 8290
		public SpectateRectSide spectateRectPreferredSide;
	}
}
