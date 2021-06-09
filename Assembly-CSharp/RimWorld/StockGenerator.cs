using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020018E6 RID: 6374
	public abstract class StockGenerator
	{
		// Token: 0x06008D33 RID: 36147 RVA: 0x0005E9FA File Offset: 0x0005CBFA
		public virtual void ResolveReferences(TraderKindDef trader)
		{
			this.trader = trader;
		}

		// Token: 0x06008D34 RID: 36148 RVA: 0x0005EA03 File Offset: 0x0005CC03
		public virtual IEnumerable<string> ConfigErrors(TraderKindDef parentDef)
		{
			yield break;
		}

		// Token: 0x06008D35 RID: 36149
		public abstract IEnumerable<Thing> GenerateThings(int forTile, Faction faction = null);

		// Token: 0x06008D36 RID: 36150
		public abstract bool HandlesThingDef(ThingDef thingDef);

		// Token: 0x06008D37 RID: 36151 RVA: 0x0005EA0C File Offset: 0x0005CC0C
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

		// Token: 0x06008D38 RID: 36152 RVA: 0x0028EAE0 File Offset: 0x0028CCE0
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

		// Token: 0x04005A19 RID: 23065
		[Unsaved(false)]
		public TraderKindDef trader;

		// Token: 0x04005A1A RID: 23066
		public IntRange countRange = IntRange.zero;

		// Token: 0x04005A1B RID: 23067
		public List<ThingDefCountRangeClass> customCountRanges;

		// Token: 0x04005A1C RID: 23068
		public FloatRange totalPriceRange = FloatRange.Zero;

		// Token: 0x04005A1D RID: 23069
		public TechLevel maxTechLevelGenerate = TechLevel.Archotech;

		// Token: 0x04005A1E RID: 23070
		public TechLevel maxTechLevelBuy = TechLevel.Archotech;

		// Token: 0x04005A1F RID: 23071
		public PriceType price = PriceType.Normal;
	}
}
