using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001527 RID: 5415
	public static class TimetableUtility
	{
		// Token: 0x0600753F RID: 30015 RVA: 0x0004F165 File Offset: 0x0004D365
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
