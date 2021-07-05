using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001917 RID: 6423
	public class StockGenerator_Armor : StockGenerator_MiscItems
	{
		// Token: 0x06008E28 RID: 36392 RVA: 0x002909E4 File Offset: 0x0028EBE4
		public override bool HandlesThingDef(ThingDef td)
		{
			if (td == ThingDefOf.Apparel_ShieldBelt)
			{
				return true;
			}
			if (td == ThingDefOf.Apparel_SmokepopBelt)
			{
				return true;
			}
			ThingDef stuff = GenStuff.DefaultStuffFor(td);
			return base.HandlesThingDef(td) && td.IsApparel && (td.GetStatValueAbstract(StatDefOf.ArmorRating_Blunt, stuff) > 0.15f || td.GetStatValueAbstract(StatDefOf.ArmorRating_Sharp, stuff) > 0.15f);
		}

		// Token: 0x06008E29 RID: 36393 RVA: 0x0005F2AB File Offset: 0x0005D4AB
		protected override float SelectionWeight(ThingDef thingDef)
		{
			return StockGenerator_Armor.SelectionWeightMarketValueCurve.Evaluate(thingDef.BaseMarketValue);
		}

		// Token: 0x04005ABF RID: 23231
		public const float MinArmor = 0.15f;

		// Token: 0x04005AC0 RID: 23232
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
	}
}
