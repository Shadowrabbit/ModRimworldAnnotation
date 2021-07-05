using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E12 RID: 3602
	public class PawnRelationWorker_Lover : PawnRelationWorker
	{
		// Token: 0x06005347 RID: 21319 RVA: 0x001C2EB4 File Offset: 0x001C10B4
		public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			return LovePartnerRelationUtility.LovePartnerRelationGenerationChance(generated, other, request, false) * base.BaseGenerationChanceFactor(generated, other, request);
		}

		// Token: 0x06005348 RID: 21320 RVA: 0x001C2EC9 File Offset: 0x001C10C9
		public override void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			generated.relations.AddDirectRelation(PawnRelationDefOf.Lover, other);
			LovePartnerRelationUtility.TryToShareChildrenForGeneratedLovePartner(generated, other, request, 0.35f);
			PawnRelationWorker_Lover.ResolveMySkinColor(ref request, generated, other);
		}

		// Token: 0x06005349 RID: 21321 RVA: 0x001C2EF8 File Offset: 0x001C10F8
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
