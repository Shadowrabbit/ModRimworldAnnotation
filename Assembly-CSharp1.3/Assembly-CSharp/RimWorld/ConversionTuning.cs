using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DEF RID: 3567
	public class ConversionTuning
	{
		// Token: 0x040030BF RID: 12479
		public const float ConvertAttempt_BaseSelectionWeight = 0.04f;

		// Token: 0x040030C0 RID: 12480
		public const float ConvertAttempt_Colonist = 1f;

		// Token: 0x040030C1 RID: 12481
		public const float ConvertAttempt_Slave = 0.5f;

		// Token: 0x040030C2 RID: 12482
		public const float ConvertAttempt_NPCFreeVsColonist = 0.5f;

		// Token: 0x040030C3 RID: 12483
		public const float ConvertAttempt_NPCFreeVsNPCFree = 0.25f;

		// Token: 0x040030C4 RID: 12484
		public const float ConvertAttempt_NPCFreeVsPrisoner = 0.25f;

		// Token: 0x040030C5 RID: 12485
		public const float ConvertAttempt_NPCFreeVsSlave = 0.5f;

		// Token: 0x040030C6 RID: 12486
		public const float ConvertAttempt_PrisonerVsColonist = 0.25f;

		// Token: 0x040030C7 RID: 12487
		public const float ConvertAttempt_PrisonerVsNPCFree = 0.25f;

		// Token: 0x040030C8 RID: 12488
		public const float ConvertAttempt_PrisonerVsPrisoner = 0.5f;

		// Token: 0x040030C9 RID: 12489
		public const float ConvertAttempt_PrisonerVsSlave = 0.5f;

		// Token: 0x040030CA RID: 12490
		public const float ConvertAttempt_FailOutcomeWeight_Nothing = 0.6f;

		// Token: 0x040030CB RID: 12491
		public const float ConvertAttempt_FailOutcomeWeight_Resentment = 0.36f;

		// Token: 0x040030CC RID: 12492
		public const float ConvertAttempt_FailOutcomeWeight_SocialFight = 0.04f;

		// Token: 0x040030CD RID: 12493
		public const float ConvertAttempt_BaseCertaintyReduction = 0.06f;

		// Token: 0x040030CE RID: 12494
		public static readonly SimpleCurve CertaintyPerDayByMoodCurve = new SimpleCurve
		{
			{
				new CurvePoint(0.2f, 0.01f),
				true
			},
			{
				new CurvePoint(0.5f, 0.02f),
				true
			},
			{
				new CurvePoint(0.8f, 0.03f),
				true
			}
		};

		// Token: 0x040030CF RID: 12495
		public const float PostConversionCertainty = 0.5f;

		// Token: 0x040030D0 RID: 12496
		public static readonly FloatRange InitialCertaintyRange = new FloatRange(0.6f, 1f);
	}
}
