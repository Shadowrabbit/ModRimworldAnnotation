using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D9A RID: 7578
	public class RoyalTitleDef : Def
	{
		// Token: 0x17001935 RID: 6453
		// (get) Token: 0x0600A487 RID: 42119 RVA: 0x0006D13B File Offset: 0x0006B33B
		public bool Awardable
		{
			get
			{
				return this.favorCost > 0;
			}
		}

		// Token: 0x17001936 RID: 6454
		// (get) Token: 0x0600A488 RID: 42120 RVA: 0x0006D146 File Offset: 0x0006B346
		public IEnumerable<WorkTypeDef> DisabledWorkTypes
		{
			get
			{
				List<WorkTypeDef> list = DefDatabase<WorkTypeDef>.AllDefsListForReading;
				int num;
				for (int i = 0; i < list.Count; i = num + 1)
				{
					if ((this.disabledWorkTags & list[i].workTags) != WorkTags.None)
					{
						yield return list[i];
					}
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x17001937 RID: 6455
		// (get) Token: 0x0600A489 RID: 42121 RVA: 0x0006D156 File Offset: 0x0006B356
		public RoyalTitleInheritanceWorker InheritanceWorkerOverride
		{
			get
			{
				if (this.inheritanceWorkerOverride == null && this.inheritanceWorkerOverrideClass != null)
				{
					this.inheritanceWorkerOverride = (RoyalTitleInheritanceWorker)Activator.CreateInstance(this.inheritanceWorkerOverrideClass);
				}
				return this.inheritanceWorkerOverride;
			}
		}

		// Token: 0x0600A48A RID: 42122 RVA: 0x0006D18A File Offset: 0x0006B38A
		public RoyalTitleInheritanceWorker GetInheritanceWorker(Faction faction)
		{
			if (this.inheritanceWorkerOverrideClass == null)
			{
				return faction.def.RoyalTitleInheritanceWorker;
			}
			return this.InheritanceWorkerOverride;
		}

		// Token: 0x17001938 RID: 6456
		// (get) Token: 0x0600A48B RID: 42123 RVA: 0x002FE364 File Offset: 0x002FC564
		public float MinThroneRoomImpressiveness
		{
			get
			{
				if (this.throneRoomRequirements.NullOrEmpty<RoomRequirement>())
				{
					return 0f;
				}
				RoomRequirement_Impressiveness roomRequirement_Impressiveness = this.throneRoomRequirements.OfType<RoomRequirement_Impressiveness>().FirstOrDefault<RoomRequirement_Impressiveness>();
				if (roomRequirement_Impressiveness == null)
				{
					return 0f;
				}
				return (float)roomRequirement_Impressiveness.impressiveness;
			}
		}

		// Token: 0x17001939 RID: 6457
		// (get) Token: 0x0600A48C RID: 42124 RVA: 0x0006D1AC File Offset: 0x0006B3AC
		public RoyalTitleAwardWorker AwardWorker
		{
			get
			{
				if (this.awardWorker == null)
				{
					this.awardWorker = (RoyalTitleAwardWorker)Activator.CreateInstance(this.awardWorkerClass);
					this.awardWorker.def = this;
				}
				return this.awardWorker;
			}
		}

		// Token: 0x0600A48D RID: 42125 RVA: 0x0006D1DE File Offset: 0x0006B3DE
		public string GetLabelFor(Pawn p)
		{
			if (p == null)
			{
				return this.GetLabelForBothGenders();
			}
			return this.GetLabelFor(p.gender);
		}

		// Token: 0x0600A48E RID: 42126 RVA: 0x0006D1F6 File Offset: 0x0006B3F6
		public string GetLabelFor(Gender g)
		{
			if (g != Gender.Female)
			{
				return this.label;
			}
			if (string.IsNullOrEmpty(this.labelFemale))
			{
				return this.label;
			}
			return this.labelFemale;
		}

		// Token: 0x0600A48F RID: 42127 RVA: 0x0006D21D File Offset: 0x0006B41D
		public string GetLabelForBothGenders()
		{
			if (!string.IsNullOrEmpty(this.labelFemale))
			{
				return this.label + " / " + this.labelFemale;
			}
			return this.label;
		}

		// Token: 0x0600A490 RID: 42128 RVA: 0x0006D249 File Offset: 0x0006B449
		public string GetLabelCapForBothGenders()
		{
			if (!string.IsNullOrEmpty(this.labelFemale))
			{
				return base.LabelCap + " / " + this.labelFemale.CapitalizeFirst();
			}
			return base.LabelCap;
		}

		// Token: 0x0600A491 RID: 42129 RVA: 0x0006D289 File Offset: 0x0006B489
		public string GetLabelCapFor(Pawn p)
		{
			return this.GetLabelFor(p).CapitalizeFirst(this);
		}

		// Token: 0x0600A492 RID: 42130 RVA: 0x0006D298 File Offset: 0x0006B498
		public IEnumerable<RoomRequirement> GetBedroomRequirements(Pawn p)
		{
			if (p.story.traits.HasTrait(TraitDefOf.Ascetic))
			{
				return null;
			}
			return this.bedroomRequirements;
		}

		// Token: 0x0600A493 RID: 42131 RVA: 0x0006D2B9 File Offset: 0x0006B4B9
		public string GetReportText(Faction faction)
		{
			return this.description + "\n\n" + RoyalTitleUtility.GetTitleProgressionInfo(faction, null);
		}

		// Token: 0x0600A494 RID: 42132 RVA: 0x0006D2D2 File Offset: 0x0006B4D2
		public bool JoyKindDisabled(JoyKindDef joyKind)
		{
			return this.disabledJoyKinds != null && this.disabledJoyKinds.Contains(joyKind);
		}

		// Token: 0x0600A495 RID: 42133 RVA: 0x002FE3A8 File Offset: 0x002FC5A8
		private bool HasSameRoomRequirement(RoomRequirement otherReq, List<RoomRequirement> list)
		{
			if (list == null)
			{
				return false;
			}
			using (List<RoomRequirement>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.SameOrSubsetOf(otherReq))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600A496 RID: 42134 RVA: 0x0006D2EA File Offset: 0x0006B4EA
		public bool HasSameThroneroomRequirement(RoomRequirement otherReq)
		{
			return this.HasSameRoomRequirement(otherReq, this.throneRoomRequirements);
		}

		// Token: 0x0600A497 RID: 42135 RVA: 0x0006D2F9 File Offset: 0x0006B4F9
		public bool HasSameBedroomRequirement(RoomRequirement otherReq)
		{
			return this.HasSameRoomRequirement(otherReq, this.bedroomRequirements);
		}

		// Token: 0x0600A498 RID: 42136 RVA: 0x002FE404 File Offset: 0x002FC604
		public int MaxAllowedPsylinkLevel(FactionDef faction)
		{
			int result = 0;
			for (int i = 0; i < faction.royalImplantRules.Count; i++)
			{
				RoyalImplantRule royalImplantRule = faction.royalImplantRules[i];
				if (royalImplantRule.implantHediff == HediffDefOf.PsychicAmplifier && royalImplantRule.minTitle.Awardable && royalImplantRule.minTitle.seniority <= this.seniority)
				{
					result = royalImplantRule.maxLevel;
				}
			}
			return result;
		}

		// Token: 0x0600A499 RID: 42137 RVA: 0x002FE46C File Offset: 0x002FC66C
		public IEnumerable<ThingDef> SatisfyingMeals(bool includeDrugs = true)
		{
			if (includeDrugs)
			{
				if (this.satisfyingMealsCached == null)
				{
					this.satisfyingMealsCached = (from t in DefDatabase<ThingDef>.AllDefsListForReading
					where this.foodRequirement.Acceptable(t)
					orderby t.ingestible.preferability descending
					select t).ToList<ThingDef>();
				}
			}
			else if (this.satisfyingMealsNoDrugsCached == null)
			{
				this.satisfyingMealsNoDrugsCached = (from t in DefDatabase<ThingDef>.AllDefsListForReading
				where this.foodRequirement.Acceptable(t) && !t.IsDrug
				orderby t.ingestible.preferability descending
				select t).ToList<ThingDef>();
			}
			if (!includeDrugs)
			{
				return this.satisfyingMealsNoDrugsCached;
			}
			return this.satisfyingMealsCached;
		}

		// Token: 0x0600A49A RID: 42138 RVA: 0x0006D308 File Offset: 0x0006B508
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			if (!this.permits.NullOrEmpty<RoyalTitlePermitDef>())
			{
				TaggedString taggedString = "RoyalTitleTooltipPermits".Translate();
				string valueString = (from r in this.permits
				select r.label).ToCommaList(false).CapitalizeFirst();
				string reportText = (from r in this.permits
				select r.LabelCap.ToString()).ToLineList("  - ", true);
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, taggedString, valueString, reportText, 99999, null, null, false);
			}
			if (this.requiredMinimumApparelQuality > QualityCategory.Awful)
			{
				TaggedString taggedString2 = "RoyalTitleTooltipRequiredApparelQuality".Translate();
				string text = this.requiredMinimumApparelQuality.GetLabel().CapitalizeFirst();
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, taggedString2, text, text, 99998, null, null, false);
			}
			if (!this.requiredApparel.NullOrEmpty<RoyalTitleDef.ApparelRequirement>())
			{
				TaggedString taggedString3 = "RoyalTitleTooltipRequiredApparel".Translate();
				TaggedString t2 = "Male".Translate().CapitalizeFirst() + ":\n" + this.RequiredApparelListForGender(Gender.Male).ToLineList("  - ", false) + "\n\n" + "Female".Translate().CapitalizeFirst() + ":\n" + this.RequiredApparelListForGender(Gender.Female).ToLineList("  - ", false);
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, taggedString3, "", "RoyalTitleRequiredApparelStatDescription".Translate() + ":\n\n" + t2, 99998, null, null, false);
			}
			if (!this.bedroomRequirements.NullOrEmpty<RoomRequirement>())
			{
				TaggedString taggedString4 = "RoyalTitleTooltipBedroomRequirements".Translate();
				string valueString2 = (from r in this.bedroomRequirements
				select r.Label(null)).ToCommaList(false).CapitalizeFirst();
				string reportText2 = (from r in this.bedroomRequirements
				select r.LabelCap(null)).ToLineList("  - ", false);
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, taggedString4, valueString2, reportText2, 99997, null, null, false);
			}
			if (!this.throneRoomRequirements.NullOrEmpty<RoomRequirement>())
			{
				TaggedString taggedString5 = "RoyalTitleTooltipThroneroomRequirements".Translate();
				string valueString3 = (from r in this.throneRoomRequirements
				select r.Label(null)).ToCommaList(false).CapitalizeFirst();
				string reportText3 = (from r in this.throneRoomRequirements
				select r.LabelCap(null)).ToArray<string>().ToLineList("  - ");
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, taggedString5, valueString3, reportText3, 99997, null, null, false);
			}
			IEnumerable<string> enumerable = from t in this.disabledWorkTags.GetAllSelectedItems<WorkTags>()
			where t > WorkTags.None
			select t into w
			select w.LabelTranslated();
			if (enumerable.Any<string>())
			{
				TaggedString taggedString6 = "DisabledWorkTypes".Translate();
				string valueString4 = enumerable.ToCommaList(false).CapitalizeFirst();
				string reportText4 = enumerable.ToLineList(" -  ", true);
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, taggedString6, valueString4, reportText4, 99994, null, null, false);
			}
			if (this.foodRequirement.Defined && this.SatisfyingMeals(true).Any<ThingDef>())
			{
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "RoyalTitleRequiredMeals".Translate(), (from m in this.SatisfyingMeals(true)
				select m.label).ToCommaList(false).CapitalizeFirst(), "RoyalTitleRequiredMealsDesc".Translate(), 99995, null, null, false);
			}
			yield break;
		}

		// Token: 0x0600A49B RID: 42139 RVA: 0x0006D318 File Offset: 0x0006B518
		private IEnumerable<string> RequiredApparelListForGender(Gender g)
		{
			IEnumerable<RoyalTitleDef.ApparelRequirement> source = this.requiredApparel;
			Func<RoyalTitleDef.ApparelRequirement, IEnumerable<ThingDef>> <>9__0;
			Func<RoyalTitleDef.ApparelRequirement, IEnumerable<ThingDef>> selector;
			if ((selector = <>9__0) == null)
			{
				selector = (<>9__0 = ((RoyalTitleDef.ApparelRequirement r) => r.AllRequiredApparel(g)));
			}
			foreach (TaggedString taggedString in from a in source.SelectMany(selector).Distinct<ThingDef>()
			select a.LabelCap)
			{
				yield return taggedString;
			}
			IEnumerator<TaggedString> enumerator = null;
			yield return "ApparelRequirementAnyPrestigeArmor".Translate();
			yield return "ApparelRequirementAnyPsycasterApparel".Translate();
			yield break;
			yield break;
		}

		// Token: 0x0600A49C RID: 42140 RVA: 0x002FE52C File Offset: 0x002FC72C
		public IEnumerable<DefHyperlink> GetHyperlinks(Faction faction)
		{
			IEnumerable<DefHyperlink> descriptionHyperlinks = this.descriptionHyperlinks;
			return descriptionHyperlinks ?? (from t in faction.def.RoyalTitlesAllInSeniorityOrderForReading
			where t != this
			select new DefHyperlink(t, faction));
		}

		// Token: 0x0600A49D RID: 42141 RVA: 0x0006D32F File Offset: 0x0006B52F
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Royal titles are a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 1222185, false);
			}
			if (this.awardThought != null && !typeof(Thought_MemoryRoyalTitle).IsAssignableFrom(this.awardThought.thoughtClass))
			{
				yield return string.Format("Royal title {0} has awardThought with thoughtClass {1} which is not deriving from Thought_MemoryRoyalTitle!", this.defName, this.awardThought.thoughtClass.FullName);
			}
			if (this.lostThought != null && !typeof(Thought_MemoryRoyalTitle).IsAssignableFrom(this.lostThought.thoughtClass))
			{
				yield return string.Format("Royal title {0} has awardThought with thoughtClass {1} which is not deriving from Thought_MemoryRoyalTitle!", this.defName, this.awardThought.thoughtClass.FullName);
			}
			if (this.disabledJoyKinds != null)
			{
				foreach (JoyKindDef joyKindDef in this.disabledJoyKinds)
				{
					if (joyKindDef.titleRequiredAny != null && joyKindDef.titleRequiredAny.Contains(this))
					{
						yield return string.Format("Royal title {0} disables joy kind {1} which requires the title!", this.defName, joyKindDef.defName);
					}
				}
				List<JoyKindDef>.Enumerator enumerator2 = default(List<JoyKindDef>.Enumerator);
			}
			if (this.Awardable && this.changeHeirQuestPoints < 0)
			{
				yield return "undefined changeHeirQuestPoints, it's required for awardable titles";
			}
			if (!this.throneRoomRequirements.NullOrEmpty<RoomRequirement>())
			{
				foreach (RoomRequirement req in this.throneRoomRequirements)
				{
					foreach (string arg in req.ConfigErrors())
					{
						yield return string.Format("Room requirement {0}: {1}", req.GetType().Name, arg);
					}
					enumerator = null;
					req = null;
				}
				List<RoomRequirement>.Enumerator enumerator3 = default(List<RoomRequirement>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x04006F87 RID: 28551
		public int seniority;

		// Token: 0x04006F88 RID: 28552
		public int favorCost;

		// Token: 0x04006F89 RID: 28553
		[MustTranslate]
		public string labelFemale;

		// Token: 0x04006F8A RID: 28554
		public int changeHeirQuestPoints = -1;

		// Token: 0x04006F8B RID: 28555
		public float commonality = 1f;

		// Token: 0x04006F8C RID: 28556
		public WorkTags disabledWorkTags;

		// Token: 0x04006F8D RID: 28557
		public Type inheritanceWorkerOverrideClass;

		// Token: 0x04006F8E RID: 28558
		public QualityCategory requiredMinimumApparelQuality;

		// Token: 0x04006F8F RID: 28559
		public List<RoyalTitleDef.ApparelRequirement> requiredApparel;

		// Token: 0x04006F90 RID: 28560
		public List<RoyalTitlePermitDef> permits;

		// Token: 0x04006F91 RID: 28561
		public ExpectationDef minExpectation;

		// Token: 0x04006F92 RID: 28562
		public List<JoyKindDef> disabledJoyKinds;

		// Token: 0x04006F93 RID: 28563
		[NoTranslate]
		public List<string> tags;

		// Token: 0x04006F94 RID: 28564
		public List<ThingDefCountClass> rewards;

		// Token: 0x04006F95 RID: 28565
		public bool suppressIdleAlert;

		// Token: 0x04006F96 RID: 28566
		public bool canBeInherited;

		// Token: 0x04006F97 RID: 28567
		public bool allowDignifiedMeditationFocus = true;

		// Token: 0x04006F98 RID: 28568
		public int permitPointsAwarded;

		// Token: 0x04006F99 RID: 28569
		public Type awardWorkerClass;

		// Token: 0x04006F9A RID: 28570
		public ThoughtDef awardThought;

		// Token: 0x04006F9B RID: 28571
		public ThoughtDef lostThought;

		// Token: 0x04006F9C RID: 28572
		public List<RoomRequirement> throneRoomRequirements;

		// Token: 0x04006F9D RID: 28573
		public List<RoomRequirement> bedroomRequirements;

		// Token: 0x04006F9E RID: 28574
		public float recruitmentDifficultyOffset;

		// Token: 0x04006F9F RID: 28575
		public float recruitmentResistanceFactor = 1f;

		// Token: 0x04006FA0 RID: 28576
		public float recruitmentResistanceOffset;

		// Token: 0x04006FA1 RID: 28577
		public RoyalTitleFoodRequirement foodRequirement;

		// Token: 0x04006FA2 RID: 28578
		public RoyalTitleDef replaceOnRecruited;

		// Token: 0x04006FA3 RID: 28579
		public float decreeMtbDays = -1f;

		// Token: 0x04006FA4 RID: 28580
		public float decreeMinIntervalDays = 2f;

		// Token: 0x04006FA5 RID: 28581
		public float decreeMentalBreakCommonality;

		// Token: 0x04006FA6 RID: 28582
		public List<string> decreeTags;

		// Token: 0x04006FA7 RID: 28583
		public List<AbilityDef> grantedAbilities = new List<AbilityDef>();

		// Token: 0x04006FA8 RID: 28584
		public IntRange speechCooldown;

		// Token: 0x04006FA9 RID: 28585
		public int maxPsylinkLevel;

		// Token: 0x04006FAA RID: 28586
		[Unsaved(false)]
		private List<ThingDef> satisfyingMealsCached;

		// Token: 0x04006FAB RID: 28587
		[Unsaved(false)]
		private List<ThingDef> satisfyingMealsNoDrugsCached;

		// Token: 0x04006FAC RID: 28588
		private RoyalTitleAwardWorker awardWorker;

		// Token: 0x04006FAD RID: 28589
		private RoyalTitleInheritanceWorker inheritanceWorkerOverride;

		// Token: 0x02001D9B RID: 7579
		public class ApparelRequirement
		{
			// Token: 0x0600A4A2 RID: 42146 RVA: 0x0006D368 File Offset: 0x0006B568
			public IEnumerable<ThingDef> AllAllowedApparelForPawn(Pawn p, bool ignoreGender = false, bool includeWorn = false)
			{
				using (List<ThingDef>.Enumerator enumerator = DefDatabase<ThingDef>.AllDefsListForReading.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ThingDef apparel = enumerator.Current;
						if (apparel.IsApparel && apparel.apparel.tags != null && (ignoreGender || apparel.apparel.CorrectGenderForWearing(p.gender)) && apparel.apparel.tags.Any((string t) => this.requiredTags.Contains(t) || this.allowedTags.Contains(t)) && apparel.apparel.bodyPartGroups.Any((BodyPartGroupDef b) => this.bodyPartGroupsMatchAny.Contains(b)) && (includeWorn || !p.apparel.WornApparel.Any((Apparel w) => w.def == apparel)))
						{
							yield return apparel;
						}
					}
				}
				List<ThingDef>.Enumerator enumerator = default(List<ThingDef>.Enumerator);
				yield break;
				yield break;
			}

			// Token: 0x0600A4A3 RID: 42147 RVA: 0x0006D38D File Offset: 0x0006B58D
			public IEnumerable<ThingDef> AllRequiredApparelForPawn(Pawn p, bool ignoreGender = false, bool includeWorn = false)
			{
				using (List<ThingDef>.Enumerator enumerator = DefDatabase<ThingDef>.AllDefsListForReading.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ThingDef apparel = enumerator.Current;
						if (apparel.IsApparel && apparel.apparel.tags != null && (ignoreGender || apparel.apparel.CorrectGenderForWearing(p.gender)) && apparel.apparel.tags.Any((string t) => this.requiredTags.Contains(t)) && apparel.apparel.bodyPartGroups.Any((BodyPartGroupDef b) => this.bodyPartGroupsMatchAny.Contains(b)) && (includeWorn || !p.apparel.WornApparel.Any((Apparel w) => w.def == apparel)))
						{
							yield return apparel;
						}
					}
				}
				List<ThingDef>.Enumerator enumerator = default(List<ThingDef>.Enumerator);
				yield break;
				yield break;
			}

			// Token: 0x0600A4A4 RID: 42148 RVA: 0x0006D3B2 File Offset: 0x0006B5B2
			public IEnumerable<ThingDef> AllRequiredApparel(Gender gender = Gender.None)
			{
				foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefsListForReading)
				{
					if (thingDef.IsApparel && thingDef.apparel.tags != null && thingDef.apparel.tags.Any((string t) => this.requiredTags.Contains(t)) && thingDef.apparel.bodyPartGroups.Any((BodyPartGroupDef b) => this.bodyPartGroupsMatchAny.Contains(b)) && (gender == Gender.None || thingDef.apparel.CorrectGenderForWearing(gender)))
					{
						yield return thingDef;
					}
				}
				List<ThingDef>.Enumerator enumerator = default(List<ThingDef>.Enumerator);
				yield break;
				yield break;
			}

			// Token: 0x0600A4A5 RID: 42149 RVA: 0x002FE5E4 File Offset: 0x002FC7E4
			public bool ApparelMeetsRequirement(ThingDef thingDef, bool allowUnmatched = true)
			{
				bool flag = false;
				for (int i = 0; i < this.bodyPartGroupsMatchAny.Count; i++)
				{
					if (thingDef.apparel.bodyPartGroups.Contains(this.bodyPartGroupsMatchAny[i]))
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					for (int j = 0; j < this.requiredTags.Count; j++)
					{
						if (thingDef.apparel.tags.Contains(this.requiredTags[j]))
						{
							return true;
						}
					}
					if (this.allowedTags != null)
					{
						for (int k = 0; k < this.allowedTags.Count; k++)
						{
							if (thingDef.apparel.tags.Contains(this.allowedTags[k]))
							{
								return true;
							}
						}
					}
					return false;
				}
				return allowUnmatched;
			}

			// Token: 0x0600A4A6 RID: 42150 RVA: 0x002FE6A8 File Offset: 0x002FC8A8
			public bool IsMet(Pawn p)
			{
				foreach (Apparel apparel in p.apparel.WornApparel)
				{
					bool flag = false;
					for (int i = 0; i < this.bodyPartGroupsMatchAny.Count; i++)
					{
						if (apparel.def.apparel.bodyPartGroups.Contains(this.bodyPartGroupsMatchAny[i]))
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						for (int j = 0; j < this.requiredTags.Count; j++)
						{
							if (apparel.def.apparel.tags.Contains(this.requiredTags[j]))
							{
								return true;
							}
						}
						if (this.allowedTags != null)
						{
							for (int k = 0; k < this.allowedTags.Count; k++)
							{
								if (apparel.def.apparel.tags.Contains(this.allowedTags[k]))
								{
									return true;
								}
							}
						}
					}
				}
				return false;
			}

			// Token: 0x0600A4A7 RID: 42151 RVA: 0x002FE7D8 File Offset: 0x002FC9D8
			public ThingDef RandomRequiredApparelForPawnInGeneration(Pawn p, Func<ThingDef, bool> validator)
			{
				ThingDef result = null;
				Predicate<BodyPartGroupDef> <>9__2;
				Predicate<string> <>9__3;
				if (!DefDatabase<ThingDef>.AllDefsListForReading.Where(delegate(ThingDef a)
				{
					if (a.IsApparel && a.apparel.tags != null)
					{
						List<BodyPartGroupDef> bodyPartGroups = a.apparel.bodyPartGroups;
						Predicate<BodyPartGroupDef> predicate;
						if ((predicate = <>9__2) == null)
						{
							predicate = (<>9__2 = ((BodyPartGroupDef b) => this.bodyPartGroupsMatchAny.Contains(b)));
						}
						if (bodyPartGroups.Any(predicate))
						{
							List<string> tags = a.apparel.tags;
							Predicate<string> predicate2;
							if ((predicate2 = <>9__3) == null)
							{
								predicate2 = (<>9__3 = ((string t) => this.requiredTags.Contains(t)));
							}
							if (tags.Any(predicate2) && a.apparel.CorrectGenderForWearing(p.gender))
							{
								return validator == null || validator(a);
							}
						}
					}
					return false;
				}).TryRandomElementByWeight((ThingDef a) => a.generateCommonality, out result))
				{
					return null;
				}
				return result;
			}

			// Token: 0x0600A4A8 RID: 42152 RVA: 0x002FE844 File Offset: 0x002FCA44
			public override string ToString()
			{
				if (this.allowedTags == null)
				{
					return string.Format("({0}) -> {1}", string.Join(",", (from a in this.bodyPartGroupsMatchAny
					select a.defName).ToArray<string>()), string.Join(",", this.requiredTags.ToArray()));
				}
				return string.Format("({0}) -> {1}|{2}", string.Join(",", (from a in this.bodyPartGroupsMatchAny
				select a.defName).ToArray<string>()), string.Join(",", this.requiredTags.ToArray()), string.Join(",", this.allowedTags.ToArray()));
			}

			// Token: 0x04006FAE RID: 28590
			public List<BodyPartGroupDef> bodyPartGroupsMatchAny;

			// Token: 0x04006FAF RID: 28591
			public List<string> requiredTags;

			// Token: 0x04006FB0 RID: 28592
			public List<string> allowedTags;
		}
	}
}
