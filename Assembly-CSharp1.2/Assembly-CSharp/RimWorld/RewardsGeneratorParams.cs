using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D03 RID: 7427
	public struct RewardsGeneratorParams
	{
		// Token: 0x0600A18A RID: 41354 RVA: 0x002F24C8 File Offset: 0x002F06C8
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

		// Token: 0x0600A18B RID: 41355 RVA: 0x0006B716 File Offset: 0x00069916
		public override string ToString()
		{
			return GenText.FieldsToString<RewardsGeneratorParams>(this);
		}

		// Token: 0x04006DA5 RID: 28069
		public float rewardValue;

		// Token: 0x04006DA6 RID: 28070
		public Faction giverFaction;

		// Token: 0x04006DA7 RID: 28071
		public string chosenPawnSignal;

		// Token: 0x04006DA8 RID: 28072
		public bool giveToCaravan;

		// Token: 0x04006DA9 RID: 28073
		public float minGeneratedRewardValue;

		// Token: 0x04006DAA RID: 28074
		public bool thingRewardDisallowed;

		// Token: 0x04006DAB RID: 28075
		public bool thingRewardRequired;

		// Token: 0x04006DAC RID: 28076
		public bool thingRewardItemsOnly;

		// Token: 0x04006DAD RID: 28077
		public List<ThingDef> disallowedThingDefs;

		// Token: 0x04006DAE RID: 28078
		public bool allowRoyalFavor;

		// Token: 0x04006DAF RID: 28079
		public bool allowGoodwill;

		// Token: 0x04006DB0 RID: 28080
		public float populationIntent;
	}
}
