using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E17 RID: 3607
	public class PawnRelationWorker_Sibling : PawnRelationWorker
	{
		// Token: 0x06005356 RID: 21334 RVA: 0x001C32FB File Offset: 0x001C14FB
		public override bool InRelation(Pawn me, Pawn other)
		{
			return me != other && (me.GetMother() != null && me.GetFather() != null && me.GetMother() == other.GetMother() && me.GetFather() == other.GetFather());
		}

		// Token: 0x06005357 RID: 21335 RVA: 0x001C3334 File Offset: 0x001C1534
		public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			float num = 1f;
			float num2 = 1f;
			if (other.GetFather() != null || other.GetMother() != null)
			{
				num = ChildRelationUtility.ChanceOfBecomingChildOf(generated, other.GetFather(), other.GetMother(), new PawnGenerationRequest?(request), null, null);
			}
			else if (request.FixedMelanin != null)
			{
				num2 = ChildRelationUtility.GetMelaninSimilarityFactor(request.FixedMelanin.Value, other.story.melanin);
			}
			else
			{
				num2 = PawnSkinColors.GetMelaninCommonalityFactor(other.story.melanin);
			}
			float num3 = Mathf.Abs(generated.ageTracker.AgeChronologicalYearsFloat - other.ageTracker.AgeChronologicalYearsFloat);
			float num4 = 1f;
			if (num3 > 40f)
			{
				num4 = 0.2f;
			}
			else if (num3 > 10f)
			{
				num4 = 0.65f;
			}
			return num * num2 * num4 * base.BaseGenerationChanceFactor(generated, other, request);
		}

		// Token: 0x06005358 RID: 21336 RVA: 0x001C3424 File Offset: 0x001C1624
		public override void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			bool flag = other.GetMother() != null;
			bool flag2 = other.GetFather() != null;
			bool flag3 = Rand.Value < 0.85f;
			if (flag && LovePartnerRelationUtility.HasAnyLovePartner(other.GetMother(), true))
			{
				flag3 = false;
			}
			if (flag2 && LovePartnerRelationUtility.HasAnyLovePartner(other.GetFather(), true))
			{
				flag3 = false;
			}
			if (!flag)
			{
				Pawn newMother = PawnRelationWorker_Sibling.GenerateParent(generated, other, Gender.Female, request, flag3);
				other.SetMother(newMother);
			}
			generated.SetMother(other.GetMother());
			if (!flag2)
			{
				Pawn newFather = PawnRelationWorker_Sibling.GenerateParent(generated, other, Gender.Male, request, flag3);
				other.SetFather(newFather);
			}
			generated.SetFather(other.GetFather());
			if (!flag || !flag2)
			{
				if (other.GetMother().story.traits.HasTrait(TraitDefOf.Gay) || other.GetFather().story.traits.HasTrait(TraitDefOf.Gay))
				{
					other.GetFather().relations.AddDirectRelation(PawnRelationDefOf.ExLover, other.GetMother());
				}
				else if (flag3)
				{
					Pawn mother = other.GetMother();
					Pawn father = other.GetFather();
					NameTriple nameTriple = mother.Name as NameTriple;
					father.relations.AddDirectRelation(PawnRelationDefOf.Spouse, mother);
					if (nameTriple != null)
					{
						PawnGenerationRequest pawnGenerationRequest = default(PawnGenerationRequest);
						SpouseRelationUtility.ResolveNameForSpouseOnGeneration(ref pawnGenerationRequest, mother);
						string b = nameTriple.Last;
						string text = null;
						if (pawnGenerationRequest.FixedLastName != null)
						{
							b = pawnGenerationRequest.FixedLastName;
						}
						if (pawnGenerationRequest.FixedBirthName != null)
						{
							text = pawnGenerationRequest.FixedBirthName;
						}
						if (mother.story != null && (nameTriple.Last != b || mother.story.birthLastName != text))
						{
							mother.story.birthLastName = text;
						}
					}
				}
				else
				{
					LovePartnerRelationUtility.GiveRandomExLoverOrExSpouseRelation(other.GetFather(), other.GetMother());
				}
			}
			PawnRelationWorker_Sibling.ResolveMyName(ref request, generated);
			PawnRelationWorker_Sibling.ResolveMySkinColor(ref request, generated);
		}

		// Token: 0x06005359 RID: 21337 RVA: 0x001C3604 File Offset: 0x001C1804
		private static Pawn GenerateParent(Pawn generatedChild, Pawn existingChild, Gender genderToGenerate, PawnGenerationRequest childRequest, bool newlyGeneratedParentsWillBeSpousesIfNotGay)
		{
			float ageChronologicalYearsFloat = generatedChild.ageTracker.AgeChronologicalYearsFloat;
			float ageChronologicalYearsFloat2 = existingChild.ageTracker.AgeChronologicalYearsFloat;
			float num = (genderToGenerate == Gender.Male) ? 14f : 16f;
			float num2 = (genderToGenerate == Gender.Male) ? 50f : 45f;
			float num3 = (genderToGenerate == Gender.Male) ? 30f : 27f;
			float num4 = Mathf.Max(ageChronologicalYearsFloat, ageChronologicalYearsFloat2) + num;
			float maxChronologicalAge = num4 + (num2 - num);
			float midChronologicalAge = num4 + (num3 - num);
			float value;
			float value2;
			float value3;
			string fixedLastName;
			PawnRelationWorker_Sibling.GenerateParentParams(num4, maxChronologicalAge, midChronologicalAge, num, generatedChild, existingChild, childRequest, out value, out value2, out value3, out fixedLastName);
			bool allowGay = true;
			Faction faction = existingChild.Faction;
			if (faction == null || faction.IsPlayer)
			{
				bool tryMedievalOrBetter = faction != null && faction.def.techLevel >= TechLevel.Medieval;
				if (!Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction(out faction, tryMedievalOrBetter, true, TechLevel.Undefined, false))
				{
					faction = Faction.OfAncients;
				}
			}
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(existingChild.kindDef, faction, PawnGenerationContext.NonPlayer, -1, true, false, true, true, false, false, 1f, false, allowGay, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, new float?(value), new float?(value2), new Gender?(genderToGenerate), new float?(value3), fixedLastName, null, null, null, false, false));
			if (!Find.WorldPawns.Contains(pawn))
			{
				Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
			}
			return pawn;
		}

		// Token: 0x0600535A RID: 21338 RVA: 0x001C375C File Offset: 0x001C195C
		private static void GenerateParentParams(float minChronologicalAge, float maxChronologicalAge, float midChronologicalAge, float minBioAgeToHaveChildren, Pawn generatedChild, Pawn existingChild, PawnGenerationRequest childRequest, out float biologicalAge, out float chronologicalAge, out float melanin, out string lastName)
		{
			chronologicalAge = Rand.GaussianAsymmetric(midChronologicalAge, (midChronologicalAge - minChronologicalAge) / 2f, (maxChronologicalAge - midChronologicalAge) / 2f);
			chronologicalAge = Mathf.Clamp(chronologicalAge, minChronologicalAge, maxChronologicalAge);
			biologicalAge = Rand.Range(minBioAgeToHaveChildren, Mathf.Min(existingChild.RaceProps.lifeExpectancy, chronologicalAge));
			if (existingChild.GetFather() != null)
			{
				melanin = ParentRelationUtility.GetRandomSecondParentSkinColor(existingChild.GetFather().story.melanin, existingChild.story.melanin, childRequest.FixedMelanin);
			}
			else if (existingChild.GetMother() != null)
			{
				melanin = ParentRelationUtility.GetRandomSecondParentSkinColor(existingChild.GetMother().story.melanin, existingChild.story.melanin, childRequest.FixedMelanin);
			}
			else if (childRequest.FixedMelanin == null)
			{
				melanin = PawnSkinColors.GetRandomMelaninSimilarTo(existingChild.story.melanin, 0f, 1f);
			}
			else
			{
				float num = Mathf.Min(childRequest.FixedMelanin.Value, existingChild.story.melanin);
				float num2 = Mathf.Max(childRequest.FixedMelanin.Value, existingChild.story.melanin);
				if (Rand.Value < 0.5f)
				{
					melanin = PawnSkinColors.GetRandomMelaninSimilarTo(num, 0f, num);
				}
				else
				{
					melanin = PawnSkinColors.GetRandomMelaninSimilarTo(num2, num2, 1f);
				}
			}
			lastName = null;
			NameTriple nameTriple;
			if (!ChildRelationUtility.DefinitelyHasNotBirthName(existingChild) && (nameTriple = (existingChild.Name as NameTriple)) != null && ChildRelationUtility.ChildWantsNameOfAnyParent(existingChild))
			{
				if (existingChild.GetMother() == null && existingChild.GetFather() == null)
				{
					if (Rand.Value < 0.5f)
					{
						lastName = nameTriple.Last;
						return;
					}
				}
				else
				{
					string last = nameTriple.Last;
					string b = null;
					NameTriple nameTriple2;
					NameTriple nameTriple3;
					if (existingChild.GetMother() != null && (nameTriple2 = (existingChild.GetMother().Name as NameTriple)) != null)
					{
						b = nameTriple2.Last;
					}
					else if (existingChild.GetFather() != null && (nameTriple3 = (existingChild.GetFather().Name as NameTriple)) != null)
					{
						b = nameTriple3.Last;
					}
					if (last != b)
					{
						lastName = last;
					}
				}
			}
		}

		// Token: 0x0600535B RID: 21339 RVA: 0x001C3988 File Offset: 0x001C1B88
		private static void ResolveMyName(ref PawnGenerationRequest request, Pawn generated)
		{
			if (request.FixedLastName != null)
			{
				return;
			}
			if (ChildRelationUtility.ChildWantsNameOfAnyParent(generated))
			{
				NameTriple nameTriple;
				if (Rand.Value < 0.5f && (nameTriple = (generated.GetFather().Name as NameTriple)) != null)
				{
					request.SetFixedLastName(nameTriple.Last);
					return;
				}
				NameTriple nameTriple2;
				if ((nameTriple2 = (generated.GetMother().Name as NameTriple)) != null)
				{
					request.SetFixedLastName(nameTriple2.Last);
				}
			}
		}

		// Token: 0x0600535C RID: 21340 RVA: 0x001C39F4 File Offset: 0x001C1BF4
		private static void ResolveMySkinColor(ref PawnGenerationRequest request, Pawn generated)
		{
			if (request.FixedMelanin != null)
			{
				return;
			}
			request.SetFixedMelanin(ChildRelationUtility.GetRandomChildSkinColor(generated.GetFather().story.melanin, generated.GetMother().story.melanin));
		}
	}
}
