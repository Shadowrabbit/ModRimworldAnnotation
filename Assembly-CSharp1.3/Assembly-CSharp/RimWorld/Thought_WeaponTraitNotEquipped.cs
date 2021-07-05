using System;

namespace RimWorld
{
	// Token: 0x020009D3 RID: 2515
	public class Thought_WeaponTraitNotEquipped : Thought_WeaponTrait
	{
		// Token: 0x17000AE7 RID: 2791
		// (get) Token: 0x06003E51 RID: 15953 RVA: 0x00154F53 File Offset: 0x00153153
		public override bool ShouldDiscard
		{
			get
			{
				return base.ShouldDiscard || this.pawn.equipment.Primary == this.weapon;
			}
		}
	}
}
