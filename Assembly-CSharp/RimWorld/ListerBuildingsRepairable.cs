using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001268 RID: 4712
	public class ListerBuildingsRepairable
	{
		// Token: 0x060066B8 RID: 26296 RVA: 0x00046323 File Offset: 0x00044523
		public List<Thing> RepairableBuildings(Faction fac)
		{
			return this.ListFor(fac);
		}

		// Token: 0x060066B9 RID: 26297 RVA: 0x0004632C File Offset: 0x0004452C
		public bool Contains(Faction fac, Building b)
		{
			return this.HashSetFor(fac).Contains(b);
		}

		// Token: 0x060066BA RID: 26298 RVA: 0x0004633B File Offset: 0x0004453B
		public void Notify_BuildingSpawned(Building b)
		{
			if (b.Faction == null)
			{
				return;
			}
			this.UpdateBuilding(b);
		}

		// Token: 0x060066BB RID: 26299 RVA: 0x0004634D File Offset: 0x0004454D
		public void Notify_BuildingDeSpawned(Building b)
		{
			if (b.Faction == null)
			{
				return;
			}
			this.ListFor(b.Faction).Remove(b);
			this.HashSetFor(b.Faction).Remove(b);
		}

		// Token: 0x060066BC RID: 26300 RVA: 0x0004633B File Offset: 0x0004453B
		public void Notify_BuildingTookDamage(Building b)
		{
			if (b.Faction == null)
			{
				return;
			}
			this.UpdateBuilding(b);
		}

		// Token: 0x060066BD RID: 26301 RVA: 0x0004633B File Offset: 0x0004453B
		public void Notify_BuildingRepaired(Building b)
		{
			if (b.Faction == null)
			{
				return;
			}
			this.UpdateBuilding(b);
		}

		// Token: 0x060066BE RID: 26302 RVA: 0x001F9F44 File Offset: 0x001F8144
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

		// Token: 0x060066BF RID: 26303 RVA: 0x001F9FC0 File Offset: 0x001F81C0
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

		// Token: 0x060066C0 RID: 26304 RVA: 0x001F9FF4 File Offset: 0x001F81F4
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

		// Token: 0x060066C1 RID: 26305 RVA: 0x001FA028 File Offset: 0x001F8228
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

		// Token: 0x0400445E RID: 17502
		private Dictionary<Faction, List<Thing>> repairables = new Dictionary<Faction, List<Thing>>();

		// Token: 0x0400445F RID: 17503
		private Dictionary<Faction, HashSet<Thing>> repairablesSet = new Dictionary<Faction, HashSet<Thing>>();
	}
}
