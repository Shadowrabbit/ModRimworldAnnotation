using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B96 RID: 2966
	public class JobDriver_PlayMusicalInstrument : JobDriver_SitFacingBuilding
	{
		// Token: 0x17000AE1 RID: 2785
		// (get) Token: 0x060045A4 RID: 17828 RVA: 0x000331A5 File Offset: 0x000313A5
		public Building_MusicalInstrument MusicalInstrument
		{
			get
			{
				return (Building_MusicalInstrument)((Thing)this.job.GetTarget(TargetIndex.A));
			}
		}

		// Token: 0x060045A5 RID: 17829 RVA: 0x000331BD File Offset: 0x000313BD
		protected override void ModifyPlayToil(Toil toil)
		{
			base.ModifyPlayToil(toil);
			toil.AddPreInitAction(delegate
			{
				this.MusicalInstrument.StartPlaying(this.pawn);
			});
			toil.AddFinishAction(delegate
			{
				this.MusicalInstrument.StopPlaying();
			});
		}
	}
}
