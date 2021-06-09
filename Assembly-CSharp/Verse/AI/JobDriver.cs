using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x020009AB RID: 2475
	public abstract class JobDriver : IExposable, IJobEndable
	{
		// Token: 0x1700099D RID: 2461
		// (get) Token: 0x06003C3D RID: 15421 RVA: 0x00171944 File Offset: 0x0016FB44
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
					}), false);
					return null;
				}
				return this.toils[this.curToilIndex];
			}
		}

		// Token: 0x1700099E RID: 2462
		// (get) Token: 0x06003C3E RID: 15422 RVA: 0x0002DDA9 File Offset: 0x0002BFA9
		protected bool HaveCurToil
		{
			get
			{
				return this.curToilIndex >= 0 && this.curToilIndex < this.toils.Count && this.job != null && this.pawn.CurJob == this.job;
			}
		}

		// Token: 0x1700099F RID: 2463
		// (get) Token: 0x06003C3F RID: 15423 RVA: 0x00171A08 File Offset: 0x0016FC08
		private bool CanStartNextToilInBusyStance
		{
			get
			{
				int num = this.curToilIndex + 1;
				return num < this.toils.Count && this.toils[num].atomicWithPrevious;
			}
		}

		// Token: 0x170009A0 RID: 2464
		// (get) Token: 0x06003C40 RID: 15424 RVA: 0x0002DDE4 File Offset: 0x0002BFE4
		public int CurToilIndex
		{
			get
			{
				return this.curToilIndex;
			}
		}

		// Token: 0x170009A1 RID: 2465
		// (get) Token: 0x06003C41 RID: 15425 RVA: 0x0002DDEC File Offset: 0x0002BFEC
		public bool OnLastToil
		{
			get
			{
				return this.CurToilIndex == this.toils.Count - 1;
			}
		}

		// Token: 0x170009A2 RID: 2466
		// (get) Token: 0x06003C42 RID: 15426 RVA: 0x0002DE03 File Offset: 0x0002C003
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

		// Token: 0x170009A3 RID: 2467
		// (get) Token: 0x06003C43 RID: 15427 RVA: 0x0002DE2C File Offset: 0x0002C02C
		public bool HandlingFacing
		{
			get
			{
				return this.CurToil != null && this.CurToil.handlingFacing;
			}
		}

		// Token: 0x170009A4 RID: 2468
		// (get) Token: 0x06003C44 RID: 15428 RVA: 0x0002DE43 File Offset: 0x0002C043
		protected LocalTargetInfo TargetA
		{
			get
			{
				return this.job.targetA;
			}
		}

		// Token: 0x170009A5 RID: 2469
		// (get) Token: 0x06003C45 RID: 15429 RVA: 0x0002DE50 File Offset: 0x0002C050
		protected LocalTargetInfo TargetB
		{
			get
			{
				return this.job.targetB;
			}
		}

		// Token: 0x170009A6 RID: 2470
		// (get) Token: 0x06003C46 RID: 15430 RVA: 0x0002DE5D File Offset: 0x0002C05D
		protected LocalTargetInfo TargetC
		{
			get
			{
				return this.job.targetC;
			}
		}

		// Token: 0x170009A7 RID: 2471
		// (get) Token: 0x06003C47 RID: 15431 RVA: 0x0002DE6A File Offset: 0x0002C06A
		// (set) Token: 0x06003C48 RID: 15432 RVA: 0x0002DE7C File Offset: 0x0002C07C
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

		// Token: 0x170009A8 RID: 2472
		// (get) Token: 0x06003C49 RID: 15433 RVA: 0x0002DE8F File Offset: 0x0002C08F
		// (set) Token: 0x06003C4A RID: 15434 RVA: 0x0002DEA1 File Offset: 0x0002C0A1
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

		// Token: 0x170009A9 RID: 2473
		// (get) Token: 0x06003C4B RID: 15435 RVA: 0x0002DEB4 File Offset: 0x0002C0B4
		protected IntVec3 TargetLocA
		{
			get
			{
				return this.job.targetA.Cell;
			}
		}

		// Token: 0x170009AA RID: 2474
		// (get) Token: 0x06003C4C RID: 15436 RVA: 0x0002DEC6 File Offset: 0x0002C0C6
		protected Map Map
		{
			get
			{
				return this.pawn.Map;
			}
		}

		// Token: 0x06003C4D RID: 15437 RVA: 0x0002DED3 File Offset: 0x0002C0D3
		public virtual string GetReport()
		{
			return this.ReportStringProcessed(this.job.def.reportString);
		}

		// Token: 0x06003C4E RID: 15438 RVA: 0x00171A40 File Offset: 0x0016FC40
		protected virtual string ReportStringProcessed(string str)
		{
			LocalTargetInfo a = this.job.targetA.IsValid ? this.job.targetA : this.job.targetQueueA.FirstValid();
			LocalTargetInfo b = this.job.targetB.IsValid ? this.job.targetB : this.job.targetQueueB.FirstValid();
			LocalTargetInfo targetC = this.job.targetC;
			return JobUtility.GetResolvedJobReport(str, a, b, targetC);
		}

		// Token: 0x06003C4F RID: 15439
		public abstract bool TryMakePreToilReservations(bool errorOnFailed);

		// Token: 0x06003C50 RID: 15440
		protected abstract IEnumerable<Toil> MakeNewToils();

		// Token: 0x06003C51 RID: 15441 RVA: 0x0002DEEB File Offset: 0x0002C0EB
		public virtual void SetInitialPosture()
		{
			this.pawn.jobs.posture = PawnPosture.Standing;
		}

		// Token: 0x06003C52 RID: 15442 RVA: 0x00171AC4 File Offset: 0x0016FCC4
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

		// Token: 0x06003C53 RID: 15443 RVA: 0x00171BBC File Offset: 0x0016FDBC
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
					}), false);
				}
			}
			if (this.curToilIndex >= 0 && this.curToilIndex < this.toils.Count)
			{
				this.toils[this.curToilIndex].Cleanup(this.curToilIndex, this);
			}
		}

		// Token: 0x06003C54 RID: 15444 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool CanBeginNowWhileLyingDown()
		{
			return false;
		}

		// Token: 0x06003C55 RID: 15445 RVA: 0x00171CB4 File Offset: 0x0016FEB4
		internal void SetupToils()
		{
			try
			{
				this.toils.Clear();
				foreach (Toil toil in this.MakeNewToils())
				{
					if (toil.defaultCompleteMode == ToilCompleteMode.Undefined)
					{
						Log.Error("Toil has undefined complete mode.", false);
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

		// Token: 0x06003C56 RID: 15446 RVA: 0x00171D6C File Offset: 0x0016FF6C
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
						}), false);
						this.ReadyForNextToil();
					}
					else
					{
						JobDriver.<>c__DisplayClass57_0 CS$<>8__locals1;
						CS$<>8__locals1.startingJob = this.pawn.CurJob;
						CS$<>8__locals1.startingJobId = CS$<>8__locals1.startingJob.loadID;
						if (this.CurToil.preTickActions != null)
						{
							Toil curToil = this.CurToil;
							for (int i = 0; i < curToil.preTickActions.Count; i++)
							{
								curToil.preTickActions[i]();
								if (this.<DriverTick>g__JobChanged|57_0(ref CS$<>8__locals1))
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
							if (this.<DriverTick>g__JobChanged|57_0(ref CS$<>8__locals1))
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

		// Token: 0x06003C57 RID: 15447 RVA: 0x0002DEFE File Offset: 0x0002C0FE
		public void ReadyForNextToil()
		{
			this.wantBeginNextToil = true;
			this.TryActuallyStartNextToil();
		}

		// Token: 0x06003C58 RID: 15448 RVA: 0x00171F94 File Offset: 0x00170194
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
					Log.ErrorOnce(this.pawn.ToStringSafe<Pawn>() + " ended job " + this.job.ToStringSafe<Job>() + " due to running out of toils during a busy stance.", 6453432, false);
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

		// Token: 0x06003C59 RID: 15449 RVA: 0x0002DF0D File Offset: 0x0002C10D
		public void EndJobWith(JobCondition condition)
		{
			if (!this.pawn.Destroyed && this.job != null && this.pawn.CurJob == this.job)
			{
				this.pawn.jobs.EndCurrentJob(condition, true, true);
			}
		}

		// Token: 0x06003C5A RID: 15450 RVA: 0x0002DF4A File Offset: 0x0002C14A
		public virtual object[] TaleParameters()
		{
			return new object[]
			{
				this.pawn
			};
		}

		// Token: 0x06003C5B RID: 15451 RVA: 0x001721F0 File Offset: 0x001703F0
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

		// Token: 0x06003C5C RID: 15452 RVA: 0x001723E0 File Offset: 0x001705E0
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
				}), false);
			}
			this.nextToilIndex = this.toils.IndexOf(to);
		}

		// Token: 0x06003C5D RID: 15453 RVA: 0x00172464 File Offset: 0x00170664
		public void JumpToToil(Toil to)
		{
			if (to == null)
			{
				Log.Warning("JumpToToil with null toil. pawn=" + this.pawn.ToStringSafe<Pawn>() + ", job=" + this.pawn.CurJob.ToStringSafe<Job>(), false);
			}
			this.SetNextToil(to);
			this.ReadyForNextToil();
		}

		// Token: 0x06003C5E RID: 15454 RVA: 0x0002DF5B File Offset: 0x0002C15B
		public virtual void Notify_Starting()
		{
			this.startTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06003C5F RID: 15455 RVA: 0x0002DF6D File Offset: 0x0002C16D
		public virtual void Notify_PatherArrived()
		{
			if (this.curToilCompleteMode == ToilCompleteMode.PatherArrival)
			{
				this.ReadyForNextToil();
			}
		}

		// Token: 0x06003C60 RID: 15456 RVA: 0x0002DF7E File Offset: 0x0002C17E
		public virtual void Notify_PatherFailed()
		{
			this.EndJobWith(JobCondition.ErroredPather);
		}

		// Token: 0x06003C61 RID: 15457 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_StanceChanged()
		{
		}

		// Token: 0x06003C62 RID: 15458 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_DamageTaken(DamageInfo dinfo)
		{
		}

		// Token: 0x06003C63 RID: 15459 RVA: 0x0002DF87 File Offset: 0x0002C187
		public Pawn GetActor()
		{
			return this.pawn;
		}

		// Token: 0x06003C64 RID: 15460 RVA: 0x0002DF8F File Offset: 0x0002C18F
		public void AddEndCondition(Func<JobCondition> newEndCondition)
		{
			this.globalFailConditions.Add(newEndCondition);
		}

		// Token: 0x06003C65 RID: 15461 RVA: 0x001724B4 File Offset: 0x001706B4
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

		// Token: 0x06003C66 RID: 15462 RVA: 0x0002DF9D File Offset: 0x0002C19D
		public void AddFinishAction(Action newAct)
		{
			this.globalFinishActions.Add(newAct);
		}

		// Token: 0x06003C67 RID: 15463 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool ModifyCarriedThingDrawPos(ref Vector3 drawPos, ref bool behind, ref bool flip)
		{
			return false;
		}

		// Token: 0x06003C68 RID: 15464 RVA: 0x0002DFAB File Offset: 0x0002C1AB
		public virtual RandomSocialMode DesiredSocialMode()
		{
			if (this.CurToil != null)
			{
				return this.CurToil.socialMode;
			}
			return RandomSocialMode.Normal;
		}

		// Token: 0x06003C69 RID: 15465 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public virtual bool IsContinuation(Job j)
		{
			return true;
		}

		// Token: 0x06003C6B RID: 15467 RVA: 0x0002DFC2 File Offset: 0x0002C1C2
		[CompilerGenerated]
		private bool <DriverTick>g__JobChanged|57_0(ref JobDriver.<>c__DisplayClass57_0 A_1)
		{
			return this.pawn.CurJob != A_1.startingJob || this.pawn.CurJob.loadID != A_1.startingJobId;
		}

		// Token: 0x040029BD RID: 10685
		public Pawn pawn;

		// Token: 0x040029BE RID: 10686
		public Job job;

		// Token: 0x040029BF RID: 10687
		private List<Toil> toils = new List<Toil>();

		// Token: 0x040029C0 RID: 10688
		public List<Func<JobCondition>> globalFailConditions = new List<Func<JobCondition>>();

		// Token: 0x040029C1 RID: 10689
		public List<Action> globalFinishActions = new List<Action>();

		// Token: 0x040029C2 RID: 10690
		public bool ended;

		// Token: 0x040029C3 RID: 10691
		private int curToilIndex = -1;

		// Token: 0x040029C4 RID: 10692
		private ToilCompleteMode curToilCompleteMode;

		// Token: 0x040029C5 RID: 10693
		public int ticksLeftThisToil = 99999;

		// Token: 0x040029C6 RID: 10694
		private bool wantBeginNextToil;

		// Token: 0x040029C7 RID: 10695
		protected int startTick = -1;

		// Token: 0x040029C8 RID: 10696
		public TargetIndex rotateToFace = TargetIndex.A;

		// Token: 0x040029C9 RID: 10697
		private int nextToilIndex = -1;

		// Token: 0x040029CA RID: 10698
		public bool asleep;

		// Token: 0x040029CB RID: 10699
		public float uninstallWorkLeft;

		// Token: 0x040029CC RID: 10700
		public bool collideWithPawns;

		// Token: 0x040029CD RID: 10701
		public Pawn locomotionUrgencySameAs;

		// Token: 0x040029CE RID: 10702
		public int debugTicksSpentThisToil;
	}
}
