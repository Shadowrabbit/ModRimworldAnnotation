using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BD4 RID: 7124
	public class Instruction_MineSteel : Lesson_Instruction
	{
		// Token: 0x1700189E RID: 6302
		// (get) Token: 0x06009CCE RID: 40142 RVA: 0x002DEEF0 File Offset: 0x002DD0F0
		protected override float ProgressPercent
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.mineCells.Count; i++)
				{
					IntVec3 c = this.mineCells[i];
					if (base.Map.designationManager.DesignationAt(c, DesignationDefOf.Mine) != null || c.GetEdifice(base.Map) == null || c.GetEdifice(base.Map).def != ThingDefOf.MineableSteel)
					{
						num++;
					}
				}
				return (float)num / (float)this.mineCells.Count;
			}
		}

		// Token: 0x06009CCF RID: 40143 RVA: 0x000683E0 File Offset: 0x000665E0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<IntVec3>(ref this.mineCells, "mineCells", LookMode.Undefined, Array.Empty<object>());
		}

		// Token: 0x06009CD0 RID: 40144 RVA: 0x002DEF74 File Offset: 0x002DD174
		public override void OnActivated()
		{
			base.OnActivated();
			CellRect cellRect = TutorUtility.FindUsableRect(10, 10, base.Map, 0f, true);
			new GenStep_ScatterLumpsMineable
			{
				forcedDefToScatter = ThingDefOf.MineableSteel
			}.ForceScatterAt(cellRect.CenterCell, base.Map);
			this.mineCells = new List<IntVec3>();
			foreach (IntVec3 intVec in cellRect)
			{
				Building edifice = intVec.GetEdifice(base.Map);
				if (edifice != null && edifice.def == ThingDefOf.MineableSteel)
				{
					this.mineCells.Add(intVec);
				}
			}
		}

		// Token: 0x06009CD1 RID: 40145 RVA: 0x000683FE File Offset: 0x000665FE
		public override void LessonOnGUI()
		{
			if (!this.mineCells.NullOrEmpty<IntVec3>())
			{
				TutorUtility.DrawLabelOnGUI(Gen.AveragePosition(this.mineCells), this.def.onMapInstruction);
			}
			base.LessonOnGUI();
		}

		// Token: 0x06009CD2 RID: 40146 RVA: 0x0006842E File Offset: 0x0006662E
		public override void LessonUpdate()
		{
			GenDraw.DrawArrowPointingAt(Gen.AveragePosition(this.mineCells), false);
		}

		// Token: 0x06009CD3 RID: 40147 RVA: 0x00068441 File Offset: 0x00066641
		public override AcceptanceReport AllowAction(EventPack ep)
		{
			if (ep.Tag == "Designate-Mine")
			{
				return TutorUtility.EventCellsAreWithin(ep, this.mineCells);
			}
			return base.AllowAction(ep);
		}

		// Token: 0x06009CD4 RID: 40148 RVA: 0x0006846F File Offset: 0x0006666F
		public override void Notify_Event(EventPack ep)
		{
			if (ep.Tag == "Designate-Mine" && this.ProgressPercent > 0.999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}

		// Token: 0x040063DC RID: 25564
		private List<IntVec3> mineCells;
	}
}
