using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C7F RID: 3199
	public class ListerBuldingOfDefInProximity
	{
		// Token: 0x06004A94 RID: 19092 RVA: 0x0018A796 File Offset: 0x00188996
		public ListerBuldingOfDefInProximity(Map map)
		{
			this.map = map;
		}

		// Token: 0x06004A95 RID: 19093 RVA: 0x0018A7B0 File Offset: 0x001889B0
		public void Notify_BuildingSpawned(Building b)
		{
			this.requestCache.Clear();
		}

		// Token: 0x06004A96 RID: 19094 RVA: 0x0018A7B0 File Offset: 0x001889B0
		public void Notify_BuildingDeSpawned(Building b)
		{
			this.requestCache.Clear();
		}

		// Token: 0x06004A97 RID: 19095 RVA: 0x0018A7C0 File Offset: 0x001889C0
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
						if (defs.Any((MeditationFocusOffsetPerBuilding d) => d.building == t.def) && t.GetRoom(RegionType.Set_All) == cell.GetRoom(this.map) && t != forThing)
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

		// Token: 0x06004A98 RID: 19096 RVA: 0x0018A8D8 File Offset: 0x00188AD8
		public List<Thing> GetForCell(IntVec3 cell, float radius, ThingDef def, Thing forThing = null)
		{
			ListerBuldingOfDefInProximity.CellRequest key = new ListerBuldingOfDefInProximity.CellRequest(cell, radius, def, forThing);
			List<Thing> list;
			if (!this.requestCache.TryGetValue(key, out list))
			{
				list = new List<Thing>();
				foreach (Thing thing in GenRadial.RadialDistinctThingsAround(cell, this.map, radius, false))
				{
					if (def == thing.def && thing.GetRoom(RegionType.Set_All) == cell.GetRoom(this.map) && thing != forThing)
					{
						list.Add(thing);
					}
				}
				this.requestCache[key] = list;
			}
			return list;
		}

		// Token: 0x04002D49 RID: 11593
		private Map map;

		// Token: 0x04002D4A RID: 11594
		private Dictionary<ListerBuldingOfDefInProximity.CellRequest, List<Thing>> requestCache = new Dictionary<ListerBuldingOfDefInProximity.CellRequest, List<Thing>>();

		// Token: 0x02002186 RID: 8582
		public struct CellRequest : IEquatable<ListerBuldingOfDefInProximity.CellRequest>
		{
			// Token: 0x0600BF6A RID: 49002 RVA: 0x003CE2BF File Offset: 0x003CC4BF
			public CellRequest(IntVec3 c, float r, List<MeditationFocusOffsetPerBuilding> d, Thing t = null)
			{
				this.cell = c;
				this.radius = r;
				this.defs = d;
				this.forThing = t;
				this.def = null;
			}

			// Token: 0x0600BF6B RID: 49003 RVA: 0x003CE2E5 File Offset: 0x003CC4E5
			public CellRequest(IntVec3 c, float r, ThingDef d, Thing t = null)
			{
				this.cell = c;
				this.radius = r;
				this.def = d;
				this.defs = null;
				this.forThing = t;
			}

			// Token: 0x0600BF6C RID: 49004 RVA: 0x003CE30C File Offset: 0x003CC50C
			public bool Equals(ListerBuldingOfDefInProximity.CellRequest other)
			{
				return this.cell.Equals(other.cell) && this.radius.Equals(other.radius) && GenCollection.ListsEqual<MeditationFocusOffsetPerBuilding>(this.defs, other.defs) && this.def == other.def && this.forThing == other.forThing;
			}

			// Token: 0x0600BF6D RID: 49005 RVA: 0x003CE378 File Offset: 0x003CC578
			public override int GetHashCode()
			{
				int num = this.cell.GetHashCode();
				num = (num * 397 ^ this.radius.GetHashCode());
				if (this.forThing != null)
				{
					num = (num * 397 ^ this.forThing.GetHashCode());
				}
				if (this.defs != null)
				{
					for (int i = 0; i < this.defs.Count; i++)
					{
						num ^= this.defs[i].GetHashCode();
					}
				}
				if (this.def != null)
				{
					num ^= this.def.GetHashCode();
				}
				return num;
			}

			// Token: 0x04008048 RID: 32840
			public readonly IntVec3 cell;

			// Token: 0x04008049 RID: 32841
			public readonly float radius;

			// Token: 0x0400804A RID: 32842
			public readonly Thing forThing;

			// Token: 0x0400804B RID: 32843
			public readonly ThingDef def;

			// Token: 0x0400804C RID: 32844
			public readonly List<MeditationFocusOffsetPerBuilding> defs;
		}
	}
}
