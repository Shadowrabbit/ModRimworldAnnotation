using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BE3 RID: 3043
	public class GameCondition_ToxicFallout : GameCondition
	{
		// Token: 0x17000C8E RID: 3214
		// (get) Token: 0x060047A2 RID: 18338 RVA: 0x0017AEF3 File Offset: 0x001790F3
		public override int TransitionTicks
		{
			get
			{
				return 5000;
			}
		}

		// Token: 0x060047A3 RID: 18339 RVA: 0x0017AF02 File Offset: 0x00179102
		public override void Init()
		{
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.ForbiddingDoors, OpportunityType.Critical);
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.AllowedAreas, OpportunityType.Critical);
		}

		// Token: 0x060047A4 RID: 18340 RVA: 0x0017AF1C File Offset: 0x0017911C
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

		// Token: 0x060047A5 RID: 18341 RVA: 0x0017AFA0 File Offset: 0x001791A0
		private void DoPawnsToxicDamage(Map map)
		{
			List<Pawn> allPawnsSpawned = map.mapPawns.AllPawnsSpawned;
			for (int i = 0; i < allPawnsSpawned.Count; i++)
			{
				GameCondition_ToxicFallout.DoPawnToxicDamage(allPawnsSpawned[i]);
			}
		}

		// Token: 0x060047A6 RID: 18342 RVA: 0x0017AFD8 File Offset: 0x001791D8
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

		// Token: 0x060047A7 RID: 18343 RVA: 0x0017B060 File Offset: 0x00179260
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

		// Token: 0x060047A8 RID: 18344 RVA: 0x0017B10C File Offset: 0x0017930C
		public override void GameConditionDraw(Map map)
		{
			for (int i = 0; i < this.overlays.Count; i++)
			{
				this.overlays[i].DrawOverlay(map);
			}
		}

		// Token: 0x060047A9 RID: 18345 RVA: 0x0017B141 File Offset: 0x00179341
		public override float SkyTargetLerpFactor(Map map)
		{
			return GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, 0.5f);
		}

		// Token: 0x060047AA RID: 18346 RVA: 0x0017B155 File Offset: 0x00179355
		public override SkyTarget? SkyTarget(Map map)
		{
			return new SkyTarget?(new SkyTarget(0.85f, this.ToxicFalloutColors, 1f, 1f));
		}

		// Token: 0x060047AB RID: 18347 RVA: 0x000682C5 File Offset: 0x000664C5
		public override float AnimalDensityFactor(Map map)
		{
			return 0f;
		}

		// Token: 0x060047AC RID: 18348 RVA: 0x000682C5 File Offset: 0x000664C5
		public override float PlantDensityFactor(Map map)
		{
			return 0f;
		}

		// Token: 0x060047AD RID: 18349 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool AllowEnjoyableOutsideNow(Map map)
		{
			return false;
		}

		// Token: 0x060047AE RID: 18350 RVA: 0x0017B176 File Offset: 0x00179376
		public override List<SkyOverlay> SkyOverlays(Map map)
		{
			return this.overlays;
		}

		// Token: 0x04002BF4 RID: 11252
		private const float MaxSkyLerpFactor = 0.5f;

		// Token: 0x04002BF5 RID: 11253
		private const float SkyGlow = 0.85f;

		// Token: 0x04002BF6 RID: 11254
		private SkyColorSet ToxicFalloutColors = new SkyColorSet(new ColorInt(216, 255, 0).ToColor, new ColorInt(234, 200, 255).ToColor, new Color(0.6f, 0.8f, 0.5f), 0.85f);

		// Token: 0x04002BF7 RID: 11255
		private List<SkyOverlay> overlays = new List<SkyOverlay>
		{
			new WeatherOverlay_Fallout()
		};

		// Token: 0x04002BF8 RID: 11256
		public const int CheckInterval = 3451;

		// Token: 0x04002BF9 RID: 11257
		private const float ToxicPerDay = 0.5f;

		// Token: 0x04002BFA RID: 11258
		private const float PlantKillChance = 0.0065f;

		// Token: 0x04002BFB RID: 11259
		private const float CorpseRotProgressAdd = 3000f;
	}
}
