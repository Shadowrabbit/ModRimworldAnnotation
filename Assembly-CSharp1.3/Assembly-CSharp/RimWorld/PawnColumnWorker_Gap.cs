using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200138C RID: 5004
	public class PawnColumnWorker_Gap : PawnColumnWorker
	{
		// Token: 0x17001563 RID: 5475
		// (get) Token: 0x060079B2 RID: 31154 RVA: 0x002B025E File Offset: 0x002AE45E
		protected virtual int Width
		{
			get
			{
				return this.def.gap;
			}
		}

		// Token: 0x060079B3 RID: 31155 RVA: 0x0000313F File Offset: 0x0000133F
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
		}

		// Token: 0x060079B4 RID: 31156 RVA: 0x002B026B File Offset: 0x002AE46B
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), this.Width);
		}

		// Token: 0x060079B5 RID: 31157 RVA: 0x002B027F File Offset: 0x002AE47F
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.Width);
		}

		// Token: 0x060079B6 RID: 31158 RVA: 0x0001276E File Offset: 0x0001096E
		public override int GetMinCellHeight(Pawn pawn)
		{
			return 0;
		}
	}
}
