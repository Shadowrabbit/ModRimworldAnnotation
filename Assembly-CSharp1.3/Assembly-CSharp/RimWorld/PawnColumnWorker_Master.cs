using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001397 RID: 5015
	public class PawnColumnWorker_Master : PawnColumnWorker
	{
		// Token: 0x17001566 RID: 5478
		// (get) Token: 0x060079F8 RID: 31224 RVA: 0x0001276E File Offset: 0x0001096E
		protected override GameFont DefaultHeaderFont
		{
			get
			{
				return GameFont.Tiny;
			}
		}

		// Token: 0x060079F9 RID: 31225 RVA: 0x002B0F31 File Offset: 0x002AF131
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 100);
		}

		// Token: 0x060079FA RID: 31226 RVA: 0x002B0F41 File Offset: 0x002AF141
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(170, this.GetMinWidth(table), this.GetMaxWidth(table));
		}

		// Token: 0x060079FB RID: 31227 RVA: 0x002AF436 File Offset: 0x002AD636
		public override void DoHeader(Rect rect, PawnTable table)
		{
			base.DoHeader(rect, table);
			MouseoverSounds.DoRegion(rect);
		}

		// Token: 0x060079FC RID: 31228 RVA: 0x002B0F5B File Offset: 0x002AF15B
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (!this.CanAssignMaster(pawn))
			{
				return;
			}
			TrainableUtility.MasterSelectButton(rect.ContractedBy(2f), pawn, true);
		}

		// Token: 0x060079FD RID: 31229 RVA: 0x002B0F7C File Offset: 0x002AF17C
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

		// Token: 0x060079FE RID: 31230 RVA: 0x002B0FB9 File Offset: 0x002AF1B9
		private bool CanAssignMaster(Pawn pawn)
		{
			return pawn.RaceProps.Animal && pawn.Faction == Faction.OfPlayer && pawn.training.HasLearned(TrainableDefOf.Obedience);
		}

		// Token: 0x060079FF RID: 31231 RVA: 0x002B0FEC File Offset: 0x002AF1EC
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

		// Token: 0x06007A00 RID: 31232 RVA: 0x002B1009 File Offset: 0x002AF209
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
