using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001567 RID: 5479
	public class PlaceWorker_MustBeIndoors : PlaceWorker
	{
		// Token: 0x060081BB RID: 33211 RVA: 0x002DDBE0 File Offset: 0x002DBDE0
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			Room room = loc.GetRoom(map);
			if (room == null || room.TouchesMapEdge)
			{
				return "MustBePlacedIndoors".Translate();
			}
			return AcceptanceReport.WasAccepted;
		}
	}
}
