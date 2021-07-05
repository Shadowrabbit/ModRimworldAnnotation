using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse.Grammar
{
	// Token: 0x02000537 RID: 1335
	public class RulePack
	{
		// Token: 0x170007E5 RID: 2021
		// (get) Token: 0x0600283C RID: 10300 RVA: 0x000F5964 File Offset: 0x000F3B64
		public List<Rule> Rules
		{
			get
			{
				if (this.rulesResolved == null)
				{
					this.rulesResolved = RulePack.GetRulesResolved(this.rulesRaw, this.rulesStrings, this.rulesFiles);
					if (this.include != null)
					{
						foreach (RulePackDef rulePackDef in this.include)
						{
							this.rulesResolved.AddRange(rulePackDef.RulesPlusIncludes);
						}
					}
				}
				return this.rulesResolved;
			}
		}

		// Token: 0x170007E6 RID: 2022
		// (get) Token: 0x0600283D RID: 10301 RVA: 0x000F59F4 File Offset: 0x000F3BF4
		public List<Rule> UntranslatedRules
		{
			get
			{
				if (this.untranslatedRulesResolved == null)
				{
					this.untranslatedRulesResolved = RulePack.GetRulesResolved(this.untranslatedRulesRaw, this.untranslatedRulesStrings, this.untranslatedRulesFiles);
					if (this.include != null)
					{
						foreach (RulePackDef rulePackDef in this.include)
						{
							this.untranslatedRulesResolved.AddRange(rulePackDef.UntranslatedRulesPlusIncludes);
						}
					}
				}
				return this.untranslatedRulesResolved;
			}
		}

		// Token: 0x0600283E RID: 10302 RVA: 0x000F5A84 File Offset: 0x000F3C84
		public void PostLoad()
		{
			this.untranslatedRulesStrings = this.rulesStrings.ToList<string>();
			this.untranslatedRulesFiles = this.rulesFiles.ToList<string>();
			if (this.rulesRaw != null)
			{
				this.untranslatedRulesRaw = new List<Rule>();
				for (int i = 0; i < this.rulesRaw.Count; i++)
				{
					this.untranslatedRulesRaw.Add(this.rulesRaw[i].DeepCopy());
				}
			}
		}

		// Token: 0x0600283F RID: 10303 RVA: 0x000F5AF8 File Offset: 0x000F3CF8
		private static List<Rule> GetRulesResolved(List<Rule> rulesRaw, List<string> rulesStrings, List<string> rulesFiles)
		{
			List<Rule> list = new List<Rule>();
			for (int i = 0; i < rulesStrings.Count; i++)
			{
				try
				{
					Rule_String rule_String = new Rule_String(rulesStrings[i]);
					rule_String.Init();
					list.Add(rule_String);
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Exception parsing grammar rule from ",
						rulesStrings[i],
						": ",
						ex
					}));
				}
			}
			for (int j = 0; j < rulesFiles.Count; j++)
			{
				try
				{
					string[] array = rulesFiles[j].Split(new string[]
					{
						"->"
					}, StringSplitOptions.None);
					Rule_File rule_File = new Rule_File();
					rule_File.keyword = array[0].Trim();
					rule_File.path = array[1].Trim();
					rule_File.Init();
					list.Add(rule_File);
				}
				catch (Exception ex2)
				{
					Log.Error(string.Concat(new object[]
					{
						"Error initializing Rule_File ",
						rulesFiles[j],
						": ",
						ex2
					}));
				}
			}
			if (rulesRaw != null)
			{
				for (int k = 0; k < rulesRaw.Count; k++)
				{
					try
					{
						rulesRaw[k].Init();
						list.Add(rulesRaw[k]);
					}
					catch (Exception ex3)
					{
						Log.Error(string.Concat(new object[]
						{
							"Error initializing rule ",
							rulesRaw[k].ToStringSafe<Rule>(),
							": ",
							ex3
						}));
					}
				}
			}
			return list;
		}

		// Token: 0x040018D4 RID: 6356
		[MustTranslate]
		[TranslationCanChangeCount]
		private List<string> rulesStrings = new List<string>();

		// Token: 0x040018D5 RID: 6357
		[MayTranslate]
		[TranslationCanChangeCount]
		private List<string> rulesFiles = new List<string>();

		// Token: 0x040018D6 RID: 6358
		private List<Rule> rulesRaw;

		// Token: 0x040018D7 RID: 6359
		public List<RulePackDef> include;

		// Token: 0x040018D8 RID: 6360
		[Unsaved(false)]
		private List<Rule> rulesResolved;

		// Token: 0x040018D9 RID: 6361
		[Unsaved(false)]
		private List<Rule> untranslatedRulesResolved;

		// Token: 0x040018DA RID: 6362
		[Unsaved(false)]
		private List<string> untranslatedRulesStrings;

		// Token: 0x040018DB RID: 6363
		[Unsaved(false)]
		private List<string> untranslatedRulesFiles;

		// Token: 0x040018DC RID: 6364
		[Unsaved(false)]
		private List<Rule> untranslatedRulesRaw;
	}
}
