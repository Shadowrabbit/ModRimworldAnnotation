using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000822 RID: 2082
	public class JobDriver_ChangeTreeMode : JobDriver
	{
		// Token: 0x0600375C RID: 14172 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x0600375D RID: 14173 RVA: 0x001388BD File Offset: 0x00136ABD
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_General.WaitWith(TargetIndex.A, 120, true, false);
			yield return Toils_General.Do(delegate
			{
				this.job.targetA.Thing.TryGetComp<CompTreeConnection>().FinalizeMode();
			});
			yield return Toils_General.Wait(60, TargetIndex.A);
			yield break;
		}

		// Token: 0x04001F11 RID: 7953
		private const int WaitTicks = 120;
	}
}
