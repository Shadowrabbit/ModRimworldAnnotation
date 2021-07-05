using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ADD RID: 2781
	public class TraderKindDef : Def
	{
		// Token: 0x17000B85 RID: 2949
		// (get) Token: 0x06004184 RID: 16772 RVA: 0x0015FB08 File Offset: 0x0015DD08
		public float CalculatedCommonality
		{
			get
			{
				float num = this.commonality;
				if (this.commonalityMultFromPopulationIntent != null)
				{
					num *= this.commonalityMultFromPopulationIntent.Evaluate(StorytellerUtilityPopulation.PopulationIntent);
				}
				return num;
			}
		}

		// Token: 0x17000B86 RID: 2950
		// (get) Token: 0x06004185 RID: 16773 RVA: 0x0015FB38 File Offset: 0x0015DD38
		public RoyalTitleDef TitleRequiredToTrade
		{
			get
			{
				if (this.permitRequiredForTrading != null)
				{
					RoyalTitleDef royalTitleDef = this.faction.RoyalTitlesAwardableInSeniorityOrderForReading.FirstOrDefault((RoyalTitleDef x) => x.permits != null && x.permits.Contains(this.permitRequiredForTrading));
					if (royalTitleDef != null)
					{
						return royalTitleDef;
					}
				}
				return null;
			}
		}

		// Token: 0x06004186 RID: 16774 RVA: 0x0015FB70 File Offset: 0x0015DD70
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			foreach (StockGenerator stockGenerator in this.stockGenerators)
			{
				stockGenerator.ResolveReferences(this);
			}
		}

		// Token: 0x06004187 RID: 16775 RVA: 0x0015FBC8 File Offset: 0x0015DDC8
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			foreach (StockGenerator stockGenerator in this.stockGenerators)
			{
				foreach (string text2 in stockGenerator.ConfigErrors(this))
				{
					yield return text2;
				}
				enumerator = null;
			}
			List<StockGenerator>.Enumerator enumerator2 = default(List<StockGenerator>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06004188 RID: 16776 RVA: 0x0015FBD8 File Offset: 0x0015DDD8
		public bool WillTrade(ThingDef td)
		{
			for (int i = 0; i < this.stockGenerators.Count; i++)
			{
				if (this.stockGenerators[i].HandlesThingDef(td))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004189 RID: 16777 RVA: 0x0015FC14 File Offset: 0x0015DE14
		public PriceType PriceTypeFor(ThingDef thingDef, TradeAction action)
		{
			if (thingDef == ThingDefOf.Silver)
			{
				return PriceType.Undefined;
			}
			if (action == TradeAction.PlayerBuys)
			{
				for (int i = 0; i < this.stockGenerators.Count; i++)
				{
					PriceType result;
					if (this.stockGenerators[i].TryGetPriceType(thingDef, action, out result))
					{
						return result;
					}
				}
			}
			return PriceType.Normal;
		}

		// Token: 0x0400278C RID: 10124
		public List<StockGenerator> stockGenerators = new List<StockGenerator>();

		// Token: 0x0400278D RID: 10125
		public bool orbital;

		// Token: 0x0400278E RID: 10126
		public bool requestable = true;

		// Token: 0x0400278F RID: 10127
		public bool hideThingsNotWillingToTrade;

		// Token: 0x04002790 RID: 10128
		public float commonality = 1f;

		// Token: 0x04002791 RID: 10129
		public string category;

		// Token: 0x04002792 RID: 10130
		public TradeCurrency tradeCurrency;

		// Token: 0x04002793 RID: 10131
		public SimpleCurve commonalityMultFromPopulationIntent;

		// Token: 0x04002794 RID: 10132
		public FactionDef faction;

		// Token: 0x04002795 RID: 10133
		public RoyalTitlePermitDef permitRequiredForTrading;
	}
}
