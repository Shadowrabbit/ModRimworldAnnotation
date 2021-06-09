using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020014B3 RID: 5299
	[CaseInsensitiveXMLParsing]
	public class Backstory
	{
		// Token: 0x1700115D RID: 4445
		// (get) Token: 0x06007204 RID: 29188 RVA: 0x0004CA15 File Offset: 0x0004AC15
		public RulePackDef NameMaker
		{
			get
			{
				return this.nameMakerResolved;
			}
		}

		// Token: 0x1700115E RID: 4446
		// (get) Token: 0x06007205 RID: 29189 RVA: 0x0004CA1D File Offset: 0x0004AC1D
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

		// Token: 0x1700115F RID: 4447
		// (get) Token: 0x06007206 RID: 29190 RVA: 0x0004CA2D File Offset: 0x0004AC2D
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

		// Token: 0x06007207 RID: 29191 RVA: 0x0022E368 File Offset: 0x0022C568
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

		// Token: 0x06007208 RID: 29192 RVA: 0x0004CA3D File Offset: 0x0004AC3D
		public string TitleFor(Gender g)
		{
			if (g != Gender.Female || this.titleFemale.NullOrEmpty())
			{
				return this.title;
			}
			return this.titleFemale;
		}

		// Token: 0x06007209 RID: 29193 RVA: 0x0004CA5D File Offset: 0x0004AC5D
		public string TitleCapFor(Gender g)
		{
			return this.TitleFor(g).CapitalizeFirst();
		}

		// Token: 0x0600720A RID: 29194 RVA: 0x0004CA6B File Offset: 0x0004AC6B
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

		// Token: 0x0600720B RID: 29195 RVA: 0x0004CAA0 File Offset: 0x0004ACA0
		public string TitleShortCapFor(Gender g)
		{
			return this.TitleShortFor(g).CapitalizeFirst();
		}

		// Token: 0x0600720C RID: 29196 RVA: 0x0004CAAE File Offset: 0x0004ACAE
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

		// Token: 0x0600720D RID: 29197 RVA: 0x0022E3C0 File Offset: 0x0022C5C0
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

		// Token: 0x0600720E RID: 29198 RVA: 0x0004CAD3 File Offset: 0x0004ACD3
		private bool AllowsWorkType(WorkTypeDef workType)
		{
			return (this.workDisables & workType.workTags) == WorkTags.None;
		}

		// Token: 0x0600720F RID: 29199 RVA: 0x0004CAE5 File Offset: 0x0004ACE5
		private bool AllowsWorkGiver(WorkGiverDef workGiver)
		{
			return (this.workDisables & workGiver.workTags) == WorkTags.None;
		}

		// Token: 0x06007210 RID: 29200 RVA: 0x0004CAF7 File Offset: 0x0004ACF7
		internal void AddForcedTrait(TraitDef traitDef, int degree = 0)
		{
			if (this.forcedTraits == null)
			{
				this.forcedTraits = new List<TraitEntry>();
			}
			this.forcedTraits.Add(new TraitEntry(traitDef, degree));
		}

		// Token: 0x06007211 RID: 29201 RVA: 0x0004CB1E File Offset: 0x0004AD1E
		internal void AddDisallowedTrait(TraitDef traitDef, int degree = 0)
		{
			if (this.disallowedTraits == null)
			{
				this.disallowedTraits = new List<TraitEntry>();
			}
			this.disallowedTraits.Add(new TraitEntry(traitDef, degree));
		}

		// Token: 0x06007212 RID: 29202 RVA: 0x0022E6B8 File Offset: 0x0022C8B8
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

		// Token: 0x06007213 RID: 29203 RVA: 0x0022E734 File Offset: 0x0022C934
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
					Log.Error("Adulthood backstory " + this.title + " is missing male body type. Defaulting...", false);
					this.bodyTypeMaleResolved = BodyTypeDefOf.Male;
				}
				if (this.bodyTypeFemaleResolved == null)
				{
					Log.Error("Adulthood backstory " + this.title + " is missing female body type. Defaulting...", false);
					this.bodyTypeFemaleResolved = BodyTypeDefOf.Female;
				}
			}
		}

		// Token: 0x06007214 RID: 29204 RVA: 0x0004CB45 File Offset: 0x0004AD45
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

		// Token: 0x06007215 RID: 29205 RVA: 0x0004CB5C File Offset: 0x0004AD5C
		public void SetTitle(string newTitle, string newTitleFemale)
		{
			this.title = newTitle;
			this.titleFemale = newTitleFemale;
		}

		// Token: 0x06007216 RID: 29206 RVA: 0x0004CB6C File Offset: 0x0004AD6C
		public void SetTitleShort(string newTitleShort, string newTitleShortFemale)
		{
			this.titleShort = newTitleShort;
			this.titleShortFemale = newTitleShortFemale;
		}

		// Token: 0x06007217 RID: 29207 RVA: 0x0004CB7C File Offset: 0x0004AD7C
		public override string ToString()
		{
			if (this.title.NullOrEmpty())
			{
				return "(NullTitleBackstory)";
			}
			return "(" + this.title + ")";
		}

		// Token: 0x06007218 RID: 29208 RVA: 0x0004CBA6 File Offset: 0x0004ADA6
		public override int GetHashCode()
		{
			return this.identifier.GetHashCode();
		}

		// Token: 0x04004AFF RID: 19199
		public string identifier;

		// Token: 0x04004B00 RID: 19200
		public BackstorySlot slot;

		// Token: 0x04004B01 RID: 19201
		public string title;

		// Token: 0x04004B02 RID: 19202
		public string titleFemale;

		// Token: 0x04004B03 RID: 19203
		public string titleShort;

		// Token: 0x04004B04 RID: 19204
		public string titleShortFemale;

		// Token: 0x04004B05 RID: 19205
		public string baseDesc;

		// Token: 0x04004B06 RID: 19206
		private Dictionary<string, int> skillGains = new Dictionary<string, int>();

		// Token: 0x04004B07 RID: 19207
		[Unsaved(false)]
		public Dictionary<SkillDef, int> skillGainsResolved = new Dictionary<SkillDef, int>();

		// Token: 0x04004B08 RID: 19208
		public WorkTags workDisables;

		// Token: 0x04004B09 RID: 19209
		public WorkTags requiredWorkTags;

		// Token: 0x04004B0A RID: 19210
		public List<string> spawnCategories = new List<string>();

		// Token: 0x04004B0B RID: 19211
		[LoadAlias("bodyNameGlobal")]
		private string bodyTypeGlobal;

		// Token: 0x04004B0C RID: 19212
		[LoadAlias("bodyNameFemale")]
		private string bodyTypeFemale;

		// Token: 0x04004B0D RID: 19213
		[LoadAlias("bodyNameMale")]
		private string bodyTypeMale;

		// Token: 0x04004B0E RID: 19214
		[Unsaved(false)]
		private BodyTypeDef bodyTypeGlobalResolved;

		// Token: 0x04004B0F RID: 19215
		[Unsaved(false)]
		private BodyTypeDef bodyTypeFemaleResolved;

		// Token: 0x04004B10 RID: 19216
		[Unsaved(false)]
		private BodyTypeDef bodyTypeMaleResolved;

		// Token: 0x04004B11 RID: 19217
		public List<TraitEntry> forcedTraits;

		// Token: 0x04004B12 RID: 19218
		public List<TraitEntry> disallowedTraits;

		// Token: 0x04004B13 RID: 19219
		public List<string> hairTags;

		// Token: 0x04004B14 RID: 19220
		private string nameMaker;

		// Token: 0x04004B15 RID: 19221
		private RulePackDef nameMakerResolved;

		// Token: 0x04004B16 RID: 19222
		public bool shuffleable = true;

		// Token: 0x04004B17 RID: 19223
		[Unsaved(false)]
		public string untranslatedTitle;

		// Token: 0x04004B18 RID: 19224
		[Unsaved(false)]
		public string untranslatedTitleFemale;

		// Token: 0x04004B19 RID: 19225
		[Unsaved(false)]
		public string untranslatedTitleShort;

		// Token: 0x04004B1A RID: 19226
		[Unsaved(false)]
		public string untranslatedTitleShortFemale;

		// Token: 0x04004B1B RID: 19227
		[Unsaved(false)]
		public string untranslatedDesc;

		// Token: 0x04004B1C RID: 19228
		[Unsaved(false)]
		public bool titleTranslated;

		// Token: 0x04004B1D RID: 19229
		[Unsaved(false)]
		public bool titleFemaleTranslated;

		// Token: 0x04004B1E RID: 19230
		[Unsaved(false)]
		public bool titleShortTranslated;

		// Token: 0x04004B1F RID: 19231
		[Unsaved(false)]
		public bool titleShortFemaleTranslated;

		// Token: 0x04004B20 RID: 19232
		[Unsaved(false)]
		public bool descTranslated;

		// Token: 0x04004B21 RID: 19233
		private List<string> unlockedMeditationTypesTemp = new List<string>();
	}
}
