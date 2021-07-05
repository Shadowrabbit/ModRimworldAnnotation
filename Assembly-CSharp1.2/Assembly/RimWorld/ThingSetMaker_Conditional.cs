using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200173D RID: 5949
	public abstract class ThingSetMaker_Conditional : ThingSetMaker
	{
		// Token: 0x0600833C RID: 33596 RVA: 0x000581FF File Offset: 0x000563FF
		protected override bool CanGenerateSub(ThingSetMakerParams parms)
		{
			return this.Condition(parms) && this.thingSetMaker.CanGenerate(parms);
		}

		// Token: 0x0600833D RID: 33597 RVA: 0x00058218 File Offset: 0x00056418
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			outThings.AddRange(this.thingSetMaker.Generate(parms));
		}

		// Token: 0x0600833E RID: 33598 RVA: 0x0005822C File Offset: 0x0005642C
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			return this.thingSetMaker.AllGeneratableThingsDebug(parms);
		}

		// Token: 0x0600833F RID: 33599 RVA: 0x0005823A File Offset: 0x0005643A
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			this.thingSetMaker.ResolveReferences();
		}

		// Token: 0x06008340 RID: 33600
		protected abstract bool Condition(ThingSetMakerParams parms);

		// Token: 0x0400550B RID: 21771
		public ThingSetMaker thingSetMaker;
	}
}
