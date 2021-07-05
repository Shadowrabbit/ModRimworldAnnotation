using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014AE RID: 5294
	public class SpecialThingFilterWorker_PlantFood : SpecialThingFilterWorker
	{
		// Token: 0x06007E99 RID: 32409 RVA: 0x002CE523 File Offset: 0x002CC723
		public override bool Matches(Thing t)
		{
			return this.AlwaysMatches(t.def);
		}

		// Token: 0x06007E9A RID: 32410 RVA: 0x002CE531 File Offset: 0x002CC731
		public override bool AlwaysMatches(ThingDef def)
		{
			return def.ingestible != null && (def.ingestible.foodType & FoodTypeFlags.Plant) > FoodTypeFlags.None;
		}

		// Token: 0x06007E9B RID: 32411 RVA: 0x002CE54E File Offset: 0x002CC74E
		public override bool CanEverMatch(ThingDef def)
		{
			return this.AlwaysMatches(def);
		}
	}
}
