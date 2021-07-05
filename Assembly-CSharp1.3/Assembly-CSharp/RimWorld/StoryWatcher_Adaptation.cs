using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C31 RID: 3121
	public class StoryWatcher_Adaptation : IExposable
	{
		// Token: 0x17000CAC RID: 3244
		// (get) Token: 0x0600493F RID: 18751 RVA: 0x00183EC7 File Offset: 0x001820C7
		public float TotalThreatPointsFactor
		{
			get
			{
				return this.StorytellerDef.pointsFactorFromAdaptDays.Evaluate(this.adaptDays);
			}
		}

		// Token: 0x17000CAD RID: 3245
		// (get) Token: 0x06004940 RID: 18752 RVA: 0x00183EDF File Offset: 0x001820DF
		public float AdaptDays
		{
			get
			{
				return this.adaptDays;
			}
		}

		// Token: 0x17000CAE RID: 3246
		// (get) Token: 0x06004941 RID: 18753 RVA: 0x00183EE7 File Offset: 0x001820E7
		private int Population
		{
			get
			{
				return PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists.Count<Pawn>();
			}
		}

		// Token: 0x17000CAF RID: 3247
		// (get) Token: 0x06004942 RID: 18754 RVA: 0x00183EF3 File Offset: 0x001820F3
		private StorytellerDef StorytellerDef
		{
			get
			{
				return Find.Storyteller.def;
			}
		}

		// Token: 0x06004943 RID: 18755 RVA: 0x00183F00 File Offset: 0x00182100
		public void Notify_PawnEvent(Pawn p, AdaptationEvent ev, DamageInfo? dinfo = null)
		{
			if (!p.RaceProps.Humanlike || !p.IsColonist)
			{
				return;
			}
			if (ev != AdaptationEvent.Downed)
			{
				this.ResolvePawnEvent(p, ev);
				return;
			}
			if (dinfo == null || !dinfo.Value.Def.ExternalViolenceFor(p))
			{
				return;
			}
			this.pawnsJustDownedThisTick.Add(p);
		}

		// Token: 0x06004944 RID: 18756 RVA: 0x00183F5C File Offset: 0x0018215C
		private void ResolvePawnEvent(Pawn p, AdaptationEvent ev)
		{
			float num;
			if (ev == AdaptationEvent.Downed)
			{
				num = this.StorytellerDef.adaptDaysLossFromColonistViolentlyDownedByPopulation.Evaluate((float)this.Population);
			}
			else
			{
				if (this.pawnsJustDownedThisTick.Contains(p))
				{
					this.pawnsJustDownedThisTick.Remove(p);
				}
				int num2 = this.Population - 1;
				num = this.StorytellerDef.adaptDaysLossFromColonistLostByPostPopulation.Evaluate((float)num2);
			}
			if (DebugViewSettings.writeStoryteller)
			{
				Log.Message(string.Concat(new object[]
				{
					"Adaptation event: ",
					p,
					" ",
					ev,
					". Loss: ",
					num.ToString("F1"),
					" from ",
					this.adaptDays.ToString("F1")
				}));
			}
			this.adaptDays = Mathf.Max(this.StorytellerDef.adaptDaysMin, this.adaptDays - num);
		}

		// Token: 0x06004945 RID: 18757 RVA: 0x00184044 File Offset: 0x00182244
		public void AdaptationWatcherTick()
		{
			for (int i = 0; i < this.pawnsJustDownedThisTick.Count; i++)
			{
				this.ResolvePawnEvent(this.pawnsJustDownedThisTick[i], AdaptationEvent.Downed);
			}
			this.pawnsJustDownedThisTick.Clear();
			if (Find.TickManager.TicksGame % 30000 == 0)
			{
				if (this.adaptDays >= 0f && (float)GenDate.DaysPassed < this.StorytellerDef.adaptDaysGameStartGraceDays)
				{
					return;
				}
				float num = 0.5f * this.StorytellerDef.adaptDaysGrowthRateCurve.Evaluate(this.adaptDays);
				if (this.adaptDays > 0f)
				{
					num *= Find.Storyteller.difficulty.adaptationGrowthRateFactorOverZero;
				}
				this.adaptDays += num;
				this.adaptDays = Mathf.Min(this.adaptDays, this.StorytellerDef.adaptDaysMax);
			}
		}

		// Token: 0x06004946 RID: 18758 RVA: 0x00184121 File Offset: 0x00182321
		public void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.adaptDays, "adaptDays", 0f, false);
		}

		// Token: 0x06004947 RID: 18759 RVA: 0x00184139 File Offset: 0x00182339
		public void Debug_OffsetAdaptDays(float days)
		{
			this.adaptDays += days;
		}

		// Token: 0x04002C93 RID: 11411
		private float adaptDays;

		// Token: 0x04002C94 RID: 11412
		private List<Pawn> pawnsJustDownedThisTick = new List<Pawn>();

		// Token: 0x04002C95 RID: 11413
		private const int UpdateInterval = 30000;
	}
}
