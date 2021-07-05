using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000716 RID: 1814
	public class JobDriver_Flick : JobDriver
	{
		// Token: 0x06003254 RID: 12884 RVA: 0x000FC76A File Offset: 0x000FA96A
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06003255 RID: 12885 RVA: 0x00122699 File Offset: 0x00120899
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
