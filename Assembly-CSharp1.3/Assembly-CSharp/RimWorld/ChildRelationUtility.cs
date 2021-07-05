using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DEE RID: 3566
	public static class ChildRelationUtility
	{
		// Token: 0x060052AB RID: 21163 RVA: 0x001BE028 File Offset: 0x001BC228
		public static float ChanceOfBecomingChildOf(Pawn child, Pawn father, Pawn mother, PawnGenerationRequest? childGenerationRequest, PawnGenerationRequest? fatherGenerationRequest, PawnGenerationRequest? motherGenerationRequest)
		{
			if (father != null && father.gender != Gender.Male)
			{
				Log.Warning("Tried to calculate chance for father with gender \"" + father.gender + "\".");
				return 0f;
			}
			if (mother != null && mother.gender != Gender.Female)
			{
				Log.Warning("Tried to calculate chance for mother with gender \"" + mother.gender + "\".");
				return 0f;
			}
			if (father != null && child.GetFather() != null && child.GetFather() != father)
			{
				return 0f;
			}
			if (mother != null && child.GetMother() != null && child.GetMother() != mother)
			{
				return 0f;
			}
			if (mother != null && father != null && !LovePartnerRelationUtility.LovePartnerRelationExists(mother, father) && !LovePartnerRelationUtility.ExLovePartnerRelationExists(mother, father))
			{
				return 0f;
			}
			float? melanin = ChildRelationUtility.GetMelanin(child, childGenerationRequest);
			float? melanin2 = ChildRelationUtility.GetMelanin(father, fatherGenerationRequest);
			float? melanin3 = ChildRelationUtility.GetMelanin(mother, motherGenerationRequest);
			bool fatherIsNew = father != null && child.GetFather() != father;
			bool motherIsNew = mother != null && child.GetMother() != mother;
			float skinColorFactor = ChildRelationUtility.GetSkinColorFactor(melanin, melanin2, melanin3, fatherIsNew, motherIsNew);
			if (skinColorFactor <= 0f)
			{
				return 0f;
			}
			float num = 1f;
			float num2 = 1f;
			float num3 = 1f;
			float num4 = 1f;
			if (father != null && child.GetFather() == null)
			{
				num = ChildRelationUtility.GetParentAgeFactor(father, child, 14f, 30f, 50f);
				if (num == 0f)
				{
					return 0f;
				}
				if (father.story.traits.HasTrait(TraitDefOf.Gay))
				{
					num4 = 0.1f;
				}
			}
			if (mother != null && child.GetMother() == null)
			{
				num2 = ChildRelationUtility.GetParentAgeFactor(mother, child, 16f, 27f, 45f);
				if (num2 == 0f)
				{
					return 0f;
				}
				int num5 = ChildRelationUtility.NumberOfChildrenFemaleWantsEver(mother);
				if (mother.relations.ChildrenCount >= num5)
				{
					return 0f;
				}
				num3 = 1f - (float)mother.relations.ChildrenCount / (float)num5;
				if (mother.story.traits.HasTrait(TraitDefOf.Gay))
				{
					num4 = 0.1f;
				}
			}
			float num6 = 1f;
			if (mother != null)
			{
				Pawn firstDirectRelationPawn = mother.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Spouse, null);
				if (firstDirectRelationPawn != null && firstDirectRelationPawn != father)
				{
					num6 *= 0.15f;
				}
			}
			if (father != null)
			{
				Pawn firstDirectRelationPawn2 = father.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Spouse, null);
				if (firstDirectRelationPawn2 != null && firstDirectRelationPawn2 != mother)
				{
					num6 *= 0.15f;
				}
			}
			return skinColorFactor * num * num2 * num3 * num6 * num4;
		}

		// Token: 0x060052AC RID: 21164 RVA: 0x001BE2A0 File Offset: 0x001BC4A0
		private static float GetParentAgeFactor(Pawn parent, Pawn child, float minAgeToHaveChildren, float usualAgeToHaveChildren, float maxAgeToHaveChildren)
		{
			float num = PawnRelationUtility.MaxPossibleBioAgeAt(parent.ageTracker.AgeBiologicalYearsFloat, parent.ageTracker.AgeChronologicalYearsFloat, child.ageTracker.AgeChronologicalYearsFloat);
			float num2 = PawnRelationUtility.MinPossibleBioAgeAt(parent.ageTracker.AgeBiologicalYearsFloat, child.ageTracker.AgeChronologicalYearsFloat);
			if (num <= 0f)
			{
				return 0f;
			}
			if (num2 > num)
			{
				if (num2 > num + 0.1f)
				{
					Log.Warning(string.Concat(new object[]
					{
						"Min possible bio age (",
						num2,
						") is greater than max possible bio age (",
						num,
						")."
					}));
				}
				return 0f;
			}
			if (num2 <= usualAgeToHaveChildren && num >= usualAgeToHaveChildren)
			{
				return 1f;
			}
			float ageFactor = ChildRelationUtility.GetAgeFactor(num2, minAgeToHaveChildren, maxAgeToHaveChildren, usualAgeToHaveChildren);
			float ageFactor2 = ChildRelationUtility.GetAgeFactor(num, minAgeToHaveChildren, maxAgeToHaveChildren, usualAgeToHaveChildren);
			return Mathf.Max(ageFactor, ageFactor2);
		}

		// Token: 0x060052AD RID: 21165 RVA: 0x001BE375 File Offset: 0x001BC575
		public static bool ChildWantsNameOfAnyParent(Pawn child)
		{
			return Rand.ValueSeeded(child.thingIDNumber ^ 88271612) < 0.99f;
		}

		// Token: 0x060052AE RID: 21166 RVA: 0x001BE38F File Offset: 0x001BC58F
		private static int NumberOfChildrenFemaleWantsEver(Pawn female)
		{
			Rand.PushState();
			Rand.Seed = female.thingIDNumber * 3;
			int result = Rand.RangeInclusive(0, 3);
			Rand.PopState();
			return result;
		}

		// Token: 0x060052AF RID: 21167 RVA: 0x001BE3B0 File Offset: 0x001BC5B0
		private static float? GetMelanin(Pawn pawn, PawnGenerationRequest? request)
		{
			if (request != null)
			{
				return request.Value.FixedMelanin;
			}
			if (pawn != null)
			{
				return new float?(pawn.story.melanin);
			}
			return null;
		}

		// Token: 0x060052B0 RID: 21168 RVA: 0x001BE3F3 File Offset: 0x001BC5F3
		private static float GetAgeFactor(float ageAtBirth, float min, float max, float mid)
		{
			return GenMath.GetFactorInInterval(min, mid, max, 1.6f, ageAtBirth);
		}

		// Token: 0x060052B1 RID: 21169 RVA: 0x001BE404 File Offset: 0x001BC604
		private static float GetSkinColorFactor(float? childMelanin, float? fatherMelanin, float? motherMelanin, bool fatherIsNew, bool motherIsNew)
		{
			if (childMelanin != null && fatherMelanin != null && motherMelanin != null)
			{
				float num = Mathf.Min(fatherMelanin.Value, motherMelanin.Value);
				float num2 = Mathf.Max(fatherMelanin.Value, motherMelanin.Value);
				float? num3 = childMelanin;
				float num4 = num - 0.05f;
				if (num3.GetValueOrDefault() < num4 & num3 != null)
				{
					return 0f;
				}
				num3 = childMelanin;
				num4 = num2 + 0.05f;
				if (num3.GetValueOrDefault() > num4 & num3 != null)
				{
					return 0f;
				}
			}
			float num5 = 1f;
			if (fatherIsNew)
			{
				num5 *= ChildRelationUtility.GetNewParentSkinColorFactor(fatherMelanin, motherMelanin, childMelanin);
			}
			if (motherIsNew)
			{
				num5 *= ChildRelationUtility.GetNewParentSkinColorFactor(motherMelanin, fatherMelanin, childMelanin);
			}
			return num5;
		}

		// Token: 0x060052B2 RID: 21170 RVA: 0x001BE4C4 File Offset: 0x001BC6C4
		private static float GetNewParentSkinColorFactor(float? newParentMelanin, float? otherParentMelanin, float? childMelanin)
		{
			if (newParentMelanin != null)
			{
				if (otherParentMelanin == null)
				{
					if (childMelanin != null)
					{
						return ChildRelationUtility.GetMelaninSimilarityFactor(newParentMelanin.Value, childMelanin.Value);
					}
					return PawnSkinColors.GetMelaninCommonalityFactor(newParentMelanin.Value);
				}
				else
				{
					if (childMelanin != null)
					{
						float reflectedSkin = ChildRelationUtility.GetReflectedSkin(otherParentMelanin.Value, childMelanin.Value);
						return ChildRelationUtility.GetMelaninSimilarityFactor(newParentMelanin.Value, reflectedSkin);
					}
					return PawnSkinColors.GetMelaninCommonalityFactor((newParentMelanin.Value + otherParentMelanin.Value) / 2f);
				}
			}
			else if (otherParentMelanin == null)
			{
				if (childMelanin != null)
				{
					return PawnSkinColors.GetMelaninCommonalityFactor(childMelanin.Value);
				}
				return 1f;
			}
			else
			{
				if (childMelanin != null)
				{
					return PawnSkinColors.GetMelaninCommonalityFactor(ChildRelationUtility.GetReflectedSkin(otherParentMelanin.Value, childMelanin.Value));
				}
				return PawnSkinColors.GetMelaninCommonalityFactor(otherParentMelanin.Value);
			}
		}

		// Token: 0x060052B3 RID: 21171 RVA: 0x001BE5A6 File Offset: 0x001BC7A6
		public static float GetReflectedSkin(float value, float mirror)
		{
			return Mathf.Clamp01(GenMath.Reflection(value, mirror));
		}

		// Token: 0x060052B4 RID: 21172 RVA: 0x001BE5B4 File Offset: 0x001BC7B4
		public static float GetMelaninSimilarityFactor(float melanin1, float melanin2)
		{
			float min = Mathf.Clamp01(melanin1 - 0.15f);
			float max = Mathf.Clamp01(melanin1 + 0.15f);
			return GenMath.GetFactorInInterval(min, melanin1, max, 2.5f, melanin2);
		}

		// Token: 0x060052B5 RID: 21173 RVA: 0x001BE5E8 File Offset: 0x001BC7E8
		public static float GetRandomChildSkinColor(float fatherMelanin, float motherMelanin)
		{
			float clampMin = Mathf.Min(fatherMelanin, motherMelanin);
			float clampMax = Mathf.Max(fatherMelanin, motherMelanin);
			return PawnSkinColors.GetRandomMelaninSimilarTo((fatherMelanin + motherMelanin) / 2f, clampMin, clampMax);
		}

		// Token: 0x060052B6 RID: 21174 RVA: 0x001BE618 File Offset: 0x001BC818
		public static bool DefinitelyHasNotBirthName(Pawn pawn)
		{
			NameTriple nameTriple;
			if ((nameTriple = (pawn.Name as NameTriple)) == null)
			{
				return true;
			}
			List<Pawn> spouses = pawn.GetSpouses(true);
			if (!spouses.Any<Pawn>())
			{
				return false;
			}
			for (int i = 0; i < spouses.Count; i++)
			{
				Pawn pawn2 = spouses[i];
				NameTriple nameTriple2;
				if ((nameTriple2 = (pawn2.Name as NameTriple)) != null)
				{
					string last = nameTriple2.Last;
					NameTriple nameTriple3;
					NameTriple nameTriple4;
					if (!(nameTriple.Last != last) && ((pawn2.GetMother() != null && (nameTriple3 = (pawn2.GetMother().Name as NameTriple)) != null && nameTriple3.Last == last) || (pawn2.GetFather() != null && (nameTriple4 = (pawn2.GetFather().Name as NameTriple)) != null && nameTriple4.Last == last)))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x040030B8 RID: 12472
		public const float MinFemaleAgeToHaveChildren = 16f;

		// Token: 0x040030B9 RID: 12473
		public const float MaxFemaleAgeToHaveChildren = 45f;

		// Token: 0x040030BA RID: 12474
		public const float UsualFemaleAgeToHaveChildren = 27f;

		// Token: 0x040030BB RID: 12475
		public const float MinMaleAgeToHaveChildren = 14f;

		// Token: 0x040030BC RID: 12476
		public const float MaxMaleAgeToHaveChildren = 50f;

		// Token: 0x040030BD RID: 12477
		public const float UsualMaleAgeToHaveChildren = 30f;

		// Token: 0x040030BE RID: 12478
		public const float ChanceForChildToHaveNameOfAnyParent = 0.99f;
	}
}
