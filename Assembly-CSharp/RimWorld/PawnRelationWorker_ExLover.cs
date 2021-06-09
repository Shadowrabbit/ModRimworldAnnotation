using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001498 RID: 5272
	public class PawnRelationWorker_ExLover : PawnRelationWorker
	{
		// Token: 0x060071B0 RID: 29104 RVA: 0x0004C73A File Offset: 0x0004A93A
		public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			return LovePartnerRelationUtility.LovePartnerRelationGenerationChance(generated, other, request, true) * base.BaseGenerationChanceFactor(generated, other, request);
		}

		// Token: 0x060071B1 RID: 29105 RVA: 0x0004C74F File Offset: 0x0004A94F
		public override void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			generated.relations.AddDirectRelation(PawnRelationDefOf.ExLover, other);
			LovePartnerRelationUtility.TryToShareChildrenForGeneratedLovePartner(generated, other, request, 0.35f);
			PawnRelationWorker_ExLover.ResolveMySkinColor(ref request, generated, other);
		}

		// Token: 0x060071B2 RID: 29106 RVA: 0x0022D188 File Offset: 0x0022B388
		private static void ResolveMySkinColor(ref PawnGenerationRequest request, Pawn generated, Pawn other)
		{
			if (request.FixedMelanin != null)
			{
				return;
			}
			request.SetFixedMelanin(PawnSkinColors.GetRandomMelaninSimilarTo(other.story.melanin, 0f, 1f));
		}
	}
}
