using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E29 RID: 3625
	public class AutoUndrafter : IExposable
	{
		// Token: 0x060053C9 RID: 21449 RVA: 0x001C5D1D File Offset: 0x001C3F1D
		public AutoUndrafter(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060053CA RID: 21450 RVA: 0x001C5D2C File Offset: 0x001C3F2C
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastNonWaitingTick, "lastNonWaitingTick", 0, false);
		}

		// Token: 0x060053CB RID: 21451 RVA: 0x001C5D40 File Offset: 0x001C3F40
		public void AutoUndraftTick()
		{
			if (Find.TickManager.TicksGame % 100 == 0 && this.pawn.Drafted)
			{
				if ((this.pawn.jobs.curJob != null && this.pawn.jobs.curJob.def != JobDefOf.Wait_Combat) || this.AnyHostilePreventingAutoUndraft())
				{
					this.lastNonWaitingTick = Find.TickManager.TicksGame;
				}
				if (this.ShouldAutoUndraft())
				{
					this.pawn.drafter.Drafted = false;
				}
			}
		}

		// Token: 0x060053CC RID: 21452 RVA: 0x001C5DC8 File Offset: 0x001C3FC8
		public void Notify_Drafted()
		{
			this.lastNonWaitingTick = Find.TickManager.TicksGame;
		}

		// Token: 0x060053CD RID: 21453 RVA: 0x001C5DDA File Offset: 0x001C3FDA
		private bool ShouldAutoUndraft()
		{
			return Find.TickManager.TicksGame - this.lastNonWaitingTick >= 10000 && !this.AnyHostilePreventingAutoUndraft();
		}

		// Token: 0x060053CE RID: 21454 RVA: 0x001C5E04 File Offset: 0x001C4004
		private bool AnyHostilePreventingAutoUndraft()
		{
			List<IAttackTarget> potentialTargetsFor = this.pawn.Map.attackTargetsCache.GetPotentialTargetsFor(this.pawn);
			for (int i = 0; i < potentialTargetsFor.Count; i++)
			{
				if (GenHostility.IsActiveThreatToPlayer(potentialTargetsFor[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04003150 RID: 12624
		private Pawn pawn;

		// Token: 0x04003151 RID: 12625
		private int lastNonWaitingTick;

		// Token: 0x04003152 RID: 12626
		private const int UndraftDelay = 10000;
	}
}
