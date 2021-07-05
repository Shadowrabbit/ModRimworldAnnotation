using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001229 RID: 4649
	public class StockGenerator_Clothes : StockGenerator_MiscItems
	{
		// Token: 0x06006F77 RID: 28535 RVA: 0x00252D4C File Offset: 0x00250F4C
		public override bool HandlesThingDef(ThingDef td)
		{
			return td != ThingDefOf.Apparel_ShieldBelt && (base.HandlesThingDef(td) && td.IsApparel && (this.apparelTag == null || (td.apparel.tags != null && td.apparel.tags.Contains(this.apparelTag)))) && (td.GetStatValueAbstract(StatDefOf.ArmorRating_Blunt, null) < 0.15f || td.GetStatValueAbstract(StatDefOf.ArmorRating_Sharp, null) < 0.15f);
		}

		// Token: 0x06006F78 RID: 28536 RVA: 0x00252DCB File Offset: 0x00250FCB
		protected override float SelectionWeight(ThingDef thingDef)
		{
			return StockGenerator_Clothes.SelectionWeightMarketValueCurve.Evaluate(thingDef.BaseMarketValue);
		}

		// Token: 0x04003D94 RID: 15764
		private static readonly SimpleCurve SelectionWeightMarketValueCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(500f, 0.5f),
				true
			},
			{
				new CurvePoint(1500f, 0.2f),
				true
			},
			{
				new CurvePoint(5000f, 0.1f),
				true
			}
		};

		// Token: 0x04003D95 RID: 15765
		public string apparelTag;
	}
}
