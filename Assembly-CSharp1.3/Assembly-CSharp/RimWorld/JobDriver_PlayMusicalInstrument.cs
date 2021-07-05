using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006FB RID: 1787
	public class JobDriver_PlayMusicalInstrument : JobDriver_SitFacingBuilding
	{
		// Token: 0x17000940 RID: 2368
		// (get) Token: 0x060031AE RID: 12718 RVA: 0x00120DE6 File Offset: 0x0011EFE6
		public Building_MusicalInstrument MusicalInstrument
		{
			get
			{
				return (Building_MusicalInstrument)((Thing)this.job.GetTarget(TargetIndex.A));
			}
		}

		// Token: 0x060031AF RID: 12719 RVA: 0x00120E00 File Offset: 0x0011F000
		protected override void ModifyPlayToil(Toil toil)
		{
			base.ModifyPlayToil(toil);
			Action <>9__2;
			toil.AddPreInitAction(delegate
			{
				Building_MusicalInstrument musicalInstrument = this.MusicalInstrument;
				if (musicalInstrument != null)
				{
					musicalInstrument.StartPlaying(this.pawn);
				}
				Toil toil2 = toil;
				Action tickAction;
				if ((tickAction = <>9__2) == null)
				{
					tickAction = (<>9__2 = delegate()
					{
						toil.actor.rotationTracker.FaceTarget(toil.actor.CurJob.GetTarget(this.TargetC.IsValid ? TargetIndex.C : TargetIndex.A));
					});
				}
				toil2.tickAction = tickAction;
			});
			toil.handlingFacing = true;
			toil.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			toil.AddFinishAction(delegate
			{
				this.MusicalInstrument.StopPlaying();
			});
		}
	}
}
