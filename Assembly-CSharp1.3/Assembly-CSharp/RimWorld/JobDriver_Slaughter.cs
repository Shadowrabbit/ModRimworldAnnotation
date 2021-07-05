using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006CE RID: 1742
	public class JobDriver_Slaughter : JobDriver
	{
		// Token: 0x1700090B RID: 2315
		// (get) Token: 0x06003092 RID: 12434 RVA: 0x0011D55B File Offset: 0x0011B75B
		protected Pawn Victim
		{
			get
			{
				return (Pawn)this.job.targetA.Thing;
			}
		}

		// Token: 0x06003093 RID: 12435 RVA: 0x0011E052 File Offset: 0x0011C252
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Victim, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06003094 RID: 12436 RVA: 0x0011E074 File Offset: 0x0011C274
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnAggroMentalState(TargetIndex.A);
			this.FailOn(() => !this.job.ignoreDesignations && !this.Victim.ShouldBeSlaughtered());
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_General.WaitWith(TargetIndex.A, 180, true, false);
			yield return Toils_General.Do(delegate
			{
				ExecutionUtility.DoExecutionByCut(this.pawn, this.Victim, 8, true);
				this.pawn.records.Increment(RecordDefOf.AnimalsSlaughtered);
				if (this.pawn.InMentalState)
				{
					this.pawn.MentalState.Notify_SlaughteredAnimal();
				}
			});
			yield break;
		}

		// Token: 0x04001D51 RID: 7505
		public const int SlaughterDuration = 180;
	}
}
