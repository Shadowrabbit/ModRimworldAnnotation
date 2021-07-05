using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BD7 RID: 7127
	public class Instruction_SetGrowingZonePlant : Lesson_Instruction
	{
		// Token: 0x1700189F RID: 6303
		// (get) Token: 0x06009CE4 RID: 40164 RVA: 0x000685EE File Offset: 0x000667EE
		private Zone_Growing GrowZone
		{
			get
			{
				return (Zone_Growing)base.Map.zoneManager.AllZones.FirstOrDefault((Zone z) => z is Zone_Growing);
			}
		}

		// Token: 0x06009CE5 RID: 40165 RVA: 0x00068629 File Offset: 0x00066829
		public override void LessonOnGUI()
		{
			TutorUtility.DrawLabelOnGUI(Gen.AveragePosition(this.GrowZone.cells), this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x06009CE6 RID: 40166 RVA: 0x00068651 File Offset: 0x00066851
		public override void LessonUpdate()
		{
			GenDraw.DrawArrowPointingAt(Gen.AveragePosition(this.GrowZone.cells), false);
		}
	}
}
