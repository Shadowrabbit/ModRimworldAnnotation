using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006DD RID: 1757
	public class JobDriver_PlaceNoCostFrame : JobDriver
	{
		// Token: 0x06003102 RID: 12546 RVA: 0x000FC76A File Offset: 0x000FA96A
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06003103 RID: 12547 RVA: 0x0011F01A File Offset: 0x0011D21A
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_Goto.MoveOffTargetBlueprint(TargetIndex.A);
			yield return Toils_Construct.MakeSolidThingFromBlueprintIfNecessary(TargetIndex.A, TargetIndex.None);
			yield break;
		}
	}
}
