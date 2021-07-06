using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200117A RID: 4474
	public class GameCondition_ToxicFallout : GameCondition
	{
		// Token: 0x17000F77 RID: 3959
		// (get) Token: 0x0600628E RID: 25230 RVA: 0x00043D77 File Offset: 0x00041F77
		public override int TransitionTicks
		{
			get
			{
				return 5000;
			}
		}

		// Token: 0x0600628F RID: 25231 RVA: 0x00043D86 File Offset: 0x00041F86
		public override void Init()
		{
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.ForbiddingDoors, OpportunityType.Critical);
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.AllowedAreas, OpportunityType.Critical);
		}

		// Token: 0x06006290 RID: 25232 RVA: 0x001EC574 File Offset: 0x001EA774
		public override void GameConditionTick()
		{
			List<Map> affectedMaps = base.AffectedMaps;
			if (Find.TickManager.TicksGame % 3451 == 0)
			{
				for (int i = 0; i < affectedMaps.Count; i++)
				{
					this.DoPawnsToxicDamage(affectedMaps[i]);
				}
			}
			for (int j = 0; j < this.overlays.Count; j++)
			{
				for (int k = 0; k < affectedMaps.Count; k++)
				{
					this.overlays[j].TickOverlay(affectedMaps[k]);
				}
			}
		}

		// Token: 0x06006291 RID: 25233 RVA: 0x001EC5F8 File Offset: 0x001EA7F8
		private void DoPawnsToxicDamage(Map map)
		{
			List<Pawn> allPawnsSpawned = map.mapPawns.AllPawnsSpawned;
			for (int i = 0; i < allPawnsSpawned.Count; i++)
			{
				GameCondition_ToxicFallout.DoPawnToxicDamage(allPawnsSpawned[i]);
			}
		}

		// Token: 0x06006292 RID: 25234 RVA: 0x001EC630 File Offset: 0x001EA830
		public static void DoPawnToxicDamage(Pawn p)
		{
			if (p.Spawned && p.Position.Roofed(p.Map))
			{
				return;
			}
			if (!p.RaceProps.IsFlesh)
			{
				return;
			}
			float num = 0.028758334f;
			num *= p.GetStatValue(StatDefOf.ToxicSensitivity, true);
			if (num != 0f)
			{
				float num2 = Mathf.Lerp(0.85f, 1.15f, Rand.ValueSeeded(p.thingIDNumber ^ 74374237));
				num *= num2;
				HealthUtility.AdjustSeverity(p, HediffDefOf.ToxicBuildup, num);
			}
		}

		// Token: 0x06006293 RID: 25235 RVA: 0x001EC6B8 File Offset: 0x001EA8B8
		public override void DoCellSteadyEffects(IntVec3 c, Map map)
		{
			if (!c.Roofed(map))
			{
				List<Thing> thingList = c.GetThingList(map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Thing thing = thingList[i];
					if (thing is Plant)
					{
						if (thing.def.plant.dieFromToxicFallout && Rand.Value < 0.0065f)
						{
							thing.Kill(null, null);
						}
					}
					else if (thing.def.category == ThingCategory.Item)
					{
						CompRottable compRottable = thing.TryGetComp<CompRottable>();
						if (compRottable != null && compRottable.Stage < RotStage.Dessicated)
						{
							compRottable.RotProgress += 3000f;
						}
					}
				}
			}
		}

		// Token: 0x06006294 RID: 25236 RVA: 0x001EC764 File Offset: 0x001EA964
		public override void GameConditionDraw(Map map)
		{
			for (int i = 0; i < this.overlays.Count; i++)
			{
				this.overlays[i].DrawOverlay(map);
			}
		}

		// Token: 0x06006295 RID: 25237 RVA: 0x00043D9E File Offset: 0x00041F9E
		public override float SkyTargetLerpFactor(Map map)
		{
			return GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, 0.5f);
		}

		// Token: 0x06006296 RID: 25238 RVA: 0x00043DB2 File Offset: 0x00041FB2
		public override SkyTarget? SkyTarget(Map map)
		{
			return new SkyTarget?(new SkyTarget(0.85f, this.ToxicFalloutColors, 1f, 1f));
		}

		// Token: 0x06006297 RID: 25239 RVA: 0x00016647 File Offset: 0x00014847
		public override float AnimalDensityFactor(Map map)
		{
			return 0f;
		}

		// Token: 0x06006298 RID: 25240 RVA: 0x00016647 File Offset: 0x00014847
		public override float PlantDensityFactor(Map map)
		{
			return 0f;
		}

		// Token: 0x06006299 RID: 25241 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool AllowEnjoyableOutsideNow(Map map)
		{
			return false;
		}

		// Token: 0x0600629A RID: 25242 RVA: 0x00043DD3 File Offset: 0x00041FD3
		public override List<SkyOverlay> SkyOverlays(Map map)
		{
			return this.overlays;
		}

		// Token: 0x0400420C RID: 16908
		private const float MaxSkyLerpFactor = 0.5f;

		// Token: 0x0400420D RID: 16909
		private const float SkyGlow = 0.85f;

		// Token: 0x0400420E RID: 16910
		private SkyColorSet ToxicFalloutColors = new SkyColorSet(new ColorInt(216, 255, 0).ToColor, new ColorInt(234, 200, 255).ToColor, new Color(0.6f, 0.8f, 0.5f), 0.85f);

		// Token: 0x0400420F RID: 16911
		private List<SkyOverlay> overlays = new List<SkyOverlay>
		{
			new WeatherOverlay_Fallout()
		};

		// Token: 0x04004210 RID: 16912
		public const int CheckInterval = 3451;

		// Token: 0x04004211 RID: 16913
		private const float ToxicPerDay = 0.5f;

		// Token: 0x04004212 RID: 16914
		private const float PlantKillChance = 0.0065f;

		// Token: 0x04004213 RID: 16915
		private const float CorpseRotProgressAdd = 3000f;
	}
}
