using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D90 RID: 7568
	public class Verb_ShootOneUse : Verb_Shoot
	{
		// Token: 0x0600A470 RID: 42096 RVA: 0x0006D03B File Offset: 0x0006B23B
		protected override bool TryCastShot()
		{
			if (base.TryCastShot())
			{
				if (this.burstShotsLeft <= 1)
				{
					this.SelfConsume();
				}
				return true;
			}
			if (this.burstShotsLeft < this.verbProps.burstShotCount)
			{
				this.SelfConsume();
			}
			return false;
		}

		// Token: 0x0600A471 RID: 42097 RVA: 0x0006D070 File Offset: 0x0006B270
		public override void Notify_EquipmentLost()
		{
			base.Notify_EquipmentLost();
			if (this.state == VerbState.Bursting && this.burstShotsLeft < this.verbProps.burstShotCount)
			{
				this.SelfConsume();
			}
		}

		// Token: 0x0600A472 RID: 42098 RVA: 0x0006D09A File Offset: 0x0006B29A
		private void SelfConsume()
		{
			if (base.EquipmentSource != null && !base.EquipmentSource.Destroyed)
			{
				base.EquipmentSource.Destroy(DestroyMode.Vanish);
			}
		}
	}
}
