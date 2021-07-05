using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020013CC RID: 5068
	public class Instruction_FinishConstruction : Lesson_Instruction
	{
		// Token: 0x17001592 RID: 5522
		// (get) Token: 0x06007B37 RID: 31543 RVA: 0x002B7DB0 File Offset: 0x002B5FB0
		protected override float ProgressPercent
		{
			get
			{
				if (this.initialBlueprintsCount < 0)
				{
					this.initialBlueprintsCount = this.ConstructionNeeders().Count<Thing>();
				}
				if (this.initialBlueprintsCount == 0)
				{
					return 1f;
				}
				return 1f - (float)this.ConstructionNeeders().Count<Thing>() / (float)this.initialBlueprintsCount;
			}
		}

		// Token: 0x06007B38 RID: 31544 RVA: 0x002B7E00 File Offset: 0x002B6000
		private IEnumerable<Thing> ConstructionNeeders()
		{
			return from b in base.Map.listerThings.ThingsInGroup(ThingRequestGroup.Blueprint).Concat(base.Map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingFrame))
			where b.Faction == Faction.OfPlayer
			select b;
		}

		// Token: 0x06007B39 RID: 31545 RVA: 0x002B7E5C File Offset: 0x002B605C
		public override void LessonUpdate()
		{
			base.LessonUpdate();
			if (this.ConstructionNeeders().Count<Thing>() < 3)
			{
				foreach (Thing thing in this.ConstructionNeeders())
				{
					GenDraw.DrawArrowPointingAt(thing.DrawPos, false);
				}
			}
			if (this.ProgressPercent > 0.9999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}

		// Token: 0x04004440 RID: 17472
		private int initialBlueprintsCount = -1;
	}
}
