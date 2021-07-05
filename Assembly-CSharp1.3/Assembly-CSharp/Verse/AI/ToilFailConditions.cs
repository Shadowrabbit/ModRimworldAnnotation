using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005AD RID: 1453
	public static class ToilFailConditions
	{
		// Token: 0x06002A74 RID: 10868 RVA: 0x000FF580 File Offset: 0x000FD780
		public static Toil FailOn(this Toil toil, Func<Toil, bool> condition)
		{
			toil.AddEndCondition(delegate
			{
				if (condition(toil))
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return toil;
		}

		// Token: 0x06002A75 RID: 10869 RVA: 0x000FF5C0 File Offset: 0x000FD7C0
		public static T FailOn<T>(this T f, Func<bool> condition) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				if (condition())
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06002A76 RID: 10870 RVA: 0x000FF5F4 File Offset: 0x000FD7F4
		public static T FailOnDestroyedOrNull<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				if (f.GetActor().jobs.curJob.GetTarget(ind).Thing.DestroyedOrNull())
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06002A77 RID: 10871 RVA: 0x000FF638 File Offset: 0x000FD838
		public static T FailOnDespawnedOrNull<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				LocalTargetInfo target = f.GetActor().jobs.curJob.GetTarget(ind);
				Thing thing = target.Thing;
				if (thing == null && target.IsValid)
				{
					return JobCondition.Ongoing;
				}
				if (thing == null || !thing.Spawned || thing.Map != f.GetActor().Map)
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06002A78 RID: 10872 RVA: 0x000FF67C File Offset: 0x000FD87C
		public static T EndOnDespawnedOrNull<T>(this T f, TargetIndex ind, JobCondition endCondition = JobCondition.Incompletable) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				LocalTargetInfo target = f.GetActor().jobs.curJob.GetTarget(ind);
				Thing thing = target.Thing;
				if (thing == null && target.IsValid)
				{
					return JobCondition.Ongoing;
				}
				if (thing == null || !thing.Spawned || thing.Map != f.GetActor().Map)
				{
					return endCondition;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06002A79 RID: 10873 RVA: 0x000FF6C8 File Offset: 0x000FD8C8
		public static T EndOnNoTargetInQueue<T>(this T f, TargetIndex ind, JobCondition endCondition = JobCondition.Incompletable) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				if (f.GetActor().jobs.curJob.GetTargetQueue(ind).NullOrEmpty<LocalTargetInfo>())
				{
					return endCondition;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06002A7A RID: 10874 RVA: 0x000FF714 File Offset: 0x000FD914
		public static T FailOnDowned<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				if (((Pawn)f.GetActor().jobs.curJob.GetTarget(ind).Thing).Downed)
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06002A7B RID: 10875 RVA: 0x000FF758 File Offset: 0x000FD958
		public static T FailOnDownedOrDead<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				Thing thing = f.GetActor().jobs.curJob.GetTarget(ind).Thing;
				if (((Pawn)thing).Downed || ((Pawn)thing).Dead)
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06002A7C RID: 10876 RVA: 0x000FF79C File Offset: 0x000FD99C
		public static T FailOnMobile<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				if (((Pawn)f.GetActor().jobs.curJob.GetTarget(ind).Thing).health.State == PawnHealthState.Mobile)
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06002A7D RID: 10877 RVA: 0x000FF7E0 File Offset: 0x000FD9E0
		public static T FailOnNotDowned<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				if (!((Pawn)f.GetActor().jobs.curJob.GetTarget(ind).Thing).Downed)
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06002A7E RID: 10878 RVA: 0x000FF824 File Offset: 0x000FDA24
		public static T FailOnNotAwake<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				if (!((Pawn)f.GetActor().jobs.curJob.GetTarget(ind).Thing).Awake())
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06002A7F RID: 10879 RVA: 0x000FF868 File Offset: 0x000FDA68
		public static T FailOnNotCasualInterruptible<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				if (!((Pawn)f.GetActor().jobs.curJob.GetTarget(ind).Thing).CanCasuallyInteractNow(false, false))
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06002A80 RID: 10880 RVA: 0x000FF8AC File Offset: 0x000FDAAC
		public static T FailOnMentalState<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				Pawn pawn = f.GetActor().jobs.curJob.GetTarget(ind).Thing as Pawn;
				if (pawn != null && pawn.InMentalState && !pawn.health.hediffSet.HasHediff(HediffDefOf.Scaria, false))
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06002A81 RID: 10881 RVA: 0x000FF8F0 File Offset: 0x000FDAF0
		public static T FailOnAggroMentalState<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				Pawn pawn = f.GetActor().jobs.curJob.GetTarget(ind).Thing as Pawn;
				if (pawn != null && pawn.InAggroMentalState && !pawn.health.hediffSet.HasHediff(HediffDefOf.Scaria, false))
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06002A82 RID: 10882 RVA: 0x000FF934 File Offset: 0x000FDB34
		public static T FailOnAggroMentalStateAndHostile<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				Pawn pawn = f.GetActor().jobs.curJob.GetTarget(ind).Thing as Pawn;
				if (pawn != null && pawn.InAggroMentalState && !pawn.health.hediffSet.HasHediff(HediffDefOf.Scaria, false) && pawn.HostileTo(f.GetActor()))
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06002A83 RID: 10883 RVA: 0x000FF978 File Offset: 0x000FDB78
		public static T FailOnSomeonePhysicallyInteracting<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				Pawn actor = f.GetActor();
				Thing thing = actor.jobs.curJob.GetTarget(ind).Thing;
				if (thing != null && actor.Map.physicalInteractionReservationManager.IsReserved(thing) && !actor.Map.physicalInteractionReservationManager.IsReservedBy(actor, thing))
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06002A84 RID: 10884 RVA: 0x000FF9BC File Offset: 0x000FDBBC
		public static T FailOnForbidden<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				Pawn actor = f.GetActor();
				if (actor.Faction != Faction.OfPlayer)
				{
					return JobCondition.Ongoing;
				}
				if (actor.jobs.curJob.ignoreForbidden)
				{
					return JobCondition.Ongoing;
				}
				Thing thing = actor.jobs.curJob.GetTarget(ind).Thing;
				if (thing == null)
				{
					return JobCondition.Ongoing;
				}
				if (thing.IsForbidden(actor))
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06002A85 RID: 10885 RVA: 0x000FFA00 File Offset: 0x000FDC00
		public static T FailOnDespawnedNullOrForbidden<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.FailOnDespawnedOrNull(ind);
			f.FailOnForbidden(ind);
			return f;
		}

		// Token: 0x06002A86 RID: 10886 RVA: 0x000FFA13 File Offset: 0x000FDC13
		public static T FailOnDestroyedNullOrForbidden<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.FailOnDestroyedOrNull(ind);
			f.FailOnForbidden(ind);
			return f;
		}

		// Token: 0x06002A87 RID: 10887 RVA: 0x000FFA28 File Offset: 0x000FDC28
		public static T FailOnThingMissingDesignation<T>(this T f, TargetIndex ind, DesignationDef desDef) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				Pawn actor = f.GetActor();
				Job curJob = actor.jobs.curJob;
				if (curJob.ignoreDesignations)
				{
					return JobCondition.Ongoing;
				}
				Thing thing = curJob.GetTarget(ind).Thing;
				if (thing == null || actor.Map.designationManager.DesignationOn(thing, desDef) == null)
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06002A88 RID: 10888 RVA: 0x000FFA74 File Offset: 0x000FDC74
		public static T FailOnThingHavingDesignation<T>(this T f, TargetIndex ind, DesignationDef desDef) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				Pawn actor = f.GetActor();
				Job curJob = actor.jobs.curJob;
				if (curJob.ignoreDesignations)
				{
					return JobCondition.Ongoing;
				}
				Thing thing = curJob.GetTarget(ind).Thing;
				if (thing == null || actor.Map.designationManager.DesignationOn(thing, desDef) != null)
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06002A89 RID: 10889 RVA: 0x000FFAC0 File Offset: 0x000FDCC0
		public static T FailOnCellMissingDesignation<T>(this T f, TargetIndex ind, DesignationDef desDef) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				Pawn actor = f.GetActor();
				Job curJob = actor.jobs.curJob;
				if (curJob.ignoreDesignations)
				{
					return JobCondition.Ongoing;
				}
				if (actor.Map.designationManager.DesignationAt(curJob.GetTarget(ind).Cell, desDef) == null)
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06002A8A RID: 10890 RVA: 0x000FFB0C File Offset: 0x000FDD0C
		public static T FailOnBurningImmobile<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				if (f.GetActor().jobs.curJob.GetTarget(ind).ToTargetInfo(f.GetActor().Map).IsBurning())
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06002A8B RID: 10891 RVA: 0x000FFB50 File Offset: 0x000FDD50
		public static T FailOnCannotTouch<T>(this T f, TargetIndex ind, PathEndMode peMode) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				if (!f.GetActor().CanReachImmediate(f.GetActor().jobs.curJob.GetTarget(ind), peMode))
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06002A8C RID: 10892 RVA: 0x000FFB9C File Offset: 0x000FDD9C
		public static T FailOnIncapable<T>(this T f, PawnCapacityDef pawnCapacity) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				if (!f.GetActor().health.capacities.CapableOf(pawnCapacity))
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06002A8D RID: 10893 RVA: 0x000FFBE0 File Offset: 0x000FDDE0
		public static Toil FailOnDespawnedNullOrForbiddenPlacedThings(this Toil toil)
		{
			toil.AddFailCondition(delegate
			{
				if (toil.actor.jobs.curJob.placedThings == null)
				{
					return false;
				}
				for (int i = 0; i < toil.actor.jobs.curJob.placedThings.Count; i++)
				{
					ThingCountClass thingCountClass = toil.actor.jobs.curJob.placedThings[i];
					if (thingCountClass.thing == null || !thingCountClass.thing.Spawned || thingCountClass.thing.Map != toil.actor.Map || (!toil.actor.CurJob.ignoreForbidden && thingCountClass.thing.IsForbidden(toil.actor)))
					{
						return true;
					}
				}
				return false;
			});
			return toil;
		}
	}
}
