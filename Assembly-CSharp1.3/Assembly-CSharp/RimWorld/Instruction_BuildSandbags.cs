using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020013C5 RID: 5061
	public class Instruction_BuildSandbags : Lesson_Instruction
	{
		// Token: 0x1700158A RID: 5514
		// (get) Token: 0x06007B16 RID: 31510 RVA: 0x002B7574 File Offset: 0x002B5774
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

		// Token: 0x06007B17 RID: 31511 RVA: 0x002B75E4 File Offset: 0x002B57E4
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

		// Token: 0x06007B18 RID: 31512 RVA: 0x002B7778 File Offset: 0x002B5978
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<IntVec3>(ref this.sandbagCells, "sandbagCells", LookMode.Undefined, Array.Empty<object>());
		}

		// Token: 0x06007B19 RID: 31513 RVA: 0x002B7796 File Offset: 0x002B5996
		public override void LessonOnGUI()
		{
			TutorUtility.DrawLabelOnGUI(Gen.AveragePosition(this.sandbagCells), this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x06007B1A RID: 31514 RVA: 0x002B77BC File Offset: 0x002B59BC
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

		// Token: 0x06007B1B RID: 31515 RVA: 0x002B7812 File Offset: 0x002B5A12
		public override AcceptanceReport AllowAction(EventPack ep)
		{
			if (ep.Tag == "Designate-Sandbags")
			{
				return TutorUtility.EventCellsAreWithin(ep, this.sandbagCells);
			}
			return base.AllowAction(ep);
		}

		// Token: 0x0400443D RID: 17469
		private List<IntVec3> sandbagCells;
	}
}
