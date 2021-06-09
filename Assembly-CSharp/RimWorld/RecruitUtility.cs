using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CDC RID: 7388
	public static class RecruitUtility
	{
		// Token: 0x0600A085 RID: 41093 RVA: 0x002F0030 File Offset: 0x002EE230
		public static float RecruitChanceFactorForMood(Pawn recruitee)
		{
			if (recruitee.needs.mood == null)
			{
				return 1f;
			}
			float curLevel = recruitee.needs.mood.CurLevel;
			return RecruitUtility.RecruitChanceFactorCurve_Mood.Evaluate(curLevel);
		}

		// Token: 0x0600A086 RID: 41094 RVA: 0x002F006C File Offset: 0x002EE26C
		public static float RecruitChanceFactorForOpinion(Pawn recruiter, Pawn recruitee)
		{
			if (recruitee.relations == null)
			{
				return 1f;
			}
			float x = (float)recruitee.relations.OpinionOf(recruiter);
			return RecruitUtility.RecruitChanceFactorCurve_Opinion.Evaluate(x);
		}

		// Token: 0x0600A087 RID: 41095 RVA: 0x0006AF6B File Offset: 0x0006916B
		public static float RecruitChanceFactorForRecruiterNegotiationAbility(Pawn recruiter)
		{
			return recruiter.GetStatValue(StatDefOf.NegotiationAbility, true) * 0.5f;
		}

		// Token: 0x0600A088 RID: 41096 RVA: 0x0006AF7F File Offset: 0x0006917F
		public static float RecruitChanceFactorForRecruiter(Pawn recruiter, Pawn recruitee)
		{
			return RecruitUtility.RecruitChanceFactorForRecruiterNegotiationAbility(recruiter) * RecruitUtility.RecruitChanceFactorForOpinion(recruiter, recruitee);
		}

		// Token: 0x0600A089 RID: 41097 RVA: 0x002F00A0 File Offset: 0x002EE2A0
		public static float RecruitChanceFactorForRecruitDifficulty(Pawn recruitee, Faction recruiterFaction)
		{
			float x = recruitee.RecruitDifficulty(recruiterFaction);
			return RecruitUtility.RecruitChanceFactorCurve_RecruitDifficulty.Evaluate(x);
		}

		// Token: 0x0600A08A RID: 41098 RVA: 0x0006AF8F File Offset: 0x0006918F
		public static float RecruitChanceFinalByFaction(this Pawn recruitee, Faction recruiterFaction)
		{
			return Mathf.Clamp01(RecruitUtility.RecruitChanceFactorForRecruitDifficulty(recruitee, recruiterFaction) * RecruitUtility.RecruitChanceFactorForMood(recruitee));
		}

		// Token: 0x0600A08B RID: 41099 RVA: 0x0006AFA4 File Offset: 0x000691A4
		public static float RecruitChanceFinalByPawn(this Pawn recruitee, Pawn recruiter)
		{
			return Mathf.Clamp01(recruitee.RecruitChanceFinalByFaction(recruiter.Faction) * RecruitUtility.RecruitChanceFactorForRecruiter(recruiter, recruitee));
		}

		// Token: 0x04006D0E RID: 27918
		private static readonly SimpleCurve RecruitChanceFactorCurve_Mood = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.2f),
				true
			},
			{
				new CurvePoint(0.5f, 1f),
				true
			},
			{
				new CurvePoint(1f, 2f),
				true
			}
		};

		// Token: 0x04006D0F RID: 27919
		private static readonly SimpleCurve RecruitChanceFactorCurve_Opinion = new SimpleCurve
		{
			{
				new CurvePoint(-100f, 0.5f),
				true
			},
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(100f, 2f),
				true
			}
		};

		// Token: 0x04006D10 RID: 27920
		private static readonly SimpleCurve RecruitChanceFactorCurve_RecruitDifficulty = new SimpleCurve
		{
			{
				new CurvePoint(0f, 2f),
				true
			},
			{
				new CurvePoint(0.5f, 1f),
				true
			},
			{
				new CurvePoint(1f, 0.02f),
				true
			}
		};

		// Token: 0x04006D11 RID: 27921
		private const float RecruitChancePerNegotiatingAbility = 0.5f;
	}
}
