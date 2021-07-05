using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001395 RID: 5013
	public class PawnColumnWorker_HostilityResponse : PawnColumnWorker
	{
		// Token: 0x060079EC RID: 31212 RVA: 0x002B0C5D File Offset: 0x002AEE5D
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (!pawn.RaceProps.Humanlike)
			{
				return;
			}
			HostilityResponseModeUtility.DrawResponseButton(rect, pawn, true);
		}

		// Token: 0x060079ED RID: 31213 RVA: 0x002B0C75 File Offset: 0x002AEE75
		public override int GetMinCellHeight(Pawn pawn)
		{
			return Mathf.Max(base.GetMinCellHeight(pawn), Mathf.CeilToInt(24f) + 3);
		}

		// Token: 0x060079EE RID: 31214 RVA: 0x002B0C8F File Offset: 0x002AEE8F
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 24);
		}

		// Token: 0x060079EF RID: 31215 RVA: 0x002AF523 File Offset: 0x002AD723
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x060079F0 RID: 31216 RVA: 0x002B0CA0 File Offset: 0x002AEEA0
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x060079F1 RID: 31217 RVA: 0x002B0CC3 File Offset: 0x002AEEC3
		private int GetValueToCompare(Pawn pawn)
		{
			if (pawn.playerSettings == null)
			{
				return int.MinValue;
			}
			return (int)pawn.playerSettings.hostilityResponse;
		}

		// Token: 0x0400438E RID: 17294
		private const int TopPadding = 3;

		// Token: 0x0400438F RID: 17295
		private const int Width = 24;
	}
}
