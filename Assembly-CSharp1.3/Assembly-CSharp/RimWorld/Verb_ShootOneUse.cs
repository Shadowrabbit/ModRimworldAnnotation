using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001518 RID: 5400
	public class Verb_ShootOneUse : Verb_Shoot
	{
		// Token: 0x06008090 RID: 32912 RVA: 0x002D8F43 File Offset: 0x002D7143
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

		// Token: 0x06008091 RID: 32913 RVA: 0x002D8F78 File Offset: 0x002D7178
		public override void Notify_EquipmentLost()
		{
			base.Notify_EquipmentLost();
			if (this.state == VerbState.Bursting && this.burstShotsLeft < this.verbProps.burstShotCount)
			{
				this.SelfConsume();
			}
		}

		// Token: 0x06008092 RID: 32914 RVA: 0x002D8FA2 File Offset: 0x002D71A2
		private void SelfConsume()
		{
			if (base.EquipmentSource != null && !base.EquipmentSource.Destroyed)
			{
				base.EquipmentSource.Destroy(DestroyMode.Vanish);
			}
		}
	}
}
