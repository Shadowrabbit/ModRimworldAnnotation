using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001380 RID: 4992
	public class PawnColumnWorker_CopyPasteWorkPriorities : PawnColumnWorker_CopyPaste
	{
		// Token: 0x17001560 RID: 5472
		// (get) Token: 0x06007971 RID: 31089 RVA: 0x002AF9A2 File Offset: 0x002ADBA2
		protected override bool AnythingInClipboard
		{
			get
			{
				return PawnColumnWorker_CopyPasteWorkPriorities.clipboard != null;
			}
		}

		// Token: 0x06007972 RID: 31090 RVA: 0x002AF9AC File Offset: 0x002ADBAC
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.Dead || pawn.workSettings == null || !pawn.workSettings.EverWork)
			{
				return;
			}
			base.DoCell(rect, pawn, table);
		}

		// Token: 0x06007973 RID: 31091 RVA: 0x002AF9D8 File Offset: 0x002ADBD8
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

		// Token: 0x06007974 RID: 31092 RVA: 0x002AFA38 File Offset: 0x002ADC38
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

		// Token: 0x0400437F RID: 17279
		private static DefMap<WorkTypeDef, int> clipboard;
	}
}
