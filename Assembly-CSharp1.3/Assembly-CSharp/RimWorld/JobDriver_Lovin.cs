using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000724 RID: 1828
	public class JobDriver_Lovin : JobDriver
	{
		// Token: 0x17000979 RID: 2425
		// (get) Token: 0x060032C3 RID: 12995 RVA: 0x001235F5 File Offset: 0x001217F5
		private Pawn Partner
		{
			get
			{
				return (Pawn)((Thing)this.job.GetTarget(this.PartnerInd));
			}
		}

		// Token: 0x1700097A RID: 2426
		// (get) Token: 0x060032C4 RID: 12996 RVA: 0x00123612 File Offset: 0x00121812
		private Building_Bed Bed
		{
			get
			{
				return (Building_Bed)((Thing)this.job.GetTarget(this.BedInd));
			}
		}

		// Token: 0x060032C5 RID: 12997 RVA: 0x0012362F File Offset: 0x0012182F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeft, "ticksLeft", 0, false);
		}

		// Token: 0x060032C6 RID: 12998 RVA: 0x0012364C File Offset: 0x0012184C
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Partner, this.job, 1, -1, null, errorOnFailed) && this.pawn.Reserve(this.Bed, this.job, this.Bed.SleepingSlotsCount, 0, null, errorOnFailed);
		}

		// Token: 0x060032C7 RID: 12999 RVA: 0x001236A7 File Offset: 0x001218A7
		public override bool CanBeginNowWhileLyingDown()
		{
			return JobInBedUtility.InBedOrRestSpotNow(this.pawn, this.job.GetTarget(this.BedInd));
		}

		// Token: 0x060032C8 RID: 13000 RVA: 0x001236C5 File Offset: 0x001218C5
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(this.BedInd);
			this.FailOnDespawnedOrNull(this.PartnerInd);
			this.FailOn(() => !this.Partner.health.capacities.CanBeAwake);
			this.KeepLyingDown(this.BedInd);
			yield return Toils_Bed.ClaimBedIfNonMedical(this.BedInd, TargetIndex.None);
			yield return Toils_Bed.GotoBed(this.BedInd);
			yield return new Toil
			{
				initAction = delegate()
				{
					if (this.Partner.CurJob == null || this.Partner.CurJob.def != JobDefOf.Lovin)
					{
						Job newJob = JobMaker.MakeJob(JobDefOf.Lovin, this.pawn, this.Bed);
						this.Partner.jobs.StartJob(newJob, JobCondition.InterruptForced, null, false, true, null, null, false, false);
						this.ticksLeft = (int)(2500f * Mathf.Clamp(Rand.Range(0.1f, 1.1f), 0.1f, 2f));
						Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.InitiatedLovin, this.pawn.Named(HistoryEventArgsNames.Doer)), true);
						return;
					}
					this.ticksLeft = 9999999;
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			Toil toil = Toils_LayDown.LayDown(this.BedInd, true, false, false, false, PawnPosture.LayingOnGroundNormal);
			toil.FailOn(() => this.Partner.CurJob == null || this.Partner.CurJob.def != JobDefOf.Lovin);
			toil.AddPreTickAction(delegate
			{
				this.ticksLeft--;
				if (this.ticksLeft <= 0)
				{
					base.ReadyForNextToil();
					return;
				}
				if (this.pawn.IsHashIntervalTick(100))
				{
					FleckMaker.ThrowMetaIcon(this.pawn.Position, this.pawn.Map, FleckDefOf.Heart, 0.42f);
				}
			});
			toil.AddFinishAction(delegate
			{
				Thought_Memory thought_Memory = (Thought_Memory)ThoughtMaker.MakeThought(ThoughtDefOf.GotSomeLovin);
				if (this.pawn.health != null && this.pawn.health.hediffSet != null)
				{
					if (this.pawn.health.hediffSet.hediffs.Any((Hediff h) => h.def == HediffDefOf.LoveEnhancer))
					{
						goto IL_C4;
					}
				}
				if (this.Partner.health == null || this.Partner.health.hediffSet == null)
				{
					goto IL_CF;
				}
				if (!this.Partner.health.hediffSet.hediffs.Any((Hediff h) => h.def == HediffDefOf.LoveEnhancer))
				{
					goto IL_CF;
				}
				IL_C4:
				thought_Memory.moodPowerFactor = 1.5f;
				IL_CF:
				if (this.pawn.needs.mood != null)
				{
					this.pawn.needs.mood.thoughts.memories.TryGainMemory(thought_Memory, this.Partner);
				}
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.GotLovin, this.pawn.Named(HistoryEventArgsNames.Doer)), true);
				HistoryEventDef def = this.pawn.relations.DirectRelationExists(PawnRelationDefOf.Spouse, this.Partner) ? HistoryEventDefOf.GotLovin_Spouse : HistoryEventDefOf.GotLovin_NonSpouse;
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(def, this.pawn.Named(HistoryEventArgsNames.Doer)), true);
				this.pawn.mindState.canLovinTick = Find.TickManager.TicksGame + this.GenerateRandomMinTicksToNextLovin(this.pawn);
			});
			toil.socialMode = RandomSocialMode.Off;
			yield return toil;
			yield break;
		}

		// Token: 0x060032C9 RID: 13001 RVA: 0x001236D8 File Offset: 0x001218D8
		private int GenerateRandomMinTicksToNextLovin(Pawn pawn)
		{
			if (DebugSettings.alwaysDoLovin)
			{
				return 100;
			}
			float num = JobDriver_Lovin.LovinIntervalHoursFromAgeCurve.Evaluate(pawn.ageTracker.AgeBiologicalYearsFloat);
			num = Rand.Gaussian(num, 0.3f);
			if (num < 0.5f)
			{
				num = 0.5f;
			}
			return (int)(num * 2500f);
		}

		// Token: 0x04001DD3 RID: 7635
		private int ticksLeft;

		// Token: 0x04001DD4 RID: 7636
		private TargetIndex PartnerInd = TargetIndex.A;

		// Token: 0x04001DD5 RID: 7637
		private TargetIndex BedInd = TargetIndex.B;

		// Token: 0x04001DD6 RID: 7638
		private const int TicksBetweenHeartMotes = 100;

		// Token: 0x04001DD7 RID: 7639
		private static readonly SimpleCurve LovinIntervalHoursFromAgeCurve = new SimpleCurve
		{
			{
				new CurvePoint(16f, 1.5f),
				true
			},
			{
				new CurvePoint(22f, 1.5f),
				true
			},
			{
				new CurvePoint(30f, 4f),
				true
			},
			{
				new CurvePoint(50f, 12f),
				true
			},
			{
				new CurvePoint(75f, 36f),
				true
			}
		};
	}
}
