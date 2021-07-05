using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CF4 RID: 3316
	public class MechClusterSketch : IExposable
	{
		// Token: 0x06004D1F RID: 19743 RVA: 0x000033AC File Offset: 0x000015AC
		public MechClusterSketch()
		{
		}

		// Token: 0x06004D20 RID: 19744 RVA: 0x0019C923 File Offset: 0x0019AB23
		public MechClusterSketch(Sketch buildingsSketch, List<MechClusterSketch.Mech> pawns, bool startDormant)
		{
			this.buildingsSketch = buildingsSketch;
			this.pawns = pawns;
			this.startDormant = startDormant;
		}

		// Token: 0x06004D21 RID: 19745 RVA: 0x0019C940 File Offset: 0x0019AB40
		public void ExposeData()
		{
			Scribe_Deep.Look<Sketch>(ref this.buildingsSketch, "buildingsSketch", Array.Empty<object>());
			Scribe_Collections.Look<MechClusterSketch.Mech>(ref this.pawns, "pawns", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.startDormant, "startDormant", false, false);
		}

		// Token: 0x04002EA8 RID: 11944
		public Sketch buildingsSketch;

		// Token: 0x04002EA9 RID: 11945
		public List<MechClusterSketch.Mech> pawns;

		// Token: 0x04002EAA RID: 11946
		public bool startDormant;

		// Token: 0x020021DF RID: 8671
		public struct Mech : IExposable
		{
			// Token: 0x0600C07A RID: 49274 RVA: 0x003D0A1C File Offset: 0x003CEC1C
			public Mech(PawnKindDef kindDef)
			{
				this.kindDef = kindDef;
				this.position = IntVec3.Invalid;
			}

			// Token: 0x0600C07B RID: 49275 RVA: 0x003D0A30 File Offset: 0x003CEC30
			public void ExposeData()
			{
				Scribe_Defs.Look<PawnKindDef>(ref this.kindDef, "kindDef");
				Scribe_Values.Look<IntVec3>(ref this.position, "position", default(IntVec3), false);
			}

			// Token: 0x0400814A RID: 33098
			public PawnKindDef kindDef;

			// Token: 0x0400814B RID: 33099
			public IntVec3 position;
		}
	}
}
