using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C7D RID: 3197
	public class ListerBuildingWithTagInProximity
	{
		// Token: 0x06004A85 RID: 19077 RVA: 0x0018A3E8 File Offset: 0x001885E8
		public ListerBuildingWithTagInProximity(Map map)
		{
			this.map = map;
		}

		// Token: 0x06004A86 RID: 19078 RVA: 0x0018A402 File Offset: 0x00188602
		public void Notify_BuildingSpawned(Building b)
		{
			this.requestCache.Clear();
		}

		// Token: 0x06004A87 RID: 19079 RVA: 0x0018A402 File Offset: 0x00188602
		public void Notify_BuildingDeSpawned(Building b)
		{
			this.requestCache.Clear();
		}

		// Token: 0x06004A88 RID: 19080 RVA: 0x0018A410 File Offset: 0x00188610
		public List<Thing> GetForCell(IntVec3 cell, float radius, string tag, Thing forThing = null)
		{
			ListerBuildingWithTagInProximity.CellRequest key = new ListerBuildingWithTagInProximity.CellRequest(cell, radius, tag, forThing);
			List<Thing> list;
			if (!this.requestCache.TryGetValue(key, out list))
			{
				list = new List<Thing>();
				foreach (Thing thing in GenRadial.RadialDistinctThingsAround(cell, this.map, radius, false))
				{
					ThingDef thingDef = thing.def;
					ThingDef thingDef2;
					if ((thing.def.IsBlueprint || thing.def.isFrameInt) && (thingDef2 = (thing.def.entityDefToBuild as ThingDef)) != null)
					{
						thingDef = thingDef2;
					}
					if (thingDef.building != null && thingDef.building.buildingTags.Contains(tag) && thing.GetRoom(RegionType.Set_All) == cell.GetRoom(this.map) && thing != forThing)
					{
						list.Add(thing);
					}
				}
				list.SortBy((Thing t) => t.Position.DistanceTo(cell));
				this.requestCache[key] = list;
			}
			return list;
		}

		// Token: 0x04002D45 RID: 11589
		private Map map;

		// Token: 0x04002D46 RID: 11590
		private Dictionary<ListerBuildingWithTagInProximity.CellRequest, List<Thing>> requestCache = new Dictionary<ListerBuildingWithTagInProximity.CellRequest, List<Thing>>();

		// Token: 0x02002184 RID: 8580
		public struct CellRequest : IEquatable<ListerBuildingWithTagInProximity.CellRequest>
		{
			// Token: 0x0600BF65 RID: 48997 RVA: 0x003CE1CD File Offset: 0x003CC3CD
			public CellRequest(IntVec3 c, float r, string tag, Thing t = null)
			{
				this.cell = c;
				this.radius = r;
				this.tag = tag;
				this.forThing = t;
			}

			// Token: 0x0600BF66 RID: 48998 RVA: 0x003CE1EC File Offset: 0x003CC3EC
			public bool Equals(ListerBuildingWithTagInProximity.CellRequest other)
			{
				return this.cell.Equals(other.cell) && this.radius.Equals(other.radius) && this.tag == other.tag && this.forThing == other.forThing;
			}

			// Token: 0x0600BF67 RID: 48999 RVA: 0x003CE248 File Offset: 0x003CC448
			public override int GetHashCode()
			{
				int num = this.cell.GetHashCode();
				num = (num * 397 ^ this.radius.GetHashCode());
				if (this.forThing != null)
				{
					num = (num * 397 ^ this.forThing.GetHashCode());
				}
				return num ^ this.tag.GetHashCode();
			}

			// Token: 0x04008043 RID: 32835
			public readonly IntVec3 cell;

			// Token: 0x04008044 RID: 32836
			public readonly float radius;

			// Token: 0x04008045 RID: 32837
			public readonly Thing forThing;

			// Token: 0x04008046 RID: 32838
			public readonly string tag;
		}
	}
}
