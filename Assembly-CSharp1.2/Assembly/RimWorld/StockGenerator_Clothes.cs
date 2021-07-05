using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001918 RID: 6424
	public class StockGenerator_Clothes : StockGenerator_MiscItems
	{
		// Token: 0x06008E2C RID: 36396 RVA: 0x00290AB8 File Offset: 0x0028ECB8
		public override bool HandlesThingDef(ThingDef td)
		{
			return td != ThingDefOf.Apparel_ShieldBelt && (base.HandlesThingDef(td) && td.IsApparel && (this.apparelTag == null || (td.apparel.tags != null && td.apparel.tags.Contains(this.apparelTag)))) && (td.GetStatValueAbstract(StatDefOf.ArmorRating_Blunt, null) < 0.15f || td.GetStatValueAbstract(StatDefOf.ArmorRating_Sharp, null) < 0.15f);
		}

		// Token: 0x06008E2D RID: 36397 RVA: 0x0005F2BD File Offset: 0x0005D4BD
		protected override float SelectionWeight(ThingDef thingDef)
		{
			return StockGenerator_Clothes.SelectionWeightMarketValueCurve.Evaluate(thingDef.BaseMarketValue);
		}

		// Token: 0x04005AC1 RID: 23233
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

		// Token: 0x04005AC2 RID: 23234
		public string apparelTag;
	}
}
