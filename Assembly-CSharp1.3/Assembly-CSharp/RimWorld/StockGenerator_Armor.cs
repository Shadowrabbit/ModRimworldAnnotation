using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001228 RID: 4648
	public class StockGenerator_Armor : StockGenerator_MiscItems
	{
		// Token: 0x06006F73 RID: 28531 RVA: 0x00252C64 File Offset: 0x00250E64
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

		// Token: 0x06006F74 RID: 28532 RVA: 0x00252CC7 File Offset: 0x00250EC7
		protected override float SelectionWeight(ThingDef thingDef)
		{
			return StockGenerator_Armor.SelectionWeightMarketValueCurve.Evaluate(thingDef.BaseMarketValue);
		}

		// Token: 0x04003D92 RID: 15762
		public const float MinArmor = 0.15f;

		// Token: 0x04003D93 RID: 15763
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
