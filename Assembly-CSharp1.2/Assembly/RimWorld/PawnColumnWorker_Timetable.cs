using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001B90 RID: 7056
	public class PawnColumnWorker_Timetable : PawnColumnWorker
	{
		// Token: 0x06009B6E RID: 39790 RVA: 0x002D9224 File Offset: 0x002D7424
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

		// Token: 0x06009B6F RID: 39791 RVA: 0x002D92B4 File Offset: 0x002D74B4
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

		// Token: 0x06009B70 RID: 39792 RVA: 0x00067710 File Offset: 0x00065910
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 360);
		}

		// Token: 0x06009B71 RID: 39793 RVA: 0x00067723 File Offset: 0x00065923
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(504, this.GetMinWidth(table), this.GetMaxWidth(table));
		}

		// Token: 0x06009B72 RID: 39794 RVA: 0x0006773D File Offset: 0x0006593D
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), 600);
		}

		// Token: 0x06009B73 RID: 39795 RVA: 0x00067750 File Offset: 0x00065950
		public override int GetMinHeaderHeight(PawnTable table)
		{
			return Mathf.Max(base.GetMinHeaderHeight(table), 15);
		}

		// Token: 0x06009B74 RID: 39796 RVA: 0x002D9328 File Offset: 0x002D7528
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x06009B75 RID: 39797 RVA: 0x00067760 File Offset: 0x00065960
		private int GetValueToCompare(Pawn pawn)
		{
			if (pawn.timetable == null)
			{
				return int.MinValue;
			}
			return pawn.timetable.times.FirstIndexOf((TimeAssignmentDef x) => x == TimeAssignmentDefOf.Work);
		}

		// Token: 0x06009B76 RID: 39798 RVA: 0x002D934C File Offset: 0x002D754C
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
				Widgets.DrawBox(rect, 2);
				if (mouseButton && assignment != TimeAssignmentSelector.selectedAssignment && TimeAssignmentSelector.selectedAssignment != null)
				{
					SoundDefOf.Designate_DragStandard_Changed.PlayOneShotOnCamera(null);
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
