using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200103C RID: 4156
	public static class PrisonerWillingToJoinQuestUtility
	{
		// Token: 0x06005A7D RID: 23165 RVA: 0x001D5954 File Offset: 0x001D3B54
		public static Pawn GeneratePrisoner(int tile, Faction hostFaction)
		{
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.Slave, hostFaction, PawnGenerationContext.NonPlayer, tile, false, false, false, false, true, false, 75f, true, true, true, true, false, false, true, true, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null));
			pawn.guest.SetGuestStatus(hostFaction, true);
			return pawn;
		}

		// Token: 0x04003CCD RID: 15565
		private const float RelationWithColonistWeight = 75f;
	}
}
