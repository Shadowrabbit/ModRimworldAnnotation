using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200149A RID: 5274
	public class PawnRelationWorker_Fiance : PawnRelationWorker
	{
		// Token: 0x060071B8 RID: 29112 RVA: 0x0022D1C8 File Offset: 0x0022B3C8
		public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			float num = 1f;
			num *= this.GetOldAgeFactor(generated);
			num *= this.GetOldAgeFactor(other);
			return LovePartnerRelationUtility.LovePartnerRelationGenerationChance(generated, other, request, false) * base.BaseGenerationChanceFactor(generated, other, request) * num;
		}

		// Token: 0x060071B9 RID: 29113 RVA: 0x0004C7A9 File Offset: 0x0004A9A9
		public override void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			generated.relations.AddDirectRelation(PawnRelationDefOf.Fiance, other);
			LovePartnerRelationUtility.TryToShareChildrenForGeneratedLovePartner(generated, other, request, 0.7f);
			PawnRelationWorker_Fiance.ResolveMySkinColor(ref request, generated, other);
		}

		// Token: 0x060071BA RID: 29114 RVA: 0x0004C7D6 File Offset: 0x0004A9D6
		private float GetOldAgeFactor(Pawn pawn)
		{
			return Mathf.Clamp(GenMath.LerpDouble(50f, 80f, 1f, 0.01f, (float)pawn.ageTracker.AgeBiologicalYears), 0.01f, 1f);
		}

		// Token: 0x060071BB RID: 29115 RVA: 0x0022D204 File Offset: 0x0022B404
		public override void OnRelationCreated(Pawn firstPawn, Pawn secondPawn)
		{
			firstPawn.relations.nextMarriageNameChange = (secondPawn.relations.nextMarriageNameChange = SpouseRelationUtility.Roll_NameChangeOnMarriage());
		}

		// Token: 0x060071BC RID: 29116 RVA: 0x0022D188 File Offset: 0x0022B388
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
