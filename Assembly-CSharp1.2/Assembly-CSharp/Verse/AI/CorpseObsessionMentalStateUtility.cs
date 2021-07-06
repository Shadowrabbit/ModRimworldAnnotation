﻿using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A11 RID: 2577
	public static class CorpseObsessionMentalStateUtility
	{
		// Token: 0x06003D9B RID: 15771 RVA: 0x00175E08 File Offset: 0x00174008
		public static Corpse GetClosestCorpseToDigUp(Pawn pawn)
		{
			if (!pawn.Spawned)
			{
				return null;
			}
			Building_Grave building_Grave = (Building_Grave)GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Grave), PathEndMode.InteractionCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, delegate(Thing x)
			{
				Building_Grave building_Grave2 = (Building_Grave)x;
				return building_Grave2.HasCorpse && CorpseObsessionMentalStateUtility.IsCorpseValid(building_Grave2.Corpse, pawn, true);
			}, null, 0, -1, false, RegionType.Set_Passable, false);
			if (building_Grave == null)
			{
				return null;
			}
			return building_Grave.Corpse;
		}

		// Token: 0x06003D9C RID: 15772 RVA: 0x00175E8C File Offset: 0x0017408C
		public static bool IsCorpseValid(Corpse corpse, Pawn pawn, bool ignoreReachability = false)
		{
			if (corpse == null || corpse.Destroyed || !corpse.InnerPawn.RaceProps.Humanlike)
			{
				return false;
			}
			if (pawn.carryTracker.CarriedThing == corpse)
			{
				return true;
			}
			if (corpse.Spawned)
			{
				return pawn.CanReserve(corpse, 1, -1, null, false) && (ignoreReachability || pawn.CanReach(corpse, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn));
			}
			Building_Grave building_Grave = corpse.ParentHolder as Building_Grave;
			return building_Grave != null && building_Grave.Spawned && pawn.CanReserve(building_Grave, 1, -1, null, false) && (ignoreReachability || pawn.CanReach(building_Grave, PathEndMode.InteractionCell, Danger.Deadly, false, TraverseMode.ByPawn));
		}
	}
}
