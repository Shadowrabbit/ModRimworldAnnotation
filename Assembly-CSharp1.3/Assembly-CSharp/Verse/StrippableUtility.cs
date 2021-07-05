using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020002EB RID: 747
	public static class StrippableUtility
	{
		// Token: 0x06001540 RID: 5440 RVA: 0x0007B984 File Offset: 0x00079B84
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

		// Token: 0x06001541 RID: 5441 RVA: 0x0007B9E0 File Offset: 0x00079BE0
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
