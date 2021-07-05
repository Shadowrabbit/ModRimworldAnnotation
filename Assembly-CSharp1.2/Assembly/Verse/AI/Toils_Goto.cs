using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A00 RID: 2560
	public static class Toils_Goto
	{
		// Token: 0x06003D5E RID: 15710 RVA: 0x0002E49C File Offset: 0x0002C69C
		public static Toil Goto(TargetIndex ind, PathEndMode peMode)
		{
			return Toils_Goto.GotoThing(ind, peMode);
		}

		// Token: 0x06003D5F RID: 15711 RVA: 0x00175394 File Offset: 0x00173594
		public static Toil GotoThing(TargetIndex ind, PathEndMode peMode)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				actor.pather.StartPath(actor.jobs.curJob.GetTarget(ind), peMode);
			};
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			toil.FailOnDespawnedOrNull(ind);
			return toil;
		}

		// Token: 0x06003D60 RID: 15712 RVA: 0x001753FC File Offset: 0x001735FC
		public static Toil GotoThing(TargetIndex ind, IntVec3 exactCell)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				toil.actor.pather.StartPath(exactCell, PathEndMode.OnCell);
			};
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			toil.FailOnDespawnedOrNull(ind);
			return toil;
		}

		// Token: 0x06003D61 RID: 15713 RVA: 0x00175458 File Offset: 0x00173658
		public static Toil GotoCell(TargetIndex ind, PathEndMode peMode)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				actor.pather.StartPath(actor.jobs.curJob.GetTarget(ind), peMode);
			};
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			return toil;
		}

		// Token: 0x06003D62 RID: 15714 RVA: 0x001754B0 File Offset: 0x001736B0
		public static Toil GotoCell(IntVec3 cell, PathEndMode peMode)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				toil.actor.pather.StartPath(cell, peMode);
			};
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			return toil;
		}

		// Token: 0x06003D63 RID: 15715 RVA: 0x00175508 File Offset: 0x00173708
		public static Toil MoveOffTargetBlueprint(TargetIndex targetInd)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Thing thing = actor.jobs.curJob.GetTarget(targetInd).Thing as Blueprint;
				if (thing == null || !actor.Position.IsInside(thing))
				{
					actor.jobs.curDriver.ReadyForNextToil();
					return;
				}
				IntVec3 c;
				if (RCellFinder.TryFindGoodAdjacentSpotToTouch(actor, thing, out c))
				{
					actor.pather.StartPath(c, PathEndMode.OnCell);
					return;
				}
				actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
			};
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			return toil;
		}
	}
}
