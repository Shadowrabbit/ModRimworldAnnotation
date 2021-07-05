using System;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000759 RID: 1881
	public static class Toils_LayDown
	{
		// Token: 0x0600341F RID: 13343 RVA: 0x00127D60 File Offset: 0x00125F60
		public static Toil LayDown(TargetIndex bedOrRestSpotIndex, bool hasBed, bool lookForOtherJobs, bool canSleep = true, bool gainRestAndHealth = true, PawnPosture noBedLayingPosture = PawnPosture.LayingOnGroundNormal)
		{
			Toil layDown = new Toil();
			layDown.initAction = delegate()
			{
				Pawn actor = layDown.actor;
				actor.pather.StopDead();
				JobDriver curDriver = actor.jobs.curDriver;
				if (hasBed)
				{
					Building_Bed building_Bed = (Building_Bed)actor.CurJob.GetTarget(bedOrRestSpotIndex).Thing;
					if (!building_Bed.OccupiedRect().Contains(actor.Position))
					{
						Log.Error("Can't start LayDown toil because pawn is not in the bed. pawn=" + actor);
						actor.jobs.EndCurrentJob(JobCondition.Errored, true, true);
						return;
					}
					actor.jobs.posture = PawnPosture.LayingInBed;
					actor.mindState.lastBedDefSleptIn = building_Bed.def;
				}
				else
				{
					actor.jobs.posture = noBedLayingPosture;
					actor.mindState.lastBedDefSleptIn = null;
				}
				curDriver.asleep = false;
				if (actor.mindState.applyBedThoughtsTick == 0)
				{
					actor.mindState.applyBedThoughtsTick = Find.TickManager.TicksGame + Rand.Range(2500, 10000);
					actor.mindState.applyBedThoughtsOnLeave = false;
				}
				if (actor.ownership != null && actor.CurrentBed() != actor.ownership.OwnedBed)
				{
					ThoughtUtility.RemovePositiveBedroomThoughts(actor);
				}
				CompCanBeDormant comp = actor.GetComp<CompCanBeDormant>();
				if (comp != null)
				{
					comp.ToSleep();
				}
			};
			layDown.tickAction = delegate()
			{
				Pawn actor = layDown.actor;
				Job curJob = actor.CurJob;
				JobDriver curDriver = actor.jobs.curDriver;
				Building_Bed building_Bed = curJob.GetTarget(bedOrRestSpotIndex).Thing as Building_Bed;
				actor.GainComfortFromCellIfPossible(false);
				if (!curDriver.asleep)
				{
					if (canSleep && ((actor.needs.rest != null && actor.needs.rest.CurLevel < RestUtility.FallAsleepMaxLevel(actor)) || curJob.forceSleep))
					{
						curDriver.asleep = true;
					}
				}
				else if (!canSleep)
				{
					curDriver.asleep = false;
				}
				else if ((actor.needs.rest == null || actor.needs.rest.CurLevel >= RestUtility.WakeThreshold(actor)) && !curJob.forceSleep)
				{
					curDriver.asleep = false;
				}
				if (curDriver.asleep && gainRestAndHealth && actor.needs.rest != null)
				{
					float restEffectiveness;
					if (building_Bed != null && building_Bed.def.statBases.StatListContains(StatDefOf.BedRestEffectiveness))
					{
						restEffectiveness = building_Bed.GetStatValue(StatDefOf.BedRestEffectiveness, true);
					}
					else
					{
						restEffectiveness = StatDefOf.BedRestEffectiveness.valueIfMissing;
					}
					actor.needs.rest.TickResting(restEffectiveness);
				}
				if (actor.mindState.applyBedThoughtsTick != 0 && actor.mindState.applyBedThoughtsTick <= Find.TickManager.TicksGame)
				{
					Toils_LayDown.ApplyBedThoughts(actor);
					actor.mindState.applyBedThoughtsTick += 60000;
					actor.mindState.applyBedThoughtsOnLeave = true;
				}
				if (actor.IsHashIntervalTick(100) && !actor.Position.Fogged(actor.Map))
				{
					if (curDriver.asleep)
					{
						FleckMaker.ThrowMetaIcon(actor.Position, actor.Map, FleckDefOf.SleepZ, 0.42f);
					}
					if (gainRestAndHealth && actor.health.hediffSet.GetNaturallyHealingInjuredParts().Any<BodyPartRecord>())
					{
						FleckMaker.ThrowMetaIcon(actor.Position, actor.Map, FleckDefOf.HealingCross, 0.42f);
					}
				}
				if (ModsConfig.IdeologyActive && building_Bed != null && actor.IsHashIntervalTick(2500) && !actor.Awake() && (actor.IsFreeColonist || actor.IsPrisonerOfColony) && !actor.IsSlaveOfColony)
				{
					Room room = building_Bed.GetRoom(RegionType.Set_All);
					if (!room.PsychologicallyOutdoors)
					{
						bool flag = false;
						foreach (Building_Bed building_Bed2 in room.ContainedBeds)
						{
							foreach (Pawn pawn in building_Bed2.CurOccupants)
							{
								if (pawn != actor && !pawn.Awake() && pawn.IsSlave && !LovePartnerRelationUtility.LovePartnerRelationExists(actor, pawn))
								{
									actor.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.SleptInRoomWithSlave, null, null);
									flag = true;
									break;
								}
							}
							if (flag)
							{
								break;
							}
						}
					}
				}
				if (actor.ownership != null && building_Bed != null && !building_Bed.Medical && !building_Bed.OwnersForReading.Contains(actor))
				{
					if (actor.Downed)
					{
						actor.Position = CellFinder.RandomClosewalkCellNear(actor.Position, actor.Map, 1, null);
					}
					actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
					return;
				}
				if (lookForOtherJobs && actor.IsHashIntervalTick(211))
				{
					actor.jobs.CheckForJobOverride();
					return;
				}
			};
			layDown.defaultCompleteMode = ToilCompleteMode.Never;
			if (hasBed)
			{
				layDown.FailOnBedNoLongerUsable(bedOrRestSpotIndex);
			}
			layDown.AddFinishAction(delegate
			{
				Pawn actor = layDown.actor;
				JobDriver curDriver = actor.jobs.curDriver;
				if (actor.mindState.applyBedThoughtsOnLeave)
				{
					Toils_LayDown.ApplyBedThoughts(actor);
				}
				curDriver.asleep = false;
			});
			return layDown;
		}

		// Token: 0x06003420 RID: 13344 RVA: 0x00127E1C File Offset: 0x0012601C
		private static void ApplyBedThoughts(Pawn actor)
		{
			if (actor.needs.mood == null)
			{
				return;
			}
			Building_Bed building_Bed = actor.CurrentBed();
			actor.needs.mood.thoughts.memories.RemoveMemoriesOfDef(ThoughtDefOf.SleptInBedroom);
			actor.needs.mood.thoughts.memories.RemoveMemoriesOfDef(ThoughtDefOf.SleptInBarracks);
			actor.needs.mood.thoughts.memories.RemoveMemoriesOfDef(ThoughtDefOf.SleptOutside);
			actor.needs.mood.thoughts.memories.RemoveMemoriesOfDef(ThoughtDefOf.SleptOnGround);
			actor.needs.mood.thoughts.memories.RemoveMemoriesOfDef(ThoughtDefOf.SleptInCold);
			actor.needs.mood.thoughts.memories.RemoveMemoriesOfDef(ThoughtDefOf.SleptInHeat);
			if (actor.GetRoom(RegionType.Set_All).PsychologicallyOutdoors)
			{
				actor.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.SleptOutside, null, null);
			}
			if (building_Bed == null || building_Bed.CostListAdjusted().Count == 0)
			{
				actor.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.SleptOnGround, null, null);
			}
			if (actor.AmbientTemperature < actor.def.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null))
			{
				actor.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.SleptInCold, null, null);
			}
			if (actor.AmbientTemperature > actor.def.GetStatValueAbstract(StatDefOf.ComfyTemperatureMax, null))
			{
				actor.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.SleptInHeat, null, null);
			}
			if (building_Bed != null && building_Bed == actor.ownership.OwnedBed && !building_Bed.ForPrisoners && !actor.story.traits.HasTrait(TraitDefOf.Ascetic))
			{
				ThoughtDef thoughtDef = null;
				if (building_Bed.GetRoom(RegionType.Set_All).Role == RoomRoleDefOf.Bedroom)
				{
					thoughtDef = ThoughtDefOf.SleptInBedroom;
				}
				else if (building_Bed.GetRoom(RegionType.Set_All).Role == RoomRoleDefOf.Barracks)
				{
					thoughtDef = ThoughtDefOf.SleptInBarracks;
				}
				if (thoughtDef != null)
				{
					int scoreStageIndex = RoomStatDefOf.Impressiveness.GetScoreStageIndex(building_Bed.GetRoom(RegionType.Set_All).GetStat(RoomStatDefOf.Impressiveness));
					if (thoughtDef.stages[scoreStageIndex] != null)
					{
						actor.needs.mood.thoughts.memories.TryGainMemory(ThoughtMaker.MakeThought(thoughtDef, scoreStageIndex), null);
					}
				}
			}
			actor.Notify_AddBedThoughts();
		}

		// Token: 0x04001E35 RID: 7733
		private const int TicksBetweenSleepZs = 100;

		// Token: 0x04001E36 RID: 7734
		private const int GetUpOrStartJobWhileInBedCheckInterval = 211;

		// Token: 0x04001E37 RID: 7735
		private const int SlavesInSleepingRoomCheckInterval = 2500;
	}
}
