using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014AB RID: 5291
	public class SpecialThingFilterWorker_CorpsesStranger : SpecialThingFilterWorker
	{
		// Token: 0x06007E90 RID: 32400 RVA: 0x002CE414 File Offset: 0x002CC614
		public override bool Matches(Thing t)
		{
			Corpse corpse = t as Corpse;
			return corpse != null && corpse.InnerPawn.def.race.Humanlike && corpse.InnerPawn.Faction != Faction.OfPlayer;
		}
	}
}
