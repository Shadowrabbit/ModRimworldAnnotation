﻿using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020020F9 RID: 8441
	public static class CaravanMaker
	{
		// Token: 0x0600B34B RID: 45899 RVA: 0x0033F7E0 File Offset: 0x0033D9E0
		public static Caravan MakeCaravan(IEnumerable<Pawn> pawns, Faction faction, int startingTile, bool addToWorldPawnsIfNotAlready)
		{
			if (startingTile < 0 && addToWorldPawnsIfNotAlready)
			{
				Log.Warning("Tried to create a caravan but chose not to spawn a caravan but pass pawns to world. This can cause bugs because pawns can be discarded.", false);
			}
			CaravanMaker.tmpPawns.Clear();
			CaravanMaker.tmpPawns.AddRange(pawns);
			Caravan caravan = (Caravan)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Caravan);
			if (startingTile >= 0)
			{
				caravan.Tile = startingTile;
			}
			caravan.SetFaction(faction);
			if (startingTile >= 0)
			{
				Find.WorldObjects.Add(caravan);
			}
			for (int i = 0; i < CaravanMaker.tmpPawns.Count; i++)
			{
				Pawn pawn = CaravanMaker.tmpPawns[i];
				if (pawn.Dead)
				{
					Log.Warning("Tried to form a caravan with a dead pawn " + pawn, false);
				}
				else
				{
					caravan.AddPawn(pawn, addToWorldPawnsIfNotAlready);
					if (addToWorldPawnsIfNotAlready && !pawn.IsWorldPawn())
					{
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
					}
				}
			}
			caravan.Name = CaravanNameGenerator.GenerateCaravanName(caravan);
			CaravanMaker.tmpPawns.Clear();
			caravan.SetUniqueId(Find.UniqueIDsManager.GetNextCaravanID());
			return caravan;
		}

		// Token: 0x04007B3A RID: 31546
		private static List<Pawn> tmpPawns = new List<Pawn>();
	}
}
