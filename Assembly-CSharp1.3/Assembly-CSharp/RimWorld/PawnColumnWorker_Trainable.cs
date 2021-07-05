using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200139B RID: 5019
	public class PawnColumnWorker_Trainable : PawnColumnWorker
	{
		// Token: 0x06007A1B RID: 31259 RVA: 0x002AF436 File Offset: 0x002AD636
		public override void DoHeader(Rect rect, PawnTable table)
		{
			base.DoHeader(rect, table);
			MouseoverSounds.DoRegion(rect);
		}

		// Token: 0x06007A1C RID: 31260 RVA: 0x002B164C File Offset: 0x002AF84C
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.training == null)
			{
				return;
			}
			bool flag;
			AcceptanceReport canTrain = pawn.training.CanAssignToTrain(this.def.trainable, out flag);
			if (!flag || !canTrain.Accepted)
			{
				return;
			}
			int num = (int)((rect.width - 24f) / 2f);
			int num2 = Mathf.Max(3, 0);
			TrainingCardUtility.DoTrainableCheckbox(new Rect(rect.x + (float)num, rect.y + (float)num2, 24f, 24f), pawn, this.def.trainable, canTrain, false, true);
		}

		// Token: 0x06007A1D RID: 31261 RVA: 0x002B0C8F File Offset: 0x002AEE8F
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 24);
		}

		// Token: 0x06007A1E RID: 31262 RVA: 0x002AF523 File Offset: 0x002AD723
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x06007A1F RID: 31263 RVA: 0x002AF538 File Offset: 0x002AD738
		public override int GetMinCellHeight(Pawn pawn)
		{
			return Mathf.Max(base.GetMinCellHeight(pawn), 24);
		}

		// Token: 0x06007A20 RID: 31264 RVA: 0x002B16DC File Offset: 0x002AF8DC
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x06007A21 RID: 31265 RVA: 0x002B1700 File Offset: 0x002AF900
		private int GetValueToCompare(Pawn pawn)
		{
			if (pawn.training == null)
			{
				return int.MinValue;
			}
			if (pawn.training.HasLearned(this.def.trainable))
			{
				return 4;
			}
			bool flag;
			AcceptanceReport acceptanceReport = pawn.training.CanAssignToTrain(this.def.trainable, out flag);
			if (!flag)
			{
				return 0;
			}
			if (!acceptanceReport.Accepted)
			{
				return 1;
			}
			if (!pawn.training.GetWanted(this.def.trainable))
			{
				return 2;
			}
			return 3;
		}

		// Token: 0x06007A22 RID: 31266 RVA: 0x002B177C File Offset: 0x002AF97C
		protected override void HeaderClicked(Rect headerRect, PawnTable table)
		{
			base.HeaderClicked(headerRect, table);
			if (Event.current.shift)
			{
				List<Pawn> pawnsListForReading = table.PawnsListForReading;
				for (int i = 0; i < pawnsListForReading.Count; i++)
				{
					if (pawnsListForReading[i].training != null && !pawnsListForReading[i].training.HasLearned(this.def.trainable))
					{
						bool flag;
						AcceptanceReport acceptanceReport = pawnsListForReading[i].training.CanAssignToTrain(this.def.trainable, out flag);
						if (flag && acceptanceReport.Accepted)
						{
							bool wanted = pawnsListForReading[i].training.GetWanted(this.def.trainable);
							if (Event.current.button == 0)
							{
								if (!wanted)
								{
									pawnsListForReading[i].training.SetWantedRecursive(this.def.trainable, true);
								}
							}
							else if (Event.current.button == 1 && wanted)
							{
								pawnsListForReading[i].training.SetWantedRecursive(this.def.trainable, false);
							}
						}
					}
				}
				if (Event.current.button == 0)
				{
					SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
					return;
				}
				if (Event.current.button == 1)
				{
					SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
				}
			}
		}

		// Token: 0x06007A23 RID: 31267 RVA: 0x002B18C9 File Offset: 0x002AFAC9
		protected override string GetHeaderTip(PawnTable table)
		{
			return base.GetHeaderTip(table) + "\n" + "CheckboxShiftClickTip".Translate();
		}
	}
}
