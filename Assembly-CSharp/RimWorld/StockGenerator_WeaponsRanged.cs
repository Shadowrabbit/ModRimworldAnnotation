using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001915 RID: 6421
	public class StockGenerator_WeaponsRanged : StockGenerator_MiscItems
	{
		// Token: 0x06008E20 RID: 36384 RVA: 0x0005F20B File Offset: 0x0005D40B
		public override bool HandlesThingDef(ThingDef td)
		{
			return base.HandlesThingDef(td) && td.IsRangedWeapon && (this.weaponTag == null || (td.weaponTags != null && td.weaponTags.Contains(this.weaponTag)));
		}

		// Token: 0x06008E21 RID: 36385 RVA: 0x0005F245 File Offset: 0x0005D445
		protected override float SelectionWeight(ThingDef thingDef)
		{
			return StockGenerator_WeaponsRanged.SelectionWeightMarketValueCurve.Evaluate(thingDef.BaseMarketValue);
		}

		// Token: 0x04005ABB RID: 23227
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
				new CurvePoint(1500f, 0.2f),
				true
			},
			{
				new CurvePoint(5000f, 0.1f),
				true
			}
		};

		// Token: 0x04005ABC RID: 23228
		public string weaponTag;
	}
}
