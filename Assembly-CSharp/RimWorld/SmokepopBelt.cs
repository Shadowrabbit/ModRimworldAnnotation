using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200173C RID: 5948
	public class SmokepopBelt : Apparel
	{
		// Token: 0x06008339 RID: 33593 RVA: 0x0026E9B8 File Offset: 0x0026CBB8
		public override void Notify_BulletImpactNearby(BulletImpactData impactData)
		{
			base.Notify_BulletImpactNearby(impactData);
			Pawn wearer = base.Wearer;
			if (wearer.Dead)
			{
				return;
			}
			if (!impactData.bullet.def.projectile.damageDef.isExplosive && impactData.bullet.Launcher is Building_Turret && impactData.bullet.Launcher != null && impactData.bullet.Launcher.HostileTo(base.Wearer) && !wearer.IsColonist && wearer.Spawned)
			{
				Gas gas = wearer.Position.GetGas(wearer.Map);
				if (gas == null || !gas.def.gas.blockTurretTracking)
				{
					Verb_SmokePop.Pop(this.TryGetComp<CompReloadable>());
				}
			}
		}

		// Token: 0x0600833A RID: 33594 RVA: 0x000581D7 File Offset: 0x000563D7
		public override float GetSpecialApparelScoreOffset()
		{
			return this.GetStatValue(StatDefOf.SmokepopBeltRadius, true) * this.ApparelScorePerBeltRadius;
		}

		// Token: 0x0400550A RID: 21770
		private float ApparelScorePerBeltRadius = 0.046f;
	}
}
