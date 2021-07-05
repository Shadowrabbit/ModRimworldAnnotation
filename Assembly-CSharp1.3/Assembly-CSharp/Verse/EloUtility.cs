using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004A7 RID: 1191
	public static class EloUtility
	{
		// Token: 0x0600240C RID: 9228 RVA: 0x000E05CD File Offset: 0x000DE7CD
		public static void Update(ref float teamA, ref float teamB, float expectedA, float scoreA, float kfactor = 32f)
		{
			teamA += kfactor * (scoreA - expectedA);
			teamB += kfactor * (expectedA - scoreA);
		}

		// Token: 0x0600240D RID: 9229 RVA: 0x000E05E8 File Offset: 0x000DE7E8
		public static float CalculateExpectation(float teamA, float teamB)
		{
			float num = Mathf.Pow(10f, teamA / 400f) + Mathf.Pow(10f, teamB / 400f);
			return Mathf.Pow(10f, teamA / 400f) / num;
		}

		// Token: 0x0600240E RID: 9230 RVA: 0x000E062C File Offset: 0x000DE82C
		public static float CalculateLinearScore(float teamRating, float referenceRating, float referenceScore)
		{
			return referenceScore * Mathf.Pow(10f, (teamRating - referenceRating) / 400f);
		}

		// Token: 0x0600240F RID: 9231 RVA: 0x000E0643 File Offset: 0x000DE843
		public static float CalculateRating(float teamScore, float referenceRating, float referenceScore)
		{
			return referenceRating + Mathf.Log(teamScore / referenceScore, 10f) * 400f;
		}

		// Token: 0x040016B8 RID: 5816
		private const float TenFactorRating = 400f;
	}
}
