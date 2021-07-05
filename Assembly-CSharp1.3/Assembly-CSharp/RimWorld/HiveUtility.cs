using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020010AE RID: 4270
	public static class HiveUtility
	{
		// Token: 0x06006606 RID: 26118 RVA: 0x00227245 File Offset: 0x00225445
		public static int TotalSpawnedHivesCount(Map map)
		{
			return map.listerThings.ThingsOfDef(ThingDefOf.Hive).Count;
		}

		// Token: 0x06006607 RID: 26119 RVA: 0x0022725C File Offset: 0x0022545C
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

		// Token: 0x06006608 RID: 26120 RVA: 0x002272C0 File Offset: 0x002254C0
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

		// Token: 0x0400398F RID: 14735
		private const float HivePreventsClaimingInRadius = 2f;
	}
}
