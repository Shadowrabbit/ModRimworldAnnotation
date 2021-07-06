using System;
using System.Collections.Generic;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000181 RID: 385
	public class RulePackDef : Def
	{
		// Token: 0x170001CD RID: 461
		// (get) Token: 0x060009A4 RID: 2468 RVA: 0x00099BE8 File Offset: 0x00097DE8
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

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x060009A5 RID: 2469 RVA: 0x00099C68 File Offset: 0x00097E68
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

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x060009A6 RID: 2470 RVA: 0x0000D895 File Offset: 0x0000BA95
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

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x060009A7 RID: 2471 RVA: 0x0000D8AC File Offset: 0x0000BAAC
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

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x060009A8 RID: 2472 RVA: 0x00099CE8 File Offset: 0x00097EE8
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

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x060009A9 RID: 2473 RVA: 0x00099D18 File Offset: 0x00097F18
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

		// Token: 0x060009AA RID: 2474 RVA: 0x0000D8C3 File Offset: 0x0000BAC3
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

		// Token: 0x060009AB RID: 2475 RVA: 0x0000D8D3 File Offset: 0x0000BAD3
		public static RulePackDef Named(string defName)
		{
			return DefDatabase<RulePackDef>.GetNamed(defName, true);
		}

		// Token: 0x04000845 RID: 2117
		public List<RulePackDef> include;

		// Token: 0x04000846 RID: 2118
		private RulePack rulePack;

		// Token: 0x04000847 RID: 2119
		[Unsaved(false)]
		private List<Rule> cachedRules;

		// Token: 0x04000848 RID: 2120
		[Unsaved(false)]
		private List<Rule> cachedUntranslatedRules;
	}
}
