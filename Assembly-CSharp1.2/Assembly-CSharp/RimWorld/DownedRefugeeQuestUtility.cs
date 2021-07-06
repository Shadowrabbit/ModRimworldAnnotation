using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001038 RID: 4152
	public static class DownedRefugeeQuestUtility
	{
		// Token: 0x06005A70 RID: 23152 RVA: 0x001D5560 File Offset: 0x001D3760
		public static Pawn GenerateRefugee(int tile)
		{
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.SpaceRefugee, DownedRefugeeQuestUtility.GetRandomFactionForRefugee(), PawnGenerationContext.NonPlayer, tile, false, false, false, false, true, false, 20f, true, true, true, true, false, false, false, false, 0f, null, 1f, null, null, null, null, new float?(0.2f), null, null, null, null, null, null, null));
			HealthUtility.DamageUntilDowned(pawn, false);
			HealthUtility.DamageLegsUntilIncapableOfMoving(pawn, false);
			return pawn;
		}

		// Token: 0x06005A71 RID: 23153 RVA: 0x001D55E4 File Offset: 0x001D37E4
		public static Faction GetRandomFactionForRefugee()
		{
			Faction result;
			if (Rand.Chance(0.6f) && Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction_NewTemp(out result, true, false, TechLevel.Undefined, false))
			{
				return result;
			}
			return null;
		}

		// Token: 0x04003CC7 RID: 15559
		private const float RelationWithColonistWeight = 20f;

		// Token: 0x04003CC8 RID: 15560
		private const float ChanceToRedressWorldPawn = 0.2f;
	}
}
