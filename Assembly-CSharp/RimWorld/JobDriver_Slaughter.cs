using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B3C RID: 2876
	public class JobDriver_Slaughter : JobDriver
	{
		// Token: 0x17000A80 RID: 2688
		// (get) Token: 0x06004386 RID: 17286 RVA: 0x00031F33 File Offset: 0x00030133
		protected Pawn Victim
		{
			get
			{
				return (Pawn)this.job.targetA.Thing;
			}
		}

		// Token: 0x06004387 RID: 17287 RVA: 0x000320AB File Offset: 0x000302AB
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Victim, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06004388 RID: 17288 RVA: 0x000320CD File Offset: 0x000302CD
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnAggroMentalState(TargetIndex.A);
			this.FailOnThingMissingDesignation(TargetIndex.A, DesignationDefOf.Slaughter);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_General.WaitWith(TargetIndex.A, 180, true, false);
			yield return Toils_General.Do(delegate
			{
				ExecutionUtility.DoExecutionByCut(this.pawn, this.Victim);
				this.pawn.records.Increment(RecordDefOf.AnimalsSlaughtered);
				if (this.pawn.InMentalState)
				{
					this.pawn.MentalState.Notify_SlaughteredAnimal();
				}
			});
			yield break;
		}

		// Token: 0x04002E1D RID: 11805
		public const int SlaughterDuration = 180;
	}
}
