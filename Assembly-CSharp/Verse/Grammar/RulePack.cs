using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse.Grammar
{
	// Token: 0x02000904 RID: 2308
	public class RulePack
	{
		// Token: 0x1700091B RID: 2331
		// (get) Token: 0x0600395A RID: 14682 RVA: 0x00166F4C File Offset: 0x0016514C
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

		// Token: 0x1700091C RID: 2332
		// (get) Token: 0x0600395B RID: 14683 RVA: 0x00166FDC File Offset: 0x001651DC
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

		// Token: 0x0600395C RID: 14684 RVA: 0x0016706C File Offset: 0x0016526C
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

		// Token: 0x0600395D RID: 14685 RVA: 0x001670E0 File Offset: 0x001652E0
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
					}), false);
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
					}), false);
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
						}), false);
					}
				}
			}
			return list;
		}

		// Token: 0x040027B2 RID: 10162
		[MustTranslate]
		[TranslationCanChangeCount]
		private List<string> rulesStrings = new List<string>();

		// Token: 0x040027B3 RID: 10163
		[MayTranslate]
		[TranslationCanChangeCount]
		private List<string> rulesFiles = new List<string>();

		// Token: 0x040027B4 RID: 10164
		private List<Rule> rulesRaw;

		// Token: 0x040027B5 RID: 10165
		public List<RulePackDef> include;

		// Token: 0x040027B6 RID: 10166
		[Unsaved(false)]
		private List<Rule> rulesResolved;

		// Token: 0x040027B7 RID: 10167
		[Unsaved(false)]
		private List<Rule> untranslatedRulesResolved;

		// Token: 0x040027B8 RID: 10168
		[Unsaved(false)]
		private List<string> untranslatedRulesStrings;

		// Token: 0x040027B9 RID: 10169
		[Unsaved(false)]
		private List<string> untranslatedRulesFiles;

		// Token: 0x040027BA RID: 10170
		[Unsaved(false)]
		private List<Rule> untranslatedRulesRaw;
	}
}
