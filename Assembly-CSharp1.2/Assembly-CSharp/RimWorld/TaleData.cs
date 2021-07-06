using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001630 RID: 5680
	public abstract class TaleData : IExposable
	{
		// Token: 0x06007B77 RID: 31607
		public abstract void ExposeData();

		// Token: 0x06007B78 RID: 31608 RVA: 0x0005302C File Offset: 0x0005122C
		public virtual IEnumerable<Rule> GetRules(string prefix)
		{
			Log.Error(base.GetType() + " cannot do GetRules with a prefix.", false);
			yield break;
		}

		// Token: 0x06007B79 RID: 31609 RVA: 0x0005303C File Offset: 0x0005123C
		public virtual IEnumerable<Rule> GetRules()
		{
			Log.Error(base.GetType() + " cannot do GetRules without a prefix.", false);
			yield break;
		}
	}
}
