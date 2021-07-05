using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020010D6 RID: 4310
	public class SmokepopBelt : Apparel
	{
		// Token: 0x0600672D RID: 26413 RVA: 0x0022DEB0 File Offset: 0x0022C0B0
		public override void Notify_BulletImpactNearby(BulletImpactData impactData)
		{
			base.Notify_BulletImpactNearby(impactData);
			Pawn wearer = base.Wearer;
			if (wearer == null || wearer.Dead)
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

		// Token: 0x0600672E RID: 26414 RVA: 0x0022DF6C File Offset: 0x0022C16C
		public override float GetSpecialApparelScoreOffset()
		{
			return this.GetStatValue(StatDefOf.SmokepopBeltRadius, true) * this.ApparelScorePerBeltRadius;
		}

		// Token: 0x04003A3F RID: 14911
		private float ApparelScorePerBeltRadius = 0.046f;
	}
}
