using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020006EF RID: 1775
	public class JobDriver_Sacrifice : JobDriver
	{
		// Token: 0x1700093B RID: 2363
		// (get) Token: 0x06003173 RID: 12659 RVA: 0x0011FFA4 File Offset: 0x0011E1A4
		protected Pawn Victim
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06003174 RID: 12660 RVA: 0x0011FFCA File Offset: 0x0011E1CA
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Victim, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06003175 RID: 12661 RVA: 0x0011FFEC File Offset: 0x0011E1EC
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModLister.CheckIdeology("Sacrifice"))
			{
				yield break;
			}
			this.FailOnDestroyedOrNull(TargetIndex.A);
			Pawn victim = this.Victim;
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.OnCell);
			yield return Toils_General.Wait(35, TargetIndex.None);
			Toil execute = new Toil();
			execute.initAction = delegate()
			{
				Lord lord = this.pawn.GetLord();
				LordJob_Ritual lordJob_Ritual;
				if (lord != null && (lordJob_Ritual = (lord.LordJob as LordJob_Ritual)) != null)
				{
					lordJob_Ritual.pawnsDeathIgnored.Add(victim);
				}
				ExecutionUtility.DoExecutionByCut(this.pawn, victim, 0, false);
				ThoughtUtility.GiveThoughtsForPawnExecuted(victim, this.pawn, PawnExecutionKind.GenericBrutal);
				TaleRecorder.RecordTale(TaleDefOf.ExecutedPrisoner, new object[]
				{
					this.pawn,
					victim
				});
				victim.health.killedByRitual = true;
			};
			execute.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return Toils_Reserve.Release(TargetIndex.A);
			yield return execute;
			yield break;
		}

		// Token: 0x04001D80 RID: 7552
		private const TargetIndex VictimIndex = TargetIndex.A;

		// Token: 0x04001D81 RID: 7553
		private const TargetIndex StandingIndex = TargetIndex.B;
	}
}
