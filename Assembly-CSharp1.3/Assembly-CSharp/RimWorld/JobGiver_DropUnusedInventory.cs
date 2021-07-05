using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007BD RID: 1981
	public class JobGiver_DropUnusedInventory : ThinkNode_JobGiver
	{
		// Token: 0x06003596 RID: 13718 RVA: 0x0012EDCC File Offset: 0x0012CFCC
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.inventory == null)
			{
				return null;
			}
			if (!pawn.Map.areaManager.Home[pawn.Position])
			{
				return null;
			}
			if (pawn.Faction != Faction.OfPlayer)
			{
				return null;
			}
			if (Find.TickManager.TicksGame > pawn.mindState.lastInventoryRawFoodUseTick + 150000)
			{
				for (int i = pawn.inventory.innerContainer.Count - 1; i >= 0; i--)
				{
					Thing thing = pawn.inventory.innerContainer[i];
					if (thing.def.IsIngestible && !thing.def.IsDrug && thing.def.ingestible.preferability <= FoodPreferability.RawTasty)
					{
						this.Drop(pawn, thing);
					}
				}
			}
			for (int j = pawn.inventory.innerContainer.Count - 1; j >= 0; j--)
			{
				Thing thing2 = pawn.inventory.innerContainer[j];
				if (thing2.def.IsDrug && pawn.drugs != null && !pawn.drugs.AllowedToTakeScheduledEver(thing2.def) && pawn.drugs.CurrentPolicy[thing2.def].takeToInventory == 0 && !AddictionUtility.IsAddicted(pawn, thing2))
				{
					this.Drop(pawn, thing2);
				}
			}
			return null;
		}

		// Token: 0x06003597 RID: 13719 RVA: 0x0012EF18 File Offset: 0x0012D118
		private void Drop(Pawn pawn, Thing thing)
		{
			Thing thing2;
			pawn.inventory.innerContainer.TryDrop(thing, pawn.Position, pawn.Map, ThingPlaceMode.Near, out thing2, null, null);
		}

		// Token: 0x04001EA5 RID: 7845
		private const int RawFoodDropDelay = 150000;
	}
}
