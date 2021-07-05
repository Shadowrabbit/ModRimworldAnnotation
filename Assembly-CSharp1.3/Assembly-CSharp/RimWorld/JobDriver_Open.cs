using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000729 RID: 1833
	public class JobDriver_Open : JobDriver
	{
		// Token: 0x1700097D RID: 2429
		// (get) Token: 0x060032E7 RID: 13031 RVA: 0x00123DC0 File Offset: 0x00121FC0
		private IOpenable Openable
		{
			get
			{
				return (IOpenable)this.job.targetA.Thing;
			}
		}

		// Token: 0x1700097E RID: 2430
		// (get) Token: 0x060032E8 RID: 13032 RVA: 0x000FE409 File Offset: 0x000FC609
		private Thing Target
		{
			get
			{
				return this.job.targetA.Thing;
			}
		}

		// Token: 0x060032E9 RID: 13033 RVA: 0x000FC76A File Offset: 0x000FA96A
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x060032EA RID: 13034 RVA: 0x00123DD7 File Offset: 0x00121FD7
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
			Toil toil = Toils_General.Wait(this.Openable.OpenTicks, TargetIndex.A).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f).FailOnDespawnedOrNull(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
			if (this.Target.def.building != null && this.Target.def.building.openingStartedSound != null)
			{
				toil.PlaySoundAtStart(this.Target.def.building.openingStartedSound);
			}
			yield return toil;
			yield return Toils_General.Open(TargetIndex.A);
			yield break;
		}
	}
}
