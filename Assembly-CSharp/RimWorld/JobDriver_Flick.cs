using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BC5 RID: 3013
	public class JobDriver_Flick : JobDriver
	{
		// Token: 0x060046D2 RID: 18130 RVA: 0x0002D6EB File Offset: 0x0002B8EB
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x060046D3 RID: 18131 RVA: 0x00033A8F File Offset: 0x00031C8F
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			this.FailOn(() => this.Map.designationManager.DesignationOn(this.TargetThingA, DesignationDefOf.Flick) == null);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_General.Wait(15, TargetIndex.None).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			Toil finalize = new Toil();
			finalize.initAction = delegate()
			{
				Pawn actor = finalize.actor;
				ThingWithComps thingWithComps = (ThingWithComps)actor.CurJob.targetA.Thing;
				for (int i = 0; i < thingWithComps.AllComps.Count; i++)
				{
					CompFlickable compFlickable = thingWithComps.AllComps[i] as CompFlickable;
					if (compFlickable != null && compFlickable.WantsFlick())
					{
						compFlickable.DoFlick();
					}
				}
				actor.records.Increment(RecordDefOf.SwitchesFlicked);
				Designation designation = this.Map.designationManager.DesignationOn(thingWithComps, DesignationDefOf.Flick);
				if (designation != null)
				{
					designation.Delete();
				}
			};
			finalize.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return finalize;
			yield break;
		}
	}
}
