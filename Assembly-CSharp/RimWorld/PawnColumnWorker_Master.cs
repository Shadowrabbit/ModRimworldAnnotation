using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001B88 RID: 7048
	public class PawnColumnWorker_Master : PawnColumnWorker
	{
		// Token: 0x1700186F RID: 6255
		// (get) Token: 0x06009B44 RID: 39748 RVA: 0x0000A2E4 File Offset: 0x000084E4
		protected override GameFont DefaultHeaderFont
		{
			get
			{
				return GameFont.Tiny;
			}
		}

		// Token: 0x06009B45 RID: 39749 RVA: 0x00067546 File Offset: 0x00065746
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 100);
		}

		// Token: 0x06009B46 RID: 39750 RVA: 0x00067556 File Offset: 0x00065756
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(170, this.GetMinWidth(table), this.GetMaxWidth(table));
		}

		// Token: 0x06009B47 RID: 39751 RVA: 0x00066D45 File Offset: 0x00064F45
		public override void DoHeader(Rect rect, PawnTable table)
		{
			base.DoHeader(rect, table);
			MouseoverSounds.DoRegion(rect);
		}

		// Token: 0x06009B48 RID: 39752 RVA: 0x00067570 File Offset: 0x00065770
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (!this.CanAssignMaster(pawn))
			{
				return;
			}
			TrainableUtility.MasterSelectButton(rect.ContractedBy(2f), pawn, true);
		}

		// Token: 0x06009B49 RID: 39753 RVA: 0x002D8C6C File Offset: 0x002D6E6C
		public override int Compare(Pawn a, Pawn b)
		{
			int valueToCompare = this.GetValueToCompare1(a);
			int valueToCompare2 = this.GetValueToCompare1(b);
			if (valueToCompare != valueToCompare2)
			{
				return valueToCompare.CompareTo(valueToCompare2);
			}
			return this.GetValueToCompare2(a).CompareTo(this.GetValueToCompare2(b));
		}

		// Token: 0x06009B4A RID: 39754 RVA: 0x0006758E File Offset: 0x0006578E
		private bool CanAssignMaster(Pawn pawn)
		{
			return pawn.RaceProps.Animal && pawn.Faction == Faction.OfPlayer && pawn.training.HasLearned(TrainableDefOf.Obedience);
		}

		// Token: 0x06009B4B RID: 39755 RVA: 0x000675C1 File Offset: 0x000657C1
		private int GetValueToCompare1(Pawn pawn)
		{
			if (!this.CanAssignMaster(pawn))
			{
				return 0;
			}
			if (pawn.playerSettings.Master == null)
			{
				return 1;
			}
			return 2;
		}

		// Token: 0x06009B4C RID: 39756 RVA: 0x000675DE File Offset: 0x000657DE
		private string GetValueToCompare2(Pawn pawn)
		{
			if (pawn.playerSettings != null && pawn.playerSettings.Master != null)
			{
				return pawn.playerSettings.Master.Label;
			}
			return "";
		}
	}
}
