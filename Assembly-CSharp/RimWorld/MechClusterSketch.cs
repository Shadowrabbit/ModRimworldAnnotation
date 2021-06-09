using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001321 RID: 4897
	public class MechClusterSketch : IExposable
	{
		// Token: 0x060069EC RID: 27116 RVA: 0x00006B8B File Offset: 0x00004D8B
		public MechClusterSketch()
		{
		}

		// Token: 0x060069ED RID: 27117 RVA: 0x00048284 File Offset: 0x00046484
		public MechClusterSketch(Sketch buildingsSketch, List<MechClusterSketch.Mech> pawns, bool startDormant)
		{
			this.buildingsSketch = buildingsSketch;
			this.pawns = pawns;
			this.startDormant = startDormant;
		}

		// Token: 0x060069EE RID: 27118 RVA: 0x000482A1 File Offset: 0x000464A1
		public void ExposeData()
		{
			Scribe_Deep.Look<Sketch>(ref this.buildingsSketch, "buildingsSketch", Array.Empty<object>());
			Scribe_Collections.Look<MechClusterSketch.Mech>(ref this.pawns, "pawns", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.startDormant, "startDormant", false, false);
		}

		// Token: 0x0400468B RID: 18059
		public Sketch buildingsSketch;

		// Token: 0x0400468C RID: 18060
		public List<MechClusterSketch.Mech> pawns;

		// Token: 0x0400468D RID: 18061
		public bool startDormant;

		// Token: 0x02001322 RID: 4898
		public struct Mech : IExposable
		{
			// Token: 0x060069EF RID: 27119 RVA: 0x000482E0 File Offset: 0x000464E0
			public Mech(PawnKindDef kindDef)
			{
				this.kindDef = kindDef;
				this.position = IntVec3.Invalid;
			}

			// Token: 0x060069F0 RID: 27120 RVA: 0x0020A5B8 File Offset: 0x002087B8
			public void ExposeData()
			{
				Scribe_Defs.Look<PawnKindDef>(ref this.kindDef, "kindDef");
				Scribe_Values.Look<IntVec3>(ref this.position, "position", default(IntVec3), false);
			}

			// Token: 0x0400468E RID: 18062
			public PawnKindDef kindDef;

			// Token: 0x0400468F RID: 18063
			public IntVec3 position;
		}
	}
}
