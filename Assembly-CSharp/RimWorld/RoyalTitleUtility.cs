using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.QuestGen;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001D04 RID: 7428
	public static class RoyalTitleUtility
	{
		// Token: 0x0600A18C RID: 41356 RVA: 0x002F2530 File Offset: 0x002F0730
		public static void FindLostAndGainedPermits(RoyalTitleDef currentTitle, RoyalTitleDef newTitle, out List<RoyalTitlePermitDef> gainedPermits, out List<RoyalTitlePermitDef> lostPermits)
		{
			gainedPermits = new List<RoyalTitlePermitDef>();
			lostPermits = new List<RoyalTitlePermitDef>();
			if (newTitle != null && newTitle.permits != null)
			{
				foreach (RoyalTitlePermitDef item in newTitle.permits)
				{
					if (currentTitle == null || currentTitle.permits == null || !currentTitle.permits.Contains(item))
					{
						gainedPermits.Add(item);
					}
				}
			}
			if (currentTitle != null && currentTitle.permits != null)
			{
				foreach (RoyalTitlePermitDef item2 in currentTitle.permits)
				{
					if (newTitle == null || newTitle.permits == null || !newTitle.permits.Contains(item2))
					{
						lostPermits.Add(item2);
					}
				}
			}
		}

		// Token: 0x0600A18D RID: 41357 RVA: 0x002F2620 File Offset: 0x002F0820
		public static string BuildDifferenceExplanationText(RoyalTitleDef currentTitle, RoyalTitleDef newTitle, Faction faction, Pawn pawn)
		{
			RoyalTitleUtility.<>c__DisplayClass1_0 CS$<>8__locals1 = new RoyalTitleUtility.<>c__DisplayClass1_0();
			CS$<>8__locals1.faction = faction;
			CS$<>8__locals1.pawn = pawn;
			CS$<>8__locals1.newTitle = newTitle;
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = RoyalTitleUtility.ShouldBecomeConceitedOnNewTitle(CS$<>8__locals1.pawn);
			List<WorkTags> list = CS$<>8__locals1.pawn.story.DisabledWorkTagsBackstoryAndTraits.GetAllSelectedItems<WorkTags>().ToList<WorkTags>();
			List<WorkTags> list2 = (CS$<>8__locals1.newTitle == null) ? new List<WorkTags>() : CS$<>8__locals1.newTitle.disabledWorkTags.GetAllSelectedItems<WorkTags>().ToList<WorkTags>();
			List<WorkTags> list3 = new List<WorkTags>();
			foreach (WorkTags item in list2)
			{
				if (!list.Contains(item))
				{
					list3.Add(item);
				}
			}
			int num = (CS$<>8__locals1.newTitle != null) ? CS$<>8__locals1.faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(CS$<>8__locals1.newTitle) : -1;
			if (CS$<>8__locals1.newTitle != null && flag)
			{
				stringBuilder.AppendLine("LetterRoyalTitleConceitedTrait".Translate(CS$<>8__locals1.pawn.Named("PAWN"), (from t in RoyalTitleUtility.GetConceitedTraits(CS$<>8__locals1.pawn)
				select t.Label).ToCommaList(true)));
				stringBuilder.AppendLine();
				if (CS$<>8__locals1.newTitle.minExpectation != null)
				{
					stringBuilder.AppendLine("LetterRoyalTitleExpectation".Translate(CS$<>8__locals1.pawn.Named("PAWN"), CS$<>8__locals1.newTitle.minExpectation.label).CapitalizeFirst());
					stringBuilder.AppendLine();
				}
			}
			if (CS$<>8__locals1.newTitle != null)
			{
				if (CS$<>8__locals1.newTitle.canBeInherited)
				{
					Pawn heir = CS$<>8__locals1.pawn.royalty.GetHeir(CS$<>8__locals1.faction);
					TaggedString taggedString = (heir != null) ? "LetterRoyalTitleHeir".Translate(CS$<>8__locals1.pawn.Named("PAWN"), heir.Named("HEIR")) : "LetterRoyalTitleNoHeir".Translate(CS$<>8__locals1.pawn.Named("PAWN"));
					stringBuilder.Append(taggedString);
					if (heir != null && heir.Faction != Faction.OfPlayer)
					{
						stringBuilder.Append(" " + "LetterRoyalTitleHeirFactionWarning".Translate(heir.Named("PAWN"), CS$<>8__locals1.faction.Named("FACTION")));
					}
					stringBuilder.AppendLine(" " + "LetterRoyalTitleChangingHeir".Translate(CS$<>8__locals1.faction.Named("FACTION")));
				}
				else
				{
					stringBuilder.Append("LetterRoyalTitleCantBeInherited".Translate(CS$<>8__locals1.newTitle.Named("TITLE")).CapitalizeFirst());
					stringBuilder.Append(" " + "LetterRoyalTitleNoHeir".Translate(CS$<>8__locals1.pawn.Named("PAWN")));
					stringBuilder.AppendLine();
				}
				stringBuilder.AppendLine();
				if (CS$<>8__locals1.newTitle.permitPointsAwarded > 0 && CS$<>8__locals1.pawn.royalty.NewHighestTitle(CS$<>8__locals1.faction, CS$<>8__locals1.newTitle))
				{
					stringBuilder.AppendLine("PermitPointsAwarded".Translate(CS$<>8__locals1.newTitle.permitPointsAwarded));
					stringBuilder.AppendLine();
				}
			}
			if (flag && list3.Count > 0)
			{
				stringBuilder.AppendLine("LetterRoyalTitleDisabledWorkTag".Translate(CS$<>8__locals1.pawn.Named("PAWN"), (from t in list3
				orderby base.<BuildDifferenceExplanationText>g__FirstTitleDisablingWorkTags|1(t).seniority
				select string.Format("{0} ({1})", t.LabelTranslated(), base.<BuildDifferenceExplanationText>g__FirstTitleDisablingWorkTags|1(t).GetLabelFor(CS$<>8__locals1.pawn))).ToLineList("- ", false)).CapitalizeFirst());
				stringBuilder.AppendLine();
			}
			if (CS$<>8__locals1.newTitle != null)
			{
				if (CS$<>8__locals1.newTitle.requiredMinimumApparelQuality > QualityCategory.Awful)
				{
					stringBuilder.AppendLine("LetterRoyalTitleApparelQualityRequirement".Translate(CS$<>8__locals1.pawn.Named("PAWN"), CS$<>8__locals1.newTitle.requiredMinimumApparelQuality.GetLabel().ToLower()).CapitalizeFirst());
					stringBuilder.AppendLine();
				}
				if (CS$<>8__locals1.newTitle.requiredApparel != null && CS$<>8__locals1.newTitle.requiredApparel.Count > 0)
				{
					stringBuilder.AppendLine("LetterRoyalTitleApparelRequirement".Translate(CS$<>8__locals1.pawn.Named("PAWN")).CapitalizeFirst());
					foreach (RoyalTitleDef.ApparelRequirement apparelRequirement in CS$<>8__locals1.newTitle.requiredApparel)
					{
						int i = 0;
						stringBuilder.Append("- ");
						stringBuilder.AppendLine(string.Join(", ", apparelRequirement.AllRequiredApparelForPawn(CS$<>8__locals1.pawn, false, true).Select(delegate(ThingDef a)
						{
							int i;
							string result = (i == 0) ? a.LabelCap.Resolve() : a.label;
							i = i;
							i++;
							return result;
						}).ToArray<string>()));
					}
					stringBuilder.AppendLine("- " + "ApparelRequirementAnyPrestigeArmor".Translate());
					stringBuilder.AppendLine("- " + "ApparelRequirementAnyPsycasterApparel".Translate());
					stringBuilder.AppendLine();
				}
				if (!CS$<>8__locals1.newTitle.throneRoomRequirements.NullOrEmpty<RoomRequirement>())
				{
					stringBuilder.AppendLine("LetterRoyalTitleThroneroomRequirements".Translate(CS$<>8__locals1.pawn.Named("PAWN"), "\n" + (from r in CS$<>8__locals1.newTitle.throneRoomRequirements
					select r.LabelCap(null)).ToLineList("- ", false)).CapitalizeFirst());
					stringBuilder.AppendLine();
				}
				if (!CS$<>8__locals1.newTitle.GetBedroomRequirements(CS$<>8__locals1.pawn).EnumerableNullOrEmpty<RoomRequirement>())
				{
					stringBuilder.AppendLine("LetterRoyalTitleBedroomRequirements".Translate(CS$<>8__locals1.pawn.Named("PAWN"), "\n" + (from r in CS$<>8__locals1.newTitle.GetBedroomRequirements(CS$<>8__locals1.pawn)
					select r.LabelCap(null)).ToLineList("- ", false)).CapitalizeFirst());
					stringBuilder.AppendLine();
				}
				if (flag && CS$<>8__locals1.newTitle.foodRequirement.Defined && CS$<>8__locals1.newTitle.SatisfyingMeals(true).Any<ThingDef>() && (CS$<>8__locals1.pawn.story == null || !CS$<>8__locals1.pawn.story.traits.HasTrait(TraitDefOf.Ascetic)))
				{
					stringBuilder.AppendLine("LetterRoyalTitleFoodRequirements".Translate(CS$<>8__locals1.pawn.Named("PAWN"), "\n" + (from m in CS$<>8__locals1.newTitle.SatisfyingMeals(false)
					select m.LabelCap.Resolve()).ToLineList("- ", false)).CapitalizeFirst());
					stringBuilder.AppendLine();
				}
			}
			List<RoyalTitlePermitDef> list4;
			List<RoyalTitlePermitDef> list5;
			RoyalTitleUtility.FindLostAndGainedPermits(currentTitle, CS$<>8__locals1.newTitle, out list4, out list5);
			if (CS$<>8__locals1.newTitle != null && CS$<>8__locals1.newTitle.permits != null)
			{
				stringBuilder.AppendLine("LetterRoyalTitlePermits".Translate(CS$<>8__locals1.pawn.Named("PAWN")).CapitalizeFirst());
				IEnumerable<RoyalTitlePermitDef> permits = CS$<>8__locals1.newTitle.permits;
				Func<RoyalTitlePermitDef, int?> keySelector;
				if ((keySelector = CS$<>8__locals1.<>9__11) == null)
				{
					keySelector = (CS$<>8__locals1.<>9__11 = delegate(RoyalTitlePermitDef p)
					{
						RoyalTitleDef royalTitleDef2 = base.<BuildDifferenceExplanationText>g__FirstTitleWithPermit|7(p);
						if (royalTitleDef2 == null)
						{
							return null;
						}
						return new int?(royalTitleDef2.seniority);
					});
				}
				foreach (RoyalTitlePermitDef royalTitlePermitDef in permits.OrderBy(keySelector))
				{
					RoyalTitleDef royalTitleDef = CS$<>8__locals1.<BuildDifferenceExplanationText>g__FirstTitleWithPermit|7(royalTitlePermitDef);
					if (royalTitleDef != null)
					{
						stringBuilder.AppendLine("- " + royalTitlePermitDef.LabelCap + " (" + royalTitleDef.GetLabelFor(CS$<>8__locals1.pawn) + ")");
					}
				}
				stringBuilder.AppendLine();
			}
			if (list5.Count > 0)
			{
				stringBuilder.AppendLine("LetterRoyalTitleLostPermits".Translate(CS$<>8__locals1.pawn.Named("PAWN")).CapitalizeFirst());
				foreach (RoyalTitlePermitDef royalTitlePermitDef2 in list5)
				{
					stringBuilder.AppendLine("- " + royalTitlePermitDef2.LabelCap);
				}
				stringBuilder.AppendLine();
			}
			if (CS$<>8__locals1.newTitle != null)
			{
				if (CS$<>8__locals1.newTitle.grantedAbilities.Contains(AbilityDefOf.Speech) && (currentTitle == null || !currentTitle.grantedAbilities.Contains(AbilityDefOf.Speech)))
				{
					stringBuilder.AppendLine("LetterRoyalTitleSpeechAbilityGained".Translate(CS$<>8__locals1.pawn.Named("PAWN")).CapitalizeFirst());
					stringBuilder.AppendLine();
				}
				List<JoyKindDef> list6 = (from def in DefDatabase<JoyKindDef>.AllDefsListForReading
				where def.titleRequiredAny != null && def.titleRequiredAny.Contains(CS$<>8__locals1.newTitle)
				select def).ToList<JoyKindDef>();
				if (list6.Count > 0)
				{
					stringBuilder.AppendLine("LetterRoyalTitleEnabledJoyKind".Translate(CS$<>8__locals1.pawn.Named("PAWN")).CapitalizeFirst());
					foreach (JoyKindDef joyKindDef in list6)
					{
						stringBuilder.AppendLine("- " + joyKindDef.LabelCap);
					}
					stringBuilder.AppendLine();
				}
				if (flag && !CS$<>8__locals1.newTitle.disabledJoyKinds.NullOrEmpty<JoyKindDef>())
				{
					stringBuilder.AppendLine("LetterRoyalTitleDisabledJoyKind".Translate(CS$<>8__locals1.pawn.Named("PAWN")).CapitalizeFirst());
					foreach (JoyKindDef joyKindDef2 in CS$<>8__locals1.newTitle.disabledJoyKinds)
					{
						stringBuilder.AppendLine("- " + joyKindDef2.LabelCap);
					}
					stringBuilder.AppendLine();
				}
				if (CS$<>8__locals1.faction.def.royalImplantRules != null)
				{
					List<RoyalImplantRule> list7 = new List<RoyalImplantRule>();
					foreach (RoyalImplantRule royalImplantRule in CS$<>8__locals1.faction.def.royalImplantRules)
					{
						RoyalTitleDef minTitleForImplant = CS$<>8__locals1.faction.GetMinTitleForImplant(royalImplantRule.implantHediff, 0);
						int num2 = CS$<>8__locals1.faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(minTitleForImplant);
						if (num >= num2)
						{
							if (royalImplantRule.maxLevel == 0)
							{
								list7.Add(royalImplantRule);
							}
							else
							{
								list7.AddDistinct(CS$<>8__locals1.faction.GetMaxAllowedImplantLevel(royalImplantRule.implantHediff, CS$<>8__locals1.newTitle));
							}
						}
					}
					if (list7.Count > 0)
					{
						stringBuilder.AppendLine("LetterRoyalTitleAllowedImplants".Translate(CS$<>8__locals1.pawn.Named("PAWN"), "\n" + list7.Select(delegate(RoyalImplantRule i)
						{
							if (i.maxLevel == 0)
							{
								return string.Format("{0} ({1})", i.implantHediff.LabelCap, CS$<>8__locals1.faction.GetMinTitleForImplant(i.implantHediff, 0).GetLabelFor(CS$<>8__locals1.pawn));
							}
							return string.Format("{0}({1}x) ({2})", i.implantHediff.LabelCap, i.maxLevel, i.minTitle.GetLabelFor(CS$<>8__locals1.pawn));
						}).ToLineList("- ", false)).CapitalizeFirst());
						stringBuilder.AppendLine();
					}
				}
				if (currentTitle != null && CS$<>8__locals1.newTitle.seniority < currentTitle.seniority)
				{
					List<Hediff> list8 = new List<Hediff>();
					if (CS$<>8__locals1.pawn.health != null && CS$<>8__locals1.pawn.health.hediffSet != null)
					{
						foreach (Hediff hediff in CS$<>8__locals1.pawn.health.hediffSet.hediffs)
						{
							if (hediff.def.HasComp(typeof(HediffComp_RoyalImplant)))
							{
								RoyalTitleDef minTitleForImplant2 = CS$<>8__locals1.faction.GetMinTitleForImplant(hediff.def, HediffComp_RoyalImplant.GetImplantLevel(hediff));
								if (CS$<>8__locals1.faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(minTitleForImplant2) > num)
								{
									list8.Add(hediff);
								}
							}
						}
					}
					if (list8.Count > 0)
					{
						stringBuilder.AppendLine("LetterRoyalTitleImplantsMustBeRemoved".Translate(CS$<>8__locals1.pawn.Named("PAWN"), "\n" + (from i in list8
						select i.LabelCap).ToLineList("- ", false)).Resolve());
						stringBuilder.AppendLine("LetterRoyalTitleImplantGracePeriod".Translate());
						stringBuilder.AppendLine();
					}
				}
				if (CS$<>8__locals1.pawn.royalty.NewHighestTitle(CS$<>8__locals1.faction, CS$<>8__locals1.newTitle) && !CS$<>8__locals1.newTitle.rewards.NullOrEmpty<ThingDefCountClass>())
				{
					stringBuilder.AppendLine("LetterRoyalTitleRewardGranted".Translate(CS$<>8__locals1.pawn.Named("PAWN"), "\n" + (from r in CS$<>8__locals1.newTitle.rewards
					select r.Label).ToLineList("- ", false)).CapitalizeFirst());
					stringBuilder.AppendLine();
				}
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x0600A18E RID: 41358 RVA: 0x0006B723 File Offset: 0x00069923
		public static RoyalTitleDef GetCurrentTitleIn(this Pawn p, Faction faction)
		{
			if (p.royalty != null)
			{
				return p.royalty.GetCurrentTitle(faction);
			}
			return null;
		}

		// Token: 0x0600A18F RID: 41359 RVA: 0x002F34D4 File Offset: 0x002F16D4
		public static int GetCurrentTitleSeniorityIn(this Pawn p, Faction faction)
		{
			RoyalTitleDef currentTitleIn = p.GetCurrentTitleIn(faction);
			if (currentTitleIn != null)
			{
				return currentTitleIn.seniority;
			}
			return 0;
		}

		// Token: 0x0600A190 RID: 41360 RVA: 0x002F34F4 File Offset: 0x002F16F4
		public static string GetTitleProgressionInfo(Faction faction, Pawn pawn = null)
		{
			TaggedString t = "RoyalTitleTooltipTitlesEarnable".Translate(faction.Named("FACTION")) + ":";
			int num = 0;
			foreach (RoyalTitleDef royalTitleDef in faction.def.RoyalTitlesAwardableInSeniorityOrderForReading)
			{
				num += royalTitleDef.favorCost;
				t += "\n  - " + ((pawn != null) ? royalTitleDef.GetLabelCapFor(pawn) : royalTitleDef.GetLabelCapForBothGenders()) + ": " + "RoyalTitleTooltipRoyalFavorAmount".Translate(royalTitleDef.favorCost, faction.def.royalFavorLabel) + " (" + "RoyalTitleTooltipRoyalFavorTotal".Translate(num.ToString()) + ")";
			}
			t += "\n\n" + "RoyalTitleTooltipTitlesNonEarnable".Translate(faction.Named("FACTION")) + ":";
			foreach (RoyalTitleDef royalTitleDef2 in from tit in faction.def.RoyalTitlesAllInSeniorityOrderForReading
			where !tit.Awardable
			select tit)
			{
				t += "\n  - " + royalTitleDef2.GetLabelCapForBothGenders();
			}
			return t.Resolve();
		}

		// Token: 0x0600A191 RID: 41361 RVA: 0x002F36AC File Offset: 0x002F18AC
		public static Building_Throne FindBestUnassignedThrone(Pawn pawn)
		{
			float num = float.PositiveInfinity;
			Building_Throne result = null;
			foreach (Thing thing in pawn.Map.listerThings.ThingsOfDef(ThingDefOf.Throne))
			{
				Building_Throne building_Throne = thing as Building_Throne;
				if (building_Throne != null && building_Throne.CompAssignableToPawn.HasFreeSlot && building_Throne.Spawned && !building_Throne.IsForbidden(pawn) && pawn.CanReserveAndReach(building_Throne, PathEndMode.InteractionCell, pawn.NormalMaxDanger(), 1, -1, null, false) && RoomRoleWorker_ThroneRoom.Validate(building_Throne.GetRoom(RegionType.Set_Passable)) == null)
				{
					PawnPath pawnPath = pawn.Map.pathFinder.FindPath(pawn.Position, building_Throne, pawn, PathEndMode.InteractionCell);
					float num2 = pawnPath.Found ? pawnPath.TotalCost : float.PositiveInfinity;
					pawnPath.ReleaseToPool();
					if (num > num2)
					{
						num = num2;
						result = building_Throne;
					}
				}
			}
			if (num == float.PositiveInfinity)
			{
				return null;
			}
			return result;
		}

		// Token: 0x0600A192 RID: 41362 RVA: 0x002F37C0 File Offset: 0x002F19C0
		public static Building_Throne FindBestUsableThrone(Pawn pawn)
		{
			Building_Throne building_Throne = pawn.ownership.AssignedThrone;
			if (building_Throne != null)
			{
				if (!building_Throne.Spawned || building_Throne.IsForbidden(pawn) || !pawn.CanReserveAndReach(building_Throne, PathEndMode.InteractionCell, pawn.NormalMaxDanger(), 1, -1, null, false))
				{
					return null;
				}
				if (RoomRoleWorker_ThroneRoom.Validate(building_Throne.GetRoom(RegionType.Set_Passable)) != null)
				{
					return null;
				}
			}
			else
			{
				building_Throne = RoyalTitleUtility.FindBestUnassignedThrone(pawn);
				if (building_Throne == null)
				{
					return null;
				}
				pawn.ownership.ClaimThrone(building_Throne);
			}
			return building_Throne;
		}

		// Token: 0x0600A193 RID: 41363 RVA: 0x002F3834 File Offset: 0x002F1A34
		public static bool BedroomSatisfiesRequirements(Room room, RoyalTitle title)
		{
			using (List<RoomRequirement>.Enumerator enumerator = title.def.bedroomRequirements.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.Met(room, null))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600A194 RID: 41364 RVA: 0x002F3894 File Offset: 0x002F1A94
		public static bool IsPawnConceited(Pawn p)
		{
			Pawn_StoryTracker story = p.story;
			TraitSet traitSet = (story != null) ? story.traits : null;
			return (traitSet == null || !traitSet.HasTrait(TraitDefOf.Ascetic)) && (!p.Faction.IsPlayer || p.IsQuestLodger() || (traitSet != null && (traitSet.HasTrait(TraitDefOf.Abrasive) || traitSet.HasTrait(TraitDefOf.Greedy) || traitSet.HasTrait(TraitDefOf.Jealous))));
		}

		// Token: 0x0600A195 RID: 41365 RVA: 0x0006B73B File Offset: 0x0006993B
		public static IEnumerable<Trait> GetConceitedTraits(Pawn p)
		{
			Pawn_StoryTracker story = p.story;
			TraitSet traits = (story != null) ? story.traits : null;
			if (traits != null)
			{
				int num;
				for (int i = 0; i < RoyalTitleUtility.ConceitedTraits.Count; i = num + 1)
				{
					Trait trait = traits.GetTrait(RoyalTitleUtility.ConceitedTraits[i]);
					if (trait != null)
					{
						yield return trait;
					}
					num = i;
				}
			}
			yield break;
		}

		// Token: 0x0600A196 RID: 41366 RVA: 0x0006B74B File Offset: 0x0006994B
		public static IEnumerable<Trait> GetTraitsAffectingPsylinkNegatively(Pawn p)
		{
			if (p.story == null || p.story.traits == null || p.story.traits.allTraits.NullOrEmpty<Trait>())
			{
				yield break;
			}
			foreach (Trait trait in p.story.traits.allTraits)
			{
				TraitDegreeData traitDegreeData = trait.def.DataAtDegree(trait.Degree);
				if (traitDegreeData.statFactors != null)
				{
					if (traitDegreeData.statFactors.Any((StatModifier f) => f.stat == StatDefOf.PsychicSensitivity && f.value < 1f))
					{
						goto IL_114;
					}
				}
				if (traitDegreeData.statOffsets == null)
				{
					continue;
				}
				if (!traitDegreeData.statOffsets.Any((StatModifier f) => f.stat == StatDefOf.PsychicSensitivity && f.value < 0f))
				{
					continue;
				}
				IL_114:
				yield return trait;
			}
			List<Trait>.Enumerator enumerator = default(List<Trait>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600A197 RID: 41367 RVA: 0x002F390C File Offset: 0x002F1B0C
		public static TaggedString GetPsylinkAffectedByTraitsNegativelyWarning(Pawn p)
		{
			if (p.HasPsylink || !RoyalTitleUtility.GetTraitsAffectingPsylinkNegatively(p).Any<Trait>())
			{
				return null;
			}
			return "RoyalWithTraitAffectingPsylinkNegatively".Translate(p.Named("PAWN"), p.Faction.Named("FACTION"), (from t in RoyalTitleUtility.GetTraitsAffectingPsylinkNegatively(p)
			select t.Label).ToCommaList(true));
		}

		// Token: 0x0600A198 RID: 41368 RVA: 0x002F3990 File Offset: 0x002F1B90
		public static bool ShouldBecomeConceitedOnNewTitle(Pawn p)
		{
			Pawn_StoryTracker story = p.story;
			TraitSet traitSet = (story != null) ? story.traits : null;
			return (traitSet == null || !traitSet.HasTrait(TraitDefOf.Ascetic)) && (p.Faction == null || !p.Faction.IsPlayer || p.IsQuestLodger() || RoyalTitleUtility.GetConceitedTraits(p).Any<Trait>());
		}

		// Token: 0x0600A199 RID: 41369 RVA: 0x002F39EC File Offset: 0x002F1BEC
		public static Quest GetCurrentBestowingCeremonyQuest(Pawn pawn, Faction faction)
		{
			foreach (Quest quest in Find.QuestManager.QuestsListForReading)
			{
				if (!quest.Historical)
				{
					QuestPart_BestowingCeremony questPart_BestowingCeremony = (QuestPart_BestowingCeremony)quest.PartsListForReading.FirstOrDefault((QuestPart p) => p is QuestPart_BestowingCeremony);
					if (questPart_BestowingCeremony != null && questPart_BestowingCeremony.target == pawn && questPart_BestowingCeremony.bestower.Faction == faction)
					{
						return quest;
					}
				}
			}
			return null;
		}

		// Token: 0x0600A19A RID: 41370 RVA: 0x0006B75B File Offset: 0x0006995B
		public static bool ShouldGetBestowingCeremonyQuest(Pawn pawn, out Faction faction)
		{
			faction = null;
			return pawn.Faction != null && pawn.Faction.IsPlayer && pawn.royalty != null && pawn.royalty.CanUpdateTitleOfAnyFaction(out faction) && RoyalTitleUtility.GetCurrentBestowingCeremonyQuest(pawn, faction) == null;
		}

		// Token: 0x0600A19B RID: 41371 RVA: 0x0006B798 File Offset: 0x00069998
		public static bool ShouldGetBestowingCeremonyQuest(Pawn pawn, Faction faction)
		{
			return pawn.Faction != null && pawn.Faction.IsPlayer && pawn.royalty != null && pawn.royalty.CanUpdateTitle(faction) && RoyalTitleUtility.GetCurrentBestowingCeremonyQuest(pawn, faction) == null;
		}

		// Token: 0x0600A19C RID: 41372 RVA: 0x002F3A98 File Offset: 0x002F1C98
		public static void EndExistingBestowingCeremonyQuest(Pawn pawn, Faction faction)
		{
			foreach (Quest quest in Find.QuestManager.QuestsListForReading)
			{
				if (!quest.Historical && quest.State != QuestState.Ongoing)
				{
					QuestPart_BestowingCeremony questPart_BestowingCeremony = (QuestPart_BestowingCeremony)quest.PartsListForReading.FirstOrDefault((QuestPart p) => p is QuestPart_BestowingCeremony);
					if (questPart_BestowingCeremony != null && questPart_BestowingCeremony.target == pawn && questPart_BestowingCeremony.bestower.Faction == faction)
					{
						quest.End(QuestEndOutcome.InvalidPreAcceptance, false);
					}
				}
			}
		}

		// Token: 0x0600A19D RID: 41373 RVA: 0x002F3B4C File Offset: 0x002F1D4C
		public static void GenerateBestowingCeremonyQuest(Pawn pawn, Faction faction)
		{
			Slate slate = new Slate();
			slate.Set<Pawn>("titleHolder", pawn, false);
			slate.Set<Faction>("bestowingFaction", faction, false);
			if (QuestScriptDefOf.BestowingCeremony.CanRun(slate))
			{
				QuestUtility.SendLetterQuestAvailable(QuestUtility.GenerateQuestAndMakeAvailable(QuestScriptDefOf.BestowingCeremony, slate));
			}
		}

		// Token: 0x0600A19E RID: 41374 RVA: 0x0006B7D1 File Offset: 0x000699D1
		public static void ResetStaticData()
		{
			RoyalTitleUtility.ConceitedTraits = new List<TraitDef>
			{
				TraitDefOf.Abrasive,
				TraitDefOf.Greedy,
				TraitDefOf.Jealous
			};
		}

		// Token: 0x0600A19F RID: 41375 RVA: 0x002F3B98 File Offset: 0x002F1D98
		public static void DoTable_IngestibleMaxSatisfiedTitle()
		{
			List<TableDataGetter<ThingDef>> list = new List<TableDataGetter<ThingDef>>();
			list.Add(new TableDataGetter<ThingDef>("name", (ThingDef f) => f.LabelCap));
			list.Add(new TableDataGetter<ThingDef>("max satisfied title", delegate(ThingDef t)
			{
				RoyalTitleDef royalTitleDef = t.ingestible.MaxSatisfiedTitle();
				if (royalTitleDef == null)
				{
					return "-";
				}
				return royalTitleDef.LabelCap;
			}));
			DebugTables.MakeTablesDialog<ThingDef>(from t in DefDatabase<ThingDef>.AllDefsListForReading
			where t.ingestible != null && !t.IsCorpse && t.ingestible.HumanEdible
			select t, list.ToArray());
		}

		// Token: 0x04006DB1 RID: 28081
		private static List<TraitDef> ConceitedTraits;
	}
}
