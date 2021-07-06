using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D63 RID: 3427
	public static class WardenFeedUtility
	{
		// Token: 0x06004E46 RID: 20038 RVA: 0x000373E7 File Offset: 0x000355E7
		public static bool ShouldBeFed(Pawn p)
		{
			return p.IsPrisonerOfColony && p.InBed() && p.guest.CanBeBroughtFood && HealthAIUtility.ShouldSeekMedicalRest(p);
		}
	}
}
