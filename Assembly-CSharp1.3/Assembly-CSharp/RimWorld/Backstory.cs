using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E21 RID: 3617
	[CaseInsensitiveXMLParsing]
	public class Backstory
	{
		// Token: 0x17000E31 RID: 3633
		// (get) Token: 0x0600538B RID: 21387 RVA: 0x001C4A4C File Offset: 0x001C2C4C
		public RulePackDef NameMaker
		{
			get
			{
				return this.nameMakerResolved;
			}
		}

		// Token: 0x17000E32 RID: 3634
		// (get) Token: 0x0600538C RID: 21388 RVA: 0x001C4A54 File Offset: 0x001C2C54
		public IEnumerable<WorkTypeDef> DisabledWorkTypes
		{
			get
			{
				List<WorkTypeDef> list = DefDatabase<WorkTypeDef>.AllDefsListForReading;
				int num;
				for (int i = 0; i < list.Count; i = num + 1)
				{
					if (!this.AllowsWorkType(list[i]))
					{
						yield return list[i];
					}
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x17000E33 RID: 3635
		// (get) Token: 0x0600538D RID: 21389 RVA: 0x001C4A64 File Offset: 0x001C2C64
		public IEnumerable<WorkGiverDef> DisabledWorkGivers
		{
			get
			{
				List<WorkGiverDef> list = DefDatabase<WorkGiverDef>.AllDefsListForReading;
				int num;
				for (int i = 0; i < list.Count; i = num + 1)
				{
					if (!this.AllowsWorkGiver(list[i]))
					{
						yield return list[i];
					}
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x0600538E RID: 21390 RVA: 0x001C4A74 File Offset: 0x001C2C74
		public bool DisallowsTrait(TraitDef def, int degree)
		{
			if (this.disallowedTraits == null)
			{
				return false;
			}
			for (int i = 0; i < this.disallowedTraits.Count; i++)
			{
				if (this.disallowedTraits[i].def == def && this.disallowedTraits[i].degree == degree)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600538F RID: 21391 RVA: 0x001C4ACC File Offset: 0x001C2CCC
		public string TitleFor(Gender g)
		{
			if (g != Gender.Female || this.titleFemale.NullOrEmpty())
			{
				return this.title;
			}
			return this.titleFemale;
		}

		// Token: 0x06005390 RID: 21392 RVA: 0x001C4AEC File Offset: 0x001C2CEC
		public string TitleCapFor(Gender g)
		{
			return this.TitleFor(g).CapitalizeFirst();
		}

		// Token: 0x06005391 RID: 21393 RVA: 0x001C4AFA File Offset: 0x001C2CFA
		public string TitleShortFor(Gender g)
		{
			if (g == Gender.Female && !this.titleShortFemale.NullOrEmpty())
			{
				return this.titleShortFemale;
			}
			if (!this.titleShort.NullOrEmpty())
			{
				return this.titleShort;
			}
			return this.TitleFor(g);
		}

		// Token: 0x06005392 RID: 21394 RVA: 0x001C4B2F File Offset: 0x001C2D2F
		public string TitleShortCapFor(Gender g)
		{
			return this.TitleShortFor(g).CapitalizeFirst();
		}

		// Token: 0x06005393 RID: 21395 RVA: 0x001C4B3D File Offset: 0x001C2D3D
		public BodyTypeDef BodyTypeFor(Gender g)
		{
			if (this.bodyTypeGlobalResolved != null || g == Gender.None)
			{
				return this.bodyTypeGlobalResolved;
			}
			if (g == Gender.Female)
			{
				return this.bodyTypeFemaleResolved;
			}
			return this.bodyTypeMaleResolved;
		}

		// Token: 0x06005394 RID: 21396 RVA: 0x001C4B64 File Offset: 0x001C2D64
		public TaggedString FullDescriptionFor(Pawn p)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.baseDesc.Formatted(p.Named("PAWN")).AdjustedFor(p, "PAWN", true).Resolve());
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			List<SkillDef> allDefsListForReading = DefDatabase<SkillDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				SkillDef skillDef = allDefsListForReading[i];
				if (this.skillGainsResolved.ContainsKey(skillDef))
				{
					stringBuilder.AppendLine(skillDef.skillLabel.CapitalizeFirst() + ":   " + this.skillGainsResolved[skillDef].ToString("+##;-##"));
				}
			}
			if (this.DisabledWorkTypes.Any<WorkTypeDef>() || this.DisabledWorkGivers.Any<WorkGiverDef>())
			{
				stringBuilder.AppendLine();
			}
			foreach (WorkTypeDef workTypeDef in this.DisabledWorkTypes)
			{
				stringBuilder.AppendLine(workTypeDef.gerundLabel.CapitalizeFirst() + " " + "DisabledLower".Translate());
			}
			foreach (WorkGiverDef workGiverDef in this.DisabledWorkGivers)
			{
				stringBuilder.AppendLine(workGiverDef.workType.gerundLabel.CapitalizeFirst() + ": " + workGiverDef.LabelCap + " " + "DisabledLower".Translate());
			}
			if (ModsConfig.RoyaltyActive)
			{
				this.unlockedMeditationTypesTemp.Clear();
				foreach (MeditationFocusDef meditationFocusDef in DefDatabase<MeditationFocusDef>.AllDefs)
				{
					for (int j = 0; j < meditationFocusDef.requiredBackstoriesAny.Count; j++)
					{
						BackstoryCategoryAndSlot backstoryCategoryAndSlot = meditationFocusDef.requiredBackstoriesAny[j];
						if (this.spawnCategories.Contains(backstoryCategoryAndSlot.categoryName) && backstoryCategoryAndSlot.slot == this.slot)
						{
							this.unlockedMeditationTypesTemp.Add(meditationFocusDef.LabelCap);
							break;
						}
					}
				}
				if (this.unlockedMeditationTypesTemp.Count > 0)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("MeditationFocusesUnlocked".Translate() + ": ");
					stringBuilder.AppendLine(this.unlockedMeditationTypesTemp.ToLineList("  - "));
				}
			}
			string str = stringBuilder.ToString().TrimEndNewlines();
			return Find.ActiveLanguageWorker.PostProcessed(str);
		}

		// Token: 0x06005395 RID: 21397 RVA: 0x001C4E5C File Offset: 0x001C305C
		private bool AllowsWorkType(WorkTypeDef workType)
		{
			return (this.workDisables & workType.workTags) == WorkTags.None;
		}

		// Token: 0x06005396 RID: 21398 RVA: 0x001C4E6E File Offset: 0x001C306E
		private bool AllowsWorkGiver(WorkGiverDef workGiver)
		{
			return (this.workDisables & workGiver.workTags) == WorkTags.None;
		}

		// Token: 0x06005397 RID: 21399 RVA: 0x001C4E80 File Offset: 0x001C3080
		internal void AddForcedTrait(TraitDef traitDef, int degree = 0)
		{
			if (this.forcedTraits == null)
			{
				this.forcedTraits = new List<TraitEntry>();
			}
			this.forcedTraits.Add(new TraitEntry(traitDef, degree));
		}

		// Token: 0x06005398 RID: 21400 RVA: 0x001C4EA7 File Offset: 0x001C30A7
		internal void AddDisallowedTrait(TraitDef traitDef, int degree = 0)
		{
			if (this.disallowedTraits == null)
			{
				this.disallowedTraits = new List<TraitEntry>();
			}
			this.disallowedTraits.Add(new TraitEntry(traitDef, degree));
		}

		// Token: 0x06005399 RID: 21401 RVA: 0x001C4ED0 File Offset: 0x001C30D0
		public void PostLoad()
		{
			this.untranslatedTitle = this.title;
			this.untranslatedTitleFemale = this.titleFemale;
			this.untranslatedTitleShort = this.titleShort;
			this.untranslatedTitleShortFemale = this.titleShortFemale;
			this.untranslatedDesc = this.baseDesc;
			this.baseDesc = this.baseDesc.TrimEnd(Array.Empty<char>());
			this.baseDesc = this.baseDesc.Replace("\r", "");
		}

		// Token: 0x0600539A RID: 21402 RVA: 0x001C4F4C File Offset: 0x001C314C
		public void ResolveReferences()
		{
			int num = Mathf.Abs(GenText.StableStringHash(this.baseDesc) % 100);
			string s = this.title.Replace('-', ' ');
			s = GenText.CapitalizedNoSpaces(s);
			this.identifier = GenText.RemoveNonAlphanumeric(s) + num.ToString();
			foreach (KeyValuePair<string, int> keyValuePair in this.skillGains)
			{
				this.skillGainsResolved.Add(DefDatabase<SkillDef>.GetNamed(keyValuePair.Key, true), keyValuePair.Value);
			}
			this.skillGains = null;
			if (!this.bodyTypeGlobal.NullOrEmpty())
			{
				this.bodyTypeGlobalResolved = DefDatabase<BodyTypeDef>.GetNamed(this.bodyTypeGlobal, true);
			}
			if (!this.bodyTypeFemale.NullOrEmpty())
			{
				this.bodyTypeFemaleResolved = DefDatabase<BodyTypeDef>.GetNamed(this.bodyTypeFemale, true);
			}
			if (!this.bodyTypeMale.NullOrEmpty())
			{
				this.bodyTypeMaleResolved = DefDatabase<BodyTypeDef>.GetNamed(this.bodyTypeMale, true);
			}
			if (!this.nameMaker.NullOrEmpty())
			{
				this.nameMakerResolved = DefDatabase<RulePackDef>.GetNamed(this.nameMaker, true);
			}
			if (this.slot == BackstorySlot.Adulthood && this.bodyTypeGlobalResolved == null)
			{
				if (this.bodyTypeMaleResolved == null)
				{
					Log.Error("Adulthood backstory " + this.title + " is missing male body type. Defaulting...");
					this.bodyTypeMaleResolved = BodyTypeDefOf.Male;
				}
				if (this.bodyTypeFemaleResolved == null)
				{
					Log.Error("Adulthood backstory " + this.title + " is missing female body type. Defaulting...");
					this.bodyTypeFemaleResolved = BodyTypeDefOf.Female;
				}
			}
		}

		// Token: 0x0600539B RID: 21403 RVA: 0x001C50E8 File Offset: 0x001C32E8
		public IEnumerable<string> ConfigErrors(bool ignoreNoSpawnCategories)
		{
			if (this.title.NullOrEmpty())
			{
				yield return "null title, baseDesc is " + this.baseDesc;
			}
			if (this.titleShort.NullOrEmpty())
			{
				yield return "null titleShort, baseDesc is " + this.baseDesc;
			}
			if ((this.workDisables & WorkTags.Violent) != WorkTags.None && this.spawnCategories.Contains("Pirate"))
			{
				yield return "cannot do Violent work but can spawn as a pirate";
			}
			if (this.spawnCategories.Count == 0 && !ignoreNoSpawnCategories)
			{
				yield return "no spawn categories";
			}
			if (!this.baseDesc.NullOrEmpty())
			{
				if (char.IsWhiteSpace(this.baseDesc[0]))
				{
					yield return "baseDesc starts with whitepspace";
				}
				if (char.IsWhiteSpace(this.baseDesc[this.baseDesc.Length - 1]))
				{
					yield return "baseDesc ends with whitespace";
				}
			}
			if (this.forcedTraits != null)
			{
				using (List<TraitEntry>.Enumerator enumerator = this.forcedTraits.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						TraitEntry forcedTrait = enumerator.Current;
						if (!forcedTrait.def.degreeDatas.Any((TraitDegreeData d) => d.degree == forcedTrait.degree))
						{
							yield return string.Concat(new object[]
							{
								"Backstory ",
								this.title,
								" has invalid trait ",
								forcedTrait.def.defName,
								" degree=",
								forcedTrait.degree
							});
						}
					}
				}
				List<TraitEntry>.Enumerator enumerator = default(List<TraitEntry>.Enumerator);
			}
			if (Prefs.DevMode)
			{
				foreach (KeyValuePair<SkillDef, int> keyValuePair in this.skillGainsResolved)
				{
					if (keyValuePair.Key.IsDisabled(this.workDisables, this.DisabledWorkTypes))
					{
						yield return "modifies skill " + keyValuePair.Key + " but also disables this skill";
					}
				}
				Dictionary<SkillDef, int>.Enumerator enumerator2 = default(Dictionary<SkillDef, int>.Enumerator);
				foreach (KeyValuePair<string, Backstory> keyValuePair2 in BackstoryDatabase.allBackstories)
				{
					if (keyValuePair2.Value != this && keyValuePair2.Value.identifier == this.identifier)
					{
						yield return "backstory identifier used more than once: " + this.identifier;
					}
				}
				Dictionary<string, Backstory>.Enumerator enumerator3 = default(Dictionary<string, Backstory>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x0600539C RID: 21404 RVA: 0x001C50FF File Offset: 0x001C32FF
		public void SetTitle(string newTitle, string newTitleFemale)
		{
			this.title = newTitle;
			this.titleFemale = newTitleFemale;
		}

		// Token: 0x0600539D RID: 21405 RVA: 0x001C510F File Offset: 0x001C330F
		public void SetTitleShort(string newTitleShort, string newTitleShortFemale)
		{
			this.titleShort = newTitleShort;
			this.titleShortFemale = newTitleShortFemale;
		}

		// Token: 0x0600539E RID: 21406 RVA: 0x001C511F File Offset: 0x001C331F
		public override string ToString()
		{
			if (this.title.NullOrEmpty())
			{
				return "(NullTitleBackstory)";
			}
			return "(" + this.title + ")";
		}

		// Token: 0x0600539F RID: 21407 RVA: 0x001C5149 File Offset: 0x001C3349
		public override int GetHashCode()
		{
			return this.identifier.GetHashCode();
		}

		// Token: 0x0400310C RID: 12556
		public string identifier;

		// Token: 0x0400310D RID: 12557
		public BackstorySlot slot;

		// Token: 0x0400310E RID: 12558
		public string title;

		// Token: 0x0400310F RID: 12559
		public string titleFemale;

		// Token: 0x04003110 RID: 12560
		public string titleShort;

		// Token: 0x04003111 RID: 12561
		public string titleShortFemale;

		// Token: 0x04003112 RID: 12562
		public string baseDesc;

		// Token: 0x04003113 RID: 12563
		private Dictionary<string, int> skillGains = new Dictionary<string, int>();

		// Token: 0x04003114 RID: 12564
		[Unsaved(false)]
		public Dictionary<SkillDef, int> skillGainsResolved = new Dictionary<SkillDef, int>();

		// Token: 0x04003115 RID: 12565
		public WorkTags workDisables;

		// Token: 0x04003116 RID: 12566
		public WorkTags requiredWorkTags;

		// Token: 0x04003117 RID: 12567
		public List<string> spawnCategories = new List<string>();

		// Token: 0x04003118 RID: 12568
		[LoadAlias("bodyNameGlobal")]
		private string bodyTypeGlobal;

		// Token: 0x04003119 RID: 12569
		[LoadAlias("bodyNameFemale")]
		private string bodyTypeFemale;

		// Token: 0x0400311A RID: 12570
		[LoadAlias("bodyNameMale")]
		private string bodyTypeMale;

		// Token: 0x0400311B RID: 12571
		[Unsaved(false)]
		private BodyTypeDef bodyTypeGlobalResolved;

		// Token: 0x0400311C RID: 12572
		[Unsaved(false)]
		private BodyTypeDef bodyTypeFemaleResolved;

		// Token: 0x0400311D RID: 12573
		[Unsaved(false)]
		private BodyTypeDef bodyTypeMaleResolved;

		// Token: 0x0400311E RID: 12574
		public List<TraitEntry> forcedTraits;

		// Token: 0x0400311F RID: 12575
		public List<TraitEntry> disallowedTraits;

		// Token: 0x04003120 RID: 12576
		private string nameMaker;

		// Token: 0x04003121 RID: 12577
		private RulePackDef nameMakerResolved;

		// Token: 0x04003122 RID: 12578
		public bool shuffleable = true;

		// Token: 0x04003123 RID: 12579
		[Unsaved(false)]
		public string untranslatedTitle;

		// Token: 0x04003124 RID: 12580
		[Unsaved(false)]
		public string untranslatedTitleFemale;

		// Token: 0x04003125 RID: 12581
		[Unsaved(false)]
		public string untranslatedTitleShort;

		// Token: 0x04003126 RID: 12582
		[Unsaved(false)]
		public string untranslatedTitleShortFemale;

		// Token: 0x04003127 RID: 12583
		[Unsaved(false)]
		public string untranslatedDesc;

		// Token: 0x04003128 RID: 12584
		[Unsaved(false)]
		public bool titleTranslated;

		// Token: 0x04003129 RID: 12585
		[Unsaved(false)]
		public bool titleFemaleTranslated;

		// Token: 0x0400312A RID: 12586
		[Unsaved(false)]
		public bool titleShortTranslated;

		// Token: 0x0400312B RID: 12587
		[Unsaved(false)]
		public bool titleShortFemaleTranslated;

		// Token: 0x0400312C RID: 12588
		[Unsaved(false)]
		public bool descTranslated;

		// Token: 0x0400312D RID: 12589
		private List<string> unlockedMeditationTypesTemp = new List<string>();
	}
}
