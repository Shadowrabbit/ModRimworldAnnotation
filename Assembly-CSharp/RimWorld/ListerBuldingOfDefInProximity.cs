using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001269 RID: 4713
	public class ListerBuldingOfDefInProximity
	{
		// Token: 0x060066C3 RID: 26307 RVA: 0x0004639C File Offset: 0x0004459C
		public ListerBuldingOfDefInProximity(Map map)
		{
			this.map = map;
		}

		// Token: 0x060066C4 RID: 26308 RVA: 0x000463B6 File Offset: 0x000445B6
		public void Notify_BuildingSpawned(Building b)
		{
			this.requestCache.Clear();
		}

		// Token: 0x060066C5 RID: 26309 RVA: 0x000463B6 File Offset: 0x000445B6
		public void Notify_BuildingDeSpawned(Building b)
		{
			this.requestCache.Clear();
		}

		// Token: 0x060066C6 RID: 26310 RVA: 0x001FA118 File Offset: 0x001F8318
		public List<Thing> GetForCell(IntVec3 cell, float radius, List<MeditationFocusOffsetPerBuilding> defs, Thing forThing = null)
		{
			ListerBuldingOfDefInProximity.CellRequest key = new ListerBuldingOfDefInProximity.CellRequest(cell, radius, defs, forThing);
			List<Thing> list;
			if (!this.requestCache.TryGetValue(key, out list))
			{
				list = new List<Thing>();
				using (IEnumerator<Thing> enumerator = GenRadial.RadialDistinctThingsAround(cell, this.map, radius, false).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Thing t = enumerator.Current;
						if (defs.Any((MeditationFocusOffsetPerBuilding d) => d.building == t.def) && t.GetRoom(RegionType.Set_Passable) == cell.GetRoom(this.map, RegionType.Set_Passable) && t != forThing)
						{
							list.Add(t);
						}
					}
				}
				list.SortBy(delegate(Thing t)
				{
					float num = t.Position.DistanceTo(cell);
					MeditationFocusOffsetPerBuilding meditationFocusOffsetPerBuilding = defs.FirstOrDefault((MeditationFocusOffsetPerBuilding d) => d.building == t.def);
					if (meditationFocusOffsetPerBuilding != null)
					{
						num -= meditationFocusOffsetPerBuilding.offset * 100000f;
					}
					return num;
				});
				this.requestCache[key] = list;
			}
			return list;
		}

		// Token: 0x04004460 RID: 17504
		private Map map;

		// Token: 0x04004461 RID: 17505
		private Dictionary<ListerBuldingOfDefInProximity.CellRequest, List<Thing>> requestCache = new Dictionary<ListerBuldingOfDefInProximity.CellRequest, List<Thing>>();

		// Token: 0x0200126A RID: 4714
		public struct CellRequest : IEquatable<ListerBuldingOfDefInProximity.CellRequest>
		{
			// Token: 0x060066C7 RID: 26311 RVA: 0x000463C3 File Offset: 0x000445C3
			public CellRequest(IntVec3 c, float r, List<MeditationFocusOffsetPerBuilding> d, Thing t = null)
			{
				this.cell = c;
				this.radius = r;
				this.defs = d;
				this.forThing = t;
			}

			// Token: 0x060066C8 RID: 26312 RVA: 0x001FA230 File Offset: 0x001F8430
			public bool Equals(ListerBuldingOfDefInProximity.CellRequest other)
			{
				return this.cell.Equals(other.cell) && this.radius.Equals(other.radius) && GenCollection.ListsEqual<MeditationFocusOffsetPerBuilding>(this.defs, other.defs) && this.forThing == other.forThing;
			}

			// Token: 0x060066C9 RID: 26313 RVA: 0x001FA28C File Offset: 0x001F848C
			public override int GetHashCode()
			{
				int num = this.cell.GetHashCode();
				num = (num * 397 ^ this.radius.GetHashCode());
				if (this.forThing != null)
				{
					num = (num * 397 ^ this.forThing.GetHashCode());
				}
				for (int i = 0; i < this.defs.Count; i++)
				{
					num ^= this.defs[i].GetHashCode();
				}
				return num;
			}

			// Token: 0x04004462 RID: 17506
			public readonly IntVec3 cell;

			// Token: 0x04004463 RID: 17507
			public readonly float radius;

			// Token: 0x04004464 RID: 17508
			public readonly Thing forThing;

			// Token: 0x04004465 RID: 17509
			public readonly List<MeditationFocusOffsetPerBuilding> defs;
		}
	}
}
