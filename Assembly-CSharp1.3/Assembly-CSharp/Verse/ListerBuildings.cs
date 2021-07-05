using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x020001B8 RID: 440
	public sealed class ListerBuildings
	{
		// Token: 0x06000CA3 RID: 3235 RVA: 0x000433F0 File Offset: 0x000415F0
		public void Add(Building b)
		{
			if (b.def.building != null && b.def.building.isNaturalRock)
			{
				return;
			}
			if (b.Faction == Faction.OfPlayer)
			{
				this.allBuildingsColonist.Add(b);
				if (b is IAttackTarget)
				{
					this.allBuildingsColonistCombatTargets.Add(b);
				}
			}
			else
			{
				this.allBuildingsNonColonist.Add(b);
			}
			CompProperties_Power compProperties = b.def.GetCompProperties<CompProperties_Power>();
			if (compProperties != null && compProperties.shortCircuitInRain)
			{
				this.allBuildingsColonistElecFire.Add(b);
			}
			if (b.TryGetComp<CompAnimalPenMarker>() != null)
			{
				this.allBuildingsAnimalPenMarkers.Add(b);
			}
		}

		// Token: 0x06000CA4 RID: 3236 RVA: 0x00043494 File Offset: 0x00041694
		public void Remove(Building b)
		{
			this.allBuildingsColonist.Remove(b);
			this.allBuildingsNonColonist.Remove(b);
			if (b is IAttackTarget)
			{
				this.allBuildingsColonistCombatTargets.Remove(b);
			}
			CompProperties_Power compProperties = b.def.GetCompProperties<CompProperties_Power>();
			if (compProperties != null && compProperties.shortCircuitInRain)
			{
				this.allBuildingsColonistElecFire.Remove(b);
			}
			this.allBuildingsAnimalPenMarkers.Remove(b);
		}

		// Token: 0x06000CA5 RID: 3237 RVA: 0x00043501 File Offset: 0x00041701
		public void RegisterInstallBlueprint(Blueprint_Install blueprint)
		{
			this.reinstallationMap.Add(blueprint.MiniToInstallOrBuildingToReinstall.GetInnerIfMinified(), blueprint);
		}

		// Token: 0x06000CA6 RID: 3238 RVA: 0x0004351C File Offset: 0x0004171C
		public void DeregisterInstallBlueprint(Blueprint_Install blueprint)
		{
			Thing miniToInstallOrBuildingToReinstall = blueprint.MiniToInstallOrBuildingToReinstall;
			Thing thing = (miniToInstallOrBuildingToReinstall != null) ? miniToInstallOrBuildingToReinstall.GetInnerIfMinified() : null;
			if (thing != null)
			{
				this.reinstallationMap.Remove(thing);
				return;
			}
			Thing thing2 = null;
			foreach (KeyValuePair<Thing, Blueprint_Install> keyValuePair in this.reinstallationMap)
			{
				if (keyValuePair.Value == blueprint)
				{
					thing2 = keyValuePair.Key;
					break;
				}
			}
			if (thing2 != null)
			{
				this.reinstallationMap.Remove(thing2);
			}
		}

		// Token: 0x06000CA7 RID: 3239 RVA: 0x000435B4 File Offset: 0x000417B4
		public bool ColonistsHaveBuilding(ThingDef def)
		{
			for (int i = 0; i < this.allBuildingsColonist.Count; i++)
			{
				if (this.allBuildingsColonist[i].def == def)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000CA8 RID: 3240 RVA: 0x000435F0 File Offset: 0x000417F0
		public bool ColonistsHaveBuilding(Func<Thing, bool> predicate)
		{
			for (int i = 0; i < this.allBuildingsColonist.Count; i++)
			{
				if (predicate(this.allBuildingsColonist[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000CA9 RID: 3241 RVA: 0x0004362C File Offset: 0x0004182C
		public bool ColonistsHaveResearchBench()
		{
			for (int i = 0; i < this.allBuildingsColonist.Count; i++)
			{
				if (this.allBuildingsColonist[i] is Building_ResearchBench)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000CAA RID: 3242 RVA: 0x00043668 File Offset: 0x00041868
		public bool ColonistsHaveBuildingWithPowerOn(ThingDef def)
		{
			for (int i = 0; i < this.allBuildingsColonist.Count; i++)
			{
				if (this.allBuildingsColonist[i].def == def)
				{
					CompPowerTrader compPowerTrader = this.allBuildingsColonist[i].TryGetComp<CompPowerTrader>();
					if (compPowerTrader == null || compPowerTrader.PowerOn)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000CAB RID: 3243 RVA: 0x000436BF File Offset: 0x000418BF
		public IEnumerable<Building> AllBuildingsColonistOfDef(ThingDef def)
		{
			int num;
			for (int i = 0; i < this.allBuildingsColonist.Count; i = num + 1)
			{
				if (this.allBuildingsColonist[i].def == def)
				{
					yield return this.allBuildingsColonist[i];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06000CAC RID: 3244 RVA: 0x000436D6 File Offset: 0x000418D6
		public bool TryGetReinstallBlueprint(Thing building, out Blueprint_Install bp)
		{
			return this.reinstallationMap.TryGetValue(building, out bp);
		}

		// Token: 0x06000CAD RID: 3245 RVA: 0x000436E5 File Offset: 0x000418E5
		public IEnumerable<T> AllBuildingsColonistOfClass<T>() where T : Building
		{
			int num;
			for (int i = 0; i < this.allBuildingsColonist.Count; i = num + 1)
			{
				T t = this.allBuildingsColonist[i] as T;
				if (t != null)
				{
					yield return t;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x04000A11 RID: 2577
		public List<Building> allBuildingsColonist = new List<Building>();

		// Token: 0x04000A12 RID: 2578
		public List<Building> allBuildingsNonColonist = new List<Building>();

		// Token: 0x04000A13 RID: 2579
		public HashSet<Building> allBuildingsColonistCombatTargets = new HashSet<Building>();

		// Token: 0x04000A14 RID: 2580
		public HashSet<Building> allBuildingsColonistElecFire = new HashSet<Building>();

		// Token: 0x04000A15 RID: 2581
		public HashSet<Building> allBuildingsAnimalPenMarkers = new HashSet<Building>();

		// Token: 0x04000A16 RID: 2582
		private Dictionary<Thing, Blueprint_Install> reinstallationMap = new Dictionary<Thing, Blueprint_Install>();
	}
}
