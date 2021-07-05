using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200137E RID: 4990
	public abstract class PawnColumnWorker_CopyPaste : PawnColumnWorker
	{
		// Token: 0x1700155E RID: 5470
		// (get) Token: 0x06007965 RID: 31077
		protected abstract bool AnythingInClipboard { get; }

		// Token: 0x06007966 RID: 31078 RVA: 0x002AF8B8 File Offset: 0x002ADAB8
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

		// Token: 0x06007967 RID: 31079 RVA: 0x002AF91F File Offset: 0x002ADB1F
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 36);
		}

		// Token: 0x06007968 RID: 31080 RVA: 0x002AF523 File Offset: 0x002AD723
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x06007969 RID: 31081
		protected abstract void CopyFrom(Pawn p);

		// Token: 0x0600796A RID: 31082
		protected abstract void PasteTo(Pawn p);
	}
}
