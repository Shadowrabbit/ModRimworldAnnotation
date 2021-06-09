using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D10 RID: 7440
	public class SpecialThingFilterWorker_CorpsesColonist : SpecialThingFilterWorker
	{
		// Token: 0x0600A1E6 RID: 41446 RVA: 0x002F4A04 File Offset: 0x002F2C04
		public override bool Matches(Thing t)
		{
			Corpse corpse = t as Corpse;
			return corpse != null && corpse.InnerPawn.def.race.Humanlike && corpse.InnerPawn.Faction == Faction.OfPlayer;
		}
	}
}
