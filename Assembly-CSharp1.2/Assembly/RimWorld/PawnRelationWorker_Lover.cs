using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014A3 RID: 5283
	public class PawnRelationWorker_Lover : PawnRelationWorker
	{
		// Token: 0x060071CE RID: 29134 RVA: 0x0004C83E File Offset: 0x0004AA3E
		public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			return LovePartnerRelationUtility.LovePartnerRelationGenerationChance(generated, other, request, false) * base.BaseGenerationChanceFactor(generated, other, request);
		}

		// Token: 0x060071CF RID: 29135 RVA: 0x0004C853 File Offset: 0x0004AA53
		public override void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			generated.relations.AddDirectRelation(PawnRelationDefOf.Lover, other);
			LovePartnerRelationUtility.TryToShareChildrenForGeneratedLovePartner(generated, other, request, 0.35f);
			PawnRelationWorker_Lover.ResolveMySkinColor(ref request, generated, other);
		}

		// Token: 0x060071D0 RID: 29136 RVA: 0x0022D188 File Offset: 0x0022B388
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
