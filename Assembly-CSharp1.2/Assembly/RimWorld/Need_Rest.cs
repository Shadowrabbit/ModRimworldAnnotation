using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020014F1 RID: 5361
	public class Need_Rest : Need
	{
		// Token: 0x170011CA RID: 4554
		// (get) Token: 0x0600737C RID: 29564 RVA: 0x0004DC08 File Offset: 0x0004BE08
		public RestCategory CurCategory
		{
			get
			{
				if (this.CurLevel < 0.01f)
				{
					return RestCategory.Exhausted;
				}
				if (this.CurLevel < 0.14f)
				{
					return RestCategory.VeryTired;
				}
				if (this.CurLevel < 0.28f)
				{
					return RestCategory.Tired;
				}
				return RestCategory.Rested;
			}
		}

		// Token: 0x170011CB RID: 4555
		// (get) Token: 0x0600737D RID: 29565 RVA: 0x00234100 File Offset: 0x00232300
		public float RestFallPerTick
		{
			get
			{
				switch (this.CurCategory)
				{
				case RestCategory.Rested:
					return 1.5833333E-05f * this.RestFallFactor;
				case RestCategory.Tired:
					return 1.5833333E-05f * this.RestFallFactor * 0.7f;
				case RestCategory.VeryTired:
					return 1.5833333E-05f * this.RestFallFactor * 0.3f;
				case RestCategory.Exhausted:
					return 1.5833333E-05f * this.RestFallFactor * 0.6f;
				default:
					return 999f;
				}
			}
		}

		// Token: 0x170011CC RID: 4556
		// (get) Token: 0x0600737E RID: 29566 RVA: 0x0004DC38 File Offset: 0x0004BE38
		private float RestFallFactor
		{
			get
			{
				return this.pawn.health.hediffSet.RestFallFactor;
			}
		}

		// Token: 0x170011CD RID: 4557
		// (get) Token: 0x0600737F RID: 29567 RVA: 0x0004DC4F File Offset: 0x0004BE4F
		public override int GUIChangeArrow
		{
			get
			{
				if (this.Resting)
				{
					return 1;
				}
				return -1;
			}
		}

		// Token: 0x170011CE RID: 4558
		// (get) Token: 0x06007380 RID: 29568 RVA: 0x0004DC5C File Offset: 0x0004BE5C
		public int TicksAtZero
		{
			get
			{
				return this.ticksAtZero;
			}
		}

		// Token: 0x170011CF RID: 4559
		// (get) Token: 0x06007381 RID: 29569 RVA: 0x0004DC64 File Offset: 0x0004BE64
		private bool Resting
		{
			get
			{
				return Find.TickManager.TicksGame < this.lastRestTick + 2;
			}
		}

		// Token: 0x06007382 RID: 29570 RVA: 0x00234178 File Offset: 0x00232378
		public Need_Rest(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>();
			this.threshPercents.Add(0.28f);
			this.threshPercents.Add(0.14f);
		}

		// Token: 0x06007383 RID: 29571 RVA: 0x0004DC7A File Offset: 0x0004BE7A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksAtZero, "ticksAtZero", 0, false);
		}

		// Token: 0x06007384 RID: 29572 RVA: 0x0004DC94 File Offset: 0x0004BE94
		public override void SetInitialLevel()
		{
			this.CurLevel = Rand.Range(0.9f, 1f);
		}

		// Token: 0x06007385 RID: 29573 RVA: 0x002341D0 File Offset: 0x002323D0
		public override void NeedInterval()
		{
			if (!this.IsFrozen)
			{
				if (this.Resting)
				{
					float num = this.lastRestEffectiveness;
					num *= this.pawn.GetStatValue(StatDefOf.RestRateMultiplier, true);
					if (num > 0f)
					{
						this.CurLevel += 0.005714286f * num;
					}
				}
				else
				{
					this.CurLevel -= this.RestFallPerTick * 150f;
				}
			}
			if (this.CurLevel < 0.0001f)
			{
				this.ticksAtZero += 150;
			}
			else
			{
				this.ticksAtZero = 0;
			}
			if (this.ticksAtZero > 1000 && this.pawn.Spawned)
			{
				float mtb;
				if (this.ticksAtZero < 15000)
				{
					mtb = 0.25f;
				}
				else if (this.ticksAtZero < 30000)
				{
					mtb = 0.125f;
				}
				else if (this.ticksAtZero < 45000)
				{
					mtb = 0.083333336f;
				}
				else
				{
					mtb = 0.0625f;
				}
				if (Rand.MTBEventOccurs(mtb, 60000f, 150f) && (this.pawn.CurJob == null || this.pawn.CurJob.def != JobDefOf.LayDown))
				{
					this.pawn.jobs.StartJob(JobMaker.MakeJob(JobDefOf.LayDown, this.pawn.Position), JobCondition.InterruptForced, null, false, true, null, new JobTag?(JobTag.SatisfyingNeeds), false, false);
					if (this.pawn.InMentalState && this.pawn.MentalStateDef.recoverFromCollapsingExhausted)
					{
						this.pawn.mindState.mentalStateHandler.CurState.RecoverFromState();
					}
					if (PawnUtility.ShouldSendNotificationAbout(this.pawn))
					{
						Messages.Message("MessageInvoluntarySleep".Translate(this.pawn.LabelShort, this.pawn), this.pawn, MessageTypeDefOf.NegativeEvent, true);
					}
					TaleRecorder.RecordTale(TaleDefOf.Exhausted, new object[]
					{
						this.pawn
					});
				}
			}
		}

		// Token: 0x06007386 RID: 29574 RVA: 0x0004DCAB File Offset: 0x0004BEAB
		public void TickResting(float restEffectiveness)
		{
			if (restEffectiveness <= 0f)
			{
				return;
			}
			this.lastRestTick = Find.TickManager.TicksGame;
			this.lastRestEffectiveness = restEffectiveness;
		}

		// Token: 0x04004C40 RID: 19520
		private int lastRestTick = -999;

		// Token: 0x04004C41 RID: 19521
		private float lastRestEffectiveness = 1f;

		// Token: 0x04004C42 RID: 19522
		private int ticksAtZero;

		// Token: 0x04004C43 RID: 19523
		private const float FullSleepHours = 10.5f;

		// Token: 0x04004C44 RID: 19524
		public const float BaseRestGainPerTick = 3.809524E-05f;

		// Token: 0x04004C45 RID: 19525
		private const float BaseRestFallPerTick = 1.5833333E-05f;

		// Token: 0x04004C46 RID: 19526
		public const float ThreshTired = 0.28f;

		// Token: 0x04004C47 RID: 19527
		public const float ThreshVeryTired = 0.14f;

		// Token: 0x04004C48 RID: 19528
		public const float DefaultFallAsleepMaxLevel = 0.75f;

		// Token: 0x04004C49 RID: 19529
		public const float DefaultNaturalWakeThreshold = 1f;

		// Token: 0x04004C4A RID: 19530
		public const float CanWakeThreshold = 0.2f;

		// Token: 0x04004C4B RID: 19531
		private const float BaseInvoluntarySleepMTBDays = 0.25f;
	}
}
