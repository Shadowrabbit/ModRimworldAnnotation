using System;
using System.Collections.Generic;

namespace Verse.Grammar
{
	// Token: 0x020008F6 RID: 2294
	public struct GrammarRequest
	{
		// Token: 0x17000905 RID: 2309
		// (get) Token: 0x06003900 RID: 14592 RVA: 0x0002C17F File Offset: 0x0002A37F
		public List<Rule> RulesAllowNull
		{
			get
			{
				return this.rules;
			}
		}

		// Token: 0x17000906 RID: 2310
		// (get) Token: 0x06003901 RID: 14593 RVA: 0x0002C187 File Offset: 0x0002A387
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

		// Token: 0x17000907 RID: 2311
		// (get) Token: 0x06003902 RID: 14594 RVA: 0x0002C1A2 File Offset: 0x0002A3A2
		public List<RulePack> IncludesBareAllowNull
		{
			get
			{
				return this.includesBare;
			}
		}

		// Token: 0x17000908 RID: 2312
		// (get) Token: 0x06003903 RID: 14595 RVA: 0x0002C1AA File Offset: 0x0002A3AA
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

		// Token: 0x17000909 RID: 2313
		// (get) Token: 0x06003904 RID: 14596 RVA: 0x0002C1C5 File Offset: 0x0002A3C5
		public List<RulePackDef> IncludesAllowNull
		{
			get
			{
				return this.includes;
			}
		}

		// Token: 0x1700090A RID: 2314
		// (get) Token: 0x06003905 RID: 14597 RVA: 0x0002C1CD File Offset: 0x0002A3CD
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

		// Token: 0x1700090B RID: 2315
		// (get) Token: 0x06003906 RID: 14598 RVA: 0x0002C1E8 File Offset: 0x0002A3E8
		public Dictionary<string, string> ConstantsAllowNull
		{
			get
			{
				return this.constants;
			}
		}

		// Token: 0x1700090C RID: 2316
		// (get) Token: 0x06003907 RID: 14599 RVA: 0x0002C1F0 File Offset: 0x0002A3F0
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

		// Token: 0x06003908 RID: 14600 RVA: 0x00164594 File Offset: 0x00162794
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

		// Token: 0x0400273F RID: 10047
		private List<Rule> rules;

		// Token: 0x04002740 RID: 10048
		private List<RulePack> includesBare;

		// Token: 0x04002741 RID: 10049
		private List<RulePackDef> includes;

		// Token: 0x04002742 RID: 10050
		private Dictionary<string, string> constants;
	}
}
