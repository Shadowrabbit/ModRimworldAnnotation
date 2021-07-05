using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020010BD RID: 4285
	[StaticConstructorOnStartup]
	public class Plant : ThingWithComps
	{
		// Token: 0x17001185 RID: 4485
		// (get) Token: 0x06006664 RID: 26212 RVA: 0x00228E91 File Offset: 0x00227091
		// (set) Token: 0x06006665 RID: 26213 RVA: 0x00228E99 File Offset: 0x00227099
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

		// Token: 0x17001186 RID: 4486
		// (get) Token: 0x06006666 RID: 26214 RVA: 0x00228EAE File Offset: 0x002270AE
		// (set) Token: 0x06006667 RID: 26215 RVA: 0x00228EB6 File Offset: 0x002270B6
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

		// Token: 0x17001187 RID: 4487
		// (get) Token: 0x06006668 RID: 26216 RVA: 0x00228EC6 File Offset: 0x002270C6
		public virtual bool HarvestableNow
		{
			get
			{
				return this.def.plant.Harvestable && this.growthInt > this.def.plant.harvestMinGrowth;
			}
		}

		// Token: 0x17001188 RID: 4488
		// (get) Token: 0x06006669 RID: 26217 RVA: 0x00228EF4 File Offset: 0x002270F4
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

		// Token: 0x17001189 RID: 4489
		// (get) Token: 0x0600666A RID: 26218 RVA: 0x00228FA4 File Offset: 0x002271A4
		public virtual bool BlightableNow
		{
			get
			{
				return !this.Blighted && this.def.plant.Blightable && this.sown && this.LifeStage != PlantLifeStage.Sowing && !base.Map.Biome.AllWildPlants.Contains(this.def);
			}
		}

		// Token: 0x1700118A RID: 4490
		// (get) Token: 0x0600666B RID: 26219 RVA: 0x00228FFB File Offset: 0x002271FB
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

		// Token: 0x1700118B RID: 4491
		// (get) Token: 0x0600666C RID: 26220 RVA: 0x0022902A File Offset: 0x0022722A
		public bool Blighted
		{
			get
			{
				return this.Blight != null;
			}
		}

		// Token: 0x1700118C RID: 4492
		// (get) Token: 0x0600666D RID: 26221 RVA: 0x00229038 File Offset: 0x00227238
		public override bool IngestibleNow
		{
			get
			{
				return base.IngestibleNow && (this.def.plant.IsTree || (this.growthInt >= this.def.plant.harvestMinGrowth && this.growthInt >= 0.1f && !this.LeaflessNow && (!base.Spawned || base.Position.GetSnowDepth(base.Map) <= this.def.hideAtSnowDepth)));
			}
		}

		// Token: 0x1700118D RID: 4493
		// (get) Token: 0x0600666E RID: 26222 RVA: 0x002290C0 File Offset: 0x002272C0
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

		// Token: 0x1700118E RID: 4494
		// (get) Token: 0x0600666F RID: 26223 RVA: 0x0022918A File Offset: 0x0022738A
		public virtual bool DyingBecauseExposedToLight
		{
			get
			{
				return this.def.plant.cavePlant && base.Spawned && base.Map.glowGrid.GameGlowAt(base.Position, true) > 0f;
			}
		}

		// Token: 0x1700118F RID: 4495
		// (get) Token: 0x06006670 RID: 26224 RVA: 0x002291C6 File Offset: 0x002273C6
		public bool Dying
		{
			get
			{
				return this.CurrentDyingDamagePerTick > 0f;
			}
		}

		// Token: 0x17001190 RID: 4496
		// (get) Token: 0x06006671 RID: 26225 RVA: 0x002291D5 File Offset: 0x002273D5
		protected virtual bool Resting
		{
			get
			{
				return GenLocalDate.DayPercent(this) < 0.25f || GenLocalDate.DayPercent(this) > 0.8f;
			}
		}

		// Token: 0x17001191 RID: 4497
		// (get) Token: 0x06006672 RID: 26226 RVA: 0x002291F4 File Offset: 0x002273F4
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

		// Token: 0x17001192 RID: 4498
		// (get) Token: 0x06006673 RID: 26227 RVA: 0x00229245 File Offset: 0x00227445
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

		// Token: 0x17001193 RID: 4499
		// (get) Token: 0x06006674 RID: 26228 RVA: 0x00229281 File Offset: 0x00227481
		public float GrowthRateFactor_Fertility
		{
			get
			{
				return PlantUtility.GrowthRateFactorFor_Fertility(this.def, base.Map.fertilityGrid.FertilityAt(base.Position));
			}
		}

		// Token: 0x17001194 RID: 4500
		// (get) Token: 0x06006675 RID: 26229 RVA: 0x002292A4 File Offset: 0x002274A4
		public float GrowthRateFactor_Light
		{
			get
			{
				float glow = base.Map.glowGrid.GameGlowAt(base.Position, false);
				return PlantUtility.GrowthRateFactorFor_Light(this.def, glow);
			}
		}

		// Token: 0x17001195 RID: 4501
		// (get) Token: 0x06006676 RID: 26230 RVA: 0x002292D8 File Offset: 0x002274D8
		public float GrowthRateFactor_Temperature
		{
			get
			{
				float cellTemp;
				if (!GenTemperature.TryGetTemperatureForCell(base.Position, base.Map, out cellTemp))
				{
					return 1f;
				}
				return PlantUtility.GrowthRateFactorFor_Temperature(cellTemp);
			}
		}

		// Token: 0x17001196 RID: 4502
		// (get) Token: 0x06006677 RID: 26231 RVA: 0x00229308 File Offset: 0x00227508
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

		// Token: 0x17001197 RID: 4503
		// (get) Token: 0x06006678 RID: 26232 RVA: 0x00229348 File Offset: 0x00227548
		protected string GrowthPercentString
		{
			get
			{
				return (this.growthInt + 0.0001f).ToStringPercent();
			}
		}

		// Token: 0x17001198 RID: 4504
		// (get) Token: 0x06006679 RID: 26233 RVA: 0x0022935C File Offset: 0x0022755C
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

		// Token: 0x17001199 RID: 4505
		// (get) Token: 0x0600667A RID: 26234 RVA: 0x00229402 File Offset: 0x00227602
		protected virtual bool HasEnoughLightToGrow
		{
			get
			{
				return this.GrowthRateFactor_Light > 0.001f;
			}
		}

		// Token: 0x1700119A RID: 4506
		// (get) Token: 0x0600667B RID: 26235 RVA: 0x00229411 File Offset: 0x00227611
		public virtual PlantLifeStage LifeStage
		{
			get
			{
				if (this.growthInt < 0.0001f)
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

		// Token: 0x1700119B RID: 4507
		// (get) Token: 0x0600667C RID: 26236 RVA: 0x00229434 File Offset: 0x00227634
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

		// Token: 0x1700119C RID: 4508
		// (get) Token: 0x0600667D RID: 26237 RVA: 0x002294BB File Offset: 0x002276BB
		public bool LeaflessNow
		{
			get
			{
				return Find.TickManager.TicksGame - this.madeLeaflessTick < 60000;
			}
		}

		// Token: 0x1700119D RID: 4509
		// (get) Token: 0x0600667E RID: 26238 RVA: 0x002294D8 File Offset: 0x002276D8
		protected virtual float LeaflessTemperatureThresh
		{
			get
			{
				float num = 8f;
				return (float)this.HashOffset() * 0.01f % num - num + -2f;
			}
		}

		// Token: 0x1700119E RID: 4510
		// (get) Token: 0x0600667F RID: 26239 RVA: 0x00229504 File Offset: 0x00227704
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
					Log.Warning("Can't determine if crop when unspawned.");
					return false;
				}
				return this.def == WorkGiver_Grower.CalculateWantedPlantDef(base.Position, base.Map);
			}
		}

		// Token: 0x06006680 RID: 26240 RVA: 0x00229552 File Offset: 0x00227752
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (Current.ProgramState == ProgramState.Playing && !respawningAfterLoad)
			{
				this.CheckTemperatureMakeLeafless();
			}
		}

		// Token: 0x06006681 RID: 26241 RVA: 0x00229570 File Offset: 0x00227770
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Blight firstBlight = base.Position.GetFirstBlight(base.Map);
			base.DeSpawn(mode);
			if (firstBlight != null)
			{
				firstBlight.Notify_PlantDeSpawned();
			}
		}

		// Token: 0x06006682 RID: 26242 RVA: 0x002295A0 File Offset: 0x002277A0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.growthInt, "growth", 0f, false);
			Scribe_Values.Look<int>(ref this.ageInt, "age", 0, false);
			Scribe_Values.Look<int>(ref this.unlitTicks, "unlitTicks", 0, false);
			Scribe_Values.Look<int>(ref this.madeLeaflessTick, "madeLeaflessTick", -99999, false);
			Scribe_Values.Look<bool>(ref this.sown, "sown", false, false);
		}

		// Token: 0x06006683 RID: 26243 RVA: 0x00229615 File Offset: 0x00227815
		public override void PostMapInit()
		{
			this.CheckTemperatureMakeLeafless();
		}

		// Token: 0x06006684 RID: 26244 RVA: 0x00229620 File Offset: 0x00227820
		protected override void IngestedCalculateAmounts(Pawn ingester, float nutritionWanted, out int numTaken, out float nutritionIngested)
		{
			float statValue = this.GetStatValue(StatDefOf.Nutrition, true);
			float num = this.growthInt * statValue;
			nutritionIngested = Mathf.Min(nutritionWanted, num);
			if (nutritionIngested >= num)
			{
				numTaken = 1;
				return;
			}
			numTaken = 0;
			this.growthInt -= nutritionIngested / statValue;
			if (base.Spawned)
			{
				base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.Things);
			}
		}

		// Token: 0x06006685 RID: 26245 RVA: 0x0022968C File Offset: 0x0022788C
		public virtual void PlantCollected(Pawn by)
		{
			if (this.def.plant.HarvestDestroys)
			{
				if (this.def.plant.IsTree && this.def.plant.treeLoversCareIfChopped)
				{
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.CutTree, by.Named(HistoryEventArgsNames.Doer)), true);
					base.Map.treeDestructionTracker.Notify_TreeCut(by);
				}
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			this.growthInt = this.def.plant.harvestAfterGrowth;
			base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.Things);
		}

		// Token: 0x06006686 RID: 26246 RVA: 0x00229738 File Offset: 0x00227938
		public override void Kill(DamageInfo? dinfo = null, Hediff exactCulprit = null)
		{
			if (base.Spawned && this.def.plant.IsTree && this.def.plant.treeLoversCareIfChopped && dinfo != null)
			{
				base.Map.treeDestructionTracker.Notify_TreeDestroyed(dinfo.Value);
			}
			base.Kill(null, null);
		}

		// Token: 0x06006687 RID: 26247 RVA: 0x002297A1 File Offset: 0x002279A1
		protected virtual void CheckTemperatureMakeLeafless()
		{
			if (base.AmbientTemperature < this.LeaflessTemperatureThresh)
			{
				this.MakeLeafless(Plant.LeaflessCause.Cold);
			}
		}

		// Token: 0x06006688 RID: 26248 RVA: 0x002297B8 File Offset: 0x002279B8
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
				base.TakeDamage(new DamageInfo(DamageDefOf.Rotting, 99999f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true));
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
				base.TakeDamage(new DamageInfo(DamageDefOf.Rotting, 99999f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true));
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

		// Token: 0x06006689 RID: 26249 RVA: 0x002299A9 File Offset: 0x00227BA9
		public override void Tick()
		{
			base.Tick();
			if (this.IsHashIntervalTick(2000))
			{
				this.TickLong();
			}
		}

		// Token: 0x0600668A RID: 26250 RVA: 0x002299C4 File Offset: 0x00227BC4
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
				base.TakeDamage(new DamageInfo(DamageDefOf.Rotting, (float)num2, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true));
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

		// Token: 0x0600668B RID: 26251 RVA: 0x00229CC0 File Offset: 0x00227EC0
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

		// Token: 0x0600668C RID: 26252 RVA: 0x00229D3C File Offset: 0x00227F3C
		public bool DeliberatelyCultivated()
		{
			if (!this.def.plant.Sowable)
			{
				return false;
			}
			if (!base.Spawned)
			{
				return false;
			}
			Zone_Growing zone_Growing;
			if ((zone_Growing = (base.Map.zoneManager.ZoneAt(base.Position) as Zone_Growing)) != null && zone_Growing.GetPlantDefToGrow() == this.def)
			{
				return true;
			}
			Building edifice = base.Position.GetEdifice(base.Map);
			return edifice != null && edifice.def.building.SupportsPlants;
		}

		// Token: 0x0600668D RID: 26253 RVA: 0x00229DC0 File Offset: 0x00227FC0
		public virtual bool CanYieldNow()
		{
			return this.HarvestableNow && this.def.plant.harvestYield > 0f && !this.Blighted;
		}

		// Token: 0x0600668E RID: 26254 RVA: 0x00229DF0 File Offset: 0x00227FF0
		public virtual int YieldNow()
		{
			if (!this.CanYieldNow())
			{
				return 0;
			}
			float harvestYield = this.def.plant.harvestYield;
			float num = Mathf.InverseLerp(this.def.plant.harvestMinGrowth, 1f, this.growthInt);
			num = 0.5f + num * 0.5f;
			return GenMath.RoundRandom(harvestYield * num * Mathf.Lerp(0.5f, 1f, (float)this.HitPoints / (float)base.MaxHitPoints) * Find.Storyteller.difficulty.cropYieldFactor);
		}

		// Token: 0x0600668F RID: 26255 RVA: 0x00229E7C File Offset: 0x0022807C
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
					IL_16C:
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
					goto IL_20A;
					IL_157:
					Log.Error(this.def + " must have plant.MaxMeshCount that is a perfect square.");
					goto IL_16C;
				}
				vector = a + Gen.RandomHorizontalVector(0.05f);
				float num10 = (float)base.Position.z;
				if (vector.z - num2 / 2f < num10)
				{
					vector.z = num10 + num2 / 2f;
					flag = true;
				}
				IL_20A:
				bool @bool = Rand.Bool;
				Material matSingle = this.Graphic.MatSingle;
				Vector2[] uvs;
				Color32 color;
				Graphic.TryGetTextureAtlasReplacementInfo(matSingle, this.def.category.ToAtlasGroup(), @bool, false, out matSingle, out uvs, out color);
				PlantUtility.SetWindExposureColors(Plant.workingColors, this);
				Vector2 size = new Vector2(num3, num3);
				Printer_Plane.PrintPlane(layer, vector, size, matSingle, 0f, @bool, uvs, Plant.workingColors, 0.1f, (float)(this.HashOffset() % 1024));
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
				center.y -= 0.04054054f;
				Vector3 volume = this.def.graphicData.shadowData.volume * num2;
				Printer_Shadow.PrintShadow(layer, center, volume, Rot4.North);
			}
			Rand.PopState();
		}

		// Token: 0x06006690 RID: 26256 RVA: 0x0022A1D8 File Offset: 0x002283D8
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.def.plant.showGrowthInInspectPane)
			{
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
							if (Mathf.Approximately(growthRateFactor_Temperature, 0f) || !PlantUtility.GrowthSeasonNow(base.Position, base.Map, false))
							{
								stringBuilder.AppendLine("OutOfIdealTemperatureRangeNotGrowing".Translate());
							}
							else
							{
								stringBuilder.AppendLine("OutOfIdealTemperatureRange".Translate(Mathf.Max(1, Mathf.RoundToInt(growthRateFactor_Temperature * 100f)).ToString()));
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
			}
			string text = base.InspectStringPartsFromComps();
			if (!text.NullOrEmpty())
			{
				stringBuilder.Append(text);
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x06006691 RID: 26257 RVA: 0x0022A410 File Offset: 0x00228610
		public virtual void CropBlighted()
		{
			if (!this.Blighted)
			{
				GenSpawn.Spawn(ThingDefOf.Blight, base.Position, base.Map, WipeMode.Vanish);
			}
		}

		// Token: 0x06006692 RID: 26258 RVA: 0x0022A432 File Offset: 0x00228632
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

		// Token: 0x040039CD RID: 14797
		protected float growthInt = 0.15f;

		// Token: 0x040039CE RID: 14798
		protected int ageInt;

		// Token: 0x040039CF RID: 14799
		protected int unlitTicks;

		// Token: 0x040039D0 RID: 14800
		protected int madeLeaflessTick = -99999;

		// Token: 0x040039D1 RID: 14801
		public bool sown;

		// Token: 0x040039D2 RID: 14802
		private string cachedLabelMouseover;

		// Token: 0x040039D3 RID: 14803
		private static Color32[] workingColors = new Color32[4];

		// Token: 0x040039D4 RID: 14804
		public const float BaseGrowthPercent = 0.15f;

		// Token: 0x040039D5 RID: 14805
		public const float BaseSownGrowthPercent = 0.0001f;

		// Token: 0x040039D6 RID: 14806
		public const float MinGrowthForAnimalIngestion = 0.1f;

		// Token: 0x040039D7 RID: 14807
		private const float BaseDyingDamagePerTick = 0.005f;

		// Token: 0x040039D8 RID: 14808
		private static readonly FloatRange DyingDamagePerTickBecauseExposedToLight = new FloatRange(0.0001f, 0.001f);

		// Token: 0x040039D9 RID: 14809
		private const float GridPosRandomnessFactor = 0.3f;

		// Token: 0x040039DA RID: 14810
		private const int TicksWithoutLightBeforeStartDying = 450000;

		// Token: 0x040039DB RID: 14811
		private const int LeaflessMinRecoveryTicks = 60000;

		// Token: 0x040039DC RID: 14812
		public const float MinGrowthTemperature = 0f;

		// Token: 0x040039DD RID: 14813
		public const float MinOptimalGrowthTemperature = 6f;

		// Token: 0x040039DE RID: 14814
		public const float MaxOptimalGrowthTemperature = 42f;

		// Token: 0x040039DF RID: 14815
		public const float MaxGrowthTemperature = 58f;

		// Token: 0x040039E0 RID: 14816
		private const float MinLeaflessTemperature = -10f;

		// Token: 0x040039E1 RID: 14817
		public const float MaxLeaflessTemperature = -2f;

		// Token: 0x040039E2 RID: 14818
		public const float TopVerticesAltitudeBias = 0.1f;

		// Token: 0x040039E3 RID: 14819
		private static Graphic GraphicSowing = GraphicDatabase.Get<Graphic_Single>("Things/Plant/Plant_Sowing", ShaderDatabase.Cutout, Vector2.one, Color.white);

		// Token: 0x040039E4 RID: 14820
		[TweakValue("Graphics", -1f, 1f)]
		private static float LeafSpawnRadius = 0.4f;

		// Token: 0x040039E5 RID: 14821
		[TweakValue("Graphics", 0f, 2f)]
		private static float LeafSpawnYMin = 0.3f;

		// Token: 0x040039E6 RID: 14822
		[TweakValue("Graphics", 0f, 2f)]
		private static float LeafSpawnYMax = 1f;

		// Token: 0x020024E9 RID: 9449
		public enum LeaflessCause
		{
			// Token: 0x04008C92 RID: 35986
			Cold,
			// Token: 0x04008C93 RID: 35987
			Poison
		}
	}
}
