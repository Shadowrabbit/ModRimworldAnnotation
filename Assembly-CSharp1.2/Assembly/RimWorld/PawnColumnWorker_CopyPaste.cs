using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B67 RID: 7015
	public abstract class PawnColumnWorker_CopyPaste : PawnColumnWorker
	{
		// Token: 0x17001863 RID: 6243
		// (get) Token: 0x06009A9D RID: 39581
		protected abstract bool AnythingInClipboard { get; }

		// Token: 0x06009A9E RID: 39582 RVA: 0x002D7B80 File Offset: 0x002D5D80
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			Action pasteAction = null;
			if (this.AnythingInClipboard)
			{
				pasteAction = delegate()
				{
					this.PasteTo(pawn);
				};
			}
			CopyPasteUI.DoCopyPasteButtons(new Rect(rect.x, rect.y, 36f, 30f), delegate
			{
				this.CopyFrom(pawn);
			}, pasteAction);
		}

		// Token: 0x06009A9F RID: 39583 RVA: 0x00066F2E File Offset: 0x0006512E
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 36);
		}

		// Token: 0x06009AA0 RID: 39584 RVA: 0x00066D65 File Offset: 0x00064F65
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x06009AA1 RID: 39585
		protected abstract void CopyFrom(Pawn p);

		// Token: 0x06009AA2 RID: 39586
		protected abstract void PasteTo(Pawn p);
	}
}
