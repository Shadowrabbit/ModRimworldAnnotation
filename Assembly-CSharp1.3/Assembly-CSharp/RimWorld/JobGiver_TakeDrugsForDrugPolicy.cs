using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000770 RID: 1904
	public class JobGiver_TakeDrugsForDrugPolicy : ThinkNode_JobGiver
	{
		// Token: 0x06003480 RID: 13440 RVA: 0x00129A48 File Offset: 0x00127C48
		public override float GetPriority(Pawn pawn)
		{
			DrugPolicy currentPolicy = pawn.drugs.CurrentPolicy;
			for (int i = 0; i < currentPolicy.Count; i++)
			{
				if (pawn.drugs.ShouldTryToTakeScheduledNow(currentPolicy[i].drug))
				{
					return 7.5f;
				}
			}
			return 0f;
		}

		// Token: 0x06003481 RID: 13441 RVA: 0x00129A98 File Offset: 0x00127C98
		protected override Job TryGiveJob(Pawn pawn)
		{
			DrugPolicy currentPolicy = pawn.drugs.CurrentPolicy;
			for (int i = 0; i < currentPolicy.Count; i++)
			{
				if (pawn.drugs.ShouldTryToTakeScheduledNow(currentPolicy[i].drug))
				{
					Thing thing = this.FindDrugFor(pawn, currentPolicy[i].drug);
					if (thing != null)
					{
						return DrugAIUtility.IngestAndTakeToInventoryJob(thing, pawn, 1);
					}
				}
			}
			return null;
		}

		// Token: 0x06003482 RID: 13442 RVA: 0x00129AFC File Offset: 0x00127CFC
		private Thing FindDrugFor(Pawn pawn, ThingDef drugDef)
		{
			ThingOwner<Thing> innerContainer = pawn.inventory.innerContainer;
			for (int i = 0; i < innerContainer.Count; i++)
			{
				if (innerContainer[i].def == drugDef && this.DrugValidator(pawn, innerContainer[i]))
				{
					return innerContainer[i];
				}
			}
			return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(drugDef), PathEndMode.ClosestTouch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false), 9999f, (Thing x) => this.DrugValidator(pawn, x), null, 0, -1, false, RegionType.Set_Passable, false);
		}

		// Token: 0x06003483 RID: 13443 RVA: 0x00129BB8 File Offset: 0x00127DB8
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
				if (!drug.IsSociallyProper(pawn))
				{
					return false;
				}
			}
			return true;
		}
	}
}
