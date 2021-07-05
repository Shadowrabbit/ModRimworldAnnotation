using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020013CE RID: 5070
	public class Instruction_MineSteel : Lesson_Instruction
	{
		// Token: 0x17001594 RID: 5524
		// (get) Token: 0x06007B3F RID: 31551 RVA: 0x002B7F3C File Offset: 0x002B613C
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

		// Token: 0x06007B40 RID: 31552 RVA: 0x002B7FBF File Offset: 0x002B61BF
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<IntVec3>(ref this.mineCells, "mineCells", LookMode.Undefined, Array.Empty<object>());
		}

		// Token: 0x06007B41 RID: 31553 RVA: 0x002B7FE0 File Offset: 0x002B61E0
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

		// Token: 0x06007B42 RID: 31554 RVA: 0x002B809C File Offset: 0x002B629C
		public override void LessonOnGUI()
		{
			if (!this.mineCells.NullOrEmpty<IntVec3>())
			{
				TutorUtility.DrawLabelOnGUI(Gen.AveragePosition(this.mineCells), this.def.onMapInstruction);
			}
			base.LessonOnGUI();
		}

		// Token: 0x06007B43 RID: 31555 RVA: 0x002B80CC File Offset: 0x002B62CC
		public override void LessonUpdate()
		{
			GenDraw.DrawArrowPointingAt(Gen.AveragePosition(this.mineCells), false);
		}

		// Token: 0x06007B44 RID: 31556 RVA: 0x002B80DF File Offset: 0x002B62DF
		public override AcceptanceReport AllowAction(EventPack ep)
		{
			if (ep.Tag == "Designate-Mine")
			{
				return TutorUtility.EventCellsAreWithin(ep, this.mineCells);
			}
			return base.AllowAction(ep);
		}

		// Token: 0x06007B45 RID: 31557 RVA: 0x002B810D File Offset: 0x002B630D
		public override void Notify_Event(EventPack ep)
		{
			if (ep.Tag == "Designate-Mine" && this.ProgressPercent > 0.999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}

		// Token: 0x04004441 RID: 17473
		private List<IntVec3> mineCells;
	}
}
