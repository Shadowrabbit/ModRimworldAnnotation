using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020013C6 RID: 5062
	public class Instruction_ChopWood : Lesson_Instruction
	{
		// Token: 0x1700158B RID: 5515
		// (get) Token: 0x06007B1E RID: 31518 RVA: 0x002B7858 File Offset: 0x002B5A58
		protected override float ProgressPercent
		{
			get
			{
				return (float)(from d in base.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.HarvestPlant)
				where d.target.Thing.def.plant.IsTree
				select d).Count<Designation>() / (float)this.def.targetCount;
			}
		}

		// Token: 0x06007B1F RID: 31519 RVA: 0x002B78B1 File Offset: 0x002B5AB1
		public override void LessonUpdate()
		{
			if (this.ProgressPercent > 0.999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}
