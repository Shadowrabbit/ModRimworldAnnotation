using System;

namespace RimWorld
{
	// Token: 0x02000EE0 RID: 3808
	public class Thought_WeaponTraitNotEquipped : Thought_WeaponTrait
	{
		// Token: 0x17000CD5 RID: 3285
		// (get) Token: 0x0600543F RID: 21567 RVA: 0x0003A7E5 File Offset: 0x000389E5
		public override bool ShouldDiscard
		{
			get
			{
				return base.ShouldDiscard || this.pawn.equipment.Primary == this.weapon;
			}
		}
	}
}
