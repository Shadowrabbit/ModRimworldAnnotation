using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002102 RID: 8450
	public static class CaravanUtility
	{
		// Token: 0x0600B375 RID: 45941 RVA: 0x000749A9 File Offset: 0x00072BA9
		public static bool IsOwner(Pawn pawn, Faction caravanFaction)
		{
			if (caravanFaction == null)
			{
				Log.Warning("Called IsOwner with null faction.", false);
				return false;
			}
			return !pawn.NonHumanlikeOrWildMan() && pawn.Faction == caravanFaction && pawn.HostFaction == null;
		}

		// Token: 0x0600B376 RID: 45942 RVA: 0x000749D7 File Offset: 0x00072BD7
		public static Caravan GetCaravan(this Pawn pawn)
		{
			return pawn.ParentHolder as Caravan;
		}

		// Token: 0x0600B377 RID: 45943 RVA: 0x000749E4 File Offset: 0x00072BE4
		public static bool IsCaravanMember(this Pawn pawn)
		{
			return pawn.GetCaravan() != null;
		}

		// Token: 0x0600B378 RID: 45944 RVA: 0x00340510 File Offset: 0x0033E710
		public static bool IsPlayerControlledCaravanMember(this Pawn pawn)
		{
			Caravan caravan = pawn.GetCaravan();
			return caravan != null && caravan.IsPlayerControlled;
		}

		// Token: 0x0600B379 RID: 45945 RVA: 0x00340530 File Offset: 0x0033E730
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

		// Token: 0x0600B37A RID: 45946 RVA: 0x00340570 File Offset: 0x0033E770
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

		/// <summary>
		/// 远征队里随机选个自己的小人
		/// </summary>
		/// <param name="caravan"></param>
		/// <returns></returns>
		public static Pawn RandomOwner(this Caravan caravan)
		{
			return (from p in caravan.PawnsListForReading
			where caravan.IsOwner(p)
			select p).RandomElement<Pawn>();
		}

		// Token: 0x0600B37C RID: 45948 RVA: 0x000749EF File Offset: 0x00072BEF
		public static bool ShouldAutoCapture(Pawn p, Faction caravanFaction)
		{
			return p.RaceProps.Humanlike && !p.Dead && p.Faction != caravanFaction && (!p.IsPrisoner || p.HostFaction != caravanFaction);
		}
	}
}
