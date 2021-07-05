using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BDB RID: 7131
	public class Instruction_UnforbidStartingResources : Lesson_Instruction
	{
		// Token: 0x170018A1 RID: 6305
		// (get) Token: 0x06009CF2 RID: 40178 RVA: 0x002DF110 File Offset: 0x002DD310
		protected override float ProgressPercent
		{
			get
			{
				return (float)(from it in Find.TutorialState.startingItems
				where !it.IsForbidden(Faction.OfPlayer) || it.Destroyed
				select it).Count<Thing>() / (float)Find.TutorialState.startingItems.Count;
			}
		}

		// Token: 0x06009CF3 RID: 40179 RVA: 0x000686E8 File Offset: 0x000668E8
		private IEnumerable<Thing> NeedUnforbidItems()
		{
			return from it in Find.TutorialState.startingItems
			where it.IsForbidden(Faction.OfPlayer) && !it.Destroyed
			select it;
		}

		// Token: 0x06009CF4 RID: 40180 RVA: 0x00068718 File Offset: 0x00066918
		public override void PostDeactivated()
		{
			base.PostDeactivated();
			Find.TutorialState.startingItems.RemoveAll((Thing it) => !Instruction_EquipWeapons.IsWeapon(it));
		}

		// Token: 0x06009CF5 RID: 40181 RVA: 0x002DF164 File Offset: 0x002DD364
		public override void LessonOnGUI()
		{
			foreach (Thing t in this.NeedUnforbidItems())
			{
				TutorUtility.DrawLabelOnThingOnGUI(t, this.def.onMapInstruction);
			}
			base.LessonOnGUI();
		}

		// Token: 0x06009CF6 RID: 40182 RVA: 0x002DF1C0 File Offset: 0x002DD3C0
		public override void LessonUpdate()
		{
			if (this.ProgressPercent > 0.9999f)
			{
				Find.ActiveLesson.Deactivate();
			}
			foreach (Thing thing in this.NeedUnforbidItems())
			{
				GenDraw.DrawArrowPointingAt(thing.DrawPos, true);
			}
		}
	}
}
