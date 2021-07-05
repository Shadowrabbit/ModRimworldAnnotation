using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020014EC RID: 5356
	public class Need_Mood : Need_Seeker
	{
		// Token: 0x170011C4 RID: 4548
		// (get) Token: 0x0600736D RID: 29549 RVA: 0x00233B50 File Offset: 0x00231D50
		public override float CurInstantLevel
		{
			get
			{
				float num = this.thoughts.TotalMoodOffset();
				if (this.pawn.IsColonist || this.pawn.IsPrisonerOfColony)
				{
					num += Find.Storyteller.difficultyValues.colonistMoodOffset;
				}
				return Mathf.Clamp01(this.def.baseLevel + num / 100f);
			}
		}

		// Token: 0x170011C5 RID: 4549
		// (get) Token: 0x0600736E RID: 29550 RVA: 0x00233BB0 File Offset: 0x00231DB0
		public string MoodString
		{
			get
			{
				if (this.pawn.MentalStateDef != null)
				{
					return "Mood_MentalState".Translate();
				}
				float breakThresholdExtreme = this.pawn.mindState.mentalBreaker.BreakThresholdExtreme;
				if (this.CurLevel < breakThresholdExtreme)
				{
					return "Mood_AboutToBreak".Translate();
				}
				if (this.CurLevel < breakThresholdExtreme + 0.05f)
				{
					return "Mood_OnEdge".Translate();
				}
				if (this.CurLevel < this.pawn.mindState.mentalBreaker.BreakThresholdMinor)
				{
					return "Mood_Stressed".Translate();
				}
				if (this.CurLevel < 0.65f)
				{
					return "Mood_Neutral".Translate();
				}
				if (this.CurLevel < 0.9f)
				{
					return "Mood_Content".Translate();
				}
				return "Mood_Happy".Translate();
			}
		}

		// Token: 0x0600736F RID: 29551 RVA: 0x0004DB67 File Offset: 0x0004BD67
		public Need_Mood(Pawn pawn) : base(pawn)
		{
			this.thoughts = new ThoughtHandler(pawn);
			this.observer = new PawnObserver(pawn);
			this.recentMemory = new PawnRecentMemory(pawn);
		}

		// Token: 0x06007370 RID: 29552 RVA: 0x00233CA0 File Offset: 0x00231EA0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<ThoughtHandler>(ref this.thoughts, "thoughts", new object[]
			{
				this.pawn
			});
			Scribe_Deep.Look<PawnRecentMemory>(ref this.recentMemory, "recentMemory", new object[]
			{
				this.pawn
			});
		}

		// Token: 0x06007371 RID: 29553 RVA: 0x0004DB94 File Offset: 0x0004BD94
		public override void NeedInterval()
		{
			base.NeedInterval();
			this.recentMemory.RecentMemoryInterval();
			this.thoughts.ThoughtInterval();
			this.observer.ObserverInterval();
		}

		// Token: 0x06007372 RID: 29554 RVA: 0x00233CF4 File Offset: 0x00231EF4
		public override string GetTipString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(base.GetTipString());
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("MentalBreakThresholdExtreme".Translate() + ": " + this.pawn.mindState.mentalBreaker.BreakThresholdExtreme.ToStringPercent());
			stringBuilder.AppendLine("MentalBreakThresholdMajor".Translate() + ": " + this.pawn.mindState.mentalBreaker.BreakThresholdMajor.ToStringPercent());
			stringBuilder.AppendLine("MentalBreakThresholdMinor".Translate() + ": " + this.pawn.mindState.mentalBreaker.BreakThresholdMinor.ToStringPercent());
			return stringBuilder.ToString();
		}

		// Token: 0x06007373 RID: 29555 RVA: 0x00233DDC File Offset: 0x00231FDC
		public override void DrawOnGUI(Rect rect, int maxThresholdMarkers = 2147483647, float customMargin = -1f, bool drawArrows = true, bool doTooltip = true)
		{
			if (this.threshPercents == null)
			{
				this.threshPercents = new List<float>();
			}
			this.threshPercents.Clear();
			this.threshPercents.Add(this.pawn.mindState.mentalBreaker.BreakThresholdExtreme);
			this.threshPercents.Add(this.pawn.mindState.mentalBreaker.BreakThresholdMajor);
			this.threshPercents.Add(this.pawn.mindState.mentalBreaker.BreakThresholdMinor);
			base.DrawOnGUI(rect, maxThresholdMarkers, customMargin, drawArrows, doTooltip);
		}

		// Token: 0x04004C28 RID: 19496
		public ThoughtHandler thoughts;

		// Token: 0x04004C29 RID: 19497
		public PawnObserver observer;

		// Token: 0x04004C2A RID: 19498
		public PawnRecentMemory recentMemory;
	}
}
