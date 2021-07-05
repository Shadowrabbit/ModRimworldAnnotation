using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001215 RID: 4629
	public abstract class StockGenerator
	{
		// Token: 0x06006F2B RID: 28459 RVA: 0x00251F2C File Offset: 0x0025012C
		public virtual void ResolveReferences(TraderKindDef trader)
		{
			this.trader = trader;
		}

		// Token: 0x06006F2C RID: 28460 RVA: 0x00251F35 File Offset: 0x00250135
		public virtual IEnumerable<string> ConfigErrors(TraderKindDef parentDef)
		{
			yield break;
		}

		// Token: 0x06006F2D RID: 28461
		public abstract IEnumerable<Thing> GenerateThings(int forTile, Faction faction = null);

		// Token: 0x06006F2E RID: 28462
		public abstract bool HandlesThingDef(ThingDef thingDef);

		// Token: 0x06006F2F RID: 28463 RVA: 0x00251F3E File Offset: 0x0025013E
		public bool TryGetPriceType(ThingDef thingDef, TradeAction action, out PriceType priceType)
		{
			if (!this.HandlesThingDef(thingDef))
			{
				priceType = PriceType.Undefined;
				return false;
			}
			priceType = this.price;
			return true;
		}

		// Token: 0x06006F30 RID: 28464 RVA: 0x00251F58 File Offset: 0x00250158
		protected int RandomCountOf(ThingDef def)
		{
			IntRange intRange = this.countRange;
			if (this.customCountRanges != null)
			{
				for (int i = 0; i < this.customCountRanges.Count; i++)
				{
					if (this.customCountRanges[i].thingDef == def)
					{
						intRange = this.customCountRanges[i].countRange;
						break;
					}
				}
			}
			if (intRange.max <= 0 && this.totalPriceRange.max <= 0f)
			{
				return 0;
			}
			if (intRange.max > 0 && this.totalPriceRange.max <= 0f)
			{
				return intRange.RandomInRange;
			}
			if (intRange.max <= 0 && this.totalPriceRange.max > 0f)
			{
				return Mathf.RoundToInt(this.totalPriceRange.RandomInRange / def.BaseMarketValue);
			}
			int num = 0;
			int randomInRange;
			do
			{
				randomInRange = intRange.RandomInRange;
				num++;
			}
			while (num <= 100 && !this.totalPriceRange.Includes((float)randomInRange * def.BaseMarketValue));
			return randomInRange;
		}

		// Token: 0x04003D6C RID: 15724
		[Unsaved(false)]
		public TraderKindDef trader;

		// Token: 0x04003D6D RID: 15725
		public IntRange countRange = IntRange.zero;

		// Token: 0x04003D6E RID: 15726
		public List<ThingDefCountRangeClass> customCountRanges;

		// Token: 0x04003D6F RID: 15727
		public FloatRange totalPriceRange = FloatRange.Zero;

		// Token: 0x04003D70 RID: 15728
		public TechLevel maxTechLevelGenerate = TechLevel.Archotech;

		// Token: 0x04003D71 RID: 15729
		public TechLevel maxTechLevelBuy = TechLevel.Archotech;

		// Token: 0x04003D72 RID: 15730
		public PriceType price = PriceType.Normal;
	}
}
