using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200173B RID: 5947
	public static class SkyfallerUtility
	{
		// Token: 0x06008337 RID: 33591 RVA: 0x0026E7F0 File Offset: 0x0026C9F0
		public static bool CanPossiblyFallOnColonist(ThingDef skyfaller, IntVec3 c, Map map)
		{
			CellRect cellRect = GenAdj.OccupiedRect(c, Rot4.North, skyfaller.size);
			int dist = Mathf.Max(Mathf.CeilToInt(skyfaller.skyfaller.explosionRadius) + 7, 14);
			foreach (IntVec3 c2 in cellRect.ExpandedBy(dist))
			{
				if (c2.InBounds(map))
				{
					List<Thing> thingList = c2.GetThingList(map);
					for (int i = 0; i < thingList.Count; i++)
					{
						Pawn pawn = thingList[i] as Pawn;
						if (pawn != null && pawn.IsColonist)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06008338 RID: 33592 RVA: 0x0026E8C0 File Offset: 0x0026CAC0
		public static void MakeDropoffShuttle(Map map, List<Thing> contents, Faction faction = null)
		{
			IntVec3 center;
			Thing thing;
			if (!DropCellFinder.TryFindShipLandingArea(map, out center, out thing))
			{
				if (thing != null)
				{
					Messages.Message("ShuttleBlocked".Translate("BlockedBy".Translate(thing).CapitalizeFirst()), thing, MessageTypeDefOf.NeutralEvent, true);
				}
				center = DropCellFinder.TryFindSafeLandingSpotCloseToColony(map, ThingDefOf.Shuttle.Size, null, 2);
			}
			Thing thing2 = ThingMaker.MakeThing(ThingDefOf.Shuttle, null);
			thing2.TryGetComp<CompShuttle>().dropEverythingOnArrival = true;
			for (int i = 0; i < contents.Count; i++)
			{
				Pawn p;
				if ((p = (contents[i] as Pawn)) != null)
				{
					Find.WorldPawns.RemovePawn(p);
				}
			}
			thing2.SetFaction(faction, null);
			thing2.TryGetComp<CompTransporter>().innerContainer.TryAddRangeOrTransfer(contents, true, false);
			GenPlace.TryPlaceThing(SkyfallerMaker.MakeSkyfaller(ThingDefOf.ShuttleIncoming, Gen.YieldSingle<Thing>(thing2)), center, map, ThingPlaceMode.Near, null, null, default(Rot4));
		}
	}
}
