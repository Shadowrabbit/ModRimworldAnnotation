using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C8D RID: 3213
	public class JobGiver_MoveDrugsToInventory : ThinkNode_JobGiver
	{
		// Token: 0x06004AF8 RID: 19192 RVA: 0x001A3BD4 File Offset: 0x001A1DD4
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

		// Token: 0x06004AF9 RID: 19193 RVA: 0x001A3C24 File Offset: 0x001A1E24
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

		// Token: 0x06004AFA RID: 19194 RVA: 0x001A3CBC File Offset: 0x001A1EBC
		private Thing FindDrugFor(Pawn pawn, ThingDef drugDef)
		{
			return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(drugDef), PathEndMode.ClosestTouch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, (Thing x) => this.DrugValidator(pawn, x), null, 0, -1, false, RegionType.Set_Passable, false);
		}

		// Token: 0x06004AFB RID: 19195 RVA: 0x0003586C File Offset: 0x00033A6C
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
