using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020013A5 RID: 5029
	public class QuickSearchFilter
	{
		// Token: 0x1700156F RID: 5487
		// (get) Token: 0x06007A6E RID: 31342 RVA: 0x002B3162 File Offset: 0x002B1362
		// (set) Token: 0x06007A6F RID: 31343 RVA: 0x002B316A File Offset: 0x002B136A
		public string Text
		{
			get
			{
				return this.inputText;
			}
			set
			{
				this.inputText = value;
				this.searchText = value.Trim();
				this.cachedMatches.Clear();
			}
		}

		// Token: 0x17001570 RID: 5488
		// (get) Token: 0x06007A70 RID: 31344 RVA: 0x002B318A File Offset: 0x002B138A
		public bool Active
		{
			get
			{
				return !this.inputText.NullOrEmpty();
			}
		}

		// Token: 0x06007A71 RID: 31345 RVA: 0x002B319C File Offset: 0x002B139C
		public bool Matches(string value)
		{
			if (!this.Active)
			{
				return true;
			}
			if (value.NullOrEmpty())
			{
				return false;
			}
			bool flag;
			if (!this.cachedMatches.TryGetValue(value, out flag))
			{
				flag = this.MatchImpl(value);
				this.cachedMatches.Add(value, flag);
			}
			return flag;
		}

		// Token: 0x06007A72 RID: 31346 RVA: 0x002B31E3 File Offset: 0x002B13E3
		private bool MatchImpl(string value)
		{
			return value.IndexOf(this.searchText, StringComparison.InvariantCultureIgnoreCase) != -1;
		}

		// Token: 0x06007A73 RID: 31347 RVA: 0x002B31F8 File Offset: 0x002B13F8
		public bool Matches(ThingDef td)
		{
			return this.Matches(td.label);
		}

		// Token: 0x06007A74 RID: 31348 RVA: 0x002B31F8 File Offset: 0x002B13F8
		public bool Matches(SpecialThingFilterDef sfDef)
		{
			return this.Matches(sfDef.label);
		}

		// Token: 0x040043AD RID: 17325
		private string inputText = "";

		// Token: 0x040043AE RID: 17326
		private string searchText = "";

		// Token: 0x040043AF RID: 17327
		private readonly LRUCache<string, bool> cachedMatches = new LRUCache<string, bool>(5000);
	}
}
