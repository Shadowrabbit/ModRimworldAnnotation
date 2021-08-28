using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000667 RID: 1639
	public static class 
	LordMaker
	{
		// Token: 0x06002E84 RID: 11908 RVA: 0x001164B4 File Offset: 0x001146B4
		public static Lord MakeNewLord(Faction faction, LordJob lordJob, Map map, IEnumerable<Pawn> startingPawns = null)
		{
			if (map == null)
			{
				Log.Warning("Tried to create a lord with null map.");
				return null;
			}
			Lord lord = new Lord();
			lord.loadID = Find.UniqueIDsManager.GetNextLordID();
			lord.faction = faction;
			map.lordManager.AddLord(lord);
			lord.SetJob(lordJob, false);
			lord.GotoToil(lord.Graph.StartingToil);
			if (startingPawns != null)
			{
				lord.AddPawns(startingPawns);
			}
			return lord;
		}
	}
}
