using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000771 RID: 1905
	public class JobGiver_MoveDrugsToInventory : ThinkNode_JobGiver
	{
		// Token: 0x06003485 RID: 13445 RVA: 0x00129C08 File Offset: 0x00127E08
		public override float GetPriority(Pawn pawn)
		{
			DrugPolicy currentPolicy = pawn.drugs.CurrentPolicy;
			for (int i = 0; i < currentPolicy.Count; i++)
			{
				if (pawn.drugs.AllowedToTakeToInventory(currentPolicy[i].drug))
				{
					return 7.5f;
				}
			}
			return 0f;
		}

		// Token: 0x06003486 RID: 13446 RVA: 0x00129C58 File Offset: 0x00127E58
		protected override Job TryGiveJob(Pawn pawn)
		{
			DrugPolicy currentPolicy = pawn.drugs.CurrentPolicy;
			for (int i = 0; i < currentPolicy.Count; i++)
			{
				if (pawn.drugs.AllowedToTakeToInventory(currentPolicy[i].drug))
				{
					Thing thing = this.FindDrugFor(pawn, currentPolicy[i].drug);
					if (thing != null)
					{
						Job job = JobMaker.MakeJob(JobDefOf.TakeInventory, thing);
						job.count = currentPolicy[i].takeToInventory - pawn.inventory.innerContainer.TotalStackCountOfDef(thing.def);
						return job;
					}
				}
			}
			return null;
		}

		// Token: 0x06003487 RID: 13447 RVA: 0x00129CF0 File Offset: 0x00127EF0
		private Thing FindDrugFor(Pawn pawn, ThingDef drugDef)
		{
			return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(drugDef), PathEndMode.ClosestTouch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false), 9999f, (Thing x) => this.DrugValidator(pawn, x), null, 0, -1, false, RegionType.Set_Passable, false);
		}

		// Token: 0x06003488 RID: 13448 RVA: 0x00129D5B File Offset: 0x00127F5B
		private bool DrugValidator(Pawn pawn, Thing drug)
		{
			if (!drug.def.IsDrug)
			{
				return false;
			}
			if (drug.Spawned)
			{
				if (drug.IsForbidden(pawn))
				{
					return false;
				}
				if (!pawn.CanReserve(drug, 10, 1, null, false))
				{
					return false;
				}
			}
			return true;
		}
	}
}
