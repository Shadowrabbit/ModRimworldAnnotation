using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014FA RID: 5370
	public class StatWorker_MeleeDamageAmountTrap : StatWorker_MeleeDamageAmount
	{
		// Token: 0x06008000 RID: 32768 RVA: 0x002D5374 File Offset: 0x002D3574
		public override bool ShouldShowFor(StatRequest req)
		{
			ThingDef thingDef = req.Def as ThingDef;
			return thingDef != null && thingDef.category == ThingCategory.Building && thingDef.building.isTrap;
		}

		// Token: 0x06008001 RID: 32769 RVA: 0x002D53A7 File Offset: 0x002D35A7
		protected override DamageArmorCategoryDef CategoryOfDamage(ThingDef def)
		{
			return def.building.trapDamageCategory;
		}
	}
}
