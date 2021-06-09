using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014C7 RID: 5319
	public class DrugPolicyEntry : IExposable
	{
		// Token: 0x06007297 RID: 29335 RVA: 0x002301A0 File Offset: 0x0022E3A0
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.drug, "drug");
			Scribe_Values.Look<bool>(ref this.allowedForAddiction, "allowedForAddiction", false, false);
			Scribe_Values.Look<bool>(ref this.allowedForJoy, "allowedForJoy", false, false);
			Scribe_Values.Look<bool>(ref this.allowScheduled, "allowScheduled", false, false);
			Scribe_Values.Look<float>(ref this.daysFrequency, "daysFrequency", 1f, false);
			Scribe_Values.Look<float>(ref this.onlyIfMoodBelow, "onlyIfMoodBelow", 1f, false);
			Scribe_Values.Look<float>(ref this.onlyIfJoyBelow, "onlyIfJoyBelow", 1f, false);
			Scribe_Values.Look<int>(ref this.takeToInventory, "takeToInventory", 0, false);
		}

		// Token: 0x04004B73 RID: 19315
		public ThingDef drug;

		// Token: 0x04004B74 RID: 19316
		public bool allowedForAddiction;

		// Token: 0x04004B75 RID: 19317
		public bool allowedForJoy;

		// Token: 0x04004B76 RID: 19318
		public bool allowScheduled;

		// Token: 0x04004B77 RID: 19319
		public float daysFrequency = 1f;

		// Token: 0x04004B78 RID: 19320
		public float onlyIfMoodBelow = 1f;

		// Token: 0x04004B79 RID: 19321
		public float onlyIfJoyBelow = 1f;

		// Token: 0x04004B7A RID: 19322
		public int takeToInventory;

		// Token: 0x04004B7B RID: 19323
		public string takeToInventoryTempBuffer;
	}
}
