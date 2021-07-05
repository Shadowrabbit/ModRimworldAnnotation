using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F61 RID: 3937
	public class RitualOutcomeComp_DataThingPresence : RitualOutcomeComp_Data
	{
		// Token: 0x06005D65 RID: 23909 RVA: 0x002004F4 File Offset: 0x001FE6F4
		public override void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.presentForTicks.RemoveAll((KeyValuePair<Thing, float> x) => x.Key.Destroyed);
			}
			Scribe_Collections.Look<Thing, float>(ref this.presentForTicks, "presentForTicks", LookMode.Reference, LookMode.Value, ref this.tmpPresentThing, ref this.tmpPresentTicks);
		}

		// Token: 0x040035FF RID: 13823
		public Dictionary<Thing, float> presentForTicks = new Dictionary<Thing, float>();

		// Token: 0x04003600 RID: 13824
		private List<Thing> tmpPresentThing;

		// Token: 0x04003601 RID: 13825
		private List<float> tmpPresentTicks;
	}
}
