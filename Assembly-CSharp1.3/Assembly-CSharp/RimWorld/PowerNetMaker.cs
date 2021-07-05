using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CD9 RID: 3289
	public static class PowerNetMaker
	{
		// Token: 0x06004CBF RID: 19647 RVA: 0x001998F8 File Offset: 0x00197AF8
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

		// Token: 0x06004CC0 RID: 19648 RVA: 0x00199AFC File Offset: 0x00197CFC
		public static PowerNet NewPowerNetStartingFrom(Building root)
		{
			return new PowerNet(PowerNetMaker.ContiguousPowerBuildings(root));
		}

		// Token: 0x06004CC1 RID: 19649 RVA: 0x0000313F File Offset: 0x0000133F
		public static void UpdateVisualLinkagesFor(PowerNet net)
		{
		}

		// Token: 0x04002E79 RID: 11897
		private static HashSet<Building> closedSet = new HashSet<Building>();

		// Token: 0x04002E7A RID: 11898
		private static HashSet<Building> openSet = new HashSet<Building>();

		// Token: 0x04002E7B RID: 11899
		private static HashSet<Building> currentSet = new HashSet<Building>();
	}
}
