using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E2B RID: 3627
	public class DrugPolicyEntry : IExposable
	{
		// Token: 0x060053DC RID: 21468 RVA: 0x001C6128 File Offset: 0x001C4328
		public void CopyFrom(DrugPolicyEntry other)
		{
			this.drug = other.drug;
			this.allowedForAddiction = other.allowedForAddiction;
			this.allowedForJoy = other.allowedForJoy;
			this.allowScheduled = other.allowScheduled;
			this.daysFrequency = other.daysFrequency;
			this.onlyIfMoodBelow = other.onlyIfMoodBelow;
			this.onlyIfJoyBelow = other.onlyIfJoyBelow;
			this.takeToInventory = other.takeToInventory;
		}

		// Token: 0x060053DD RID: 21469 RVA: 0x001C6198 File Offset: 0x001C4398
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

		// Token: 0x04003157 RID: 12631
		public ThingDef drug;

		// Token: 0x04003158 RID: 12632
		public bool allowedForAddiction;

		// Token: 0x04003159 RID: 12633
		public bool allowedForJoy;

		// Token: 0x0400315A RID: 12634
		public bool allowScheduled;

		// Token: 0x0400315B RID: 12635
		public float daysFrequency = 1f;

		// Token: 0x0400315C RID: 12636
		public float onlyIfMoodBelow = 1f;

		// Token: 0x0400315D RID: 12637
		public float onlyIfJoyBelow = 1f;

		// Token: 0x0400315E RID: 12638
		public int takeToInventory;

		// Token: 0x0400315F RID: 12639
		public string takeToInventoryTempBuffer;
	}
}
