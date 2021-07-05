using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008D2 RID: 2258
	public class LordToilData_MarriageCeremony : LordToilData
	{
		// Token: 0x06003B61 RID: 15201 RVA: 0x0014BBF0 File Offset: 0x00149DF0
		public override void ExposeData()
		{
			Scribe_Values.Look<CellRect>(ref this.spectateRect, "spectateRect", default(CellRect), false);
			Scribe_Values.Look<SpectateRectSide>(ref this.spectateRectAllowedSides, "spectateRectAllowedSides", SpectateRectSide.None, false);
		}

		// Token: 0x0400205C RID: 8284
		public CellRect spectateRect;

		// Token: 0x0400205D RID: 8285
		public SpectateRectSide spectateRectAllowedSides = SpectateRectSide.All;
	}
}
