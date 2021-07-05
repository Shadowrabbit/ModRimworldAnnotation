using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007D4 RID: 2004
	public class JobGiver_TakeForInventoryStock : ThinkNode_JobGiver
	{
		// Token: 0x060035E1 RID: 13793 RVA: 0x001312E4 File Offset: 0x0012F4E4
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (Find.TickManager.TicksGame < pawn.mindState.nextInventoryStockTick)
			{
				return null;
			}
			if (!pawn.inventoryStock.AnyThingsRequiredNow())
			{
				return null;
			}
			foreach (InventoryStockEntry inventoryStockEntry in pawn.inventoryStock.stockEntries.Values)
			{
				if (pawn.inventory.Count(inventoryStockEntry.thingDef) < inventoryStockEntry.count)
				{
					Thing thing = this.FindThingFor(pawn, inventoryStockEntry.thingDef);
					if (thing != null)
					{
						Job job = JobMaker.MakeJob(JobDefOf.TakeCountToInventory, thing);
						int b = inventoryStockEntry.count - pawn.inventory.Count(thing.def);
						job.count = Mathf.Min(thing.stackCount, b);
						return job;
					}
				}
			}
			pawn.mindState.nextInventoryStockTick = Find.TickManager.TicksGame + Rand.Range(6000, 9000);
			return null;
		}

		// Token: 0x060035E2 RID: 13794 RVA: 0x001313F4 File Offset: 0x0012F5F4
		private Thing FindThingFor(Pawn pawn, ThingDef thingDef)
		{
			return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(thingDef), PathEndMode.ClosestTouch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false), 9999f, (Thing x) => this.ThingValidator(pawn, x), null, 0, -1, false, RegionType.Set_Passable, false);
		}

		// Token: 0x060035E3 RID: 13795 RVA: 0x0013145F File Offset: 0x0012F65F
		private bool ThingValidator(Pawn pawn, Thing thing)
		{
			return !thing.IsForbidden(pawn) && pawn.CanReserve(thing, 1, -1, null, false);
		}

		// Token: 0x04001EC6 RID: 7878
		private const int InventoryStockCheckIntervalMin = 6000;

		// Token: 0x04001EC7 RID: 7879
		private const int InventoryStockCheckIntervalMax = 9000;
	}
}
