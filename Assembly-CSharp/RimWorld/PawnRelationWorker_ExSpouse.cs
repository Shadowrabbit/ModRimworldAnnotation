using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001499 RID: 5273
	public class PawnRelationWorker_ExSpouse : PawnRelationWorker
	{
		// Token: 0x060071B4 RID: 29108 RVA: 0x0004C73A File Offset: 0x0004A93A
		public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			return LovePartnerRelationUtility.LovePartnerRelationGenerationChance(generated, other, request, true) * base.BaseGenerationChanceFactor(generated, other, request);
		}

		// Token: 0x060071B5 RID: 29109 RVA: 0x0004C77C File Offset: 0x0004A97C
		public override void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			generated.relations.AddDirectRelation(PawnRelationDefOf.ExSpouse, other);
			LovePartnerRelationUtility.TryToShareChildrenForGeneratedLovePartner(generated, other, request, 1f);
			PawnRelationWorker_ExSpouse.ResolveMySkinColor(ref request, generated, other);
		}

		// Token: 0x060071B6 RID: 29110 RVA: 0x0022D188 File Offset: 0x0022B388
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
