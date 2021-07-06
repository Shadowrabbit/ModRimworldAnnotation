using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001919 RID: 6425
	public class StockGenerator_Art : StockGenerator_MiscItems
	{
		// Token: 0x06008E30 RID: 36400 RVA: 0x0005F2CF File Offset: 0x0005D4CF
		public override bool HandlesThingDef(ThingDef td)
		{
			return base.HandlesThingDef(td) && td.Minifiable && td.category == ThingCategory.Building && td.thingClass == typeof(Building_Art);
		}

		// Token: 0x06008E31 RID: 36401 RVA: 0x0005F302 File Offset: 0x0005D502
		protected override float SelectionWeight(ThingDef thingDef)
		{
			return StockGenerator_Art.SelectionWeightMarketValueCurve.Evaluate(thingDef.BaseMarketValue);
		}

		// Token: 0x04005AC3 RID: 23235
		private static readonly SimpleCurve SelectionWeightMarketValueCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(500f, 1f),
				true
			},
			{
				new CurvePoint(1000f, 0.2f),
				true
			}
		};
	}
}
