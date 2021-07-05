using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011F7 RID: 4599
	public abstract class CompAutoPowered : ThingComp
	{
		// Token: 0x17001334 RID: 4916
		// (get) Token: 0x06006EAA RID: 28330
		public abstract bool WantsToBeOn { get; }

		// Token: 0x04003D46 RID: 15686
		public const string AutoPoweredWantsOffSignal = "AutoPoweredWantsOff";
	}
}
