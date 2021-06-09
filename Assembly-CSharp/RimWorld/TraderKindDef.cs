using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FFE RID: 4094
	public class TraderKindDef : Def
	{
		// Token: 0x17000DD0 RID: 3536
		// (get) Token: 0x06005946 RID: 22854 RVA: 0x001D1EBC File Offset: 0x001D00BC
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

		// Token: 0x17000DD1 RID: 3537
		// (get) Token: 0x06005947 RID: 22855 RVA: 0x001D1EEC File Offset: 0x001D00EC
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

		// Token: 0x06005948 RID: 22856 RVA: 0x001D1F24 File Offset: 0x001D0124
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			foreach (StockGenerator stockGenerator in this.stockGenerators)
			{
				stockGenerator.ResolveReferences(this);
			}
		}

		// Token: 0x06005949 RID: 22857 RVA: 0x0003E01B File Offset: 0x0003C21B
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

		// Token: 0x0600594A RID: 22858 RVA: 0x001D1F7C File Offset: 0x001D017C
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

		// Token: 0x0600594B RID: 22859 RVA: 0x001D1FB8 File Offset: 0x001D01B8
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

		// Token: 0x04003BD8 RID: 15320
		public List<StockGenerator> stockGenerators = new List<StockGenerator>();

		// Token: 0x04003BD9 RID: 15321
		public bool orbital;

		// Token: 0x04003BDA RID: 15322
		public bool requestable = true;

		// Token: 0x04003BDB RID: 15323
		public bool hideThingsNotWillingToTrade;

		// Token: 0x04003BDC RID: 15324
		public float commonality = 1f;

		// Token: 0x04003BDD RID: 15325
		public string category;

		// Token: 0x04003BDE RID: 15326
		public TradeCurrency tradeCurrency;

		// Token: 0x04003BDF RID: 15327
		public SimpleCurve commonalityMultFromPopulationIntent;

		// Token: 0x04003BE0 RID: 15328
		public FactionDef faction;

		// Token: 0x04003BE1 RID: 15329
		public RoyalTitlePermitDef permitRequiredForTrading;
	}
}
