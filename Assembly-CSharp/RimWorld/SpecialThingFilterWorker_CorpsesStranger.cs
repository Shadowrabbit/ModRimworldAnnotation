using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D11 RID: 7441
	public class SpecialThingFilterWorker_CorpsesStranger : SpecialThingFilterWorker
	{
		// Token: 0x0600A1E8 RID: 41448 RVA: 0x002F4A48 File Offset: 0x002F2C48
		public override bool Matches(Thing t)
		{
			Corpse corpse = t as Corpse;
			return corpse != null && corpse.InnerPawn.def.race.Humanlike && corpse.InnerPawn.Faction != Faction.OfPlayer;
		}
	}
}
