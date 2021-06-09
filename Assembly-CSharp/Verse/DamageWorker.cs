using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000101 RID: 257
	public class DamageWorker
	{
		// Token: 0x06000739 RID: 1849 RVA: 0x0009105C File Offset: 0x0008F25C
		public virtual DamageWorker.DamageResult Apply(DamageInfo dinfo, Thing victim)
		{
			DamageWorker.DamageResult damageResult = new DamageWorker.DamageResult();
			if (victim.SpawnedOrAnyParentSpawned)
			{
				ImpactSoundUtility.PlayImpactSound(victim, dinfo.Def.impactSoundType, victim.MapHeld);
			}
			if (victim.def.useHitPoints && dinfo.Def.harmsHealth)
			{
				float num = dinfo.Amount;
				if (victim.def.category == ThingCategory.Building)
				{
					num *= dinfo.Def.buildingDamageFactor;
				}
				if (victim.def.category == ThingCategory.Plant)
				{
					num *= dinfo.Def.plantDamageFactor;
				}
				damageResult.totalDamageDealt = (float)Mathf.Min(victim.HitPoints, GenMath.RoundRandom(num));
				victim.HitPoints -= Mathf.RoundToInt(damageResult.totalDamageDealt);
				if (victim.HitPoints <= 0)
				{
					victim.HitPoints = 0;
					victim.Kill(new DamageInfo?(dinfo), null);
				}
			}
			return damageResult;
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x00091140 File Offset: 0x0008F340
		public virtual void ExplosionStart(Explosion explosion, List<IntVec3> cellsToAffect)
		{
			if (this.def.explosionHeatEnergyPerCell > 1E-45f)
			{
				GenTemperature.PushHeat(explosion.Position, explosion.Map, this.def.explosionHeatEnergyPerCell * (float)cellsToAffect.Count);
			}
			MoteMaker.MakeStaticMote(explosion.Position, explosion.Map, ThingDefOf.Mote_ExplosionFlash, explosion.radius * 6f);
			if (explosion.Map == Find.CurrentMap)
			{
				float magnitude = (explosion.Position.ToVector3Shifted() - Find.Camera.transform.position).magnitude;
				Find.CameraDriver.shaker.DoShake(4f * explosion.radius / magnitude);
			}
			this.ExplosionVisualEffectCenter(explosion);
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x00091204 File Offset: 0x0008F404
		protected virtual void ExplosionVisualEffectCenter(Explosion explosion)
		{
			for (int i = 0; i < 4; i++)
			{
				MoteMaker.ThrowSmoke(explosion.Position.ToVector3Shifted() + Gen.RandomHorizontalVector(explosion.radius * 0.7f), explosion.Map, explosion.radius * 0.6f);
			}
			if (this.def.explosionInteriorMote != null)
			{
				int num = Mathf.RoundToInt(3.1415927f * explosion.radius * explosion.radius / 6f);
				for (int j = 0; j < num; j++)
				{
					MoteMaker.ThrowExplosionInteriorMote(explosion.Position.ToVector3Shifted() + Gen.RandomHorizontalVector(explosion.radius * 0.7f), explosion.Map, this.def.explosionInteriorMote);
				}
			}
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x000912CC File Offset: 0x0008F4CC
		public virtual void ExplosionAffectCell(Explosion explosion, IntVec3 c, List<Thing> damagedThings, List<Thing> ignoredThings, bool canThrowMotes)
		{
			if (this.def.explosionCellMote != null && canThrowMotes)
			{
				Mote mote = c.GetFirstThing(explosion.Map, this.def.explosionCellMote) as Mote;
				if (mote != null)
				{
					mote.spawnTick = Find.TickManager.TicksGame;
				}
				else
				{
					float t = Mathf.Clamp01((explosion.Position - c).LengthHorizontal / explosion.radius);
					Color color = Color.Lerp(this.def.explosionColorCenter, this.def.explosionColorEdge, t);
					MoteMaker.ThrowExplosionCell(c, explosion.Map, this.def.explosionCellMote, color);
				}
			}
			DamageWorker.thingsToAffect.Clear();
			float num = float.MinValue;
			bool flag = false;
			List<Thing> list = explosion.Map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				if (thing.def.category != ThingCategory.Mote && thing.def.category != ThingCategory.Ethereal)
				{
					DamageWorker.thingsToAffect.Add(thing);
					if (thing.def.Fillage == FillCategory.Full && thing.def.Altitude > num)
					{
						flag = true;
						num = thing.def.Altitude;
					}
				}
			}
			for (int j = 0; j < DamageWorker.thingsToAffect.Count; j++)
			{
				if (DamageWorker.thingsToAffect[j].def.Altitude >= num)
				{
					this.ExplosionDamageThing(explosion, DamageWorker.thingsToAffect[j], damagedThings, ignoredThings, c);
				}
			}
			if (!flag)
			{
				this.ExplosionDamageTerrain(explosion, c);
			}
			if (this.def.explosionSnowMeltAmount > 0.0001f)
			{
				float lengthHorizontal = (c - explosion.Position).LengthHorizontal;
				float num2 = 1f - lengthHorizontal / explosion.radius;
				if (num2 > 0f)
				{
					explosion.Map.snowGrid.AddDepth(c, -num2 * this.def.explosionSnowMeltAmount);
				}
			}
			if (this.def == DamageDefOf.Bomb || this.def == DamageDefOf.Flame)
			{
				List<Thing> list2 = explosion.Map.listerThings.ThingsOfDef(ThingDefOf.RectTrigger);
				for (int k = 0; k < list2.Count; k++)
				{
					RectTrigger rectTrigger = (RectTrigger)list2[k];
					if (rectTrigger.activateOnExplosion && rectTrigger.Rect.Contains(c))
					{
						rectTrigger.ActivatedBy(null);
					}
				}
			}
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x0009154C File Offset: 0x0008F74C
		protected virtual void ExplosionDamageThing(Explosion explosion, Thing t, List<Thing> damagedThings, List<Thing> ignoredThings, IntVec3 cell)
		{
			if (t.def.category == ThingCategory.Mote || t.def.category == ThingCategory.Ethereal)
			{
				return;
			}
			if (damagedThings.Contains(t))
			{
				return;
			}
			damagedThings.Add(t);
			if (ignoredThings != null && ignoredThings.Contains(t))
			{
				return;
			}
			if (this.def == DamageDefOf.Bomb && t.def == ThingDefOf.Fire && !t.Destroyed)
			{
				t.Destroy(DestroyMode.Vanish);
				return;
			}
			float angle;
			if (t.Position == explosion.Position)
			{
				angle = (float)Rand.RangeInclusive(0, 359);
			}
			else
			{
				angle = (t.Position - explosion.Position).AngleFlat;
			}
			DamageInfo dinfo = new DamageInfo(this.def, (float)explosion.GetDamageAmountAt(cell), explosion.GetArmorPenetrationAt(cell), angle, explosion.instigator, null, explosion.weapon, DamageInfo.SourceCategory.ThingOrUnknown, explosion.intendedTarget);
			if (this.def.explosionAffectOutsidePartsOnly)
			{
				dinfo.SetBodyRegion(BodyPartHeight.Undefined, BodyPartDepth.Outside);
			}
			BattleLogEntry_ExplosionImpact battleLogEntry_ExplosionImpact = null;
			Pawn pawn = t as Pawn;
			if (pawn != null)
			{
				battleLogEntry_ExplosionImpact = new BattleLogEntry_ExplosionImpact(explosion.instigator, t, explosion.weapon, explosion.projectile, this.def);
				Find.BattleLog.Add(battleLogEntry_ExplosionImpact);
			}
			DamageWorker.DamageResult damageResult = t.TakeDamage(dinfo);
			damageResult.AssociateWithLog(battleLogEntry_ExplosionImpact);
			if (pawn != null && damageResult.wounded && pawn.stances != null)
			{
				pawn.stances.StaggerFor(95);
			}
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x000916B0 File Offset: 0x0008F8B0
		protected virtual void ExplosionDamageTerrain(Explosion explosion, IntVec3 c)
		{
			if (this.def != DamageDefOf.Bomb)
			{
				return;
			}
			if (!explosion.Map.terrainGrid.CanRemoveTopLayerAt(c))
			{
				return;
			}
			TerrainDef terrain = c.GetTerrain(explosion.Map);
			if (terrain.destroyOnBombDamageThreshold < 0f)
			{
				return;
			}
			if ((float)explosion.GetDamageAmountAt(c) >= terrain.destroyOnBombDamageThreshold)
			{
				explosion.Map.terrainGrid.Notify_TerrainDestroyed(c);
			}
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x0000BE00 File Offset: 0x0000A000
		public IEnumerable<IntVec3> ExplosionCellsToHit(Explosion explosion)
		{
			return this.ExplosionCellsToHit(explosion.Position, explosion.Map, explosion.radius, explosion.needLOSToCell1, explosion.needLOSToCell2);
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x0009171C File Offset: 0x0008F91C
		public virtual IEnumerable<IntVec3> ExplosionCellsToHit(IntVec3 center, Map map, float radius, IntVec3? needLOSToCell1 = null, IntVec3? needLOSToCell2 = null)
		{
			DamageWorker.openCells.Clear();
			DamageWorker.adjWallCells.Clear();
			int num = GenRadial.NumCellsInRadius(radius);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = center + GenRadial.RadialPattern[i];
				if (intVec.InBounds(map) && GenSight.LineOfSight(center, intVec, map, true, null, 0, 0))
				{
					if (needLOSToCell1 != null || needLOSToCell2 != null)
					{
						bool flag = needLOSToCell1 != null && GenSight.LineOfSight(needLOSToCell1.Value, intVec, map, false, null, 0, 0);
						bool flag2 = needLOSToCell2 != null && GenSight.LineOfSight(needLOSToCell2.Value, intVec, map, false, null, 0, 0);
						if (!flag && !flag2)
						{
							goto IL_B1;
						}
					}
					DamageWorker.openCells.Add(intVec);
				}
				IL_B1:;
			}
			for (int j = 0; j < DamageWorker.openCells.Count; j++)
			{
				IntVec3 intVec2 = DamageWorker.openCells[j];
				if (intVec2.Walkable(map))
				{
					for (int k = 0; k < 4; k++)
					{
						IntVec3 intVec3 = intVec2 + GenAdj.CardinalDirections[k];
						if (intVec3.InHorDistOf(center, radius) && intVec3.InBounds(map) && !intVec3.Standable(map) && intVec3.GetEdifice(map) != null && !DamageWorker.openCells.Contains(intVec3) && DamageWorker.adjWallCells.Contains(intVec3))
						{
							DamageWorker.adjWallCells.Add(intVec3);
						}
					}
				}
			}
			return DamageWorker.openCells.Concat(DamageWorker.adjWallCells);
		}

		// Token: 0x04000453 RID: 1107
		public DamageDef def;

		// Token: 0x04000454 RID: 1108
		private const float ExplosionCamShakeMultiplier = 4f;

		// Token: 0x04000455 RID: 1109
		private static List<Thing> thingsToAffect = new List<Thing>();

		// Token: 0x04000456 RID: 1110
		private static List<IntVec3> openCells = new List<IntVec3>();

		// Token: 0x04000457 RID: 1111
		private static List<IntVec3> adjWallCells = new List<IntVec3>();

		// Token: 0x02000102 RID: 258
		public class DamageResult
		{
			// Token: 0x17000149 RID: 329
			// (get) Token: 0x06000743 RID: 1859 RVA: 0x0000BE46 File Offset: 0x0000A046
			public BodyPartRecord LastHitPart
			{
				get
				{
					if (this.parts == null)
					{
						return null;
					}
					if (this.parts.Count <= 0)
					{
						return null;
					}
					return this.parts[this.parts.Count - 1];
				}
			}

			// Token: 0x06000744 RID: 1860 RVA: 0x000918A4 File Offset: 0x0008FAA4
			public void AddPart(Thing hitThing, BodyPartRecord part)
			{
				if (this.hitThing != null && this.hitThing != hitThing)
				{
					Log.ErrorOnce("Single damage worker referring to multiple things; will cause issues with combat log", 30667935, false);
				}
				this.hitThing = hitThing;
				if (this.parts == null)
				{
					this.parts = new List<BodyPartRecord>();
				}
				this.parts.Add(part);
			}

			// Token: 0x06000745 RID: 1861 RVA: 0x0000BE7A File Offset: 0x0000A07A
			public void AddHediff(Hediff hediff)
			{
				if (this.hediffs == null)
				{
					this.hediffs = new List<Hediff>();
				}
				this.hediffs.Add(hediff);
			}

			// Token: 0x06000746 RID: 1862 RVA: 0x000918F8 File Offset: 0x0008FAF8
			public void AssociateWithLog(LogEntry_DamageResult log)
			{
				if (log == null)
				{
					return;
				}
				Pawn hitPawn = this.hitThing as Pawn;
				if (hitPawn != null)
				{
					List<BodyPartRecord> list = null;
					List<bool> recipientPartsDestroyed = null;
					if (!this.parts.NullOrEmpty<BodyPartRecord>() && hitPawn != null)
					{
						list = this.parts.Distinct<BodyPartRecord>().ToList<BodyPartRecord>();
						recipientPartsDestroyed = (from part in list
						select hitPawn.health.hediffSet.GetPartHealth(part) <= 0f).ToList<bool>();
					}
					log.FillTargets(list, recipientPartsDestroyed, this.deflected);
				}
				if (this.hediffs != null)
				{
					for (int i = 0; i < this.hediffs.Count; i++)
					{
						this.hediffs[i].combatLogEntry = new WeakReference<LogEntry>(log);
						this.hediffs[i].combatLogText = log.ToGameStringFromPOV(null, false);
					}
				}
			}

			// Token: 0x04000458 RID: 1112
			public bool wounded;

			// Token: 0x04000459 RID: 1113
			public bool headshot;

			// Token: 0x0400045A RID: 1114
			public bool deflected;

			// Token: 0x0400045B RID: 1115
			public bool stunned;

			// Token: 0x0400045C RID: 1116
			public bool deflectedByMetalArmor;

			// Token: 0x0400045D RID: 1117
			public bool diminished;

			// Token: 0x0400045E RID: 1118
			public bool diminishedByMetalArmor;

			// Token: 0x0400045F RID: 1119
			public Thing hitThing;

			// Token: 0x04000460 RID: 1120
			public List<BodyPartRecord> parts;

			// Token: 0x04000461 RID: 1121
			public List<Hediff> hediffs;

			// Token: 0x04000462 RID: 1122
			public float totalDamageDealt;
		}
	}
}
