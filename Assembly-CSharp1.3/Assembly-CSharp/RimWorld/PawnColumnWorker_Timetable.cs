using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200139A RID: 5018
	public class PawnColumnWorker_Timetable : PawnColumnWorker
	{
		// Token: 0x06007A11 RID: 31249 RVA: 0x002B13F0 File Offset: 0x002AF5F0
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.timetable == null)
			{
				return;
			}
			float num = rect.x;
			float num2 = rect.width / 24f;
			for (int i = 0; i < 24; i++)
			{
				Rect rect2 = new Rect(num, rect.y, num2, rect.height);
				this.DoTimeAssignment(rect2, pawn, i);
				num += num2;
			}
			GUI.color = Color.white;
			if (TimeAssignmentSelector.selectedAssignment != null)
			{
				UIHighlighter.HighlightOpportunity(rect, "TimeAssignmentTableRow-If" + TimeAssignmentSelector.selectedAssignment.defName + "Selected");
			}
		}

		// Token: 0x06007A12 RID: 31250 RVA: 0x002B1480 File Offset: 0x002AF680
		public override void DoHeader(Rect rect, PawnTable table)
		{
			float num = rect.x;
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.LowerCenter;
			float num2 = rect.width / 24f;
			for (int i = 0; i < 24; i++)
			{
				Widgets.Label(new Rect(num, rect.y, num2, rect.height + 3f), i.ToString());
				num += num2;
			}
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06007A13 RID: 31251 RVA: 0x002B14F3 File Offset: 0x002AF6F3
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 360);
		}

		// Token: 0x06007A14 RID: 31252 RVA: 0x002B1506 File Offset: 0x002AF706
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(504, this.GetMinWidth(table), this.GetMaxWidth(table));
		}

		// Token: 0x06007A15 RID: 31253 RVA: 0x002B1520 File Offset: 0x002AF720
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), 600);
		}

		// Token: 0x06007A16 RID: 31254 RVA: 0x002B1533 File Offset: 0x002AF733
		public override int GetMinHeaderHeight(PawnTable table)
		{
			return Mathf.Max(base.GetMinHeaderHeight(table), 15);
		}

		// Token: 0x06007A17 RID: 31255 RVA: 0x002B1544 File Offset: 0x002AF744
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x06007A18 RID: 31256 RVA: 0x002B1567 File Offset: 0x002AF767
		private int GetValueToCompare(Pawn pawn)
		{
			if (pawn.timetable == null)
			{
				return int.MinValue;
			}
			return pawn.timetable.times.FirstIndexOf((TimeAssignmentDef x) => x == TimeAssignmentDefOf.Work);
		}

		// Token: 0x06007A19 RID: 31257 RVA: 0x002B15A8 File Offset: 0x002AF7A8
		private void DoTimeAssignment(Rect rect, Pawn p, int hour)
		{
			rect = rect.ContractedBy(1f);
			bool mouseButton = Input.GetMouseButton(0);
			TimeAssignmentDef assignment = p.timetable.GetAssignment(hour);
			GUI.DrawTexture(rect, assignment.ColorTexture);
			if (!mouseButton)
			{
				MouseoverSounds.DoRegion(rect);
			}
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawBox(rect, 2, null);
				if (mouseButton && assignment != TimeAssignmentSelector.selectedAssignment && TimeAssignmentSelector.selectedAssignment != null)
				{
					SoundDefOf.Designate_DragStandard_Changed_NoCam.PlayOneShotOnCamera(null);
					p.timetable.SetAssignment(hour, TimeAssignmentSelector.selectedAssignment);
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.TimeAssignments, KnowledgeAmount.SmallInteraction);
					if (TimeAssignmentSelector.selectedAssignment == TimeAssignmentDefOf.Meditate)
					{
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.MeditationSchedule, KnowledgeAmount.Total);
					}
				}
			}
		}
	}
}
