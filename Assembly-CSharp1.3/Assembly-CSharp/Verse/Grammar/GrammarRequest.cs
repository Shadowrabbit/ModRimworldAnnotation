using System;
using System.Collections.Generic;

namespace Verse.Grammar
{
	// Token: 0x02000534 RID: 1332
	public struct GrammarRequest
	{
		// Token: 0x170007DD RID: 2013
		// (get) Token: 0x0600281F RID: 10271 RVA: 0x000F4987 File Offset: 0x000F2B87
		public List<Rule> RulesAllowNull
		{
			get
			{
				return this.rules;
			}
		}

		// Token: 0x170007DE RID: 2014
		// (get) Token: 0x06002820 RID: 10272 RVA: 0x000F498F File Offset: 0x000F2B8F
		public List<Rule> Rules
		{
			get
			{
				if (this.rules == null)
				{
					this.rules = new List<Rule>();
				}
				return this.rules;
			}
		}

		// Token: 0x170007DF RID: 2015
		// (get) Token: 0x06002821 RID: 10273 RVA: 0x000F49AA File Offset: 0x000F2BAA
		public List<RulePack> IncludesBareAllowNull
		{
			get
			{
				return this.includesBare;
			}
		}

		// Token: 0x170007E0 RID: 2016
		// (get) Token: 0x06002822 RID: 10274 RVA: 0x000F49B2 File Offset: 0x000F2BB2
		public List<RulePack> IncludesBare
		{
			get
			{
				if (this.includesBare == null)
				{
					this.includesBare = new List<RulePack>();
				}
				return this.includesBare;
			}
		}

		// Token: 0x170007E1 RID: 2017
		// (get) Token: 0x06002823 RID: 10275 RVA: 0x000F49CD File Offset: 0x000F2BCD
		public List<RulePackDef> IncludesAllowNull
		{
			get
			{
				return this.includes;
			}
		}

		// Token: 0x170007E2 RID: 2018
		// (get) Token: 0x06002824 RID: 10276 RVA: 0x000F49D5 File Offset: 0x000F2BD5
		public List<RulePackDef> Includes
		{
			get
			{
				if (this.includes == null)
				{
					this.includes = new List<RulePackDef>();
				}
				return this.includes;
			}
		}

		// Token: 0x170007E3 RID: 2019
		// (get) Token: 0x06002825 RID: 10277 RVA: 0x000F49F0 File Offset: 0x000F2BF0
		public Dictionary<string, string> ConstantsAllowNull
		{
			get
			{
				return this.constants;
			}
		}

		// Token: 0x170007E4 RID: 2020
		// (get) Token: 0x06002826 RID: 10278 RVA: 0x000F49F8 File Offset: 0x000F2BF8
		public Dictionary<string, string> Constants
		{
			get
			{
				if (this.constants == null)
				{
					this.constants = new Dictionary<string, string>();
				}
				return this.constants;
			}
		}

		// Token: 0x06002827 RID: 10279 RVA: 0x000F4A14 File Offset: 0x000F2C14
		public bool HasRule(string keyword)
		{
			GrammarRequest.<>c__DisplayClass22_0 CS$<>8__locals1 = new GrammarRequest.<>c__DisplayClass22_0();
			CS$<>8__locals1.keyword = keyword;
			return (this.rules != null && this.rules.Any(new Predicate<Rule>(CS$<>8__locals1.<HasRule>g__HasTargetRule|0))) || (this.includes != null && this.includes.Any((RulePackDef i) => i.RulesPlusIncludes.Any(new Predicate<Rule>(base.<HasRule>g__HasTargetRule|0)))) || (this.includesBare != null && this.includesBare.Any((RulePack rp) => rp.Rules.Any(new Predicate<Rule>(base.<HasRule>g__HasTargetRule|0))));
		}

		// Token: 0x06002828 RID: 10280 RVA: 0x000F4A98 File Offset: 0x000F2C98
		public void Clear()
		{
			if (this.rules != null)
			{
				this.rules.Clear();
			}
			if (this.includesBare != null)
			{
				this.includesBare.Clear();
			}
			if (this.includes != null)
			{
				this.includes.Clear();
			}
			if (this.constants != null)
			{
				this.constants.Clear();
			}
		}

		// Token: 0x040018C2 RID: 6338
		private List<Rule> rules;

		// Token: 0x040018C3 RID: 6339
		private List<RulePack> includesBare;

		// Token: 0x040018C4 RID: 6340
		private List<RulePackDef> includes;

		// Token: 0x040018C5 RID: 6341
		private Dictionary<string, string> constants;

		// Token: 0x040018C6 RID: 6342
		public GrammarRequest.ICustomizer customizer;

		// Token: 0x02001CEE RID: 7406
		public interface ICustomizer
		{
			// Token: 0x0600A8AA RID: 43178
			IComparer<Rule> StrictRulePrioritizer();

			// Token: 0x0600A8AB RID: 43179
			void Notify_RuleUsed(Rule rule);

			// Token: 0x0600A8AC RID: 43180
			bool ValidateRule(Rule rule);
		}
	}
}
