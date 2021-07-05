using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020010DD RID: 4317
	public class ThingSetMaker_SubTree : ThingSetMaker
	{
		// Token: 0x06006744 RID: 26436 RVA: 0x0022E276 File Offset: 0x0022C476
		protected override bool CanGenerateSub(ThingSetMakerParams parms)
		{
			return this.def.root.CanGenerate(parms);
		}

		// Token: 0x06006745 RID: 26437 RVA: 0x0022E289 File Offset: 0x0022C489
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			outThings.AddRange(this.def.root.Generate(parms));
		}

		// Token: 0x06006746 RID: 26438 RVA: 0x0022E2A2 File Offset: 0x0022C4A2
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			return this.def.root.AllGeneratableThingsDebug(parms);
		}

		// Token: 0x04003A4B RID: 14923
		public ThingSetMakerDef def;
	}
}
