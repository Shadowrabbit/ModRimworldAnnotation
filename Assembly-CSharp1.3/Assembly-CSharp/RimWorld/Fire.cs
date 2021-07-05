using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020010A9 RID: 4265
	public class Fire : AttachableThing, ISizeReporter
	{
		// Token: 0x17001167 RID: 4455
		// (get) Token: 0x060065B8 RID: 26040 RVA: 0x00225C23 File Offset: 0x00223E23
		public int TicksSinceSpawn
		{
			get
			{
				return this.ticksSinceSpawn;
			}
		}

		// Token: 0x17001168 RID: 4456
		// (get) Token: 0x060065B9 RID: 26041 RVA: 0x00225C2B File Offset: 0x00223E2B
		public override string Label
		{
			get
			{
				if (this.parent != null)
				{
					return "FireOn".Translate(this.parent.LabelCap, this.parent);
				}
				return this.def.label;
			}
		}

		// Token: 0x17001169 RID: 4457
		// (get) Token: 0x060065BA RID: 26042 RVA: 0x00225C6C File Offset: 0x00223E6C
		public override string InspectStringAddon
		{
			get
			{
				return "Burning".Translate() + " (" + "FireSizeLower".Translate((this.fireSize * 100f).ToString("F0")) + ")";
			}
		}

		// Token: 0x1700116A RID: 4458
		// (get) Token: 0x060065BB RID: 26043 RVA: 0x00225CCC File Offset: 0x00223ECC
		private float SpreadInterval
		{
			get
			{
				float num = 150f - (this.fireSize - 1f) * 40f;
				if (num < 75f)
				{
					num = 75f;
				}
				return num;
			}
		}

		// Token: 0x060065BC RID: 26044 RVA: 0x00225D01 File Offset: 0x00223F01
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksSinceSpawn, "ticksSinceSpawn", 0, false);
			Scribe_Values.Look<float>(ref this.fireSize, "fireSize", 0f, false);
		}

		// Token: 0x060065BD RID: 26045 RVA: 0x00225D31 File Offset: 0x00223F31
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.RecalcPathsOnAndAroundMe(map);
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.HomeArea, this, OpportunityType.Important);
			this.ticksSinceSpread = (int)(this.SpreadInterval * Rand.Value);
		}

		// Token: 0x060065BE RID: 26046 RVA: 0x00225D61 File Offset: 0x00223F61
		public float CurrentSize()
		{
			return this.fireSize;
		}

		// Token: 0x060065BF RID: 26047 RVA: 0x00225D6C File Offset: 0x00223F6C
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			if (this.sustainer != null)
			{
				if (this.sustainer.externalParams.sizeAggregator == null)
				{
					this.sustainer.externalParams.sizeAggregator = new SoundSizeAggregator();
				}
				this.sustainer.externalParams.sizeAggregator.RemoveReporter(this);
			}
			Map map = base.Map;
			base.DeSpawn(mode);
			this.RecalcPathsOnAndAroundMe(map);
		}

		// Token: 0x060065C0 RID: 26048 RVA: 0x00225DD4 File Offset: 0x00223FD4
		private void RecalcPathsOnAndAroundMe(Map map)
		{
			IntVec3[] adjacentCellsAndInside = GenAdj.AdjacentCellsAndInside;
			for (int i = 0; i < adjacentCellsAndInside.Length; i++)
			{
				IntVec3 c = base.Position + adjacentCellsAndInside[i];
				if (c.InBounds(map))
				{
					map.pathing.RecalculatePerceivedPathCostAt(c);
				}
			}
		}

		// Token: 0x060065C1 RID: 26049 RVA: 0x00225E20 File Offset: 0x00224020
		public override void AttachTo(Thing parent)
		{
			base.AttachTo(parent);
			Pawn pawn = parent as Pawn;
			if (pawn != null)
			{
				TaleRecorder.RecordTale(TaleDefOf.WasOnFire, new object[]
				{
					pawn
				});
			}
		}

		// Token: 0x060065C2 RID: 26050 RVA: 0x00225E54 File Offset: 0x00224054
		public override void Tick()
		{
			this.ticksSinceSpawn++;
			if (Fire.lastFireCountUpdateTick != Find.TickManager.TicksGame)
			{
				Fire.fireCount = base.Map.listerThings.ThingsOfDef(this.def).Count;
				Fire.lastFireCountUpdateTick = Find.TickManager.TicksGame;
			}
			if (this.sustainer != null)
			{
				this.sustainer.Maintain();
			}
			else if (!base.Position.Fogged(base.Map))
			{
				SoundInfo info = SoundInfo.InMap(new TargetInfo(base.Position, base.Map, false), MaintenanceType.PerTick);
				this.sustainer = SustainerAggregatorUtility.AggregateOrSpawnSustainerFor(this, SoundDefOf.FireBurning, info);
			}
			this.ticksUntilSmoke--;
			if (this.ticksUntilSmoke <= 0)
			{
				this.SpawnSmokeParticles();
			}
			if (Fire.fireCount < 15 && this.fireSize > 0.7f && Rand.Value < this.fireSize * 0.01f)
			{
				FleckMaker.ThrowMicroSparks(this.DrawPos, base.Map);
			}
			if (this.fireSize > 1f)
			{
				this.ticksSinceSpread++;
				if ((float)this.ticksSinceSpread >= this.SpreadInterval)
				{
					this.TrySpread();
					this.ticksSinceSpread = 0;
				}
			}
			if (this.IsHashIntervalTick(150))
			{
				this.DoComplexCalcs();
			}
			if (this.ticksSinceSpawn >= 7500)
			{
				this.TryBurnFloor();
			}
		}

		// Token: 0x060065C3 RID: 26051 RVA: 0x00225FB8 File Offset: 0x002241B8
		private void SpawnSmokeParticles()
		{
			if (Fire.fireCount < 15)
			{
				FleckMaker.ThrowSmoke(this.DrawPos, base.Map, this.fireSize);
			}
			if (this.fireSize > 0.5f && this.parent == null)
			{
				FleckMaker.ThrowFireGlow(base.Position.ToVector3Shifted(), base.Map, this.fireSize);
			}
			float num = this.fireSize / 2f;
			if (num > 1f)
			{
				num = 1f;
			}
			num = 1f - num;
			this.ticksUntilSmoke = Fire.SmokeIntervalRange.Lerped(num) + (int)(10f * Rand.Value);
		}

		// Token: 0x060065C4 RID: 26052 RVA: 0x00226060 File Offset: 0x00224260
		private void DoComplexCalcs()
		{
			bool flag = false;
			Fire.flammableList.Clear();
			this.flammabilityMax = 0f;
			if (!base.Position.GetTerrain(base.Map).extinguishesFire)
			{
				if (this.parent == null)
				{
					if (base.Position.TerrainFlammableNow(base.Map))
					{
						this.flammabilityMax = base.Position.GetTerrain(base.Map).GetStatValueAbstract(StatDefOf.Flammability, null);
					}
					List<Thing> list = base.Map.thingGrid.ThingsListAt(base.Position);
					for (int i = 0; i < list.Count; i++)
					{
						Thing thing = list[i];
						if (thing is Building_Door)
						{
							flag = true;
						}
						float statValue = thing.GetStatValue(StatDefOf.Flammability, true);
						if (statValue >= 0.01f)
						{
							Fire.flammableList.Add(list[i]);
							if (statValue > this.flammabilityMax)
							{
								this.flammabilityMax = statValue;
							}
							if (this.parent == null && this.fireSize > 0.4f && list[i].def.category == ThingCategory.Pawn && Rand.Chance(FireUtility.ChanceToAttachFireCumulative(list[i], 150f)))
							{
								list[i].TryAttachFire(this.fireSize * 0.2f);
							}
						}
					}
				}
				else
				{
					Fire.flammableList.Add(this.parent);
					this.flammabilityMax = this.parent.GetStatValue(StatDefOf.Flammability, true);
				}
			}
			if (this.flammabilityMax < 0.01f)
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			Thing thing2;
			if (this.parent != null)
			{
				thing2 = this.parent;
			}
			else if (Fire.flammableList.Count > 0)
			{
				thing2 = Fire.flammableList.RandomElement<Thing>();
			}
			else
			{
				thing2 = null;
			}
			if (thing2 != null && (this.fireSize >= 0.4f || thing2 == this.parent || thing2.def.category != ThingCategory.Pawn))
			{
				this.DoFireDamage(thing2);
			}
			if (base.Spawned)
			{
				float num = this.fireSize * 160f;
				if (flag)
				{
					num *= 0.15f;
				}
				GenTemperature.PushHeat(base.Position, base.Map, num);
				if (Rand.Value < 0.4f)
				{
					float radius = this.fireSize * 3f;
					SnowUtility.AddSnowRadial(base.Position, base.Map, radius, -(this.fireSize * 0.1f));
				}
				this.fireSize += 0.00055f * this.flammabilityMax * 150f;
				if (this.fireSize > 1.75f)
				{
					this.fireSize = 1.75f;
				}
				if (base.Map.weatherManager.RainRate > 0.01f && this.VulnerableToRain() && Rand.Value < 6f)
				{
					base.TakeDamage(new DamageInfo(DamageDefOf.Extinguish, 10f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true));
				}
			}
		}

		// Token: 0x060065C5 RID: 26053 RVA: 0x00226349 File Offset: 0x00224549
		private void TryBurnFloor()
		{
			if (this.parent != null || !base.Spawned)
			{
				return;
			}
			if (base.Position.TerrainFlammableNow(base.Map))
			{
				base.Map.terrainGrid.Notify_TerrainBurned(base.Position);
			}
		}

		// Token: 0x060065C6 RID: 26054 RVA: 0x00226388 File Offset: 0x00224588
		private bool VulnerableToRain()
		{
			if (!base.Spawned)
			{
				return false;
			}
			RoofDef roofDef = base.Map.roofGrid.RoofAt(base.Position);
			if (roofDef == null)
			{
				return true;
			}
			if (roofDef.isThickRoof)
			{
				return false;
			}
			Thing edifice = base.Position.GetEdifice(base.Map);
			return edifice != null && edifice.def.holdsRoof;
		}

		// Token: 0x060065C7 RID: 26055 RVA: 0x002263E8 File Offset: 0x002245E8
		private void DoFireDamage(Thing targ)
		{
			int num = GenMath.RoundRandom(Mathf.Clamp(0.0125f + 0.0036f * this.fireSize, 0.0125f, 0.05f) * 150f);
			if (num < 1)
			{
				num = 1;
			}
			Pawn pawn = targ as Pawn;
			if (pawn != null)
			{
				BattleLogEntry_DamageTaken battleLogEntry_DamageTaken = new BattleLogEntry_DamageTaken(pawn, RulePackDefOf.DamageEvent_Fire, null);
				Find.BattleLog.Add(battleLogEntry_DamageTaken);
				DamageInfo dinfo = new DamageInfo(DamageDefOf.Flame, (float)num, 0f, -1f, this, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true);
				dinfo.SetBodyRegion(BodyPartHeight.Undefined, BodyPartDepth.Outside);
				targ.TakeDamage(dinfo).AssociateWithLog(battleLogEntry_DamageTaken);
				Apparel apparel;
				if (pawn.apparel != null && pawn.apparel.WornApparel.TryRandomElement(out apparel))
				{
					apparel.TakeDamage(new DamageInfo(DamageDefOf.Flame, (float)num, 0f, -1f, this, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true));
					return;
				}
			}
			else
			{
				targ.TakeDamage(new DamageInfo(DamageDefOf.Flame, (float)num, 0f, -1f, this, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true));
			}
		}

		// Token: 0x060065C8 RID: 26056 RVA: 0x002264E8 File Offset: 0x002246E8
		protected void TrySpread()
		{
			IntVec3 intVec = base.Position;
			bool flag;
			if (Rand.Chance(0.8f))
			{
				intVec = base.Position + GenRadial.ManualRadialPattern[Rand.RangeInclusive(1, 8)];
				flag = true;
			}
			else
			{
				intVec = base.Position + GenRadial.ManualRadialPattern[Rand.RangeInclusive(10, 20)];
				flag = false;
			}
			if (!intVec.InBounds(base.Map))
			{
				return;
			}
			if (Rand.Chance(FireUtility.ChanceToStartFireIn(intVec, base.Map)))
			{
				if (!flag)
				{
					CellRect startRect = CellRect.SingleCell(base.Position);
					CellRect endRect = CellRect.SingleCell(intVec);
					if (!GenSight.LineOfSight(base.Position, intVec, base.Map, startRect, endRect, null))
					{
						return;
					}
					((Spark)GenSpawn.Spawn(ThingDefOf.Spark, base.Position, base.Map, WipeMode.Vanish)).Launch(this, intVec, intVec, ProjectileHitFlags.All, false, null);
					return;
				}
				else
				{
					FireUtility.TryStartFireIn(intVec, base.Map, 0.1f);
				}
			}
		}

		// Token: 0x04003964 RID: 14692
		private int ticksSinceSpawn;

		// Token: 0x04003965 RID: 14693
		public float fireSize = 0.1f;

		// Token: 0x04003966 RID: 14694
		private int ticksSinceSpread;

		// Token: 0x04003967 RID: 14695
		private float flammabilityMax = 0.5f;

		// Token: 0x04003968 RID: 14696
		private int ticksUntilSmoke;

		// Token: 0x04003969 RID: 14697
		private Sustainer sustainer;

		// Token: 0x0400396A RID: 14698
		private static List<Thing> flammableList = new List<Thing>();

		// Token: 0x0400396B RID: 14699
		private static int fireCount;

		// Token: 0x0400396C RID: 14700
		private static int lastFireCountUpdateTick;

		// Token: 0x0400396D RID: 14701
		public const float MinFireSize = 0.1f;

		// Token: 0x0400396E RID: 14702
		private const float MinSizeForSpark = 1f;

		// Token: 0x0400396F RID: 14703
		private const float TicksBetweenSparksBase = 150f;

		// Token: 0x04003970 RID: 14704
		private const float TicksBetweenSparksReductionPerFireSize = 40f;

		// Token: 0x04003971 RID: 14705
		private const float MinTicksBetweenSparks = 75f;

		// Token: 0x04003972 RID: 14706
		private const float MinFireSizeToEmitSpark = 1f;

		// Token: 0x04003973 RID: 14707
		public const float MaxFireSize = 1.75f;

		// Token: 0x04003974 RID: 14708
		private const int TicksToBurnFloor = 7500;

		// Token: 0x04003975 RID: 14709
		private const int ComplexCalcsInterval = 150;

		// Token: 0x04003976 RID: 14710
		private const float CellIgniteChancePerTickPerSize = 0.01f;

		// Token: 0x04003977 RID: 14711
		private const float MinSizeForIgniteMovables = 0.4f;

		// Token: 0x04003978 RID: 14712
		private const float FireBaseGrowthPerTick = 0.00055f;

		// Token: 0x04003979 RID: 14713
		private static readonly IntRange SmokeIntervalRange = new IntRange(130, 200);

		// Token: 0x0400397A RID: 14714
		private const int SmokeIntervalRandomAddon = 10;

		// Token: 0x0400397B RID: 14715
		private const float BaseSkyExtinguishChance = 0.04f;

		// Token: 0x0400397C RID: 14716
		private const int BaseSkyExtinguishDamage = 10;

		// Token: 0x0400397D RID: 14717
		private const float HeatPerFireSizePerInterval = 160f;

		// Token: 0x0400397E RID: 14718
		private const float HeatFactorWhenDoorPresent = 0.15f;

		// Token: 0x0400397F RID: 14719
		private const float SnowClearRadiusPerFireSize = 3f;

		// Token: 0x04003980 RID: 14720
		private const float SnowClearDepthFactor = 0.1f;

		// Token: 0x04003981 RID: 14721
		private const int FireCountParticlesOff = 15;
	}
}
