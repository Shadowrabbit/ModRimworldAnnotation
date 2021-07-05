using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200093E RID: 2366
	public class ThoughtWorker_Precept_RecentConquest : ThoughtWorker_Precept, IPreceptCompDescriptionArgs
	{
		// Token: 0x17000ACE RID: 2766
		// (get) Token: 0x06003CCB RID: 15563 RVA: 0x00150404 File Offset: 0x0014E604
		private float DaysSinceLastRaid
		{
			get
			{
				return (float)(Find.TickManager.TicksGame - Find.History.lastTickPlayerRaidedSomeone) / 60000f;
			}
		}

		// Token: 0x06003CCC RID: 15564 RVA: 0x00150424 File Offset: 0x0014E624
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!p.IsColonist)
			{
				return false;
			}
			if (p.IsSlave)
			{
				return false;
			}
			int num = (this.DaysSinceLastRaid > (float)this.DaysSinceLastRaidThreshold) ? 1 : 0;
			if (num == 1 && (float)Find.TickManager.TicksGame < 1800000f)
			{
				return false;
			}
			return ThoughtState.ActiveAtStage(num);
		}

		// Token: 0x06003CCD RID: 15565 RVA: 0x00150486 File Offset: 0x0014E686
		public override float MoodMultiplier(Pawn p)
		{
			return ThoughtWorker_Precept_RecentConquest.MoodMultiplierCurve.Evaluate(this.DaysSinceLastRaid);
		}

		// Token: 0x06003CCE RID: 15566 RVA: 0x00150498 File Offset: 0x0014E698
		public IEnumerable<NamedArgument> GetDescriptionArgs()
		{
			yield return this.DaysSinceLastRaidThreshold.Named("DAYSSINCELASTRAIDTHRESHOLD");
			yield break;
		}

		// Token: 0x040020BC RID: 8380
		private static readonly SimpleCurve MoodMultiplierCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(25f, 0f),
				true
			},
			{
				new CurvePoint(50f, 1f),
				true
			},
			{
				new CurvePoint(75f, 1.5f),
				true
			}
		};

		// Token: 0x040020BD RID: 8381
		private int DaysSinceLastRaidThreshold = 25;
	}
}
