using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200170B RID: 5899
	public static class HiveUtility
	{
		// Token: 0x060081E5 RID: 33253 RVA: 0x0005737D File Offset: 0x0005557D
		public static int TotalSpawnedHivesCount(Map map)
		{
			return map.listerThings.ThingsOfDef(ThingDefOf.Hive).Count;
		}

		// Token: 0x060081E6 RID: 33254 RVA: 0x00268024 File Offset: 0x00266224
		public static bool AnyHivePreventsClaiming(Thing thing)
		{
			if (!thing.Spawned)
			{
				return false;
			}
			int num = GenRadial.NumCellsInRadius(2f);
			for (int i = 0; i < num; i++)
			{
				IntVec3 c = thing.Position + GenRadial.RadialPattern[i];
				if (c.InBounds(thing.Map) && c.GetFirstThing(thing.Map) != null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060081E7 RID: 33255 RVA: 0x00268088 File Offset: 0x00266288
		public static void Notify_HiveDespawned(Hive hive, Map map)
		{
			int num = GenRadial.NumCellsInRadius(2f);
			for (int i = 0; i < num; i++)
			{
				IntVec3 c = hive.Position + GenRadial.RadialPattern[i];
				if (c.InBounds(map))
				{
					List<Thing> thingList = c.GetThingList(map);
					for (int j = 0; j < thingList.Count; j++)
					{
						if (thingList[j].Faction == Faction.OfInsects && !HiveUtility.AnyHivePreventsClaiming(thingList[j]) && !(thingList[j] is Pawn))
						{
							thingList[j].SetFaction(null, null);
						}
					}
				}
			}
		}

		// Token: 0x0400543E RID: 21566
		private const float HivePreventsClaimingInRadius = 2f;
	}
}
