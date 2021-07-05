using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020010D7 RID: 4311
	public abstract class ThingSetMaker_Conditional : ThingSetMaker
	{
		// Token: 0x06006730 RID: 26416 RVA: 0x0022DF94 File Offset: 0x0022C194
		protected override bool CanGenerateSub(ThingSetMakerParams parms)
		{
			return this.Condition(parms) && this.thingSetMaker.CanGenerate(parms);
		}

		// Token: 0x06006731 RID: 26417 RVA: 0x0022DFAD File Offset: 0x0022C1AD
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			outThings.AddRange(this.thingSetMaker.Generate(parms));
		}

		// Token: 0x06006732 RID: 26418 RVA: 0x0022DFC1 File Offset: 0x0022C1C1
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			return this.thingSetMaker.AllGeneratableThingsDebug(parms);
		}

		// Token: 0x06006733 RID: 26419 RVA: 0x0022DFCF File Offset: 0x0022C1CF
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			this.thingSetMaker.ResolveReferences();
		}

		// Token: 0x06006734 RID: 26420
		protected abstract bool Condition(ThingSetMakerParams parms);

		// Token: 0x04003A40 RID: 14912
		public ThingSetMaker thingSetMaker;
	}
}
