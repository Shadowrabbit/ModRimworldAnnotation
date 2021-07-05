using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000800 RID: 2048
	public class JobGiver_Work : ThinkNode
	{
		// Token: 0x0600369F RID: 13983 RVA: 0x001358A9 File Offset: 0x00133AA9
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_Work jobGiver_Work = (JobGiver_Work)base.DeepCopy(resolve);
			jobGiver_Work.emergency = this.emergency;
			return jobGiver_Work;
		}

		// Token: 0x060036A0 RID: 13984 RVA: 0x001358C4 File Offset: 0x00133AC4
		public override float GetPriority(Pawn pawn)
		{
			if (pawn.workSettings == null || !pawn.workSettings.EverWork)
			{
				return 0f;
			}
			TimeAssignmentDef timeAssignmentDef = (pawn.timetable == null) ? TimeAssignmentDefOf.Anything : pawn.timetable.CurrentAssignment;
			if (timeAssignmentDef == TimeAssignmentDefOf.Anything)
			{
				return 5.5f;
			}
			if (timeAssignmentDef == TimeAssignmentDefOf.Work)
			{
				return 9f;
			}
			if (timeAssignmentDef == TimeAssignmentDefOf.Sleep)
			{
				return 3f;
			}
			if (timeAssignmentDef == TimeAssignmentDefOf.Joy)
			{
				return 2f;
			}
			if (timeAssignmentDef == TimeAssignmentDefOf.Meditate)
			{
				return 2f;
			}
			throw new NotImplementedException();
		}

		// Token: 0x060036A1 RID: 13985 RVA: 0x00135954 File Offset: 0x00133B54
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			JobGiver_Work.<>c__DisplayClass3_0 CS$<>8__locals1 = new JobGiver_Work.<>c__DisplayClass3_0();
			CS$<>8__locals1.pawn = pawn;
			if (this.emergency && CS$<>8__locals1.pawn.mindState.priorityWork.IsPrioritized)
			{
				List<WorkGiverDef> workGiversByPriority = CS$<>8__locals1.pawn.mindState.priorityWork.WorkGiver.workType.workGiversByPriority;
				for (int i = 0; i < workGiversByPriority.Count; i++)
				{
					WorkGiver worker = workGiversByPriority[i].Worker;
					if (this.WorkGiversRelated(CS$<>8__locals1.pawn.mindState.priorityWork.WorkGiver, worker.def))
					{
						Job job = this.GiverTryGiveJobPrioritized(CS$<>8__locals1.pawn, worker, CS$<>8__locals1.pawn.mindState.priorityWork.Cell);
						if (job != null)
						{
							job.playerForced = true;
							return new ThinkResult(job, this, new JobTag?(workGiversByPriority[i].tagToGive), false);
						}
					}
				}
				CS$<>8__locals1.pawn.mindState.priorityWork.Clear();
			}
			List<WorkGiver> list = (!this.emergency) ? CS$<>8__locals1.pawn.workSettings.WorkGiversInOrderNormal : CS$<>8__locals1.pawn.workSettings.WorkGiversInOrderEmergency;
			int num = -999;
			CS$<>8__locals1.bestTargetOfLastPriority = TargetInfo.Invalid;
			CS$<>8__locals1.scannerWhoProvidedTarget = null;
			for (int j = 0; j < list.Count; j++)
			{
				WorkGiver workGiver = list[j];
				if (workGiver.def.priorityInType != num && CS$<>8__locals1.bestTargetOfLastPriority.IsValid)
				{
					break;
				}
				if (this.PawnCanUseWorkGiver(CS$<>8__locals1.pawn, workGiver))
				{
					try
					{
						JobGiver_Work.<>c__DisplayClass3_1 CS$<>8__locals2 = new JobGiver_Work.<>c__DisplayClass3_1();
						CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
						Job job2 = workGiver.NonScanJob(CS$<>8__locals2.CS$<>8__locals1.pawn);
						if (job2 != null)
						{
							return new ThinkResult(job2, this, new JobTag?(list[j].def.tagToGive), false);
						}
						CS$<>8__locals2.scanner = (workGiver as WorkGiver_Scanner);
						if (CS$<>8__locals2.scanner != null)
						{
							if (CS$<>8__locals2.scanner.def.scanThings)
							{
								Predicate<Thing> validator = (Thing t) => !t.IsForbidden(CS$<>8__locals2.CS$<>8__locals1.pawn) && CS$<>8__locals2.scanner.HasJobOnThing(CS$<>8__locals2.CS$<>8__locals1.pawn, t, false);
								IEnumerable<Thing> enumerable = CS$<>8__locals2.scanner.PotentialWorkThingsGlobal(CS$<>8__locals2.CS$<>8__locals1.pawn);
								Thing thing;
								if (CS$<>8__locals2.scanner.Prioritized)
								{
									IEnumerable<Thing> enumerable2 = enumerable;
									if (enumerable2 == null)
									{
										enumerable2 = CS$<>8__locals2.CS$<>8__locals1.pawn.Map.listerThings.ThingsMatching(CS$<>8__locals2.scanner.PotentialWorkThingRequest);
									}
									if (CS$<>8__locals2.scanner.AllowUnreachable)
									{
										thing = GenClosest.ClosestThing_Global(CS$<>8__locals2.CS$<>8__locals1.pawn.Position, enumerable2, 99999f, validator, (Thing x) => CS$<>8__locals2.scanner.GetPriority(CS$<>8__locals2.CS$<>8__locals1.pawn, x));
									}
									else
									{
										thing = GenClosest.ClosestThing_Global_Reachable(CS$<>8__locals2.CS$<>8__locals1.pawn.Position, CS$<>8__locals2.CS$<>8__locals1.pawn.Map, enumerable2, CS$<>8__locals2.scanner.PathEndMode, TraverseParms.For(CS$<>8__locals2.CS$<>8__locals1.pawn, CS$<>8__locals2.scanner.MaxPathDanger(CS$<>8__locals2.CS$<>8__locals1.pawn), TraverseMode.ByPawn, false, false, false), 9999f, validator, (Thing x) => CS$<>8__locals2.scanner.GetPriority(CS$<>8__locals2.CS$<>8__locals1.pawn, x));
									}
								}
								else if (CS$<>8__locals2.scanner.AllowUnreachable)
								{
									IEnumerable<Thing> enumerable3 = enumerable;
									if (enumerable3 == null)
									{
										enumerable3 = CS$<>8__locals2.CS$<>8__locals1.pawn.Map.listerThings.ThingsMatching(CS$<>8__locals2.scanner.PotentialWorkThingRequest);
									}
									thing = GenClosest.ClosestThing_Global(CS$<>8__locals2.CS$<>8__locals1.pawn.Position, enumerable3, 99999f, validator, null);
								}
								else
								{
									thing = GenClosest.ClosestThingReachable(CS$<>8__locals2.CS$<>8__locals1.pawn.Position, CS$<>8__locals2.CS$<>8__locals1.pawn.Map, CS$<>8__locals2.scanner.PotentialWorkThingRequest, CS$<>8__locals2.scanner.PathEndMode, TraverseParms.For(CS$<>8__locals2.CS$<>8__locals1.pawn, CS$<>8__locals2.scanner.MaxPathDanger(CS$<>8__locals2.CS$<>8__locals1.pawn), TraverseMode.ByPawn, false, false, false), 9999f, validator, enumerable, 0, CS$<>8__locals2.scanner.MaxRegionsToScanBeforeGlobalSearch, enumerable != null, RegionType.Set_Passable, false);
								}
								if (thing != null)
								{
									CS$<>8__locals2.CS$<>8__locals1.bestTargetOfLastPriority = thing;
									CS$<>8__locals2.CS$<>8__locals1.scannerWhoProvidedTarget = CS$<>8__locals2.scanner;
								}
							}
							if (CS$<>8__locals2.scanner.def.scanCells)
							{
								JobGiver_Work.<>c__DisplayClass3_2 CS$<>8__locals3;
								CS$<>8__locals3.pawnPosition = CS$<>8__locals2.CS$<>8__locals1.pawn.Position;
								CS$<>8__locals3.closestDistSquared = 99999f;
								CS$<>8__locals3.bestPriority = float.MinValue;
								CS$<>8__locals3.prioritized = CS$<>8__locals2.scanner.Prioritized;
								CS$<>8__locals3.allowUnreachable = CS$<>8__locals2.scanner.AllowUnreachable;
								CS$<>8__locals3.maxPathDanger = CS$<>8__locals2.scanner.MaxPathDanger(CS$<>8__locals2.CS$<>8__locals1.pawn);
								IEnumerable<IntVec3> enumerable4 = CS$<>8__locals2.scanner.PotentialWorkCellsGlobal(CS$<>8__locals2.CS$<>8__locals1.pawn);
								IList<IntVec3> list2;
								if ((list2 = (enumerable4 as IList<IntVec3>)) != null)
								{
									for (int k = 0; k < list2.Count; k++)
									{
										CS$<>8__locals2.<TryIssueJobPackage>g__ProcessCell|3(list2[k], ref CS$<>8__locals3);
									}
								}
								else
								{
									foreach (IntVec3 c in enumerable4)
									{
										CS$<>8__locals2.<TryIssueJobPackage>g__ProcessCell|3(c, ref CS$<>8__locals3);
									}
								}
							}
						}
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							CS$<>8__locals1.pawn,
							" threw exception in WorkGiver ",
							workGiver.def.defName,
							": ",
							ex.ToString()
						}));
					}
					finally
					{
					}
					if (CS$<>8__locals1.bestTargetOfLastPriority.IsValid)
					{
						Job job3;
						if (CS$<>8__locals1.bestTargetOfLastPriority.HasThing)
						{
							job3 = CS$<>8__locals1.scannerWhoProvidedTarget.JobOnThing(CS$<>8__locals1.pawn, CS$<>8__locals1.bestTargetOfLastPriority.Thing, false);
						}
						else
						{
							job3 = CS$<>8__locals1.scannerWhoProvidedTarget.JobOnCell(CS$<>8__locals1.pawn, CS$<>8__locals1.bestTargetOfLastPriority.Cell, false);
						}
						if (job3 != null)
						{
							job3.workGiverDef = CS$<>8__locals1.scannerWhoProvidedTarget.def;
							return new ThinkResult(job3, this, new JobTag?(list[j].def.tagToGive), false);
						}
						Log.ErrorOnce(string.Concat(new object[]
						{
							CS$<>8__locals1.scannerWhoProvidedTarget,
							" provided target ",
							CS$<>8__locals1.bestTargetOfLastPriority,
							" but yielded no actual job for pawn ",
							CS$<>8__locals1.pawn,
							". The CanGiveJob and JobOnX methods may not be synchronized."
						}), 6112651);
					}
					num = workGiver.def.priorityInType;
				}
			}
			return ThinkResult.NoJob;
		}

		// Token: 0x060036A2 RID: 13986 RVA: 0x00136060 File Offset: 0x00134260
		private bool PawnCanUseWorkGiver(Pawn pawn, WorkGiver giver)
		{
			return (giver.def.nonColonistsCanDo || pawn.IsColonist) && !pawn.WorkTagIsDisabled(giver.def.workTags) && !giver.ShouldSkip(pawn, false) && giver.MissingRequiredCapacity(pawn) == null;
		}

		// Token: 0x060036A3 RID: 13987 RVA: 0x001360B1 File Offset: 0x001342B1
		private bool WorkGiversRelated(WorkGiverDef current, WorkGiverDef next)
		{
			return (next != WorkGiverDefOf.Repair || current == WorkGiverDefOf.Repair) && next.doesSmoothing == current.doesSmoothing;
		}

		// Token: 0x060036A4 RID: 13988 RVA: 0x001360D4 File Offset: 0x001342D4
		private Job GiverTryGiveJobPrioritized(Pawn pawn, WorkGiver giver, IntVec3 cell)
		{
			if (!this.PawnCanUseWorkGiver(pawn, giver))
			{
				return null;
			}
			try
			{
				Job job = giver.NonScanJob(pawn);
				if (job != null)
				{
					return job;
				}
				WorkGiver_Scanner scanner = giver as WorkGiver_Scanner;
				if (scanner != null)
				{
					if (giver.def.scanThings)
					{
						Predicate<Thing> predicate = (Thing t) => !t.IsForbidden(pawn) && scanner.HasJobOnThing(pawn, t, false);
						List<Thing> thingList = cell.GetThingList(pawn.Map);
						for (int i = 0; i < thingList.Count; i++)
						{
							Thing thing = thingList[i];
							if (scanner.PotentialWorkThingRequest.Accepts(thing) && predicate(thing))
							{
								Job job2 = scanner.JobOnThing(pawn, thing, false);
								if (job2 != null)
								{
									job2.workGiverDef = giver.def;
								}
								return job2;
							}
						}
					}
					if (giver.def.scanCells && !cell.IsForbidden(pawn) && scanner.HasJobOnCell(pawn, cell, false))
					{
						Job job3 = scanner.JobOnCell(pawn, cell, false);
						if (job3 != null)
						{
							job3.workGiverDef = giver.def;
						}
						return job3;
					}
				}
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					pawn,
					" threw exception in GiverTryGiveJobTargeted on WorkGiver ",
					giver.def.defName,
					": ",
					ex.ToString()
				}));
			}
			return null;
		}

		// Token: 0x04001EFB RID: 7931
		public bool emergency;
	}
}
