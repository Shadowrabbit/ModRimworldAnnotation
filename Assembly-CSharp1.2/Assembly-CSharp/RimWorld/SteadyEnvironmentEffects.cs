using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02001D71 RID: 7537
	public class SteadyEnvironmentEffects
	{
		// Token: 0x0600A3D7 RID: 41943 RVA: 0x0006CB05 File Offset: 0x0006AD05
		public SteadyEnvironmentEffects(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600A3D8 RID: 41944 RVA: 0x002FB348 File Offset: 0x002F9548
		public void SteadyEnvironmentEffectsTick()
		{
			if ((float)Find.TickManager.TicksGame % 97f == 0f && Rand.Chance(0.02f))
			{
				this.RollForRainFire();
			}
			this.outdoorMeltAmount = this.MeltAmountAt(this.map.mapTemperature.OutdoorTemp);
			this.snowRate = this.map.weatherManager.SnowRate;
			this.rainRate = this.map.weatherManager.RainRate;
			this.deteriorationRate = Mathf.Lerp(1f, 5f, this.rainRate);
			int num = Mathf.CeilToInt((float)this.map.Area * 0.0006f);
			int area = this.map.Area;
			for (int i = 0; i < num; i++)
			{
				if (this.cycleIndex >= area)
				{
					this.cycleIndex = 0;
				}
				IntVec3 c = this.map.cellsInRandomOrder.Get(this.cycleIndex);
				this.DoCellSteadyEffects(c);
				this.cycleIndex++;
			}
		}

		// Token: 0x0600A3D9 RID: 41945 RVA: 0x002FB450 File Offset: 0x002F9650
		private void DoCellSteadyEffects(IntVec3 c)
		{
			Room room = c.GetRoom(this.map, RegionType.Set_All);
			bool flag = this.map.roofGrid.Roofed(c);
			bool flag2 = room != null && room.UsesOutdoorTemperature;
			if (room == null || flag2)
			{
				if (this.outdoorMeltAmount > 0f)
				{
					this.map.snowGrid.AddDepth(c, -this.outdoorMeltAmount);
				}
				if (!flag && this.snowRate > 0.001f)
				{
					this.AddFallenSnowAt(c, 0.046f * this.map.weatherManager.SnowRate);
				}
			}
			if (room != null)
			{
				bool protectedByEdifice = SteadyEnvironmentEffects.ProtectedByEdifice(c, this.map);
				TerrainDef terrain = c.GetTerrain(this.map);
				List<Thing> thingList = c.GetThingList(this.map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Thing thing = thingList[i];
					Filth filth = thing as Filth;
					if (filth != null)
					{
						if (!flag && thing.def.filth.rainWashes && Rand.Chance(this.rainRate))
						{
							filth.ThinFilth();
						}
						if (filth.DisappearAfterTicks != 0 && filth.TicksSinceThickened > filth.DisappearAfterTicks)
						{
							filth.Destroy(DestroyMode.Vanish);
						}
					}
					else
					{
						this.TryDoDeteriorate(thing, flag, flag2, protectedByEdifice, terrain);
					}
				}
				if (!flag2)
				{
					float temperature = room.Temperature;
					if (temperature > 0f)
					{
						float num = this.MeltAmountAt(temperature);
						if (num > 0f)
						{
							this.map.snowGrid.AddDepth(c, -num);
						}
						if (room.RegionType.Passable() && temperature > SteadyEnvironmentEffects.AutoIgnitionTemperatureRange.min)
						{
							float value = Rand.Value;
							if (value < SteadyEnvironmentEffects.AutoIgnitionTemperatureRange.InverseLerpThroughRange(temperature) * 0.7f && Rand.Chance(FireUtility.ChanceToStartFireIn(c, this.map)))
							{
								FireUtility.TryStartFireIn(c, this.map, 0.1f);
							}
							if (value < 0.33f)
							{
								MoteMaker.ThrowHeatGlow(c, this.map, 2.3f);
							}
						}
					}
				}
			}
			this.map.gameConditionManager.DoSteadyEffects(c, this.map);
		}

		// Token: 0x0600A3DA RID: 41946 RVA: 0x002FB66C File Offset: 0x002F986C
		private static bool ProtectedByEdifice(IntVec3 c, Map map)
		{
			Building edifice = c.GetEdifice(map);
			return edifice != null && edifice.def.building != null && edifice.def.building.preventDeteriorationOnTop;
		}

		// Token: 0x0600A3DB RID: 41947 RVA: 0x0006CB14 File Offset: 0x0006AD14
		private float MeltAmountAt(float temperature)
		{
			if (temperature < 0f)
			{
				return 0f;
			}
			if (temperature < 10f)
			{
				return temperature * temperature * 0.0058f * 0.1f;
			}
			return temperature * 0.0058f;
		}

		// Token: 0x0600A3DC RID: 41948 RVA: 0x002FB6A8 File Offset: 0x002F98A8
		public void AddFallenSnowAt(IntVec3 c, float baseAmount)
		{
			if (this.snowNoise == null)
			{
				this.snowNoise = new Perlin(0.03999999910593033, 2.0, 0.5, 5, Rand.Range(0, 651431), QualityMode.Medium);
			}
			float num = this.snowNoise.GetValue(c);
			num += 1f;
			num *= 0.5f;
			if (num < 0.5f)
			{
				num = 0.5f;
			}
			float depthToAdd = baseAmount * num;
			this.map.snowGrid.AddDepth(c, depthToAdd);
		}

		// Token: 0x0600A3DD RID: 41949 RVA: 0x002FB734 File Offset: 0x002F9934
		public static float FinalDeteriorationRate(Thing t, List<string> reasons = null)
		{
			if (t.Spawned)
			{
				Room room = t.GetRoom(RegionType.Set_Passable);
				return SteadyEnvironmentEffects.FinalDeteriorationRate(t, t.Position.Roofed(t.Map), room != null && room.UsesOutdoorTemperature, SteadyEnvironmentEffects.ProtectedByEdifice(t.Position, t.Map), t.Position.GetTerrain(t.Map), reasons);
			}
			return SteadyEnvironmentEffects.FinalDeteriorationRate(t, false, false, false, null, reasons);
		}

		// Token: 0x0600A3DE RID: 41950 RVA: 0x002FB7A4 File Offset: 0x002F99A4
		public static float FinalDeteriorationRate(Thing t, bool roofed, bool roomUsesOutdoorTemperature, bool protectedByEdifice, TerrainDef terrain, List<string> reasons = null)
		{
			if (!t.def.CanEverDeteriorate)
			{
				return 0f;
			}
			if (protectedByEdifice)
			{
				return 0f;
			}
			float statValue = t.GetStatValue(StatDefOf.DeteriorationRate, true);
			if (statValue <= 0f)
			{
				return 0f;
			}
			float num = 0f;
			if (!roofed)
			{
				num += 0.5f;
				if (reasons != null)
				{
					reasons.Add("DeterioratingUnroofed".Translate());
				}
			}
			if (roomUsesOutdoorTemperature)
			{
				num += 0.5f;
				if (reasons != null)
				{
					reasons.Add("DeterioratingOutdoors".Translate());
				}
			}
			if (terrain != null && terrain.extraDeteriorationFactor != 0f)
			{
				num += terrain.extraDeteriorationFactor;
				if (reasons != null)
				{
					reasons.Add(terrain.label);
				}
			}
			if (num <= 0f)
			{
				return 0f;
			}
			return statValue * num;
		}

		// Token: 0x0600A3DF RID: 41951 RVA: 0x002FB878 File Offset: 0x002F9A78
		private void TryDoDeteriorate(Thing t, bool roofed, bool roomUsesOutdoorTemperature, bool protectedByEdifice, TerrainDef terrain)
		{
			Corpse corpse = t as Corpse;
			if (corpse != null && corpse.InnerPawn.apparel != null)
			{
				List<Apparel> wornApparel = corpse.InnerPawn.apparel.WornApparel;
				for (int i = 0; i < wornApparel.Count; i++)
				{
					this.TryDoDeteriorate(wornApparel[i], roofed, roomUsesOutdoorTemperature, protectedByEdifice, terrain);
				}
			}
			float num = SteadyEnvironmentEffects.FinalDeteriorationRate(t, roofed, roomUsesOutdoorTemperature, protectedByEdifice, terrain, null);
			if (num < 0.001f)
			{
				return;
			}
			if (Rand.Chance(this.deteriorationRate * num / 36f))
			{
				IntVec3 position = t.Position;
				Map map = t.Map;
				bool flag = t.IsInAnyStorage();
				t.TakeDamage(new DamageInfo(DamageDefOf.Deterioration, 1f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
				if (flag && t.Destroyed && t.def.messageOnDeteriorateInStorage)
				{
					Messages.Message("MessageDeterioratedAway".Translate(t.Label), new TargetInfo(position, map, false), MessageTypeDefOf.NegativeEvent, true);
				}
			}
		}

		// Token: 0x0600A3E0 RID: 41952 RVA: 0x002FB988 File Offset: 0x002F9B88
		private void RollForRainFire()
		{
			if (!Rand.Chance(0.2f * (float)this.map.listerBuildings.allBuildingsColonistElecFire.Count * this.map.weatherManager.RainRate))
			{
				return;
			}
			Building building = this.map.listerBuildings.allBuildingsColonistElecFire.RandomElement<Building>();
			if (!this.map.roofGrid.Roofed(building.Position))
			{
				ShortCircuitUtility.TryShortCircuitInRain(building);
			}
		}

		// Token: 0x04006EF2 RID: 28402
		private Map map;

		// Token: 0x04006EF3 RID: 28403
		private ModuleBase snowNoise;

		// Token: 0x04006EF4 RID: 28404
		private int cycleIndex;

		// Token: 0x04006EF5 RID: 28405
		private float outdoorMeltAmount;

		// Token: 0x04006EF6 RID: 28406
		private float snowRate;

		// Token: 0x04006EF7 RID: 28407
		private float rainRate;

		// Token: 0x04006EF8 RID: 28408
		private float deteriorationRate;

		// Token: 0x04006EF9 RID: 28409
		private const float MapFractionCheckPerTick = 0.0006f;

		// Token: 0x04006EFA RID: 28410
		private const float RainFireCheckInterval = 97f;

		// Token: 0x04006EFB RID: 28411
		private const float RainFireChanceOverall = 0.02f;

		// Token: 0x04006EFC RID: 28412
		private const float RainFireChancePerBuilding = 0.2f;

		// Token: 0x04006EFD RID: 28413
		private const float SnowFallRateFactor = 0.046f;

		// Token: 0x04006EFE RID: 28414
		private const float SnowMeltRateFactor = 0.0058f;

		// Token: 0x04006EFF RID: 28415
		private static readonly FloatRange AutoIgnitionTemperatureRange = new FloatRange(240f, 1000f);

		// Token: 0x04006F00 RID: 28416
		private const float AutoIgnitionChanceFactor = 0.7f;

		// Token: 0x04006F01 RID: 28417
		private const float FireGlowRate = 0.33f;
	}
}
