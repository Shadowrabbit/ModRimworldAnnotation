using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E14 RID: 3604
	public class PawnRelationWorker_Parent : PawnRelationWorker
	{
		// Token: 0x0600534D RID: 21325 RVA: 0x001C2FC8 File Offset: 0x001C11C8
		public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			float num = 0f;
			if (other.gender == Gender.Male)
			{
				num = ChildRelationUtility.ChanceOfBecomingChildOf(generated, other, other.GetFirstSpouseOfOppositeGender(), new PawnGenerationRequest?(request), null, null);
			}
			else if (other.gender == Gender.Female)
			{
				num = ChildRelationUtility.ChanceOfBecomingChildOf(generated, other.GetFirstSpouseOfOppositeGender(), other, new PawnGenerationRequest?(request), null, null);
			}
			return num * base.BaseGenerationChanceFactor(generated, other, request);
		}

		// Token: 0x0600534E RID: 21326 RVA: 0x001C3048 File Offset: 0x001C1248
		public override void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			if (other.gender == Gender.Male)
			{
				generated.SetFather(other);
				Pawn firstSpouseOfOppositeGender = other.GetFirstSpouseOfOppositeGender();
				if (firstSpouseOfOppositeGender != null)
				{
					generated.SetMother(firstSpouseOfOppositeGender);
				}
				PawnRelationWorker_Parent.ResolveMyName(ref request, generated);
				PawnRelationWorker_Parent.ResolveMySkinColor(ref request, generated);
				return;
			}
			if (other.gender == Gender.Female)
			{
				generated.SetMother(other);
				Pawn firstSpouseOfOppositeGender2 = other.GetFirstSpouseOfOppositeGender();
				if (firstSpouseOfOppositeGender2 != null)
				{
					generated.SetFather(firstSpouseOfOppositeGender2);
				}
				PawnRelationWorker_Parent.ResolveMyName(ref request, generated);
				PawnRelationWorker_Parent.ResolveMySkinColor(ref request, generated);
			}
		}

		// Token: 0x0600534F RID: 21327 RVA: 0x001C30B4 File Offset: 0x001C12B4
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

		// Token: 0x06005350 RID: 21328 RVA: 0x001C3130 File Offset: 0x001C1330
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
