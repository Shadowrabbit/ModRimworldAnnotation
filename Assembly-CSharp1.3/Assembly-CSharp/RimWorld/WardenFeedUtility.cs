using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000830 RID: 2096
	public static class WardenFeedUtility
	{
		// Token: 0x06003792 RID: 14226 RVA: 0x0013953E File Offset: 0x0013773E
		public static bool ShouldBeFed(Pawn p)
		{
			return p.IsPrisonerOfColony && p.InBed() && p.guest.CanBeBroughtFood && HealthAIUtility.ShouldSeekMedicalRest(p);
		}
	}
}
