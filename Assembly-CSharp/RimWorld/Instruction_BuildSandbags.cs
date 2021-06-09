using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BC7 RID: 7111
	public class Instruction_BuildSandbags : Lesson_Instruction
	{
		// Token: 0x17001894 RID: 6292
		// (get) Token: 0x06009C97 RID: 40087 RVA: 0x002DE754 File Offset: 0x002DC954
		protected override float ProgressPercent
		{
			get
			{
				int num = 0;
				int num2 = 0;
				using (List<IntVec3>.Enumerator enumerator = this.sandbagCells.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (TutorUtility.BuildingOrBlueprintOrFrameCenterExists(enumerator.Current, base.Map, ThingDefOf.Sandbags))
						{
							num2++;
						}
						num++;
					}
				}
				return (float)num2 / (float)num;
			}
		}

		// Token: 0x06009C98 RID: 40088 RVA: 0x002DE7C4 File Offset: 0x002DC9C4
		public override void OnActivated()
		{
			base.OnActivated();
			Find.TutorialState.sandbagsRect = TutorUtility.FindUsableRect(7, 7, base.Map, 0f, false);
			this.sandbagCells = new List<IntVec3>();
			foreach (IntVec3 intVec in Find.TutorialState.sandbagsRect.EdgeCells)
			{
				if (intVec.x != Find.TutorialState.sandbagsRect.CenterCell.x && intVec.z != Find.TutorialState.sandbagsRect.CenterCell.z)
				{
					this.sandbagCells.Add(intVec);
				}
			}
			foreach (IntVec3 c in Find.TutorialState.sandbagsRect.ContractedBy(1))
			{
				if (!Find.TutorialState.sandbagsRect.ContractedBy(2).Contains(c))
				{
					List<Thing> thingList = c.GetThingList(base.Map);
					for (int i = thingList.Count - 1; i >= 0; i--)
					{
						Thing thing = thingList[i];
						if (thing.def.passability != Traversability.Standable && (thing.def.category == ThingCategory.Plant || thing.def.category == ThingCategory.Item))
						{
							thing.Destroy(DestroyMode.Vanish);
						}
					}
				}
			}
		}

		// Token: 0x06009C99 RID: 40089 RVA: 0x0006812D File Offset: 0x0006632D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<IntVec3>(ref this.sandbagCells, "sandbagCells", LookMode.Undefined, Array.Empty<object>());
		}

		// Token: 0x06009C9A RID: 40090 RVA: 0x0006814B File Offset: 0x0006634B
		public override void LessonOnGUI()
		{
			TutorUtility.DrawLabelOnGUI(Gen.AveragePosition(this.sandbagCells), this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x06009C9B RID: 40091 RVA: 0x002DE958 File Offset: 0x002DCB58
		public override void LessonUpdate()
		{
			GenDraw.DrawFieldEdges((from c in this.sandbagCells
			where !TutorUtility.BuildingOrBlueprintOrFrameCenterExists(c, base.Map, ThingDefOf.Sandbags)
			select c).ToList<IntVec3>());
			GenDraw.DrawArrowPointingAt(Gen.AveragePosition(this.sandbagCells), false);
			if (this.ProgressPercent > 0.9999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}

		// Token: 0x06009C9C RID: 40092 RVA: 0x0006816E File Offset: 0x0006636E
		public override AcceptanceReport AllowAction(EventPack ep)
		{
			if (ep.Tag == "Designate-Sandbags")
			{
				return TutorUtility.EventCellsAreWithin(ep, this.sandbagCells);
			}
			return base.AllowAction(ep);
		}

		// Token: 0x040063CE RID: 25550
		private List<IntVec3> sandbagCells;
	}
}
