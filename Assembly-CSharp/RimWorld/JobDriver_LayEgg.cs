using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B27 RID: 2855
	public class JobDriver_LayEgg : JobDriver
	{
		// Token: 0x06004301 RID: 17153 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06004302 RID: 17154 RVA: 0x00031CA3 File Offset: 0x0002FEA3
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			yield return Toils_General.Wait(500, TargetIndex.None);
			yield return Toils_General.Do(delegate
			{
				GenSpawn.Spawn(this.pawn.GetComp<CompEggLayer>().ProduceEgg(), this.pawn.Position, base.Map, WipeMode.Vanish).SetForbiddenIfOutsideHomeArea();
			});
			yield break;
		}

		// Token: 0x04002DD6 RID: 11734
		private const int LayEgg = 500;

		// Token: 0x04002DD7 RID: 11735
		private const TargetIndex LaySpotInd = TargetIndex.A;
	}
}
