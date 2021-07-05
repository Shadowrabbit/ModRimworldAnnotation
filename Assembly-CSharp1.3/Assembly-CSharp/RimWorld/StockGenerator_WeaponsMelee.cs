using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001227 RID: 4647
	public class StockGenerator_WeaponsMelee : StockGenerator_MiscItems
	{
		// Token: 0x06006F6F RID: 28527 RVA: 0x00252BA7 File Offset: 0x00250DA7
		public override bool HandlesThingDef(ThingDef td)
		{
			return base.HandlesThingDef(td) && td.IsMeleeWeapon && (this.weaponTag == null || (td.weaponTags != null && td.weaponTags.Contains(this.weaponTag)));
		}

		// Token: 0x06006F70 RID: 28528 RVA: 0x00252BE1 File Offset: 0x00250DE1
		protected override float SelectionWeight(ThingDef thingDef)
		{
			return StockGenerator_WeaponsMelee.SelectionWeightMarketValueCurve.Evaluate(thingDef.BaseMarketValue);
		}

		// Token: 0x04003D90 RID: 15760
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

		// Token: 0x04003D91 RID: 15761
		public string weaponTag;
	}
}
