using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020014C4 RID: 5316
	public class AutoUndrafter : IExposable
	{
		// Token: 0x0600727C RID: 29308 RVA: 0x0004CF9F File Offset: 0x0004B19F
		public AutoUndrafter(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600727D RID: 29309 RVA: 0x0004CFAE File Offset: 0x0004B1AE
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastNonWaitingTick, "lastNonWaitingTick", 0, false);
		}

		// Token: 0x0600727E RID: 29310 RVA: 0x0022FCCC File Offset: 0x0022DECC
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

		// Token: 0x0600727F RID: 29311 RVA: 0x0004CFC2 File Offset: 0x0004B1C2
		public void Notify_Drafted()
		{
			this.lastNonWaitingTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06007280 RID: 29312 RVA: 0x0004CFD4 File Offset: 0x0004B1D4
		private bool ShouldAutoUndraft()
		{
			return Find.TickManager.TicksGame - this.lastNonWaitingTick >= 10000 && !this.AnyHostilePreventingAutoUndraft();
		}

		// Token: 0x06007281 RID: 29313 RVA: 0x0022FD54 File Offset: 0x0022DF54
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

		// Token: 0x04004B68 RID: 19304
		private Pawn pawn;

		// Token: 0x04004B69 RID: 19305
		private int lastNonWaitingTick;

		// Token: 0x04004B6A RID: 19306
		private const int UndraftDelay = 10000;
	}
}
