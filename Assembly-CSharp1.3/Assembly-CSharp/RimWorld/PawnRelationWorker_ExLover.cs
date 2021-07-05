using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E07 RID: 3591
	public class PawnRelationWorker_ExLover : PawnRelationWorker
	{
		// Token: 0x06005329 RID: 21289 RVA: 0x001C2A6A File Offset: 0x001C0C6A
		public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			return LovePartnerRelationUtility.LovePartnerRelationGenerationChance(generated, other, request, true) * base.BaseGenerationChanceFactor(generated, other, request);
		}

		// Token: 0x0600532A RID: 21290 RVA: 0x001C2A7F File Offset: 0x001C0C7F
		public override void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			generated.relations.AddDirectRelation(PawnRelationDefOf.ExLover, other);
			LovePartnerRelationUtility.TryToShareChildrenForGeneratedLovePartner(generated, other, request, 0.35f);
			PawnRelationWorker_ExLover.ResolveMySkinColor(ref request, generated, other);
		}

		// Token: 0x0600532B RID: 21291 RVA: 0x001C2AAC File Offset: 0x001C0CAC
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
