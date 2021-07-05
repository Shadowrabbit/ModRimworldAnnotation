using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C7E RID: 3198
	public class ListerBuildingsRepairable
	{
		// Token: 0x06004A89 RID: 19081 RVA: 0x0018A548 File Offset: 0x00188748
		public List<Thing> RepairableBuildings(Faction fac)
		{
			return this.ListFor(fac);
		}

		// Token: 0x06004A8A RID: 19082 RVA: 0x0018A551 File Offset: 0x00188751
		public bool Contains(Faction fac, Building b)
		{
			return this.HashSetFor(fac).Contains(b);
		}

		// Token: 0x06004A8B RID: 19083 RVA: 0x0018A560 File Offset: 0x00188760
		public void Notify_BuildingSpawned(Building b)
		{
			if (b.Faction == null)
			{
				return;
			}
			this.UpdateBuilding(b);
		}

		// Token: 0x06004A8C RID: 19084 RVA: 0x0018A572 File Offset: 0x00188772
		public void Notify_BuildingDeSpawned(Building b)
		{
			if (b.Faction == null)
			{
				return;
			}
			this.ListFor(b.Faction).Remove(b);
			this.HashSetFor(b.Faction).Remove(b);
		}

		// Token: 0x06004A8D RID: 19085 RVA: 0x0018A560 File Offset: 0x00188760
		public void Notify_BuildingTookDamage(Building b)
		{
			if (b.Faction == null)
			{
				return;
			}
			this.UpdateBuilding(b);
		}

		// Token: 0x06004A8E RID: 19086 RVA: 0x0018A560 File Offset: 0x00188760
		public void Notify_BuildingRepaired(Building b)
		{
			if (b.Faction == null)
			{
				return;
			}
			this.UpdateBuilding(b);
		}

		// Token: 0x06004A8F RID: 19087 RVA: 0x0018A5A4 File Offset: 0x001887A4
		private void UpdateBuilding(Building b)
		{
			if (b.Faction == null || !b.def.building.repairable)
			{
				return;
			}
			List<Thing> list = this.ListFor(b.Faction);
			HashSet<Thing> hashSet = this.HashSetFor(b.Faction);
			if (b.HitPoints < b.MaxHitPoints)
			{
				if (!list.Contains(b))
				{
					list.Add(b);
				}
				hashSet.Add(b);
				return;
			}
			list.Remove(b);
			hashSet.Remove(b);
		}

		// Token: 0x06004A90 RID: 19088 RVA: 0x0018A620 File Offset: 0x00188820
		private List<Thing> ListFor(Faction fac)
		{
			List<Thing> list;
			if (!this.repairables.TryGetValue(fac, out list))
			{
				list = new List<Thing>();
				this.repairables.Add(fac, list);
			}
			return list;
		}

		// Token: 0x06004A91 RID: 19089 RVA: 0x0018A654 File Offset: 0x00188854
		private HashSet<Thing> HashSetFor(Faction fac)
		{
			HashSet<Thing> hashSet;
			if (!this.repairablesSet.TryGetValue(fac, out hashSet))
			{
				hashSet = new HashSet<Thing>();
				this.repairablesSet.Add(fac, hashSet);
			}
			return hashSet;
		}

		// Token: 0x06004A92 RID: 19090 RVA: 0x0018A688 File Offset: 0x00188888
		internal string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				List<Thing> list = this.ListFor(faction);
				if (!list.NullOrEmpty<Thing>())
				{
					stringBuilder.AppendLine(string.Concat(new object[]
					{
						"=======",
						faction.Name,
						" (",
						faction.def,
						")"
					}));
					foreach (Thing thing in list)
					{
						stringBuilder.AppendLine(thing.ThingID);
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04002D47 RID: 11591
		private Dictionary<Faction, List<Thing>> repairables = new Dictionary<Faction, List<Thing>>();

		// Token: 0x04002D48 RID: 11592
		private Dictionary<Faction, HashSet<Thing>> repairablesSet = new Dictionary<Faction, HashSet<Thing>>();
	}
}
