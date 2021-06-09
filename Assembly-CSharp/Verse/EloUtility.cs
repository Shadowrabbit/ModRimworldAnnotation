using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200081A RID: 2074
	public static class EloUtility
	{
		// Token: 0x06003409 RID: 13321 RVA: 0x00028C5D File Offset: 0x00026E5D
		public static void Update(ref float teamA, ref float teamB, float expectedA, float scoreA, float kfactor = 32f)
		{
			teamA += kfactor * (scoreA - expectedA);
			teamB += kfactor * (expectedA - scoreA);
		}

		// Token: 0x0600340A RID: 13322 RVA: 0x00151B5C File Offset: 0x0014FD5C
		public static float CalculateExpectation(float teamA, float teamB)
		{
			float num = Mathf.Pow(10f, teamA / 400f) + Mathf.Pow(10f, teamB / 400f);
			return Mathf.Pow(10f, teamA / 400f) / num;
		}

		// Token: 0x0600340B RID: 13323 RVA: 0x00028C75 File Offset: 0x00026E75
		public static float CalculateLinearScore(float teamRating, float referenceRating, float referenceScore)
		{
			return referenceScore * Mathf.Pow(10f, (teamRating - referenceRating) / 400f);
		}

		// Token: 0x0600340C RID: 13324 RVA: 0x00028C8C File Offset: 0x00026E8C
		public static float CalculateRating(float teamScore, float referenceRating, float referenceScore)
		{
			return referenceRating + Mathf.Log(teamScore / referenceScore, 10f) * 400f;
		}

		// Token: 0x0400240A RID: 9226
		private const float TenFactorRating = 400f;
	}
}
