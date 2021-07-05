using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014B3 RID: 5299
	public class SpecialThingFilterWorker_NonBurnable : SpecialThingFilterWorker
	{
		// Token: 0x06007EAD RID: 32429 RVA: 0x002CE681 File Offset: 0x002CC881
		public override bool Matches(Thing t)
		{
			return this.CanEverMatch(t.def) && !t.BurnableByRecipe;
		}

		// Token: 0x06007EAE RID: 32430 RVA: 0x002CE69C File Offset: 0x002CC89C
		public override bool CanEverMatch(ThingDef def)
		{
			return !def.burnableByRecipe || def.MadeFromStuff;
		}

		// Token: 0x06007EAF RID: 32431 RVA: 0x002CE6AE File Offset: 0x002CC8AE
		public override bool AlwaysMatches(ThingDef def)
		{
			return !def.burnableByRecipe && !def.MadeFromStuff;
		}
	}
}
