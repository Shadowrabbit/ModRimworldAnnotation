using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B8F RID: 2959
	public class JobDriver_GoForWalk : JobDriver
	{
		// Token: 0x06004582 RID: 17794 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06004583 RID: 17795 RVA: 0x000330C7 File Offset: 0x000312C7
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(() => !JoyUtility.EnjoyableOutsideNow(this.pawn, null));
			Toil goToil = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			goToil.tickAction = delegate()
			{
				if (Find.TickManager.TicksGame > this.startTick + this.job.def.joyDuration)
				{
					this.EndJobWith(JobCondition.Succeeded);
					return;
				}
				JoyUtility.JoyTickCheckEnd(this.pawn, JoyTickFullJoyAction.EndJob, 1f, null);
			};
			yield return goToil;
			yield return new Toil
			{
				initAction = delegate()
				{
					if (this.job.targetQueueA.Count > 0)
					{
						LocalTargetInfo targetA = this.job.targetQueueA[0];
						this.job.targetQueueA.RemoveAt(0);
						this.job.targetA = targetA;
						this.JumpToToil(goToil);
						return;
					}
				}
			};
			yield break;
		}
	}
}
