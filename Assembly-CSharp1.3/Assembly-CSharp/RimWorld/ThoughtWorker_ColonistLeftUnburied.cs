using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020009AF RID: 2479
	public class ThoughtWorker_ColonistLeftUnburied : ThoughtWorker
	{
		// Token: 0x06003DEC RID: 15852 RVA: 0x00153C00 File Offset: 0x00151E00
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.Faction != Faction.OfPlayer)
			{
				return false;
			}
			List<Thing> list = p.Map.listerThings.ThingsMatching(ThingRequest.ForGroup(ThingRequestGroup.Corpse));
			for (int i = 0; i < list.Count; i++)
			{
				Corpse corpse = (Corpse)list[i];
				if ((float)corpse.Age > 90000f && Alert_ColonistLeftUnburied.IsCorpseOfColonist(corpse))
				{
					return true;
				}
			}
			return false;
		}
	}
}
