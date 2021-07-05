using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005BE RID: 1470
	public static class CorpseObsessionMentalStateUtility
	{
		// Token: 0x06002AE7 RID: 10983 RVA: 0x0010125C File Offset: 0x000FF45C
		public static Corpse GetClosestCorpseToDigUp(Pawn pawn)
		{
			if (!pawn.Spawned)
			{
				return null;
			}
			Building_Grave building_Grave = (Building_Grave)GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Grave), PathEndMode.InteractionCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false), 9999f, delegate(Thing x)
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

		// Token: 0x06002AE8 RID: 10984 RVA: 0x001012E4 File Offset: 0x000FF4E4
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
				return pawn.CanReserve(corpse, 1, -1, null, false) && (ignoreReachability || pawn.CanReach(corpse, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn));
			}
			Building_Grave building_Grave = corpse.ParentHolder as Building_Grave;
			return building_Grave != null && building_Grave.Spawned && pawn.CanReserve(building_Grave, 1, -1, null, false) && (ignoreReachability || pawn.CanReach(building_Grave, PathEndMode.InteractionCell, Danger.Deadly, false, false, TraverseMode.ByPawn));
		}
	}
}
