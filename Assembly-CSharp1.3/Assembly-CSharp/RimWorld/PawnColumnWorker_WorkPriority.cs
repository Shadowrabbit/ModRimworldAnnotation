using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200139C RID: 5020
	public class PawnColumnWorker_WorkPriority : PawnColumnWorker
	{
		// Token: 0x06007A25 RID: 31269 RVA: 0x002B18F0 File Offset: 0x002AFAF0
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.Dead || pawn.workSettings == null || !pawn.workSettings.EverWork)
			{
				return;
			}
			Text.Font = GameFont.Medium;
			float x = rect.x + (rect.width - 25f) / 2f;
			float y = rect.y + 2.5f;
			bool incapable = this.IsIncapableOfWholeWorkType(pawn, this.def.workType);
			WidgetsWork.DrawWorkBoxFor(x, y, pawn, this.def.workType, incapable);
			Rect rect2 = new Rect(x, y, 25f, 25f);
			if (Mouse.IsOver(rect2))
			{
				TooltipHandler.TipRegion(rect2, () => WidgetsWork.TipForPawnWorker(pawn, this.def.workType, incapable), pawn.thingIDNumber ^ this.def.workType.GetHashCode());
			}
			Text.Font = GameFont.Small;
		}

		// Token: 0x06007A26 RID: 31270 RVA: 0x002B19F8 File Offset: 0x002AFBF8
		public override void DoHeader(Rect rect, PawnTable table)
		{
			base.DoHeader(rect, table);
			Text.Font = GameFont.Small;
			if (this.cachedWorkLabelSize == default(Vector2))
			{
				this.cachedWorkLabelSize = Text.CalcSize(this.def.workType.labelShort);
			}
			Rect labelRect = this.GetLabelRect(rect);
			MouseoverSounds.DoRegion(labelRect);
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(labelRect, this.def.workType.labelShort);
			GUI.color = new Color(1f, 1f, 1f, 0.3f);
			Widgets.DrawLineVertical(labelRect.center.x, labelRect.yMax - 3f, rect.y + 50f - labelRect.yMax + 3f);
			Widgets.DrawLineVertical(labelRect.center.x + 1f, labelRect.yMax - 3f, rect.y + 50f - labelRect.yMax + 3f);
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06007A27 RID: 31271 RVA: 0x002B1B14 File Offset: 0x002AFD14
		public override int GetMinHeaderHeight(PawnTable table)
		{
			return 50;
		}

		// Token: 0x06007A28 RID: 31272 RVA: 0x002B1B18 File Offset: 0x002AFD18
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 32);
		}

		// Token: 0x06007A29 RID: 31273 RVA: 0x002B1B28 File Offset: 0x002AFD28
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(39, this.GetMinWidth(table), this.GetMaxWidth(table));
		}

		// Token: 0x06007A2A RID: 31274 RVA: 0x002B1B3F File Offset: 0x002AFD3F
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), 80);
		}

		// Token: 0x06007A2B RID: 31275 RVA: 0x002B1B50 File Offset: 0x002AFD50
		private bool IsIncapableOfWholeWorkType(Pawn p, WorkTypeDef work)
		{
			for (int i = 0; i < work.workGiversByPriority.Count; i++)
			{
				bool flag = true;
				for (int j = 0; j < work.workGiversByPriority[i].requiredCapacities.Count; j++)
				{
					PawnCapacityDef capacity = work.workGiversByPriority[i].requiredCapacities[j];
					if (!p.health.capacities.CapableOf(capacity))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06007A2C RID: 31276 RVA: 0x002B1BCB File Offset: 0x002AFDCB
		protected override Rect GetInteractableHeaderRect(Rect headerRect, PawnTable table)
		{
			return this.GetLabelRect(headerRect);
		}

		// Token: 0x06007A2D RID: 31277 RVA: 0x002B1BD4 File Offset: 0x002AFDD4
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x06007A2E RID: 31278 RVA: 0x002B1BF8 File Offset: 0x002AFDF8
		private float GetValueToCompare(Pawn pawn)
		{
			if (pawn.workSettings == null || !pawn.workSettings.EverWork)
			{
				return -2f;
			}
			if (pawn.WorkTypeIsDisabled(this.def.workType))
			{
				return -1f;
			}
			return pawn.skills.AverageOfRelevantSkillsFor(this.def.workType);
		}

		// Token: 0x06007A2F RID: 31279 RVA: 0x002B1C50 File Offset: 0x002AFE50
		private Rect GetLabelRect(Rect headerRect)
		{
			float x = headerRect.center.x;
			Rect result = new Rect(x - this.cachedWorkLabelSize.x / 2f, headerRect.y, this.cachedWorkLabelSize.x, this.cachedWorkLabelSize.y);
			if (this.def.moveWorkTypeLabelDown)
			{
				result.y += 20f;
			}
			return result;
		}

		// Token: 0x06007A30 RID: 31280 RVA: 0x002B1CC4 File Offset: 0x002AFEC4
		protected override string GetHeaderTip(PawnTable table)
		{
			string text = string.Concat(new string[]
			{
				this.def.workType.gerundLabel.CapitalizeFirst(),
				"\n\n",
				this.def.workType.description,
				"\n\n",
				PawnColumnWorker_WorkPriority.SpecificWorkListString(this.def.workType)
			});
			text += "\n";
			if (this.def.sortable)
			{
				text += "\n" + "ClickToSortByThisColumn".Translate();
			}
			if (Find.PlaySettings.useWorkPriorities)
			{
				text += "\n" + "WorkPriorityShiftClickTip".Translate();
			}
			else
			{
				text += "\n" + "WorkPriorityShiftClickEnableDisableTip".Translate();
			}
			return text;
		}

		// Token: 0x06007A31 RID: 31281 RVA: 0x002B1DB4 File Offset: 0x002AFFB4
		private static string SpecificWorkListString(WorkTypeDef def)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < def.workGiversByPriority.Count; i++)
			{
				stringBuilder.Append(def.workGiversByPriority[i].LabelCap);
				if (def.workGiversByPriority[i].emergency)
				{
					stringBuilder.Append(" (" + "EmergencyWorkMarker".Translate() + ")");
				}
				if (i < def.workGiversByPriority.Count - 1)
				{
					stringBuilder.AppendLine();
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06007A32 RID: 31282 RVA: 0x002B1E58 File Offset: 0x002B0058
		protected override void HeaderClicked(Rect headerRect, PawnTable table)
		{
			base.HeaderClicked(headerRect, table);
			if (Event.current.shift)
			{
				List<Pawn> pawnsListForReading = table.PawnsListForReading;
				for (int i = 0; i < pawnsListForReading.Count; i++)
				{
					Pawn pawn = pawnsListForReading[i];
					if (pawn.workSettings != null && pawn.workSettings.EverWork && !pawn.WorkTypeIsDisabled(this.def.workType))
					{
						if (Find.PlaySettings.useWorkPriorities)
						{
							int priority = pawn.workSettings.GetPriority(this.def.workType);
							if (Event.current.button == 0 && priority != 1)
							{
								int num = priority - 1;
								if (num < 0)
								{
									num = 4;
								}
								pawn.workSettings.SetPriority(this.def.workType, num);
							}
							if (Event.current.button == 1 && priority != 0)
							{
								int num2 = priority + 1;
								if (num2 > 4)
								{
									num2 = 0;
								}
								pawn.workSettings.SetPriority(this.def.workType, num2);
							}
						}
						else if (pawn.workSettings.GetPriority(this.def.workType) > 0)
						{
							if (Event.current.button == 1)
							{
								pawn.workSettings.SetPriority(this.def.workType, 0);
							}
						}
						else if (Event.current.button == 0)
						{
							pawn.workSettings.SetPriority(this.def.workType, 3);
						}
					}
				}
				if (Find.PlaySettings.useWorkPriorities)
				{
					SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
					return;
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

		// Token: 0x04004395 RID: 17301
		private const int LabelRowHeight = 50;

		// Token: 0x04004396 RID: 17302
		private Vector2 cachedWorkLabelSize;
	}
}
