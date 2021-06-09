using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D65 RID: 7525
	public class StatWorker_MeleeDamageAmountTrap : StatWorker_MeleeDamageAmount
	{
		// Token: 0x0600A398 RID: 41880 RVA: 0x002FA318 File Offset: 0x002F8518
		public override bool ShouldShowFor(StatRequest req)
		{
			ThingDef thingDef = req.Def as ThingDef;
			return thingDef != null && thingDef.category == ThingCategory.Building && thingDef.building.isTrap;
		}

		// Token: 0x0600A399 RID: 41881 RVA: 0x0006C993 File Offset: 0x0006AB93
		protected override DamageArmorCategoryDef CategoryOfDamage(ThingDef def)
		{
			return def.building.trapDamageCategory;
		}
	}
}
