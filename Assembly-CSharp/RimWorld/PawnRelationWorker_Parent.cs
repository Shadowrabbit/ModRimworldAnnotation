using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014A5 RID: 5285
	public class PawnRelationWorker_Parent : PawnRelationWorker
	{
		// Token: 0x060071D4 RID: 29140 RVA: 0x0022D4BC File Offset: 0x0022B6BC
		public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			float num = 0f;
			if (other.gender == Gender.Male)
			{
				num = ChildRelationUtility.ChanceOfBecomingChildOf(generated, other, other.GetSpouseOppositeGender(), new PawnGenerationRequest?(request), null, null);
			}
			else if (other.gender == Gender.Female)
			{
				num = ChildRelationUtility.ChanceOfBecomingChildOf(generated, other.GetSpouseOppositeGender(), other, new PawnGenerationRequest?(request), null, null);
			}
			return num * base.BaseGenerationChanceFactor(generated, other, request);
		}

		// Token: 0x060071D5 RID: 29141 RVA: 0x0022D53C File Offset: 0x0022B73C
		public override void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			if (other.gender == Gender.Male)
			{
				generated.SetFather(other);
				Pawn spouseOppositeGender = other.GetSpouseOppositeGender();
				if (spouseOppositeGender != null)
				{
					generated.SetMother(spouseOppositeGender);
				}
				PawnRelationWorker_Parent.ResolveMyName(ref request, generated);
				PawnRelationWorker_Parent.ResolveMySkinColor(ref request, generated);
				return;
			}
			if (other.gender == Gender.Female)
			{
				generated.SetMother(other);
				Pawn spouseOppositeGender2 = other.GetSpouseOppositeGender();
				if (spouseOppositeGender2 != null)
				{
					generated.SetFather(spouseOppositeGender2);
				}
				PawnRelationWorker_Parent.ResolveMyName(ref request, generated);
				PawnRelationWorker_Parent.ResolveMySkinColor(ref request, generated);
			}
		}

		// Token: 0x060071D6 RID: 29142 RVA: 0x0022D5A8 File Offset: 0x0022B7A8
		private static void ResolveMyName(ref PawnGenerationRequest request, Pawn generatedChild)
		{
			if (request.FixedLastName != null)
			{
				return;
			}
			if (ChildRelationUtility.ChildWantsNameOfAnyParent(generatedChild))
			{
				bool flag = Rand.Value < 0.5f || generatedChild.GetMother() == null;
				if (generatedChild.GetFather() == null)
				{
					flag = false;
				}
				if (flag)
				{
					request.SetFixedLastName(((NameTriple)generatedChild.GetFather().Name).Last);
					return;
				}
				request.SetFixedLastName(((NameTriple)generatedChild.GetMother().Name).Last);
			}
		}

		// Token: 0x060071D7 RID: 29143 RVA: 0x0022D624 File Offset: 0x0022B824
		private static void ResolveMySkinColor(ref PawnGenerationRequest request, Pawn generatedChild)
		{
			if (request.FixedMelanin != null)
			{
				return;
			}
			if (generatedChild.GetFather() != null && generatedChild.GetMother() != null)
			{
				request.SetFixedMelanin(ChildRelationUtility.GetRandomChildSkinColor(generatedChild.GetFather().story.melanin, generatedChild.GetMother().story.melanin));
				return;
			}
			if (generatedChild.GetFather() != null)
			{
				request.SetFixedMelanin(PawnSkinColors.GetRandomMelaninSimilarTo(generatedChild.GetFather().story.melanin, 0f, 1f));
				return;
			}
			request.SetFixedMelanin(PawnSkinColors.GetRandomMelaninSimilarTo(generatedChild.GetMother().story.melanin, 0f, 1f));
		}
	}
}
