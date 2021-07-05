using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020010D9 RID: 4313
	public class ThingSetMaker_Conditional_MakingFaction : ThingSetMaker_Conditional
	{
		// Token: 0x06006738 RID: 26424 RVA: 0x0022E054 File Offset: 0x0022C254
		protected override bool Condition(ThingSetMakerParams parms)
		{
			return (!this.requireNonNull || parms.makingFaction != null) && (this.makingFaction == null || (parms.makingFaction != null && parms.makingFaction.def == this.makingFaction)) && (this.makingFactionCategories.NullOrEmpty<string>() || (parms.makingFaction != null && this.makingFactionCategories.Contains(parms.makingFaction.def.categoryTag)));
		}

		// Token: 0x04003A45 RID: 14917
		public FactionDef makingFaction;

		// Token: 0x04003A46 RID: 14918
		public List<string> makingFactionCategories;

		// Token: 0x04003A47 RID: 14919
		public bool requireNonNull;
	}
}
