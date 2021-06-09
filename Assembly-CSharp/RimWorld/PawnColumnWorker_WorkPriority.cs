using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001B93 RID: 7059
	public class PawnColumnWorker_WorkPriority : PawnColumnWorker
	{
		// Token: 0x06009B85 RID: 39813 RVA: 0x002D9670 File Offset: 0x002D7870
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

		// Token: 0x06009B86 RID: 39814 RVA: 0x002D9778 File Offset: 0x002D7978
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

		// Token: 0x06009B87 RID: 39815 RVA: 0x000677B5 File Offset: 0x000659B5
		public override int GetMinHeaderHeight(PawnTable table)
		{
			return 50;
		}

		// Token: 0x06009B88 RID: 39816 RVA: 0x000677B9 File Offset: 0x000659B9
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 32);
		}

		// Token: 0x06009B89 RID: 39817 RVA: 0x000677C9 File Offset: 0x000659C9
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(39, this.GetMinWidth(table), this.GetMaxWidth(table));
		}

		// Token: 0x06009B8A RID: 39818 RVA: 0x000677E0 File Offset: 0x000659E0
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), 80);
		}

		// Token: 0x06009B8B RID: 39819 RVA: 0x002D9894 File Offset: 0x002D7A94
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

		// Token: 0x06009B8C RID: 39820 RVA: 0x000677F0 File Offset: 0x000659F0
		protected override Rect GetInteractableHeaderRect(Rect headerRect, PawnTable table)
		{
			return this.GetLabelRect(headerRect);
		}

		// Token: 0x06009B8D RID: 39821 RVA: 0x002D9910 File Offset: 0x002D7B10
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x06009B8E RID: 39822 RVA: 0x002D9934 File Offset: 0x002D7B34
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

		// Token: 0x06009B8F RID: 39823 RVA: 0x002D998C File Offset: 0x002D7B8C
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

		// Token: 0x06009B90 RID: 39824 RVA: 0x002D9A00 File Offset: 0x002D7C00
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

		// Token: 0x06009B91 RID: 39825 RVA: 0x002D9AF0 File Offset: 0x002D7CF0
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

		// Token: 0x06009B92 RID: 39826 RVA: 0x002D9B94 File Offset: 0x002D7D94
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

		// Token: 0x04006307 RID: 25351
		private const int LabelRowHeight = 50;

		// Token: 0x04006308 RID: 25352
		private Vector2 cachedWorkLabelSize;
	}
}
