using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200138D RID: 5005
	public class PawnColumnWorker_RemainingSpace : PawnColumnWorker
	{
		// Token: 0x060079B8 RID: 31160 RVA: 0x0000313F File Offset: 0x0000133F
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
		}

		// Token: 0x060079B9 RID: 31161 RVA: 0x0001276E File Offset: 0x0001096E
		public override int GetMinWidth(PawnTable table)
		{
			return 0;
		}

		// Token: 0x060079BA RID: 31162 RVA: 0x0015D6DE File Offset: 0x0015B8DE
		public override int GetMaxWidth(PawnTable table)
		{
			return 1000000;
		}

		// Token: 0x060079BB RID: 31163 RVA: 0x002B0293 File Offset: 0x002AE493
		public override int GetOptimalWidth(PawnTable table)
		{
			return this.GetMaxWidth(table);
		}

		// Token: 0x060079BC RID: 31164 RVA: 0x0001276E File Offset: 0x0001096E
		public override int GetMinCellHeight(Pawn pawn)
		{
			return 0;
		}
	}
}
