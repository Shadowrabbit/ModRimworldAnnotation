using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000842 RID: 2114
	public class WorkGiver_GrowerSow : WorkGiver_Grower
	{
		// Token: 0x170009FC RID: 2556
		// (get) Token: 0x06003802 RID: 14338 RVA: 0x00034716 File Offset: 0x00032916
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.ClosestTouch;
			}
		}

		// Token: 0x06003803 RID: 14339 RVA: 0x0013B7F1 File Offset: 0x001399F1
		public static void ResetStaticData()
		{
			WorkGiver_GrowerSow.CantSowCavePlantBecauseOfLightTrans = "CantSowCavePlantBecauseOfLight".Translate();
			WorkGiver_GrowerSow.CantSowCavePlantBecauseUnroofedTrans = "CantSowCavePlantBecauseUnroofed".Translate();
		}

		// Token: 0x06003804 RID: 14340 RVA: 0x0013B81C File Offset: 0x00139A1C
		protected override bool ExtraRequirements(IPlantToGrowSettable settable, Pawn pawn)
		{
			if (!settable.CanAcceptSowNow())
			{
				return false;
			}
			Zone_Growing zone_Growing = settable as Zone_Growing;
			IntVec3 c;
			if (zone_Growing != null)
			{
				if (!zone_Growing.allowSow)
				{
					return false;
				}
				c = zone_Growing.Cells[0];
			}
			else
			{
				c = ((Thing)settable).Position;
			}
			WorkGiver_Grower.wantedPlantDef = WorkGiver_Grower.CalculateWantedPlantDef(c, pawn.Map);
			return WorkGiver_Grower.wantedPlantDef != null;
		}

		// Token: 0x06003805 RID: 14341 RVA: 0x0013B880 File Offset: 0x00139A80
		public override Job JobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			Map map = pawn.Map;
			if (c.IsForbidden(pawn))
			{
				return null;
			}
			if (!PlantUtility.GrowthSeasonNow(c, map, true))
			{
				return null;
			}
			if (WorkGiver_Grower.wantedPlantDef == null)
			{
				WorkGiver_Grower.wantedPlantDef = WorkGiver_Grower.CalculateWantedPlantDef(c, map);
				if (WorkGiver_Grower.wantedPlantDef == null)
				{
					return null;
				}
			}
			List<Thing> thingList = c.GetThingList(map);
			Zone_Growing zone_Growing = c.GetZone(map) as Zone_Growing;
			bool flag = false;
			for (int i = 0; i < thingList.Count; i++)
			{
				Thing thing = thingList[i];
				if (thing.def == WorkGiver_Grower.wantedPlantDef)
				{
					return null;
				}
				if ((thing is Blueprint || thing is Frame) && thing.Faction == pawn.Faction)
				{
					flag = true;
				}
			}
			if (flag)
			{
				Thing edifice = c.GetEdifice(map);
				if (edifice == null || edifice.def.fertility < 0f)
				{
					return null;
				}
			}
			if (WorkGiver_Grower.wantedPlantDef.plant.cavePlant)
			{
				if (!c.Roofed(map))
				{
					JobFailReason.Is(WorkGiver_GrowerSow.CantSowCavePlantBecauseUnroofedTrans, null);
					return null;
				}
				if (map.glowGrid.GameGlowAt(c, true) > 0f)
				{
					JobFailReason.Is(WorkGiver_GrowerSow.CantSowCavePlantBecauseOfLightTrans, null);
					return null;
				}
			}
			if (WorkGiver_Grower.wantedPlantDef.plant.interferesWithRoof && c.Roofed(pawn.Map))
			{
				return null;
			}
			Plant plant = c.GetPlant(map);
			if (plant != null && plant.def.plant.blockAdjacentSow)
			{
				if (!pawn.CanReserve(plant, 1, -1, null, forced) || plant.IsForbidden(pawn))
				{
					return null;
				}
				if (zone_Growing != null && !zone_Growing.allowCut)
				{
					return null;
				}
				if (!PlantUtility.PawnWillingToCutPlant_Job(plant, pawn))
				{
					return null;
				}
				return JobMaker.MakeJob(JobDefOf.CutPlant, plant);
			}
			else
			{
				Thing thing2 = PlantUtility.AdjacentSowBlocker(WorkGiver_Grower.wantedPlantDef, c, map);
				if (thing2 != null)
				{
					Plant plant2 = thing2 as Plant;
					if (plant2 != null && pawn.CanReserveAndReach(plant2, PathEndMode.Touch, Danger.Deadly, 1, -1, null, forced) && !plant2.IsForbidden(pawn))
					{
						IPlantToGrowSettable plantToGrowSettable = plant2.Position.GetPlantToGrowSettable(plant2.Map);
						if (plantToGrowSettable == null || plantToGrowSettable.GetPlantDefToGrow() != plant2.def)
						{
							Zone_Growing zone_Growing2 = thing2.Position.GetZone(map) as Zone_Growing;
							if (zone_Growing2 != null && !zone_Growing2.allowCut)
							{
								return null;
							}
							if (!PlantUtility.PawnWillingToCutPlant_Job(plant2, pawn))
							{
								return null;
							}
							return JobMaker.MakeJob(JobDefOf.CutPlant, plant2);
						}
					}
					return null;
				}
				if (WorkGiver_Grower.wantedPlantDef.plant.sowMinSkill > 0 && pawn.skills != null && pawn.skills.GetSkill(SkillDefOf.Plants).Level < WorkGiver_Grower.wantedPlantDef.plant.sowMinSkill)
				{
					JobFailReason.Is("UnderAllowedSkill".Translate(WorkGiver_Grower.wantedPlantDef.plant.sowMinSkill), this.def.label);
					return null;
				}
				int j = 0;
				while (j < thingList.Count)
				{
					Thing thing3 = thingList[j];
					if (thing3.def.BlocksPlanting(false))
					{
						if (!pawn.CanReserve(thing3, 1, -1, null, forced))
						{
							return null;
						}
						if (thing3.def.category == ThingCategory.Plant)
						{
							if (thing3.IsForbidden(pawn))
							{
								return null;
							}
							if (zone_Growing != null && !zone_Growing.allowCut)
							{
								return null;
							}
							if (!PlantUtility.PawnWillingToCutPlant_Job(thing3, pawn))
							{
								return null;
							}
							return JobMaker.MakeJob(JobDefOf.CutPlant, thing3);
						}
						else
						{
							if (thing3.def.EverHaulable)
							{
								return HaulAIUtility.HaulAsideJobFor(pawn, thing3);
							}
							return null;
						}
					}
					else
					{
						j++;
					}
				}
				if (!WorkGiver_Grower.wantedPlantDef.CanNowPlantAt(c, map, false) || !PlantUtility.GrowthSeasonNow(c, map, true) || !pawn.CanReserve(c, 1, -1, null, forced))
				{
					return null;
				}
				Job job = JobMaker.MakeJob(JobDefOf.Sow, c);
				job.plantDefToSow = WorkGiver_Grower.wantedPlantDef;
				return job;
			}
		}

		// Token: 0x04001F2B RID: 7979
		protected static string CantSowCavePlantBecauseOfLightTrans;

		// Token: 0x04001F2C RID: 7980
		protected static string CantSowCavePlantBecauseUnroofedTrans;
	}
}
