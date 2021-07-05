using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E08 RID: 3592
	public class PawnRelationWorker_ExSpouse : PawnRelationWorker
	{
		// Token: 0x0600532D RID: 21293 RVA: 0x001C2A6A File Offset: 0x001C0C6A
		public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			return LovePartnerRelationUtility.LovePartnerRelationGenerationChance(generated, other, request, true) * base.BaseGenerationChanceFactor(generated, other, request);
		}

		// Token: 0x0600532E RID: 21294 RVA: 0x001C2AEA File Offset: 0x001C0CEA
		public override void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			generated.relations.AddDirectRelation(PawnRelationDefOf.ExSpouse, other);
			LovePartnerRelationUtility.TryToShareChildrenForGeneratedLovePartner(generated, other, request, 1f);
			PawnRelationWorker_ExSpouse.ResolveMySkinColor(ref request, generated, other);
		}

		// Token: 0x0600532F RID: 21295 RVA: 0x001C2B18 File Offset: 0x001C0D18
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
