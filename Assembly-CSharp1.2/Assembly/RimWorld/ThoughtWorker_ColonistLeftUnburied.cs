using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EB9 RID: 3769
	public class ThoughtWorker_ColonistLeftUnburied : ThoughtWorker
	{
		// Token: 0x060053C3 RID: 21443 RVA: 0x001C1A84 File Offset: 0x001BFC84
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
