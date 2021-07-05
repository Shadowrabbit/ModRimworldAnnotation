using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000752 RID: 1874
	public class JobDriver_GetNeuralSupercharge : JobDriver
	{
		// Token: 0x060033DA RID: 13274 RVA: 0x0012666C File Offset: 0x0012486C
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A).Thing, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x060033DB RID: 13275 RVA: 0x001266A7 File Offset: 0x001248A7
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
			yield return Toils_General.Wait(0.55f.SecondsToTicks(), TargetIndex.None);
			Toil toil = Toils_General.Wait(1.5f.SecondsToTicks(), TargetIndex.None);
			toil.WithEffect(() => EffecterDefOf.NeuralSuperchargerUse, this.job.GetTarget(TargetIndex.A).Thing, null);
			yield return toil;
			yield return Toils_General.Do(delegate
			{
				Thing thing = this.job.GetTarget(TargetIndex.A).Thing;
				CompRechargeable compRechargeable = (thing != null) ? thing.TryGetComp<CompRechargeable>() : null;
				if (compRechargeable != null)
				{
					compRechargeable.Discharge();
				}
				this.pawn.health.AddHediff(HediffDefOf.NeuralSupercharge, this.pawn.health.hediffSet.GetBrain(), null, null);
			});
			yield return Toils_General.Wait(0.35f.SecondsToTicks(), TargetIndex.None);
			yield break;
		}

		// Token: 0x04001E20 RID: 7712
		private const TargetIndex ChargerInd = TargetIndex.A;
	}
}
