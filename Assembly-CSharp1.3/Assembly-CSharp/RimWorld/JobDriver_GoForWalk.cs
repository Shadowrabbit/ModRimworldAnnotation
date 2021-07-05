using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006F7 RID: 1783
	public class JobDriver_GoForWalk : JobDriver
	{
		// Token: 0x060031A0 RID: 12704 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x060031A1 RID: 12705 RVA: 0x00120BFF File Offset: 0x0011EDFF
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
