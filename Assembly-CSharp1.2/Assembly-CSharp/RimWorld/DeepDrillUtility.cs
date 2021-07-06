using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020017B0 RID: 6064
	public static class DeepDrillUtility
	{
		// Token: 0x0600861A RID: 34330 RVA: 0x00277914 File Offset: 0x00275B14
		public static ThingDef GetNextResource(IntVec3 p, Map map)
		{
			ThingDef result;
			int num;
			IntVec3 intVec;
			DeepDrillUtility.GetNextResource(p, map, out result, out num, out intVec);
			return result;
		}

		// Token: 0x0600861B RID: 34331 RVA: 0x00277930 File Offset: 0x00275B30
		public static bool GetNextResource(IntVec3 p, Map map, out ThingDef resDef, out int countPresent, out IntVec3 cell)
		{
			for (int i = 0; i < 21; i++)
			{
				IntVec3 intVec = p + GenRadial.RadialPattern[i];
				if (intVec.InBounds(map))
				{
					ThingDef thingDef = map.deepResourceGrid.ThingDefAt(intVec);
					if (thingDef != null)
					{
						resDef = thingDef;
						countPresent = map.deepResourceGrid.CountAt(intVec);
						cell = intVec;
						return true;
					}
				}
			}
			resDef = DeepDrillUtility.GetBaseResource(map, p);
			countPresent = int.MaxValue;
			cell = p;
			return false;
		}

		// Token: 0x0600861C RID: 34332 RVA: 0x002779AC File Offset: 0x00275BAC
		public static ThingDef GetBaseResource(Map map, IntVec3 cell)
		{
			if (!map.Biome.hasBedrock)
			{
				return null;
			}
			Rand.PushState();
			Rand.Seed = cell.GetHashCode();
			ThingDef result = (from rock in Find.World.NaturalRockTypesIn(map.Tile)
			select rock.building.mineableThing).RandomElement<ThingDef>();
			Rand.PopState();
			return result;
		}

		// Token: 0x0400566D RID: 22125
		public const int NumCellsToScan = 21;
	}
}
