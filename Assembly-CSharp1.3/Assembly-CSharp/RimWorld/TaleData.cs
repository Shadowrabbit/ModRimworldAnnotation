using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001023 RID: 4131
	public abstract class TaleData : IExposable
	{
		// Token: 0x0600618F RID: 24975
		public abstract void ExposeData();

		// Token: 0x06006190 RID: 24976 RVA: 0x002125C2 File Offset: 0x002107C2
		public virtual IEnumerable<Rule> GetRules(string prefix, Dictionary<string, string> constants = null)
		{
			Log.Error(base.GetType() + " cannot do GetRules with a prefix.");
			yield break;
		}

		// Token: 0x06006191 RID: 24977 RVA: 0x002125D2 File Offset: 0x002107D2
		public virtual IEnumerable<Rule> GetRules(Dictionary<string, string> constants = null)
		{
			Log.Error(base.GetType() + " cannot do GetRules without a prefix.");
			yield break;
		}
	}
}
