using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001490 RID: 5264
	public static class ParentRelationUtility
	{
		// Token: 0x06007188 RID: 29064 RVA: 0x0022C330 File Offset: 0x0022A530
		public static Pawn GetFather(this Pawn pawn)
		{
			if (!pawn.RaceProps.IsFlesh)
			{
				return null;
			}
			List<DirectPawnRelation> directRelations = pawn.relations.DirectRelations;
			for (int i = 0; i < directRelations.Count; i++)
			{
				DirectPawnRelation directPawnRelation = directRelations[i];
				if (directPawnRelation.def == PawnRelationDefOf.Parent && directPawnRelation.otherPawn.gender != Gender.Female)
				{
					return directPawnRelation.otherPawn;
				}
			}
			return null;
		}

		// Token: 0x06007189 RID: 29065 RVA: 0x0022C394 File Offset: 0x0022A594
		public static Pawn GetMother(this Pawn pawn)
		{
			if (!pawn.RaceProps.IsFlesh)
			{
				return null;
			}
			List<DirectPawnRelation> directRelations = pawn.relations.DirectRelations;
			for (int i = 0; i < directRelations.Count; i++)
			{
				DirectPawnRelation directPawnRelation = directRelations[i];
				if (directPawnRelation.def == PawnRelationDefOf.Parent && directPawnRelation.otherPawn.gender == Gender.Female)
				{
					return directPawnRelation.otherPawn;
				}
			}
			return null;
		}

		// Token: 0x0600718A RID: 29066 RVA: 0x0022C3F8 File Offset: 0x0022A5F8
		public static void SetFather(this Pawn pawn, Pawn newFather)
		{
			if (newFather != null && newFather.gender == Gender.Female)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to set ",
					newFather,
					" with gender ",
					newFather.gender,
					" as ",
					pawn,
					"'s father."
				}), false);
				return;
			}
			Pawn father = pawn.GetFather();
			if (father != newFather)
			{
				if (father != null)
				{
					pawn.relations.RemoveDirectRelation(PawnRelationDefOf.Parent, father);
				}
				if (newFather != null)
				{
					pawn.relations.AddDirectRelation(PawnRelationDefOf.Parent, newFather);
				}
			}
		}

		// Token: 0x0600718B RID: 29067 RVA: 0x0022C48C File Offset: 0x0022A68C
		public static void SetMother(this Pawn pawn, Pawn newMother)
		{
			if (newMother != null && newMother.gender != Gender.Female)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to set ",
					newMother,
					" with gender ",
					newMother.gender,
					" as ",
					pawn,
					"'s mother."
				}), false);
				return;
			}
			Pawn mother = pawn.GetMother();
			if (mother != newMother)
			{
				if (mother != null)
				{
					pawn.relations.RemoveDirectRelation(PawnRelationDefOf.Parent, mother);
				}
				if (newMother != null)
				{
					pawn.relations.AddDirectRelation(PawnRelationDefOf.Parent, newMother);
				}
			}
		}

		// Token: 0x0600718C RID: 29068 RVA: 0x0022C520 File Offset: 0x0022A720
		public static float GetRandomSecondParentSkinColor(float otherParentSkin, float childSkin, float? secondChildSkin = null)
		{
			float mirror;
			if (secondChildSkin != null)
			{
				mirror = (childSkin + secondChildSkin.Value) / 2f;
			}
			else
			{
				mirror = childSkin;
			}
			float reflectedSkin = ChildRelationUtility.GetReflectedSkin(otherParentSkin, mirror);
			float num = childSkin;
			float num2 = childSkin;
			if (secondChildSkin != null)
			{
				num = Mathf.Min(num, secondChildSkin.Value);
				num2 = Mathf.Max(num2, secondChildSkin.Value);
			}
			float clampMin = 0f;
			float clampMax = 1f;
			if (reflectedSkin >= num2)
			{
				clampMin = num2;
			}
			else
			{
				clampMax = num;
			}
			return PawnSkinColors.GetRandomMelaninSimilarTo(reflectedSkin, clampMin, clampMax);
		}
	}
}
