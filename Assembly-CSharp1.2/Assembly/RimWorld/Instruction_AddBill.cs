using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BBF RID: 7103
	public class Instruction_AddBill : Lesson_Instruction
	{
		// Token: 0x1700188A RID: 6282
		// (get) Token: 0x06009C66 RID: 40038 RVA: 0x002DE110 File Offset: 0x002DC310
		protected override float ProgressPercent
		{
			get
			{
				int num = this.def.recipeTargetCount + 1;
				int num2 = 0;
				Bill_Production bill_Production = this.RelevantBill();
				if (bill_Production != null)
				{
					num2++;
					if (bill_Production.repeatMode == BillRepeatModeDefOf.RepeatCount)
					{
						num2 += bill_Production.repeatCount;
					}
				}
				return (float)num2 / (float)num;
			}
		}

		// Token: 0x06009C67 RID: 40039 RVA: 0x002DE158 File Offset: 0x002DC358
		private Bill_Production RelevantBill()
		{
			if (Find.Selector.SingleSelectedThing != null && Find.Selector.SingleSelectedThing.def == this.def.thingDef)
			{
				IBillGiver billGiver = Find.Selector.SingleSelectedThing as IBillGiver;
				if (billGiver != null)
				{
					return (Bill_Production)billGiver.BillStack.Bills.FirstOrDefault((Bill b) => b.recipe == this.def.recipeDef);
				}
			}
			return null;
		}

		// Token: 0x06009C68 RID: 40040 RVA: 0x00067EF8 File Offset: 0x000660F8
		private IEnumerable<Thing> ThingsToSelect()
		{
			if (Find.Selector.SingleSelectedThing == null || Find.Selector.SingleSelectedThing.def != this.def.thingDef)
			{
				foreach (Building building in base.Map.listerBuildings.AllBuildingsColonistOfDef(this.def.thingDef))
				{
					yield return building;
				}
				IEnumerator<Building> enumerator = null;
				yield break;
			}
			yield break;
			yield break;
		}

		// Token: 0x06009C69 RID: 40041 RVA: 0x002DE1C4 File Offset: 0x002DC3C4
		public override void LessonOnGUI()
		{
			foreach (Thing t in this.ThingsToSelect())
			{
				TutorUtility.DrawLabelOnThingOnGUI(t, this.def.onMapInstruction);
			}
			if (this.RelevantBill() == null)
			{
				UIHighlighter.HighlightTag("AddBill");
			}
			base.LessonOnGUI();
		}

		// Token: 0x06009C6A RID: 40042 RVA: 0x002DE234 File Offset: 0x002DC434
		public override void LessonUpdate()
		{
			foreach (Thing thing in this.ThingsToSelect())
			{
				GenDraw.DrawArrowPointingAt(thing.DrawPos, false);
			}
			if (this.ProgressPercent > 0.999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}
