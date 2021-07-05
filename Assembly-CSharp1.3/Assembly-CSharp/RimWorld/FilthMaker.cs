using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020010A8 RID: 4264
	public static class FilthMaker
	{
		// Token: 0x060065B0 RID: 26032 RVA: 0x0022597C File Offset: 0x00223B7C
		public static bool CanMakeFilth(IntVec3 c, Map map, ThingDef filthDef, FilthSourceFlags additionalFlags = FilthSourceFlags.None)
		{
			TerrainDef terrain = c.GetTerrain(map);
			if ((filthDef.filth.placementMask & FilthSourceFlags.Natural) == FilthSourceFlags.None && Rand.Value > terrain.GetStatValueAbstract(StatDefOf.FilthMultiplier, null))
			{
				return false;
			}
			FilthSourceFlags filthSourceFlags = filthDef.filth.placementMask | additionalFlags;
			if (terrain.filthAcceptanceMask != FilthSourceFlags.None && filthSourceFlags.HasFlag(FilthSourceFlags.Pawn))
			{
				if (c.GetRoof(map) != null)
				{
					return true;
				}
				Room room = c.GetRoom(map);
				if (room != null && !room.TouchesMapEdge && !room.UsesOutdoorTemperature)
				{
					return true;
				}
			}
			return FilthMaker.TerrainAcceptsFilth(terrain, filthDef, additionalFlags);
		}

		// Token: 0x060065B1 RID: 26033 RVA: 0x00225A10 File Offset: 0x00223C10
		public static bool TerrainAcceptsFilth(TerrainDef terrainDef, ThingDef filthDef, FilthSourceFlags additionalFlags = FilthSourceFlags.None)
		{
			if (terrainDef.filthAcceptanceMask == FilthSourceFlags.None)
			{
				return false;
			}
			FilthSourceFlags filthSourceFlags = filthDef.filth.placementMask | additionalFlags;
			return (terrainDef.filthAcceptanceMask & filthSourceFlags) == filthSourceFlags;
		}

		// Token: 0x060065B2 RID: 26034 RVA: 0x00225A40 File Offset: 0x00223C40
		public static bool TryMakeFilth(IntVec3 c, Map map, ThingDef filthDef, int count = 1, FilthSourceFlags additionalFlags = FilthSourceFlags.None)
		{
			bool flag = false;
			for (int i = 0; i < count; i++)
			{
				flag |= FilthMaker.TryMakeFilth(c, map, filthDef, null, true, additionalFlags);
			}
			return flag;
		}

		// Token: 0x060065B3 RID: 26035 RVA: 0x00225A6C File Offset: 0x00223C6C
		public static bool TryMakeFilth(IntVec3 c, Map map, ThingDef filthDef, string source, int count = 1, FilthSourceFlags additionalFlags = FilthSourceFlags.None)
		{
			bool flag = false;
			for (int i = 0; i < count; i++)
			{
				flag |= FilthMaker.TryMakeFilth(c, map, filthDef, Gen.YieldSingle<string>(source), true, additionalFlags);
			}
			return flag;
		}

		// Token: 0x060065B4 RID: 26036 RVA: 0x00225A9D File Offset: 0x00223C9D
		public static bool TryMakeFilth(IntVec3 c, Map map, ThingDef filthDef, IEnumerable<string> sources, FilthSourceFlags additionalFlags = FilthSourceFlags.None)
		{
			return FilthMaker.TryMakeFilth(c, map, filthDef, sources, true, additionalFlags);
		}

		// Token: 0x060065B5 RID: 26037 RVA: 0x00225AAC File Offset: 0x00223CAC
		private static bool TryMakeFilth(IntVec3 c, Map map, ThingDef filthDef, IEnumerable<string> sources, bool shouldPropagate, FilthSourceFlags additionalFlags = FilthSourceFlags.None)
		{
			Filth filth = (Filth)(from t in c.GetThingList(map)
			where t.def == filthDef
			select t).FirstOrDefault<Thing>();
			if (!c.WalkableByAny(map) || (filth != null && !filth.CanBeThickened))
			{
				if (shouldPropagate)
				{
					List<IntVec3> list = GenAdj.AdjacentCells8WayRandomized();
					for (int i = 0; i < 8; i++)
					{
						IntVec3 c2 = c + list[i];
						if (c2.InBounds(map) && FilthMaker.TryMakeFilth(c2, map, filthDef, sources, false, FilthSourceFlags.None))
						{
							return true;
						}
					}
				}
				if (filth != null)
				{
					filth.AddSources(sources);
				}
				return false;
			}
			if (filth != null)
			{
				filth.ThickenFilth();
				filth.AddSources(sources);
			}
			else
			{
				if (!FilthMaker.CanMakeFilth(c, map, filthDef, additionalFlags))
				{
					return false;
				}
				Filth filth2 = (Filth)ThingMaker.MakeThing(filthDef, null);
				filth2.AddSources(sources);
				GenSpawn.Spawn(filth2, c, map, WipeMode.Vanish);
			}
			FilthMonitor.Notify_FilthSpawned();
			return true;
		}

		// Token: 0x060065B6 RID: 26038 RVA: 0x00225B9C File Offset: 0x00223D9C
		public static void RemoveAllFilth(IntVec3 c, Map map)
		{
			FilthMaker.toBeRemoved.Clear();
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Filth filth = thingList[i] as Filth;
				if (filth != null)
				{
					FilthMaker.toBeRemoved.Add(filth);
				}
			}
			for (int j = 0; j < FilthMaker.toBeRemoved.Count; j++)
			{
				FilthMaker.toBeRemoved[j].Destroy(DestroyMode.Vanish);
			}
			FilthMaker.toBeRemoved.Clear();
		}

		// Token: 0x04003963 RID: 14691
		private static List<Filth> toBeRemoved = new List<Filth>();
	}
}
