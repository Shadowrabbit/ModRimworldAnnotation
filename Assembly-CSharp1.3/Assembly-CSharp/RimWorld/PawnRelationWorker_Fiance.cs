using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E09 RID: 3593
	public class PawnRelationWorker_Fiance : PawnRelationWorker
	{
		// Token: 0x06005331 RID: 21297 RVA: 0x001C2B58 File Offset: 0x001C0D58
		public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			float num = 1f;
			num *= this.GetOldAgeFactor(generated);
			num *= this.GetOldAgeFactor(other);
			return LovePartnerRelationUtility.LovePartnerRelationGenerationChance(generated, other, request, false) * base.BaseGenerationChanceFactor(generated, other, request) * num;
		}

		// Token: 0x06005332 RID: 21298 RVA: 0x001C2B94 File Offset: 0x001C0D94
		public override void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			generated.relations.AddDirectRelation(PawnRelationDefOf.Fiance, other);
			LovePartnerRelationUtility.TryToShareChildrenForGeneratedLovePartner(generated, other, request, 0.7f);
			PawnRelationWorker_Fiance.ResolveMySkinColor(ref request, generated, other);
		}

		// Token: 0x06005333 RID: 21299 RVA: 0x001C2BC1 File Offset: 0x001C0DC1
		private float GetOldAgeFactor(Pawn pawn)
		{
			return Mathf.Clamp(GenMath.LerpDouble(50f, 80f, 1f, 0.01f, (float)pawn.ageTracker.AgeBiologicalYears), 0.01f, 1f);
		}

		// Token: 0x06005334 RID: 21300 RVA: 0x001C2BF8 File Offset: 0x001C0DF8
		public override void OnRelationCreated(Pawn firstPawn, Pawn secondPawn)
		{
			Pawn pawn;
			if (firstPawn.Ideo == secondPawn.Ideo)
			{
				pawn = firstPawn;
			}
			else
			{
				pawn = ((Rand.Value < 0.5f) ? firstPawn : secondPawn);
			}
			firstPawn.relations.nextMarriageNameChange = (secondPawn.relations.nextMarriageNameChange = SpouseRelationUtility.Roll_NameChangeOnMarriage(pawn));
		}

		// Token: 0x06005335 RID: 21301 RVA: 0x001C2C48 File Offset: 0x001C0E48
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
