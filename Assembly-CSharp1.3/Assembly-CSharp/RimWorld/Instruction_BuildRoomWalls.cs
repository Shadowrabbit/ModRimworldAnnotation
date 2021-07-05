using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020013C4 RID: 5060
	public class Instruction_BuildRoomWalls : Lesson_Instruction
	{
		// Token: 0x17001588 RID: 5512
		// (get) Token: 0x06007B0C RID: 31500 RVA: 0x002B7230 File Offset: 0x002B5430
		// (set) Token: 0x06007B0D RID: 31501 RVA: 0x002B73B9 File Offset: 0x002B55B9
		private CellRect RoomRect
		{
			get
			{
				return Find.TutorialState.roomRect;
			}
			set
			{
				Find.TutorialState.roomRect = value;
			}
		}

		// Token: 0x17001589 RID: 5513
		// (get) Token: 0x06007B0E RID: 31502 RVA: 0x002B73C8 File Offset: 0x002B55C8
		protected override float ProgressPercent
		{
			get
			{
				int num = 0;
				int num2 = 0;
				using (IEnumerator<IntVec3> enumerator = this.RoomRect.EdgeCells.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (TutorUtility.BuildingOrBlueprintOrFrameCenterExists(enumerator.Current, base.Map, ThingDefOf.Wall))
						{
							num2++;
						}
						num++;
					}
				}
				return (float)num2 / (float)num;
			}
		}

		// Token: 0x06007B0F RID: 31503 RVA: 0x002B7438 File Offset: 0x002B5638
		public override void OnActivated()
		{
			base.OnActivated();
			this.RoomRect = TutorUtility.FindUsableRect(12, 8, base.Map, 0f, false);
		}

		// Token: 0x06007B10 RID: 31504 RVA: 0x002B745A File Offset: 0x002B565A
		public override void LessonOnGUI()
		{
			TutorUtility.DrawCellRectOnGUI(this.RoomRect, this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x06007B11 RID: 31505 RVA: 0x002B7478 File Offset: 0x002B5678
		public override void LessonUpdate()
		{
			this.cachedEdgeCells.Clear();
			this.cachedEdgeCells.AddRange((from c in this.RoomRect.EdgeCells
			where !TutorUtility.BuildingOrBlueprintOrFrameCenterExists(c, base.Map, ThingDefOf.Wall)
			select c).ToList<IntVec3>());
			GenDraw.DrawFieldEdges((from c in this.cachedEdgeCells
			where c.GetEdifice(base.Map) == null
			select c).ToList<IntVec3>());
			GenDraw.DrawArrowPointingAt(this.RoomRect.CenterVector3, false);
			if (this.ProgressPercent > 0.9999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}

		// Token: 0x06007B12 RID: 31506 RVA: 0x002B750B File Offset: 0x002B570B
		public override AcceptanceReport AllowAction(EventPack ep)
		{
			if (ep.Tag == "Designate-Wall")
			{
				return TutorUtility.EventCellsAreWithin(ep, this.cachedEdgeCells);
			}
			return base.AllowAction(ep);
		}

		// Token: 0x0400443C RID: 17468
		private List<IntVec3> cachedEdgeCells = new List<IntVec3>();
	}
}
