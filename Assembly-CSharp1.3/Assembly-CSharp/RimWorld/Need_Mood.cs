using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E4F RID: 3663
	public class Need_Mood : Need_Seeker
	{
		// Token: 0x17000E96 RID: 3734
		// (get) Token: 0x060054D8 RID: 21720 RVA: 0x001CBC80 File Offset: 0x001C9E80
		public override float CurInstantLevel
		{
			get
			{
				float num = this.thoughts.TotalMoodOffset();
				if (this.pawn.IsColonist || this.pawn.IsPrisonerOfColony)
				{
					num += Find.Storyteller.difficulty.colonistMoodOffset;
				}
				return Mathf.Clamp01(this.def.baseLevel + num / 100f);
			}
		}

		// Token: 0x17000E97 RID: 3735
		// (get) Token: 0x060054D9 RID: 21721 RVA: 0x001CBCE0 File Offset: 0x001C9EE0
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

		// Token: 0x060054DA RID: 21722 RVA: 0x001CBDCE File Offset: 0x001C9FCE
		public Need_Mood(Pawn pawn) : base(pawn)
		{
			this.thoughts = new ThoughtHandler(pawn);
			this.observer = new PawnObserver(pawn);
			this.recentMemory = new PawnRecentMemory(pawn);
		}

		// Token: 0x060054DB RID: 21723 RVA: 0x001CBDFC File Offset: 0x001C9FFC
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

		// Token: 0x060054DC RID: 21724 RVA: 0x001CBE4D File Offset: 0x001CA04D
		public override void NeedInterval()
		{
			base.NeedInterval();
			this.recentMemory.RecentMemoryInterval();
			this.thoughts.ThoughtInterval();
			this.observer.ObserverInterval();
		}

		// Token: 0x060054DD RID: 21725 RVA: 0x001CBE78 File Offset: 0x001CA078
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

		// Token: 0x060054DE RID: 21726 RVA: 0x001CBF60 File Offset: 0x001CA160
		public override void DrawOnGUI(Rect rect, int maxThresholdMarkers = 2147483647, float customMargin = -1f, bool drawArrows = true, bool doTooltip = true, Rect? rectForTooltip = null)
		{
			if (this.threshPercents == null)
			{
				this.threshPercents = new List<float>();
			}
			this.threshPercents.Clear();
			this.threshPercents.Add(this.pawn.mindState.mentalBreaker.BreakThresholdExtreme);
			this.threshPercents.Add(this.pawn.mindState.mentalBreaker.BreakThresholdMajor);
			this.threshPercents.Add(this.pawn.mindState.mentalBreaker.BreakThresholdMinor);
			base.DrawOnGUI(rect, maxThresholdMarkers, customMargin, drawArrows, doTooltip, rectForTooltip);
		}

		// Token: 0x0400322D RID: 12845
		public ThoughtHandler thoughts;

		// Token: 0x0400322E RID: 12846
		public PawnObserver observer;

		// Token: 0x0400322F RID: 12847
		public PawnRecentMemory recentMemory;
	}
}
