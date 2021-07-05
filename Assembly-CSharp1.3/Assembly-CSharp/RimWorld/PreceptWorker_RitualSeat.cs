using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EEE RID: 3822
	public class PreceptWorker_RitualSeat : PreceptWorker
	{
		// Token: 0x06005AC7 RID: 23239 RVA: 0x001F639C File Offset: 0x001F459C
		public override AcceptanceReport CanUse(ThingDef def, Ideo ideo)
		{
			PreceptWorker_RitualSeat.tmpRitualSeats.Clear();
			List<ThingDef> list = null;
			foreach (MemeDef memeDef in ideo.memes)
			{
				if (!memeDef.requireAnyRitualSeat.NullOrEmpty<ThingDef>())
				{
					if (list == null)
					{
						PreceptWorker_RitualSeat.tmpRitualSeats.AddRange(memeDef.requireAnyRitualSeat);
						list = PreceptWorker_RitualSeat.tmpRitualSeats;
					}
					else
					{
						PreceptWorker_RitualSeat.tmpRitualSeatsMerge.Clear();
						foreach (ThingDef item in memeDef.requireAnyRitualSeat)
						{
							if (list.Contains(item))
							{
								PreceptWorker_RitualSeat.tmpRitualSeatsMerge.Add(item);
							}
						}
						list.Clear();
						list.AddRange(PreceptWorker_RitualSeat.tmpRitualSeatsMerge);
						if (list.Count == 0)
						{
							Log.Error("Ideo has 2 memes which have conflicting ritual set requirements!");
						}
					}
				}
			}
			return list == null || list.Contains(def);
		}

		// Token: 0x0400351A RID: 13594
		private static List<ThingDef> tmpRitualSeats = new List<ThingDef>();

		// Token: 0x0400351B RID: 13595
		private static List<ThingDef> tmpRitualSeatsMerge = new List<ThingDef>();
	}
}
