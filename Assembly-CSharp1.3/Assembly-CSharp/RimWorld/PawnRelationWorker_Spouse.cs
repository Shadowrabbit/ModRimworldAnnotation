using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E18 RID: 3608
	public class PawnRelationWorker_Spouse : PawnRelationWorker
	{
		// Token: 0x0600535E RID: 21342 RVA: 0x001C2EB4 File Offset: 0x001C10B4
		public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			return LovePartnerRelationUtility.LovePartnerRelationGenerationChance(generated, other, request, false) * base.BaseGenerationChanceFactor(generated, other, request);
		}

		// Token: 0x0600535F RID: 21343 RVA: 0x001C3A3D File Offset: 0x001C1C3D
		public override void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			generated.relations.AddDirectRelation(PawnRelationDefOf.Spouse, other);
			LovePartnerRelationUtility.TryToShareChildrenForGeneratedLovePartner(generated, other, request, 1f);
			SpouseRelationUtility.ResolveNameForSpouseOnGeneration(ref request, generated);
			PawnRelationWorker_Spouse.ResolveMySkinColor(ref request, generated, other);
		}

		// Token: 0x06005360 RID: 21344 RVA: 0x001C3A74 File Offset: 0x001C1C74
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
