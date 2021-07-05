using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014AA RID: 5290
	public class SpecialThingFilterWorker_CorpsesColonist : SpecialThingFilterWorker
	{
		// Token: 0x06007E8E RID: 32398 RVA: 0x002CE3C8 File Offset: 0x002CC5C8
		public override bool Matches(Thing t)
		{
			Corpse corpse = t as Corpse;
			return corpse != null && corpse.InnerPawn.def.race.Humanlike && corpse.InnerPawn.Faction == Faction.OfPlayer;
		}
	}
}
