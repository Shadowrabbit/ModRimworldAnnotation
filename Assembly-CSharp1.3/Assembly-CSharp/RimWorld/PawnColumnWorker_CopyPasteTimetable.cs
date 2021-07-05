using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200137F RID: 4991
	public class PawnColumnWorker_CopyPasteTimetable : PawnColumnWorker_CopyPaste
	{
		// Token: 0x1700155F RID: 5471
		// (get) Token: 0x0600796C RID: 31084 RVA: 0x002AF92F File Offset: 0x002ADB2F
		protected override bool AnythingInClipboard
		{
			get
			{
				return PawnColumnWorker_CopyPasteTimetable.clipboard != null;
			}
		}

		// Token: 0x0600796D RID: 31085 RVA: 0x002AF939 File Offset: 0x002ADB39
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.timetable == null)
			{
				return;
			}
			base.DoCell(rect, pawn, table);
		}

		// Token: 0x0600796E RID: 31086 RVA: 0x002AF94D File Offset: 0x002ADB4D
		protected override void CopyFrom(Pawn p)
		{
			PawnColumnWorker_CopyPasteTimetable.clipboard = p.timetable.times.ToList<TimeAssignmentDef>();
		}

		// Token: 0x0600796F RID: 31087 RVA: 0x002AF964 File Offset: 0x002ADB64
		protected override void PasteTo(Pawn p)
		{
			for (int i = 0; i < 24; i++)
			{
				p.timetable.times[i] = PawnColumnWorker_CopyPasteTimetable.clipboard[i];
			}
		}

		// Token: 0x0400437E RID: 17278
		private static List<TimeAssignmentDef> clipboard;
	}
}
