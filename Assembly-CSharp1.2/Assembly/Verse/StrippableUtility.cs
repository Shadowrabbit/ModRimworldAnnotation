using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200044E RID: 1102
	public static class StrippableUtility
	{
		// Token: 0x06001BA6 RID: 7078 RVA: 0x000ED35C File Offset: 0x000EB55C
		public static bool CanBeStrippedByColony(Thing th)
		{
			IStrippable strippable = th as IStrippable;
			if (strippable == null)
			{
				return false;
			}
			if (!strippable.AnythingToStrip())
			{
				return false;
			}
			Pawn pawn = th as Pawn;
			return pawn == null || (!pawn.IsQuestLodger() && (pawn.Downed || (pawn.IsPrisonerOfColony && pawn.guest.PrisonerIsSecure)));
		}

		// Token: 0x06001BA7 RID: 7079 RVA: 0x000ED3B8 File Offset: 0x000EB5B8
		public static void CheckSendStrippingImpactsGoodwillMessage(Thing th)
		{
			Pawn pawn;
			if ((pawn = (th as Pawn)) != null && !pawn.Dead && pawn.Faction != null && pawn.Faction != Faction.OfPlayer && !pawn.Faction.HostileTo(Faction.OfPlayer) && !pawn.Faction.Hidden)
			{
				Messages.Message("MessageStrippingWillAngerFaction".Translate(pawn.Named("PAWN")), pawn, MessageTypeDefOf.CautionInput, false);
			}
		}
	}
}
