using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BCF RID: 3023
	public class JobDriver_Ignite : JobDriver
	{
		// Token: 0x06004716 RID: 18198 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06004717 RID: 18199 RVA: 0x00033D36 File Offset: 0x00031F36
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
