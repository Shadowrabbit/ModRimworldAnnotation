using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E79 RID: 3705
	public static class TimetableUtility
	{
		// Token: 0x060056B5 RID: 22197 RVA: 0x001D6C13 File Offset: 0x001D4E13
		public static TimeAssignmentDef GetTimeAssignment(this Pawn pawn)
		{
			if (pawn.timetable == null)
			{
				return TimeAssignmentDefOf.Anything;
			}
			return pawn.timetable.CurrentAssignment;
		}
	}
}
