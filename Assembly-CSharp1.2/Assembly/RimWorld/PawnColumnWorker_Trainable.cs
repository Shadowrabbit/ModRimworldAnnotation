using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001B92 RID: 7058
	public class PawnColumnWorker_Trainable : PawnColumnWorker
	{
		// Token: 0x06009B7B RID: 39803 RVA: 0x00066D45 File Offset: 0x00064F45
		public override void DoHeader(Rect rect, PawnTable table)
		{
			base.DoHeader(rect, table);
			MouseoverSounds.DoRegion(rect);
		}

		// Token: 0x06009B7C RID: 39804 RVA: 0x002D93F0 File Offset: 0x002D75F0
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

		// Token: 0x06009B7D RID: 39805 RVA: 0x000674DB File Offset: 0x000656DB
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 24);
		}

		// Token: 0x06009B7E RID: 39806 RVA: 0x00066D65 File Offset: 0x00064F65
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x06009B7F RID: 39807 RVA: 0x00066D7A File Offset: 0x00064F7A
		public override int GetMinCellHeight(Pawn pawn)
		{
			return Mathf.Max(base.GetMinCellHeight(pawn), 24);
		}

		// Token: 0x06009B80 RID: 39808 RVA: 0x002D9480 File Offset: 0x002D7680
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x06009B81 RID: 39809 RVA: 0x002D94A4 File Offset: 0x002D76A4
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

		// Token: 0x06009B82 RID: 39810 RVA: 0x002D9520 File Offset: 0x002D7720
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

		// Token: 0x06009B83 RID: 39811 RVA: 0x00066DA3 File Offset: 0x00064FA3
		protected override string GetHeaderTip(PawnTable table)
		{
			return base.GetHeaderTip(table) + "\n" + "CheckboxShiftClickTip".Translate();
		}
	}
}
