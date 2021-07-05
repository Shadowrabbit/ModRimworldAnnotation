using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014B2 RID: 5298
	public class SpecialThingFilterWorker_Burnable : SpecialThingFilterWorker
	{
		// Token: 0x06007EA9 RID: 32425 RVA: 0x002CE64C File Offset: 0x002CC84C
		public override bool Matches(Thing t)
		{
			return this.CanEverMatch(t.def) && t.BurnableByRecipe;
		}

		// Token: 0x06007EAA RID: 32426 RVA: 0x002CE664 File Offset: 0x002CC864
		public override bool CanEverMatch(ThingDef def)
		{
			return def.burnableByRecipe;
		}

		// Token: 0x06007EAB RID: 32427 RVA: 0x002CE66C File Offset: 0x002CC86C
		public override bool AlwaysMatches(ThingDef def)
		{
			return def.burnableByRecipe && !def.MadeFromStuff;
		}
	}
}
