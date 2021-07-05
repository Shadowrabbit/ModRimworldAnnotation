using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200095B RID: 2395
	public class ThoughtWorker_Precept_HighLife : ThoughtWorker_Precept, IPreceptCompDescriptionArgs
	{
		// Token: 0x06003D14 RID: 15636 RVA: 0x0015110C File Offset: 0x0014F30C
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (ThoughtUtility.ThoughtNullified(p, this.def))
			{
				return false;
			}
			float num = (float)(Find.TickManager.TicksGame - p.mindState.lastTakeRecreationalDrugTick) / 60000f;
			if (num > 1f && this.def.minExpectationForNegativeThought != null && p.MapHeld != null && ExpectationsUtility.CurrentExpectationFor(p.MapHeld).order < this.def.minExpectationForNegativeThought.order)
			{
				return false;
			}
			if (num < 1f)
			{
				return ThoughtState.ActiveAtStage(0);
			}
			if (num < 2f)
			{
				return ThoughtState.ActiveAtStage(1);
			}
			if (num < 11f)
			{
				return ThoughtState.ActiveAtStage(2);
			}
			return ThoughtState.ActiveAtStage(3);
		}

		// Token: 0x06003D15 RID: 15637 RVA: 0x001511C8 File Offset: 0x0014F3C8
		public IEnumerable<NamedArgument> GetDescriptionArgs()
		{
			yield return 0.75f.Named("DAYSSATISIFED");
			yield break;
		}

		// Token: 0x040020BF RID: 8383
		private const float DaysSatisfied = 0.75f;

		// Token: 0x040020C0 RID: 8384
		private const float DaysNoBonus = 1f;

		// Token: 0x040020C1 RID: 8385
		private const float DaysMissing = 2f;

		// Token: 0x040020C2 RID: 8386
		private const float DaysMissing_Major = 11f;

		// Token: 0x040020C3 RID: 8387
		public static readonly SimpleCurve MoodOffsetFromDaysSinceLastDrugCurve = new SimpleCurve
		{
			{
				new CurvePoint(0.75f, 3f),
				true
			},
			{
				new CurvePoint(1f, 0f),
				true
			},
			{
				new CurvePoint(2f, -1f),
				true
			},
			{
				new CurvePoint(11f, -10f),
				true
			}
		};
	}
}
