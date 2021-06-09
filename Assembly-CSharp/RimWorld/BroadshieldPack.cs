using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001664 RID: 5732
	public class BroadshieldPack : Apparel
	{
		// Token: 0x06007CDF RID: 31967 RVA: 0x00255314 File Offset: 0x00253514
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

		// Token: 0x06007CE0 RID: 31968 RVA: 0x002553B0 File Offset: 0x002535B0
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

		// Token: 0x06007CE1 RID: 31969 RVA: 0x00053E5F File Offset: 0x0005205F
		private static CompProperties_ProjectileInterceptor BroadshieldProjectileInterceptorProperties()
		{
			return ThingDefOf.BroadshieldProjector.GetCompProperties<CompProperties_ProjectileInterceptor>();
		}
	}
}
