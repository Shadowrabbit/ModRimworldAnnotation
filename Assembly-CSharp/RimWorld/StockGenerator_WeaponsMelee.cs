using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001916 RID: 6422
	public class StockGenerator_WeaponsMelee : StockGenerator_MiscItems
	{
		// Token: 0x06008E24 RID: 36388 RVA: 0x0005F25F File Offset: 0x0005D45F
		public override bool HandlesThingDef(ThingDef td)
		{
			return base.HandlesThingDef(td) && td.IsMeleeWeapon && (this.weaponTag == null || (td.weaponTags != null && td.weaponTags.Contains(this.weaponTag)));
		}

		// Token: 0x06008E25 RID: 36389 RVA: 0x0005F299 File Offset: 0x0005D499
		protected override float SelectionWeight(ThingDef thingDef)
		{
			return StockGenerator_WeaponsMelee.SelectionWeightMarketValueCurve.Evaluate(thingDef.BaseMarketValue);
		}

		// Token: 0x04005ABD RID: 23229
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

		// Token: 0x04005ABE RID: 23230
		public string weaponTag;
	}
}
