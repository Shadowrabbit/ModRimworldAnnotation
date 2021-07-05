using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001226 RID: 4646
	public class StockGenerator_WeaponsRanged : StockGenerator_MiscItems
	{
		// Token: 0x06006F6B RID: 28523 RVA: 0x00252AE1 File Offset: 0x00250CE1
		public override bool HandlesThingDef(ThingDef td)
		{
			return base.HandlesThingDef(td) && td.IsRangedWeapon && (this.weaponTag == null || (td.weaponTags != null && td.weaponTags.Contains(this.weaponTag)));
		}

		// Token: 0x06006F6C RID: 28524 RVA: 0x00252B1B File Offset: 0x00250D1B
		protected override float SelectionWeight(ThingDef thingDef)
		{
			return StockGenerator_WeaponsRanged.SelectionWeightMarketValueCurve.Evaluate(thingDef.BaseMarketValue);
		}

		// Token: 0x04003D8E RID: 15758
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

		// Token: 0x04003D8F RID: 15759
		public string weaponTag;
	}
}
