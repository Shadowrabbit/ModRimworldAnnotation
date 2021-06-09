using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001479 RID: 5241
	public static class ChildRelationUtility
	{
		// Token: 0x06007125 RID: 28965 RVA: 0x002292C4 File Offset: 0x002274C4
		public static float ChanceOfBecomingChildOf(Pawn child, Pawn father, Pawn mother, PawnGenerationRequest? childGenerationRequest, PawnGenerationRequest? fatherGenerationRequest, PawnGenerationRequest? motherGenerationRequest)
		{
			if (father != null && father.gender != Gender.Male)
			{
				Log.Warning("Tried to calculate chance for father with gender \"" + father.gender + "\".", false);
				return 0f;
			}
			if (mother != null && mother.gender != Gender.Female)
			{
				Log.Warning("Tried to calculate chance for mother with gender \"" + mother.gender + "\".", false);
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

		// Token: 0x06007126 RID: 28966 RVA: 0x00229540 File Offset: 0x00227740
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
					}), false);
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

		// Token: 0x06007127 RID: 28967 RVA: 0x0004C2E3 File Offset: 0x0004A4E3
		public static bool ChildWantsNameOfAnyParent(Pawn child)
		{
			return Rand.ValueSeeded(child.thingIDNumber ^ 88271612) < 0.99f;
		}

		// Token: 0x06007128 RID: 28968 RVA: 0x0004C2FD File Offset: 0x0004A4FD
		private static int NumberOfChildrenFemaleWantsEver(Pawn female)
		{
			Rand.PushState();
			Rand.Seed = female.thingIDNumber * 3;
			int result = Rand.RangeInclusive(0, 3);
			Rand.PopState();
			return result;
		}

		// Token: 0x06007129 RID: 28969 RVA: 0x00229618 File Offset: 0x00227818
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

		// Token: 0x0600712A RID: 28970 RVA: 0x0004C31D File Offset: 0x0004A51D
		private static float GetAgeFactor(float ageAtBirth, float min, float max, float mid)
		{
			return GenMath.GetFactorInInterval(min, mid, max, 1.6f, ageAtBirth);
		}

		// Token: 0x0600712B RID: 28971 RVA: 0x0022965C File Offset: 0x0022785C
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

		// Token: 0x0600712C RID: 28972 RVA: 0x0022971C File Offset: 0x0022791C
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

		// Token: 0x0600712D RID: 28973 RVA: 0x0004C32D File Offset: 0x0004A52D
		public static float GetReflectedSkin(float value, float mirror)
		{
			return Mathf.Clamp01(GenMath.Reflection(value, mirror));
		}

		// Token: 0x0600712E RID: 28974 RVA: 0x00229800 File Offset: 0x00227A00
		public static float GetMelaninSimilarityFactor(float melanin1, float melanin2)
		{
			float min = Mathf.Clamp01(melanin1 - 0.15f);
			float max = Mathf.Clamp01(melanin1 + 0.15f);
			return GenMath.GetFactorInInterval(min, melanin1, max, 2.5f, melanin2);
		}

		// Token: 0x0600712F RID: 28975 RVA: 0x00229834 File Offset: 0x00227A34
		public static float GetRandomChildSkinColor(float fatherMelanin, float motherMelanin)
		{
			float clampMin = Mathf.Min(fatherMelanin, motherMelanin);
			float clampMax = Mathf.Max(fatherMelanin, motherMelanin);
			return PawnSkinColors.GetRandomMelaninSimilarTo((fatherMelanin + motherMelanin) / 2f, clampMin, clampMax);
		}

		// Token: 0x06007130 RID: 28976 RVA: 0x00229864 File Offset: 0x00227A64
		public static bool DefinitelyHasNotBirthName(Pawn pawn)
		{
			Pawn spouse = pawn.GetSpouse();
			if (spouse == null)
			{
				return false;
			}
			string last = ((NameTriple)spouse.Name).Last;
			return !(((NameTriple)pawn.Name).Last != last) && ((spouse.GetMother() != null && ((NameTriple)spouse.GetMother().Name).Last == last) || (spouse.GetFather() != null && ((NameTriple)spouse.GetFather().Name).Last == last));
		}

		// Token: 0x04004AAE RID: 19118
		public const float MinFemaleAgeToHaveChildren = 16f;

		// Token: 0x04004AAF RID: 19119
		public const float MaxFemaleAgeToHaveChildren = 45f;

		// Token: 0x04004AB0 RID: 19120
		public const float UsualFemaleAgeToHaveChildren = 27f;

		// Token: 0x04004AB1 RID: 19121
		public const float MinMaleAgeToHaveChildren = 14f;

		// Token: 0x04004AB2 RID: 19122
		public const float MaxMaleAgeToHaveChildren = 50f;

		// Token: 0x04004AB3 RID: 19123
		public const float UsualMaleAgeToHaveChildren = 30f;

		// Token: 0x04004AB4 RID: 19124
		public const float ChanceForChildToHaveNameOfAnyParent = 0.99f;
	}
}
