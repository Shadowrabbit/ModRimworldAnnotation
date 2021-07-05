using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005B1 RID: 1457
	public static class Toils_General
	{
		// Token: 0x06002A9A RID: 10906 RVA: 0x000FFF84 File Offset: 0x000FE184
		public static Toil StopDead()
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				toil.actor.pather.StopDead();
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}

		// Token: 0x06002A9B RID: 10907 RVA: 0x000FFFCC File Offset: 0x000FE1CC
		public static Toil Wait(int ticks, TargetIndex face = TargetIndex.None)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				toil.actor.pather.StopDead();
			};
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = ticks;
			if (face != TargetIndex.None)
			{
				toil.handlingFacing = true;
				toil.tickAction = delegate()
				{
					toil.actor.rotationTracker.FaceTarget(toil.actor.CurJob.GetTarget(face));
				};
			}
			return toil;
		}

		// Token: 0x06002A9C RID: 10908 RVA: 0x00100054 File Offset: 0x000FE254
		public static Toil WaitWith(TargetIndex targetInd, int ticks, bool useProgressBar = false, bool maintainPosture = false)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				toil.actor.pather.StopDead();
				Pawn pawn = toil.actor.CurJob.GetTarget(targetInd).Thing as Pawn;
				if (pawn != null)
				{
					if (pawn == toil.actor)
					{
						Log.Warning("Executing WaitWith toil but otherPawn is the same as toil.actor");
						return;
					}
					PawnUtility.ForceWait(pawn, ticks, null, maintainPosture);
				}
			};
			toil.FailOnDespawnedOrNull(targetInd);
			toil.FailOnCannotTouch(targetInd, PathEndMode.Touch);
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = ticks;
			if (useProgressBar)
			{
				toil.WithProgressBarToilDelay(targetInd, false, -0.5f);
			}
			return toil;
		}

		// Token: 0x06002A9D RID: 10909 RVA: 0x00100104 File Offset: 0x000FE304
		public static Toil RemoveDesignationsOnThing(TargetIndex ind, DesignationDef def)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				toil.actor.Map.designationManager.RemoveAllDesignationsOn(toil.actor.jobs.curJob.GetTarget(ind).Thing, false);
			};
			return toil;
		}

		// Token: 0x06002A9E RID: 10910 RVA: 0x00100148 File Offset: 0x000FE348
		public static Toil ClearTarget(TargetIndex ind)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				toil.GetActor().CurJob.SetTarget(ind, null);
			};
			return toil;
		}

		// Token: 0x06002A9F RID: 10911 RVA: 0x0010018C File Offset: 0x000FE38C
		public static Toil PutCarriedThingInInventory()
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.GetActor();
				if (actor.carryTracker.CarriedThing != null && !actor.carryTracker.innerContainer.TryTransferToContainer(actor.carryTracker.CarriedThing, actor.inventory.innerContainer, true))
				{
					Thing thing;
					actor.carryTracker.TryDropCarriedThing(actor.Position, actor.carryTracker.CarriedThing.stackCount, ThingPlaceMode.Near, out thing, null);
				}
			};
			return toil;
		}

		// Token: 0x06002AA0 RID: 10912 RVA: 0x001001C7 File Offset: 0x000FE3C7
		public static Toil Do(Action action)
		{
			return new Toil
			{
				initAction = action
			};
		}

		// Token: 0x06002AA1 RID: 10913 RVA: 0x001001D5 File Offset: 0x000FE3D5
		public static Toil DoAtomic(Action action)
		{
			return new Toil
			{
				initAction = action,
				atomicWithPrevious = true
			};
		}

		// Token: 0x06002AA2 RID: 10914 RVA: 0x001001EC File Offset: 0x000FE3EC
		public static Toil Open(TargetIndex openableInd)
		{
			Toil open = new Toil();
			open.initAction = delegate()
			{
				Pawn actor = open.actor;
				Thing thing = actor.CurJob.GetTarget(openableInd).Thing;
				Designation designation = actor.Map.designationManager.DesignationOn(thing, DesignationDefOf.Open);
				if (designation != null)
				{
					designation.Delete();
				}
				IOpenable openable = (IOpenable)thing;
				if (openable.CanOpen)
				{
					openable.Open();
					actor.records.Increment(RecordDefOf.ContainersOpened);
				}
			};
			open.defaultCompleteMode = ToilCompleteMode.Instant;
			return open;
		}

		// Token: 0x06002AA3 RID: 10915 RVA: 0x0010023A File Offset: 0x000FE43A
		public static Toil Label()
		{
			return new Toil
			{
				atomicWithPrevious = true,
				defaultCompleteMode = ToilCompleteMode.Instant
			};
		}

		// Token: 0x06002AA4 RID: 10916 RVA: 0x00100250 File Offset: 0x000FE450
		public static Toil WaitWhileExtractingContents(TargetIndex containerInd, TargetIndex contentsInd, int openTicks)
		{
			Toil extract = Toils_General.Wait(openTicks, containerInd).WithProgressBarToilDelay(containerInd, false, -0.5f).FailOnDespawnedOrNull(containerInd);
			extract.handlingFacing = true;
			extract.AddPreInitAction(delegate
			{
				Thing thing = extract.actor.CurJob.GetTarget(contentsInd).Thing;
				QuestUtility.SendQuestTargetSignals(thing.questTags, "StartedExtractingFromContainer", thing.Named("SUBJECT"));
			});
			return extract;
		}
	}
}
