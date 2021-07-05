using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E54 RID: 3668
	public class Need_Rest : Need
	{
		// Token: 0x17000E9C RID: 3740
		// (get) Token: 0x060054E7 RID: 21735 RVA: 0x001CC2E6 File Offset: 0x001CA4E6
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

		// Token: 0x17000E9D RID: 3741
		// (get) Token: 0x060054E8 RID: 21736 RVA: 0x001CC318 File Offset: 0x001CA518
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

		// Token: 0x17000E9E RID: 3742
		// (get) Token: 0x060054E9 RID: 21737 RVA: 0x001CC38F File Offset: 0x001CA58F
		private float RestFallFactor
		{
			get
			{
				return this.pawn.health.hediffSet.RestFallFactor;
			}
		}

		// Token: 0x17000E9F RID: 3743
		// (get) Token: 0x060054EA RID: 21738 RVA: 0x001CC3A6 File Offset: 0x001CA5A6
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

		// Token: 0x17000EA0 RID: 3744
		// (get) Token: 0x060054EB RID: 21739 RVA: 0x001CC3B3 File Offset: 0x001CA5B3
		public int TicksAtZero
		{
			get
			{
				return this.ticksAtZero;
			}
		}

		// Token: 0x17000EA1 RID: 3745
		// (get) Token: 0x060054EC RID: 21740 RVA: 0x001CC3BB File Offset: 0x001CA5BB
		private bool Resting
		{
			get
			{
				return Find.TickManager.TicksGame < this.lastRestTick + 2;
			}
		}

		// Token: 0x060054ED RID: 21741 RVA: 0x001CC3D4 File Offset: 0x001CA5D4
		public Need_Rest(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>();
			this.threshPercents.Add(0.28f);
			this.threshPercents.Add(0.14f);
		}

		// Token: 0x060054EE RID: 21742 RVA: 0x001CC429 File Offset: 0x001CA629
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksAtZero, "ticksAtZero", 0, false);
		}

		// Token: 0x060054EF RID: 21743 RVA: 0x001CC443 File Offset: 0x001CA643
		public override void SetInitialLevel()
		{
			this.CurLevel = Rand.Range(0.9f, 1f);
		}

		// Token: 0x060054F0 RID: 21744 RVA: 0x001CC45C File Offset: 0x001CA65C
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

		// Token: 0x060054F1 RID: 21745 RVA: 0x001CC66B File Offset: 0x001CA86B
		public void TickResting(float restEffectiveness)
		{
			if (restEffectiveness <= 0f)
			{
				return;
			}
			this.lastRestTick = Find.TickManager.TicksGame;
			this.lastRestEffectiveness = restEffectiveness;
		}

		// Token: 0x04003245 RID: 12869
		private int lastRestTick = -999;

		// Token: 0x04003246 RID: 12870
		private float lastRestEffectiveness = 1f;

		// Token: 0x04003247 RID: 12871
		private int ticksAtZero;

		// Token: 0x04003248 RID: 12872
		private const float FullSleepHours = 10.5f;

		// Token: 0x04003249 RID: 12873
		public const float BaseRestGainPerTick = 3.809524E-05f;

		// Token: 0x0400324A RID: 12874
		private const float BaseRestFallPerTick = 1.5833333E-05f;

		// Token: 0x0400324B RID: 12875
		public const float ThreshTired = 0.28f;

		// Token: 0x0400324C RID: 12876
		public const float ThreshVeryTired = 0.14f;

		// Token: 0x0400324D RID: 12877
		public const float DefaultFallAsleepMaxLevel = 0.75f;

		// Token: 0x0400324E RID: 12878
		public const float DefaultNaturalWakeThreshold = 1f;

		// Token: 0x0400324F RID: 12879
		public const float CanWakeThreshold = 0.2f;

		// Token: 0x04003250 RID: 12880
		private const float BaseInvoluntarySleepMTBDays = 0.25f;
	}
}
