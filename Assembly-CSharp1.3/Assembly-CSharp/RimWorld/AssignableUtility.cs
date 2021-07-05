using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001102 RID: 4354
	public static class AssignableUtility
	{
		// Token: 0x0600688E RID: 26766 RVA: 0x00234E74 File Offset: 0x00233074
		public static Pawn GetAssignedPawn(this Building building)
		{
			CompAssignableToPawn compAssignableToPawn = building.TryGetComp<CompAssignableToPawn>();
			if (compAssignableToPawn == null || !compAssignableToPawn.AssignedPawnsForReading.Any<Pawn>())
			{
				return null;
			}
			return compAssignableToPawn.AssignedPawnsForReading[0];
		}

		// Token: 0x0600688F RID: 26767 RVA: 0x00234EA8 File Offset: 0x002330A8
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
