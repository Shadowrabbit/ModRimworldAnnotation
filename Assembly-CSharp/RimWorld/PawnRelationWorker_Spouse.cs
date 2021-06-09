using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014A9 RID: 5289
	public class PawnRelationWorker_Spouse : PawnRelationWorker
	{
		// Token: 0x060071E5 RID: 29157 RVA: 0x0004C83E File Offset: 0x0004AA3E
		public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			return LovePartnerRelationUtility.LovePartnerRelationGenerationChance(generated, other, request, false) * base.BaseGenerationChanceFactor(generated, other, request);
		}

		// Token: 0x060071E6 RID: 29158 RVA: 0x0004C8B7 File Offset: 0x0004AAB7
		public override void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			generated.relations.AddDirectRelation(PawnRelationDefOf.Spouse, other);
			LovePartnerRelationUtility.TryToShareChildrenForGeneratedLovePartner(generated, other, request, 1f);
			SpouseRelationUtility.ResolveNameForSpouseOnGeneration(ref request, generated);
			PawnRelationWorker_Spouse.ResolveMySkinColor(ref request, generated, other);
		}

		// Token: 0x060071E7 RID: 29159 RVA: 0x0022D188 File Offset: 0x0022B388
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
