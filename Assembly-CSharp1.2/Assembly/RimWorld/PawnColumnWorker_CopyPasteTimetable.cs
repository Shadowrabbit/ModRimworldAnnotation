using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B69 RID: 7017
	public class PawnColumnWorker_CopyPasteTimetable : PawnColumnWorker_CopyPaste
	{
		// Token: 0x17001864 RID: 6244
		// (get) Token: 0x06009AA7 RID: 39591 RVA: 0x00066F64 File Offset: 0x00065164
		protected override bool AnythingInClipboard
		{
			get
			{
				return PawnColumnWorker_CopyPasteTimetable.clipboard != null;
			}
		}

		// Token: 0x06009AA8 RID: 39592 RVA: 0x00066F6E File Offset: 0x0006516E
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.timetable == null)
			{
				return;
			}
			base.DoCell(rect, pawn, table);
		}

		// Token: 0x06009AA9 RID: 39593 RVA: 0x00066F82 File Offset: 0x00065182
		protected override void CopyFrom(Pawn p)
		{
			PawnColumnWorker_CopyPasteTimetable.clipboard = p.timetable.times.ToList<TimeAssignmentDef>();
		}

		// Token: 0x06009AAA RID: 39594 RVA: 0x002D7BE8 File Offset: 0x002D5DE8
		protected override void PasteTo(Pawn p)
		{
			for (int i = 0; i < 24; i++)
			{
				p.timetable.times[i] = PawnColumnWorker_CopyPasteTimetable.clipboard[i];
			}
		}

		// Token: 0x040062C7 RID: 25287
		private static List<TimeAssignmentDef> clipboard;
	}
}
