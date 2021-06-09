using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200173F RID: 5951
	public class ThingSetMaker_Conditional_MakingFaction : ThingSetMaker_Conditional
	{
		// Token: 0x06008344 RID: 33604 RVA: 0x0026EAD4 File Offset: 0x0026CCD4
		protected override bool Condition(ThingSetMakerParams parms)
		{
			return (!this.requireNonNull || parms.makingFaction != null) && (this.makingFaction == null || (parms.makingFaction != null && parms.makingFaction.def == this.makingFaction)) && (this.makingFactionCategories.NullOrEmpty<string>() || (parms.makingFaction != null && this.makingFactionCategories.Contains(parms.makingFaction.def.categoryTag)));
		}

		// Token: 0x04005510 RID: 21776
		public FactionDef makingFaction;

		// Token: 0x04005511 RID: 21777
		public List<string> makingFactionCategories;

		// Token: 0x04005512 RID: 21778
		public bool requireNonNull;
	}
}
