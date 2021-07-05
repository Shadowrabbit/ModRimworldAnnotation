using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C0C RID: 3084
	public static class InfestationUtility
	{
		// Token: 0x06004882 RID: 18562 RVA: 0x0017F7D8 File Offset: 0x0017D9D8
		private static IntVec3 FindRootTunnelLoc(Map map, bool spawnAnywhereIfNoGoodCell = false, bool ignoreRoofIfNoGoodCell = false)
		{
			IntVec3 result;
			if (InfestationCellFinder.TryFindCell(out result, map))
			{
				return result;
			}
			if (!spawnAnywhereIfNoGoodCell)
			{
				return IntVec3.Invalid;
			}
			Func<IntVec3, bool, bool> validator = delegate(IntVec3 x, bool canIgnoreRoof)
			{
				if (!x.Standable(map) || x.Fogged(map))
				{
					return false;
				}
				if (!canIgnoreRoof)
				{
					bool flag = false;
					int num = GenRadial.NumCellsInRadius(3f);
					for (int i = 0; i < num; i++)
					{
						IntVec3 c = x + GenRadial.RadialPattern[i];
						if (c.InBounds(map))
						{
							RoofDef roof = c.GetRoof(map);
							if (roof != null && roof.isThickRoof)
							{
								flag = true;
								break;
							}
						}
					}
					if (!flag)
					{
						return false;
					}
				}
				return true;
			};
			if (RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith((IntVec3 x) => validator(x, false), map, out result))
			{
				return result;
			}
			if (ignoreRoofIfNoGoodCell && RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith((IntVec3 x) => validator(x, true), map, out result))
			{
				return result;
			}
			return IntVec3.Invalid;
		}

		// Token: 0x06004883 RID: 18563 RVA: 0x0017F860 File Offset: 0x0017DA60
		public static Thing SpawnTunnels(int hiveCount, Map map, bool spawnAnywhereIfNoGoodCell = false, bool ignoreRoofedRequirement = false, string questTag = null, IntVec3? overrideLoc = null, float? insectsPoints = null)
		{
			IntVec3 loc = (overrideLoc != null) ? overrideLoc.Value : default(IntVec3);
			if (overrideLoc == null)
			{
				loc = InfestationUtility.FindRootTunnelLoc(map, spawnAnywhereIfNoGoodCell, false);
			}
			if (!loc.IsValid)
			{
				return null;
			}
			TunnelHiveSpawner tunnelHiveSpawner = (TunnelHiveSpawner)ThingMaker.MakeThing(ThingDefOf.TunnelHiveSpawner, null);
			Thing thing = GenSpawn.Spawn(tunnelHiveSpawner, loc, map, WipeMode.FullRefund);
			if (insectsPoints != null)
			{
				tunnelHiveSpawner.insectsPoints = insectsPoints.Value;
			}
			QuestUtility.AddQuestTag(thing, questTag);
			for (int i = 0; i < hiveCount - 1; i++)
			{
				loc = CompSpawnerHives.FindChildHiveLocation(thing.Position, map, ThingDefOf.Hive, ThingDefOf.Hive.GetCompProperties<CompProperties_SpawnerHives>(), ignoreRoofedRequirement, true);
				if (loc.IsValid)
				{
					tunnelHiveSpawner = (TunnelHiveSpawner)ThingMaker.MakeThing(ThingDefOf.TunnelHiveSpawner, null);
					thing = GenSpawn.Spawn(tunnelHiveSpawner, loc, map, WipeMode.FullRefund);
					if (insectsPoints != null)
					{
						tunnelHiveSpawner.insectsPoints = insectsPoints.Value;
					}
					QuestUtility.AddQuestTag(thing, questTag);
				}
			}
			return thing;
		}

		// Token: 0x06004884 RID: 18564 RVA: 0x0017F954 File Offset: 0x0017DB54
		public static Thing SpawnJellyTunnels(int tunnelCount, int jellyCount, Map map)
		{
			IntVec3 loc = InfestationUtility.FindRootTunnelLoc(map, true, true);
			if (!loc.IsValid)
			{
				return null;
			}
			TunnelJellySpawner tunnelJellySpawner = (TunnelJellySpawner)ThingMaker.MakeThing(ThingDefOf.TunnelJellySpawner, null);
			tunnelJellySpawner.jellyCount = jellyCount / tunnelCount;
			int num = jellyCount - tunnelJellySpawner.jellyCount;
			GenSpawn.Spawn(tunnelJellySpawner, loc, map, WipeMode.FullRefund);
			for (int i = 0; i < tunnelCount - 1; i++)
			{
				loc = CompSpawnerHives.FindChildHiveLocation(tunnelJellySpawner.Position, map, ThingDefOf.Hive, ThingDefOf.Hive.GetCompProperties<CompProperties_SpawnerHives>(), true, false);
				if (loc.IsValid)
				{
					tunnelJellySpawner = (TunnelJellySpawner)ThingMaker.MakeThing(ThingDefOf.TunnelJellySpawner, null);
					if (i < tunnelCount - 2)
					{
						tunnelJellySpawner.jellyCount = jellyCount / tunnelCount;
						num -= tunnelJellySpawner.jellyCount;
					}
					else
					{
						tunnelJellySpawner.jellyCount = num;
					}
					GenSpawn.Spawn(tunnelJellySpawner, loc, map, WipeMode.FullRefund);
				}
			}
			return tunnelJellySpawner;
		}
	}
}
