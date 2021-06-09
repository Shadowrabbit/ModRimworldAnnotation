using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BE7 RID: 3047
	public class JobDriver_Open : JobDriver
	{
		// Token: 0x17000B44 RID: 2884
		// (get) Token: 0x060047AE RID: 18350 RVA: 0x0003420E File Offset: 0x0003240E
		private IOpenable Openable
		{
			get
			{
				return (IOpenable)this.job.targetA.Thing;
			}
		}

		// Token: 0x060047AF RID: 18351 RVA: 0x0002D6EB File Offset: 0x0002B8EB
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x060047B0 RID: 18352 RVA: 0x00034225 File Offset: 0x00032425
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return new Toil
			{
				initAction = delegate()
				{
					if (!this.Openable.CanOpen)
					{
						Designation designation = base.Map.designationManager.DesignationOn(this.job.targetA.Thing, DesignationDefOf.Open);
						if (designation != null)
						{
							designation.Delete();
						}
					}
				}
			}.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell).FailOnThingMissingDesignation(TargetIndex.A, DesignationDefOf.Open).FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_General.Wait(300, TargetIndex.None).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f).FailOnDespawnedOrNull(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
			yield return Toils_General.Open(TargetIndex.A);
			yield break;
		}

		// Token: 0x04002FF5 RID: 12277
		public const int OpenTicks = 300;
	}
}
