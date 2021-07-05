using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200016F RID: 367
	public abstract class AnimalPenEnclosureCalculator
	{
		// Token: 0x06000A3C RID: 2620 RVA: 0x0003864F File Offset: 0x0003684F
		protected AnimalPenEnclosureCalculator()
		{
			this.regionProcessor = new RegionProcessor(this.ProcessRegion);
			this.regionEntryPredicate = new RegionEntryPredicate(this.EnterRegion);
		}

		// Token: 0x06000A3D RID: 2621 RVA: 0x0000313F File Offset: 0x0000133F
		protected virtual void VisitDirectlyConnectedRegion(Region r)
		{
		}

		// Token: 0x06000A3E RID: 2622 RVA: 0x0000313F File Offset: 0x0000133F
		protected virtual void VisitIndirectlyDirectlyConnectedRegion(Region r)
		{
		}

		// Token: 0x06000A3F RID: 2623 RVA: 0x0000313F File Offset: 0x0000133F
		protected virtual void VisitPassableDoorway(Region r)
		{
		}

		// Token: 0x06000A40 RID: 2624 RVA: 0x0000313F File Offset: 0x0000133F
		protected virtual void VisitImpassableDoorway(Region r)
		{
		}

		// Token: 0x06000A41 RID: 2625 RVA: 0x0003867C File Offset: 0x0003687C
		protected bool VisitPen(IntVec3 position, Map map)
		{
			this.rootDistrict = position.GetDistrict(map, RegionType.Set_Passable);
			if (this.rootDistrict == null || this.rootDistrict.TouchesMapEdge)
			{
				return false;
			}
			this.isEnclosed = true;
			RegionTraverser.BreadthFirstTraverse(position, map, this.regionEntryPredicate, this.regionProcessor, 999999, RegionType.Set_Passable);
			return this.isEnclosed;
		}

		// Token: 0x06000A42 RID: 2626 RVA: 0x000386D6 File Offset: 0x000368D6
		public static bool RoamerCanPass(Building_Door door)
		{
			return door.FreePassage || AnimalPenEnclosureCalculator.RoamerCanPass(door.def);
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x000386ED File Offset: 0x000368ED
		public static bool RoamerCanPass(ThingDef thingDef)
		{
			return thingDef.building.roamerCanOpen;
		}

		// Token: 0x06000A44 RID: 2628 RVA: 0x000386FA File Offset: 0x000368FA
		private bool EnterRegion(Region from, Region to)
		{
			return (!from.IsDoorway || AnimalPenEnclosureCalculator.RoamerCanPass(from.door)) && (to.type == RegionType.Normal || to.IsDoorway);
		}

		// Token: 0x06000A45 RID: 2629 RVA: 0x00038724 File Offset: 0x00036924
		private bool ProcessRegion(Region reg)
		{
			if (reg.touchesMapEdge)
			{
				this.isEnclosed = false;
				return true;
			}
			if (reg.type == RegionType.Normal)
			{
				if (reg.District == this.rootDistrict)
				{
					this.VisitDirectlyConnectedRegion(reg);
				}
				else
				{
					this.VisitIndirectlyDirectlyConnectedRegion(reg);
				}
			}
			else if (reg.IsDoorway)
			{
				if (AnimalPenEnclosureCalculator.RoamerCanPass(reg.door))
				{
					this.VisitPassableDoorway(reg);
				}
				else
				{
					this.VisitImpassableDoorway(reg);
				}
			}
			return false;
		}

		// Token: 0x040008CC RID: 2252
		private District rootDistrict;

		// Token: 0x040008CD RID: 2253
		private bool isEnclosed;

		// Token: 0x040008CE RID: 2254
		private readonly RegionProcessor regionProcessor;

		// Token: 0x040008CF RID: 2255
		private readonly RegionEntryPredicate regionEntryPredicate;
	}
}
