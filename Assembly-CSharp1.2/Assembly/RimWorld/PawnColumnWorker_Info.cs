using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B71 RID: 7025
	[StaticConstructorOnStartup]
	public class PawnColumnWorker_Info : PawnColumnWorker
	{
		// Token: 0x06009AD3 RID: 39635 RVA: 0x000670A5 File Offset: 0x000652A5
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			Widgets.InfoCardButtonCentered(rect, pawn);
		}

		// Token: 0x06009AD4 RID: 39636 RVA: 0x000670AF File Offset: 0x000652AF
		public override int GetMinWidth(PawnTable table)
		{
			return 24;
		}

		// Token: 0x06009AD5 RID: 39637 RVA: 0x000670AF File Offset: 0x000652AF
		public override int GetMaxWidth(PawnTable table)
		{
			return 24;
		}
	}
}
