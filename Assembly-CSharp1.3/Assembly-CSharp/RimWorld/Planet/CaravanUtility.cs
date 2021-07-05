using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017AF RID: 6063
	public static class CaravanUtility
	{
		// Token: 0x06008C90 RID: 35984 RVA: 0x00327A0A File Offset: 0x00325C0A
		public static bool IsOwner(Pawn pawn, Faction caravanFaction)
		{
			if (caravanFaction == null)
			{
				Log.Warning("Called IsOwner with null faction.");
				return false;
			}
			return !pawn.NonHumanlikeOrWildMan() && pawn.Faction == caravanFaction && pawn.HostFaction == null && !pawn.IsSlave;
		}

		// Token: 0x06008C91 RID: 35985 RVA: 0x00327A3F File Offset: 0x00325C3F
		public static Caravan GetCaravan(this Pawn pawn)
		{
			return pawn.ParentHolder as Caravan;
		}

		// Token: 0x06008C92 RID: 35986 RVA: 0x00327A4C File Offset: 0x00325C4C
		public static bool IsCaravanMember(this Pawn pawn)
		{
			return pawn.GetCaravan() != null;
		}

		// Token: 0x06008C93 RID: 35987 RVA: 0x00327A58 File Offset: 0x00325C58
		public static bool IsPlayerControlledCaravanMember(this Pawn pawn)
		{
			Caravan caravan = pawn.GetCaravan();
			return caravan != null && caravan.IsPlayerControlled;
		}

		// Token: 0x06008C94 RID: 35988 RVA: 0x00327A78 File Offset: 0x00325C78
		public static int BestGotoDestNear(int tile, Caravan c)
		{
			Predicate<int> predicate = (int t) => !Find.World.Impassable(t) && c.CanReach(t);
			if (predicate(tile))
			{
				return tile;
			}
			int result;
			GenWorldClosest.TryFindClosestTile(tile, predicate, out result, 50, true);
			return result;
		}

		// Token: 0x06008C95 RID: 35989 RVA: 0x00327AB8 File Offset: 0x00325CB8
		public static bool PlayerHasAnyCaravan()
		{
			List<Caravan> caravans = Find.WorldObjects.Caravans;
			for (int i = 0; i < caravans.Count; i++)
			{
				if (caravans[i].IsPlayerControlled)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008C96 RID: 35990 RVA: 0x00327AF4 File Offset: 0x00325CF4
		public static Pawn RandomOwner(this Caravan caravan)
		{
			return (from p in caravan.PawnsListForReading
			where caravan.IsOwner(p)
			select p).RandomElement<Pawn>();
		}

		// Token: 0x06008C97 RID: 35991 RVA: 0x00327B2F File Offset: 0x00325D2F
		public static bool ShouldAutoCapture(Pawn p, Faction caravanFaction)
		{
			return p.RaceProps.Humanlike && !p.Dead && p.Faction != caravanFaction && (!p.IsPrisoner || p.HostFaction != caravanFaction);
		}
	}
}
