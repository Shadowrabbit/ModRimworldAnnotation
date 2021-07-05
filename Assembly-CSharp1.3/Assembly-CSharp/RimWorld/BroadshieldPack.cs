using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200103E RID: 4158
	public class BroadshieldPack : Apparel
	{
		// Token: 0x0600623C RID: 25148 RVA: 0x00215394 File Offset: 0x00213594
		public override void Notify_BulletImpactNearby(BulletImpactData impactData)
		{
			base.Notify_BulletImpactNearby(impactData);
			Pawn wearer = base.Wearer;
			if (wearer.Dead)
			{
				return;
			}
			if (!impactData.bullet.def.projectile.damageDef.isExplosive && CompProjectileInterceptor.InterceptsProjectile(BroadshieldPack.BroadshieldProjectileInterceptorProperties(), impactData.bullet) && impactData.bullet.Launcher != null && impactData.bullet.Launcher.HostileTo(base.Wearer) && !wearer.IsColonist && wearer.Spawned && !this.NearbyActiveBroadshield())
			{
				Verb_DeployBroadshield.Deploy(this.TryGetComp<CompReloadable>());
			}
		}

		// Token: 0x0600623D RID: 25149 RVA: 0x00215430 File Offset: 0x00213630
		private bool NearbyActiveBroadshield()
		{
			float radius = BroadshieldPack.BroadshieldProjectileInterceptorProperties().radius;
			foreach (Thing thing in GenRadial.RadialDistinctThingsAround(base.PositionHeld, base.MapHeld, radius, true))
			{
				CompProjectileInterceptor compProjectileInterceptor = thing.TryGetComp<CompProjectileInterceptor>();
				if (thing.def == ThingDefOf.BroadshieldProjector && compProjectileInterceptor != null && compProjectileInterceptor.Active)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600623E RID: 25150 RVA: 0x002154B4 File Offset: 0x002136B4
		private static CompProperties_ProjectileInterceptor BroadshieldProjectileInterceptorProperties()
		{
			return ThingDefOf.BroadshieldProjector.GetCompProperties<CompProperties_ProjectileInterceptor>();
		}
	}
}
