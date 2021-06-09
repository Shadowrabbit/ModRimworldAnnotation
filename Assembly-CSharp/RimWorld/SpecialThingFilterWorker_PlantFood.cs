using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D14 RID: 7444
	public class SpecialThingFilterWorker_PlantFood : SpecialThingFilterWorker
	{
		// Token: 0x0600A1F1 RID: 41457 RVA: 0x0006BA18 File Offset: 0x00069C18
		public override bool Matches(Thing t)
		{
			return this.AlwaysMatches(t.def);
		}

		// Token: 0x0600A1F2 RID: 41458 RVA: 0x0006BA26 File Offset: 0x00069C26
		public override bool AlwaysMatches(ThingDef def)
		{
			return def.ingestible != null && (def.ingestible.foodType & FoodTypeFlags.Plant) > FoodTypeFlags.None;
		}

		// Token: 0x0600A1F3 RID: 41459 RVA: 0x0006BA43 File Offset: 0x00069C43
		public override bool CanEverMatch(ThingDef def)
		{
			return this.AlwaysMatches(def);
		}
	}
}
