using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000726 RID: 1830
	public class JobDriver_ManTurret : JobDriver
	{
		// Token: 0x060032D4 RID: 13012 RVA: 0x00123AFC File Offset: 0x00121CFC
		private static bool GunNeedsLoading(Building b)
		{
			Building_TurretGun building_TurretGun = b as Building_TurretGun;
			if (building_TurretGun == null)
			{
				return false;
			}
			CompChangeableProjectile compChangeableProjectile = building_TurretGun.gun.TryGetComp<CompChangeableProjectile>();
			return compChangeableProjectile != null && !compChangeableProjectile.Loaded;
		}

		// Token: 0x060032D5 RID: 13013 RVA: 0x00123B30 File Offset: 0x00121D30
		private static bool GunNeedsRefueling(Building b)
		{
			Building_TurretGun building_TurretGun = b as Building_TurretGun;
			if (building_TurretGun == null)
			{
				return false;
			}
			CompRefuelable compRefuelable = building_TurretGun.TryGetComp<CompRefuelable>();
			return compRefuelable != null && !compRefuelable.HasFuel && compRefuelable.Props.fuelIsMortarBarrel && Find.Storyteller.difficulty.classicMortars;
		}

		// Token: 0x060032D6 RID: 13014 RVA: 0x00123B7C File Offset: 0x00121D7C
		public static Thing FindAmmoForTurret(Pawn pawn, Building_TurretGun gun)
		{
			StorageSettings allowedShellsSettings = pawn.IsColonist ? gun.gun.TryGetComp<CompChangeableProjectile>().allowedShellsSettings : null;
			Predicate<Thing> validator = (Thing t) => !t.IsForbidden(pawn) && pawn.CanReserve(t, 10, 1, null, false) && (allowedShellsSettings == null || allowedShellsSettings.AllowedToAccept(t));
			return GenClosest.ClosestThingReachable(gun.Position, gun.Map, ThingRequest.ForGroup(ThingRequestGroup.Shell), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false), 40f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
		}

		// Token: 0x060032D7 RID: 13015 RVA: 0x00123C00 File Offset: 0x00121E00
		public static Thing FindFuelForTurret(Pawn pawn, Building_TurretGun gun)
		{
			CompRefuelable refuelableComp = gun.TryGetComp<CompRefuelable>();
			if (refuelableComp == null)
			{
				return null;
			}
			Predicate<Thing> validator = (Thing t) => !t.IsForbidden(pawn) && pawn.CanReserve(t, 10, 1, null, false) && refuelableComp.Props.fuelFilter.Allows(t);
			return GenClosest.ClosestThingReachable(gun.Position, gun.Map, ThingRequest.ForGroup(ThingRequestGroup.HaulableEver), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false), 40f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
		}

		// Token: 0x060032D8 RID: 13016 RVA: 0x000FC76A File Offset: 0x000FA96A
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x060032D9 RID: 13017 RVA: 0x00123C72 File Offset: 0x00121E72
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			Toil gotoTurret = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
			Toil refuelIfNeeded = new Toil();
			refuelIfNeeded.initAction = delegate()
			{
				Pawn actor = refuelIfNeeded.actor;
				Building building = (Building)actor.CurJob.targetA.Thing;
				Building_TurretGun building_TurretGun = building as Building_TurretGun;
				if (!JobDriver_ManTurret.GunNeedsRefueling(building))
				{
					this.JumpToToil(gotoTurret);
					return;
				}
				Thing thing = JobDriver_ManTurret.FindFuelForTurret(this.pawn, building_TurretGun);
				if (thing == null)
				{
					CompRefuelable compRefuelable = building.TryGetComp<CompRefuelable>();
					if (actor.Faction == Faction.OfPlayer && compRefuelable != null)
					{
						Messages.Message("MessageOutOfNearbyFuelFor".Translate(actor.LabelShort, building_TurretGun.Label, actor.Named("PAWN"), building_TurretGun.Named("GUN"), compRefuelable.Props.fuelFilter.Summary.Named("FUEL")).CapitalizeFirst(), building_TurretGun, MessageTypeDefOf.NegativeEvent, true);
					}
					actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
				}
				actor.CurJob.targetB = thing;
				actor.CurJob.count = 1;
			};
			yield return refuelIfNeeded;
			yield return Toils_Reserve.Reserve(TargetIndex.B, 10, 1, null);
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.OnCell).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, false, false);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_General.Wait(240, TargetIndex.None).FailOnDestroyedNullOrForbidden(TargetIndex.B).FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			yield return Toils_Refuel.FinalizeRefueling(TargetIndex.A, TargetIndex.B);
			Toil loadIfNeeded = new Toil();
			loadIfNeeded.initAction = delegate()
			{
				Pawn actor = loadIfNeeded.actor;
				Building building = (Building)actor.CurJob.targetA.Thing;
				Building_TurretGun building_TurretGun = building as Building_TurretGun;
				if (!JobDriver_ManTurret.GunNeedsLoading(building))
				{
					this.JumpToToil(gotoTurret);
					return;
				}
				Thing thing = JobDriver_ManTurret.FindAmmoForTurret(this.pawn, building_TurretGun);
				if (thing == null)
				{
					if (actor.Faction == Faction.OfPlayer)
					{
						Messages.Message("MessageOutOfNearbyShellsFor".Translate(actor.LabelShort, building_TurretGun.Label, actor.Named("PAWN"), building_TurretGun.Named("GUN")).CapitalizeFirst(), building_TurretGun, MessageTypeDefOf.NegativeEvent, true);
					}
					actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
				}
				actor.CurJob.targetB = thing;
				actor.CurJob.count = 1;
			};
			yield return loadIfNeeded;
			yield return Toils_Reserve.Reserve(TargetIndex.B, 10, 1, null);
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.OnCell).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, false, false);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			Toil loadShell = new Toil();
			loadShell.initAction = delegate()
			{
				Pawn actor = loadShell.actor;
				Building_TurretGun building_TurretGun = ((Building)actor.CurJob.targetA.Thing) as Building_TurretGun;
				SoundDefOf.Artillery_ShellLoaded.PlayOneShot(new TargetInfo(building_TurretGun.Position, building_TurretGun.Map, false));
				building_TurretGun.gun.TryGetComp<CompChangeableProjectile>().LoadShell(actor.CurJob.targetB.Thing.def, 1);
				actor.carryTracker.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
			};
			yield return loadShell;
			yield return gotoTurret;
			Toil man = new Toil();
			man.tickAction = delegate()
			{
				Pawn actor = man.actor;
				Building building = (Building)actor.CurJob.targetA.Thing;
				if (JobDriver_ManTurret.GunNeedsLoading(building))
				{
					this.JumpToToil(loadIfNeeded);
					return;
				}
				if (JobDriver_ManTurret.GunNeedsRefueling(building))
				{
					this.JumpToToil(refuelIfNeeded);
					return;
				}
				building.GetComp<CompMannable>().ManForATick(actor);
				man.actor.rotationTracker.FaceCell(building.Position);
			};
			man.handlingFacing = true;
			man.defaultCompleteMode = ToilCompleteMode.Never;
			man.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
			yield return man;
			yield break;
		}

		// Token: 0x04001DD9 RID: 7641
		private const float SearchRadius = 40f;

		// Token: 0x04001DDA RID: 7642
		private const int MaxPawnReservations = 10;
	}
}
