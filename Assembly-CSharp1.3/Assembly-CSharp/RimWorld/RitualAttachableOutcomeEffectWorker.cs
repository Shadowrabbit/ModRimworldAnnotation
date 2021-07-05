using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F00 RID: 3840
	public abstract class RitualAttachableOutcomeEffectWorker
	{
		// Token: 0x06005BBA RID: 23482
		public abstract void Apply(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, OutcomeChance outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets);

		// Token: 0x06005BBB RID: 23483 RVA: 0x0015E338 File Offset: 0x0015C538
		public virtual AcceptanceReport CanApplyNow(Precept_Ritual ritual, Map map)
		{
			return true;
		}

		// Token: 0x04003577 RID: 13687
		public RitualAttachableOutcomeEffectDef def;
	}
}
