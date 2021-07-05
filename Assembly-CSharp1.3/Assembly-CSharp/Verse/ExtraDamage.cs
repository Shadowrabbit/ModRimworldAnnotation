using System;

namespace Verse
{
	// Token: 0x020000BD RID: 189
	public class ExtraDamage
	{
		// Token: 0x060005A9 RID: 1449 RVA: 0x0001D251 File Offset: 0x0001B451
		public float AdjustedDamageAmount(Verb verb, Pawn caster)
		{
			return this.amount * verb.verbProps.GetDamageFactorFor(verb, caster);
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x0001D267 File Offset: 0x0001B467
		public float AdjustedArmorPenetration(Verb verb, Pawn caster)
		{
			if (this.armorPenetration < 0f)
			{
				return this.AdjustedDamageAmount(verb, caster) * 0.015f;
			}
			return this.armorPenetration;
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x0001D28B File Offset: 0x0001B48B
		public float AdjustedArmorPenetration()
		{
			if (this.armorPenetration < 0f)
			{
				return this.amount * 0.015f;
			}
			return this.armorPenetration;
		}

		// Token: 0x040003A4 RID: 932
		public DamageDef def;

		// Token: 0x040003A5 RID: 933
		public float amount;

		// Token: 0x040003A6 RID: 934
		public float armorPenetration = -1f;

		// Token: 0x040003A7 RID: 935
		public float chance = 1f;
	}
}
