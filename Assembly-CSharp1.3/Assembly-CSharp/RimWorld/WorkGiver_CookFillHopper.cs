using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000837 RID: 2103
	public class WorkGiver_CookFillHopper : WorkGiver_Scanner
	{
		// Token: 0x170009EB RID: 2539
		// (get) Token: 0x060037AD RID: 14253 RVA: 0x00139AD7 File Offset: 0x00137CD7
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(ThingDefOf.Hopper);
			}
		}

		// Token: 0x170009EC RID: 2540
		// (get) Token: 0x060037AE RID: 14254 RVA: 0x00034716 File Offset: 0x00032916
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.ClosestTouch;
			}
		}

		// Token: 0x060037AF RID: 14255 RVA: 0x00139AE3 File Offset: 0x00137CE3
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

		// Token: 0x060037B0 RID: 14256 RVA: 0x00139B24 File Offset: 0x00137D24
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

		// Token: 0x060037B1 RID: 14257 RVA: 0x00139BC4 File Offset: 0x00137DC4
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

		// Token: 0x04001F18 RID: 7960
		private static string TheOnlyAvailableFoodIsInStorageOfHigherPriorityTrans;

		// Token: 0x04001F19 RID: 7961
		private static string NoFoodToFillHopperTrans;
	}
}
