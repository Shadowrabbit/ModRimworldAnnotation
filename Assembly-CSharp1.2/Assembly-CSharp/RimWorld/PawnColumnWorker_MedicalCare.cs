using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001B89 RID: 7049
	public class PawnColumnWorker_MedicalCare : PawnColumnWorker
	{
		// Token: 0x06009B4E RID: 39758 RVA: 0x0006760B File Offset: 0x0006580B
		public override void DoHeader(Rect rect, PawnTable table)
		{
			MouseoverSounds.DoRegion(rect);
			base.DoHeader(rect, table);
		}

		// Token: 0x06009B4F RID: 39759 RVA: 0x00066D55 File Offset: 0x00064F55
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 28);
		}

		// Token: 0x06009B50 RID: 39760 RVA: 0x00066D65 File Offset: 0x00064F65
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x06009B51 RID: 39761 RVA: 0x0006761B File Offset: 0x0006581B
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			MedicalCareUtility.MedicalCareSelectButton(rect, pawn);
		}

		// Token: 0x06009B52 RID: 39762 RVA: 0x00067624 File Offset: 0x00065824
		public override int Compare(Pawn a, Pawn b)
		{
			return a.playerSettings.medCare.CompareTo(b.playerSettings.medCare);
		}
	}
}
