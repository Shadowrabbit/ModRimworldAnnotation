using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200071D RID: 1821
	public class JobDriver_Ignite : JobDriver
	{
		// Token: 0x06003295 RID: 12949 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06003296 RID: 12950 RVA: 0x00123101 File Offset: 0x00121301
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnBurningImmobile(TargetIndex.A);
			yield return new Toil
			{
				initAction = delegate()
				{
					this.pawn.natives.TryStartIgnite(base.TargetThingA);
				}
			};
			yield break;
		}
	}
}
