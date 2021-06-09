using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BC5 RID: 7109
	public class Instruction_BuildRoomDoor : Lesson_Instruction
	{
		// Token: 0x17001891 RID: 6289
		// (get) Token: 0x06009C85 RID: 40069 RVA: 0x00068001 File Offset: 0x00066201
		private CellRect RoomRect
		{
			get
			{
				return Find.TutorialState.roomRect;
			}
		}

		// Token: 0x06009C86 RID: 40070 RVA: 0x002DE53C File Offset: 0x002DC73C
		public override void OnActivated()
		{
			base.OnActivated();
			this.allowedPlaceCells = this.RoomRect.EdgeCells.ToList<IntVec3>();
			this.allowedPlaceCells.RemoveAll((IntVec3 c) => (c.x == this.RoomRect.minX && c.z == this.RoomRect.minZ) || (c.x == this.RoomRect.minX && c.z == this.RoomRect.maxZ) || (c.x == this.RoomRect.maxX && c.z == this.RoomRect.minZ) || (c.x == this.RoomRect.maxX && c.z == this.RoomRect.maxZ));
		}

		// Token: 0x06009C87 RID: 40071 RVA: 0x0006800D File Offset: 0x0006620D
		public override void LessonOnGUI()
		{
			TutorUtility.DrawCellRectOnGUI(this.RoomRect, this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x06009C88 RID: 40072 RVA: 0x002DE580 File Offset: 0x002DC780
		public override void LessonUpdate()
		{
			GenDraw.DrawArrowPointingAt(this.RoomRect.CenterVector3, false);
		}

		// Token: 0x06009C89 RID: 40073 RVA: 0x0006802B File Offset: 0x0006622B
		public override AcceptanceReport AllowAction(EventPack ep)
		{
			if (ep.Tag == "Designate-Door")
			{
				return TutorUtility.EventCellsAreWithin(ep, this.allowedPlaceCells);
			}
			return base.AllowAction(ep);
		}

		// Token: 0x06009C8A RID: 40074 RVA: 0x00068059 File Offset: 0x00066259
		public override void Notify_Event(EventPack ep)
		{
			if (ep.Tag == "Designate-Door")
			{
				Find.ActiveLesson.Deactivate();
			}
		}

		// Token: 0x040063CC RID: 25548
		private List<IntVec3> allowedPlaceCells;
	}
}
