using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B6A RID: 7018
	public class PawnColumnWorker_CopyPasteWorkPriorities : PawnColumnWorker_CopyPaste
	{
		// Token: 0x17001865 RID: 6245
		// (get) Token: 0x06009AAC RID: 39596 RVA: 0x00066FA1 File Offset: 0x000651A1
		protected override bool AnythingInClipboard
		{
			get
			{
				return PawnColumnWorker_CopyPasteWorkPriorities.clipboard != null;
			}
		}

		// Token: 0x06009AAD RID: 39597 RVA: 0x00066FAB File Offset: 0x000651AB
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.Dead || pawn.workSettings == null || !pawn.workSettings.EverWork)
			{
				return;
			}
			base.DoCell(rect, pawn, table);
		}

		// Token: 0x06009AAE RID: 39598 RVA: 0x002D7C20 File Offset: 0x002D5E20
		protected override void CopyFrom(Pawn p)
		{
			if (PawnColumnWorker_CopyPasteWorkPriorities.clipboard == null)
			{
				PawnColumnWorker_CopyPasteWorkPriorities.clipboard = new DefMap<WorkTypeDef, int>();
			}
			List<WorkTypeDef> allDefsListForReading = DefDatabase<WorkTypeDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				WorkTypeDef workTypeDef = allDefsListForReading[i];
				PawnColumnWorker_CopyPasteWorkPriorities.clipboard[workTypeDef] = ((!p.WorkTypeIsDisabled(workTypeDef)) ? p.workSettings.GetPriority(workTypeDef) : 3);
			}
		}

		// Token: 0x06009AAF RID: 39599 RVA: 0x002D7C80 File Offset: 0x002D5E80
		protected override void PasteTo(Pawn p)
		{
			List<WorkTypeDef> allDefsListForReading = DefDatabase<WorkTypeDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				WorkTypeDef workTypeDef = allDefsListForReading[i];
				if (!p.WorkTypeIsDisabled(workTypeDef))
				{
					p.workSettings.SetPriority(workTypeDef, PawnColumnWorker_CopyPasteWorkPriorities.clipboard[workTypeDef]);
				}
			}
		}

		// Token: 0x040062C8 RID: 25288
		private static DefMap<WorkTypeDef, int> clipboard;
	}
}
