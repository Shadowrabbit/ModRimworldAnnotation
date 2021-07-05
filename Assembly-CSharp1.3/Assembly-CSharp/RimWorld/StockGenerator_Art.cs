using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200122A RID: 4650
	public class StockGenerator_Art : StockGenerator_MiscItems
	{
		// Token: 0x06006F7B RID: 28539 RVA: 0x00252E4F File Offset: 0x0025104F
		public override bool HandlesThingDef(ThingDef td)
		{
			return base.HandlesThingDef(td) && td.Minifiable && td.category == ThingCategory.Building && td.thingClass == typeof(Building_Art);
		}

		// Token: 0x06006F7C RID: 28540 RVA: 0x00252E82 File Offset: 0x00251082
		protected override float SelectionWeight(ThingDef thingDef)
		{
			return StockGenerator_Art.SelectionWeightMarketValueCurve.Evaluate(thingDef.BaseMarketValue);
		}

		// Token: 0x04003D96 RID: 15766
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
