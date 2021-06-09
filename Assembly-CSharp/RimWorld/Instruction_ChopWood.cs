using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BC8 RID: 7112
	public class Instruction_ChopWood : Lesson_Instruction
	{
		// Token: 0x17001895 RID: 6293
		// (get) Token: 0x06009C9F RID: 40095 RVA: 0x002DE9B0 File Offset: 0x002DCBB0
		protected override float ProgressPercent
		{
			get
			{
				return (float)(from d in base.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.HarvestPlant)
				where d.target.Thing.def.plant.IsTree
				select d).Count<Designation>() / (float)this.def.targetCount;
			}
		}

		// Token: 0x06009CA0 RID: 40096 RVA: 0x000681B2 File Offset: 0x000663B2
		public override void LessonUpdate()
		{
			if (this.ProgressPercent > 0.999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}
