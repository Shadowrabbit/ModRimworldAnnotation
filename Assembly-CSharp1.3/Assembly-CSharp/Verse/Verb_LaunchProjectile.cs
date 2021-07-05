using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004F9 RID: 1273
	public class Verb_LaunchProjectile : Verb
	{
		// Token: 0x1700078C RID: 1932
		// (get) Token: 0x060026A1 RID: 9889 RVA: 0x000EF870 File Offset: 0x000EDA70
		public virtual ThingDef Projectile
		{
			get
			{
				if (base.EquipmentSource != null)
				{
					CompChangeableProjectile comp = base.EquipmentSource.GetComp<CompChangeableProjectile>();
					if (comp != null && comp.Loaded)
					{
						return comp.Projectile;
					}
				}
				return this.verbProps.defaultProjectile;
			}
		}

		// Token: 0x060026A2 RID: 9890 RVA: 0x000EF8B0 File Offset: 0x000EDAB0
		public override void WarmupComplete()
		{
			base.WarmupComplete();
			Find.BattleLog.Add(new BattleLogEntry_RangedFire(this.caster, this.currentTarget.HasThing ? this.currentTarget.Thing : null, (base.EquipmentSource != null) ? base.EquipmentSource.def : null, this.Projectile, this.ShotsPerBurst > 1));
		}

		// Token: 0x060026A3 RID: 9891 RVA: 0x000EF918 File Offset: 0x000EDB18
		protected override bool TryCastShot()
		{
			if (this.currentTarget.HasThing && this.currentTarget.Thing.Map != this.caster.Map)
			{
				return false;
			}
			ThingDef projectile = this.Projectile;
			if (projectile == null)
			{
				return false;
			}
			ShootLine shootLine;
			bool flag = base.TryFindShootLineFromTo(this.caster.Position, this.currentTarget, out shootLine);
			if (this.verbProps.stopBurstWithoutLos && !flag)
			{
				return false;
			}
			if (base.EquipmentSource != null)
			{
				CompChangeableProjectile comp = base.EquipmentSource.GetComp<CompChangeableProjectile>();
				if (comp != null)
				{
					comp.Notify_ProjectileLaunched();
				}
				CompReloadable comp2 = base.EquipmentSource.GetComp<CompReloadable>();
				if (comp2 != null)
				{
					comp2.UsedOnce();
				}
			}
			this.lastShotTick = Find.TickManager.TicksGame;
			Thing launcher = this.caster;
			Thing equipment = base.EquipmentSource;
			CompMannable compMannable = this.caster.TryGetComp<CompMannable>();
			if (compMannable != null && compMannable.ManningPawn != null)
			{
				launcher = compMannable.ManningPawn;
				equipment = this.caster;
			}
			Vector3 drawPos = this.caster.DrawPos;
			Projectile projectile2 = (Projectile)GenSpawn.Spawn(projectile, shootLine.Source, this.caster.Map, WipeMode.Vanish);
			if (this.verbProps.ForcedMissRadius > 0.5f)
			{
				float num = VerbUtility.CalculateAdjustedForcedMiss(this.verbProps.ForcedMissRadius, this.currentTarget.Cell - this.caster.Position);
				if (num > 0.5f)
				{
					int max = GenRadial.NumCellsInRadius(num);
					int num2 = Rand.Range(0, max);
					if (num2 > 0)
					{
						IntVec3 c = this.currentTarget.Cell + GenRadial.RadialPattern[num2];
						this.ThrowDebugText("ToRadius");
						this.ThrowDebugText("Rad\nDest", c);
						ProjectileHitFlags projectileHitFlags = ProjectileHitFlags.NonTargetWorld;
						if (Rand.Chance(0.5f))
						{
							projectileHitFlags = ProjectileHitFlags.All;
						}
						if (!this.canHitNonTargetPawnsNow)
						{
							projectileHitFlags &= ~ProjectileHitFlags.NonTargetPawns;
						}
						projectile2.Launch(launcher, drawPos, c, this.currentTarget, projectileHitFlags, this.preventFriendlyFire, equipment, null);
						return true;
					}
				}
			}
			ShotReport shotReport = ShotReport.HitReportFor(this.caster, this, this.currentTarget);
			Thing randomCoverToMissInto = shotReport.GetRandomCoverToMissInto();
			ThingDef targetCoverDef = (randomCoverToMissInto != null) ? randomCoverToMissInto.def : null;
			if (!Rand.Chance(shotReport.AimOnTargetChance_IgnoringPosture))
			{
				shootLine.ChangeDestToMissWild(shotReport.AimOnTargetChance_StandardTarget);
				this.ThrowDebugText("ToWild" + (this.canHitNonTargetPawnsNow ? "\nchntp" : ""));
				this.ThrowDebugText("Wild\nDest", shootLine.Dest);
				ProjectileHitFlags projectileHitFlags2 = ProjectileHitFlags.NonTargetWorld;
				if (Rand.Chance(0.5f) && this.canHitNonTargetPawnsNow)
				{
					projectileHitFlags2 |= ProjectileHitFlags.NonTargetPawns;
				}
				projectile2.Launch(launcher, drawPos, shootLine.Dest, this.currentTarget, projectileHitFlags2, this.preventFriendlyFire, equipment, targetCoverDef);
				return true;
			}
			if (this.currentTarget.Thing != null && this.currentTarget.Thing.def.category == ThingCategory.Pawn && !Rand.Chance(shotReport.PassCoverChance))
			{
				this.ThrowDebugText("ToCover" + (this.canHitNonTargetPawnsNow ? "\nchntp" : ""));
				this.ThrowDebugText("Cover\nDest", randomCoverToMissInto.Position);
				ProjectileHitFlags projectileHitFlags3 = ProjectileHitFlags.NonTargetWorld;
				if (this.canHitNonTargetPawnsNow)
				{
					projectileHitFlags3 |= ProjectileHitFlags.NonTargetPawns;
				}
				projectile2.Launch(launcher, drawPos, randomCoverToMissInto, this.currentTarget, projectileHitFlags3, this.preventFriendlyFire, equipment, targetCoverDef);
				return true;
			}
			ProjectileHitFlags projectileHitFlags4 = ProjectileHitFlags.IntendedTarget;
			if (this.canHitNonTargetPawnsNow)
			{
				projectileHitFlags4 |= ProjectileHitFlags.NonTargetPawns;
			}
			if (!this.currentTarget.HasThing || this.currentTarget.Thing.def.Fillage == FillCategory.Full)
			{
				projectileHitFlags4 |= ProjectileHitFlags.NonTargetWorld;
			}
			this.ThrowDebugText("ToHit" + (this.canHitNonTargetPawnsNow ? "\nchntp" : ""));
			if (this.currentTarget.Thing != null)
			{
				projectile2.Launch(launcher, drawPos, this.currentTarget, this.currentTarget, projectileHitFlags4, this.preventFriendlyFire, equipment, targetCoverDef);
				this.ThrowDebugText("Hit\nDest", this.currentTarget.Cell);
			}
			else
			{
				projectile2.Launch(launcher, drawPos, shootLine.Dest, this.currentTarget, projectileHitFlags4, this.preventFriendlyFire, equipment, targetCoverDef);
				this.ThrowDebugText("Hit\nDest", shootLine.Dest);
			}
			return true;
		}

		// Token: 0x060026A4 RID: 9892 RVA: 0x000EFD61 File Offset: 0x000EDF61
		private void ThrowDebugText(string text)
		{
			if (DebugViewSettings.drawShooting)
			{
				MoteMaker.ThrowText(this.caster.DrawPos, this.caster.Map, text, -1f);
			}
		}

		// Token: 0x060026A5 RID: 9893 RVA: 0x000EFD8B File Offset: 0x000EDF8B
		private void ThrowDebugText(string text, IntVec3 c)
		{
			if (DebugViewSettings.drawShooting)
			{
				MoteMaker.ThrowText(c.ToVector3Shifted(), this.caster.Map, text, -1f);
			}
		}

		// Token: 0x060026A6 RID: 9894 RVA: 0x000EFDB4 File Offset: 0x000EDFB4
		public override float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
		{
			needLOSToCenter = true;
			ThingDef projectile = this.Projectile;
			if (projectile == null)
			{
				return 0f;
			}
			return projectile.projectile.explosionRadius;
		}

		// Token: 0x060026A7 RID: 9895 RVA: 0x000EFDE0 File Offset: 0x000EDFE0
		public override bool Available()
		{
			if (!base.Available())
			{
				return false;
			}
			if (this.CasterIsPawn)
			{
				Pawn casterPawn = this.CasterPawn;
				if (casterPawn.Faction != Faction.OfPlayer && casterPawn.mindState.MeleeThreatStillThreat && casterPawn.mindState.meleeThreat.Position.AdjacentTo8WayOrInside(casterPawn.Position))
				{
					return false;
				}
			}
			return this.Projectile != null;
		}
	}
}
