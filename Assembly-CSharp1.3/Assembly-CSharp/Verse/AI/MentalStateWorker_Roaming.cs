using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005D8 RID: 1496
	public class MentalStateWorker_Roaming : MentalStateWorker
	{
		// Token: 0x06002B4A RID: 11082 RVA: 0x00102E54 File Offset: 0x00101054
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && MentalStateWorker_Roaming.CanRoamNow(pawn);
		}

		// Token: 0x06002B4B RID: 11083 RVA: 0x00102E68 File Offset: 0x00101068
		public static bool CanRoamNow(Pawn pawn)
		{
			return pawn.Spawned && pawn.Map.IsPlayerHome && GenTicks.TicksGame >= 300000 && pawn.RaceProps.Roamer && pawn.Faction == Faction.OfPlayer && !pawn.roping.IsRoped && !pawn.mindState.InRoamingCooldown && pawn.CanReachMapEdge();
		}

		// Token: 0x04001A73 RID: 6771
		public const int GracePeriodSinceGameStartedDays = 5;
	}
}
