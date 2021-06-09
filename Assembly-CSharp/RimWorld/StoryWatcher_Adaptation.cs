using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001254 RID: 4692
	public class StoryWatcher_Adaptation : IExposable
	{
		// Token: 0x17000FD3 RID: 4051
		// (get) Token: 0x06006659 RID: 26201 RVA: 0x00045EC2 File Offset: 0x000440C2
		public float TotalThreatPointsFactor
		{
			get
			{
				return this.StorytellerDef.pointsFactorFromAdaptDays.Evaluate(this.adaptDays);
			}
		}

		// Token: 0x17000FD4 RID: 4052
		// (get) Token: 0x0600665A RID: 26202 RVA: 0x00045EDA File Offset: 0x000440DA
		public float AdaptDays
		{
			get
			{
				return this.adaptDays;
			}
		}

		// Token: 0x17000FD5 RID: 4053
		// (get) Token: 0x0600665B RID: 26203 RVA: 0x00045EE2 File Offset: 0x000440E2
		private int Population
		{
			get
			{
				return PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists.Count<Pawn>();
			}
		}

		// Token: 0x17000FD6 RID: 4054
		// (get) Token: 0x0600665C RID: 26204 RVA: 0x00045D32 File Offset: 0x00043F32
		private StorytellerDef StorytellerDef
		{
			get
			{
				return Find.Storyteller.def;
			}
		}

		// Token: 0x0600665D RID: 26205 RVA: 0x001F9440 File Offset: 0x001F7640
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

		// Token: 0x0600665E RID: 26206 RVA: 0x001F949C File Offset: 0x001F769C
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
				}), false);
			}
			this.adaptDays = Mathf.Max(this.StorytellerDef.adaptDaysMin, this.adaptDays - num);
		}

		// Token: 0x0600665F RID: 26207 RVA: 0x001F9584 File Offset: 0x001F7784
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
					num *= Find.Storyteller.difficultyValues.adaptationGrowthRateFactorOverZero;
				}
				this.adaptDays += num;
				this.adaptDays = Mathf.Min(this.adaptDays, this.StorytellerDef.adaptDaysMax);
			}
		}

		// Token: 0x06006660 RID: 26208 RVA: 0x00045EEE File Offset: 0x000440EE
		public void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.adaptDays, "adaptDays", 0f, false);
		}

		// Token: 0x06006661 RID: 26209 RVA: 0x00045F06 File Offset: 0x00044106
		public void Debug_OffsetAdaptDays(float days)
		{
			this.adaptDays += days;
		}

		// Token: 0x04004432 RID: 17458
		private float adaptDays;

		// Token: 0x04004433 RID: 17459
		private List<Pawn> pawnsJustDownedThisTick = new List<Pawn>();

		// Token: 0x04004434 RID: 17460
		private const int UpdateInterval = 30000;
	}
}
