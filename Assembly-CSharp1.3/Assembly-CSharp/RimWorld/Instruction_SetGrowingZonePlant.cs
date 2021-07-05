using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020013D1 RID: 5073
	public class Instruction_SetGrowingZonePlant : Lesson_Instruction
	{
		// Token: 0x17001595 RID: 5525
		// (get) Token: 0x06007B55 RID: 31573 RVA: 0x002B8308 File Offset: 0x002B6508
		private Zone_Growing GrowZone
		{
			get
			{
				return (Zone_Growing)base.Map.zoneManager.AllZones.FirstOrDefault((Zone z) => z is Zone_Growing);
			}
		}

		// Token: 0x06007B56 RID: 31574 RVA: 0x002B8343 File Offset: 0x002B6543
		public override void LessonOnGUI()
		{
			TutorUtility.DrawLabelOnGUI(Gen.AveragePosition(this.GrowZone.cells), this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x06007B57 RID: 31575 RVA: 0x002B836B File Offset: 0x002B656B
		public override void LessonUpdate()
		{
			GenDraw.DrawArrowPointingAt(Gen.AveragePosition(this.GrowZone.cells), false);
		}
	}
}
