using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B70 RID: 2928
	public static class Toils_Construct
	{
		// Token: 0x060044DC RID: 17628 RVA: 0x00191058 File Offset: 0x0018F258
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

		// Token: 0x060044DD RID: 17629 RVA: 0x001910A4 File Offset: 0x0018F2A4
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
			uninstallIfMinifiable.WithProgressBar(thingInd, () => 1f - uninstallIfMinifiable.actor.jobs.curDriver.uninstallWorkLeft / uninstallIfMinifiable.actor.CurJob.targetA.Thing.def.building.uninstallWork, false, -0.5f);
			return uninstallIfMinifiable;
		}
	}
}
