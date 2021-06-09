using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000271 RID: 625
	public sealed class ListerBuildings
	{
		// Token: 0x0600102B RID: 4139 RVA: 0x000B8B48 File Offset: 0x000B6D48
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
		}

		// Token: 0x0600102C RID: 4140 RVA: 0x000B8BD8 File Offset: 0x000B6DD8
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
		}

		// Token: 0x0600102D RID: 4141 RVA: 0x000B8C38 File Offset: 0x000B6E38
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

		// Token: 0x0600102E RID: 4142 RVA: 0x000B8C74 File Offset: 0x000B6E74
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

		// Token: 0x0600102F RID: 4143 RVA: 0x000B8CB0 File Offset: 0x000B6EB0
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

		// Token: 0x06001030 RID: 4144 RVA: 0x000B8CEC File Offset: 0x000B6EEC
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

		// Token: 0x06001031 RID: 4145 RVA: 0x000120FB File Offset: 0x000102FB
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

		// Token: 0x06001032 RID: 4146 RVA: 0x00012112 File Offset: 0x00010312
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

		// Token: 0x04000CDF RID: 3295
		public List<Building> allBuildingsColonist = new List<Building>();

		// Token: 0x04000CE0 RID: 3296
		public List<Building> allBuildingsNonColonist = new List<Building>();

		// Token: 0x04000CE1 RID: 3297
		public HashSet<Building> allBuildingsColonistCombatTargets = new HashSet<Building>();

		// Token: 0x04000CE2 RID: 3298
		public HashSet<Building> allBuildingsColonistElecFire = new HashSet<Building>();
	}
}
