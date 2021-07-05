using System;
using System.Collections.Generic;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000101 RID: 257
	public class RulePackDef : Def
	{
		// Token: 0x17000146 RID: 326
		// (get) Token: 0x060006DE RID: 1758 RVA: 0x000211E8 File Offset: 0x0001F3E8
		public List<Rule> RulesPlusIncludes
		{
			get
			{
				if (this.cachedRules == null)
				{
					this.cachedRules = new List<Rule>();
					if (this.rulePack != null)
					{
						this.cachedRules.AddRange(this.rulePack.Rules);
					}
					if (this.include != null)
					{
						for (int i = 0; i < this.include.Count; i++)
						{
							this.cachedRules.AddRange(this.include[i].RulesPlusIncludes);
						}
					}
				}
				return this.cachedRules;
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x060006DF RID: 1759 RVA: 0x00021268 File Offset: 0x0001F468
		public List<Rule> UntranslatedRulesPlusIncludes
		{
			get
			{
				if (this.cachedUntranslatedRules == null)
				{
					this.cachedUntranslatedRules = new List<Rule>();
					if (this.rulePack != null)
					{
						this.cachedUntranslatedRules.AddRange(this.rulePack.UntranslatedRules);
					}
					if (this.include != null)
					{
						for (int i = 0; i < this.include.Count; i++)
						{
							this.cachedUntranslatedRules.AddRange(this.include[i].UntranslatedRulesPlusIncludes);
						}
					}
				}
				return this.cachedUntranslatedRules;
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x060006E0 RID: 1760 RVA: 0x000212E6 File Offset: 0x0001F4E6
		public List<Rule> RulesImmediate
		{
			get
			{
				if (this.rulePack == null)
				{
					return null;
				}
				return this.rulePack.Rules;
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x060006E1 RID: 1761 RVA: 0x000212FD File Offset: 0x0001F4FD
		public List<Rule> UntranslatedRulesImmediate
		{
			get
			{
				if (this.rulePack == null)
				{
					return null;
				}
				return this.rulePack.UntranslatedRules;
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x060006E2 RID: 1762 RVA: 0x00021314 File Offset: 0x0001F514
		public string FirstRuleKeyword
		{
			get
			{
				List<Rule> rulesPlusIncludes = this.RulesPlusIncludes;
				if (!rulesPlusIncludes.Any<Rule>())
				{
					return "none";
				}
				return rulesPlusIncludes[0].keyword;
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x060006E3 RID: 1763 RVA: 0x00021344 File Offset: 0x0001F544
		public string FirstUntranslatedRuleKeyword
		{
			get
			{
				List<Rule> untranslatedRulesPlusIncludes = this.UntranslatedRulesPlusIncludes;
				if (!untranslatedRulesPlusIncludes.Any<Rule>())
				{
					return "none";
				}
				return untranslatedRulesPlusIncludes[0].keyword;
			}
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x00021372 File Offset: 0x0001F572
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.include != null)
			{
				int num;
				for (int i = 0; i < this.include.Count; i = num + 1)
				{
					if (this.include[i].include != null && this.include[i].include.Contains(this))
					{
						yield return "includes other RulePackDef which includes it: " + this.include[i].defName;
					}
					num = i;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x00021382 File Offset: 0x0001F582
		public static RulePackDef Named(string defName)
		{
			return DefDatabase<RulePackDef>.GetNamed(defName, true);
		}

		// Token: 0x0400061E RID: 1566
		public List<RulePackDef> include;

		// Token: 0x0400061F RID: 1567
		private RulePack rulePack;

		// Token: 0x04000620 RID: 1568
		public bool directTestable;

		// Token: 0x04000621 RID: 1569
		[Unsaved(false)]
		private List<Rule> cachedRules;

		// Token: 0x04000622 RID: 1570
		[Unsaved(false)]
		private List<Rule> cachedUntranslatedRules;
	}
}
