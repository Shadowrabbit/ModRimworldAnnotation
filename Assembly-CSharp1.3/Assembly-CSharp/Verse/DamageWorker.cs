using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200009E RID: 158
	public class DamageWorker
	{
		// Token: 0x06000534 RID: 1332 RVA: 0x0001AB94 File Offset: 0x00018D94
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

		// Token: 0x06000535 RID: 1333 RVA: 0x0001AC78 File Offset: 0x00018E78
		public virtual void ExplosionStart(Explosion explosion, List<IntVec3> cellsToAffect)
		{
			if (this.def.explosionHeatEnergyPerCell > 1E-45f)
			{
				GenTemperature.PushHeat(explosion.Position, explosion.Map, this.def.explosionHeatEnergyPerCell * (float)cellsToAffect.Count);
			}
			FleckMaker.Static(explosion.Position, explosion.Map, FleckDefOf.ExplosionFlash, explosion.radius * 6f);
			if (explosion.Map == Find.CurrentMap)
			{
				float magnitude = (explosion.Position.ToVector3Shifted() - Find.Camera.transform.position).magnitude;
				Find.CameraDriver.shaker.DoShake(4f * explosion.radius / magnitude);
			}
			this.ExplosionVisualEffectCenter(explosion);
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x0001AD3C File Offset: 0x00018F3C
		protected virtual void ExplosionVisualEffectCenter(Explosion explosion)
		{
			for (int i = 0; i < 4; i++)
			{
				FleckMaker.ThrowSmoke(explosion.Position.ToVector3Shifted() + Gen.RandomHorizontalVector(explosion.radius * 0.7f), explosion.Map, explosion.radius * 0.6f);
			}
			if (this.def.explosionInteriorMote != null || this.def.explosionInteriorFleck != null)
			{
				int num = Mathf.RoundToInt(3.1415927f * explosion.radius * explosion.radius / 6f);
				for (int j = 0; j < num; j++)
				{
					if (this.def.explosionInteriorFleck != null)
					{
						FleckMaker.ThrowExplosionInterior(explosion.Position.ToVector3Shifted() + Gen.RandomHorizontalVector(explosion.radius * 0.7f), explosion.Map, this.def.explosionInteriorFleck);
					}
					else
					{
						MoteMaker.ThrowExplosionInteriorMote(explosion.Position.ToVector3Shifted() + Gen.RandomHorizontalVector(explosion.radius * 0.7f), explosion.Map, this.def.explosionInteriorMote);
					}
				}
			}
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x0001AE64 File Offset: 0x00019064
		public virtual void ExplosionAffectCell(Explosion explosion, IntVec3 c, List<Thing> damagedThings, List<Thing> ignoredThings, bool canThrowMotes)
		{
			if ((this.def.explosionCellMote != null || this.def.explosionCellFleck != null) && canThrowMotes)
			{
				float t = Mathf.Clamp01((explosion.Position - c).LengthHorizontal / explosion.radius);
				Color color = Color.Lerp(this.def.explosionColorCenter, this.def.explosionColorEdge, t);
				if (this.def.explosionCellMote != null)
				{
					Mote mote = c.GetFirstThing(explosion.Map, this.def.explosionCellMote) as Mote;
					if (mote != null)
					{
						mote.spawnTick = Find.TickManager.TicksGame;
					}
					else
					{
						MoteMaker.ThrowExplosionCell(c, explosion.Map, this.def.explosionCellMote, color);
					}
				}
				else
				{
					FleckMaker.ThrowExplosionCell(c, explosion.Map, this.def.explosionCellFleck, color);
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

		// Token: 0x06000538 RID: 1336 RVA: 0x0001B11C File Offset: 0x0001931C
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
			DamageInfo dinfo = new DamageInfo(this.def, (float)explosion.GetDamageAmountAt(cell), explosion.GetArmorPenetrationAt(cell), angle, explosion.instigator, null, explosion.weapon, DamageInfo.SourceCategory.ThingOrUnknown, explosion.intendedTarget, true, true);
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

		// Token: 0x06000539 RID: 1337 RVA: 0x0001B284 File Offset: 0x00019484
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

		// Token: 0x0600053A RID: 1338 RVA: 0x0001B2EF File Offset: 0x000194EF
		public IEnumerable<IntVec3> ExplosionCellsToHit(Explosion explosion)
		{
			return this.ExplosionCellsToHit(explosion.Position, explosion.Map, explosion.radius, explosion.needLOSToCell1, explosion.needLOSToCell2);
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x0001B318 File Offset: 0x00019518
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
						if (intVec3.InHorDistOf(center, radius) && intVec3.InBounds(map) && !intVec3.Standable(map) && intVec3.GetEdifice(map) != null && !DamageWorker.openCells.Contains(intVec3) && !DamageWorker.adjWallCells.Contains(intVec3))
						{
							DamageWorker.adjWallCells.Add(intVec3);
						}
					}
				}
			}
			return DamageWorker.openCells.Concat(DamageWorker.adjWallCells);
		}

		// Token: 0x04000280 RID: 640
		public DamageDef def;

		// Token: 0x04000281 RID: 641
		private const float ExplosionCamShakeMultiplier = 4f;

		// Token: 0x04000282 RID: 642
		private static List<Thing> thingsToAffect = new List<Thing>();

		// Token: 0x04000283 RID: 643
		private static List<IntVec3> openCells = new List<IntVec3>();

		// Token: 0x04000284 RID: 644
		private static List<IntVec3> adjWallCells = new List<IntVec3>();

		// Token: 0x020018C6 RID: 6342
		public class DamageResult
		{
			// Token: 0x170018A0 RID: 6304
			// (get) Token: 0x060094E4 RID: 38116 RVA: 0x0035082B File Offset: 0x0034EA2B
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

			// Token: 0x060094E5 RID: 38117 RVA: 0x00350860 File Offset: 0x0034EA60
			public void AddPart(Thing hitThing, BodyPartRecord part)
			{
				if (this.hitThing != null && this.hitThing != hitThing)
				{
					Log.ErrorOnce("Single damage worker referring to multiple things; will cause issues with combat log", 30667935);
				}
				this.hitThing = hitThing;
				if (this.parts == null)
				{
					this.parts = new List<BodyPartRecord>();
				}
				this.parts.Add(part);
			}

			// Token: 0x060094E6 RID: 38118 RVA: 0x003508B3 File Offset: 0x0034EAB3
			public void AddHediff(Hediff hediff)
			{
				if (this.hediffs == null)
				{
					this.hediffs = new List<Hediff>();
				}
				this.hediffs.Add(hediff);
			}

			// Token: 0x060094E7 RID: 38119 RVA: 0x003508D4 File Offset: 0x0034EAD4
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

			// Token: 0x04005EC3 RID: 24259
			public bool wounded;

			// Token: 0x04005EC4 RID: 24260
			public bool headshot;

			// Token: 0x04005EC5 RID: 24261
			public bool deflected;

			// Token: 0x04005EC6 RID: 24262
			public bool stunned;

			// Token: 0x04005EC7 RID: 24263
			public bool deflectedByMetalArmor;

			// Token: 0x04005EC8 RID: 24264
			public bool diminished;

			// Token: 0x04005EC9 RID: 24265
			public bool diminishedByMetalArmor;

			// Token: 0x04005ECA RID: 24266
			public Thing hitThing;

			// Token: 0x04005ECB RID: 24267
			public List<BodyPartRecord> parts;

			// Token: 0x04005ECC RID: 24268
			public List<Hediff> hediffs;

			// Token: 0x04005ECD RID: 24269
			public float totalDamageDealt;
		}
	}
}
