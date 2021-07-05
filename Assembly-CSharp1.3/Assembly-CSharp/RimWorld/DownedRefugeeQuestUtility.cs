using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B0A RID: 2826
	public static class DownedRefugeeQuestUtility
	{
		// Token: 0x0600425E RID: 16990 RVA: 0x0016377C File Offset: 0x0016197C
		public static Pawn GenerateRefugee(int tile, PawnKindDef pawnKind = null, float chanceForFaction = 0.6f)
		{
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(pawnKind ?? PawnKindDefOf.SpaceRefugee, DownedRefugeeQuestUtility.GetRandomFactionForRefugee(chanceForFaction), PawnGenerationContext.NonPlayer, tile, false, false, false, false, true, false, 20f, true, true, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, new float?(0.2f), null, null, null, null, null, null, null, null, false, false));
			HealthUtility.DamageUntilDowned(pawn, false);
			HealthUtility.DamageLegsUntilIncapableOfMoving(pawn, false);
			return pawn;
		}

		// Token: 0x0600425F RID: 16991 RVA: 0x00163810 File Offset: 0x00161A10
		public static Faction GetRandomFactionForRefugee(float chanceForFaction = 0.6f)
		{
			Faction result;
			if (Rand.Chance(chanceForFaction) && Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction(out result, true, false, TechLevel.Undefined, false))
			{
				return result;
			}
			return null;
		}

		// Token: 0x04002866 RID: 10342
		private const float RelationWithColonistWeight = 20f;

		// Token: 0x04002867 RID: 10343
		private const float ChanceToRedressWorldPawn = 0.2f;
	}
}
