using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001525 RID: 5413
	public class RoyalTitleDef : Def
	{
		// Token: 0x170015E7 RID: 5607
		// (get) Token: 0x060080B7 RID: 32951 RVA: 0x002D9895 File Offset: 0x002D7A95
		public bool Awardable
		{
			get
			{
				return this.favorCost > 0;
			}
		}

		// Token: 0x170015E8 RID: 5608
		// (get) Token: 0x060080B8 RID: 32952 RVA: 0x002D98A0 File Offset: 0x002D7AA0
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

		// Token: 0x170015E9 RID: 5609
		// (get) Token: 0x060080B9 RID: 32953 RVA: 0x002D98B0 File Offset: 0x002D7AB0
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

		// Token: 0x060080BA RID: 32954 RVA: 0x002D98E4 File Offset: 0x002D7AE4
		public RoyalTitleInheritanceWorker GetInheritanceWorker(Faction faction)
		{
			if (this.inheritanceWorkerOverrideClass == null)
			{
				return faction.def.RoyalTitleInheritanceWorker;
			}
			return this.InheritanceWorkerOverride;
		}

		// Token: 0x170015EA RID: 5610
		// (get) Token: 0x060080BB RID: 32955 RVA: 0x002D9908 File Offset: 0x002D7B08
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

		// Token: 0x170015EB RID: 5611
		// (get) Token: 0x060080BC RID: 32956 RVA: 0x002D9949 File Offset: 0x002D7B49
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

		// Token: 0x060080BD RID: 32957 RVA: 0x002D997B File Offset: 0x002D7B7B
		public string GetLabelFor(Pawn p)
		{
			if (p == null)
			{
				return this.GetLabelForBothGenders();
			}
			return this.GetLabelFor(p.gender);
		}

		// Token: 0x060080BE RID: 32958 RVA: 0x002D9993 File Offset: 0x002D7B93
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

		// Token: 0x060080BF RID: 32959 RVA: 0x002D99BA File Offset: 0x002D7BBA
		public string GetLabelForBothGenders()
		{
			if (!string.IsNullOrEmpty(this.labelFemale))
			{
				return this.label + " / " + this.labelFemale;
			}
			return this.label;
		}

		// Token: 0x060080C0 RID: 32960 RVA: 0x002D99E6 File Offset: 0x002D7BE6
		public string GetLabelCapForBothGenders()
		{
			if (!string.IsNullOrEmpty(this.labelFemale))
			{
				return this.LabelCap + " / " + this.labelFemale.CapitalizeFirst();
			}
			return this.LabelCap;
		}

		// Token: 0x060080C1 RID: 32961 RVA: 0x002D9A26 File Offset: 0x002D7C26
		public string GetLabelCapFor(Pawn p)
		{
			return this.GetLabelFor(p).CapitalizeFirst(this);
		}

		// Token: 0x060080C2 RID: 32962 RVA: 0x002D9A35 File Offset: 0x002D7C35
		public IEnumerable<RoomRequirement> GetBedroomRequirements(Pawn p)
		{
			if (p.story.traits.HasTrait(TraitDefOf.Ascetic))
			{
				return null;
			}
			return this.bedroomRequirements;
		}

		// Token: 0x060080C3 RID: 32963 RVA: 0x002D9A56 File Offset: 0x002D7C56
		public string GetReportText(Faction faction)
		{
			return this.description + "\n\n" + RoyalTitleUtility.GetTitleProgressionInfo(faction, null);
		}

		// Token: 0x060080C4 RID: 32964 RVA: 0x002D9A6F File Offset: 0x002D7C6F
		public bool JoyKindDisabled(JoyKindDef joyKind)
		{
			return this.disabledJoyKinds != null && this.disabledJoyKinds.Contains(joyKind);
		}

		// Token: 0x060080C5 RID: 32965 RVA: 0x002D9A88 File Offset: 0x002D7C88
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

		// Token: 0x060080C6 RID: 32966 RVA: 0x002D9AE4 File Offset: 0x002D7CE4
		public bool HasSameThroneroomRequirement(RoomRequirement otherReq)
		{
			return this.HasSameRoomRequirement(otherReq, this.throneRoomRequirements);
		}

		// Token: 0x060080C7 RID: 32967 RVA: 0x002D9AF3 File Offset: 0x002D7CF3
		public bool HasSameBedroomRequirement(RoomRequirement otherReq)
		{
			return this.HasSameRoomRequirement(otherReq, this.bedroomRequirements);
		}

		// Token: 0x060080C8 RID: 32968 RVA: 0x002D9B04 File Offset: 0x002D7D04
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

		// Token: 0x060080C9 RID: 32969 RVA: 0x002D9B6C File Offset: 0x002D7D6C
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

		// Token: 0x060080CA RID: 32970 RVA: 0x002D9C2B File Offset: 0x002D7E2B
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			if (!this.permits.NullOrEmpty<RoyalTitlePermitDef>())
			{
				TaggedString taggedString = "RoyalTitleTooltipPermits".Translate();
				string valueString = (from r in this.permits
				select r.label).ToCommaList(false, false).CapitalizeFirst();
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
			if (!this.requiredApparel.NullOrEmpty<ApparelRequirement>())
			{
				TaggedString taggedString3 = "RoyalTitleTooltipRequiredApparel".Translate();
				TaggedString t2 = "Male".Translate().CapitalizeFirst() + ":\n" + this.RequiredApparelListForGender(Gender.Male, req.Pawn).ToLineList("  - ", false) + "\n\n" + "Female".Translate().CapitalizeFirst() + ":\n" + this.RequiredApparelListForGender(Gender.Female, req.Pawn).ToLineList("  - ", false);
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, taggedString3, "", "RoyalTitleRequiredApparelStatDescription".Translate() + ":\n\n" + t2, 99998, null, null, false);
			}
			if (!this.bedroomRequirements.NullOrEmpty<RoomRequirement>())
			{
				TaggedString taggedString4 = "RoyalTitleTooltipBedroomRequirements".Translate();
				string valueString2 = (from r in this.bedroomRequirements
				select r.Label(null)).ToCommaList(false, false).CapitalizeFirst();
				string reportText2 = (from r in this.bedroomRequirements
				select r.LabelCap(null)).ToLineList("  - ", false);
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, taggedString4, valueString2, reportText2, 99997, null, null, false);
			}
			if (!this.throneRoomRequirements.NullOrEmpty<RoomRequirement>())
			{
				TaggedString taggedString5 = "RoyalTitleTooltipThroneroomRequirements".Translate();
				string valueString3 = (from r in this.throneRoomRequirements
				select r.Label(null)).ToCommaList(false, false).CapitalizeFirst();
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
				string valueString4 = enumerable.ToCommaList(false, false).CapitalizeFirst();
				string reportText4 = enumerable.ToLineList(" -  ", true);
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, taggedString6, valueString4, reportText4, 99994, null, null, false);
			}
			if (this.foodRequirement.Defined && this.SatisfyingMeals(true).Any<ThingDef>())
			{
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "RoyalTitleRequiredMeals".Translate(), (from m in this.SatisfyingMeals(true)
				select m.label).ToCommaList(false, false).CapitalizeFirst(), "RoyalTitleRequiredMealsDesc".Translate(), 99995, null, null, false);
			}
			yield break;
		}

		// Token: 0x060080CB RID: 32971 RVA: 0x002D9C42 File Offset: 0x002D7E42
		private IEnumerable<string> RequiredApparelListForGender(Gender g, Pawn forPawn = null)
		{
			bool anyRequirementValid = false;
			foreach (ApparelRequirement apparelRequirement in this.requiredApparel)
			{
				string appendDisabledRequirement = null;
				string t;
				if (forPawn != null && !ApparelUtility.IsRequirementActive(apparelRequirement, ApparelRequirementSource.Title, forPawn, out t))
				{
					appendDisabledRequirement = " [" + "ApparelRequirementDisabledLabel".Translate() + ": " + t + "]";
				}
				else
				{
					anyRequirementValid = true;
				}
				foreach (TaggedString taggedString in from a in apparelRequirement.AllRequiredApparel(g).Distinct<ThingDef>()
				select a.LabelCap)
				{
					if (appendDisabledRequirement == null)
					{
						yield return taggedString;
					}
					else
					{
						yield return taggedString + " " + appendDisabledRequirement;
					}
				}
				IEnumerator<TaggedString> enumerator2 = null;
				appendDisabledRequirement = null;
			}
			List<ApparelRequirement>.Enumerator enumerator = default(List<ApparelRequirement>.Enumerator);
			if (anyRequirementValid)
			{
				yield return "ApparelRequirementAnyPrestigeArmor".Translate();
				yield return "ApparelRequirementAnyPsycasterApparel".Translate();
			}
			yield break;
			yield break;
		}

		// Token: 0x060080CC RID: 32972 RVA: 0x002D9C60 File Offset: 0x002D7E60
		public IEnumerable<DefHyperlink> GetHyperlinks(Faction faction)
		{
			IEnumerable<DefHyperlink> descriptionHyperlinks = this.descriptionHyperlinks;
			return descriptionHyperlinks ?? (from t in faction.def.RoyalTitlesAllInSeniorityOrderForReading
			where t != this
			select new DefHyperlink(t, faction));
		}

		// Token: 0x060080CD RID: 32973 RVA: 0x002D9CBF File Offset: 0x002D7EBF
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			ModLister.CheckRoyalty("Royal title");
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
			if (!typeof(RoyalTitleAwardWorker).IsAssignableFrom(this.awardWorkerClass))
			{
				yield return "awardWorkerClass does not derive from RoyalTitleAwardWorker";
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

		// Token: 0x0400502B RID: 20523
		public int seniority;

		// Token: 0x0400502C RID: 20524
		public int favorCost;

		// Token: 0x0400502D RID: 20525
		[MustTranslate]
		public string labelFemale;

		// Token: 0x0400502E RID: 20526
		public int changeHeirQuestPoints = -1;

		// Token: 0x0400502F RID: 20527
		public float commonality = 1f;

		// Token: 0x04005030 RID: 20528
		public WorkTags disabledWorkTags;

		// Token: 0x04005031 RID: 20529
		public Type inheritanceWorkerOverrideClass;

		// Token: 0x04005032 RID: 20530
		public QualityCategory requiredMinimumApparelQuality;

		// Token: 0x04005033 RID: 20531
		public List<ApparelRequirement> requiredApparel;

		// Token: 0x04005034 RID: 20532
		public List<RoyalTitlePermitDef> permits;

		// Token: 0x04005035 RID: 20533
		public ExpectationDef minExpectation;

		// Token: 0x04005036 RID: 20534
		public List<JoyKindDef> disabledJoyKinds;

		// Token: 0x04005037 RID: 20535
		[NoTranslate]
		public List<string> tags;

		// Token: 0x04005038 RID: 20536
		public List<ThingDefCountClass> rewards;

		// Token: 0x04005039 RID: 20537
		public bool suppressIdleAlert;

		// Token: 0x0400503A RID: 20538
		public bool canBeInherited;

		// Token: 0x0400503B RID: 20539
		public bool allowDignifiedMeditationFocus = true;

		// Token: 0x0400503C RID: 20540
		public int permitPointsAwarded;

		// Token: 0x0400503D RID: 20541
		public Type awardWorkerClass = typeof(RoyalTitleAwardWorker);

		// Token: 0x0400503E RID: 20542
		public ThoughtDef awardThought;

		// Token: 0x0400503F RID: 20543
		public ThoughtDef lostThought;

		// Token: 0x04005040 RID: 20544
		public List<RoomRequirement> throneRoomRequirements;

		// Token: 0x04005041 RID: 20545
		public List<RoomRequirement> bedroomRequirements;

		// Token: 0x04005042 RID: 20546
		public float recruitmentResistanceOffset;

		// Token: 0x04005043 RID: 20547
		public RoyalTitleFoodRequirement foodRequirement;

		// Token: 0x04005044 RID: 20548
		public RoyalTitleDef replaceOnRecruited;

		// Token: 0x04005045 RID: 20549
		public float decreeMtbDays = -1f;

		// Token: 0x04005046 RID: 20550
		public float decreeMinIntervalDays = 2f;

		// Token: 0x04005047 RID: 20551
		public float decreeMentalBreakCommonality;

		// Token: 0x04005048 RID: 20552
		public List<string> decreeTags;

		// Token: 0x04005049 RID: 20553
		public List<AbilityDef> grantedAbilities = new List<AbilityDef>();

		// Token: 0x0400504A RID: 20554
		public IntRange speechCooldown;

		// Token: 0x0400504B RID: 20555
		public int maxPsylinkLevel;

		// Token: 0x0400504C RID: 20556
		[Unsaved(false)]
		private List<ThingDef> satisfyingMealsCached;

		// Token: 0x0400504D RID: 20557
		[Unsaved(false)]
		private List<ThingDef> satisfyingMealsNoDrugsCached;

		// Token: 0x0400504E RID: 20558
		private RoyalTitleAwardWorker awardWorker;

		// Token: 0x0400504F RID: 20559
		private RoyalTitleInheritanceWorker inheritanceWorkerOverride;
	}
}
