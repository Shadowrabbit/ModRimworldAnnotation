using System;

namespace Verse
{
	// Token: 0x02000125 RID: 293
	public class ExtraDamage
	{
		// Token: 0x060007DE RID: 2014 RVA: 0x0000C424 File Offset: 0x0000A624
		public float AdjustedDamageAmount(Verb verb, Pawn caster)
		{
			return this.amount * verb.verbProps.GetDamageFactorFor(verb, caster);
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x0000C43A File Offset: 0x0000A63A
		public float AdjustedArmorPenetration(Verb verb, Pawn caster)
		{
			if (this.armorPenetration < 0f)
			{
				return this.AdjustedDamageAmount(verb, caster) * 0.015f;
			}
			return this.armorPenetration;
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x0000C45E File Offset: 0x0000A65E
		public float AdjustedArmorPenetration()
		{
			if (this.armorPenetration < 0f)
			{
				return this.amount * 0.015f;
			}
			return this.armorPenetration;
		}

		// Token: 0x04000587 RID: 1415
		public DamageDef def;

		// Token: 0x04000588 RID: 1416
		public float amount;

		// Token: 0x04000589 RID: 1417
		public float armorPenetration = -1f;

		// Token: 0x0400058A RID: 1418
		public float chance = 1f;
	}
}
