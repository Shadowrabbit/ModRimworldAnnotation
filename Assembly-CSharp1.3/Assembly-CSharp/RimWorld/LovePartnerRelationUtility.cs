using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E00 RID: 3584
	public static class LovePartnerRelationUtility
	{
		// Token: 0x060052EC RID: 21228 RVA: 0x001C0FB9 File Offset: 0x001BF1B9
		public static bool HasAnyLovePartner(Pawn pawn, bool allowDead = true)
		{
			return LovePartnerRelationUtility.ExistingLovePartner(pawn, allowDead) != null;
		}

		// Token: 0x060052ED RID: 21229 RVA: 0x001C0FC5 File Offset: 0x001BF1C5
		public static bool IsLovePartnerRelation(PawnRelationDef relation)
		{
			return relation == PawnRelationDefOf.Lover || relation == PawnRelationDefOf.Fiance || relation == PawnRelationDefOf.Spouse;
		}

		// Token: 0x060052EE RID: 21230 RVA: 0x001C0FE1 File Offset: 0x001BF1E1
		public static bool IsExLovePartnerRelation(PawnRelationDef relation)
		{
			return relation == PawnRelationDefOf.ExLover || relation == PawnRelationDefOf.ExSpouse;
		}

		// Token: 0x060052EF RID: 21231 RVA: 0x001C0FF8 File Offset: 0x001BF1F8
		public static bool HasAnyLovePartnerOfTheSameGender(Pawn pawn)
		{
			return pawn.relations.DirectRelations.Find((DirectPawnRelation x) => LovePartnerRelationUtility.IsLovePartnerRelation(x.def) && x.otherPawn.gender == pawn.gender) != null;
		}

		// Token: 0x060052F0 RID: 21232 RVA: 0x001C1038 File Offset: 0x001BF238
		public static bool HasAnyExLovePartnerOfTheSameGender(Pawn pawn)
		{
			return pawn.relations.DirectRelations.Find((DirectPawnRelation x) => LovePartnerRelationUtility.IsExLovePartnerRelation(x.def) && x.otherPawn.gender == pawn.gender) != null;
		}

		// Token: 0x060052F1 RID: 21233 RVA: 0x001C1078 File Offset: 0x001BF278
		public static bool HasAnyLovePartnerOfTheOppositeGender(Pawn pawn)
		{
			return pawn.relations.DirectRelations.Find((DirectPawnRelation x) => LovePartnerRelationUtility.IsLovePartnerRelation(x.def) && x.otherPawn.gender != pawn.gender) != null;
		}

		// Token: 0x060052F2 RID: 21234 RVA: 0x001C10B8 File Offset: 0x001BF2B8
		public static bool HasAnyExLovePartnerOfTheOppositeGender(Pawn pawn)
		{
			return pawn.relations.DirectRelations.Find((DirectPawnRelation x) => LovePartnerRelationUtility.IsExLovePartnerRelation(x.def) && x.otherPawn.gender != pawn.gender) != null;
		}

		// Token: 0x060052F3 RID: 21235 RVA: 0x001C10F8 File Offset: 0x001BF2F8
		public static Pawn ExistingLovePartner(Pawn pawn, bool allowDead = true)
		{
			List<DirectPawnRelation> directRelations = pawn.relations.DirectRelations;
			for (int i = 0; i < directRelations.Count; i++)
			{
				if (LovePartnerRelationUtility.IsLovePartnerRelation(directRelations[i].def) && (!directRelations[i].otherPawn.Destroyed || allowDead))
				{
					return directRelations[i].otherPawn;
				}
			}
			return null;
		}

		// Token: 0x060052F4 RID: 21236 RVA: 0x001C115C File Offset: 0x001BF35C
		public static List<DirectPawnRelation> ExistingLovePartners(Pawn pawn, bool allowDead = true)
		{
			LovePartnerRelationUtility.tmpExistingLovePartners.Clear();
			List<DirectPawnRelation> directRelations = pawn.relations.DirectRelations;
			for (int i = 0; i < directRelations.Count; i++)
			{
				if (LovePartnerRelationUtility.IsLovePartnerRelation(directRelations[i].def) && (!directRelations[i].otherPawn.Destroyed || allowDead))
				{
					LovePartnerRelationUtility.tmpExistingLovePartners.Add(directRelations[i]);
				}
			}
			return LovePartnerRelationUtility.tmpExistingLovePartners;
		}

		// Token: 0x060052F5 RID: 21237 RVA: 0x001C11D4 File Offset: 0x001BF3D4
		public static int ExistingLovePartnersCount(Pawn pawn, bool allowDead = true)
		{
			int num = 0;
			List<DirectPawnRelation> directRelations = pawn.relations.DirectRelations;
			for (int i = 0; i < directRelations.Count; i++)
			{
				if (LovePartnerRelationUtility.IsLovePartnerRelation(directRelations[i].def) && (!directRelations[i].otherPawn.Destroyed || allowDead))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x060052F6 RID: 21238 RVA: 0x001C1230 File Offset: 0x001BF430
		public static bool LovePartnerRelationExists(Pawn first, Pawn second)
		{
			return first.relations.DirectRelationExists(PawnRelationDefOf.Lover, second) || first.relations.DirectRelationExists(PawnRelationDefOf.Fiance, second) || first.relations.DirectRelationExists(PawnRelationDefOf.Spouse, second);
		}

		// Token: 0x060052F7 RID: 21239 RVA: 0x001C126B File Offset: 0x001BF46B
		public static bool ExLovePartnerRelationExists(Pawn first, Pawn second)
		{
			return first.relations.DirectRelationExists(PawnRelationDefOf.ExSpouse, second) || first.relations.DirectRelationExists(PawnRelationDefOf.ExLover, second);
		}

		// Token: 0x060052F8 RID: 21240 RVA: 0x001C1294 File Offset: 0x001BF494
		public static void GiveRandomExLoverOrExSpouseRelation(Pawn first, Pawn second)
		{
			PawnRelationDef def;
			if (Rand.Value < 0.5f)
			{
				def = PawnRelationDefOf.ExLover;
			}
			else
			{
				def = PawnRelationDefOf.ExSpouse;
			}
			first.relations.AddDirectRelation(def, second);
		}

		// Token: 0x060052F9 RID: 21241 RVA: 0x001C12C8 File Offset: 0x001BF4C8
		public static Pawn GetPartnerInMyBed(Pawn pawn)
		{
			Building_Bed building_Bed = pawn.CurrentBed();
			if (building_Bed == null)
			{
				return null;
			}
			if (building_Bed.SleepingSlotsCount <= 1)
			{
				return null;
			}
			if (!LovePartnerRelationUtility.HasAnyLovePartner(pawn, true))
			{
				return null;
			}
			foreach (Pawn pawn2 in building_Bed.CurOccupants)
			{
				if (pawn2 != pawn && LovePartnerRelationUtility.LovePartnerRelationExists(pawn, pawn2))
				{
					return pawn2;
				}
			}
			return null;
		}

		// Token: 0x060052FA RID: 21242 RVA: 0x001C1344 File Offset: 0x001BF544
		public static Pawn ExistingLeastLikedPawnWithRelation(Pawn p, Func<DirectPawnRelation, bool> validator)
		{
			DirectPawnRelation directPawnRelation = LovePartnerRelationUtility.ExistingLeastLikedRel(p, validator);
			if (directPawnRelation != null)
			{
				return directPawnRelation.otherPawn;
			}
			return null;
		}

		// Token: 0x060052FB RID: 21243 RVA: 0x001C1364 File Offset: 0x001BF564
		public static DirectPawnRelation ExistingLeastLikedRel(Pawn p, Func<DirectPawnRelation, bool> validator)
		{
			if (!p.RaceProps.IsFlesh)
			{
				return null;
			}
			DirectPawnRelation directPawnRelation = null;
			int num = int.MaxValue;
			List<DirectPawnRelation> directRelations = p.relations.DirectRelations;
			for (int i = 0; i < directRelations.Count; i++)
			{
				if (validator(directRelations[i]))
				{
					int num2 = p.relations.OpinionOf(directRelations[i].otherPawn);
					if (directPawnRelation == null || num2 < num)
					{
						directPawnRelation = directRelations[i];
						num = num2;
					}
				}
			}
			return directPawnRelation;
		}

		// Token: 0x060052FC RID: 21244 RVA: 0x001C13E4 File Offset: 0x001BF5E4
		public static Pawn ExistingMostLikedLovePartner(Pawn p, bool allowDead)
		{
			DirectPawnRelation directPawnRelation = LovePartnerRelationUtility.ExistingMostLikedLovePartnerRel(p, allowDead);
			if (directPawnRelation != null)
			{
				return directPawnRelation.otherPawn;
			}
			return null;
		}

		// Token: 0x060052FD RID: 21245 RVA: 0x001C1404 File Offset: 0x001BF604
		public static DirectPawnRelation ExistingMostLikedLovePartnerRel(Pawn p, bool allowDead)
		{
			if (!p.RaceProps.IsFlesh)
			{
				return null;
			}
			DirectPawnRelation directPawnRelation = null;
			int num = int.MinValue;
			List<DirectPawnRelation> directRelations = p.relations.DirectRelations;
			for (int i = 0; i < directRelations.Count; i++)
			{
				if ((allowDead || !directRelations[i].otherPawn.Dead) && LovePartnerRelationUtility.IsLovePartnerRelation(directRelations[i].def))
				{
					int num2 = p.relations.OpinionOf(directRelations[i].otherPawn);
					if (directPawnRelation == null || num2 > num)
					{
						directPawnRelation = directRelations[i];
						num = num2;
					}
				}
			}
			return directPawnRelation;
		}

		// Token: 0x060052FE RID: 21246 RVA: 0x001C149C File Offset: 0x001BF69C
		public static HistoryEventDef GetHistoryEventLoveRelationCount(this Pawn pawn)
		{
			int count = pawn.GetLoveRelations(false).Count;
			if (count <= 1)
			{
				return HistoryEventDefOf.GotMarried_SpouseCount_OneOrFewer;
			}
			if (count <= 2)
			{
				return HistoryEventDefOf.GotMarried_SpouseCount_Two;
			}
			if (count <= 3)
			{
				return HistoryEventDefOf.GotMarried_SpouseCount_Three;
			}
			if (count <= 4)
			{
				return HistoryEventDefOf.GotMarried_SpouseCount_Four;
			}
			return HistoryEventDefOf.GotMarried_SpouseCount_FiveOrMore;
		}

		// Token: 0x060052FF RID: 21247 RVA: 0x001C14E4 File Offset: 0x001BF6E4
		public static HistoryEventDef GetHistoryEventForLoveRelationCountPlusOne(this Pawn pawn)
		{
			int count = pawn.GetLoveRelations(false).Count;
			if (count == 0)
			{
				return HistoryEventDefOf.GotMarried_SpouseCount_OneOrFewer;
			}
			if (count < 2)
			{
				return HistoryEventDefOf.GotMarried_SpouseCount_Two;
			}
			if (count < 3)
			{
				return HistoryEventDefOf.GotMarried_SpouseCount_Three;
			}
			if (count < 4)
			{
				return HistoryEventDefOf.GotMarried_SpouseCount_Four;
			}
			return HistoryEventDefOf.GotMarried_SpouseCount_FiveOrMore;
		}

		// Token: 0x06005300 RID: 21248 RVA: 0x001C152C File Offset: 0x001BF72C
		public static float GetLovinMtbHours(Pawn pawn, Pawn partner)
		{
			if (pawn.Dead || partner.Dead)
			{
				return -1f;
			}
			if (DebugSettings.alwaysDoLovin)
			{
				return 0.1f;
			}
			if (pawn.needs.food.Starving || partner.needs.food.Starving)
			{
				return -1f;
			}
			if (pawn.health.hediffSet.BleedRateTotal > 0f || partner.health.hediffSet.BleedRateTotal > 0f)
			{
				return -1f;
			}
			float num = LovePartnerRelationUtility.LovinMtbSinglePawnFactor(pawn);
			if (num <= 0f)
			{
				return -1f;
			}
			float num2 = LovePartnerRelationUtility.LovinMtbSinglePawnFactor(partner);
			if (num2 <= 0f)
			{
				return -1f;
			}
			float num3 = 12f;
			num3 *= num;
			num3 *= num2;
			num3 /= Mathf.Max(pawn.relations.SecondaryLovinChanceFactor(partner), 0.1f);
			num3 /= Mathf.Max(partner.relations.SecondaryLovinChanceFactor(pawn), 0.1f);
			num3 *= GenMath.LerpDouble(-100f, 100f, 1.3f, 0.7f, (float)pawn.relations.OpinionOf(partner));
			num3 *= GenMath.LerpDouble(-100f, 100f, 1.3f, 0.7f, (float)partner.relations.OpinionOf(pawn));
			if (pawn.health.hediffSet.HasHediff(HediffDefOf.PsychicLove, false))
			{
				num3 /= 4f;
			}
			return num3;
		}

		// Token: 0x06005301 RID: 21249 RVA: 0x001C1698 File Offset: 0x001BF898
		private static float LovinMtbSinglePawnFactor(Pawn pawn)
		{
			float num = 1f;
			num /= 1f - pawn.health.hediffSet.PainTotal;
			float level = pawn.health.capacities.GetLevel(PawnCapacityDefOf.Consciousness);
			if (level < 0.5f)
			{
				num /= level * 2f;
			}
			return num / GenMath.FlatHill(0f, 14f, 16f, 25f, 80f, 0.2f, pawn.ageTracker.AgeBiologicalYearsFloat);
		}

		// Token: 0x06005302 RID: 21250 RVA: 0x001C171E File Offset: 0x001BF91E
		public static void TryToShareBed(Pawn first, Pawn second)
		{
			if (LovePartnerRelationUtility.TryToShareBed_Int(first, second))
			{
				return;
			}
			LovePartnerRelationUtility.TryToShareBed_Int(second, first);
		}

		// Token: 0x06005303 RID: 21251 RVA: 0x001C1734 File Offset: 0x001BF934
		private static bool TryToShareBed_Int(Pawn bedOwner, Pawn otherPawn)
		{
			Building_Bed ownedBed = bedOwner.ownership.OwnedBed;
			if (ownedBed != null && ownedBed.AnyUnownedSleepingSlot && BedUtility.WillingToShareBed(bedOwner, otherPawn))
			{
				otherPawn.ownership.ClaimBedIfNonMedical(ownedBed);
				return true;
			}
			return false;
		}

		// Token: 0x06005304 RID: 21252 RVA: 0x001C1774 File Offset: 0x001BF974
		public static float LovePartnerRelationGenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request, bool ex)
		{
			if (generated.ageTracker.AgeBiologicalYearsFloat < 14f)
			{
				return 0f;
			}
			if (other.ageTracker.AgeBiologicalYearsFloat < 14f)
			{
				return 0f;
			}
			if (generated.gender == other.gender && (!other.story.traits.HasTrait(TraitDefOf.Gay) || !request.AllowGay))
			{
				return 0f;
			}
			if (generated.gender != other.gender && other.story.traits.HasTrait(TraitDefOf.Gay))
			{
				return 0f;
			}
			float num = 1f;
			if (ex)
			{
				int num2 = 0;
				List<DirectPawnRelation> directRelations = other.relations.DirectRelations;
				for (int i = 0; i < directRelations.Count; i++)
				{
					if (LovePartnerRelationUtility.IsExLovePartnerRelation(directRelations[i].def))
					{
						num2++;
					}
				}
				num = Mathf.Pow(0.2f, (float)num2);
			}
			else if (LovePartnerRelationUtility.HasAnyLovePartner(other, true))
			{
				return 0f;
			}
			float num3 = (generated.gender == other.gender) ? 0.01f : 1f;
			float generationChanceAgeFactor = LovePartnerRelationUtility.GetGenerationChanceAgeFactor(generated);
			float generationChanceAgeFactor2 = LovePartnerRelationUtility.GetGenerationChanceAgeFactor(other);
			float generationChanceAgeGapFactor = LovePartnerRelationUtility.GetGenerationChanceAgeGapFactor(generated, other, ex);
			float num4 = 1f;
			if (generated.GetRelations(other).Any((PawnRelationDef x) => x.familyByBloodRelation))
			{
				num4 = 0.01f;
			}
			float num5;
			if (request.FixedMelanin != null)
			{
				num5 = ChildRelationUtility.GetMelaninSimilarityFactor(request.FixedMelanin.Value, other.story.melanin);
			}
			else
			{
				num5 = PawnSkinColors.GetMelaninCommonalityFactor(other.story.melanin);
			}
			return num * generationChanceAgeFactor * generationChanceAgeFactor2 * generationChanceAgeGapFactor * num3 * num5 * num4;
		}

		// Token: 0x06005305 RID: 21253 RVA: 0x001C1946 File Offset: 0x001BFB46
		private static float GetGenerationChanceAgeFactor(Pawn p)
		{
			return Mathf.Clamp(GenMath.LerpDouble(14f, 27f, 0f, 1f, p.ageTracker.AgeBiologicalYearsFloat), 0f, 1f);
		}

		// Token: 0x06005306 RID: 21254 RVA: 0x001C197C File Offset: 0x001BFB7C
		private static float GetGenerationChanceAgeGapFactor(Pawn p1, Pawn p2, bool ex)
		{
			float num = Mathf.Abs(p1.ageTracker.AgeBiologicalYearsFloat - p2.ageTracker.AgeBiologicalYearsFloat);
			if (ex)
			{
				float num2 = LovePartnerRelationUtility.MinPossibleAgeGapAtMinAgeToGenerateAsLovers(p1, p2);
				if (num2 >= 0f)
				{
					num = Mathf.Min(num, num2);
				}
				float num3 = LovePartnerRelationUtility.MinPossibleAgeGapAtMinAgeToGenerateAsLovers(p2, p1);
				if (num3 >= 0f)
				{
					num = Mathf.Min(num, num3);
				}
			}
			if (num > 40f)
			{
				return 0f;
			}
			return Mathf.Clamp(GenMath.LerpDouble(0f, 20f, 1f, 0.001f, num), 0.001f, 1f);
		}

		// Token: 0x06005307 RID: 21255 RVA: 0x001C1A10 File Offset: 0x001BFC10
		private static float MinPossibleAgeGapAtMinAgeToGenerateAsLovers(Pawn p1, Pawn p2)
		{
			float num = p1.ageTracker.AgeChronologicalYearsFloat - 14f;
			if (num < 0f)
			{
				Log.Warning("at < 0");
				return 0f;
			}
			float num2 = PawnRelationUtility.MaxPossibleBioAgeAt(p2.ageTracker.AgeBiologicalYearsFloat, p2.ageTracker.AgeChronologicalYearsFloat, num);
			float num3 = PawnRelationUtility.MinPossibleBioAgeAt(p2.ageTracker.AgeBiologicalYearsFloat, num);
			if (num2 < 0f)
			{
				return -1f;
			}
			if (num2 < 14f)
			{
				return -1f;
			}
			if (num3 <= 14f)
			{
				return 0f;
			}
			return num3 - 14f;
		}

		// Token: 0x06005308 RID: 21256 RVA: 0x001C1AA8 File Offset: 0x001BFCA8
		public static void TryToShareChildrenForGeneratedLovePartner(Pawn generated, Pawn other, PawnGenerationRequest request, float extraChanceFactor)
		{
			if (generated.gender == other.gender)
			{
				return;
			}
			List<Pawn> list = other.relations.Children.ToList<Pawn>();
			for (int i = 0; i < list.Count; i++)
			{
				Pawn pawn = list[i];
				float num = 1f;
				if (generated.gender == Gender.Male)
				{
					num = ChildRelationUtility.ChanceOfBecomingChildOf(pawn, generated, other, null, new PawnGenerationRequest?(request), null);
				}
				else if (generated.gender == Gender.Female)
				{
					num = ChildRelationUtility.ChanceOfBecomingChildOf(pawn, other, generated, null, null, new PawnGenerationRequest?(request));
				}
				num *= extraChanceFactor;
				if (Rand.Value < num)
				{
					if (generated.gender == Gender.Male)
					{
						pawn.SetFather(generated);
					}
					else if (generated.gender == Gender.Female)
					{
						pawn.SetMother(generated);
					}
				}
			}
		}

		// Token: 0x06005309 RID: 21257 RVA: 0x001C1B84 File Offset: 0x001BFD84
		public static void ChangeSpouseRelationsToExSpouse(Pawn pawn)
		{
			List<Pawn> spouses = pawn.GetSpouses(true);
			for (int i = spouses.Count - 1; i >= 0; i--)
			{
				HistoryEvent ev = new HistoryEvent(pawn.GetHistoryEventForSpouseCountPlusOne(), pawn.Named(HistoryEventArgsNames.Doer));
				if (spouses[i].Dead || !ev.DoerWillingToDo())
				{
					pawn.relations.RemoveDirectRelation(PawnRelationDefOf.Spouse, spouses[i]);
					pawn.relations.AddDirectRelation(PawnRelationDefOf.ExSpouse, spouses[i]);
				}
			}
		}

		// Token: 0x0600530A RID: 21258 RVA: 0x001C1C08 File Offset: 0x001BFE08
		public static Pawn GetMostDislikedNonPartnerBedOwner(Pawn p)
		{
			Building_Bed ownedBed = p.ownership.OwnedBed;
			if (ownedBed == null)
			{
				return null;
			}
			Pawn pawn = null;
			int num = 0;
			for (int i = 0; i < ownedBed.OwnersForReading.Count; i++)
			{
				if (ownedBed.OwnersForReading[i] != p && !LovePartnerRelationUtility.LovePartnerRelationExists(p, ownedBed.OwnersForReading[i]))
				{
					int num2 = p.relations.OpinionOf(ownedBed.OwnersForReading[i]);
					if (pawn == null || num2 < num)
					{
						pawn = ownedBed.OwnersForReading[i];
						num = num2;
					}
				}
			}
			return pawn;
		}

		// Token: 0x0600530B RID: 21259 RVA: 0x001C1C98 File Offset: 0x001BFE98
		public static float IncestOpinionOffsetFor(Pawn other, Pawn pawn)
		{
			float num = 0f;
			List<DirectPawnRelation> directRelations = other.relations.DirectRelations;
			for (int i = 0; i < directRelations.Count; i++)
			{
				if (LovePartnerRelationUtility.IsLovePartnerRelation(directRelations[i].def) && directRelations[i].otherPawn != pawn && !directRelations[i].otherPawn.Dead)
				{
					foreach (PawnRelationDef pawnRelationDef in other.GetRelations(directRelations[i].otherPawn))
					{
						float incestOpinionOffset = pawnRelationDef.incestOpinionOffset;
						if (incestOpinionOffset < num)
						{
							num = incestOpinionOffset;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x0600530C RID: 21260 RVA: 0x001C1D54 File Offset: 0x001BFF54
		public static bool AreNearEachOther(Pawn p1, Pawn p2)
		{
			return !p1.DestroyedOrNull() && !p2.DestroyedOrNull() && ((p1.MapHeld != null && p1.MapHeld == p2.MapHeld) || (p1.GetCaravan() != null && p1.GetCaravan() == p2.GetCaravan()) || (p1.ParentHolder != null && p1.ParentHolder == p2.ParentHolder));
		}

		// Token: 0x040030F1 RID: 12529
		private const float MinAgeToGenerateWithLovePartnerRelation = 14f;

		// Token: 0x040030F2 RID: 12530
		private static List<DirectPawnRelation> tmpExistingLovePartners = new List<DirectPawnRelation>();
	}
}
