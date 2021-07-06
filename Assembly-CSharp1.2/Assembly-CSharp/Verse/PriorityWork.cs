using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x020000AE RID: 174
	public class PriorityWork : IExposable
	{
		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000586 RID: 1414 RVA: 0x0000AAE6 File Offset: 0x00008CE6
		public bool IsPrioritized
		{
			get
			{
				if (this.prioritizedCell.IsValid)
				{
					if (Find.TickManager.TicksGame < this.prioritizeTick + 30000)
					{
						return true;
					}
					this.Clear();
				}
				return false;
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000587 RID: 1415 RVA: 0x0000AB16 File Offset: 0x00008D16
		public IntVec3 Cell
		{
			get
			{
				return this.prioritizedCell;
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000588 RID: 1416 RVA: 0x0000AB1E File Offset: 0x00008D1E
		public WorkGiverDef WorkGiver
		{
			get
			{
				return this.prioritizedWorkGiver;
			}
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x0000AB26 File Offset: 0x00008D26
		public PriorityWork()
		{
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x0000AB49 File Offset: 0x00008D49
		public PriorityWork(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x0008BD04 File Offset: 0x00089F04
		public void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.prioritizedCell, "prioritizedCell", default(IntVec3), false);
			Scribe_Defs.Look<WorkGiverDef>(ref this.prioritizedWorkGiver, "prioritizedWorkGiver");
			Scribe_Values.Look<int>(ref this.prioritizeTick, "prioritizeTick", 0, false);
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x0000AB73 File Offset: 0x00008D73
		public void Set(IntVec3 prioritizedCell, WorkGiverDef prioritizedWorkGiver)
		{
			this.prioritizedCell = prioritizedCell;
			this.prioritizedWorkGiver = prioritizedWorkGiver;
			this.prioritizeTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x0000AB93 File Offset: 0x00008D93
		public void Clear()
		{
			this.prioritizedCell = IntVec3.Invalid;
			this.prioritizedWorkGiver = null;
			this.prioritizeTick = 0;
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x0000ABAE File Offset: 0x00008DAE
		public void ClearPrioritizedWorkAndJobQueue()
		{
			this.Clear();
			this.pawn.jobs.ClearQueuedJobs(true);
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x0000ABC7 File Offset: 0x00008DC7
		public IEnumerable<Gizmo> GetGizmos()
		{
			if ((this.IsPrioritized || (this.pawn.CurJob != null && this.pawn.CurJob.playerForced && this.pawn.CurJob.def.playerInterruptible) || this.pawn.jobs.jobQueue.AnyPlayerForced) && !this.pawn.Drafted)
			{
				yield return new Command_Action
				{
					defaultLabel = "CommandClearPrioritizedWork".Translate(),
					defaultDesc = "CommandClearPrioritizedWorkDesc".Translate(),
					icon = TexCommand.ClearPrioritizedWork,
					activateSound = SoundDefOf.Tick_Low,
					action = delegate()
					{
						this.ClearPrioritizedWorkAndJobQueue();
						if (this.pawn.CurJob.playerForced)
						{
							this.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
						}
					},
					hotKey = KeyBindingDefOf.Designator_Cancel,
					groupKey = 6165612
				};
			}
			yield break;
		}

		// Token: 0x040002B2 RID: 690
		private Pawn pawn;

		// Token: 0x040002B3 RID: 691
		private IntVec3 prioritizedCell = IntVec3.Invalid;

		// Token: 0x040002B4 RID: 692
		private WorkGiverDef prioritizedWorkGiver;

		// Token: 0x040002B5 RID: 693
		private int prioritizeTick = Find.TickManager.TicksGame;

		// Token: 0x040002B6 RID: 694
		private const int Timeout = 30000;
	}
}
