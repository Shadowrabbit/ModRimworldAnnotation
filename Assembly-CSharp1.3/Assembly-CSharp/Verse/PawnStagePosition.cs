using System;

namespace Verse
{
	// Token: 0x02000051 RID: 81
	public class PawnStagePosition : IExposable
	{
		// Token: 0x060003D2 RID: 978 RVA: 0x00014E6A File Offset: 0x0001306A
		public PawnStagePosition()
		{
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x00014E7D File Offset: 0x0001307D
		public PawnStagePosition(IntVec3 cell, Thing thing, Rot4 orientation, bool highlight)
		{
			this.cell = cell;
			this.thing = thing;
			this.orientation = orientation;
			this.highlight = highlight;
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x00014EB0 File Offset: 0x000130B0
		public void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.cell, "cell", default(IntVec3), false);
			Scribe_Values.Look<Rot4>(ref this.orientation, "orientation", Rot4.Invalid, false);
			Scribe_Values.Look<bool>(ref this.highlight, "highlight", false, false);
			Scribe_References.Look<Thing>(ref this.thing, "thing", false);
		}

		// Token: 0x0400011D RID: 285
		public IntVec3 cell;

		// Token: 0x0400011E RID: 286
		public Thing thing;

		// Token: 0x0400011F RID: 287
		public Rot4 orientation = Rot4.Invalid;

		// Token: 0x04000120 RID: 288
		public bool highlight;
	}
}
