using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000172 RID: 370
	public class AnimalPenBlueprintEnclosureCalculator
	{
		// Token: 0x06000A5A RID: 2650 RVA: 0x000389D6 File Offset: 0x00036BD6
		public AnimalPenBlueprintEnclosureCalculator()
		{
			this.passCheck = new Predicate<IntVec3>(this.PassCheck);
			this.cellProcessor = new Func<IntVec3, bool>(this.CellProcessor);
		}

		// Token: 0x06000A5B RID: 2651 RVA: 0x00038A10 File Offset: 0x00036C10
		public void VisitPen(IntVec3 position, Map map)
		{
			int num = Gen.HashCombineInt(map.listerThings.StateHashOfGroup(ThingRequestGroup.Blueprint), map.listerThings.StateHashOfGroup(ThingRequestGroup.BuildingFrame), map.listerThings.StateHashOfGroup(ThingRequestGroup.BuildingArtificial), 42);
			if (this.map == null || this.map != map || !this.last_position.Equals(position) || this.last_stateHash != num)
			{
				this.map = map;
				this.last_position = position;
				this.last_stateHash = num;
				this.isEnclosed = true;
				this.cellsFound.Clear();
				this.FloodFill(position);
			}
		}

		// Token: 0x06000A5C RID: 2652 RVA: 0x00038AA6 File Offset: 0x00036CA6
		private void FloodFill(IntVec3 position)
		{
			this.map.floodFiller.FloodFill(position, this.passCheck, this.cellProcessor, int.MaxValue, false, null);
		}

		// Token: 0x06000A5D RID: 2653 RVA: 0x00038ACC File Offset: 0x00036CCC
		private bool CellProcessor(IntVec3 c)
		{
			this.cellsFound.Add(c);
			if (c.OnEdge(this.map))
			{
				this.isEnclosed = false;
				return true;
			}
			return false;
		}

		// Token: 0x06000A5E RID: 2654 RVA: 0x00038AF4 File Offset: 0x00036CF4
		private bool PassCheck(IntVec3 c)
		{
			if (!c.WalkableByFenceBlocked(this.map))
			{
				return false;
			}
			foreach (Thing thing in c.GetThingList(this.map))
			{
				ThingDef def = thing.def;
				if (def.passability == Traversability.Impassable)
				{
					return false;
				}
				Building_Door door;
				ThingDef thingDef;
				if ((door = (thing as Building_Door)) != null)
				{
					if (AnimalPenEnclosureCalculator.RoamerCanPass(door))
					{
						return true;
					}
					return false;
				}
				else if ((def.IsBlueprint || def.IsFrame) && (thingDef = (def.entityDefToBuild as ThingDef)) != null)
				{
					if (thingDef.IsFence || thingDef.passability == Traversability.Impassable)
					{
						return false;
					}
					if (thingDef.IsDoor)
					{
						if (AnimalPenEnclosureCalculator.RoamerCanPass(thingDef))
						{
							return true;
						}
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x040008D7 RID: 2263
		private readonly Predicate<IntVec3> passCheck;

		// Token: 0x040008D8 RID: 2264
		private readonly Func<IntVec3, bool> cellProcessor;

		// Token: 0x040008D9 RID: 2265
		public bool isEnclosed;

		// Token: 0x040008DA RID: 2266
		public List<IntVec3> cellsFound = new List<IntVec3>();

		// Token: 0x040008DB RID: 2267
		private Map map;

		// Token: 0x040008DC RID: 2268
		private IntVec3 last_position;

		// Token: 0x040008DD RID: 2269
		private int last_stateHash;
	}
}
