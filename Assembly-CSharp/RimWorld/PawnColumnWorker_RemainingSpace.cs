using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B77 RID: 7031
	public class PawnColumnWorker_RemainingSpace : PawnColumnWorker
	{
		// Token: 0x06009AEE RID: 39662 RVA: 0x00006A05 File Offset: 0x00004C05
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
		}

		// Token: 0x06009AEF RID: 39663 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override int GetMinWidth(PawnTable table)
		{
			return 0;
		}

		// Token: 0x06009AF0 RID: 39664 RVA: 0x0003D325 File Offset: 0x0003B525
		public override int GetMaxWidth(PawnTable table)
		{
			return 1000000;
		}

		// Token: 0x06009AF1 RID: 39665 RVA: 0x0006720A File Offset: 0x0006540A
		public override int GetOptimalWidth(PawnTable table)
		{
			return this.GetMaxWidth(table);
		}

		// Token: 0x06009AF2 RID: 39666 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override int GetMinCellHeight(Pawn pawn)
		{
			return 0;
		}
	}
}
