using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002E8 RID: 744
	public static class PawnGenerator
	{
		// Token: 0x060014C6 RID: 5318 RVA: 0x0007794C File Offset: 0x00075B4C
		public static void Reset()
		{
			PawnGenerator.relationsGeneratableBlood = (from rel in DefDatabase<PawnRelationDef>.AllDefsListForReading
			where rel.familyByBloodRelation && rel.generationChanceFactor > 0f
			select rel).ToArray<PawnRelationDef>();
			PawnGenerator.relationsGeneratableNonblood = (from rel in DefDatabase<PawnRelationDef>.AllDefsListForReading
			where !rel.familyByBloodRelation && rel.generationChanceFactor > 0f
			select rel).ToArray<PawnRelationDef>();
		}

		// Token: 0x060014C7 RID: 5319 RVA: 0x000779C0 File Offset: 0x00075BC0
		public static Pawn GeneratePawn(PawnKindDef kindDef, Faction faction = null)
		{
			return PawnGenerator.GeneratePawn(new PawnGenerationRequest(kindDef, faction, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 1f, false, true, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, null, false, false));
		}

		// Token: 0x060014C8 RID: 5320 RVA: 0x00077A38 File Offset: 0x00075C38
		public static Pawn GeneratePawn(PawnGenerationRequest request)
		{
			Pawn result;
			try
			{
				Pawn pawn = PawnGenerator.GenerateOrRedressPawnInternal(request);
				if (pawn != null && !request.AllowDead && pawn.health.hediffSet.hediffs.Any<Hediff>())
				{
					bool dead = pawn.Dead;
					bool downed = pawn.Downed;
					pawn.health.hediffSet.DirtyCache();
					pawn.health.CheckForStateChange(null, null);
					if (pawn.Dead)
					{
						Log.Error(string.Concat(new object[]
						{
							"Pawn was generated dead but the pawn generation request specified the pawn must be alive. This shouldn't ever happen even if we ran out of tries because null pawn should have been returned instead in this case. Resetting health...\npawn.Dead=",
							pawn.Dead.ToString(),
							" pawn.Downed=",
							pawn.Downed.ToString(),
							" deadBefore=",
							dead.ToString(),
							" downedBefore=",
							downed.ToString(),
							"\nrequest=",
							request
						}));
						pawn.health.Reset();
					}
				}
				if (pawn.Faction == Faction.OfPlayerSilentFail && !pawn.IsQuestLodger())
				{
					Find.StoryWatcher.watcherPopAdaptation.Notify_PawnEvent(pawn, PopAdaptationEvent.GainedColonist);
				}
				result = pawn;
			}
			catch (Exception arg)
			{
				Log.Error("Error while generating pawn. Rethrowing. Exception: " + arg);
				throw;
			}
			finally
			{
			}
			return result;
		}

		// Token: 0x060014C9 RID: 5321 RVA: 0x00077BB4 File Offset: 0x00075DB4
		private static Pawn GenerateOrRedressPawnInternal(PawnGenerationRequest request)
		{
			Pawn pawn = null;
			if (!request.Newborn && !request.ForceGenerateNewPawn)
			{
				if (request.ForceRedressWorldPawnIfFormerColonist)
				{
					if ((from x in PawnGenerator.GetValidCandidatesToRedress(request)
					where PawnUtility.EverBeenColonistOrTameAnimal(x)
					select x).TryRandomElementByWeight((Pawn x) => PawnGenerator.WorldPawnSelectionWeight(x), out pawn))
					{
						PawnGenerator.RedressPawn(pawn, request);
						Find.WorldPawns.RemovePawn(pawn);
					}
				}
				if (pawn == null && request.Inhabitant && request.Tile != -1)
				{
					Settlement settlement = Find.WorldObjects.WorldObjectAt<Settlement>(request.Tile);
					if (settlement != null && settlement.previouslyGeneratedInhabitants.Any<Pawn>())
					{
						if ((from x in PawnGenerator.GetValidCandidatesToRedress(request)
						where settlement.previouslyGeneratedInhabitants.Contains(x)
						select x).TryRandomElementByWeight((Pawn x) => PawnGenerator.WorldPawnSelectionWeight(x), out pawn))
						{
							PawnGenerator.RedressPawn(pawn, request);
							Find.WorldPawns.RemovePawn(pawn);
						}
					}
				}
				if (pawn == null && Rand.Chance(PawnGenerator.ChanceToRedressAnyWorldPawn(request)))
				{
					if (PawnGenerator.GetValidCandidatesToRedress(request).TryRandomElementByWeight((Pawn x) => PawnGenerator.WorldPawnSelectionWeight(x), out pawn))
					{
						PawnGenerator.RedressPawn(pawn, request);
						Find.WorldPawns.RemovePawn(pawn);
					}
				}
			}
			bool redressed;
			if (pawn == null)
			{
				redressed = false;
				pawn = PawnGenerator.GenerateNewPawnInternal(ref request);
				if (pawn == null)
				{
					return null;
				}
				if (request.Inhabitant && request.Tile != -1)
				{
					Settlement settlement2 = Find.WorldObjects.WorldObjectAt<Settlement>(request.Tile);
					if (settlement2 != null)
					{
						settlement2.previouslyGeneratedInhabitants.Add(pawn);
					}
				}
			}
			else
			{
				redressed = true;
			}
			if (pawn.Ideo != null)
			{
				pawn.Ideo.Notify_MemberGenerated(pawn);
			}
			if (Find.Scenario != null)
			{
				Find.Scenario.Notify_PawnGenerated(pawn, request.Context, redressed);
			}
			return pawn;
		}

		// Token: 0x060014CA RID: 5322 RVA: 0x00077DB8 File Offset: 0x00075FB8
		public static void RedressPawn(Pawn pawn, PawnGenerationRequest request)
		{
			try
			{
				if (pawn.becameWorldPawnTickAbs != -1 && pawn.health != null)
				{
					float x = (GenTicks.TicksAbs - pawn.becameWorldPawnTickAbs).TicksToDays();
					List<Hediff> list = SimplePool<List<Hediff>>.Get();
					list.Clear();
					foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
					{
						if (Rand.Chance(hediff.def.removeOnRedressChanceByDaysCurve.Evaluate(x)))
						{
							list.Add(hediff);
						}
					}
					foreach (Hediff hediff2 in list)
					{
						pawn.health.RemoveHediff(hediff2);
					}
					list.Clear();
					SimplePool<List<Hediff>>.Return(list);
				}
				pawn.ChangeKind(request.KindDef);
				if (pawn.royalty != null)
				{
					pawn.royalty.allowRoomRequirements = pawn.kindDef.allowRoyalRoomRequirements;
					pawn.royalty.allowApparelRequirements = pawn.kindDef.allowRoyalApparelRequirements;
				}
				if (pawn.Faction != request.Faction)
				{
					pawn.SetFaction(request.Faction, null);
					if (request.FixedIdeo != null)
					{
						pawn.ideo.SetIdeo(request.FixedIdeo);
					}
					else if (pawn.ideo != null && request.Faction != null && request.Faction.ideos != null && !request.Faction.ideos.Has(pawn.Ideo))
					{
						pawn.ideo.SetIdeo(request.Faction.ideos.GetRandomIdeoForNewPawn());
					}
				}
				PawnGenerator.GenerateGearFor(pawn, request);
				PawnGenerator.AddRequiredScars(pawn);
				if (pawn.guest != null)
				{
					pawn.guest.SetGuestStatus(null, GuestStatus.Guest);
					pawn.guest.RandomizeJoinStatus();
				}
				if (pawn.needs != null)
				{
					pawn.needs.SetInitialLevels();
				}
				if (pawn.mindState != null)
				{
					pawn.mindState.SetupLastHumanMeatTick();
				}
				if (pawn.surroundings != null)
				{
					pawn.surroundings.Clear();
				}
			}
			finally
			{
			}
		}

		// Token: 0x060014CB RID: 5323 RVA: 0x0007801C File Offset: 0x0007621C
		public static bool IsBeingGenerated(Pawn pawn)
		{
			for (int i = 0; i < PawnGenerator.pawnsBeingGenerated.Count; i++)
			{
				if (PawnGenerator.pawnsBeingGenerated[i].Pawn == pawn)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060014CC RID: 5324 RVA: 0x00078058 File Offset: 0x00076258
		private static bool IsValidCandidateToRedress(Pawn pawn, PawnGenerationRequest request)
		{
			if (pawn.def != request.KindDef.race)
			{
				return false;
			}
			if (!request.WorldPawnFactionDoesntMatter && pawn.Faction != request.Faction)
			{
				return false;
			}
			if (!request.AllowDead)
			{
				if (pawn.Dead || pawn.Destroyed)
				{
					return false;
				}
				if (pawn.health.hediffSet.GetBrain() == null)
				{
					return false;
				}
			}
			if (!request.AllowDowned && pawn.Downed)
			{
				return false;
			}
			if (pawn.health.hediffSet.BleedRateTotal > 0.001f)
			{
				return false;
			}
			if (!request.CanGeneratePawnRelations && pawn.RaceProps.IsFlesh && pawn.relations.RelatedToAnyoneOrAnyoneRelatedToMe)
			{
				return false;
			}
			if (!request.AllowGay && pawn.RaceProps.Humanlike && pawn.story.traits.HasTrait(TraitDefOf.Gay))
			{
				return false;
			}
			if (!request.AllowAddictions && AddictionUtility.AddictedToAnything(pawn))
			{
				return false;
			}
			if (request.ProhibitedTraits != null && request.ProhibitedTraits.Any((TraitDef t) => pawn.story.traits.HasTrait(t)))
			{
				return false;
			}
			if (request.KindDef.forcedHair != null && pawn.story.hairDef != request.KindDef.forcedHair)
			{
				return false;
			}
			List<SkillRange> skills = request.KindDef.skills;
			if (skills != null)
			{
				for (int i = 0; i < skills.Count; i++)
				{
					SkillRecord skill = pawn.skills.GetSkill(skills[i].Skill);
					if (skill.TotallyDisabled)
					{
						return false;
					}
					if (skill.Level < skills[i].Range.min || skill.Level > skills[i].Range.max)
					{
						return false;
					}
				}
			}
			if (request.KindDef.missingParts != null)
			{
				foreach (MissingPart missingPart in request.KindDef.missingParts)
				{
					using (List<Hediff>.Enumerator enumerator2 = pawn.health.hediffSet.hediffs.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							Hediff_MissingPart hediff_MissingPart;
							if ((hediff_MissingPart = (enumerator2.Current as Hediff_MissingPart)) != null)
							{
								bool flag = false;
								if (missingPart.BodyPart == hediff_MissingPart.Part.def && !PawnGenerator.tmpMissingParts.Contains(hediff_MissingPart))
								{
									PawnGenerator.tmpMissingParts.Add(hediff_MissingPart);
									break;
								}
								if (!flag)
								{
									PawnGenerator.tmpMissingParts.Clear();
									return false;
								}
							}
						}
					}
				}
				PawnGenerator.tmpMissingParts.Clear();
			}
			if (request.KindDef.forcedTraits != null)
			{
				using (List<TraitRequirement>.Enumerator enumerator3 = request.KindDef.forcedTraits.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						if (!enumerator3.Current.HasTrait(pawn))
						{
							return false;
						}
					}
				}
			}
			if (request.ForcedTraits != null)
			{
				foreach (TraitDef tDef in request.ForcedTraits)
				{
					if (!pawn.story.traits.HasTrait(tDef))
					{
						return false;
					}
				}
			}
			if (request.KindDef.fixedGender != null && pawn.gender != request.KindDef.fixedGender.Value)
			{
				return false;
			}
			if (request.ValidatorPreGear != null && !request.ValidatorPreGear(pawn))
			{
				return false;
			}
			if (request.ValidatorPostGear != null && !request.ValidatorPostGear(pawn))
			{
				return false;
			}
			if (request.FixedBiologicalAge != null)
			{
				float ageBiologicalYearsFloat = pawn.ageTracker.AgeBiologicalYearsFloat;
				float? num = request.FixedBiologicalAge;
				if (!(ageBiologicalYearsFloat == num.GetValueOrDefault() & num != null))
				{
					return false;
				}
			}
			if (request.FixedChronologicalAge != null)
			{
				float num2 = (float)pawn.ageTracker.AgeChronologicalYears;
				float? num = request.FixedChronologicalAge;
				if (!(num2 == num.GetValueOrDefault() & num != null))
				{
					return false;
				}
			}
			if (request.KindDef.chronologicalAgeRange != null && !request.KindDef.chronologicalAgeRange.Value.Includes((float)pawn.ageTracker.AgeChronologicalYears))
			{
				return false;
			}
			if (request.FixedGender != null)
			{
				Gender gender = pawn.gender;
				Gender? fixedGender = request.FixedGender;
				if (!(gender == fixedGender.GetValueOrDefault() & fixedGender != null))
				{
					return false;
				}
			}
			if (request.FixedLastName != null && (!(pawn.Name is NameTriple) || ((NameTriple)pawn.Name).Last != request.FixedLastName))
			{
				return false;
			}
			if (request.FixedMelanin != null && pawn.story != null)
			{
				float melanin = pawn.story.melanin;
				float? num = request.FixedMelanin;
				if (!(melanin == num.GetValueOrDefault() & num != null))
				{
					return false;
				}
			}
			if (request.FixedTitle != null && (pawn.royalty == null || !pawn.royalty.HasTitle(request.FixedTitle)))
			{
				return false;
			}
			if (request.ForceNoIdeo && pawn.Ideo != null)
			{
				return false;
			}
			if (request.ForceNoBackstory && (pawn.story.adulthood != null || pawn.story.childhood != null))
			{
				return false;
			}
			if (request.KindDef.minTitleRequired != null)
			{
				if (pawn.royalty == null)
				{
					return false;
				}
				RoyalTitleDef royalTitleDef = pawn.royalty.MainTitle();
				if (royalTitleDef == null || royalTitleDef.seniority < request.KindDef.minTitleRequired.seniority)
				{
					return false;
				}
			}
			if (request.Context == PawnGenerationContext.PlayerStarter && Find.Scenario != null && !Find.Scenario.AllowPlayerStartingPawn(pawn, true, request))
			{
				return false;
			}
			if (request.MustBeCapableOfViolence)
			{
				if (pawn.WorkTagIsDisabled(WorkTags.Violent))
				{
					return false;
				}
				if (pawn.RaceProps.ToolUser && !pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
				{
					return false;
				}
			}
			return (request.KindDef.requiredWorkTags == WorkTags.None || pawn.kindDef == request.KindDef || (pawn.CombinedDisabledWorkTags & request.KindDef.requiredWorkTags) == WorkTags.None) && PawnGenerator.HasCorrectMinBestSkillLevel(pawn, request.KindDef) && PawnGenerator.HasCorrectMinTotalSkillLevels(pawn, request.KindDef) && (pawn.royalty == null || !pawn.royalty.AllTitlesForReading.Any<RoyalTitle>() || request.KindDef.titleRequired != null || !request.KindDef.titleSelectOne.NullOrEmpty<RoyalTitleDef>() || request.KindDef == pawn.kindDef) && (request.RedressValidator == null || request.RedressValidator(pawn)) && (request.KindDef.requiredWorkTags == WorkTags.None || pawn.kindDef == request.KindDef || (pawn.CombinedDisabledWorkTags & request.KindDef.requiredWorkTags) == WorkTags.None);
		}

		// Token: 0x060014CD RID: 5325 RVA: 0x0007889C File Offset: 0x00076A9C
		private static bool HasCorrectMinBestSkillLevel(Pawn pawn, PawnKindDef kind)
		{
			if (kind.minBestSkillLevel <= 0)
			{
				return true;
			}
			int num = 0;
			for (int i = 0; i < pawn.skills.skills.Count; i++)
			{
				num = Mathf.Max(num, pawn.skills.skills[i].Level);
				if (num >= kind.minBestSkillLevel)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060014CE RID: 5326 RVA: 0x000788FC File Offset: 0x00076AFC
		private static bool HasCorrectMinTotalSkillLevels(Pawn pawn, PawnKindDef kind)
		{
			if (kind.minTotalSkillLevels <= 0)
			{
				return true;
			}
			int num = 0;
			for (int i = 0; i < pawn.skills.skills.Count; i++)
			{
				num += pawn.skills.skills[i].Level;
				if (num >= kind.minTotalSkillLevels)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060014CF RID: 5327 RVA: 0x00078958 File Offset: 0x00076B58
		private static Pawn GenerateNewPawnInternal(ref PawnGenerationRequest request)
		{
			Pawn pawn = null;
			string text = null;
			bool ignoreScenarioRequirements = false;
			bool ignoreValidator = false;
			for (int i = 0; i < 120; i++)
			{
				if (i == 70)
				{
					Log.Error(string.Concat(new object[]
					{
						"Could not generate a pawn after ",
						70,
						" tries. Last error: ",
						text,
						" Ignoring scenario requirements."
					}));
					ignoreScenarioRequirements = true;
				}
				if (i == 100)
				{
					Log.Error(string.Concat(new object[]
					{
						"Could not generate a pawn after ",
						100,
						" tries. Last error: ",
						text,
						" Ignoring validator."
					}));
					ignoreValidator = true;
				}
				PawnGenerationRequest pawnGenerationRequest = request;
				pawn = PawnGenerator.TryGenerateNewPawnInternal(ref pawnGenerationRequest, out text, ignoreScenarioRequirements, ignoreValidator);
				if (pawn != null)
				{
					request = pawnGenerationRequest;
					break;
				}
			}
			if (pawn == null)
			{
				Log.Error(string.Concat(new object[]
				{
					"Pawn generation error: ",
					text,
					" Too many tries (",
					120,
					"), returning null. Generation request: ",
					request
				}));
				return null;
			}
			return pawn;
		}

		// Token: 0x060014D0 RID: 5328 RVA: 0x00078A6C File Offset: 0x00076C6C
		private static Pawn TryGenerateNewPawnInternal(ref PawnGenerationRequest request, out string error, bool ignoreScenarioRequirements, bool ignoreValidator)
		{
			error = null;
			Pawn pawn = (Pawn)ThingMaker.MakeThing(request.KindDef.race, null);
			PawnGenerator.pawnsBeingGenerated.Add(new PawnGenerator.PawnGenerationStatus(pawn, null));
			Pawn result;
			try
			{
				pawn.kindDef = request.KindDef;
				pawn.SetFactionDirect(request.Faction);
				PawnComponentsUtility.CreateInitialComponents(pawn);
				if (request.FixedGender != null)
				{
					pawn.gender = request.FixedGender.Value;
				}
				else if (request.KindDef.fixedGender != null)
				{
					pawn.gender = request.KindDef.fixedGender.Value;
				}
				else if (pawn.RaceProps.hasGenders)
				{
					if (Rand.Value < 0.5f)
					{
						pawn.gender = Gender.Male;
					}
					else
					{
						pawn.gender = Gender.Female;
					}
				}
				else
				{
					pawn.gender = Gender.None;
				}
				PawnGenerator.GenerateRandomAge(pawn, request);
				pawn.needs.SetInitialLevels();
				if (!request.Newborn && request.CanGeneratePawnRelations)
				{
					PawnGenerator.GeneratePawnRelations(pawn, ref request);
				}
				if (pawn.RaceProps.Humanlike)
				{
					Faction faction;
					Faction faction2;
					if (request.Faction != null)
					{
						faction = request.Faction;
					}
					else if (Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction(out faction2, false, true, TechLevel.Undefined, false))
					{
						faction = faction2;
					}
					else
					{
						faction = Faction.OfAncients;
					}
					pawn.story.skinColorOverride = pawn.kindDef.skinColorOverride;
					pawn.story.melanin = ((request.FixedMelanin != null) ? request.FixedMelanin.Value : PawnSkinColors.RandomMelanin(request.Faction));
					pawn.story.crownType = ((Rand.Value < 0.5f) ? CrownType.Average : CrownType.Narrow);
					pawn.story.hairColor = (pawn.kindDef.forcedHairColor ?? PawnHairColors.RandomHairColor(pawn.story.SkinColor, pawn.ageTracker.AgeBiologicalYears));
					if (ModsConfig.IdeologyActive)
					{
						pawn.story.favoriteColor = new Color?(DefDatabase<ColorDef>.AllDefsListForReading.RandomElement<ColorDef>().color);
					}
					PawnBioAndNameGenerator.GiveAppropriateBioAndNameTo(pawn, request.FixedLastName, faction.def, request.ForceNoBackstory);
					if (pawn.story != null)
					{
						if (request.FixedBirthName != null)
						{
							pawn.story.birthLastName = request.FixedBirthName;
						}
						else if (pawn.Name is NameTriple)
						{
							pawn.story.birthLastName = ((NameTriple)pawn.Name).Last;
						}
					}
					PawnGenerator.GenerateTraits(pawn, request);
					PawnGenerator.GenerateBodyType(pawn, request);
					PawnGenerator.GenerateSkills(pawn);
				}
				if (pawn.RaceProps.Animal && request.Faction != null && request.Faction.IsPlayer)
				{
					pawn.training.SetWantedRecursive(TrainableDefOf.Tameness, true);
					pawn.training.Train(TrainableDefOf.Tameness, null, true);
				}
				if (!request.ForbidAnyTitle)
				{
					RoyalTitleDef royalTitleDef = request.FixedTitle;
					if (royalTitleDef == null)
					{
						if (request.KindDef.titleRequired != null)
						{
							royalTitleDef = request.KindDef.titleRequired;
						}
						else if (!request.KindDef.titleSelectOne.NullOrEmpty<RoyalTitleDef>() && Rand.Chance(request.KindDef.royalTitleChance))
						{
							royalTitleDef = request.KindDef.titleSelectOne.RandomElementByWeight((RoyalTitleDef t) => t.commonality);
						}
					}
					if (request.KindDef.minTitleRequired != null && (royalTitleDef == null || royalTitleDef.seniority < request.KindDef.minTitleRequired.seniority))
					{
						royalTitleDef = request.KindDef.minTitleRequired;
					}
					if (royalTitleDef != null)
					{
						Faction faction3 = (request.Faction != null && request.Faction.def.HasRoyalTitles) ? request.Faction : Find.FactionManager.RandomRoyalFaction(false, false, true, TechLevel.Undefined);
						pawn.royalty.SetTitle(faction3, royalTitleDef, false, false, true);
						if (request.Faction != null && !request.Faction.IsPlayer)
						{
							PawnGenerator.PurchasePermits(pawn, faction3);
						}
						int amount = 0;
						if (royalTitleDef.GetNextTitle(faction3) != null)
						{
							amount = Rand.Range(0, royalTitleDef.GetNextTitle(faction3).favorCost - 1);
						}
						pawn.royalty.SetFavor(faction3, amount, true);
						if (royalTitleDef.maxPsylinkLevel > 0)
						{
							Hediff_Level hediff_Level = HediffMaker.MakeHediff(HediffDefOf.PsychicAmplifier, pawn, pawn.health.hediffSet.GetBrain()) as Hediff_Level;
							pawn.health.AddHediff(hediff_Level, null, null, null);
							hediff_Level.SetLevelTo(royalTitleDef.maxPsylinkLevel);
						}
					}
				}
				if (pawn.royalty != null)
				{
					pawn.royalty.allowRoomRequirements = request.KindDef.allowRoyalRoomRequirements;
					pawn.royalty.allowApparelRequirements = request.KindDef.allowRoyalApparelRequirements;
				}
				if (pawn.guest != null)
				{
					pawn.guest.RandomizeJoinStatus();
				}
				if (pawn.workSettings != null && request.Faction != null && request.Faction.IsPlayer)
				{
					pawn.workSettings.EnableAndInitialize();
				}
				if (request.Faction != null && pawn.RaceProps.Animal)
				{
					pawn.GenerateNecessaryName();
				}
				if (pawn.ideo != null && !request.ForceNoIdeo)
				{
					Ideo ideo;
					if (request.FixedIdeo != null)
					{
						pawn.ideo.SetIdeo(request.FixedIdeo);
					}
					else if (request.Faction != null && request.Faction.ideos != null)
					{
						pawn.ideo.SetIdeo(request.Faction.ideos.GetRandomIdeoForNewPawn());
					}
					else if (Find.IdeoManager.IdeosListForReading.TryRandomElement(out ideo))
					{
						pawn.ideo.SetIdeo(ideo);
					}
				}
				if (pawn.mindState != null)
				{
					pawn.mindState.SetupLastHumanMeatTick();
				}
				if (pawn.surroundings != null)
				{
					pawn.surroundings.Clear();
				}
				PawnGenerator.GenerateInitialHediffs(pawn, request);
				if (pawn.RaceProps.Humanlike)
				{
					pawn.story.hairDef = PawnStyleItemChooser.RandomHairFor(pawn);
					if (pawn.style != null)
					{
						pawn.style.beardDef = ((pawn.gender == Gender.Male) ? PawnStyleItemChooser.ChooseStyleItem<BeardDef>(pawn, null) : BeardDefOf.NoBeard);
						if (ModsConfig.IdeologyActive)
						{
							pawn.style.FaceTattoo = PawnStyleItemChooser.ChooseStyleItem<TattooDef>(pawn, new TattooType?(TattooType.Face));
							pawn.style.BodyTattoo = PawnStyleItemChooser.ChooseStyleItem<TattooDef>(pawn, new TattooType?(TattooType.Body));
						}
						else
						{
							pawn.style.SetupTattoos_NoIdeology();
						}
					}
				}
				if (Find.Scenario != null)
				{
					Find.Scenario.Notify_NewPawnGenerating(pawn, request.Context);
				}
				if (!request.AllowDead && (pawn.Dead || pawn.Destroyed))
				{
					PawnGenerator.DiscardGeneratedPawn(pawn);
					error = "Generated dead pawn.";
					result = null;
				}
				else if (!request.AllowDowned && pawn.Downed)
				{
					PawnGenerator.DiscardGeneratedPawn(pawn);
					error = "Generated downed pawn.";
					result = null;
				}
				else if (request.MustBeCapableOfViolence && ((pawn.story != null && pawn.WorkTagIsDisabled(WorkTags.Violent)) || (!pawn.RaceProps.IsMechanoid && pawn.RaceProps.ToolUser && !pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))))
				{
					PawnGenerator.DiscardGeneratedPawn(pawn);
					error = "Generated pawn incapable of violence.";
					result = null;
				}
				else
				{
					if (request.KindDef != null && !request.KindDef.skills.NullOrEmpty<SkillRange>())
					{
						List<SkillRange> skills = request.KindDef.skills;
						for (int i = 0; i < skills.Count; i++)
						{
							if (pawn.skills.GetSkill(skills[i].Skill).TotallyDisabled)
							{
								error = "Generated pawn incapable of required skill: " + skills[i].Skill.defName;
								return null;
							}
						}
					}
					if (request.KindDef.requiredWorkTags != WorkTags.None && (pawn.CombinedDisabledWorkTags & request.KindDef.requiredWorkTags) != WorkTags.None)
					{
						PawnGenerator.DiscardGeneratedPawn(pawn);
						error = "Generated pawn with disabled requiredWorkTags.";
						result = null;
					}
					else if (!PawnGenerator.HasCorrectMinBestSkillLevel(pawn, request.KindDef))
					{
						PawnGenerator.DiscardGeneratedPawn(pawn);
						error = "Generated pawn with too low best skill level.";
						result = null;
					}
					else if (!PawnGenerator.HasCorrectMinTotalSkillLevels(pawn, request.KindDef))
					{
						PawnGenerator.DiscardGeneratedPawn(pawn);
						error = "Generated pawn with bad skills.";
						result = null;
					}
					else if (!ignoreScenarioRequirements && request.Context == PawnGenerationContext.PlayerStarter && Find.Scenario != null && !Find.Scenario.AllowPlayerStartingPawn(pawn, false, request))
					{
						PawnGenerator.DiscardGeneratedPawn(pawn);
						error = "Generated pawn doesn't meet scenario requirements.";
						result = null;
					}
					else if (!ignoreValidator && request.ValidatorPreGear != null && !request.ValidatorPreGear(pawn))
					{
						PawnGenerator.DiscardGeneratedPawn(pawn);
						error = "Generated pawn didn't pass validator check (pre-gear).";
						result = null;
					}
					else
					{
						if (!request.Newborn)
						{
							PawnGenerator.GenerateGearFor(pawn, request);
						}
						if (!ignoreValidator && request.ValidatorPostGear != null && !request.ValidatorPostGear(pawn))
						{
							PawnGenerator.DiscardGeneratedPawn(pawn);
							error = "Generated pawn didn't pass validator check (post-gear).";
							result = null;
						}
						else
						{
							for (int j = 0; j < PawnGenerator.pawnsBeingGenerated.Count - 1; j++)
							{
								if (PawnGenerator.pawnsBeingGenerated[j].PawnsGeneratedInTheMeantime == null)
								{
									PawnGenerator.pawnsBeingGenerated[j] = new PawnGenerator.PawnGenerationStatus(PawnGenerator.pawnsBeingGenerated[j].Pawn, new List<Pawn>());
								}
								PawnGenerator.pawnsBeingGenerated[j].PawnsGeneratedInTheMeantime.Add(pawn);
							}
							if (pawn.Faction != null)
							{
								pawn.Faction.Notify_PawnJoined(pawn);
							}
							result = pawn;
						}
					}
				}
			}
			finally
			{
				PawnGenerator.pawnsBeingGenerated.RemoveLast<PawnGenerator.PawnGenerationStatus>();
			}
			return result;
		}

		// Token: 0x060014D1 RID: 5329 RVA: 0x000793F8 File Offset: 0x000775F8
		private static void PurchasePermits(Pawn pawn, Faction faction)
		{
			int num = 200;
			Func<RoyalTitlePermitDef, bool> <>9__0;
			do
			{
				IEnumerable<RoyalTitlePermitDef> allDefs = DefDatabase<RoyalTitlePermitDef>.AllDefs;
				Func<RoyalTitlePermitDef, bool> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((RoyalTitlePermitDef x) => x.permitPointCost > 0 && x.AvailableForPawn(pawn, faction) && !x.IsPrerequisiteOfHeldPermit(pawn, faction)));
				}
				IEnumerable<RoyalTitlePermitDef> source = allDefs.Where(predicate);
				if (!source.Any<RoyalTitlePermitDef>())
				{
					return;
				}
				pawn.royalty.AddPermit(source.RandomElement<RoyalTitlePermitDef>(), faction);
				num--;
			}
			while (num > 0);
			Log.ErrorOnce("PurchasePermits exceeded max iterations.", 947492);
		}

		// Token: 0x060014D2 RID: 5330 RVA: 0x00079484 File Offset: 0x00077684
		private static void DiscardGeneratedPawn(Pawn pawn)
		{
			if (Find.WorldPawns.Contains(pawn))
			{
				Find.WorldPawns.RemovePawn(pawn);
			}
			Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
			List<Pawn> pawnsGeneratedInTheMeantime = PawnGenerator.pawnsBeingGenerated.Last<PawnGenerator.PawnGenerationStatus>().PawnsGeneratedInTheMeantime;
			if (pawnsGeneratedInTheMeantime != null)
			{
				for (int i = 0; i < pawnsGeneratedInTheMeantime.Count; i++)
				{
					Pawn pawn2 = pawnsGeneratedInTheMeantime[i];
					if (Find.WorldPawns.Contains(pawn2))
					{
						Find.WorldPawns.RemovePawn(pawn2);
					}
					Find.WorldPawns.PassToWorld(pawn2, PawnDiscardDecideMode.Discard);
					for (int j = 0; j < PawnGenerator.pawnsBeingGenerated.Count; j++)
					{
						PawnGenerator.pawnsBeingGenerated[j].PawnsGeneratedInTheMeantime.Remove(pawn2);
					}
				}
			}
		}

		// Token: 0x060014D3 RID: 5331 RVA: 0x0007953C File Offset: 0x0007773C
		private static IEnumerable<Pawn> GetValidCandidatesToRedress(PawnGenerationRequest request)
		{
			IEnumerable<Pawn> enumerable = Find.WorldPawns.GetPawnsBySituation(WorldPawnSituation.Free);
			if (request.KindDef.factionLeader)
			{
				enumerable = enumerable.Concat(Find.WorldPawns.GetPawnsBySituation(WorldPawnSituation.FactionLeader));
			}
			return from x in enumerable
			where PawnGenerator.IsValidCandidateToRedress(x, request)
			select x;
		}

		// Token: 0x060014D4 RID: 5332 RVA: 0x0007959C File Offset: 0x0007779C
		private static float ChanceToRedressAnyWorldPawn(PawnGenerationRequest request)
		{
			int pawnsBySituationCount = Find.WorldPawns.GetPawnsBySituationCount(WorldPawnSituation.Free);
			float num = Mathf.Min(0.02f + 0.01f * ((float)pawnsBySituationCount / 10f), 0.8f);
			if (request.MinChanceToRedressWorldPawn != null)
			{
				num = Mathf.Max(num, request.MinChanceToRedressWorldPawn.Value);
			}
			return num;
		}

		// Token: 0x060014D5 RID: 5333 RVA: 0x000795FC File Offset: 0x000777FC
		private static float WorldPawnSelectionWeight(Pawn p)
		{
			if (p.RaceProps.IsFlesh && !p.relations.everSeenByPlayer && p.relations.RelatedToAnyoneOrAnyoneRelatedToMe)
			{
				return 0.1f;
			}
			return 1f;
		}

		// Token: 0x060014D6 RID: 5334 RVA: 0x00079630 File Offset: 0x00077830
		private static void GenerateGearFor(Pawn pawn, PawnGenerationRequest request)
		{
			PawnApparelGenerator.GenerateStartingApparelFor(pawn, request);
			PawnWeaponGenerator.TryGenerateWeaponFor(pawn, request);
			PawnInventoryGenerator.GenerateInventoryFor(pawn, request);
		}

		// Token: 0x060014D7 RID: 5335 RVA: 0x00079648 File Offset: 0x00077848
		private static void GenerateInitialHediffs(Pawn pawn, PawnGenerationRequest request)
		{
			int num = 0;
			do
			{
				AgeInjuryUtility.GenerateRandomOldAgeInjuries(pawn, !request.AllowDead);
				PawnTechHediffsGenerator.GenerateTechHediffsFor(pawn);
				if (!pawn.kindDef.missingParts.NullOrEmpty<MissingPart>())
				{
					using (List<MissingPart>.Enumerator enumerator = pawn.kindDef.missingParts.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							MissingPart t = enumerator.Current;
							Hediff_MissingPart hediff_MissingPart = (Hediff_MissingPart)HediffMaker.MakeHediff(HediffDefOf.MissingBodyPart, pawn, null);
							if (t.Injury != null)
							{
								hediff_MissingPart.lastInjury = t.Injury;
							}
							IEnumerable<BodyPartRecord> source = from x in pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null)
							where x.depth == BodyPartDepth.Outside && (x.def.permanentInjuryChanceFactor != 0f || x.def.pawnGeneratorCanAmputate) && !pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(x) && x.def == t.BodyPart
							select x;
							if (source.Any<BodyPartRecord>())
							{
								hediff_MissingPart.Part = source.RandomElement<BodyPartRecord>();
								pawn.health.AddHediff(hediff_MissingPart, null, null, null);
							}
						}
					}
				}
				if (request.AllowAddictions)
				{
					PawnAddictionHediffsGenerator.GenerateAddictionsAndTolerancesFor(pawn);
				}
				PawnGenerator.AddRequiredScars(pawn);
				PawnGenerator.AddBlindness(pawn);
				if ((request.AllowDead && pawn.Dead) || request.AllowDowned || !pawn.Downed)
				{
					goto IL_225;
				}
				pawn.health.Reset();
				num++;
			}
			while (num <= 80);
			Log.Warning(string.Concat(new object[]
			{
				"Could not generate old age injuries for ",
				pawn.ThingID,
				" of age ",
				pawn.ageTracker.AgeBiologicalYears,
				" that allow pawn to move after ",
				80,
				" tries. request=",
				request
			}));
			IL_225:
			if (!pawn.Dead && (request.Faction == null || !request.Faction.IsPlayer))
			{
				int num2 = 0;
				while (pawn.health.HasHediffsNeedingTend(false))
				{
					num2++;
					if (num2 > 10000)
					{
						Log.Error("Too many iterations.");
						return;
					}
					TendUtility.DoTend(null, pawn, null);
				}
			}
		}

		// Token: 0x060014D8 RID: 5336 RVA: 0x000798F0 File Offset: 0x00077AF0
		private static void GenerateRandomAge(Pawn pawn, PawnGenerationRequest request)
		{
			if (request.FixedBiologicalAge != null && request.FixedChronologicalAge != null)
			{
				float? fixedBiologicalAge = request.FixedBiologicalAge;
				float? fixedChronologicalAge = request.FixedChronologicalAge;
				if (fixedBiologicalAge.GetValueOrDefault() > fixedChronologicalAge.GetValueOrDefault() & (fixedBiologicalAge != null & fixedChronologicalAge != null))
				{
					Log.Warning(string.Concat(new object[]
					{
						"Tried to generate age for pawn ",
						pawn,
						", but pawn generation request demands biological age (",
						request.FixedBiologicalAge,
						") to be greater than chronological age (",
						request.FixedChronologicalAge,
						")."
					}));
				}
			}
			if (request.Newborn)
			{
				pawn.ageTracker.AgeBiologicalTicks = 0L;
			}
			else if (request.FixedBiologicalAge != null)
			{
				pawn.ageTracker.AgeBiologicalTicks = (long)(request.FixedBiologicalAge.Value * 3600000f);
			}
			else
			{
				int num = 0;
				float num2;
				for (;;)
				{
					if (pawn.RaceProps.ageGenerationCurve != null)
					{
						num2 = (float)Mathf.RoundToInt(Rand.ByCurve(pawn.RaceProps.ageGenerationCurve));
					}
					else if (pawn.RaceProps.IsMechanoid)
					{
						num2 = Rand.Range(0f, 2500f);
					}
					else
					{
						num2 = Rand.ByCurve(PawnGenerator.DefaultAgeGenerationCurve) * pawn.RaceProps.lifeExpectancy;
					}
					num++;
					if (num > 300)
					{
						break;
					}
					if (num2 <= (float)pawn.kindDef.maxGenerationAge && num2 >= (float)pawn.kindDef.minGenerationAge)
					{
						goto IL_1A4;
					}
				}
				Log.Error("Tried 300 times to generate age for " + pawn);
				IL_1A4:
				pawn.ageTracker.AgeBiologicalTicks = (long)(num2 * 3600000f) + (long)Rand.Range(0, 3600000);
			}
			if (request.Newborn)
			{
				pawn.ageTracker.AgeChronologicalTicks = 0L;
			}
			else if (request.FixedChronologicalAge != null)
			{
				pawn.ageTracker.AgeChronologicalTicks = (long)(request.FixedChronologicalAge.Value * 3600000f);
			}
			else if (request.KindDef.chronologicalAgeRange != null)
			{
				pawn.ageTracker.AgeChronologicalTicks = (long)(request.KindDef.chronologicalAgeRange.Value.RandomInRange * 3600000f);
			}
			else
			{
				int num3;
				if (request.CertainlyBeenInCryptosleep || Rand.Value < pawn.kindDef.backstoryCryptosleepCommonality)
				{
					float value = Rand.Value;
					if (value < 0.7f)
					{
						num3 = Rand.Range(0, 100);
					}
					else if (value < 0.95f)
					{
						num3 = Rand.Range(100, 1000);
					}
					else
					{
						int max = GenDate.Year((long)GenTicks.TicksAbs, 0f) - 2026 - pawn.ageTracker.AgeBiologicalYears;
						num3 = Rand.Range(1000, max);
					}
				}
				else
				{
					num3 = 0;
				}
				long num4 = (long)GenTicks.TicksAbs - pawn.ageTracker.AgeBiologicalTicks;
				num4 -= (long)num3 * 3600000L;
				pawn.ageTracker.BirthAbsTicks = num4;
			}
			if (pawn.ageTracker.AgeBiologicalTicks > pawn.ageTracker.AgeChronologicalTicks)
			{
				pawn.ageTracker.AgeChronologicalTicks = pawn.ageTracker.AgeBiologicalTicks;
			}
			pawn.ageTracker.ResetAgeReversalDemand(Pawn_AgeTracker.AgeReversalReason.Initial, true);
		}

		// Token: 0x060014D9 RID: 5337 RVA: 0x00079C4C File Offset: 0x00077E4C
		public static int RandomTraitDegree(TraitDef traitDef)
		{
			if (traitDef.degreeDatas.Count == 1)
			{
				return traitDef.degreeDatas[0].degree;
			}
			return traitDef.degreeDatas.RandomElementByWeight((TraitDegreeData dd) => dd.commonality).degree;
		}

		// Token: 0x060014DA RID: 5338 RVA: 0x00079CA8 File Offset: 0x00077EA8
		private static void GenerateTraits(Pawn pawn, PawnGenerationRequest request)
		{
			if (pawn.story == null)
			{
				return;
			}
			if (pawn.kindDef.forcedTraits != null)
			{
				foreach (TraitRequirement traitRequirement in pawn.kindDef.forcedTraits)
				{
					pawn.story.traits.GainTrait(new Trait(traitRequirement.def, traitRequirement.degree ?? 0, true));
				}
			}
			if (request.ForcedTraits != null)
			{
				foreach (TraitDef traitDef in request.ForcedTraits)
				{
					if (traitDef != null && !pawn.story.traits.HasTrait(traitDef))
					{
						pawn.story.traits.GainTrait(new Trait(traitDef, 0, true));
					}
				}
			}
			Backstory childhood = pawn.story.childhood;
			if (((childhood != null) ? childhood.forcedTraits : null) != null)
			{
				List<TraitEntry> forcedTraits = pawn.story.childhood.forcedTraits;
				for (int i = 0; i < forcedTraits.Count; i++)
				{
					TraitEntry traitEntry = forcedTraits[i];
					if (traitEntry.def == null)
					{
						Log.Error("Null forced trait def on " + pawn.story.childhood);
					}
					else if ((request.KindDef.disallowedTraits == null || !request.KindDef.disallowedTraits.Contains(traitEntry.def)) && !pawn.story.traits.HasTrait(traitEntry.def) && (request.ProhibitedTraits == null || !request.ProhibitedTraits.Contains(traitEntry.def)))
					{
						pawn.story.traits.GainTrait(new Trait(traitEntry.def, traitEntry.degree, false));
					}
				}
			}
			if (pawn.story.adulthood != null && pawn.story.adulthood.forcedTraits != null)
			{
				List<TraitEntry> forcedTraits2 = pawn.story.adulthood.forcedTraits;
				for (int j = 0; j < forcedTraits2.Count; j++)
				{
					TraitEntry traitEntry2 = forcedTraits2[j];
					if (traitEntry2.def == null)
					{
						Log.Error("Null forced trait def on " + pawn.story.adulthood);
					}
					else if ((request.KindDef.disallowedTraits == null || !request.KindDef.disallowedTraits.Contains(traitEntry2.def)) && !pawn.story.traits.HasTrait(traitEntry2.def) && (request.ProhibitedTraits == null || !request.ProhibitedTraits.Contains(traitEntry2.def)))
					{
						pawn.story.traits.GainTrait(new Trait(traitEntry2.def, traitEntry2.degree, false));
					}
				}
			}
			int num = Rand.RangeInclusive(2, 3);
			if (request.AllowGay && (LovePartnerRelationUtility.HasAnyLovePartnerOfTheSameGender(pawn) || LovePartnerRelationUtility.HasAnyExLovePartnerOfTheSameGender(pawn)))
			{
				Trait trait = new Trait(TraitDefOf.Gay, PawnGenerator.RandomTraitDegree(TraitDefOf.Gay), false);
				pawn.story.traits.GainTrait(trait);
			}
			Func<TraitDef, float> <>9__0;
			Predicate<SkillDef> <>9__2;
			while (pawn.story.traits.allTraits.Count < num)
			{
				PawnGenerator.<>c__DisplayClass30_1 CS$<>8__locals2 = new PawnGenerator.<>c__DisplayClass30_1();
				PawnGenerator.<>c__DisplayClass30_1 CS$<>8__locals3 = CS$<>8__locals2;
				IEnumerable<TraitDef> allDefsListForReading = DefDatabase<TraitDef>.AllDefsListForReading;
				Func<TraitDef, float> weightSelector;
				if ((weightSelector = <>9__0) == null)
				{
					weightSelector = (<>9__0 = ((TraitDef tr) => tr.GetGenderSpecificCommonality(pawn.gender)));
				}
				CS$<>8__locals3.newTraitDef = allDefsListForReading.RandomElementByWeight(weightSelector);
				if (!pawn.story.traits.HasTrait(CS$<>8__locals2.newTraitDef) && (request.KindDef.disallowedTraits == null || !request.KindDef.disallowedTraits.Contains(CS$<>8__locals2.newTraitDef)) && (request.KindDef.requiredWorkTags == WorkTags.None || (CS$<>8__locals2.newTraitDef.disabledWorkTags & request.KindDef.requiredWorkTags) == WorkTags.None) && (CS$<>8__locals2.newTraitDef != TraitDefOf.Gay || (request.AllowGay && !LovePartnerRelationUtility.HasAnyLovePartnerOfTheOppositeGender(pawn) && !LovePartnerRelationUtility.HasAnyExLovePartnerOfTheOppositeGender(pawn))) && (request.ProhibitedTraits == null || !request.ProhibitedTraits.Contains(CS$<>8__locals2.newTraitDef)) && (request.Faction == null || Faction.OfPlayerSilentFail == null || !request.Faction.HostileTo(Faction.OfPlayer) || CS$<>8__locals2.newTraitDef.allowOnHostileSpawn) && !pawn.story.traits.allTraits.Any((Trait tr) => CS$<>8__locals2.newTraitDef.ConflictsWith(tr)) && (CS$<>8__locals2.newTraitDef.requiredWorkTypes == null || !pawn.OneOfWorkTypesIsDisabled(CS$<>8__locals2.newTraitDef.requiredWorkTypes)) && !pawn.WorkTagIsDisabled(CS$<>8__locals2.newTraitDef.requiredWorkTags))
				{
					if (CS$<>8__locals2.newTraitDef.forcedPassions != null && pawn.workSettings != null)
					{
						List<SkillDef> forcedPassions = CS$<>8__locals2.newTraitDef.forcedPassions;
						Predicate<SkillDef> predicate;
						if ((predicate = <>9__2) == null)
						{
							predicate = (<>9__2 = ((SkillDef p) => p.IsDisabled(pawn.story.DisabledWorkTagsBackstoryAndTraits, pawn.GetDisabledWorkTypes(true))));
						}
						if (forcedPassions.Any(predicate))
						{
							continue;
						}
					}
					int degree = PawnGenerator.RandomTraitDegree(CS$<>8__locals2.newTraitDef);
					if ((pawn.story.childhood == null || !pawn.story.childhood.DisallowsTrait(CS$<>8__locals2.newTraitDef, degree)) && (pawn.story.adulthood == null || !pawn.story.adulthood.DisallowsTrait(CS$<>8__locals2.newTraitDef, degree)))
					{
						Trait trait2 = new Trait(CS$<>8__locals2.newTraitDef, degree, false);
						if (pawn.mindState == null || pawn.mindState.mentalBreaker == null || (pawn.mindState.mentalBreaker.BreakThresholdMinor + trait2.OffsetOfStat(StatDefOf.MentalBreakThreshold)) * trait2.MultiplierOfStat(StatDefOf.MentalBreakThreshold) <= 0.5f)
						{
							pawn.story.traits.GainTrait(trait2);
						}
					}
				}
			}
		}

		// Token: 0x060014DB RID: 5339 RVA: 0x0007A3A4 File Offset: 0x000785A4
		private static void GenerateBodyType(Pawn pawn, PawnGenerationRequest request)
		{
			if (request.ForceBodyType != null)
			{
				pawn.story.bodyType = request.ForceBodyType;
				return;
			}
			if (pawn.story.adulthood != null)
			{
				pawn.story.bodyType = pawn.story.adulthood.BodyTypeFor(pawn.gender);
				return;
			}
			if (Rand.Value < 0.5f)
			{
				pawn.story.bodyType = BodyTypeDefOf.Thin;
				return;
			}
			pawn.story.bodyType = ((pawn.gender == Gender.Female) ? BodyTypeDefOf.Female : BodyTypeDefOf.Male);
		}

		// Token: 0x060014DC RID: 5340 RVA: 0x0007A43C File Offset: 0x0007863C
		private static void GenerateSkills(Pawn pawn)
		{
			List<SkillDef> allDefsListForReading = DefDatabase<SkillDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				SkillDef skillDef = allDefsListForReading[i];
				int level = PawnGenerator.FinalLevelOfSkill(pawn, skillDef);
				pawn.skills.GetSkill(skillDef).Level = level;
			}
			PawnGenerator.<>c__DisplayClass32_0 CS$<>8__locals1;
			CS$<>8__locals1.minorPassions = 0;
			CS$<>8__locals1.majorPassions = 0;
			float num = 5f + Mathf.Clamp(Rand.Gaussian(0f, 1f), -4f, 4f);
			while (num >= 1f)
			{
				if (num >= 1.5f && Rand.Bool)
				{
					int num2 = CS$<>8__locals1.majorPassions;
					CS$<>8__locals1.majorPassions = num2 + 1;
					num -= 1.5f;
				}
				else
				{
					int num2 = CS$<>8__locals1.minorPassions;
					CS$<>8__locals1.minorPassions = num2 + 1;
					num -= 1f;
				}
			}
			foreach (SkillRecord skillRecord in pawn.skills.skills)
			{
				if (!skillRecord.TotallyDisabled)
				{
					using (List<Trait>.Enumerator enumerator2 = pawn.story.traits.allTraits.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							if (enumerator2.Current.def.RequiresPassion(skillRecord.def))
							{
								PawnGenerator.<GenerateSkills>g__CreatePassion|32_0(skillRecord, true, ref CS$<>8__locals1);
							}
						}
					}
				}
			}
			foreach (SkillRecord skillRecord2 in from sr in pawn.skills.skills
			orderby sr.Level descending
			select sr)
			{
				if (!skillRecord2.TotallyDisabled)
				{
					bool flag = false;
					using (List<Trait>.Enumerator enumerator2 = pawn.story.traits.allTraits.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							if (enumerator2.Current.def.ConflictsWithPassion(skillRecord2.def))
							{
								flag = true;
								break;
							}
						}
					}
					if (!flag)
					{
						PawnGenerator.<GenerateSkills>g__CreatePassion|32_0(skillRecord2, false, ref CS$<>8__locals1);
					}
				}
			}
		}

		// Token: 0x060014DD RID: 5341 RVA: 0x0007A69C File Offset: 0x0007889C
		private static int FinalLevelOfSkill(Pawn pawn, SkillDef sk)
		{
			float num;
			if (sk.usuallyDefinedInBackstories)
			{
				num = (float)Rand.RangeInclusive(0, 4);
			}
			else
			{
				num = Rand.ByCurve(PawnGenerator.LevelRandomCurve);
			}
			foreach (Backstory backstory in from bs in pawn.story.AllBackstories
			where bs != null
			select bs)
			{
				foreach (KeyValuePair<SkillDef, int> keyValuePair in backstory.skillGainsResolved)
				{
					if (keyValuePair.Key == sk)
					{
						num += (float)keyValuePair.Value * Rand.Range(1f, 1.4f);
					}
				}
			}
			for (int i = 0; i < pawn.story.traits.allTraits.Count; i++)
			{
				int num2 = 0;
				if (pawn.story.traits.allTraits[i].CurrentData.skillGains.TryGetValue(sk, out num2))
				{
					num += (float)num2;
				}
			}
			float num3 = Rand.Range(1f, PawnGenerator.AgeSkillMaxFactorCurve.Evaluate((float)pawn.ageTracker.AgeBiologicalYears));
			num *= num3;
			num = PawnGenerator.LevelFinalAdjustmentCurve.Evaluate(num);
			if (num > 0f)
			{
				num += (float)pawn.kindDef.extraSkillLevels;
			}
			if (pawn.kindDef.skills != null)
			{
				foreach (SkillRange skillRange in pawn.kindDef.skills)
				{
					if (skillRange.Skill == sk)
					{
						if (num < (float)skillRange.Range.min || num > (float)skillRange.Range.max)
						{
							num = (float)skillRange.Range.RandomInRange;
							break;
						}
						break;
					}
				}
			}
			return Mathf.Clamp(Mathf.RoundToInt(num), 0, 20);
		}

		// Token: 0x060014DE RID: 5342 RVA: 0x0007A8C4 File Offset: 0x00078AC4
		public static void PostProcessGeneratedGear(Thing gear, Pawn pawn)
		{
			CompQuality compQuality = gear.TryGetComp<CompQuality>();
			if (compQuality != null)
			{
				QualityCategory qualityCategory = QualityUtility.GenerateQualityGeneratingPawn(pawn.kindDef, gear.def);
				if (pawn.royalty != null && pawn.Faction != null)
				{
					RoyalTitleDef currentTitle = pawn.royalty.GetCurrentTitle(pawn.Faction);
					if (currentTitle != null)
					{
						qualityCategory = (QualityCategory)Mathf.Clamp((int)qualityCategory, (int)currentTitle.requiredMinimumApparelQuality, 6);
					}
				}
				compQuality.SetQuality(qualityCategory, ArtGenerationContext.Outsider);
			}
			if (gear.def.useHitPoints)
			{
				float randomInRange = pawn.kindDef.gearHealthRange.RandomInRange;
				if (randomInRange < 1f)
				{
					int num = Mathf.RoundToInt(randomInRange * (float)gear.MaxHitPoints);
					num = Mathf.Max(1, num);
					gear.HitPoints = num;
				}
			}
		}

		// Token: 0x060014DF RID: 5343 RVA: 0x0007A974 File Offset: 0x00078B74
		private static void GeneratePawnRelations(Pawn pawn, ref PawnGenerationRequest request)
		{
			if (!pawn.RaceProps.Humanlike)
			{
				return;
			}
			Pawn[] array = (from x in PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead
			where x.def == pawn.def
			select x).ToArray<Pawn>();
			if (array.Length == 0)
			{
				return;
			}
			int num = 0;
			foreach (Pawn pawn2 in array)
			{
				if (pawn2.Discarded)
				{
					Log.Warning(string.Concat(new object[]
					{
						"Warning during generating pawn relations for ",
						pawn,
						": Pawn ",
						pawn2,
						" is discarded, yet he was yielded by PawnUtility. Discarding a pawn means that he is no longer managed by anything."
					}));
				}
				else if (pawn2.Faction != null && pawn2.Faction.IsPlayer)
				{
					num++;
				}
			}
			float num2 = 45f;
			num2 += (float)num * 2.7f;
			PawnGenerationRequest localReq = request;
			Pair<Pawn, PawnRelationDef> pair = PawnGenerator.GenerateSamples(array, PawnGenerator.relationsGeneratableBlood, 40).RandomElementByWeightWithDefault((Pair<Pawn, PawnRelationDef> x) => x.Second.generationChanceFactor * x.Second.Worker.GenerationChance(pawn, x.First, localReq), num2 * 40f / (float)(array.Length * PawnGenerator.relationsGeneratableBlood.Length));
			if (pair.First != null)
			{
				pair.Second.Worker.CreateRelation(pawn, pair.First, ref request);
			}
			if (pawn.kindDef.generateInitialNonFamilyRelations)
			{
				Pair<Pawn, PawnRelationDef> pair2 = PawnGenerator.GenerateSamples(array, PawnGenerator.relationsGeneratableNonblood, 40).RandomElementByWeightWithDefault((Pair<Pawn, PawnRelationDef> x) => x.Second.generationChanceFactor * x.Second.Worker.GenerationChance(pawn, x.First, localReq), num2 * 40f / (float)(array.Length * PawnGenerator.relationsGeneratableNonblood.Length));
				if (pair2.First != null)
				{
					pair2.Second.Worker.CreateRelation(pawn, pair2.First, ref request);
				}
			}
		}

		// Token: 0x060014E0 RID: 5344 RVA: 0x0007AB24 File Offset: 0x00078D24
		private static Pair<Pawn, PawnRelationDef>[] GenerateSamples(Pawn[] pawns, PawnRelationDef[] relations, int count)
		{
			Pair<Pawn, PawnRelationDef>[] array = new Pair<Pawn, PawnRelationDef>[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = new Pair<Pawn, PawnRelationDef>(pawns[Rand.Range(0, pawns.Length)], relations[Rand.Range(0, relations.Length)]);
			}
			return array;
		}

		// Token: 0x060014E1 RID: 5345 RVA: 0x0007AB68 File Offset: 0x00078D68
		private static void AddRequiredScars(Pawn pawn)
		{
			if (pawn.ideo == null || pawn.ideo.Ideo == null || pawn.health == null || (pawn.story != null && pawn.story.traits != null && pawn.story.traits.HasTrait(TraitDefOf.Wimp)))
			{
				return;
			}
			int num = 0;
			using (List<Hediff>.Enumerator enumerator = pawn.health.hediffSet.hediffs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.def == HediffDefOf.Scarification)
					{
						num++;
					}
				}
			}
			int num2 = pawn.ideo.Ideo.RequiredScars;
			if (pawn.Faction != null && pawn.Faction.IsPlayer && !Rand.Chance(0.5f))
			{
				num2 = Rand.RangeInclusive(0, num2 - 1);
			}
			Func<BodyPartRecord, bool> <>9__0;
			for (int i = num; i < num2; i++)
			{
				IEnumerable<BodyPartRecord> partsToApplyOn = JobDriver_Scarify.GetPartsToApplyOn(pawn);
				Func<BodyPartRecord, bool> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((BodyPartRecord p) => JobDriver_Scarify.AvailableOnNow(pawn, p)));
				}
				List<BodyPartRecord> list = partsToApplyOn.Where(predicate).ToList<BodyPartRecord>();
				if (list.Count == 0)
				{
					break;
				}
				BodyPartRecord part = list.RandomElement<BodyPartRecord>();
				JobDriver_Scarify.Scarify(pawn, part);
			}
		}

		// Token: 0x060014E2 RID: 5346 RVA: 0x0007ACFC File Offset: 0x00078EFC
		private static void AddBlindness(Pawn pawn)
		{
			if (pawn.ideo == null || pawn.ideo.Ideo == null || pawn.health == null)
			{
				return;
			}
			if (Rand.Chance(pawn.ideo.Ideo.BlindPawnChance))
			{
				foreach (BodyPartRecord part in pawn.RaceProps.body.GetPartsWithTag(BodyPartTagDefOf.SightSource))
				{
					if (!pawn.health.hediffSet.PartIsMissing(part))
					{
						Hediff_MissingPart hediff_MissingPart = (Hediff_MissingPart)HediffMaker.MakeHediff(HediffDefOf.MissingBodyPart, pawn, null);
						hediff_MissingPart.lastInjury = HediffDefOf.Cut;
						hediff_MissingPart.Part = part;
						hediff_MissingPart.IsFresh = false;
						pawn.health.AddHediff(hediff_MissingPart, part, null, null);
					}
				}
			}
		}

		// Token: 0x060014E3 RID: 5347 RVA: 0x0007ADE0 File Offset: 0x00078FE0
		[DebugOutput("Performance", false)]
		public static void PawnGenerationHistogram()
		{
			DebugHistogram debugHistogram = new DebugHistogram((from x in Enumerable.Range(1, 20)
			select (float)x * 10f).ToArray<float>());
			for (int i = 0; i < 100; i++)
			{
				long timestamp = Stopwatch.GetTimestamp();
				Thing thing = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.Colonist, null, PawnGenerationContext.NonPlayer, -1, true, false, false, false, true, false, 1f, false, true, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, null, false, false));
				debugHistogram.Add((float)((Stopwatch.GetTimestamp() - timestamp) * 1000L / Stopwatch.Frequency));
				thing.Destroy(DestroyMode.Vanish);
			}
			debugHistogram.Display();
		}

		// Token: 0x060014E5 RID: 5349 RVA: 0x0007B14C File Offset: 0x0007934C
		[CompilerGenerated]
		internal static void <GenerateSkills>g__CreatePassion|32_0(SkillRecord record, bool force, ref PawnGenerator.<>c__DisplayClass32_0 A_2)
		{
			if (A_2.majorPassions > 0)
			{
				record.passion = Passion.Major;
				int num = A_2.majorPassions;
				A_2.majorPassions = num - 1;
				return;
			}
			if (A_2.minorPassions > 0 || force)
			{
				record.passion = Passion.Minor;
				int num = A_2.minorPassions;
				A_2.minorPassions = num - 1;
			}
		}

		// Token: 0x04000ED7 RID: 3799
		private static List<PawnGenerator.PawnGenerationStatus> pawnsBeingGenerated = new List<PawnGenerator.PawnGenerationStatus>();

		// Token: 0x04000ED8 RID: 3800
		private static PawnRelationDef[] relationsGeneratableBlood = (from rel in DefDatabase<PawnRelationDef>.AllDefsListForReading
		where rel.familyByBloodRelation && rel.generationChanceFactor > 0f
		select rel).ToArray<PawnRelationDef>();

		// Token: 0x04000ED9 RID: 3801
		private static PawnRelationDef[] relationsGeneratableNonblood = (from rel in DefDatabase<PawnRelationDef>.AllDefsListForReading
		where !rel.familyByBloodRelation && rel.generationChanceFactor > 0f
		select rel).ToArray<PawnRelationDef>();

		// Token: 0x04000EDA RID: 3802
		public const float MaxStartMinorMentalBreakThreshold = 0.5f;

		// Token: 0x04000EDB RID: 3803
		public const float JoinAsSlaveChance = 0.75f;

		// Token: 0x04000EDC RID: 3804
		public const float GenerateAllRequiredScarsChance = 0.5f;

		// Token: 0x04000EDD RID: 3805
		private static List<Hediff_MissingPart> tmpMissingParts = new List<Hediff_MissingPart>();

		// Token: 0x04000EDE RID: 3806
		private static SimpleCurve DefaultAgeGenerationCurve = new SimpleCurve
		{
			{
				new CurvePoint(0.05f, 0f),
				true
			},
			{
				new CurvePoint(0.1f, 100f),
				true
			},
			{
				new CurvePoint(0.675f, 100f),
				true
			},
			{
				new CurvePoint(0.75f, 30f),
				true
			},
			{
				new CurvePoint(0.875f, 18f),
				true
			},
			{
				new CurvePoint(1f, 10f),
				true
			},
			{
				new CurvePoint(1.125f, 3f),
				true
			},
			{
				new CurvePoint(1.25f, 0f),
				true
			}
		};

		// Token: 0x04000EDF RID: 3807
		public const float MaxGeneratedMechanoidAge = 2500f;

		// Token: 0x04000EE0 RID: 3808
		private static readonly SimpleCurve AgeSkillMaxFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(10f, 0.7f),
				true
			},
			{
				new CurvePoint(35f, 1f),
				true
			},
			{
				new CurvePoint(60f, 1.6f),
				true
			}
		};

		// Token: 0x04000EE1 RID: 3809
		private static readonly SimpleCurve LevelFinalAdjustmentCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(10f, 10f),
				true
			},
			{
				new CurvePoint(20f, 16f),
				true
			},
			{
				new CurvePoint(27f, 20f),
				true
			}
		};

		// Token: 0x04000EE2 RID: 3810
		private static readonly SimpleCurve LevelRandomCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(0.5f, 150f),
				true
			},
			{
				new CurvePoint(4f, 150f),
				true
			},
			{
				new CurvePoint(5f, 25f),
				true
			},
			{
				new CurvePoint(10f, 5f),
				true
			},
			{
				new CurvePoint(15f, 0f),
				true
			}
		};

		// Token: 0x02001A17 RID: 6679
		private struct PawnGenerationStatus
		{
			// Token: 0x17001991 RID: 6545
			// (get) Token: 0x06009B8C RID: 39820 RVA: 0x00367780 File Offset: 0x00365980
			// (set) Token: 0x06009B8D RID: 39821 RVA: 0x00367788 File Offset: 0x00365988
			public Pawn Pawn { get; private set; }

			// Token: 0x17001992 RID: 6546
			// (get) Token: 0x06009B8E RID: 39822 RVA: 0x00367791 File Offset: 0x00365991
			// (set) Token: 0x06009B8F RID: 39823 RVA: 0x00367799 File Offset: 0x00365999
			public List<Pawn> PawnsGeneratedInTheMeantime { get; private set; }

			// Token: 0x06009B90 RID: 39824 RVA: 0x003677A2 File Offset: 0x003659A2
			public PawnGenerationStatus(Pawn pawn, List<Pawn> pawnsGeneratedInTheMeantime)
			{
				this = default(PawnGenerator.PawnGenerationStatus);
				this.Pawn = pawn;
				this.PawnsGeneratedInTheMeantime = pawnsGeneratedInTheMeantime;
			}
		}
	}
}
