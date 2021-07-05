using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001398 RID: 5016
	public class PawnColumnWorker_MedicalCare : PawnColumnWorker
	{
		// Token: 0x06007A02 RID: 31234 RVA: 0x002B1036 File Offset: 0x002AF236
		public override void DoHeader(Rect rect, PawnTable table)
		{
			MouseoverSounds.DoRegion(rect);
			base.DoHeader(rect, table);
		}

		// Token: 0x06007A03 RID: 31235 RVA: 0x002AF513 File Offset: 0x002AD713
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 28);
		}

		// Token: 0x06007A04 RID: 31236 RVA: 0x002AF523 File Offset: 0x002AD723
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x06007A05 RID: 31237 RVA: 0x002B1046 File Offset: 0x002AF246
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			MedicalCareUtility.MedicalCareSelectButton(rect, pawn);
		}

		// Token: 0x06007A06 RID: 31238 RVA: 0x002B104F File Offset: 0x002AF24F
		public override int Compare(Pawn a, Pawn b)
		{
			return a.playerSettings.medCare.CompareTo(b.playerSettings.medCare);
		}
	}
}
