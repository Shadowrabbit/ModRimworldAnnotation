using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001091 RID: 4241
	[StaticConstructorOnStartup]
	public class Bombardment : OrbitalStrike
	{
		// Token: 0x06006502 RID: 25858 RVA: 0x00221665 File Offset: 0x0021F865
		public override void StartStrike()
		{
			this.duration = this.bombIntervalTicks * this.explosionCount;
			base.StartStrike();
		}

		// Token: 0x06006503 RID: 25859 RVA: 0x00221680 File Offset: 0x0021F880
		public override void Tick()
		{
			if (base.Destroyed)
			{
				return;
			}
			if (this.warmupTicks > 0)
			{
				this.warmupTicks--;
				if (this.warmupTicks == 0)
				{
					this.StartStrike();
				}
			}
			else
			{
				base.Tick();
				if (Find.TickManager.TicksGame % 20 == 0 && base.TicksLeft > 0)
				{
					this.StartRandomFire();
				}
			}
			this.EffectTick();
		}

		// Token: 0x06006504 RID: 25860 RVA: 0x002216E8 File Offset: 0x0021F8E8
		private void EffectTick()
		{
			if (!this.nextExplosionCell.IsValid)
			{
				this.ticksToNextEffect = this.warmupTicks - this.bombIntervalTicks;
				this.GetNextExplosionCell();
			}
			this.ticksToNextEffect--;
			if (this.ticksToNextEffect <= 0 && base.TicksLeft >= this.bombIntervalTicks)
			{
				SoundDefOf.Bombardment_PreImpact.PlayOneShot(new TargetInfo(this.nextExplosionCell, base.Map, false));
				this.projectiles.Add(new Bombardment.BombardmentProjectile(60, this.nextExplosionCell));
				this.ticksToNextEffect = this.bombIntervalTicks;
				this.GetNextExplosionCell();
			}
			for (int i = this.projectiles.Count - 1; i >= 0; i--)
			{
				this.projectiles[i].Tick();
				if (this.projectiles[i].LifeTime <= 0)
				{
					this.TryDoExplosion(this.projectiles[i]);
					this.projectiles.RemoveAt(i);
				}
			}
		}

		// Token: 0x06006505 RID: 25861 RVA: 0x002217E8 File Offset: 0x0021F9E8
		private void TryDoExplosion(Bombardment.BombardmentProjectile proj)
		{
			List<Thing> list = base.Map.listerThings.ThingsInGroup(ThingRequestGroup.ProjectileInterceptor);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].TryGetComp<CompProjectileInterceptor>().CheckBombardmentIntercept(this, proj))
				{
					return;
				}
			}
			IntVec3 targetCell = proj.targetCell;
			Map map = base.Map;
			float randomInRange = this.explosionRadiusRange.RandomInRange;
			DamageDef bomb = DamageDefOf.Bomb;
			Thing instigator = this.instigator;
			int damAmount = -1;
			float armorPenetration = -1f;
			SoundDef explosionSound = null;
			ThingDef def = this.def;
			GenExplosion.DoExplosion(targetCell, map, randomInRange, bomb, instigator, damAmount, armorPenetration, explosionSound, this.weaponDef, def, null, null, 0f, 1, false, null, 0f, 1, 0f, false, null, null);
		}

		// Token: 0x06006506 RID: 25862 RVA: 0x0022188C File Offset: 0x0021FA8C
		public override void Draw()
		{
			base.Draw();
			if (this.projectiles.NullOrEmpty<Bombardment.BombardmentProjectile>())
			{
				return;
			}
			for (int i = 0; i < this.projectiles.Count; i++)
			{
				this.projectiles[i].Draw(Bombardment.ProjectileMaterial);
			}
		}

		// Token: 0x06006507 RID: 25863 RVA: 0x002218DC File Offset: 0x0021FADC
		private void StartRandomFire()
		{
			IntVec3 intVec = (from x in GenRadial.RadialCellsAround(base.Position, (float)this.randomFireRadius, true)
			where x.InBounds(base.Map)
			select x).RandomElementByWeight((IntVec3 x) => Bombardment.DistanceChanceFactor.Evaluate(x.DistanceTo(base.Position)));
			List<Thing> list = base.Map.listerThings.ThingsInGroup(ThingRequestGroup.ProjectileInterceptor);
			for (int i = 0; i < list.Count; i++)
			{
				if (!list[i].TryGetComp<CompProjectileInterceptor>().BombardmentCanStartFireAt(this, intVec))
				{
					return;
				}
			}
			FireUtility.TryStartFireIn(intVec, base.Map, Rand.Range(0.1f, 0.925f));
		}

		// Token: 0x06006508 RID: 25864 RVA: 0x00221975 File Offset: 0x0021FB75
		private void GetNextExplosionCell()
		{
			this.nextExplosionCell = (from x in GenRadial.RadialCellsAround(base.Position, this.impactAreaRadius, true)
			where x.InBounds(base.Map)
			select x).RandomElementByWeight((IntVec3 x) => Bombardment.DistanceChanceFactor.Evaluate(x.DistanceTo(base.Position) / this.impactAreaRadius));
		}

		// Token: 0x06006509 RID: 25865 RVA: 0x002219B4 File Offset: 0x0021FBB4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.impactAreaRadius, "impactAreaRadius", 15f, false);
			Scribe_Values.Look<FloatRange>(ref this.explosionRadiusRange, "explosionRadiusRange", new FloatRange(6f, 8f), false);
			Scribe_Values.Look<int>(ref this.randomFireRadius, "randomFireRadius", 25, false);
			Scribe_Values.Look<int>(ref this.bombIntervalTicks, "bombIntervalTicks", 18, false);
			Scribe_Values.Look<int>(ref this.warmupTicks, "warmupTicks", 0, false);
			Scribe_Values.Look<int>(ref this.ticksToNextEffect, "ticksToNextEffect", 0, false);
			Scribe_Values.Look<IntVec3>(ref this.nextExplosionCell, "nextExplosionCell", default(IntVec3), false);
			Scribe_Collections.Look<Bombardment.BombardmentProjectile>(ref this.projectiles, "projectiles", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (!this.nextExplosionCell.IsValid)
				{
					this.GetNextExplosionCell();
				}
				if (this.projectiles == null)
				{
					this.projectiles = new List<Bombardment.BombardmentProjectile>();
				}
			}
		}

		// Token: 0x040038DD RID: 14557
		public float impactAreaRadius = 15f;

		// Token: 0x040038DE RID: 14558
		public FloatRange explosionRadiusRange = new FloatRange(6f, 8f);

		// Token: 0x040038DF RID: 14559
		public int randomFireRadius = 25;

		// Token: 0x040038E0 RID: 14560
		public int bombIntervalTicks = 18;

		// Token: 0x040038E1 RID: 14561
		public int warmupTicks = 60;

		// Token: 0x040038E2 RID: 14562
		public int explosionCount = 30;

		// Token: 0x040038E3 RID: 14563
		private int ticksToNextEffect;

		// Token: 0x040038E4 RID: 14564
		private IntVec3 nextExplosionCell = IntVec3.Invalid;

		// Token: 0x040038E5 RID: 14565
		private List<Bombardment.BombardmentProjectile> projectiles = new List<Bombardment.BombardmentProjectile>();

		// Token: 0x040038E6 RID: 14566
		public const int EffectiveAreaRadius = 23;

		// Token: 0x040038E7 RID: 14567
		private const int StartRandomFireEveryTicks = 20;

		// Token: 0x040038E8 RID: 14568
		private const int EffectDuration = 60;

		// Token: 0x040038E9 RID: 14569
		private static readonly Material ProjectileMaterial = MaterialPool.MatFrom("Things/Projectile/Bullet_Big", ShaderDatabase.Transparent, Color.white);

		// Token: 0x040038EA RID: 14570
		public static readonly SimpleCurve DistanceChanceFactor = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(1f, 0.1f),
				true
			}
		};

		// Token: 0x020024D8 RID: 9432
		public class BombardmentProjectile : IExposable
		{
			// Token: 0x17001F98 RID: 8088
			// (get) Token: 0x0600CEB4 RID: 52916 RVA: 0x003F44C5 File Offset: 0x003F26C5
			public int LifeTime
			{
				get
				{
					return this.lifeTime;
				}
			}

			// Token: 0x0600CEB5 RID: 52917 RVA: 0x000033AC File Offset: 0x000015AC
			public BombardmentProjectile()
			{
			}

			// Token: 0x0600CEB6 RID: 52918 RVA: 0x003F44CD File Offset: 0x003F26CD
			public BombardmentProjectile(int lifeTime, IntVec3 targetCell)
			{
				this.lifeTime = lifeTime;
				this.maxLifeTime = lifeTime;
				this.targetCell = targetCell;
			}

			// Token: 0x0600CEB7 RID: 52919 RVA: 0x003F44EA File Offset: 0x003F26EA
			public void Tick()
			{
				this.lifeTime--;
			}

			// Token: 0x0600CEB8 RID: 52920 RVA: 0x003F44FC File Offset: 0x003F26FC
			public void Draw(Material material)
			{
				if (this.lifeTime > 0)
				{
					Vector3 pos = this.targetCell.ToVector3() + Vector3.forward * Mathf.Lerp(60f, 0f, 1f - (float)this.lifeTime / (float)this.maxLifeTime);
					pos.z += 1.25f;
					pos.y = AltitudeLayer.MoteOverhead.AltitudeFor();
					Matrix4x4 matrix = default(Matrix4x4);
					matrix.SetTRS(pos, Quaternion.Euler(0f, 180f, 0f), new Vector3(2.5f, 1f, 2.5f));
					Graphics.DrawMesh(MeshPool.plane10, matrix, material, 0);
				}
			}

			// Token: 0x0600CEB9 RID: 52921 RVA: 0x003F45B8 File Offset: 0x003F27B8
			public void ExposeData()
			{
				Scribe_Values.Look<int>(ref this.lifeTime, "lifeTime", 0, false);
				Scribe_Values.Look<int>(ref this.maxLifeTime, "maxLifeTime", 0, false);
				Scribe_Values.Look<IntVec3>(ref this.targetCell, "targetCell", default(IntVec3), false);
			}

			// Token: 0x04008C53 RID: 35923
			private int lifeTime;

			// Token: 0x04008C54 RID: 35924
			private int maxLifeTime;

			// Token: 0x04008C55 RID: 35925
			public IntVec3 targetCell;

			// Token: 0x04008C56 RID: 35926
			private const float StartZ = 60f;

			// Token: 0x04008C57 RID: 35927
			private const float Scale = 2.5f;

			// Token: 0x04008C58 RID: 35928
			private const float Angle = 180f;
		}
	}
}
