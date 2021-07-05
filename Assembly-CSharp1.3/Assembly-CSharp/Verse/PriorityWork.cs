using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000065 RID: 101
	public class PriorityWork : IExposable
	{
		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000425 RID: 1061 RVA: 0x00015E68 File Offset: 0x00014068
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

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000426 RID: 1062 RVA: 0x00015E98 File Offset: 0x00014098
		public IntVec3 Cell
		{
			get
			{
				return this.prioritizedCell;
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000427 RID: 1063 RVA: 0x00015EA0 File Offset: 0x000140A0
		public WorkGiverDef WorkGiver
		{
			get
			{
				return this.prioritizedWorkGiver;
			}
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x00015EA8 File Offset: 0x000140A8
		public PriorityWork()
		{
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x00015ECB File Offset: 0x000140CB
		public PriorityWork(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x00015EF8 File Offset: 0x000140F8
		public void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.prioritizedCell, "prioritizedCell", default(IntVec3), false);
			Scribe_Defs.Look<WorkGiverDef>(ref this.prioritizedWorkGiver, "prioritizedWorkGiver");
			Scribe_Values.Look<int>(ref this.prioritizeTick, "prioritizeTick", 0, false);
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x00015F41 File Offset: 0x00014141
		public void Set(IntVec3 prioritizedCell, WorkGiverDef prioritizedWorkGiver)
		{
			this.prioritizedCell = prioritizedCell;
			this.prioritizedWorkGiver = prioritizedWorkGiver;
			this.prioritizeTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x00015F61 File Offset: 0x00014161
		public void Clear()
		{
			this.prioritizedCell = IntVec3.Invalid;
			this.prioritizedWorkGiver = null;
			this.prioritizeTick = 0;
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x00015F7C File Offset: 0x0001417C
		public void ClearPrioritizedWorkAndJobQueue()
		{
			this.Clear();
			this.pawn.jobs.ClearQueuedJobs(true);
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x00015F95 File Offset: 0x00014195
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

		// Token: 0x04000141 RID: 321
		private Pawn pawn;

		// Token: 0x04000142 RID: 322
		private IntVec3 prioritizedCell = IntVec3.Invalid;

		// Token: 0x04000143 RID: 323
		private WorkGiverDef prioritizedWorkGiver;

		// Token: 0x04000144 RID: 324
		private int prioritizeTick = Find.TickManager.TicksGame;

		// Token: 0x04000145 RID: 325
		private const int Timeout = 30000;
	}
}
