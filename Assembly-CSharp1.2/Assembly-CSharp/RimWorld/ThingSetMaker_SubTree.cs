using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001746 RID: 5958
	public class ThingSetMaker_SubTree : ThingSetMaker
	{
		// Token: 0x0600835D RID: 33629 RVA: 0x0005831C File Offset: 0x0005651C
		protected override bool CanGenerateSub(ThingSetMakerParams parms)
		{
			return this.def.root.CanGenerate(parms);
		}

		// Token: 0x0600835E RID: 33630 RVA: 0x0005832F File Offset: 0x0005652F
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			outThings.AddRange(this.def.root.Generate(parms));
		}

		// Token: 0x0600835F RID: 33631 RVA: 0x00058348 File Offset: 0x00056548
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			return this.def.root.AllGeneratableThingsDebug(parms);
		}

		// Token: 0x04005524 RID: 21796
		public ThingSetMakerDef def;
	}
}
