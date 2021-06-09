using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000BDD RID: 3037
	public class JobDriver_ManTurret : JobDriver
	{
		// Token: 0x06004779 RID: 18297 RVA: 0x00197ED8 File Offset: 0x001960D8
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

		// Token: 0x0600477A RID: 18298 RVA: 0x00197F0C File Offset: 0x0019610C
		public static Thing FindAmmoForTurret(Pawn pawn, Building_TurretGun gun)
		{
			StorageSettings allowedShellsSettings = pawn.IsColonist ? gun.gun.TryGetComp<CompChangeableProjectile>().allowedShellsSettings : null;
			Predicate<Thing> validator = (Thing t) => !t.IsForbidden(pawn) && pawn.CanReserve(t, 10, 1, null, false) && (allowedShellsSettings == null || allowedShellsSettings.AllowedToAccept(t));
			return GenClosest.ClosestThingReachable(gun.Position, gun.Map, ThingRequest.ForGroup(ThingRequestGroup.Shell), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 40f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
		}

		// Token: 0x0600477B RID: 18299 RVA: 0x0002D6EB File Offset: 0x0002B8EB
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x0600477C RID: 18300 RVA: 0x000340A6 File Offset: 0x000322A6
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			Toil gotoTurret = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
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
			yield return new Toil
			{
				initAction = delegate()
				{
					Pawn actor = loadIfNeeded.actor;
					Building_TurretGun building_TurretGun = ((Building)actor.CurJob.targetA.Thing) as Building_TurretGun;
					SoundDefOf.Artillery_ShellLoaded.PlayOneShot(new TargetInfo(building_TurretGun.Position, building_TurretGun.Map, false));
					building_TurretGun.gun.TryGetComp<CompChangeableProjectile>().LoadShell(actor.CurJob.targetB.Thing.def, 1);
					actor.carryTracker.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
				}
			};
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
				building.GetComp<CompMannable>().ManForATick(actor);
				man.actor.rotationTracker.FaceCell(building.Position);
			};
			man.handlingFacing = true;
			man.defaultCompleteMode = ToilCompleteMode.Never;
			man.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
			yield return man;
			yield break;
		}

		// Token: 0x04002FD4 RID: 12244
		private const float ShellSearchRadius = 40f;

		// Token: 0x04002FD5 RID: 12245
		private const int MaxPawnAmmoReservations = 10;
	}
}
