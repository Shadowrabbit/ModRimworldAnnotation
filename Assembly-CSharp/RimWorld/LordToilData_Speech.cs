using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000E1E RID: 3614
	public class LordToilData_Speech : LordToilData
	{
		// Token: 0x06005207 RID: 20999 RVA: 0x001BD528 File Offset: 0x001BB728
		public override void ExposeData()
		{
			Scribe_Values.Look<CellRect>(ref this.spectateRect, "spectateRect", default(CellRect), false);
			Scribe_Values.Look<SpectateRectSide>(ref this.spectateRectAllowedSides, "spectateRectAllowedSides", SpectateRectSide.None, false);
			Scribe_Values.Look<SpectateRectSide>(ref this.spectateRectPreferredSide, "spectateRectPreferredSide", SpectateRectSide.None, false);
		}

		// Token: 0x04003481 RID: 13441
		public CellRect spectateRect;

		// Token: 0x04003482 RID: 13442
		public SpectateRectSide spectateRectAllowedSides = SpectateRectSide.All;

		// Token: 0x04003483 RID: 13443
		public SpectateRectSide spectateRectPreferredSide;
	}
}
