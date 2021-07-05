using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FC9 RID: 4041
	public class CompRitualEffect_SpawnOnOrganizer : CompRitualEffect_SpawnOnPawn
	{
		// Token: 0x06005F23 RID: 24355 RVA: 0x00208C04 File Offset: 0x00206E04
		protected override Pawn GetPawn(LordJob_Ritual ritual)
		{
			return ritual.Organizer;
		}
	}
}
