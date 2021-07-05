using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006F3 RID: 1779
	public class JobDriver_TendPatient : JobDriver
	{
		// Token: 0x1700093D RID: 2365
		// (get) Token: 0x06003189 RID: 12681 RVA: 0x000FE42E File Offset: 0x000FC62E
		protected Thing MedicineUsed
		{
			get
			{
				return this.job.targetB.Thing;
			}
		}

		// Token: 0x1700093E RID: 2366
		// (get) Token: 0x0600318A RID: 12682 RVA: 0x0011D55B File Offset: 0x0011B75B
		protected Pawn Deliveree
		{
			get
			{
				return (Pawn)this.job.targetA.Thing;
			}
		}

		// Token: 0x1700093F RID: 2367
		// (get) Token: 0x0600318B RID: 12683 RVA: 0x0012038E File Offset: 0x0011E58E
		protected bool IsMedicineInInventory
		{
			get
			{
				return this.MedicineUsed != null && this.pawn.inventory.Contains(this.MedicineUsed);
			}
		}

		// Token: 0x0600318C RID: 12684 RVA: 0x001203B0 File Offset: 0x0011E5B0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.usesMedicine, "usesMedicine", false, false);
		}

		// Token: 0x0600318D RID: 12685 RVA: 0x001203CA File Offset: 0x0011E5CA
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.usesMedicine = (this.MedicineUsed != null);
		}

		// Token: 0x0600318E RID: 12686 RVA: 0x001203E4 File Offset: 0x0011E5E4
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (this.Deliveree != this.pawn && !this.pawn.Reserve(this.Deliveree, this.job, 1, -1, null, errorOnFailed))
			{
				return false;
			}
			if (this.usesMedicine)
			{
				int num = this.pawn.Map.reservationManager.CanReserveStack(this.pawn, this.MedicineUsed, 10, null, false);
				if (num <= 0 || !this.pawn.Reserve(this.MedicineUsed, this.job, 10, Mathf.Min(num, Medicine.GetMedicineCountToFullyHeal(this.Deliveree)), null, errorOnFailed))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600318F RID: 12687 RVA: 0x00120490 File Offset: 0x0011E690
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOn(delegate()
			{
				if (this.MedicineUsed != null && this.pawn.Faction == Faction.OfPlayer)
				{
					if (this.Deliveree.playerSettings == null)
					{
						return true;
					}
					if (!this.Deliveree.playerSettings.medCare.AllowsMedicine(this.MedicineUsed.def))
					{
						return true;
					}
				}
				return this.pawn == this.Deliveree && this.pawn.Faction == Faction.OfPlayer && !this.pawn.playerSettings.selfTend;
			});
			base.AddEndCondition(delegate
			{
				if (this.pawn.Faction == Faction.OfPlayer && HealthAIUtility.ShouldBeTendedNowByPlayer(this.Deliveree))
				{
					return JobCondition.Ongoing;
				}
				if (this.pawn.Faction != Faction.OfPlayer && this.Deliveree.health.HasHediffsNeedingTend(false))
				{
					return JobCondition.Ongoing;
				}
				return JobCondition.Succeeded;
			});
			this.FailOnAggroMentalState(TargetIndex.A);
			Toil reserveMedicine = null;
			PathEndMode interactionCell = PathEndMode.None;
			if (this.Deliveree == this.pawn)
			{
				interactionCell = PathEndMode.OnCell;
			}
			else if (this.Deliveree.InBed())
			{
				interactionCell = PathEndMode.InteractionCell;
			}
			else if (this.Deliveree != this.pawn)
			{
				interactionCell = PathEndMode.ClosestTouch;
			}
			Toil gotoToil = Toils_Goto.GotoThing(TargetIndex.A, interactionCell);
			if (this.usesMedicine)
			{
				reserveMedicine = Toils_Tend.ReserveMedicine(TargetIndex.B, this.Deliveree).FailOnDespawnedNullOrForbidden(TargetIndex.B);
				yield return Toils_Jump.JumpIf(gotoToil, () => this.IsMedicineInInventory);
				yield return reserveMedicine;
				yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.B);
				yield return Toils_Tend.PickupMedicine(TargetIndex.B, this.Deliveree).FailOnDestroyedOrNull(TargetIndex.B);
				yield return Toils_Haul.CheckForGetOpportunityDuplicate(reserveMedicine, TargetIndex.B, TargetIndex.None, true, null);
			}
			yield return gotoToil;
			int ticks = (int)(1f / this.pawn.GetStatValue(StatDefOf.MedicalTendSpeed, true) * 600f);
			Toil waitToil;
			if (!this.job.draftedTend)
			{
				waitToil = Toils_General.Wait(ticks, TargetIndex.None);
			}
			else
			{
				waitToil = Toils_General.WaitWith(TargetIndex.A, ticks, false, true);
				waitToil.AddFinishAction(delegate
				{
					if (this.Deliveree != null && this.Deliveree != this.pawn && this.Deliveree.CurJob != null && (this.Deliveree.CurJob.def == JobDefOf.Wait || this.Deliveree.CurJob.def == JobDefOf.Wait_MaintainPosture))
					{
						this.Deliveree.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
					}
				});
			}
			waitToil.FailOnCannotTouch(TargetIndex.A, interactionCell).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f).PlaySustainerOrSound(SoundDefOf.Interact_Tend, 1f);
			waitToil.activeSkill = (() => SkillDefOf.Medicine);
			waitToil.handlingFacing = true;
			waitToil.tickAction = delegate()
			{
				if (this.pawn == this.Deliveree && this.pawn.Faction != Faction.OfPlayer && this.pawn.IsHashIntervalTick(100) && !this.pawn.Position.Fogged(this.pawn.Map))
				{
					FleckMaker.ThrowMetaIcon(this.pawn.Position, this.pawn.Map, FleckDefOf.HealingCross, 0.42f);
				}
				if (this.pawn != this.Deliveree)
				{
					this.pawn.rotationTracker.FaceTarget(this.Deliveree);
				}
			};
			yield return Toils_Jump.JumpIf(waitToil, () => !this.usesMedicine || !this.IsMedicineInInventory);
			yield return Toils_Tend.PickupMedicine(TargetIndex.B, this.Deliveree).FailOnDestroyedOrNull(TargetIndex.B);
			yield return waitToil;
			yield return Toils_Tend.FinalizeTend(this.Deliveree);
			if (this.usesMedicine)
			{
				yield return new Toil
				{
					initAction = delegate()
					{
						if (this.MedicineUsed.DestroyedOrNull())
						{
							Thing thing = HealthAIUtility.FindBestMedicine(this.pawn, this.Deliveree, false);
							if (thing != null)
							{
								this.job.targetB = thing;
								this.JumpToToil(reserveMedicine);
							}
						}
					}
				};
			}
			yield return Toils_Jump.Jump(gotoToil);
			yield break;
		}

		// Token: 0x06003190 RID: 12688 RVA: 0x001204A0 File Offset: 0x0011E6A0
		public override void Notify_DamageTaken(DamageInfo dinfo)
		{
			base.Notify_DamageTaken(dinfo);
			if (dinfo.Def.ExternalViolenceFor(this.pawn) && this.pawn.Faction != Faction.OfPlayer && this.pawn == this.Deliveree)
			{
				this.pawn.jobs.CheckForJobOverride();
			}
		}

		// Token: 0x04001D87 RID: 7559
		private bool usesMedicine;

		// Token: 0x04001D88 RID: 7560
		private const int BaseTendDuration = 600;

		// Token: 0x04001D89 RID: 7561
		private const int TicksBetweenSelfTendMotes = 100;
	}
}
