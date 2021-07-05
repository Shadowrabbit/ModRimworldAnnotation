using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001387 RID: 4999
	[StaticConstructorOnStartup]
	public class PawnColumnWorker_Info : PawnColumnWorker
	{
		// Token: 0x0600799C RID: 31132 RVA: 0x002AFF6C File Offset: 0x002AE16C
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			Widgets.InfoCardButtonCentered(rect, pawn);
		}

		// Token: 0x0600799D RID: 31133 RVA: 0x002AFF76 File Offset: 0x002AE176
		public override int GetMinWidth(PawnTable table)
		{
			return 24;
		}

		// Token: 0x0600799E RID: 31134 RVA: 0x002AFF76 File Offset: 0x002AE176
		public override int GetMaxWidth(PawnTable table)
		{
			return 24;
		}
	}
}
