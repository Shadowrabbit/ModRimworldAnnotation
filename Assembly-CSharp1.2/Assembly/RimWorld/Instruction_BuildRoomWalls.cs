using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BC6 RID: 7110
	public class Instruction_BuildRoomWalls : Lesson_Instruction
	{
		// Token: 0x17001892 RID: 6290
		// (get) Token: 0x06009C8D RID: 40077 RVA: 0x00068001 File Offset: 0x00066201
		// (set) Token: 0x06009C8E RID: 40078 RVA: 0x00068078 File Offset: 0x00066278
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

		// Token: 0x17001893 RID: 6291
		// (get) Token: 0x06009C8F RID: 40079 RVA: 0x002DE650 File Offset: 0x002DC850
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

		// Token: 0x06009C90 RID: 40080 RVA: 0x00068085 File Offset: 0x00066285
		public override void OnActivated()
		{
			base.OnActivated();
			this.RoomRect = TutorUtility.FindUsableRect(12, 8, base.Map, 0f, false);
		}

		// Token: 0x06009C91 RID: 40081 RVA: 0x000680A7 File Offset: 0x000662A7
		public override void LessonOnGUI()
		{
			TutorUtility.DrawCellRectOnGUI(this.RoomRect, this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x06009C92 RID: 40082 RVA: 0x002DE6C0 File Offset: 0x002DC8C0
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

		// Token: 0x06009C93 RID: 40083 RVA: 0x000680C5 File Offset: 0x000662C5
		public override AcceptanceReport AllowAction(EventPack ep)
		{
			if (ep.Tag == "Designate-Wall")
			{
				return TutorUtility.EventCellsAreWithin(ep, this.cachedEdgeCells);
			}
			return base.AllowAction(ep);
		}

		// Token: 0x040063CD RID: 25549
		private List<IntVec3> cachedEdgeCells = new List<IntVec3>();
	}
}
