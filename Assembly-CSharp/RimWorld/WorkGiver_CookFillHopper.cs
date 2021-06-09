using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D69 RID: 3433
	public class WorkGiver_CookFillHopper : WorkGiver_Scanner
	{
		// Token: 0x17000BFC RID: 3068
		// (get) Token: 0x06004E5D RID: 20061 RVA: 0x000374FA File Offset: 0x000356FA
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(ThingDefOf.Hopper);
			}
		}

		// Token: 0x17000BFD RID: 3069
		// (get) Token: 0x06004E5E RID: 20062 RVA: 0x0002EB44 File Offset: 0x0002CD44
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.ClosestTouch;
			}
		}

		// Token: 0x06004E5F RID: 20063 RVA: 0x00037506 File Offset: 0x00035706
		public WorkGiver_CookFillHopper()
		{
			if (WorkGiver_CookFillHopper.TheOnlyAvailableFoodIsInStorageOfHigherPriorityTrans == null)
			{
				WorkGiver_CookFillHopper.TheOnlyAvailableFoodIsInStorageOfHigherPriorityTrans = "TheOnlyAvailableFoodIsInStorageOfHigherPriority".Translate();
			}
			if (WorkGiver_CookFillHopper.NoFoodToFillHopperTrans == null)
			{
				WorkGiver_CookFillHopper.NoFoodToFillHopperTrans = "NoFoodToFillHopper".Translate();
			}
		}

		// Token: 0x06004E60 RID: 20064 RVA: 0x001B101C File Offset: 0x001AF21C
		public override Job JobOnThing(Pawn pawn, Thing thing, bool forced = false)
		{
			ISlotGroupParent slotGroupParent = thing as ISlotGroupParent;
			if (slotGroupParent == null)
			{
				return null;
			}
			if (!pawn.CanReserve(thing.Position, 1, -1, null, false))
			{
				return null;
			}
			int num = 0;
			List<Thing> list = pawn.Map.thingGrid.ThingsListAt(thing.Position);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing2 = list[i];
				if (Building_NutrientPasteDispenser.IsAcceptableFeedstock(thing2.def))
				{
					num += thing2.stackCount;
				}
			}
			if (num > 25)
			{
				JobFailReason.Is("AlreadyFilledLower".Translate(), null);
				return null;
			}
			return WorkGiver_CookFillHopper.HopperFillFoodJob(pawn, slotGroupParent);
		}

		// Token: 0x06004E61 RID: 20065 RVA: 0x001B10BC File Offset: 0x001AF2BC
		public static Job HopperFillFoodJob(Pawn pawn, ISlotGroupParent hopperSgp)
		{
			Building building = (Building)hopperSgp;
			if (!pawn.CanReserveAndReach(building.Position, PathEndMode.Touch, pawn.NormalMaxDanger(), 1, -1, null, false))
			{
				return null;
			}
			ThingDef thingDef = null;
			Thing firstItem = building.Position.GetFirstItem(building.Map);
			if (firstItem != null)
			{
				if (Building_NutrientPasteDispenser.IsAcceptableFeedstock(firstItem.def))
				{
					thingDef = firstItem.def;
				}
				else
				{
					if (firstItem.IsForbidden(pawn))
					{
						return null;
					}
					return HaulAIUtility.HaulAsideJobFor(pawn, firstItem);
				}
			}
			List<Thing> list;
			if (thingDef == null)
			{
				list = pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.FoodSourceNotPlantOrTree);
			}
			else
			{
				list = pawn.Map.listerThings.ThingsOfDef(thingDef);
			}
			bool flag = false;
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				if (thing.def.IsNutritionGivingIngestible && (thing.def.ingestible.preferability == FoodPreferability.RawBad || thing.def.ingestible.preferability == FoodPreferability.RawTasty) && HaulAIUtility.PawnCanAutomaticallyHaul(pawn, thing, false) && pawn.Map.haulDestinationManager.SlotGroupAt(building.Position).Settings.AllowedToAccept(thing))
				{
					if (StoreUtility.CurrentStoragePriorityOf(thing) >= hopperSgp.GetSlotGroup().Settings.Priority)
					{
						flag = true;
						JobFailReason.Is(WorkGiver_CookFillHopper.TheOnlyAvailableFoodIsInStorageOfHigherPriorityTrans, null);
					}
					else
					{
						Job job = HaulAIUtility.HaulToCellStorageJob(pawn, thing, building.Position, true);
						if (job != null)
						{
							return job;
						}
					}
				}
			}
			if (!flag)
			{
				JobFailReason.Is(WorkGiver_CookFillHopper.NoFoodToFillHopperTrans, null);
			}
			return null;
		}

		// Token: 0x0400331D RID: 13085
		private static string TheOnlyAvailableFoodIsInStorageOfHigherPriorityTrans;

		// Token: 0x0400331E RID: 13086
		private static string NoFoodToFillHopperTrans;
	}
}
