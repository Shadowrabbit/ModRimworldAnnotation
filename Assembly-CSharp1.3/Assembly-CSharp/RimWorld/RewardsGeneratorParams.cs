using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020014A6 RID: 5286
	public struct RewardsGeneratorParams
	{
		// Token: 0x06007E6E RID: 32366 RVA: 0x002CC350 File Offset: 0x002CA550
		public string ConfigError()
		{
			if (this.rewardValue <= 0f)
			{
				return "rewardValue is " + this.rewardValue;
			}
			if (this.thingRewardDisallowed && this.thingRewardRequired)
			{
				return "thing reward is both disallowed and required";
			}
			if (this.thingRewardDisallowed && !this.allowRoyalFavor && !this.allowGoodwill)
			{
				return "no reward types are allowed";
			}
			return null;
		}

		// Token: 0x06007E6F RID: 32367 RVA: 0x002CC3B5 File Offset: 0x002CA5B5
		public override string ToString()
		{
			return GenText.FieldsToString<RewardsGeneratorParams>(this);
		}

		// Token: 0x04004EB5 RID: 20149
		public float rewardValue;

		// Token: 0x04004EB6 RID: 20150
		public Faction giverFaction;

		// Token: 0x04004EB7 RID: 20151
		public string chosenPawnSignal;

		// Token: 0x04004EB8 RID: 20152
		public bool giveToCaravan;

		// Token: 0x04004EB9 RID: 20153
		public float minGeneratedRewardValue;

		// Token: 0x04004EBA RID: 20154
		public bool thingRewardDisallowed;

		// Token: 0x04004EBB RID: 20155
		public bool thingRewardRequired;

		// Token: 0x04004EBC RID: 20156
		public bool thingRewardItemsOnly;

		// Token: 0x04004EBD RID: 20157
		public List<ThingDef> disallowedThingDefs;

		// Token: 0x04004EBE RID: 20158
		public bool allowRoyalFavor;

		// Token: 0x04004EBF RID: 20159
		public bool allowGoodwill;

		// Token: 0x04004EC0 RID: 20160
		public float populationIntent;
	}
}
