using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B0C RID: 2828
	public static class PrisonerWillingToJoinQuestUtility
	{
		// Token: 0x06004266 RID: 16998 RVA: 0x00163BA4 File Offset: 0x00161DA4
		public static Pawn GeneratePrisoner(int tile, Faction hostFaction)
		{
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.Slave, hostFaction, PawnGenerationContext.NonPlayer, tile, false, false, false, false, true, false, 75f, true, true, true, true, false, false, true, true, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, null, false, false));
			pawn.guest.SetGuestStatus(hostFaction, GuestStatus.Prisoner);
			return pawn;
		}

		// Token: 0x04002868 RID: 10344
		private const float RelationWithColonistWeight = 75f;
	}
}
