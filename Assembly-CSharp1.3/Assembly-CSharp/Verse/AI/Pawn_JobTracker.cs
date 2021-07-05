using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.AI.Group;

namespace Verse.AI
{
	// Token: 0x0200060C RID: 1548
	public class Pawn_JobTracker : IExposable
	{
		// Token: 0x17000879 RID: 2169
		// (get) Token: 0x06002C8F RID: 11407 RVA: 0x0010A323 File Offset: 0x00108523
		public bool HandlingFacing
		{
			get
			{
				return this.curDriver != null && this.curDriver.HandlingFacing;
			}
		}

		// Token: 0x06002C90 RID: 11408 RVA: 0x0010A33C File Offset: 0x0010853C
		public Pawn_JobTracker(Pawn newPawn)
		{
			this.pawn = newPawn;
		}

		// Token: 0x06002C91 RID: 11409 RVA: 0x0010A398 File Offset: 0x00108598
		public virtual void ExposeData()
		{
			Scribe_Deep.Look<Job>(ref this.curJob, "curJob", Array.Empty<object>());
			Scribe_Deep.Look<JobDriver>(ref this.curDriver, "curDriver", Array.Empty<object>());
			Scribe_Deep.Look<JobQueue>(ref this.jobQueue, "jobQueue", Array.Empty<object>());
			Scribe_Values.Look<PawnPosture>(ref this.posture, "posture", PawnPosture.Standing, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				if (this.curDriver != null)
				{
					this.curDriver.pawn = this.pawn;
					this.curDriver.job = this.curJob;
					return;
				}
			}
			else if (Scribe.mode == LoadSaveMode.PostLoadInit && this.curDriver == null && this.curJob != null)
			{
				Log.Warning(string.Format("Cleaning up invalid job state on {0}", this.pawn));
				this.EndCurrentJob(JobCondition.Errored, true, true);
			}
		}

		// Token: 0x06002C92 RID: 11410 RVA: 0x0010A460 File Offset: 0x00108660
		public void Notify_WorkTypeDisabled(WorkTypeDef wType)
		{
			bool flag = this.pawn.WorkTypeIsDisabled(wType);
			try
			{
				if (this.curJob != null && this.curJob.workGiverDef != null && this.curJob.workGiverDef.workType == wType && (flag || !this.curJob.playerForced))
				{
					Pawn_JobTracker.tmpJobsToDequeue.Add(this.curJob);
				}
				foreach (QueuedJob queuedJob in this.jobQueue)
				{
					if (queuedJob.job.workGiverDef != null && queuedJob.job.workGiverDef.workType == wType && (flag || !queuedJob.job.playerForced))
					{
						Pawn_JobTracker.tmpJobsToDequeue.Add(queuedJob.job);
					}
				}
				foreach (Job job in Pawn_JobTracker.tmpJobsToDequeue)
				{
					this.EndCurrentOrQueuedJob(job, JobCondition.InterruptForced, true);
				}
			}
			finally
			{
				Pawn_JobTracker.tmpJobsToDequeue.Clear();
			}
		}

		// Token: 0x06002C93 RID: 11411 RVA: 0x0010A598 File Offset: 0x00108798
		public void Notify_JoyKindDisabled(JoyKindDef joyKind)
		{
			try
			{
				if (this.curJob != null && this.curJob.def.joyKind == joyKind)
				{
					Pawn_JobTracker.tmpJobsToDequeue.Add(this.curJob);
				}
				foreach (QueuedJob queuedJob in this.jobQueue)
				{
					if (queuedJob.job.def.joyKind == joyKind)
					{
						Pawn_JobTracker.tmpJobsToDequeue.Add(queuedJob.job);
					}
				}
				foreach (Job job in Pawn_JobTracker.tmpJobsToDequeue)
				{
					this.EndCurrentOrQueuedJob(job, JobCondition.InterruptForced, true);
				}
			}
			finally
			{
				Pawn_JobTracker.tmpJobsToDequeue.Clear();
			}
		}

		// Token: 0x06002C94 RID: 11412 RVA: 0x0010A688 File Offset: 0x00108888
		public virtual void JobTrackerTick()
		{
			this.jobsGivenThisTick = 0;
			this.jobsGivenThisTickTextual = "";
			if (this.pawn.IsHashIntervalTick(30))
			{
				ThinkResult thinkResult = this.DetermineNextConstantThinkTreeJob();
				if (thinkResult.IsValid)
				{
					if (this.ShouldStartJobFromThinkTree(thinkResult))
					{
						this.CheckLeaveJoinableLordBecauseJobIssued(thinkResult);
						this.StartJob(thinkResult.Job, JobCondition.InterruptForced, thinkResult.SourceNode, false, false, this.pawn.thinker.ConstantThinkTree, thinkResult.Tag, false, false);
					}
					else if (thinkResult.Job != this.curJob && !this.jobQueue.Contains(thinkResult.Job))
					{
						JobMaker.ReturnToPool(thinkResult.Job);
					}
				}
			}
			if (this.curDriver != null)
			{
				if (this.curJob.expiryInterval > 0 && (Find.TickManager.TicksGame - this.curJob.startTick) % this.curJob.expiryInterval == 0 && Find.TickManager.TicksGame != this.curJob.startTick)
				{
					if (!this.curJob.expireRequiresEnemiesNearby || PawnUtility.EnemiesAreNearby(this.pawn, 25, false))
					{
						if (this.debugLog)
						{
							this.DebugLogEvent("Job expire");
						}
						if (!this.curJob.checkOverrideOnExpire)
						{
							this.EndCurrentJob(JobCondition.Succeeded, true, true);
						}
						else
						{
							this.CheckForJobOverride();
						}
						this.FinalizeTick();
						return;
					}
					if (this.debugLog)
					{
						this.DebugLogEvent("Job expire skipped because there are no enemies nearby");
					}
				}
				this.curDriver.DriverTick();
			}
			if (this.curJob == null && !this.pawn.Dead && this.pawn.mindState.Active && this.CanDoAnyJob())
			{
				if (this.debugLog)
				{
					this.DebugLogEvent("Starting job from Tick because curJob == null.");
				}
				this.TryFindAndStartJob();
			}
			this.FinalizeTick();
		}

		// Token: 0x06002C95 RID: 11413 RVA: 0x0010A858 File Offset: 0x00108A58
		private void FinalizeTick()
		{
			this.jobsGivenRecentTicks.Add(this.jobsGivenThisTick);
			this.jobsGivenRecentTicksTextual.Add(this.jobsGivenThisTickTextual);
			while (this.jobsGivenRecentTicks.Count > 10)
			{
				this.jobsGivenRecentTicks.RemoveAt(0);
				this.jobsGivenRecentTicksTextual.RemoveAt(0);
			}
			if (this.jobsGivenThisTick != 0)
			{
				int num = 0;
				for (int i = 0; i < this.jobsGivenRecentTicks.Count; i++)
				{
					num += this.jobsGivenRecentTicks[i];
				}
				if (num >= 10)
				{
					string text = this.jobsGivenRecentTicksTextual.ToCommaList(false, false);
					this.jobsGivenRecentTicks.Clear();
					this.jobsGivenRecentTicksTextual.Clear();
					JobUtility.TryStartErrorRecoverJob(this.pawn, string.Concat(new object[]
					{
						this.pawn.ToStringSafe<Pawn>(),
						" started ",
						10,
						" jobs in ",
						10,
						" ticks. List: ",
						text
					}), null, null);
				}
			}
		}

		// Token: 0x06002C96 RID: 11414 RVA: 0x0010A961 File Offset: 0x00108B61
		public IEnumerable<Job> AllJobs()
		{
			if (this.curJob != null)
			{
				yield return this.curJob;
			}
			foreach (QueuedJob queuedJob in this.jobQueue)
			{
				yield return queuedJob.job;
			}
			IEnumerator<QueuedJob> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06002C97 RID: 11415 RVA: 0x0010A974 File Offset: 0x00108B74
		public void StartJob(Job newJob, JobCondition lastJobEndCondition = JobCondition.None, ThinkNode jobGiver = null, bool resumeCurJobAfterwards = false, bool cancelBusyStances = true, ThinkTreeDef thinkTree = null, JobTag? tag = null, bool fromQueue = false, bool canReturnCurJobToPool = false)
		{
			this.startingNewJob = true;
			try
			{
				if (!fromQueue && (!Find.TickManager.Paused || this.lastJobGivenAtFrame == RealTime.frameCount))
				{
					this.jobsGivenThisTick++;
					if (Prefs.DevMode)
					{
						this.jobsGivenThisTickTextual = this.jobsGivenThisTickTextual + "(" + newJob.ToString() + ") ";
					}
				}
				this.lastJobGivenAtFrame = RealTime.frameCount;
				if (this.jobsGivenThisTick > 10)
				{
					string text = this.jobsGivenThisTickTextual;
					this.jobsGivenThisTick = 0;
					this.jobsGivenThisTickTextual = "";
					this.startingNewJob = false;
					this.pawn.ClearReservationsForJob(newJob);
					JobUtility.TryStartErrorRecoverJob(this.pawn, string.Concat(new string[]
					{
						this.pawn.ToStringSafe<Pawn>(),
						" started 10 jobs in one tick. newJob=",
						newJob.ToStringSafe<Job>(),
						" jobGiver=",
						jobGiver.ToStringSafe<ThinkNode>(),
						" jobList=",
						text
					}), null, null);
				}
				else
				{
					if (this.debugLog)
					{
						this.DebugLogEvent(string.Concat(new object[]
						{
							"StartJob [",
							newJob,
							"] lastJobEndCondition=",
							lastJobEndCondition,
							", jobGiver=",
							jobGiver,
							", cancelBusyStances=",
							cancelBusyStances.ToString()
						}));
					}
					if (cancelBusyStances && this.pawn.stances.FullBodyBusy)
					{
						this.pawn.stances.CancelBusyStanceHard();
					}
					if (this.curJob != null)
					{
						if (lastJobEndCondition == JobCondition.None)
						{
							Log.Warning(string.Concat(new object[]
							{
								this.pawn,
								" starting job ",
								newJob,
								" from JobGiver ",
								newJob.jobGiver,
								" while already having job ",
								this.curJob,
								" without a specific job end condition."
							}));
							lastJobEndCondition = JobCondition.InterruptForced;
						}
						if (resumeCurJobAfterwards && this.curJob.def.suspendable)
						{
							this.jobQueue.EnqueueFirst(this.curJob, null);
							if (this.debugLog)
							{
								this.DebugLogEvent("   JobQueue EnqueueFirst curJob: " + this.curJob);
							}
							this.CleanupCurrentJob(lastJobEndCondition, false, cancelBusyStances, false);
						}
						else
						{
							this.CleanupCurrentJob(lastJobEndCondition, true, cancelBusyStances, canReturnCurJobToPool);
						}
					}
					if (newJob == null)
					{
						Log.Warning(this.pawn + " tried to start doing a null job.");
					}
					else
					{
						newJob.startTick = Find.TickManager.TicksGame;
						if (this.pawn.Drafted || newJob.playerForced)
						{
							newJob.ignoreForbidden = true;
							newJob.ignoreDesignations = true;
						}
						this.curJob = newJob;
						this.curJob.jobGiverThinkTree = thinkTree;
						this.curJob.jobGiver = jobGiver;
						this.curDriver = this.curJob.MakeDriver(this.pawn);
						if (this.curDriver.TryMakePreToilReservations(!fromQueue))
						{
							Job job = this.TryOpportunisticJob(newJob);
							if (job != null)
							{
								this.jobQueue.EnqueueFirst(newJob, null);
								this.curJob = null;
								this.curDriver = null;
								this.StartJob(job, JobCondition.None, null, false, true, null, null, false, false);
							}
							else
							{
								if (tag != null)
								{
									JobTag? jobTag = tag;
									JobTag jobTag2 = JobTag.Fieldwork;
									if (jobTag.GetValueOrDefault() == jobTag2 & jobTag != null)
									{
										JobTag lastJobTag = this.pawn.mindState.lastJobTag;
										jobTag = tag;
										if (!(lastJobTag == jobTag.GetValueOrDefault() & jobTag != null))
										{
											foreach (Pawn pawn in PawnUtility.SpawnedMasteredPawns(this.pawn))
											{
												pawn.jobs.Notify_MasterStartedFieldWork();
											}
										}
									}
									this.pawn.mindState.lastJobTag = tag.Value;
								}
								if (!this.pawn.Destroyed && this.pawn.ShouldDropCarriedThingBeforeJob(this.curJob))
								{
									if (DebugViewSettings.logCarriedBetweenJobs)
									{
										Log.Message(string.Format("Dropping {0} before starting job {1}", this.pawn.carryTracker.CarriedThing, newJob));
									}
									Thing thing;
									this.pawn.carryTracker.TryDropCarriedThing(this.pawn.Position, ThingPlaceMode.Near, out thing, null);
								}
								this.curDriver.SetInitialPosture();
								this.curDriver.Notify_Starting();
								this.curDriver.SetupToils();
								this.curDriver.ReadyForNextToil();
							}
						}
						else if (fromQueue)
						{
							this.EndCurrentJob(JobCondition.QueuedNoLongerValid, true, true);
						}
						else
						{
							Log.Warning("TryMakePreToilReservations() returned false for a non-queued job right after StartJob(). This should have been checked before. curJob=" + this.curJob.ToStringSafe<Job>());
							this.EndCurrentJob(JobCondition.Errored, true, true);
						}
					}
				}
			}
			finally
			{
				this.startingNewJob = false;
			}
		}

		// Token: 0x06002C98 RID: 11416 RVA: 0x0010AE48 File Offset: 0x00109048
		public void EndCurrentOrQueuedJob(Job job, JobCondition condition, bool canReturnToPool = true)
		{
			if (this.debugLog)
			{
				this.DebugLogEvent(string.Concat(new object[]
				{
					"EndJob [",
					job,
					"] condition=",
					condition
				}));
			}
			QueuedJob queuedJob = this.jobQueue.Extract(job);
			if (queuedJob != null)
			{
				this.pawn.ClearReservationsForJob(queuedJob.job);
				if (canReturnToPool)
				{
					JobMaker.ReturnToPool(queuedJob.job);
				}
			}
			if (this.curJob == job)
			{
				this.EndCurrentJob(condition, true, canReturnToPool);
			}
		}

		// Token: 0x06002C99 RID: 11417 RVA: 0x0010AECC File Offset: 0x001090CC
		public void EndCurrentJob(JobCondition condition, bool startNewJob = true, bool canReturnToPool = true)
		{
			if (this.debugLog)
			{
				this.DebugLogEvent(string.Concat(new object[]
				{
					"EndCurrentJob ",
					(this.curJob != null) ? this.curJob.ToString() : "null",
					" condition=",
					condition,
					" curToil=",
					(this.curDriver != null) ? this.curDriver.CurToilIndex.ToString() : "null_driver"
				}));
			}
			if (condition == JobCondition.Ongoing)
			{
				Log.Warning("Ending a job with Ongoing as the condition. This makes no sense.");
			}
			if (condition == JobCondition.Succeeded && this.curJob != null && this.curJob.def.taleOnCompletion != null)
			{
				TaleRecorder.RecordTale(this.curJob.def.taleOnCompletion, this.curDriver.TaleParameters());
			}
			JobDef jobDef = (this.curJob != null) ? this.curJob.def : null;
			this.CleanupCurrentJob(condition, true, true, canReturnToPool);
			if (startNewJob)
			{
				if (condition == JobCondition.ErroredPather || condition == JobCondition.Errored)
				{
					this.StartJob(JobMaker.MakeJob(JobDefOf.Wait, 250, false), JobCondition.None, null, false, true, null, null, false, false);
					return;
				}
				if (condition == JobCondition.Succeeded && jobDef != null && jobDef != JobDefOf.Wait_MaintainPosture && !this.pawn.pather.Moving)
				{
					this.StartJob(JobMaker.MakeJob(JobDefOf.Wait_MaintainPosture, 1, false), JobCondition.None, null, false, false, null, null, false, false);
					return;
				}
				this.TryFindAndStartJob();
			}
		}

		// Token: 0x06002C9A RID: 11418 RVA: 0x0010B040 File Offset: 0x00109240
		private void CleanupCurrentJob(JobCondition condition, bool releaseReservations, bool cancelBusyStancesSoft = true, bool canReturnToPool = false)
		{
			if (this.debugLog)
			{
				this.DebugLogEvent(string.Concat(new object[]
				{
					"CleanupCurrentJob ",
					(this.curJob != null) ? this.curJob.def.ToString() : "null",
					" condition ",
					condition
				}));
			}
			if (this.curJob == null)
			{
				return;
			}
			if (releaseReservations)
			{
				this.pawn.ClearReservationsForJob(this.curJob);
			}
			if (this.curDriver != null)
			{
				this.curDriver.ended = true;
				this.curDriver.Cleanup(condition);
			}
			this.curDriver = null;
			Job job = this.curJob;
			this.curJob = null;
			this.pawn.VerifyReservations();
			if (cancelBusyStancesSoft)
			{
				this.pawn.stances.CancelBusyStanceSoft();
			}
			if (!this.pawn.Destroyed && this.pawn.ShouldDropCarriedThingAfterJob(job))
			{
				if (DebugViewSettings.logCarriedBetweenJobs)
				{
					Log.Message(string.Format("Dropping {0} after finishing job {1}", this.pawn.carryTracker.CarriedThing, job));
				}
				Thing thing;
				this.pawn.carryTracker.TryDropCarriedThing(this.pawn.Position, ThingPlaceMode.Near, out thing, null);
			}
			if (releaseReservations && canReturnToPool)
			{
				JobMaker.ReturnToPool(job);
			}
		}

		// Token: 0x06002C9B RID: 11419 RVA: 0x0010B17F File Offset: 0x0010937F
		public JobQueue CaptureAndClearJobQueue()
		{
			JobQueue result = this.jobQueue.Capture();
			this.ClearQueuedJobs(false);
			return result;
		}

		// Token: 0x06002C9C RID: 11420 RVA: 0x0010B194 File Offset: 0x00109394
		public void RestoreCapturedJobs(JobQueue incomming, bool canReturnToPool = true)
		{
			bool flag = false;
			QueuedJob queuedJob;
			while ((queuedJob = incomming.Dequeue()) != null)
			{
				if (flag)
				{
					if (canReturnToPool)
					{
						JobMaker.ReturnToPool(queuedJob.job);
					}
				}
				else if (!this.pawn.jobs.TryTakeOrderedJob(queuedJob.job, queuedJob.tag, true))
				{
					flag = true;
				}
			}
		}

		// Token: 0x06002C9D RID: 11421 RVA: 0x0010B1E4 File Offset: 0x001093E4
		public void ClearQueuedJobs(bool canReturnToPool = true)
		{
			if (this.debugLog)
			{
				this.DebugLogEvent("ClearQueuedJobs");
			}
			while (this.jobQueue.Count > 0)
			{
				Job job = this.jobQueue.Dequeue().job;
				this.pawn.ClearReservationsForJob(job);
				if (canReturnToPool)
				{
					JobMaker.ReturnToPool(job);
				}
			}
		}

		// Token: 0x06002C9E RID: 11422 RVA: 0x0010B23C File Offset: 0x0010943C
		public void CheckForJobOverride()
		{
			if (this.debugLog)
			{
				this.DebugLogEvent("CheckForJobOverride");
			}
			ThinkTreeDef thinkTree;
			ThinkResult thinkResult = this.DetermineNextJob(out thinkTree);
			if (thinkResult.IsValid)
			{
				if (this.ShouldStartJobFromThinkTree(thinkResult))
				{
					this.CheckLeaveJoinableLordBecauseJobIssued(thinkResult);
					this.StartJob(thinkResult.Job, JobCondition.InterruptOptional, thinkResult.SourceNode, false, false, thinkTree, thinkResult.Tag, thinkResult.FromQueue, false);
					return;
				}
				if (thinkResult.Job != this.curJob && !this.jobQueue.Contains(thinkResult.Job))
				{
					JobMaker.ReturnToPool(thinkResult.Job);
				}
			}
		}

		// Token: 0x06002C9F RID: 11423 RVA: 0x0010B2D8 File Offset: 0x001094D8
		public void StopAll(bool ifLayingKeepLaying = false, bool canReturnToPool = true)
		{
			if ((!this.pawn.InBed() && (this.pawn.CurJob == null || this.pawn.CurJob.def != JobDefOf.LayDown || !this.pawn.GetPosture().Laying())) || !ifLayingKeepLaying)
			{
				this.CleanupCurrentJob(JobCondition.InterruptForced, true, true, canReturnToPool);
			}
			this.ClearQueuedJobs(canReturnToPool);
		}

		// Token: 0x06002CA0 RID: 11424 RVA: 0x0010B344 File Offset: 0x00109544
		private void TryFindAndStartJob()
		{
			if (this.pawn.thinker == null)
			{
				Log.ErrorOnce(this.pawn + " did TryFindAndStartJob but had no thinker.", 8573261);
				return;
			}
			if (this.curJob != null)
			{
				Log.Warning(this.pawn + " doing TryFindAndStartJob while still having job " + this.curJob);
			}
			if (this.debugLog)
			{
				this.DebugLogEvent("TryFindAndStartJob");
			}
			if (!this.CanDoAnyJob())
			{
				if (this.debugLog)
				{
					this.DebugLogEvent("   CanDoAnyJob is false. Clearing queue and returning");
				}
				this.ClearQueuedJobs(true);
				return;
			}
			ThinkTreeDef thinkTree;
			ThinkResult result = this.DetermineNextJob(out thinkTree);
			if (result.IsValid)
			{
				this.CheckLeaveJoinableLordBecauseJobIssued(result);
				this.StartJob(result.Job, JobCondition.None, result.SourceNode, false, false, thinkTree, result.Tag, result.FromQueue, false);
			}
		}

		// Token: 0x06002CA1 RID: 11425 RVA: 0x0010B414 File Offset: 0x00109614
		public Job TryOpportunisticJob(Job job)
		{
			if (this.pawn.def.race.intelligence < Intelligence.Humanlike)
			{
				return null;
			}
			if (this.pawn.Faction != Faction.OfPlayer)
			{
				return null;
			}
			if (this.pawn.Drafted)
			{
				return null;
			}
			if (job.playerForced)
			{
				return null;
			}
			if (this.pawn.RaceProps.intelligence < Intelligence.Humanlike)
			{
				return null;
			}
			if (!job.def.allowOpportunisticPrefix)
			{
				return null;
			}
			if (this.pawn.WorkTagIsDisabled(WorkTags.ManualDumb | WorkTags.Hauling | WorkTags.AllWork))
			{
				return null;
			}
			if (this.pawn.InMentalState || this.pawn.IsBurning())
			{
				return null;
			}
			IntVec3 cell = job.targetA.Cell;
			if (!cell.IsValid || cell.IsForbidden(this.pawn))
			{
				return null;
			}
			float num = this.pawn.Position.DistanceTo(cell);
			if (num < 3f)
			{
				return null;
			}
			List<Thing> list = this.pawn.Map.listerHaulables.ThingsPotentiallyNeedingHauling();
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				float num2 = this.pawn.Position.DistanceTo(thing.Position);
				if (num2 <= 30f && num2 <= num * 0.5f && num2 + thing.Position.DistanceTo(cell) <= num * 1.7f && this.pawn.Map.reservationManager.FirstRespectedReserver(thing, this.pawn) == null && !thing.IsForbidden(this.pawn) && HaulAIUtility.PawnCanAutomaticallyHaulFast(this.pawn, thing, false))
				{
					StoragePriority currentPriority = StoreUtility.CurrentStoragePriorityOf(thing);
					IntVec3 invalid = IntVec3.Invalid;
					if (StoreUtility.TryFindBestBetterStoreCellFor(thing, this.pawn, this.pawn.Map, currentPriority, this.pawn.Faction, out invalid, true))
					{
						float num3 = invalid.DistanceTo(cell);
						if (num3 <= 50f && num3 <= num * 0.6f && num2 + thing.Position.DistanceTo(invalid) + num3 <= num * 1.7f && num2 + num3 <= num && this.pawn.Position.WithinRegions(thing.Position, this.pawn.Map, 25, TraverseParms.For(this.pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false), RegionType.Set_Passable) && invalid.WithinRegions(cell, this.pawn.Map, 25, TraverseParms.For(this.pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false), RegionType.Set_Passable))
						{
							if (DebugViewSettings.drawOpportunisticJobs)
							{
								Log.Message("Opportunistic job spawned");
								this.pawn.Map.debugDrawer.FlashLine(this.pawn.Position, thing.Position, 600, SimpleColor.Red);
								this.pawn.Map.debugDrawer.FlashLine(thing.Position, invalid, 600, SimpleColor.Green);
								this.pawn.Map.debugDrawer.FlashLine(invalid, cell, 600, SimpleColor.Blue);
							}
							return HaulAIUtility.HaulToCellStorageJob(this.pawn, thing, invalid, false);
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06002CA2 RID: 11426 RVA: 0x0010B754 File Offset: 0x00109954
		private ThinkResult DetermineNextJob(out ThinkTreeDef thinkTree)
		{
			ThinkResult result = this.DetermineNextConstantThinkTreeJob();
			if (result.Job != null)
			{
				thinkTree = this.pawn.thinker.ConstantThinkTree;
				return result;
			}
			ThinkResult result2 = ThinkResult.NoJob;
			try
			{
				result2 = this.pawn.thinker.MainThinkNodeRoot.TryIssueJobPackage(this.pawn, default(JobIssueParams));
			}
			catch (Exception exception)
			{
				JobUtility.TryStartErrorRecoverJob(this.pawn, this.pawn.ToStringSafe<Pawn>() + " threw exception while determining job (main)", exception, null);
				thinkTree = null;
				return ThinkResult.NoJob;
			}
			finally
			{
			}
			thinkTree = this.pawn.thinker.MainThinkTree;
			return result2;
		}

		// Token: 0x06002CA3 RID: 11427 RVA: 0x0010B814 File Offset: 0x00109A14
		private ThinkResult DetermineNextConstantThinkTreeJob()
		{
			if (this.pawn.thinker.ConstantThinkTree == null)
			{
				return ThinkResult.NoJob;
			}
			try
			{
				return this.pawn.thinker.ConstantThinkNodeRoot.TryIssueJobPackage(this.pawn, default(JobIssueParams));
			}
			catch (Exception exception)
			{
				JobUtility.TryStartErrorRecoverJob(this.pawn, this.pawn.ToStringSafe<Pawn>() + " threw exception while determining job (constant)", exception, null);
			}
			finally
			{
			}
			return ThinkResult.NoJob;
		}

		// Token: 0x06002CA4 RID: 11428 RVA: 0x0010B8AC File Offset: 0x00109AAC
		private void CheckLeaveJoinableLordBecauseJobIssued(ThinkResult result)
		{
			if (!result.IsValid || result.SourceNode == null)
			{
				return;
			}
			Lord lord = this.pawn.GetLord();
			if (lord == null || !(lord.LordJob is LordJob_VoluntarilyJoinable))
			{
				return;
			}
			bool flag = false;
			ThinkNode thinkNode = result.SourceNode;
			while (!thinkNode.leaveJoinableLordIfIssuesJob)
			{
				thinkNode = thinkNode.parent;
				if (thinkNode == null)
				{
					IL_50:
					if (flag)
					{
						lord.Notify_PawnLost(this.pawn, PawnLostCondition.LeftVoluntarily, null);
					}
					return;
				}
			}
			flag = true;
			goto IL_50;
		}

		// Token: 0x06002CA5 RID: 11429 RVA: 0x0010B922 File Offset: 0x00109B22
		private bool CanDoAnyJob()
		{
			return this.pawn.Spawned;
		}

		// Token: 0x06002CA6 RID: 11430 RVA: 0x0010B930 File Offset: 0x00109B30
		private bool ShouldStartJobFromThinkTree(ThinkResult thinkResult)
		{
			return this.curJob == null || (this.curJob != thinkResult.Job && (thinkResult.FromQueue || (thinkResult.Job.def != this.curJob.def || thinkResult.SourceNode != this.curJob.jobGiver || !this.curDriver.IsContinuation(thinkResult.Job))));
		}

		// Token: 0x06002CA7 RID: 11431 RVA: 0x0010B9A7 File Offset: 0x00109BA7
		public bool IsCurrentJobPlayerInterruptible()
		{
			return (this.curJob == null || this.curJob.def.playerInterruptible) && !this.pawn.HasAttachment(ThingDefOf.Fire);
		}

		// Token: 0x06002CA8 RID: 11432 RVA: 0x0010B9DC File Offset: 0x00109BDC
		public bool TryTakeOrderedJobPrioritizedWork(Job job, WorkGiver giver, IntVec3 cell)
		{
			if (this.TryTakeOrderedJob(job, new JobTag?(giver.def.tagToGive), false))
			{
				job.workGiverDef = giver.def;
				if (giver.def.prioritizeSustains)
				{
					this.pawn.mindState.priorityWork.Set(cell, giver.def);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06002CA9 RID: 11433 RVA: 0x0010BA3C File Offset: 0x00109C3C
		public bool TryTakeOrderedJob(Job job, JobTag? tag = 0, bool requestQueueing = false)
		{
			if (this.debugLog)
			{
				this.DebugLogEvent("TryTakeOrderedJob " + job);
			}
			job.playerForced = true;
			if (this.curJob != null && this.curJob.JobIsSameAs(job))
			{
				return true;
			}
			bool flag = this.pawn.jobs.IsCurrentJobPlayerInterruptible();
			bool flag2 = this.pawn.mindState.IsIdle || this.pawn.CurJob == null || this.pawn.CurJob.def.isIdle;
			bool flag3 = KeyBindingDefOf.QueueOrder.IsDownEvent;
			if (flag3)
			{
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.QueueOrders, KnowledgeAmount.NoteTaught);
			}
			flag3 = (flag3 || requestQueueing);
			if (flag && (!flag3 || flag2))
			{
				this.pawn.stances.CancelBusyStanceSoft();
				if (this.debugLog)
				{
					this.DebugLogEvent("    Queueing job");
				}
				this.ClearQueuedJobs(true);
				if (job.TryMakePreToilReservations(this.pawn, true))
				{
					this.jobQueue.EnqueueFirst(job, tag);
					if (this.curJob != null)
					{
						this.curDriver.EndJobWith(JobCondition.InterruptForced);
					}
					else
					{
						this.CheckForJobOverride();
					}
					return true;
				}
				Log.Warning("TryMakePreToilReservations() returned false right after TryTakeOrderedJob(). This should have been checked before. job=" + job.ToStringSafe<Job>());
				this.pawn.ClearReservationsForJob(job);
				return false;
			}
			else if (flag3)
			{
				if (job.TryMakePreToilReservations(this.pawn, true))
				{
					this.jobQueue.EnqueueLast(job, tag);
					return true;
				}
				Log.Warning("TryMakePreToilReservations() returned false right after TryTakeOrderedJob(). This should have been checked before. job=" + job.ToStringSafe<Job>());
				this.pawn.ClearReservationsForJob(job);
				return false;
			}
			else
			{
				this.ClearQueuedJobs(true);
				if (job.TryMakePreToilReservations(this.pawn, true))
				{
					this.jobQueue.EnqueueLast(job, tag);
					return true;
				}
				Log.Warning("TryMakePreToilReservations() returned false right after TryTakeOrderedJob(). This should have been checked before. job=" + job.ToStringSafe<Job>());
				this.pawn.ClearReservationsForJob(job);
				return false;
			}
		}

		// Token: 0x06002CAA RID: 11434 RVA: 0x0010BC08 File Offset: 0x00109E08
		public void Notify_TuckedIntoBed(Building_Bed bed)
		{
			this.pawn.Position = RestUtility.GetBedSleepingSlotPosFor(this.pawn, bed);
			this.pawn.Notify_Teleported(false, true);
			this.pawn.stances.CancelBusyStanceHard();
			this.StartJob(JobMaker.MakeJob(JobDefOf.LayDown, bed), JobCondition.InterruptForced, null, false, true, null, new JobTag?(JobTag.TuckedIntoBed), false, false);
		}

		// Token: 0x06002CAB RID: 11435 RVA: 0x0010BC70 File Offset: 0x00109E70
		public void Notify_DamageTaken(DamageInfo dinfo)
		{
			if (this.curJob == null)
			{
				return;
			}
			Job job = this.curJob;
			this.curDriver.Notify_DamageTaken(dinfo);
			if (this.curJob == job && dinfo.Def.ExternalViolenceFor(this.pawn) && dinfo.Def.canInterruptJobs && !this.curJob.playerForced && Find.TickManager.TicksGame >= this.lastDamageCheckTick + 180)
			{
				Thing instigator = dinfo.Instigator;
				if (this.curJob.def.checkOverrideOnDamage == CheckJobOverrideOnDamageMode.Always || (this.curJob.def.checkOverrideOnDamage == CheckJobOverrideOnDamageMode.OnlyIfInstigatorNotJobTarget && !this.curJob.AnyTargetIs(instigator)))
				{
					this.lastDamageCheckTick = Find.TickManager.TicksGame;
					this.CheckForJobOverride();
				}
			}
		}

		// Token: 0x06002CAC RID: 11436 RVA: 0x0010BD48 File Offset: 0x00109F48
		internal void Notify_MasterDraftedOrUndrafted()
		{
			Pawn master = this.pawn.playerSettings.Master;
			if (master.Spawned && master.Map == this.pawn.Map && this.pawn.playerSettings.followDrafted)
			{
				this.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
		}

		// Token: 0x06002CAD RID: 11437 RVA: 0x0010BD9C File Offset: 0x00109F9C
		public void Notify_MasterStartedFieldWork()
		{
			Pawn master = this.pawn.playerSettings.Master;
			if (master.Spawned && master.Map == this.pawn.Map && this.pawn.playerSettings.followFieldwork && this.IsCurrentJobPlayerInterruptible())
			{
				this.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
		}

		// Token: 0x06002CAE RID: 11438 RVA: 0x0010BDF8 File Offset: 0x00109FF8
		public void DrawLinesBetweenTargets()
		{
			Vector3 a = this.pawn.Position.ToVector3Shifted();
			if (this.pawn.pather.curPath != null)
			{
				a = this.pawn.pather.Destination.CenterVector3;
			}
			else if (this.curJob != null && this.curJob.targetA.IsValid && (!this.curJob.targetA.HasThing || (this.curJob.targetA.Thing.Spawned && this.curJob.targetA.Thing.Map == this.pawn.Map)))
			{
				GenDraw.DrawLineBetween(a, this.curJob.targetA.CenterVector3, AltitudeLayer.Item.AltitudeFor());
				a = this.curJob.targetA.CenterVector3;
			}
			for (int i = 0; i < this.jobQueue.Count; i++)
			{
				if (this.jobQueue[i].job.targetA.IsValid)
				{
					if (!this.jobQueue[i].job.targetA.HasThing || (this.jobQueue[i].job.targetA.Thing.Spawned && this.jobQueue[i].job.targetA.Thing.Map == this.pawn.Map))
					{
						Vector3 centerVector = this.jobQueue[i].job.targetA.CenterVector3;
						GenDraw.DrawLineBetween(a, centerVector, AltitudeLayer.Item.AltitudeFor());
						a = centerVector;
					}
				}
				else
				{
					List<LocalTargetInfo> targetQueueA = this.jobQueue[i].job.targetQueueA;
					if (targetQueueA != null)
					{
						for (int j = 0; j < targetQueueA.Count; j++)
						{
							if (!targetQueueA[j].HasThing || (targetQueueA[j].Thing.Spawned && targetQueueA[j].Thing.Map == this.pawn.Map))
							{
								Vector3 centerVector2 = targetQueueA[j].CenterVector3;
								GenDraw.DrawLineBetween(a, centerVector2, AltitudeLayer.Item.AltitudeFor());
								a = centerVector2;
							}
						}
					}
				}
			}
		}

		// Token: 0x06002CAF RID: 11439 RVA: 0x0010C06C File Offset: 0x0010A26C
		public void DebugLogEvent(string s)
		{
			if (this.debugLog)
			{
				Log.Message(string.Concat(new object[]
				{
					Find.TickManager.TicksGame,
					" ",
					this.pawn,
					": ",
					s
				}));
			}
		}

		// Token: 0x04001B43 RID: 6979
		protected Pawn pawn;

		// Token: 0x04001B44 RID: 6980
		public Job curJob;

		// Token: 0x04001B45 RID: 6981
		public JobDriver curDriver;

		// Token: 0x04001B46 RID: 6982
		public JobQueue jobQueue = new JobQueue();

		// Token: 0x04001B47 RID: 6983
		public PawnPosture posture;

		// Token: 0x04001B48 RID: 6984
		public bool startingNewJob;

		// Token: 0x04001B49 RID: 6985
		private int jobsGivenThisTick;

		// Token: 0x04001B4A RID: 6986
		private string jobsGivenThisTickTextual = "";

		// Token: 0x04001B4B RID: 6987
		private int lastJobGivenAtFrame = -1;

		// Token: 0x04001B4C RID: 6988
		private List<int> jobsGivenRecentTicks = new List<int>(10);

		// Token: 0x04001B4D RID: 6989
		private List<string> jobsGivenRecentTicksTextual = new List<string>(10);

		// Token: 0x04001B4E RID: 6990
		public bool debugLog;

		// Token: 0x04001B4F RID: 6991
		private const int RecentJobQueueMaxLength = 10;

		// Token: 0x04001B50 RID: 6992
		private const int MaxRecentJobs = 10;

		// Token: 0x04001B51 RID: 6993
		private static List<Job> tmpJobsToDequeue = new List<Job>();

		// Token: 0x04001B52 RID: 6994
		private int lastDamageCheckTick = -99999;

		// Token: 0x04001B53 RID: 6995
		private const int DamageCheckMinInterval = 180;
	}
}
