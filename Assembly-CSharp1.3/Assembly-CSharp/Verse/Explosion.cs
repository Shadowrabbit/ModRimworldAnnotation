using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200033E RID: 830
	public class Explosion : Thing
	{
		// Token: 0x060017A5 RID: 6053 RVA: 0x0008D2A4 File Offset: 0x0008B4A4
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.cellsToAffect = SimplePool<List<IntVec3>>.Get();
				this.cellsToAffect.Clear();
				this.damagedThings = SimplePool<List<Thing>>.Get();
				this.damagedThings.Clear();
				this.addedCellsAffectedOnlyByDamage = SimplePool<HashSet<IntVec3>>.Get();
				this.addedCellsAffectedOnlyByDamage.Clear();
			}
		}

		// Token: 0x060017A6 RID: 6054 RVA: 0x0008D300 File Offset: 0x0008B500
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			base.DeSpawn(mode);
			this.cellsToAffect.Clear();
			SimplePool<List<IntVec3>>.Return(this.cellsToAffect);
			this.cellsToAffect = null;
			this.damagedThings.Clear();
			SimplePool<List<Thing>>.Return(this.damagedThings);
			this.damagedThings = null;
			this.addedCellsAffectedOnlyByDamage.Clear();
			SimplePool<HashSet<IntVec3>>.Return(this.addedCellsAffectedOnlyByDamage);
			this.addedCellsAffectedOnlyByDamage = null;
		}

		// Token: 0x060017A7 RID: 6055 RVA: 0x0008D36C File Offset: 0x0008B56C
		public virtual void StartExplosion(SoundDef explosionSound, List<Thing> ignoredThings)
		{
			if (!base.Spawned)
			{
				Log.Error("Called StartExplosion() on unspawned thing.");
				return;
			}
			this.startTick = Find.TickManager.TicksGame;
			this.ignoredThings = ignoredThings;
			this.cellsToAffect.Clear();
			this.damagedThings.Clear();
			this.addedCellsAffectedOnlyByDamage.Clear();
			this.cellsToAffect.AddRange(this.damType.Worker.ExplosionCellsToHit(this));
			if (this.applyDamageToExplosionCellsNeighbors)
			{
				this.AddCellsNeighbors(this.cellsToAffect);
			}
			this.damType.Worker.ExplosionStart(this, this.cellsToAffect);
			this.PlayExplosionSound(explosionSound);
			FleckMaker.WaterSplash(base.Position.ToVector3Shifted(), base.Map, this.radius * 6f, 20f);
			this.cellsToAffect.Sort((IntVec3 a, IntVec3 b) => this.GetCellAffectTick(b).CompareTo(this.GetCellAffectTick(a)));
			RegionTraverser.BreadthFirstTraverse(base.Position, base.Map, (Region from, Region to) => true, delegate(Region x)
			{
				List<Thing> allThings = x.ListerThings.AllThings;
				for (int i = allThings.Count - 1; i >= 0; i--)
				{
					if (allThings[i].Spawned)
					{
						allThings[i].Notify_Explosion(this);
					}
				}
				return false;
			}, 25, RegionType.Set_Passable);
		}

		// Token: 0x060017A8 RID: 6056 RVA: 0x0008D494 File Offset: 0x0008B694
		public override void Tick()
		{
			int ticksGame = Find.TickManager.TicksGame;
			int num = this.cellsToAffect.Count - 1;
			while (num >= 0 && ticksGame >= this.GetCellAffectTick(this.cellsToAffect[num]))
			{
				try
				{
					this.AffectCell(this.cellsToAffect[num]);
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Explosion could not affect cell ",
						this.cellsToAffect[num],
						": ",
						ex
					}));
				}
				this.cellsToAffect.RemoveAt(num);
				num--;
			}
			if (!this.cellsToAffect.Any<IntVec3>())
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x060017A9 RID: 6057 RVA: 0x0008D558 File Offset: 0x0008B758
		public int GetDamageAmountAt(IntVec3 c)
		{
			if (!this.damageFalloff)
			{
				return this.damAmount;
			}
			float t = c.DistanceTo(base.Position) / this.radius;
			return Mathf.Max(GenMath.RoundRandom(Mathf.Lerp((float)this.damAmount, (float)this.damAmount * 0.2f, t)), 1);
		}

		// Token: 0x060017AA RID: 6058 RVA: 0x0008D5B0 File Offset: 0x0008B7B0
		public float GetArmorPenetrationAt(IntVec3 c)
		{
			if (!this.damageFalloff)
			{
				return this.armorPenetration;
			}
			float t = c.DistanceTo(base.Position) / this.radius;
			return Mathf.Lerp(this.armorPenetration, this.armorPenetration * 0.2f, t);
		}

		// Token: 0x060017AB RID: 6059 RVA: 0x0008D5F8 File Offset: 0x0008B7F8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.radius, "radius", 0f, false);
			Scribe_Defs.Look<DamageDef>(ref this.damType, "damType");
			Scribe_Values.Look<int>(ref this.damAmount, "damAmount", 0, false);
			Scribe_Values.Look<float>(ref this.armorPenetration, "armorPenetration", 0f, false);
			Scribe_References.Look<Thing>(ref this.instigator, "instigator", false);
			Scribe_Defs.Look<ThingDef>(ref this.weapon, "weapon");
			Scribe_Defs.Look<ThingDef>(ref this.projectile, "projectile");
			Scribe_References.Look<Thing>(ref this.intendedTarget, "intendedTarget", false);
			Scribe_Values.Look<bool>(ref this.applyDamageToExplosionCellsNeighbors, "applyDamageToExplosionCellsNeighbors", false, false);
			Scribe_Defs.Look<ThingDef>(ref this.preExplosionSpawnThingDef, "preExplosionSpawnThingDef");
			Scribe_Values.Look<float>(ref this.preExplosionSpawnChance, "preExplosionSpawnChance", 0f, false);
			Scribe_Values.Look<int>(ref this.preExplosionSpawnThingCount, "preExplosionSpawnThingCount", 1, false);
			Scribe_Defs.Look<ThingDef>(ref this.postExplosionSpawnThingDef, "postExplosionSpawnThingDef");
			Scribe_Values.Look<float>(ref this.postExplosionSpawnChance, "postExplosionSpawnChance", 0f, false);
			Scribe_Values.Look<int>(ref this.postExplosionSpawnThingCount, "postExplosionSpawnThingCount", 1, false);
			Scribe_Values.Look<float>(ref this.chanceToStartFire, "chanceToStartFire", 0f, false);
			Scribe_Values.Look<bool>(ref this.damageFalloff, "dealMoreDamageAtCenter", false, false);
			Scribe_Values.Look<IntVec3?>(ref this.needLOSToCell1, "needLOSToCell1", null, false);
			Scribe_Values.Look<IntVec3?>(ref this.needLOSToCell2, "needLOSToCell2", null, false);
			Scribe_Values.Look<int>(ref this.startTick, "startTick", 0, false);
			Scribe_Collections.Look<IntVec3>(ref this.cellsToAffect, "cellsToAffect", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<Thing>(ref this.damagedThings, "damagedThings", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Thing>(ref this.ignoredThings, "ignoredThings", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<IntVec3>(ref this.addedCellsAffectedOnlyByDamage, "addedCellsAffectedOnlyByDamage", LookMode.Value);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.damagedThings != null)
				{
					this.damagedThings.RemoveAll((Thing x) => x == null);
				}
				if (this.ignoredThings != null)
				{
					this.ignoredThings.RemoveAll((Thing x) => x == null);
				}
			}
		}

		// Token: 0x060017AC RID: 6060 RVA: 0x0008D84C File Offset: 0x0008BA4C
		private int GetCellAffectTick(IntVec3 cell)
		{
			return this.startTick + (int)((cell - base.Position).LengthHorizontal * 1.5f);
		}

		// Token: 0x060017AD RID: 6061 RVA: 0x0008D87C File Offset: 0x0008BA7C
		private void AffectCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return;
			}
			bool flag = this.ShouldCellBeAffectedOnlyByDamage(c);
			if (!flag && Rand.Chance(this.preExplosionSpawnChance) && c.Walkable(base.Map))
			{
				this.TrySpawnExplosionThing(this.preExplosionSpawnThingDef, c, this.preExplosionSpawnThingCount);
			}
			this.damType.Worker.ExplosionAffectCell(this, c, this.damagedThings, this.ignoredThings, !flag);
			if (!flag && Rand.Chance(this.postExplosionSpawnChance) && c.Walkable(base.Map))
			{
				this.TrySpawnExplosionThing(this.postExplosionSpawnThingDef, c, this.postExplosionSpawnThingCount);
			}
			float num = this.chanceToStartFire;
			if (this.damageFalloff)
			{
				num *= Mathf.Lerp(1f, 0.2f, c.DistanceTo(base.Position) / this.radius);
			}
			if (Rand.Chance(num))
			{
				FireUtility.TryStartFireIn(c, base.Map, Rand.Range(0.1f, 0.925f));
			}
		}

		// Token: 0x060017AE RID: 6062 RVA: 0x0008D97C File Offset: 0x0008BB7C
		private void TrySpawnExplosionThing(ThingDef thingDef, IntVec3 c, int count)
		{
			if (thingDef == null)
			{
				return;
			}
			if (thingDef.IsFilth)
			{
				FilthMaker.TryMakeFilth(c, base.Map, thingDef, count, FilthSourceFlags.None);
				return;
			}
			Thing thing = ThingMaker.MakeThing(thingDef, null);
			thing.stackCount = count;
			GenSpawn.Spawn(thing, c, base.Map, WipeMode.Vanish);
		}

		// Token: 0x060017AF RID: 6063 RVA: 0x0008D9B8 File Offset: 0x0008BBB8
		private void PlayExplosionSound(SoundDef explosionSound)
		{
			bool flag;
			if (Prefs.DevMode)
			{
				flag = (explosionSound != null);
			}
			else
			{
				flag = !explosionSound.NullOrUndefined();
			}
			if (flag)
			{
				explosionSound.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
				return;
			}
			this.damType.soundExplosion.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
		}

		// Token: 0x060017B0 RID: 6064 RVA: 0x0008DA28 File Offset: 0x0008BC28
		private void AddCellsNeighbors(List<IntVec3> cells)
		{
			Explosion.tmpCells.Clear();
			this.addedCellsAffectedOnlyByDamage.Clear();
			for (int i = 0; i < cells.Count; i++)
			{
				Explosion.tmpCells.Add(cells[i]);
			}
			for (int j = 0; j < cells.Count; j++)
			{
				if (cells[j].Walkable(base.Map))
				{
					for (int k = 0; k < GenAdj.AdjacentCells.Length; k++)
					{
						IntVec3 intVec = cells[j] + GenAdj.AdjacentCells[k];
						if (intVec.InBounds(base.Map) && Explosion.tmpCells.Add(intVec))
						{
							this.addedCellsAffectedOnlyByDamage.Add(intVec);
						}
					}
				}
			}
			cells.Clear();
			foreach (IntVec3 item in Explosion.tmpCells)
			{
				cells.Add(item);
			}
			Explosion.tmpCells.Clear();
		}

		// Token: 0x060017B1 RID: 6065 RVA: 0x0008DB3C File Offset: 0x0008BD3C
		private bool ShouldCellBeAffectedOnlyByDamage(IntVec3 c)
		{
			return this.applyDamageToExplosionCellsNeighbors && this.addedCellsAffectedOnlyByDamage.Contains(c);
		}

		// Token: 0x0400103A RID: 4154
		public float radius;

		// Token: 0x0400103B RID: 4155
		public DamageDef damType;

		// Token: 0x0400103C RID: 4156
		public int damAmount;

		// Token: 0x0400103D RID: 4157
		public float armorPenetration;

		// Token: 0x0400103E RID: 4158
		public Thing instigator;

		// Token: 0x0400103F RID: 4159
		public ThingDef weapon;

		// Token: 0x04001040 RID: 4160
		public ThingDef projectile;

		// Token: 0x04001041 RID: 4161
		public Thing intendedTarget;

		// Token: 0x04001042 RID: 4162
		public bool applyDamageToExplosionCellsNeighbors;

		// Token: 0x04001043 RID: 4163
		public ThingDef preExplosionSpawnThingDef;

		// Token: 0x04001044 RID: 4164
		public float preExplosionSpawnChance;

		// Token: 0x04001045 RID: 4165
		public int preExplosionSpawnThingCount = 1;

		// Token: 0x04001046 RID: 4166
		public ThingDef postExplosionSpawnThingDef;

		// Token: 0x04001047 RID: 4167
		public float postExplosionSpawnChance;

		// Token: 0x04001048 RID: 4168
		public int postExplosionSpawnThingCount = 1;

		// Token: 0x04001049 RID: 4169
		public float chanceToStartFire;

		// Token: 0x0400104A RID: 4170
		public bool damageFalloff;

		// Token: 0x0400104B RID: 4171
		public IntVec3? needLOSToCell1;

		// Token: 0x0400104C RID: 4172
		public IntVec3? needLOSToCell2;

		// Token: 0x0400104D RID: 4173
		private int startTick;

		// Token: 0x0400104E RID: 4174
		private List<IntVec3> cellsToAffect;

		// Token: 0x0400104F RID: 4175
		private List<Thing> damagedThings;

		// Token: 0x04001050 RID: 4176
		private List<Thing> ignoredThings;

		// Token: 0x04001051 RID: 4177
		private HashSet<IntVec3> addedCellsAffectedOnlyByDamage;

		// Token: 0x04001052 RID: 4178
		private const float DamageFactorAtEdge = 0.2f;

		// Token: 0x04001053 RID: 4179
		private static HashSet<IntVec3> tmpCells = new HashSet<IntVec3>();
	}
}
