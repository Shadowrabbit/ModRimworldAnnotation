using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020012F9 RID: 4857
	public static class PowerNetMaker
	{
		// Token: 0x06006968 RID: 26984 RVA: 0x00207874 File Offset: 0x00205A74
		private static IEnumerable<CompPower> ContiguousPowerBuildings(Building root)
		{
			PowerNetMaker.closedSet.Clear();
			PowerNetMaker.openSet.Clear();
			PowerNetMaker.currentSet.Clear();
			PowerNetMaker.openSet.Add(root);
			do
			{
				foreach (Building item in PowerNetMaker.openSet)
				{
					PowerNetMaker.closedSet.Add(item);
				}
				HashSet<Building> hashSet = PowerNetMaker.currentSet;
				PowerNetMaker.currentSet = PowerNetMaker.openSet;
				PowerNetMaker.openSet = hashSet;
				PowerNetMaker.openSet.Clear();
				foreach (Building building in PowerNetMaker.currentSet)
				{
					foreach (IntVec3 c in GenAdj.CellsAdjacentCardinal(building))
					{
						if (c.InBounds(building.Map))
						{
							List<Thing> thingList = c.GetThingList(building.Map);
							for (int i = 0; i < thingList.Count; i++)
							{
								Building building2 = thingList[i] as Building;
								if (building2 != null && building2.TransmitsPowerNow && !PowerNetMaker.openSet.Contains(building2) && !PowerNetMaker.currentSet.Contains(building2) && !PowerNetMaker.closedSet.Contains(building2))
								{
									PowerNetMaker.openSet.Add(building2);
									break;
								}
							}
						}
					}
				}
			}
			while (PowerNetMaker.openSet.Count > 0);
			IEnumerable<CompPower> result = (from b in PowerNetMaker.closedSet
			select b.PowerComp).ToArray<CompPower>();
			PowerNetMaker.closedSet.Clear();
			PowerNetMaker.openSet.Clear();
			PowerNetMaker.currentSet.Clear();
			return result;
		}

		// Token: 0x06006969 RID: 26985 RVA: 0x00047DD4 File Offset: 0x00045FD4
		public static PowerNet NewPowerNetStartingFrom(Building root)
		{
			return new PowerNet(PowerNetMaker.ContiguousPowerBuildings(root));
		}

		// Token: 0x0600696A RID: 26986 RVA: 0x00006A05 File Offset: 0x00004C05
		public static void UpdateVisualLinkagesFor(PowerNet net)
		{
		}

		// Token: 0x04004637 RID: 17975
		private static HashSet<Building> closedSet = new HashSet<Building>();

		// Token: 0x04004638 RID: 17976
		private static HashSet<Building> openSet = new HashSet<Building>();

		// Token: 0x04004639 RID: 17977
		private static HashSet<Building> currentSet = new HashSet<Building>();
	}
}
