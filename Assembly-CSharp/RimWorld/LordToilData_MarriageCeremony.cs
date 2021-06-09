using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000E19 RID: 3609
	public class LordToilData_MarriageCeremony : LordToilData
	{
		// Token: 0x060051F8 RID: 20984 RVA: 0x001BD278 File Offset: 0x001BB478
		public override void ExposeData()
		{
			Scribe_Values.Look<CellRect>(ref this.spectateRect, "spectateRect", default(CellRect), false);
			Scribe_Values.Look<SpectateRectSide>(ref this.spectateRectAllowedSides, "spectateRectAllowedSides", SpectateRectSide.None, false);
		}

		// Token: 0x04003479 RID: 13433
		public CellRect spectateRect;

		// Token: 0x0400347A RID: 13434
		public SpectateRectSide spectateRectAllowedSides = SpectateRectSide.All;
	}
}
