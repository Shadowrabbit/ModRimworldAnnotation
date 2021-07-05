using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200171A RID: 5914
	[StaticConstructorOnStartup]
	public class Plant : ThingWithComps
	{
		// Token: 0x17001436 RID: 5174
		// (get) Token: 0x06008257 RID: 33367 RVA: 0x0005784E File Offset: 0x00055A4E
		// (set) Token: 0x06008258 RID: 33368 RVA: 0x00057856 File Offset: 0x00055A56
		public virtual float Growth
		{
			get
			{
				return this.growthInt;
			}
			set
			{
				this.growthInt = Mathf.Clamp01(value);
				this.cachedLabelMouseover = null;
			}
		}

		// Token: 0x17001437 RID: 5175
		// (get) Token: 0x06008259 RID: 33369 RVA: 0x0005786B File Offset: 0x00055A6B
		// (set) Token: 0x0600825A RID: 33370 RVA: 0x00057873 File Offset: 0x00055A73
		public virtual int Age
		{
			get
			{
				return this.ageInt;
			}
			set
			{
				this.ageInt = value;
				this.cachedLabelMouseover = null;
			}
		}

		// Token: 0x17001438 RID: 5176
		// (get) Token: 0x0600825B RID: 33371 RVA: 0x00057883 File Offset: 0x00055A83
		public virtual bool HarvestableNow
		{
			get
			{
				return this.def.plant.Harvestable && this.growthInt > this.def.plant.harvestMinGrowth;
			}
		}

		// Token: 0x17001439 RID: 5177
		// (get) Token: 0x0600825C RID: 33372 RVA: 0x0026A25C File Offset: 0x0026845C
		public bool HarvestableSoon
		{
			get
			{
				if (this.HarvestableNow)
				{
					return true;
				}
				if (!this.def.plant.Harvestable)
				{
					return false;
				}
				float num = Mathf.Max(1f - this.Growth, 0f) * this.def.plant.growDays;
				float num2 = Mathf.Max(1f - this.def.plant.harvestMinGrowth, 0f) * this.def.plant.growDays;
				return (num <= 10f || num2 <= 1f) && this.GrowthRateFactor_Fertility > 0f && this.GrowthRateFactor_Temperature > 0f;
			}
		}

		// Token: 0x1700143A RID: 5178
		// (get) Token: 0x0600825D RID: 33373 RVA: 0x0026A30C File Offset: 0x0026850C
		public virtual bool BlightableNow
		{
			get
			{
				return !this.Blighted && this.def.plant.Blightable && this.sown && this.LifeStage != PlantLifeStage.Sowing && !base.Map.Biome.AllWildPlants.Contains(this.def);
			}
		}

		// Token: 0x1700143B RID: 5179
		// (get) Token: 0x0600825E RID: 33374 RVA: 0x000578B1 File Offset: 0x00055AB1
		public Blight Blight
		{
			get
			{
				if (!base.Spawned || !this.def.plant.Blightable)
				{
					return null;
				}
				return base.Position.GetFirstBlight(base.Map);
			}
		}

		// Token: 0x1700143C RID: 5180
		// (get) Token: 0x0600825F RID: 33375 RVA: 0x000578E0 File Offset: 0x00055AE0
		public bool Blighted
		{
			get
			{
				return this.Blight != null;
			}
		}

		// Token: 0x1700143D RID: 5181
		// (get) Token: 0x06008260 RID: 33376 RVA: 0x0026A364 File Offset: 0x00268564
		public override bool IngestibleNow
		{
			get
			{
				return base.IngestibleNow && (this.def.plant.IsTree || (this.growthInt >= this.def.plant.harvestMinGrowth && !this.LeaflessNow && (!base.Spawned || base.Position.GetSnowDepth(base.Map) <= this.def.hideAtSnowDepth)));
			}
		}

		// Token: 0x1700143E RID: 5182
		// (get) Token: 0x06008261 RID: 33377 RVA: 0x0026A3DC File Offset: 0x002685DC
		public virtual float CurrentDyingDamagePerTick
		{
			get
			{
				if (!base.Spawned)
				{
					return 0f;
				}
				float num = 0f;
				if (this.def.plant.LimitedLifespan && this.ageInt > this.def.plant.LifespanTicks)
				{
					num = Mathf.Max(num, 0.005f);
				}
				if (!this.def.plant.cavePlant && this.def.plant.dieIfNoSunlight && this.unlitTicks > 450000)
				{
					num = Mathf.Max(num, 0.005f);
				}
				if (this.DyingBecauseExposedToLight)
				{
					float lerpPct = base.Map.glowGrid.GameGlowAt(base.Position, true);
					num = Mathf.Max(num, Plant.DyingDamagePerTickBecauseExposedToLight.LerpThroughRange(lerpPct));
				}
				return num;
			}
		}

		// Token: 0x1700143F RID: 5183
		// (get) Token: 0x06008262 RID: 33378 RVA: 0x000578EB File Offset: 0x00055AEB
		public virtual bool DyingBecauseExposedToLight
		{
			get
			{
				return this.def.plant.cavePlant && base.Spawned && base.Map.glowGrid.GameGlowAt(base.Position, true) > 0f;
			}
		}

		// Token: 0x17001440 RID: 5184
		// (get) Token: 0x06008263 RID: 33379 RVA: 0x00057927 File Offset: 0x00055B27
		public bool Dying
		{
			get
			{
				return this.CurrentDyingDamagePerTick > 0f;
			}
		}

		// Token: 0x17001441 RID: 5185
		// (get) Token: 0x06008264 RID: 33380 RVA: 0x00057936 File Offset: 0x00055B36
		protected virtual bool Resting
		{
			get
			{
				return GenLocalDate.DayPercent(this) < 0.25f || GenLocalDate.DayPercent(this) > 0.8f;
			}
		}

		// Token: 0x17001442 RID: 5186
		// (get) Token: 0x06008265 RID: 33381 RVA: 0x0026A4A8 File Offset: 0x002686A8
		public virtual float GrowthRate
		{
			get
			{
				if (this.Blighted)
				{
					return 0f;
				}
				if (base.Spawned && !PlantUtility.GrowthSeasonNow(base.Position, base.Map, false))
				{
					return 0f;
				}
				return this.GrowthRateFactor_Fertility * this.GrowthRateFactor_Temperature * this.GrowthRateFactor_Light;
			}
		}

		// Token: 0x17001443 RID: 5187
		// (get) Token: 0x06008266 RID: 33382 RVA: 0x00057954 File Offset: 0x00055B54
		protected float GrowthPerTick
		{
			get
			{
				if (this.LifeStage != PlantLifeStage.Growing || this.Resting)
				{
					return 0f;
				}
				return 1f / (60000f * this.def.plant.growDays) * this.GrowthRate;
			}
		}

		// Token: 0x17001444 RID: 5188
		// (get) Token: 0x06008267 RID: 33383 RVA: 0x00057990 File Offset: 0x00055B90
		public float GrowthRateFactor_Fertility
		{
			get
			{
				return base.Map.fertilityGrid.FertilityAt(base.Position) * this.def.plant.fertilitySensitivity + (1f - this.def.plant.fertilitySensitivity);
			}
		}

		// Token: 0x17001445 RID: 5189
		// (get) Token: 0x06008268 RID: 33384 RVA: 0x0026A4FC File Offset: 0x002686FC
		public float GrowthRateFactor_Light
		{
			get
			{
				float num = base.Map.glowGrid.GameGlowAt(base.Position, false);
				if (this.def.plant.growMinGlow == this.def.plant.growOptimalGlow && num == this.def.plant.growOptimalGlow)
				{
					return 1f;
				}
				return GenMath.InverseLerp(this.def.plant.growMinGlow, this.def.plant.growOptimalGlow, num);
			}
		}

		// Token: 0x17001446 RID: 5190
		// (get) Token: 0x06008269 RID: 33385 RVA: 0x0026A584 File Offset: 0x00268784
		public float GrowthRateFactor_Temperature
		{
			get
			{
				float num;
				if (!GenTemperature.TryGetTemperatureForCell(base.Position, base.Map, out num))
				{
					return 1f;
				}
				if (num < 10f)
				{
					return Mathf.InverseLerp(0f, 10f, num);
				}
				if (num > 42f)
				{
					return Mathf.InverseLerp(58f, 42f, num);
				}
				return 1f;
			}
		}

		// Token: 0x17001447 RID: 5191
		// (get) Token: 0x0600826A RID: 33386 RVA: 0x0026A5E4 File Offset: 0x002687E4
		protected int TicksUntilFullyGrown
		{
			get
			{
				if (this.growthInt > 0.9999f)
				{
					return 0;
				}
				float growthPerTick = this.GrowthPerTick;
				if (growthPerTick == 0f)
				{
					return int.MaxValue;
				}
				return (int)((1f - this.growthInt) / growthPerTick);
			}
		}

		// Token: 0x17001448 RID: 5192
		// (get) Token: 0x0600826B RID: 33387 RVA: 0x000579D0 File Offset: 0x00055BD0
		protected string GrowthPercentString
		{
			get
			{
				return (this.growthInt + 0.0001f).ToStringPercent();
			}
		}

		// Token: 0x17001449 RID: 5193
		// (get) Token: 0x0600826C RID: 33388 RVA: 0x0026A624 File Offset: 0x00268824
		public override string LabelMouseover
		{
			get
			{
				if (this.cachedLabelMouseover == null)
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append(this.def.LabelCap);
					stringBuilder.Append(" (" + "PercentGrowth".Translate(this.GrowthPercentString));
					if (this.Dying)
					{
						stringBuilder.Append(", " + "DyingLower".Translate());
					}
					stringBuilder.Append(")");
					this.cachedLabelMouseover = stringBuilder.ToString();
				}
				return this.cachedLabelMouseover;
			}
		}

		// Token: 0x1700144A RID: 5194
		// (get) Token: 0x0600826D RID: 33389 RVA: 0x000579E3 File Offset: 0x00055BE3
		protected virtual bool HasEnoughLightToGrow
		{
			get
			{
				return this.GrowthRateFactor_Light > 0.001f;
			}
		}

		// Token: 0x1700144B RID: 5195
		// (get) Token: 0x0600826E RID: 33390 RVA: 0x000579F2 File Offset: 0x00055BF2
		public virtual PlantLifeStage LifeStage
		{
			get
			{
				if (this.growthInt < 0.001f)
				{
					return PlantLifeStage.Sowing;
				}
				if (this.growthInt > 0.999f)
				{
					return PlantLifeStage.Mature;
				}
				return PlantLifeStage.Growing;
			}
		}

		// Token: 0x1700144C RID: 5196
		// (get) Token: 0x0600826F RID: 33391 RVA: 0x0026A6CC File Offset: 0x002688CC
		public override Graphic Graphic
		{
			get
			{
				if (this.LifeStage == PlantLifeStage.Sowing)
				{
					return Plant.GraphicSowing;
				}
				if (this.def.plant.leaflessGraphic != null && this.LeaflessNow && (!this.sown || !this.HarvestableNow))
				{
					return this.def.plant.leaflessGraphic;
				}
				if (this.def.plant.immatureGraphic != null && !this.HarvestableNow)
				{
					return this.def.plant.immatureGraphic;
				}
				return base.Graphic;
			}
		}

		// Token: 0x1700144D RID: 5197
		// (get) Token: 0x06008270 RID: 33392 RVA: 0x00057A13 File Offset: 0x00055C13
		public bool LeaflessNow
		{
			get
			{
				return Find.TickManager.TicksGame - this.madeLeaflessTick < 60000;
			}
		}

		// Token: 0x1700144E RID: 5198
		// (get) Token: 0x06008271 RID: 33393 RVA: 0x0026A754 File Offset: 0x00268954
		protected virtual float LeaflessTemperatureThresh
		{
			get
			{
				float num = 8f;
				return (float)this.HashOffset() * 0.01f % num - num + -2f;
			}
		}

		// Token: 0x1700144F RID: 5199
		// (get) Token: 0x06008272 RID: 33394 RVA: 0x0026A780 File Offset: 0x00268980
		public bool IsCrop
		{
			get
			{
				if (!this.def.plant.Sowable)
				{
					return false;
				}
				if (!base.Spawned)
				{
					Log.Warning("Can't determine if crop when unspawned.", false);
					return false;
				}
				return this.def == WorkGiver_Grower.CalculateWantedPlantDef(base.Position, base.Map);
			}
		}

		// Token: 0x06008273 RID: 33395 RVA: 0x00057A30 File Offset: 0x00055C30
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (Current.ProgramState == ProgramState.Playing && !respawningAfterLoad)
			{
				this.CheckTemperatureMakeLeafless();
			}
		}

		// Token: 0x06008274 RID: 33396 RVA: 0x0026A7D0 File Offset: 0x002689D0
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Blight firstBlight = base.Position.GetFirstBlight(base.Map);
			base.DeSpawn(mode);
			if (firstBlight != null)
			{
				firstBlight.Notify_PlantDeSpawned();
			}
		}

		// Token: 0x06008275 RID: 33397 RVA: 0x0026A800 File Offset: 0x00268A00
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.growthInt, "growth", 0f, false);
			Scribe_Values.Look<int>(ref this.ageInt, "age", 0, false);
			Scribe_Values.Look<int>(ref this.unlitTicks, "unlitTicks", 0, false);
			Scribe_Values.Look<int>(ref this.madeLeaflessTick, "madeLeaflessTick", -99999, false);
			Scribe_Values.Look<bool>(ref this.sown, "sown", false, false);
		}

		// Token: 0x06008276 RID: 33398 RVA: 0x00057A4B File Offset: 0x00055C4B
		public override void PostMapInit()
		{
			this.CheckTemperatureMakeLeafless();
		}

		// Token: 0x06008277 RID: 33399 RVA: 0x0026A878 File Offset: 0x00268A78
		protected override void IngestedCalculateAmounts(Pawn ingester, float nutritionWanted, out int numTaken, out float nutritionIngested)
		{
			float statValue = this.GetStatValue(StatDefOf.Nutrition, true);
			if (this.def.plant.HarvestDestroys)
			{
				numTaken = 1;
			}
			else
			{
				this.growthInt -= 0.3f;
				if (this.growthInt < 0.08f)
				{
					this.growthInt = 0.08f;
				}
				if (base.Spawned)
				{
					base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.Things);
				}
				numTaken = 0;
			}
			nutritionIngested = statValue;
		}

		// Token: 0x06008278 RID: 33400 RVA: 0x0026A8FC File Offset: 0x00268AFC
		public virtual void PlantCollected()
		{
			if (this.def.plant.HarvestDestroys)
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			this.growthInt = this.def.plant.harvestAfterGrowth;
			base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.Things);
		}

		// Token: 0x06008279 RID: 33401 RVA: 0x00057A53 File Offset: 0x00055C53
		protected virtual void CheckTemperatureMakeLeafless()
		{
			if (base.AmbientTemperature < this.LeaflessTemperatureThresh)
			{
				this.MakeLeafless(Plant.LeaflessCause.Cold);
			}
		}

		// Token: 0x0600827A RID: 33402 RVA: 0x0026A950 File Offset: 0x00268B50
		public virtual void MakeLeafless(Plant.LeaflessCause cause)
		{
			bool flag = !this.LeaflessNow;
			Map map = base.Map;
			if (cause == Plant.LeaflessCause.Poison && this.def.plant.leaflessGraphic == null)
			{
				if (this.IsCrop && MessagesRepeatAvoider.MessageShowAllowed("MessagePlantDiedOfPoison-" + this.def.defName, 240f))
				{
					Messages.Message("MessagePlantDiedOfPoison".Translate(this.GetCustomLabelNoCount(false)), new TargetInfo(base.Position, map, false), MessageTypeDefOf.NegativeEvent, true);
				}
				base.TakeDamage(new DamageInfo(DamageDefOf.Rotting, 99999f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			}
			else if (this.def.plant.dieIfLeafless)
			{
				if (this.IsCrop)
				{
					if (cause == Plant.LeaflessCause.Cold)
					{
						if (MessagesRepeatAvoider.MessageShowAllowed("MessagePlantDiedOfCold-" + this.def.defName, 240f))
						{
							Messages.Message("MessagePlantDiedOfCold".Translate(this.GetCustomLabelNoCount(false)), new TargetInfo(base.Position, map, false), MessageTypeDefOf.NegativeEvent, true);
						}
					}
					else if (cause == Plant.LeaflessCause.Poison && MessagesRepeatAvoider.MessageShowAllowed("MessagePlantDiedOfPoison-" + this.def.defName, 240f))
					{
						Messages.Message("MessagePlantDiedOfPoison".Translate(this.GetCustomLabelNoCount(false)), new TargetInfo(base.Position, map, false), MessageTypeDefOf.NegativeEvent, true);
					}
				}
				base.TakeDamage(new DamageInfo(DamageDefOf.Rotting, 99999f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			}
			else
			{
				this.madeLeaflessTick = Find.TickManager.TicksGame;
			}
			if (flag)
			{
				map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.Things);
			}
		}

		// Token: 0x0600827B RID: 33403 RVA: 0x0026AB40 File Offset: 0x00268D40
		public override void TickLong()
		{
			this.CheckTemperatureMakeLeafless();
			if (base.Destroyed)
			{
				return;
			}
			base.TickLong();
			if (PlantUtility.GrowthSeasonNow(base.Position, base.Map, false))
			{
				float num = this.growthInt;
				bool flag = this.LifeStage == PlantLifeStage.Mature;
				this.growthInt += this.GrowthPerTick * 2000f;
				if (this.growthInt > 1f)
				{
					this.growthInt = 1f;
				}
				if (((!flag && this.LifeStage == PlantLifeStage.Mature) || (int)(num * 10f) != (int)(this.growthInt * 10f)) && this.CurrentlyCultivated())
				{
					base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.Things);
				}
			}
			if (!this.HasEnoughLightToGrow)
			{
				this.unlitTicks += 2000;
			}
			else
			{
				this.unlitTicks = 0;
			}
			this.ageInt += 2000;
			if (this.Dying)
			{
				Map map = base.Map;
				bool isCrop = this.IsCrop;
				bool harvestableNow = this.HarvestableNow;
				bool dyingBecauseExposedToLight = this.DyingBecauseExposedToLight;
				int num2 = Mathf.CeilToInt(this.CurrentDyingDamagePerTick * 2000f);
				base.TakeDamage(new DamageInfo(DamageDefOf.Rotting, (float)num2, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
				if (base.Destroyed)
				{
					if (isCrop && this.def.plant.Harvestable && MessagesRepeatAvoider.MessageShowAllowed("MessagePlantDiedOfRot-" + this.def.defName, 240f))
					{
						string key;
						if (harvestableNow)
						{
							key = "MessagePlantDiedOfRot_LeftUnharvested";
						}
						else if (dyingBecauseExposedToLight)
						{
							key = "MessagePlantDiedOfRot_ExposedToLight";
						}
						else
						{
							key = "MessagePlantDiedOfRot";
						}
						Messages.Message(key.Translate(this.GetCustomLabelNoCount(false)), new TargetInfo(base.Position, map, false), MessageTypeDefOf.NegativeEvent, true);
					}
					return;
				}
			}
			this.cachedLabelMouseover = null;
			if (this.def.plant.dropLeaves)
			{
				MoteLeaf moteLeaf = MoteMaker.MakeStaticMote(Vector3.zero, base.Map, ThingDefOf.Mote_Leaf, 1f) as MoteLeaf;
				if (moteLeaf != null)
				{
					float num3 = this.def.plant.visualSizeRange.LerpThroughRange(this.growthInt);
					float treeHeight = this.def.graphicData.drawSize.x * num3;
					Vector3 vector = Rand.InsideUnitCircleVec3 * Plant.LeafSpawnRadius;
					moteLeaf.Initialize(base.Position.ToVector3Shifted() + Vector3.up * Rand.Range(Plant.LeafSpawnYMin, Plant.LeafSpawnYMax) + vector + Vector3.forward * this.def.graphicData.shadowData.offset.z, Rand.Value * 2000.TicksToSeconds(), vector.z > 0f, treeHeight);
				}
			}
		}

		// Token: 0x0600827C RID: 33404 RVA: 0x0026AE38 File Offset: 0x00269038
		protected virtual bool CurrentlyCultivated()
		{
			if (!this.def.plant.Sowable)
			{
				return false;
			}
			if (!base.Spawned)
			{
				return false;
			}
			Zone zone = base.Map.zoneManager.ZoneAt(base.Position);
			if (zone != null && zone is Zone_Growing)
			{
				return true;
			}
			Building edifice = base.Position.GetEdifice(base.Map);
			return edifice != null && edifice.def.building.SupportsPlants;
		}

		// Token: 0x0600827D RID: 33405 RVA: 0x00057A6A File Offset: 0x00055C6A
		public virtual bool CanYieldNow()
		{
			return this.HarvestableNow && this.def.plant.harvestYield > 0f && !this.Blighted;
		}

		// Token: 0x0600827E RID: 33406 RVA: 0x0026AEB4 File Offset: 0x002690B4
		public virtual int YieldNow()
		{
			if (!this.CanYieldNow())
			{
				return 0;
			}
			float harvestYield = this.def.plant.harvestYield;
			float num = Mathf.InverseLerp(this.def.plant.harvestMinGrowth, 1f, this.growthInt);
			num = 0.5f + num * 0.5f;
			return GenMath.RoundRandom(harvestYield * num * Mathf.Lerp(0.5f, 1f, (float)this.HitPoints / (float)base.MaxHitPoints) * Find.Storyteller.difficultyValues.cropYieldFactor);
		}

		// Token: 0x0600827F RID: 33407 RVA: 0x0026AF40 File Offset: 0x00269140
		public override void Print(SectionLayer layer)
		{
			Vector3 a = this.TrueCenter();
			Rand.PushState();
			Rand.Seed = base.Position.GetHashCode();
			int num = Mathf.CeilToInt(this.growthInt * (float)this.def.plant.maxMeshCount);
			if (num < 1)
			{
				num = 1;
			}
			float num2 = this.def.plant.visualSizeRange.LerpThroughRange(this.growthInt);
			float num3 = this.def.graphicData.drawSize.x * num2;
			Vector3 vector = Vector3.zero;
			int num4 = 0;
			int[] positionIndices = PlantPosIndices.GetPositionIndices(this);
			bool flag = false;
			foreach (int num5 in positionIndices)
			{
				if (this.def.plant.maxMeshCount != 1)
				{
					int num6 = 1;
					int maxMeshCount = this.def.plant.maxMeshCount;
					if (maxMeshCount <= 4)
					{
						if (maxMeshCount != 1)
						{
							if (maxMeshCount != 4)
							{
								goto IL_157;
							}
							num6 = 2;
						}
						else
						{
							num6 = 1;
						}
					}
					else if (maxMeshCount != 9)
					{
						if (maxMeshCount != 16)
						{
							if (maxMeshCount != 25)
							{
								goto IL_157;
							}
							num6 = 5;
						}
						else
						{
							num6 = 4;
						}
					}
					else
					{
						num6 = 3;
					}
					IL_16D:
					float num7 = 1f / (float)num6;
					vector = base.Position.ToVector3();
					vector.y = this.def.Altitude;
					vector.x += 0.5f * num7;
					vector.z += 0.5f * num7;
					int num8 = num5 / num6;
					int num9 = num5 % num6;
					vector.x += (float)num8 * num7;
					vector.z += (float)num9 * num7;
					float max = num7 * 0.3f;
					vector += Gen.RandomHorizontalVector(max);
					goto IL_20B;
					IL_157:
					Log.Error(this.def + " must have plant.MaxMeshCount that is a perfect square.", false);
					goto IL_16D;
				}
				vector = a + Gen.RandomHorizontalVector(0.05f);
				float num10 = (float)base.Position.z;
				if (vector.z - num2 / 2f < num10)
				{
					vector.z = num10 + num2 / 2f;
					flag = true;
				}
				IL_20B:
				bool @bool = Rand.Bool;
				Material matSingle = this.Graphic.MatSingle;
				PlantUtility.SetWindExposureColors(Plant.workingColors, this);
				Vector2 size = new Vector2(num3, num3);
				Printer_Plane.PrintPlane(layer, vector, size, matSingle, 0f, @bool, null, Plant.workingColors, 0.1f, (float)(this.HashOffset() % 1024));
				num4++;
				if (num4 >= num)
				{
					break;
				}
			}
			if (this.def.graphicData.shadowData != null)
			{
				Vector3 center = a + this.def.graphicData.shadowData.offset * num2;
				if (flag)
				{
					center.z = base.Position.ToVector3Shifted().z + this.def.graphicData.shadowData.offset.z;
				}
				center.y -= 0.042857144f;
				Vector3 volume = this.def.graphicData.shadowData.volume * num2;
				Printer_Shadow.PrintShadow(layer, center, volume, Rot4.North);
			}
			Rand.PopState();
		}

		// Token: 0x06008280 RID: 33408 RVA: 0x0026B27C File Offset: 0x0026947C
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.LifeStage == PlantLifeStage.Growing)
			{
				stringBuilder.AppendLine("PercentGrowth".Translate(this.GrowthPercentString));
				stringBuilder.AppendLine("GrowthRate".Translate() + ": " + this.GrowthRate.ToStringPercent());
				if (!this.Blighted)
				{
					if (this.Resting)
					{
						stringBuilder.AppendLine("PlantResting".Translate());
					}
					if (!this.HasEnoughLightToGrow)
					{
						stringBuilder.AppendLine("PlantNeedsLightLevel".Translate() + ": " + this.def.plant.growMinGlow.ToStringPercent());
					}
					float growthRateFactor_Temperature = this.GrowthRateFactor_Temperature;
					if (growthRateFactor_Temperature < 0.99f)
					{
						if (growthRateFactor_Temperature < 0.01f)
						{
							stringBuilder.AppendLine("OutOfIdealTemperatureRangeNotGrowing".Translate());
						}
						else
						{
							stringBuilder.AppendLine("OutOfIdealTemperatureRange".Translate(Mathf.RoundToInt(growthRateFactor_Temperature * 100f).ToString()));
						}
					}
				}
			}
			else if (this.LifeStage == PlantLifeStage.Mature)
			{
				if (this.HarvestableNow)
				{
					stringBuilder.AppendLine("ReadyToHarvest".Translate());
				}
				else
				{
					stringBuilder.AppendLine("Mature".Translate());
				}
			}
			if (this.DyingBecauseExposedToLight)
			{
				stringBuilder.AppendLine("DyingBecauseExposedToLight".Translate());
			}
			if (this.Blighted)
			{
				stringBuilder.AppendLine("Blighted".Translate() + " (" + this.Blight.Severity.ToStringPercent() + ")");
			}
			string text = base.InspectStringPartsFromComps();
			if (!text.NullOrEmpty())
			{
				stringBuilder.Append(text);
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x06008281 RID: 33409 RVA: 0x00057A9A File Offset: 0x00055C9A
		public virtual void CropBlighted()
		{
			if (!this.Blighted)
			{
				GenSpawn.Spawn(ThingDefOf.Blight, base.Position, base.Map, WipeMode.Vanish);
			}
		}

		// Token: 0x06008282 RID: 33410 RVA: 0x00057ABC File Offset: 0x00055CBC
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (Prefs.DevMode && this.Blighted)
			{
				yield return new Command_Action
				{
					defaultLabel = "Dev: Spread blight",
					action = delegate()
					{
						this.Blight.TryReproduceNow();
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x0400547E RID: 21630
		protected float growthInt = 0.05f;

		// Token: 0x0400547F RID: 21631
		protected int ageInt;

		// Token: 0x04005480 RID: 21632
		protected int unlitTicks;

		// Token: 0x04005481 RID: 21633
		protected int madeLeaflessTick = -99999;

		// Token: 0x04005482 RID: 21634
		public bool sown;

		// Token: 0x04005483 RID: 21635
		private string cachedLabelMouseover;

		// Token: 0x04005484 RID: 21636
		private static Color32[] workingColors = new Color32[4];

		// Token: 0x04005485 RID: 21637
		public const float BaseGrowthPercent = 0.05f;

		// Token: 0x04005486 RID: 21638
		private const float BaseDyingDamagePerTick = 0.005f;

		// Token: 0x04005487 RID: 21639
		private static readonly FloatRange DyingDamagePerTickBecauseExposedToLight = new FloatRange(0.0001f, 0.001f);

		// Token: 0x04005488 RID: 21640
		private const float GridPosRandomnessFactor = 0.3f;

		// Token: 0x04005489 RID: 21641
		private const int TicksWithoutLightBeforeStartDying = 450000;

		// Token: 0x0400548A RID: 21642
		private const int LeaflessMinRecoveryTicks = 60000;

		// Token: 0x0400548B RID: 21643
		public const float MinGrowthTemperature = 0f;

		// Token: 0x0400548C RID: 21644
		public const float MinOptimalGrowthTemperature = 10f;

		// Token: 0x0400548D RID: 21645
		public const float MaxOptimalGrowthTemperature = 42f;

		// Token: 0x0400548E RID: 21646
		public const float MaxGrowthTemperature = 58f;

		// Token: 0x0400548F RID: 21647
		public const float MaxLeaflessTemperature = -2f;

		// Token: 0x04005490 RID: 21648
		private const float MinLeaflessTemperature = -10f;

		// Token: 0x04005491 RID: 21649
		private const float MinAnimalEatPlantsTemperature = 0f;

		// Token: 0x04005492 RID: 21650
		public const float TopVerticesAltitudeBias = 0.1f;

		// Token: 0x04005493 RID: 21651
		private static Graphic GraphicSowing = GraphicDatabase.Get<Graphic_Single>("Things/Plant/Plant_Sowing", ShaderDatabase.Cutout, Vector2.one, Color.white);

		// Token: 0x04005494 RID: 21652
		[TweakValue("Graphics", -1f, 1f)]
		private static float LeafSpawnRadius = 0.4f;

		// Token: 0x04005495 RID: 21653
		[TweakValue("Graphics", 0f, 2f)]
		private static float LeafSpawnYMin = 0.3f;

		// Token: 0x04005496 RID: 21654
		[TweakValue("Graphics", 0f, 2f)]
		private static float LeafSpawnYMax = 1f;

		// Token: 0x0200171B RID: 5915
		public enum LeaflessCause
		{
			// Token: 0x04005498 RID: 21656
			Cold,
			// Token: 0x04005499 RID: 21657
			Poison
		}
	}
}
