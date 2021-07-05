using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006E3 RID: 1763
	public static class Toils_Construct
	{
		// Token: 0x06003128 RID: 12584 RVA: 0x0011F31C File Offset: 0x0011D51C
		public static Toil MakeSolidThingFromBlueprintIfNecessary(TargetIndex blueTarget, TargetIndex targetToUpdate = TargetIndex.None)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				Blueprint blueprint = curJob.GetTarget(blueTarget).Thing as Blueprint;
				if (blueprint != null)
				{
					bool flag = targetToUpdate != TargetIndex.None && curJob.GetTarget(targetToUpdate).Thing == blueprint;
					Thing thing;
					bool flag2;
					if (blueprint.TryReplaceWithSolidThing(actor, out thing, out flag2))
					{
						curJob.SetTarget(blueTarget, thing);
						if (flag)
						{
							curJob.SetTarget(targetToUpdate, thing);
						}
						if (thing is Frame)
						{
							actor.Reserve(thing, curJob, 1, -1, null, true);
						}
					}
					return;
				}
			};
			return toil;
		}

		// Token: 0x06003129 RID: 12585 RVA: 0x0011F368 File Offset: 0x0011D568
		public static Toil UninstallIfMinifiable(TargetIndex thingInd)
		{
			Toil uninstallIfMinifiable = new Toil().FailOnDestroyedNullOrForbidden(thingInd);
			uninstallIfMinifiable.initAction = delegate()
			{
				Pawn actor = uninstallIfMinifiable.actor;
				JobDriver curDriver = actor.jobs.curDriver;
				Thing thing = actor.CurJob.GetTarget(thingInd).Thing;
				if (thing.def.Minifiable)
				{
					curDriver.uninstallWorkLeft = thing.def.building.uninstallWork;
					return;
				}
				curDriver.ReadyForNextToil();
			};
			uninstallIfMinifiable.tickAction = delegate()
			{
				Pawn actor = uninstallIfMinifiable.actor;
				JobDriver curDriver = actor.jobs.curDriver;
				Job curJob = actor.CurJob;
				curDriver.uninstallWorkLeft -= actor.GetStatValue(StatDefOf.ConstructionSpeed, true) * 1.7f;
				if (curDriver.uninstallWorkLeft <= 0f)
				{
					Thing thing = curJob.GetTarget(thingInd).Thing;
					MinifiedThing minifiedThing = thing.MakeMinified();
					GenSpawn.Spawn(minifiedThing, thing.Position, uninstallIfMinifiable.actor.Map, WipeMode.Vanish);
					curJob.SetTarget(thingInd, minifiedThing);
					actor.jobs.curDriver.ReadyForNextToil();
					return;
				}
			};
			uninstallIfMinifiable.defaultCompleteMode = ToilCompleteMode.Never;
			uninstallIfMinifiable.WithProgressBar(thingInd, () => 1f - uninstallIfMinifiable.actor.jobs.curDriver.uninstallWorkLeft / uninstallIfMinifiable.actor.CurJob.targetA.Thing.def.building.uninstallWork, false, -0.5f, false);
			return uninstallIfMinifiable;
		}
	}
}
