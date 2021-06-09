using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000ACC RID: 2764
	public static class LordMaker
	{
		// Token: 0x06004167 RID: 16743 RVA: 0x001877E4 File Offset: 0x001859E4
		public static Lord MakeNewLord(Faction faction, LordJob lordJob, Map map, IEnumerable<Pawn> startingPawns = null)
		{
			if (map == null)
			{
				Log.Warning("Tried to create a lord with null map.", false);
				return null;
			}
			Lord lord = new Lord();
			lord.loadID = Find.UniqueIDsManager.GetNextLordID();
			lord.faction = faction;
			map.lordManager.AddLord(lord);
			lord.SetJob(lordJob);
			lord.GotoToil(lord.Graph.StartingToil);
			if (startingPawns != null)
			{
				foreach (Pawn p in startingPawns)
				{
					lord.AddPawn(p);
				}
			}
			return lord;
		}
	}
}
