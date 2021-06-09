using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B76 RID: 7030
	public class PawnColumnWorker_Gap : PawnColumnWorker
	{
		// Token: 0x17001868 RID: 6248
		// (get) Token: 0x06009AE8 RID: 39656 RVA: 0x000671D5 File Offset: 0x000653D5
		protected virtual int Width
		{
			get
			{
				return this.def.gap;
			}
		}

		// Token: 0x06009AE9 RID: 39657 RVA: 0x00006A05 File Offset: 0x00004C05
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
		}

		// Token: 0x06009AEA RID: 39658 RVA: 0x000671E2 File Offset: 0x000653E2
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), this.Width);
		}

		// Token: 0x06009AEB RID: 39659 RVA: 0x000671F6 File Offset: 0x000653F6
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.Width);
		}

		// Token: 0x06009AEC RID: 39660 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override int GetMinCellHeight(Pawn pawn)
		{
			return 0;
		}
	}
}
