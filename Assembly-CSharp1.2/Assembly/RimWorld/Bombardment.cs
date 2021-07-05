using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020016E8 RID: 5864
	[StaticConstructorOnStartup]
	public class Bombardment : OrbitalStrike
	{
		// Token: 0x060080CF RID: 32975 RVA: 0x00056889 File Offset: 0x00054A89
		public override void StartStrike()
		{
			this.duration = this.bombIntervalTicks * this.explosionCount;
			base.StartStrike();
		}

		// Token: 0x060080D0 RID: 32976 RVA: 0x00262850 File Offset: 0x00260A50
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

		// Token: 0x060080D1 RID: 32977 RVA: 0x002628B8 File Offset: 0x00260AB8
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
					IntVec3 targetCell = this.projectiles[i].targetCell;
					Map map = base.Map;
					float randomInRange = this.explosionRadiusRange.RandomInRange;
					DamageDef bomb = DamageDefOf.Bomb;
					Thing instigator = this.instigator;
					int damAmount = -1;
					float armorPenetration = -1f;
					SoundDef explosionSound = null;
					ThingDef def = this.def;
					GenExplosion.DoExplosion(targetCell, map, randomInRange, bomb, instigator, damAmount, armorPenetration, explosionSound, this.weaponDef, def, null, null, 0f, 1, false, null, 0f, 1, 0f, false, null, null);
					this.projectiles.RemoveAt(i);
				}
			}
		}

		// Token: 0x060080D2 RID: 32978 RVA: 0x00262A14 File Offset: 0x00260C14
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

		// Token: 0x060080D3 RID: 32979 RVA: 0x00262A64 File Offset: 0x00260C64
		private void StartRandomFire()
		{
			FireUtility.TryStartFireIn((from x in GenRadial.RadialCellsAround(base.Position, (float)this.randomFireRadius, true)
			where x.InBounds(base.Map)
			select x).RandomElementByWeight((IntVec3 x) => Bombardment.DistanceChanceFactor.Evaluate(x.DistanceTo(base.Position))), base.Map, Rand.Range(0.1f, 0.925f));
		}

		// Token: 0x060080D4 RID: 32980 RVA: 0x000568A4 File Offset: 0x00054AA4
		private void GetNextExplosionCell()
		{
			this.nextExplosionCell = (from x in GenRadial.RadialCellsAround(base.Position, this.impactAreaRadius, true)
			where x.InBounds(base.Map)
			select x).RandomElementByWeight((IntVec3 x) => Bombardment.DistanceChanceFactor.Evaluate(x.DistanceTo(base.Position) / this.impactAreaRadius));
		}

		// Token: 0x060080D5 RID: 32981 RVA: 0x00262AC4 File Offset: 0x00260CC4
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

		// Token: 0x0400536D RID: 21357
		public float impactAreaRadius = 15f;

		// Token: 0x0400536E RID: 21358
		public FloatRange explosionRadiusRange = new FloatRange(6f, 8f);

		// Token: 0x0400536F RID: 21359
		public int randomFireRadius = 25;

		// Token: 0x04005370 RID: 21360
		public int bombIntervalTicks = 18;

		// Token: 0x04005371 RID: 21361
		public int warmupTicks = 60;

		// Token: 0x04005372 RID: 21362
		public int explosionCount = 30;

		// Token: 0x04005373 RID: 21363
		private int ticksToNextEffect;

		// Token: 0x04005374 RID: 21364
		private IntVec3 nextExplosionCell = IntVec3.Invalid;

		// Token: 0x04005375 RID: 21365
		private List<Bombardment.BombardmentProjectile> projectiles = new List<Bombardment.BombardmentProjectile>();

		// Token: 0x04005376 RID: 21366
		public const int EffectiveAreaRadius = 23;

		// Token: 0x04005377 RID: 21367
		private const int StartRandomFireEveryTicks = 20;

		// Token: 0x04005378 RID: 21368
		private const int EffectDuration = 60;

		// Token: 0x04005379 RID: 21369
		private static readonly Material ProjectileMaterial = MaterialPool.MatFrom("Things/Projectile/Bullet_Big", ShaderDatabase.Transparent, Color.white);

		// Token: 0x0400537A RID: 21370
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

		// Token: 0x020016E9 RID: 5865
		public class BombardmentProjectile : IExposable
		{
			// Token: 0x17001405 RID: 5125
			// (get) Token: 0x060080DC RID: 32988 RVA: 0x00056917 File Offset: 0x00054B17
			public int LifeTime
			{
				get
				{
					return this.lifeTime;
				}
			}

			// Token: 0x060080DD RID: 32989 RVA: 0x00006B8B File Offset: 0x00004D8B
			public BombardmentProjectile()
			{
			}

			// Token: 0x060080DE RID: 32990 RVA: 0x0005691F File Offset: 0x00054B1F
			public BombardmentProjectile(int lifeTime, IntVec3 targetCell)
			{
				this.lifeTime = lifeTime;
				this.maxLifeTime = lifeTime;
				this.targetCell = targetCell;
			}

			// Token: 0x060080DF RID: 32991 RVA: 0x0005693C File Offset: 0x00054B3C
			public void Tick()
			{
				this.lifeTime--;
			}

			// Token: 0x060080E0 RID: 32992 RVA: 0x00262C80 File Offset: 0x00260E80
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

			// Token: 0x060080E1 RID: 32993 RVA: 0x00262D3C File Offset: 0x00260F3C
			public void ExposeData()
			{
				Scribe_Values.Look<int>(ref this.lifeTime, "lifeTime", 0, false);
				Scribe_Values.Look<int>(ref this.maxLifeTime, "maxLifeTime", 0, false);
				Scribe_Values.Look<IntVec3>(ref this.targetCell, "targetCell", default(IntVec3), false);
			}

			// Token: 0x0400537B RID: 21371
			private int lifeTime;

			// Token: 0x0400537C RID: 21372
			private int maxLifeTime;

			// Token: 0x0400537D RID: 21373
			public IntVec3 targetCell;

			// Token: 0x0400537E RID: 21374
			private const float StartZ = 60f;

			// Token: 0x0400537F RID: 21375
			private const float Scale = 2.5f;

			// Token: 0x04005380 RID: 21376
			private const float Angle = 180f;
		}
	}
}
