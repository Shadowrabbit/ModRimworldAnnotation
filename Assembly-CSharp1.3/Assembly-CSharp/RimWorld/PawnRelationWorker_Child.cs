using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E03 RID: 3587
	public class PawnRelationWorker_Child : PawnRelationWorker
	{
		// Token: 0x0600531D RID: 21277 RVA: 0x001C25F9 File Offset: 0x001C07F9
		public override bool InRelation(Pawn me, Pawn other)
		{
			return me != other && (other.GetMother() == me || other.GetFather() == me);
		}

		// Token: 0x0600531E RID: 21278 RVA: 0x001C2618 File Offset: 0x001C0818
		public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			float num = 0f;
			if (generated.gender == Gender.Male)
			{
				num = ChildRelationUtility.ChanceOfBecomingChildOf(other, generated, other.GetMother(), null, new PawnGenerationRequest?(request), null);
			}
			else if (generated.gender == Gender.Female)
			{
				num = ChildRelationUtility.ChanceOfBecomingChildOf(other, other.GetFather(), generated, null, null, new PawnGenerationRequest?(request));
			}
			return num * base.BaseGenerationChanceFactor(generated, other, request);
		}

		// Token: 0x0600531F RID: 21279 RVA: 0x001C2698 File Offset: 0x001C0898
		public override void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			if (generated.gender == Gender.Male)
			{
				other.SetFather(generated);
				PawnRelationWorker_Child.ResolveMyName(ref request, other, other.GetMother());
				PawnRelationWorker_Child.ResolveMySkinColor(ref request, other, other.GetMother());
				if (other.GetMother() != null)
				{
					if (other.GetMother().story.traits.HasTrait(TraitDefOf.Gay))
					{
						generated.relations.AddDirectRelation(PawnRelationDefOf.ExLover, other.GetMother());
						return;
					}
					if (Rand.Value < 0.85f && !LovePartnerRelationUtility.HasAnyLovePartner(other.GetMother(), true))
					{
						generated.relations.AddDirectRelation(PawnRelationDefOf.Spouse, other.GetMother());
						SpouseRelationUtility.ResolveNameForSpouseOnGeneration(ref request, generated);
						return;
					}
					LovePartnerRelationUtility.GiveRandomExLoverOrExSpouseRelation(generated, other.GetMother());
					return;
				}
			}
			else if (generated.gender == Gender.Female)
			{
				other.SetMother(generated);
				PawnRelationWorker_Child.ResolveMyName(ref request, other, other.GetFather());
				PawnRelationWorker_Child.ResolveMySkinColor(ref request, other, other.GetFather());
				if (other.GetFather() != null)
				{
					if (other.GetFather().story.traits.HasTrait(TraitDefOf.Gay))
					{
						generated.relations.AddDirectRelation(PawnRelationDefOf.ExLover, other.GetFather());
						return;
					}
					if (Rand.Value < 0.85f && !LovePartnerRelationUtility.HasAnyLovePartner(other.GetFather(), true))
					{
						generated.relations.AddDirectRelation(PawnRelationDefOf.Spouse, other.GetFather());
						SpouseRelationUtility.ResolveNameForSpouseOnGeneration(ref request, generated);
						return;
					}
					LovePartnerRelationUtility.GiveRandomExLoverOrExSpouseRelation(generated, other.GetFather());
				}
			}
		}

		// Token: 0x06005320 RID: 21280 RVA: 0x001C2804 File Offset: 0x001C0A04
		private static void ResolveMyName(ref PawnGenerationRequest request, Pawn child, Pawn otherParent)
		{
			if (request.FixedLastName != null)
			{
				return;
			}
			if (ChildRelationUtility.DefinitelyHasNotBirthName(child))
			{
				return;
			}
			if (ChildRelationUtility.ChildWantsNameOfAnyParent(child))
			{
				if (otherParent == null || !(otherParent.Name is NameTriple))
				{
					float num = 0.875f;
					if (Rand.Value < num)
					{
						request.SetFixedLastName(((NameTriple)child.Name).Last);
						return;
					}
				}
				else
				{
					string last = ((NameTriple)child.Name).Last;
					string last2 = ((NameTriple)otherParent.Name).Last;
					if (last != last2)
					{
						request.SetFixedLastName(last);
					}
				}
			}
		}

		// Token: 0x06005321 RID: 21281 RVA: 0x001C2894 File Offset: 0x001C0A94
		private static void ResolveMySkinColor(ref PawnGenerationRequest request, Pawn child, Pawn otherParent)
		{
			if (request.FixedMelanin != null)
			{
				return;
			}
			if (otherParent != null)
			{
				request.SetFixedMelanin(ParentRelationUtility.GetRandomSecondParentSkinColor(otherParent.story.melanin, child.story.melanin, null));
				return;
			}
			request.SetFixedMelanin(PawnSkinColors.GetRandomMelaninSimilarTo(child.story.melanin, 0f, 1f));
		}
	}
}
