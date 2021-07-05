using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C98 RID: 3224
	public static class ScatterDebrisUtility
	{
		// Token: 0x06004B41 RID: 19265 RVA: 0x0018FB10 File Offset: 0x0018DD10
		public static bool CanPlaceThingAt(IntVec3 c, Rot4 rot, Map map, ThingDef thingDef)
		{
			if (!c.InBounds(map))
			{
				return false;
			}
			if (!GenConstruct.CanBuildOnTerrain(thingDef, c, map, rot, null, null))
			{
				return false;
			}
			foreach (IntVec3 c2 in GenAdj.OccupiedRect(c, rot, thingDef.size))
			{
				if (!c2.InBounds(map) || c2.Roofed(map) || c2.GetEdifice(map) != null || !map.reachability.CanReachMapEdge(c2, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false)))
				{
					return false;
				}
				if (c2.GetThingList(map).Count > 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004B42 RID: 19266 RVA: 0x0018FBD0 File Offset: 0x0018DDD0
		public static void ScatterFilthAroundThing(Thing thing, Map map, ThingDef filth, float scatterChance = 0.5f, int expandBy = 1, int maxFilth = 2147483647, Func<IntVec3, bool> cellValidator = null)
		{
			int num = 0;
			foreach (IntVec3 intVec in thing.OccupiedRect().ExpandedBy(expandBy).InRandomOrder(null))
			{
				if (intVec.InBounds(map) && Rand.Chance(scatterChance) && (cellValidator == null || cellValidator(intVec)) && FilthMaker.TryMakeFilth(intVec, map, filth, 1, FilthSourceFlags.None))
				{
					num++;
				}
				if (num >= maxFilth)
				{
					break;
				}
			}
		}

		// Token: 0x06004B43 RID: 19267 RVA: 0x0018FC64 File Offset: 0x0018DE64
		public static void ScatterAround(IntVec3 center, IntVec2 size, Rot4 rot, Sketch sketch, ThingDef scatterThing, float scatterChance = 0.5f, int expandBy = 1, int maxFilth = 2147483647, Func<IntVec3, bool> cellValidator = null)
		{
			int num = 0;
			foreach (IntVec3 intVec in GenAdj.OccupiedRect(center, rot, size).ExpandedBy(expandBy).InRandomOrder(null))
			{
				if (Rand.Chance(scatterChance) && (cellValidator == null || cellValidator(intVec)))
				{
					sketch.AddThing(scatterThing, intVec, Rot4.North, null, 1, null, null, true);
					num++;
				}
				if (num >= maxFilth)
				{
					break;
				}
			}
		}
	}
}
