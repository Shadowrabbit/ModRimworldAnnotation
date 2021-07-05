using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001799 RID: 6041
	public static class AssignableUtility
	{
		// Token: 0x0600857F RID: 34175 RVA: 0x0027637C File Offset: 0x0027457C
		public static Pawn GetAssignedPawn(this Building building)
		{
			CompAssignableToPawn compAssignableToPawn = building.TryGetComp<CompAssignableToPawn>();
			if (compAssignableToPawn == null || !compAssignableToPawn.AssignedPawnsForReading.Any<Pawn>())
			{
				return null;
			}
			return compAssignableToPawn.AssignedPawnsForReading[0];
		}

		// Token: 0x06008580 RID: 34176 RVA: 0x002763B0 File Offset: 0x002745B0
		public static IEnumerable<Pawn> GetAssignedPawns(this Building building)
		{
			CompAssignableToPawn compAssignableToPawn = building.TryGetComp<CompAssignableToPawn>();
			if (compAssignableToPawn == null || !compAssignableToPawn.AssignedPawnsForReading.Any<Pawn>())
			{
				return null;
			}
			return compAssignableToPawn.AssignedPawns;
		}
	}
}
