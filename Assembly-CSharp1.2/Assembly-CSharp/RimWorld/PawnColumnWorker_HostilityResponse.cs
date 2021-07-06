using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B86 RID: 7046
	public class PawnColumnWorker_HostilityResponse : PawnColumnWorker
	{
		// Token: 0x06009B38 RID: 39736 RVA: 0x000674A9 File Offset: 0x000656A9
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (!pawn.RaceProps.Humanlike)
			{
				return;
			}
			HostilityResponseModeUtility.DrawResponseButton(rect, pawn, true);
		}

		// Token: 0x06009B39 RID: 39737 RVA: 0x000674C1 File Offset: 0x000656C1
		public override int GetMinCellHeight(Pawn pawn)
		{
			return Mathf.Max(base.GetMinCellHeight(pawn), Mathf.CeilToInt(24f) + 3);
		}

		// Token: 0x06009B3A RID: 39738 RVA: 0x000674DB File Offset: 0x000656DB
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 24);
		}

		// Token: 0x06009B3B RID: 39739 RVA: 0x00066D65 File Offset: 0x00064F65
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x06009B3C RID: 39740 RVA: 0x002D8A4C File Offset: 0x002D6C4C
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x06009B3D RID: 39741 RVA: 0x000674EB File Offset: 0x000656EB
		private int GetValueToCompare(Pawn pawn)
		{
			if (pawn.playerSettings == null)
			{
				return int.MinValue;
			}
			return (int)pawn.playerSettings.hostilityResponse;
		}

		// Token: 0x040062F1 RID: 25329
		private const int TopPadding = 3;

		// Token: 0x040062F2 RID: 25330
		private const int Width = 24;
	}
}
