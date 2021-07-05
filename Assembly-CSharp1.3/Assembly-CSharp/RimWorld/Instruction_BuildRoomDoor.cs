using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020013C3 RID: 5059
	public class Instruction_BuildRoomDoor : Lesson_Instruction
	{
		// Token: 0x17001587 RID: 5511
		// (get) Token: 0x06007B04 RID: 31492 RVA: 0x002B7230 File Offset: 0x002B5430
		private CellRect RoomRect
		{
			get
			{
				return Find.TutorialState.roomRect;
			}
		}

		// Token: 0x06007B05 RID: 31493 RVA: 0x002B723C File Offset: 0x002B543C
		public override void OnActivated()
		{
			base.OnActivated();
			this.allowedPlaceCells = this.RoomRect.EdgeCells.ToList<IntVec3>();
			this.allowedPlaceCells.RemoveAll((IntVec3 c) => (c.x == this.RoomRect.minX && c.z == this.RoomRect.minZ) || (c.x == this.RoomRect.minX && c.z == this.RoomRect.maxZ) || (c.x == this.RoomRect.maxX && c.z == this.RoomRect.minZ) || (c.x == this.RoomRect.maxX && c.z == this.RoomRect.maxZ));
		}

		// Token: 0x06007B06 RID: 31494 RVA: 0x002B7280 File Offset: 0x002B5480
		public override void LessonOnGUI()
		{
			TutorUtility.DrawCellRectOnGUI(this.RoomRect, this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x06007B07 RID: 31495 RVA: 0x002B72A0 File Offset: 0x002B54A0
		public override void LessonUpdate()
		{
			GenDraw.DrawArrowPointingAt(this.RoomRect.CenterVector3, false);
		}

		// Token: 0x06007B08 RID: 31496 RVA: 0x002B72C1 File Offset: 0x002B54C1
		public override AcceptanceReport AllowAction(EventPack ep)
		{
			if (ep.Tag == "Designate-Door")
			{
				return TutorUtility.EventCellsAreWithin(ep, this.allowedPlaceCells);
			}
			return base.AllowAction(ep);
		}

		// Token: 0x06007B09 RID: 31497 RVA: 0x002B72EF File Offset: 0x002B54EF
		public override void Notify_Event(EventPack ep)
		{
			if (ep.Tag == "Designate-Door")
			{
				Find.ActiveLesson.Deactivate();
			}
		}

		// Token: 0x0400443B RID: 17467
		private List<IntVec3> allowedPlaceCells;
	}
}
