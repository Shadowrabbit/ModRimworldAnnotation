using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x020005A8 RID: 1448
	public abstract class JobDriver : IExposable, IJobEndable
	{
		// Token: 0x17000839 RID: 2105
		// (get) Token: 0x06002A2D RID: 10797 RVA: 0x000FE241 File Offset: 0x000FC441
		public virtual Rot4 ForcedLayingRotation
		{
			get
			{
				return Rot4.Invalid;
			}
		}

		// Token: 0x1700083A RID: 2106
		// (get) Token: 0x06002A2E RID: 10798 RVA: 0x000FE248 File Offset: 0x000FC448
		public virtual Vector3 ForcedBodyOffset
		{
			get
			{
				return Vector3.zero;
			}
		}

		// Token: 0x1700083B RID: 2107
		// (get) Token: 0x06002A2F RID: 10799 RVA: 0x000FE250 File Offset: 0x000FC450
		protected Toil CurToil
		{
			get
			{
				if (this.curToilIndex < 0 || this.job == null || this.pawn.CurJob != this.job)
				{
					return null;
				}
				if (this.curToilIndex >= this.toils.Count)
				{
					Log.Error(string.Concat(new object[]
					{
						this.pawn,
						" with job ",
						this.pawn.CurJob,
						" tried to get CurToil with curToilIndex=",
						this.curToilIndex,
						" but only has ",
						this.toils.Count,
						" toils."
					}));
					return null;
				}
				return this.toils[this.curToilIndex];
			}
		}

		// Token: 0x1700083C RID: 2108
		// (get) Token: 0x06002A30 RID: 10800 RVA: 0x000FE311 File Offset: 0x000FC511
		protected bool HaveCurToil
		{
			get
			{
				return this.curToilIndex >= 0 && this.curToilIndex < this.toils.Count && this.job != null && this.pawn.CurJob == this.job;
			}
		}

		// Token: 0x1700083D RID: 2109
		// (get) Token: 0x06002A31 RID: 10801 RVA: 0x000FE34C File Offset: 0x000FC54C
		private bool CanStartNextToilInBusyStance
		{
			get
			{
				int num = this.curToilIndex + 1;
				return num < this.toils.Count && this.toils[num].atomicWithPrevious;
			}
		}

		// Token: 0x1700083E RID: 2110
		// (get) Token: 0x06002A32 RID: 10802 RVA: 0x000FE383 File Offset: 0x000FC583
		public int CurToilIndex
		{
			get
			{
				return this.curToilIndex;
			}
		}

		// Token: 0x1700083F RID: 2111
		// (get) Token: 0x06002A33 RID: 10803 RVA: 0x000FE38B File Offset: 0x000FC58B
		public bool OnLastToil
		{
			get
			{
				return this.CurToilIndex == this.toils.Count - 1;
			}
		}

		// Token: 0x17000840 RID: 2112
		// (get) Token: 0x06002A34 RID: 10804 RVA: 0x000FE3A2 File Offset: 0x000FC5A2
		public SkillDef ActiveSkill
		{
			get
			{
				if (!this.HaveCurToil || this.CurToil.activeSkill == null)
				{
					return null;
				}
				return this.CurToil.activeSkill();
			}
		}

		// Token: 0x17000841 RID: 2113
		// (get) Token: 0x06002A35 RID: 10805 RVA: 0x000FE3CB File Offset: 0x000FC5CB
		public bool HandlingFacing
		{
			get
			{
				return this.CurToil != null && this.CurToil.handlingFacing;
			}
		}

		// Token: 0x17000842 RID: 2114
		// (get) Token: 0x06002A36 RID: 10806 RVA: 0x000FE3E2 File Offset: 0x000FC5E2
		protected LocalTargetInfo TargetA
		{
			get
			{
				return this.job.targetA;
			}
		}

		// Token: 0x17000843 RID: 2115
		// (get) Token: 0x06002A37 RID: 10807 RVA: 0x000FE3EF File Offset: 0x000FC5EF
		protected LocalTargetInfo TargetB
		{
			get
			{
				return this.job.targetB;
			}
		}

		// Token: 0x17000844 RID: 2116
		// (get) Token: 0x06002A38 RID: 10808 RVA: 0x000FE3FC File Offset: 0x000FC5FC
		protected LocalTargetInfo TargetC
		{
			get
			{
				return this.job.targetC;
			}
		}

		// Token: 0x17000845 RID: 2117
		// (get) Token: 0x06002A39 RID: 10809 RVA: 0x000FE409 File Offset: 0x000FC609
		// (set) Token: 0x06002A3A RID: 10810 RVA: 0x000FE41B File Offset: 0x000FC61B
		protected Thing TargetThingA
		{
			get
			{
				return this.job.targetA.Thing;
			}
			set
			{
				this.job.targetA = value;
			}
		}

		// Token: 0x17000846 RID: 2118
		// (get) Token: 0x06002A3B RID: 10811 RVA: 0x000FE42E File Offset: 0x000FC62E
		// (set) Token: 0x06002A3C RID: 10812 RVA: 0x000FE440 File Offset: 0x000FC640
		protected Thing TargetThingB
		{
			get
			{
				return this.job.targetB.Thing;
			}
			set
			{
				this.job.targetB = value;
			}
		}

		// Token: 0x17000847 RID: 2119
		// (get) Token: 0x06002A3D RID: 10813 RVA: 0x000FE453 File Offset: 0x000FC653
		protected IntVec3 TargetLocA
		{
			get
			{
				return this.job.targetA.Cell;
			}
		}

		// Token: 0x17000848 RID: 2120
		// (get) Token: 0x06002A3E RID: 10814 RVA: 0x000FE465 File Offset: 0x000FC665
		protected Map Map
		{
			get
			{
				return this.pawn.Map;
			}
		}

		// Token: 0x06002A3F RID: 10815 RVA: 0x000FE472 File Offset: 0x000FC672
		public virtual string GetReport()
		{
			return this.ReportStringProcessed(this.job.def.reportString);
		}

		// Token: 0x06002A40 RID: 10816 RVA: 0x000FE48C File Offset: 0x000FC68C
		protected virtual string ReportStringProcessed(string str)
		{
			LocalTargetInfo a = this.job.targetA.IsValid ? this.job.targetA : this.job.targetQueueA.FirstValid();
			LocalTargetInfo b = this.job.targetB.IsValid ? this.job.targetB : this.job.targetQueueB.FirstValid();
			LocalTargetInfo targetC = this.job.targetC;
			return JobUtility.GetResolvedJobReport(str, a, b, targetC);
		}

		// Token: 0x06002A41 RID: 10817
		public abstract bool TryMakePreToilReservations(bool errorOnFailed);

		// Token: 0x06002A42 RID: 10818
		protected abstract IEnumerable<Toil> MakeNewToils();

		// Token: 0x06002A43 RID: 10819 RVA: 0x000FE50E File Offset: 0x000FC70E
		public virtual void SetInitialPosture()
		{
			this.pawn.jobs.posture = PawnPosture.Standing;
		}

		// Token: 0x06002A44 RID: 10820 RVA: 0x000FE524 File Offset: 0x000FC724
		public virtual void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.ended, "ended", false, false);
			Scribe_Values.Look<int>(ref this.curToilIndex, "curToilIndex", 0, true);
			Scribe_Values.Look<int>(ref this.ticksLeftThisToil, "ticksLeftThisToil", 0, false);
			Scribe_Values.Look<bool>(ref this.wantBeginNextToil, "wantBeginNextToil", false, false);
			Scribe_Values.Look<ToilCompleteMode>(ref this.curToilCompleteMode, "curToilCompleteMode", ToilCompleteMode.Undefined, false);
			Scribe_Values.Look<int>(ref this.startTick, "startTick", 0, false);
			Scribe_Values.Look<TargetIndex>(ref this.rotateToFace, "rotateToFace", TargetIndex.A, false);
			Scribe_Values.Look<bool>(ref this.asleep, "asleep", false, false);
			Scribe_Values.Look<float>(ref this.uninstallWorkLeft, "uninstallWorkLeft", 0f, false);
			Scribe_Values.Look<int>(ref this.nextToilIndex, "nextToilIndex", -1, false);
			Scribe_Values.Look<bool>(ref this.collideWithPawns, "collideWithPawns", false, false);
			Scribe_References.Look<Pawn>(ref this.locomotionUrgencySameAs, "locomotionUrgencySameAs", false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.SetupToils();
			}
		}

		// Token: 0x06002A45 RID: 10821 RVA: 0x000FE61C File Offset: 0x000FC81C
		public void Cleanup(JobCondition condition)
		{
			for (int i = 0; i < this.globalFinishActions.Count; i++)
			{
				try
				{
					this.globalFinishActions[i]();
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Pawn ",
						this.pawn.ToStringSafe<Pawn>(),
						" threw exception while executing a global finish action (",
						i,
						"), jobDriver=",
						this.ToStringSafe<JobDriver>(),
						", job=",
						this.job.ToStringSafe<Job>(),
						": ",
						ex
					}));
				}
			}
			if (this.curToilIndex >= 0 && this.curToilIndex < this.toils.Count)
			{
				this.toils[this.curToilIndex].Cleanup(this.curToilIndex, this);
			}
		}

		// Token: 0x06002A46 RID: 10822 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool CanBeginNowWhileLyingDown()
		{
			return false;
		}

		// Token: 0x06002A47 RID: 10823 RVA: 0x000FE714 File Offset: 0x000FC914
		internal void SetupToils()
		{
			try
			{
				this.toils.Clear();
				foreach (Toil toil in this.MakeNewToils())
				{
					if (toil.defaultCompleteMode == ToilCompleteMode.Undefined)
					{
						Log.Error("Toil has undefined complete mode.");
						toil.defaultCompleteMode = ToilCompleteMode.Instant;
					}
					toil.actor = this.pawn;
					this.toils.Add(toil);
				}
			}
			catch (Exception exception)
			{
				JobUtility.TryStartErrorRecoverJob(this.pawn, "Exception in SetupToils for pawn " + this.pawn.ToStringSafe<Pawn>(), exception, this);
			}
		}

		// Token: 0x06002A48 RID: 10824 RVA: 0x000FE7CC File Offset: 0x000FC9CC
		public void DriverTick()
		{
			try
			{
				this.ticksLeftThisToil--;
				this.debugTicksSpentThisToil++;
				if (this.CurToil == null)
				{
					if (!this.pawn.stances.FullBodyBusy || this.CanStartNextToilInBusyStance)
					{
						this.ReadyForNextToil();
					}
				}
				else if (!this.CheckCurrentToilEndOrFail())
				{
					if (this.curToilCompleteMode == ToilCompleteMode.Delay)
					{
						if (this.ticksLeftThisToil <= 0)
						{
							this.ReadyForNextToil();
							return;
						}
					}
					else if (this.curToilCompleteMode == ToilCompleteMode.FinishedBusy && !this.pawn.stances.FullBodyBusy)
					{
						this.ReadyForNextToil();
						return;
					}
					if (this.wantBeginNextToil)
					{
						this.TryActuallyStartNextToil();
					}
					else if (this.curToilCompleteMode == ToilCompleteMode.Instant && this.debugTicksSpentThisToil > 300)
					{
						Log.Error(string.Concat(new object[]
						{
							this.pawn,
							" had to be broken from frozen state. He was doing job ",
							this.job,
							", toilindex=",
							this.curToilIndex
						}));
						this.ReadyForNextToil();
					}
					else
					{
						JobDriver.<>c__DisplayClass61_0 CS$<>8__locals1;
						CS$<>8__locals1.startingJob = this.pawn.CurJob;
						CS$<>8__locals1.startingJobId = CS$<>8__locals1.startingJob.loadID;
						if (this.CurToil.preTickActions != null)
						{
							Toil curToil = this.CurToil;
							for (int i = 0; i < curToil.preTickActions.Count; i++)
							{
								curToil.preTickActions[i]();
								if (this.<DriverTick>g__JobChanged|61_0(ref CS$<>8__locals1))
								{
									return;
								}
								if (this.CurToil != curToil || this.wantBeginNextToil)
								{
									return;
								}
							}
						}
						if (this.CurToil.tickAction != null)
						{
							this.CurToil.tickAction();
							if (this.<DriverTick>g__JobChanged|61_0(ref CS$<>8__locals1))
							{
								return;
							}
						}
						if (this.job.mote != null)
						{
							this.job.mote.Maintain();
						}
					}
				}
			}
			catch (Exception exception)
			{
				JobUtility.TryStartErrorRecoverJob(this.pawn, "Exception in JobDriver tick for pawn " + this.pawn.ToStringSafe<Pawn>(), exception, this);
			}
		}

		// Token: 0x06002A49 RID: 10825 RVA: 0x000FE9F0 File Offset: 0x000FCBF0
		public void ReadyForNextToil()
		{
			this.wantBeginNextToil = true;
			this.TryActuallyStartNextToil();
		}

		// Token: 0x06002A4A RID: 10826 RVA: 0x000FEA00 File Offset: 0x000FCC00
		private void TryActuallyStartNextToil()
		{
			if (!this.pawn.Spawned)
			{
				return;
			}
			if (this.pawn.stances.FullBodyBusy && !this.CanStartNextToilInBusyStance)
			{
				return;
			}
			if (this.job == null || this.pawn.CurJob != this.job)
			{
				return;
			}
			if (this.HaveCurToil)
			{
				this.CurToil.Cleanup(this.curToilIndex, this);
			}
			if (this.nextToilIndex >= 0)
			{
				this.curToilIndex = this.nextToilIndex;
				this.nextToilIndex = -1;
			}
			else
			{
				this.curToilIndex++;
			}
			this.wantBeginNextToil = false;
			if (!this.HaveCurToil)
			{
				if (this.pawn.stances != null && this.pawn.stances.curStance.StanceBusy)
				{
					Log.ErrorOnce(this.pawn.ToStringSafe<Pawn>() + " ended job " + this.job.ToStringSafe<Job>() + " due to running out of toils during a busy stance.", 6453432);
				}
				this.EndJobWith(JobCondition.Succeeded);
				return;
			}
			this.debugTicksSpentThisToil = 0;
			this.ticksLeftThisToil = this.CurToil.defaultDuration;
			this.curToilCompleteMode = this.CurToil.defaultCompleteMode;
			if (!this.CheckCurrentToilEndOrFail())
			{
				Toil curToil = this.CurToil;
				if (this.CurToil.preInitActions != null)
				{
					for (int i = 0; i < this.CurToil.preInitActions.Count; i++)
					{
						try
						{
							this.CurToil.preInitActions[i]();
						}
						catch (Exception exception)
						{
							JobUtility.TryStartErrorRecoverJob(this.pawn, string.Concat(new object[]
							{
								"JobDriver threw exception in preInitActions[",
								i,
								"] for pawn ",
								this.pawn.ToStringSafe<Pawn>()
							}), exception, this);
							return;
						}
						if (this.CurToil != curToil)
						{
							break;
						}
					}
				}
				if (this.CurToil == curToil)
				{
					if (this.CurToil.initAction != null)
					{
						try
						{
							this.CurToil.initAction();
						}
						catch (Exception exception2)
						{
							JobUtility.TryStartErrorRecoverJob(this.pawn, "JobDriver threw exception in initAction for pawn " + this.pawn.ToStringSafe<Pawn>(), exception2, this);
							return;
						}
					}
					if (!this.ended && this.curToilCompleteMode == ToilCompleteMode.Instant && this.CurToil == curToil)
					{
						this.ReadyForNextToil();
					}
				}
			}
		}

		// Token: 0x06002A4B RID: 10827 RVA: 0x000FEC58 File Offset: 0x000FCE58
		public void EndJobWith(JobCondition condition)
		{
			if (!this.pawn.Destroyed && this.job != null && this.pawn.CurJob == this.job)
			{
				this.pawn.jobs.EndCurrentJob(condition, true, true);
			}
		}

		// Token: 0x06002A4C RID: 10828 RVA: 0x000FEC95 File Offset: 0x000FCE95
		public virtual object[] TaleParameters()
		{
			return new object[]
			{
				this.pawn
			};
		}

		// Token: 0x06002A4D RID: 10829 RVA: 0x000FECA8 File Offset: 0x000FCEA8
		private bool CheckCurrentToilEndOrFail()
		{
			bool result;
			try
			{
				Toil curToil = this.CurToil;
				if (this.globalFailConditions != null)
				{
					for (int i = 0; i < this.globalFailConditions.Count; i++)
					{
						JobCondition jobCondition = this.globalFailConditions[i]();
						if (jobCondition != JobCondition.Ongoing)
						{
							if (this.pawn.jobs.debugLog)
							{
								this.pawn.jobs.DebugLogEvent(string.Concat(new object[]
								{
									base.GetType().Name,
									" ends current job ",
									this.job.ToStringSafe<Job>(),
									" because of globalFailConditions[",
									i,
									"]"
								}));
							}
							this.EndJobWith(jobCondition);
							return true;
						}
					}
				}
				if (curToil != null && curToil.endConditions != null)
				{
					for (int j = 0; j < curToil.endConditions.Count; j++)
					{
						JobCondition jobCondition2 = curToil.endConditions[j]();
						if (jobCondition2 != JobCondition.Ongoing)
						{
							if (this.pawn.jobs.debugLog)
							{
								this.pawn.jobs.DebugLogEvent(string.Concat(new object[]
								{
									base.GetType().Name,
									" ends current job ",
									this.job.ToStringSafe<Job>(),
									" because of toils[",
									this.curToilIndex,
									"].endConditions[",
									j,
									"]"
								}));
							}
							this.EndJobWith(jobCondition2);
							return true;
						}
					}
				}
				result = false;
			}
			catch (Exception exception)
			{
				JobUtility.TryStartErrorRecoverJob(this.pawn, "Exception in CheckCurrentToilEndOrFail for pawn " + this.pawn.ToStringSafe<Pawn>(), exception, this);
				result = true;
			}
			return result;
		}

		// Token: 0x06002A4E RID: 10830 RVA: 0x000FEE98 File Offset: 0x000FD098
		private void SetNextToil(Toil to)
		{
			if (to != null && !this.toils.Contains(to))
			{
				Log.Warning(string.Concat(new string[]
				{
					"SetNextToil with non-existent toil (",
					to.ToStringSafe<Toil>(),
					"). pawn=",
					this.pawn.ToStringSafe<Pawn>(),
					", job=",
					this.pawn.CurJob.ToStringSafe<Job>()
				}));
			}
			this.nextToilIndex = this.toils.IndexOf(to);
		}

		// Token: 0x06002A4F RID: 10831 RVA: 0x000FEF1C File Offset: 0x000FD11C
		public void JumpToToil(Toil to)
		{
			if (to == null)
			{
				Log.Warning("JumpToToil with null toil. pawn=" + this.pawn.ToStringSafe<Pawn>() + ", job=" + this.pawn.CurJob.ToStringSafe<Job>());
			}
			this.SetNextToil(to);
			this.ReadyForNextToil();
		}

		// Token: 0x06002A50 RID: 10832 RVA: 0x000FEF68 File Offset: 0x000FD168
		public virtual void Notify_Starting()
		{
			this.startTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06002A51 RID: 10833 RVA: 0x000FEF7A File Offset: 0x000FD17A
		public virtual void Notify_PatherArrived()
		{
			if (this.curToilCompleteMode == ToilCompleteMode.PatherArrival)
			{
				this.ReadyForNextToil();
			}
		}

		// Token: 0x06002A52 RID: 10834 RVA: 0x000FEF8B File Offset: 0x000FD18B
		public virtual void Notify_PatherFailed()
		{
			this.EndJobWith(JobCondition.ErroredPather);
		}

		// Token: 0x06002A53 RID: 10835 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_StanceChanged()
		{
		}

		// Token: 0x06002A54 RID: 10836 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_DamageTaken(DamageInfo dinfo)
		{
		}

		// Token: 0x06002A55 RID: 10837 RVA: 0x000FEF94 File Offset: 0x000FD194
		public Pawn GetActor()
		{
			return this.pawn;
		}

		// Token: 0x06002A56 RID: 10838 RVA: 0x000FEF9C File Offset: 0x000FD19C
		public void AddEndCondition(Func<JobCondition> newEndCondition)
		{
			this.globalFailConditions.Add(newEndCondition);
		}

		// Token: 0x06002A57 RID: 10839 RVA: 0x000FEFAC File Offset: 0x000FD1AC
		public void AddFailCondition(Func<bool> newFailCondition)
		{
			this.globalFailConditions.Add(delegate
			{
				if (newFailCondition())
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
		}

		// Token: 0x06002A58 RID: 10840 RVA: 0x000FEFDD File Offset: 0x000FD1DD
		public void AddFinishAction(Action newAct)
		{
			this.globalFinishActions.Add(newAct);
		}

		// Token: 0x06002A59 RID: 10841 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool ModifyCarriedThingDrawPos(ref Vector3 drawPos, ref bool behind, ref bool flip)
		{
			return false;
		}

		// Token: 0x06002A5A RID: 10842 RVA: 0x000FEFEB File Offset: 0x000FD1EB
		public virtual RandomSocialMode DesiredSocialMode()
		{
			if (this.CurToil != null)
			{
				return this.CurToil.socialMode;
			}
			return RandomSocialMode.Normal;
		}

		// Token: 0x06002A5B RID: 10843 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool IsContinuation(Job j)
		{
			return true;
		}

		// Token: 0x06002A5D RID: 10845 RVA: 0x000FF05F File Offset: 0x000FD25F
		[CompilerGenerated]
		private bool <DriverTick>g__JobChanged|61_0(ref JobDriver.<>c__DisplayClass61_0 A_1)
		{
			return this.pawn.CurJob != A_1.startingJob || this.pawn.CurJob.loadID != A_1.startingJobId;
		}

		// Token: 0x04001A28 RID: 6696
		public Pawn pawn;

		// Token: 0x04001A29 RID: 6697
		public Job job;

		// Token: 0x04001A2A RID: 6698
		private List<Toil> toils = new List<Toil>();

		// Token: 0x04001A2B RID: 6699
		public List<Func<JobCondition>> globalFailConditions = new List<Func<JobCondition>>();

		// Token: 0x04001A2C RID: 6700
		public List<Action> globalFinishActions = new List<Action>();

		// Token: 0x04001A2D RID: 6701
		public bool ended;

		// Token: 0x04001A2E RID: 6702
		private int curToilIndex = -1;

		// Token: 0x04001A2F RID: 6703
		private ToilCompleteMode curToilCompleteMode;

		// Token: 0x04001A30 RID: 6704
		public int ticksLeftThisToil = 99999;

		// Token: 0x04001A31 RID: 6705
		private bool wantBeginNextToil;

		// Token: 0x04001A32 RID: 6706
		protected int startTick = -1;

		// Token: 0x04001A33 RID: 6707
		public TargetIndex rotateToFace = TargetIndex.A;

		// Token: 0x04001A34 RID: 6708
		private int nextToilIndex = -1;

		// Token: 0x04001A35 RID: 6709
		public bool asleep;

		// Token: 0x04001A36 RID: 6710
		public float uninstallWorkLeft;

		// Token: 0x04001A37 RID: 6711
		public bool collideWithPawns;

		// Token: 0x04001A38 RID: 6712
		public Pawn locomotionUrgencySameAs;

		// Token: 0x04001A39 RID: 6713
		public int debugTicksSpentThisToil;
	}
}
