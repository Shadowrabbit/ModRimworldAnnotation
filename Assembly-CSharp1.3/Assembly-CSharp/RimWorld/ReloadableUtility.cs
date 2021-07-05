using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001182 RID: 4482
	public static class ReloadableUtility
	{
		// Token: 0x06006BE3 RID: 27619 RVA: 0x00242F84 File Offset: 0x00241184
		public static CompReloadable FindSomeReloadableComponent(Pawn pawn, bool allowForcedReload)
		{
			if (pawn.apparel == null)
			{
				return null;
			}
			List<Apparel> wornApparel = pawn.apparel.WornApparel;
			for (int i = 0; i < wornApparel.Count; i++)
			{
				CompReloadable compReloadable = wornApparel[i].TryGetComp<CompReloadable>();
				if (compReloadable != null && compReloadable.NeedsReload(allowForcedReload))
				{
					return compReloadable;
				}
			}
			return null;
		}

		// Token: 0x06006BE4 RID: 27620 RVA: 0x00242FD4 File Offset: 0x002411D4
		public static List<Thing> FindEnoughAmmo(Pawn pawn, IntVec3 rootCell, CompReloadable comp, bool forceReload)
		{
			if (comp == null)
			{
				return null;
			}
			IntRange desiredQuantity = new IntRange(comp.MinAmmoNeeded(forceReload), comp.MaxAmmoNeeded(forceReload));
			return RefuelWorkGiverUtility.FindEnoughReservableThings(pawn, rootCell, desiredQuantity, (Thing t) => t.def == comp.AmmoDef);
		}

		// Token: 0x06006BE5 RID: 27621 RVA: 0x0024302B File Offset: 0x0024122B
		public static IEnumerable<Pair<CompReloadable, Thing>> FindPotentiallyReloadableGear(Pawn pawn, List<Thing> potentialAmmo)
		{
			if (pawn.apparel == null)
			{
				yield break;
			}
			List<Apparel> worn = pawn.apparel.WornApparel;
			int num;
			for (int i = 0; i < worn.Count; i = num + 1)
			{
				CompReloadable comp = worn[i].TryGetComp<CompReloadable>();
				CompReloadable compReloadable = comp;
				if (((compReloadable != null) ? compReloadable.AmmoDef : null) != null)
				{
					for (int j = 0; j < potentialAmmo.Count; j = num + 1)
					{
						Thing thing = potentialAmmo[j];
						if (thing.def == comp.Props.ammoDef)
						{
							yield return new Pair<CompReloadable, Thing>(comp, thing);
						}
						num = j;
					}
					comp = null;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06006BE6 RID: 27622 RVA: 0x00243044 File Offset: 0x00241244
		public static Pawn WearerOf(CompReloadable comp)
		{
			Pawn_ApparelTracker pawn_ApparelTracker = comp.ParentHolder as Pawn_ApparelTracker;
			if (pawn_ApparelTracker != null)
			{
				return pawn_ApparelTracker.pawn;
			}
			return null;
		}

		// Token: 0x06006BE7 RID: 27623 RVA: 0x00243068 File Offset: 0x00241268
		public static int TotalChargesFromQueuedJobs(Pawn pawn, ThingWithComps gear)
		{
			CompReloadable compReloadable = gear.TryGetComp<CompReloadable>();
			int num = 0;
			if (compReloadable != null && pawn != null)
			{
				foreach (Job job in pawn.jobs.AllJobs())
				{
					Verb verbToUse = job.verbToUse;
					if (verbToUse != null && compReloadable == verbToUse.ReloadableCompSource)
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x06006BE8 RID: 27624 RVA: 0x002430D8 File Offset: 0x002412D8
		public static bool CanUseConsideringQueuedJobs(Pawn pawn, ThingWithComps gear, bool showMessage = true)
		{
			CompReloadable compReloadable = gear.TryGetComp<CompReloadable>();
			if (compReloadable == null)
			{
				return true;
			}
			string text = null;
			if (!Event.current.shift)
			{
				if (!compReloadable.CanBeUsed)
				{
					text = compReloadable.DisabledReason(compReloadable.MinAmmoNeeded(false), compReloadable.MaxAmmoNeeded(false));
				}
			}
			else if (ReloadableUtility.TotalChargesFromQueuedJobs(pawn, gear) + 1 > compReloadable.RemainingCharges)
			{
				text = compReloadable.DisabledReason(compReloadable.MaxAmmoAmount(), compReloadable.MaxAmmoAmount());
			}
			if (text != null)
			{
				if (showMessage)
				{
					Messages.Message(text, pawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}
	}
}
