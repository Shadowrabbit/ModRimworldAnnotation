using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.AI.Group;

namespace Verse.AI
{
    // Token: 0x02000A62 RID: 2658
    public class Pawn_JobTracker : IExposable
    {
        // Token: 0x170009DF RID: 2527
        // (get) Token: 0x06003F3F RID: 16191 RVA: 0x0002F781 File Offset: 0x0002D981
        public bool HandlingFacing
        {
            get
            {
                return this.curDriver != null && this.curDriver.HandlingFacing;
            }
        }

        // Token: 0x06003F40 RID: 16192 RVA: 0x0017D094 File Offset: 0x0017B294
        public Pawn_JobTracker(Pawn newPawn)
        {
            this.pawn = newPawn;
        }

        // Token: 0x06003F41 RID: 16193 RVA: 0x0017D0F0 File Offset: 0x0017B2F0
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
                Log.Warning(string.Format("Cleaning up invalid job state on {0}", this.pawn), false);
                this.EndCurrentJob(JobCondition.Errored, true, true);
            }
        }

        // Token: 0x06003F42 RID: 16194 RVA: 0x0017D1B8 File Offset: 0x0017B3B8
        public void Notify_WorkTypeDisabled(WorkTypeDef wType)
        {
            bool flag = this.pawn.WorkTypeIsDisabled(wType);
            try
            {
                if (this.curJob != null && this.curJob.workGiverDef != null && this.curJob.workGiverDef.workType == wType &&
                    (flag || !this.curJob.playerForced))
                {
                    Pawn_JobTracker.tmpJobsToDequeue.Add(this.curJob);
                }
                foreach (QueuedJob queuedJob in this.jobQueue)
                {
                    if (queuedJob.job.workGiverDef != null && queuedJob.job.workGiverDef.workType == wType &&
                        (flag || !queuedJob.job.playerForced))
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

        // Token: 0x06003F43 RID: 16195 RVA: 0x0017D2F0 File Offset: 0x0017B4F0
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

        // Token: 0x06003F44 RID: 16196 RVA: 0x0017D3E0 File Offset: 0x0017B5E0
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
                        this.StartJob(thinkResult.Job, JobCondition.InterruptForced, thinkResult.SourceNode, false, false,
                            this.pawn.thinker.ConstantThinkTree, thinkResult.Tag, false, false);
                    }
                    else if (thinkResult.Job != this.curJob && !this.jobQueue.Contains(thinkResult.Job))
                    {
                        JobMaker.ReturnToPool(thinkResult.Job);
                    }
                }
            }
            if (this.curDriver != null)
            {
                if (this.curJob.expiryInterval > 0 &&
                    (Find.TickManager.TicksGame - this.curJob.startTick) % this.curJob.expiryInterval == 0 &&
                    Find.TickManager.TicksGame != this.curJob.startTick)
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

        // Token: 0x06003F45 RID: 16197 RVA: 0x0017D5B0 File Offset: 0x0017B7B0
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
                    string text = this.jobsGivenRecentTicksTextual.ToCommaList(false);
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

        // Token: 0x06003F46 RID: 16198 RVA: 0x0002F798 File Offset: 0x0002D998
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

        // Token: 0x06003F47 RID: 16199 RVA: 0x0017D6B8 File Offset: 0x0017B8B8
        public void StartJob(Job newJob, JobCondition lastJobEndCondition = JobCondition.None, ThinkNode jobGiver = null,
            bool resumeCurJobAfterwards = false, bool cancelBusyStances = true, ThinkTreeDef thinkTree = null, JobTag? tag = null,
            bool fromQueue = false, bool canReturnCurJobToPool = false)
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
                            }), false);
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
                        Log.Warning(this.pawn + " tried to start doing a null job.", false);
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
                                    this.pawn.mindState.lastJobTag = tag.Value;
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
                            Log.Warning(
                                "TryMakePreToilReservations() returned false for a non-queued job right after StartJob(). This should have been checked before. curJob=" +
                                this.curJob.ToStringSafe<Job>(), false);
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

        // Token: 0x06003F48 RID: 16200 RVA: 0x0017DA90 File Offset: 0x0017BC90
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

        // Token: 0x06003F49 RID: 16201 RVA: 0x0017DB14 File Offset: 0x0017BD14
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
                Log.Warning("Ending a job with Ongoing as the condition. This makes no sense.", false);
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
                    this.StartJob(JobMaker.MakeJob(JobDefOf.Wait, 250, false), JobCondition.None, null, false, true, null, null, false,
                        false);
                    return;
                }
                if (condition == JobCondition.Succeeded && jobDef != null && jobDef != JobDefOf.Wait_MaintainPosture &&
                    !this.pawn.pather.Moving)
                {
                    this.StartJob(JobMaker.MakeJob(JobDefOf.Wait_MaintainPosture, 1, false), JobCondition.None, null, false, false, null,
                        null, false, false);
                    return;
                }
                this.TryFindAndStartJob();
            }
        }

        // Token: 0x06003F4A RID: 16202 RVA: 0x0017DC88 File Offset: 0x0017BE88
        private void CleanupCurrentJob(JobCondition condition, bool releaseReservations, bool cancelBusyStancesSoft = true,
            bool canReturnToPool = false)
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
            if (!this.pawn.Destroyed && this.pawn.carryTracker != null && this.pawn.carryTracker.CarriedThing != null)
            {
                Thing thing;
                this.pawn.carryTracker.TryDropCarriedThing(this.pawn.Position, ThingPlaceMode.Near, out thing, null);
            }
            if (releaseReservations && canReturnToPool)
            {
                JobMaker.ReturnToPool(job);
            }
        }

        // Token: 0x06003F4B RID: 16203 RVA: 0x0002F7A8 File Offset: 0x0002D9A8
        public JobQueue CaptureAndClearJobQueue()
        {
            JobQueue result = this.jobQueue.Capture();
            this.ClearQueuedJobs(false);
            return result;
        }

        // Token: 0x06003F4C RID: 16204 RVA: 0x0017DDB4 File Offset: 0x0017BFB4
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
                else if (!this.pawn.jobs.TryTakeOrderedJob_NewTemp(queuedJob.job, queuedJob.tag, true))
                {
                    flag = true;
                }
            }
        }

        // Token: 0x06003F4D RID: 16205 RVA: 0x0017DE04 File Offset: 0x0017C004
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

        // Token: 0x06003F4E RID: 16206 RVA: 0x0017DE5C File Offset: 0x0017C05C
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
                    this.StartJob(thinkResult.Job, JobCondition.InterruptOptional, thinkResult.SourceNode, false, false, thinkTree,
                        thinkResult.Tag, thinkResult.FromQueue, false);
                    return;
                }
                if (thinkResult.Job != this.curJob && !this.jobQueue.Contains(thinkResult.Job))
                {
                    JobMaker.ReturnToPool(thinkResult.Job);
                }
            }
        }

        // Token: 0x06003F4F RID: 16207 RVA: 0x0017DEF8 File Offset: 0x0017C0F8
        public void StopAll(bool ifLayingKeepLaying = false, bool canReturnToPool = true)
        {
            if ((!this.pawn.InBed() &&
                 (this.pawn.CurJob == null || this.pawn.CurJob.def != JobDefOf.LayDown || !this.pawn.GetPosture().Laying())) ||
                !ifLayingKeepLaying)
            {
                this.CleanupCurrentJob(JobCondition.InterruptForced, true, true, canReturnToPool);
            }
            this.ClearQueuedJobs(canReturnToPool);
        }

        // Token: 0x06003F50 RID: 16208 RVA: 0x0017DF64 File Offset: 0x0017C164
        private void TryFindAndStartJob()
        {
            if (this.pawn.thinker == null)
            {
                Log.ErrorOnce(this.pawn + " did TryFindAndStartJob but had no thinker.", 8573261, false);
                return;
            }
            if (this.curJob != null)
            {
                Log.Warning(this.pawn + " doing TryFindAndStartJob while still having job " + this.curJob, false);
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
                this.StartJob(result.Job, JobCondition.None, result.SourceNode, false, false, thinkTree, result.Tag, result.FromQueue,
                    false);
            }
        }

        // Token: 0x06003F51 RID: 16209 RVA: 0x0017E034 File Offset: 0x0017C234
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
            if (this.pawn.WorkTagIsDisabled(WorkTags.ManualDumb | WorkTags.Hauling))
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
                if (num2 <= 30f && num2 <= num * 0.5f && num2 + thing.Position.DistanceTo(cell) <= num * 1.7f &&
                    this.pawn.Map.reservationManager.FirstRespectedReserver(thing, this.pawn) == null && !thing.IsForbidden(this.pawn) &&
                    HaulAIUtility.PawnCanAutomaticallyHaulFast(this.pawn, thing, false))
                {
                    StoragePriority currentPriority = StoreUtility.CurrentStoragePriorityOf(thing);
                    IntVec3 invalid = IntVec3.Invalid;
                    if (StoreUtility.TryFindBestBetterStoreCellFor(thing, this.pawn, this.pawn.Map, currentPriority, this.pawn.Faction,
                        out invalid, true))
                    {
                        float num3 = invalid.DistanceTo(cell);
                        if (num3 <= 50f && num3 <= num * 0.6f && num2 + thing.Position.DistanceTo(invalid) + num3 <= num * 1.7f &&
                            num2 + num3 <= num &&
                            this.pawn.Position.WithinRegions(thing.Position, this.pawn.Map, 25,
                                TraverseParms.For(this.pawn, Danger.Deadly, TraverseMode.ByPawn, false), RegionType.Set_Passable) &&
                            invalid.WithinRegions(cell, this.pawn.Map, 25,
                                TraverseParms.For(this.pawn, Danger.Deadly, TraverseMode.ByPawn, false), RegionType.Set_Passable))
                        {
                            if (DebugViewSettings.drawOpportunisticJobs)
                            {
                                Log.Message("Opportunistic job spawned", false);
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

        // Token: 0x06003F52 RID: 16210 RVA: 0x0017E370 File Offset: 0x0017C570
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
                JobUtility.TryStartErrorRecoverJob(this.pawn,
                    this.pawn.ToStringSafe<Pawn>() + " threw exception while determining job (main)", exception, null);
                thinkTree = null;
                return ThinkResult.NoJob;
            }
            finally
            {
            }
            thinkTree = this.pawn.thinker.MainThinkTree;
            return result2;
        }

        // Token: 0x06003F53 RID: 16211 RVA: 0x0017E430 File Offset: 0x0017C630
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
                JobUtility.TryStartErrorRecoverJob(this.pawn,
                    this.pawn.ToStringSafe<Pawn>() + " threw exception while determining job (constant)", exception, null);
            }
            finally
            {
            }
            return ThinkResult.NoJob;
        }

        // Token: 0x06003F54 RID: 16212 RVA: 0x0017E4C8 File Offset: 0x0017C6C8
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

        // Token: 0x06003F55 RID: 16213 RVA: 0x0002F7BC File Offset: 0x0002D9BC
        private bool CanDoAnyJob()
        {
            return this.pawn.Spawned;
        }

        // Token: 0x06003F56 RID: 16214 RVA: 0x0017E540 File Offset: 0x0017C740
        private bool ShouldStartJobFromThinkTree(ThinkResult thinkResult)
        {
            return this.curJob == null || (this.curJob != thinkResult.Job && (thinkResult.FromQueue ||
                                                                              (thinkResult.Job.def != this.curJob.def ||
                                                                               thinkResult.SourceNode != this.curJob.jobGiver ||
                                                                               !this.curDriver.IsContinuation(thinkResult.Job))));
        }

        // Token: 0x06003F57 RID: 16215 RVA: 0x0002F7C9 File Offset: 0x0002D9C9
        public bool IsCurrentJobPlayerInterruptible()
        {
            return (this.curJob == null || this.curJob.def.playerInterruptible) && !this.pawn.HasAttachment(ThingDefOf.Fire);
        }

        // Token: 0x06003F58 RID: 16216 RVA: 0x0017E5B8 File Offset: 0x0017C7B8
        public bool TryTakeOrderedJobPrioritizedWork(Job job, WorkGiver giver, IntVec3 cell)
        {
            if (this.TryTakeOrderedJob(job, giver.def.tagToGive))
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

        // Token: 0x06003F59 RID: 16217 RVA: 0x0002F7FC File Offset: 0x0002D9FC
        public bool TryTakeOrderedJob(Job job, JobTag tag = JobTag.Misc)
        {
            return this.TryTakeOrderedJob_NewTemp(job, new JobTag?(tag), false);
        }

        // Token: 0x06003F5A RID: 16218 RVA: 0x0017E614 File Offset: 0x0017C814
        public bool TryTakeOrderedJob_NewTemp(Job job, JobTag? tag = 0, bool requestQueueing = false)
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
                Log.Warning(
                    "TryMakePreToilReservations() returned false right after TryTakeOrderedJob(). This should have been checked before. job=" +
                    job.ToStringSafe<Job>(), false);
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
                Log.Warning(
                    "TryMakePreToilReservations() returned false right after TryTakeOrderedJob(). This should have been checked before. job=" +
                    job.ToStringSafe<Job>(), false);
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
                Log.Warning(
                    "TryMakePreToilReservations() returned false right after TryTakeOrderedJob(). This should have been checked before. job=" +
                    job.ToStringSafe<Job>(), false);
                this.pawn.ClearReservationsForJob(job);
                return false;
            }
        }

        // Token: 0x06003F5B RID: 16219 RVA: 0x0017E7E4 File Offset: 0x0017C9E4
        public void Notify_TuckedIntoBed(Building_Bed bed)
        {
            this.pawn.Position = RestUtility.GetBedSleepingSlotPosFor(this.pawn, bed);
            this.pawn.Notify_Teleported(false, true);
            this.pawn.stances.CancelBusyStanceHard();
            this.StartJob(JobMaker.MakeJob(JobDefOf.LayDown, bed), JobCondition.InterruptForced, null, false, true, null,
                new JobTag?(JobTag.TuckedIntoBed), false, false);
        }

        // Token: 0x06003F5C RID: 16220 RVA: 0x0017E84C File Offset: 0x0017CA4C
        public void Notify_DamageTaken(DamageInfo dinfo)
        {
            if (this.curJob == null)
            {
                return;
            }
            Job job = this.curJob;
            this.curDriver.Notify_DamageTaken(dinfo);
            if (this.curJob == job && dinfo.Def.ExternalViolenceFor(this.pawn) && dinfo.Def.canInterruptJobs &&
                !this.curJob.playerForced && Find.TickManager.TicksGame >= this.lastDamageCheckTick + 180)
            {
                Thing instigator = dinfo.Instigator;
                if (this.curJob.def.checkOverrideOnDamage == CheckJobOverrideOnDamageMode.Always ||
                    (this.curJob.def.checkOverrideOnDamage == CheckJobOverrideOnDamageMode.OnlyIfInstigatorNotJobTarget &&
                     !this.curJob.AnyTargetIs(instigator)))
                {
                    this.lastDamageCheckTick = Find.TickManager.TicksGame;
                    this.CheckForJobOverride();
                }
            }
        }

        // Token: 0x06003F5D RID: 16221 RVA: 0x0017E924 File Offset: 0x0017CB24
        internal void Notify_MasterDraftedOrUndrafted()
        {
            Pawn master = this.pawn.playerSettings.Master;
            if (master.Spawned && master.Map == this.pawn.Map && this.pawn.playerSettings.followDrafted)
            {
                this.EndCurrentJob(JobCondition.InterruptForced, true, true);
            }
        }

        // Token: 0x06003F5E RID: 16222 RVA: 0x0017E978 File Offset: 0x0017CB78
        public void DrawLinesBetweenTargets()
        {
            Vector3 a = this.pawn.Position.ToVector3Shifted();
            if (this.pawn.pather.curPath != null)
            {
                a = this.pawn.pather.Destination.CenterVector3;
            }
            else if (this.curJob != null && this.curJob.targetA.IsValid && (!this.curJob.targetA.HasThing ||
                                                                            (this.curJob.targetA.Thing.Spawned &&
                                                                             this.curJob.targetA.Thing.Map == this.pawn.Map)))
            {
                GenDraw.DrawLineBetween(a, this.curJob.targetA.CenterVector3, AltitudeLayer.Item.AltitudeFor());
                a = this.curJob.targetA.CenterVector3;
            }
            for (int i = 0; i < this.jobQueue.Count; i++)
            {
                if (this.jobQueue[i].job.targetA.IsValid)
                {
                    if (!this.jobQueue[i].job.targetA.HasThing || (this.jobQueue[i].job.targetA.Thing.Spawned &&
                                                                   this.jobQueue[i].job.targetA.Thing.Map == this.pawn.Map))
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

        // Token: 0x06003F5F RID: 16223 RVA: 0x0017EBEC File Offset: 0x0017CDEC
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
                }), false);
            }
        }

        // Token: 0x04002B94 RID: 11156
        protected Pawn pawn;

        // Token: 0x04002B95 RID: 11157
        public Job curJob;

        // Token: 0x04002B96 RID: 11158
        public JobDriver curDriver;

        // Token: 0x04002B97 RID: 11159
        public JobQueue jobQueue = new JobQueue();

        // Token: 0x04002B98 RID: 11160
        public PawnPosture posture;

        // Token: 0x04002B99 RID: 11161
        public bool startingNewJob;

        // Token: 0x04002B9A RID: 11162
        private int jobsGivenThisTick;

        // Token: 0x04002B9B RID: 11163
        private string jobsGivenThisTickTextual = "";

        // Token: 0x04002B9C RID: 11164
        private int lastJobGivenAtFrame = -1;

        // Token: 0x04002B9D RID: 11165
        private List<int> jobsGivenRecentTicks = new List<int>(10);

        // Token: 0x04002B9E RID: 11166
        private List<string> jobsGivenRecentTicksTextual = new List<string>(10);

        // Token: 0x04002B9F RID: 11167
        public bool debugLog;

        // Token: 0x04002BA0 RID: 11168
        private const int RecentJobQueueMaxLength = 10;

        // Token: 0x04002BA1 RID: 11169
        private const int MaxRecentJobs = 10;

        // Token: 0x04002BA2 RID: 11170
        private static List<Job> tmpJobsToDequeue = new List<Job>();

        // Token: 0x04002BA3 RID: 11171
        private int lastDamageCheckTick = -99999;

        // Token: 0x04002BA4 RID: 11172
        private const int DamageCheckMinInterval = 180;
    }
}
