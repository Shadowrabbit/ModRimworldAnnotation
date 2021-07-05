﻿using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006F4 RID: 1780
	public class Toils_Tend
	{
		// Token: 0x06003192 RID: 12690 RVA: 0x001204F8 File Offset: 0x0011E6F8
		public static Toil ReserveMedicine(TargetIndex ind, Pawn injured)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				Thing thing = curJob.GetTarget(ind).Thing;
				int num = actor.Map.reservationManager.CanReserveStack(actor, thing, 10, null, false);
				if (num <= 0 || !actor.Reserve(thing, curJob, 10, Mathf.Min(num, Medicine.GetMedicineCountToFullyHeal(injured)), null, true))
				{
					toil.actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			toil.atomicWithPrevious = true;
			return toil;
		}

		// Token: 0x06003193 RID: 12691 RVA: 0x0012055C File Offset: 0x0011E75C
		public static Toil PickupMedicine(TargetIndex ind, Pawn injured)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				Thing thing = curJob.GetTarget(ind).Thing;
				int num = Medicine.GetMedicineCountToFullyHeal(injured);
				if (actor.carryTracker.CarriedThing != null)
				{
					num -= actor.carryTracker.CarriedThing.stackCount;
				}
				int num2 = Mathf.Min(actor.Map.reservationManager.CanReserveStack(actor, thing, 10, null, false), num);
				if (num2 > 0)
				{
					actor.carryTracker.TryStartCarry(thing, num2, true);
				}
				curJob.count = num - num2;
				if (thing.Spawned)
				{
					toil.actor.Map.reservationManager.Release(thing, actor, curJob);
				}
				curJob.SetTarget(ind, actor.carryTracker.CarriedThing);
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}

		// Token: 0x06003194 RID: 12692 RVA: 0x001205B4 File Offset: 0x0011E7B4
		public static Toil FinalizeTend(Pawn patient)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Medicine medicine = (Medicine)actor.CurJob.targetB.Thing;
				float num = patient.RaceProps.Animal ? 175f : 500f;
				float num2 = (medicine == null) ? 0.5f : medicine.def.MedicineTendXpGainFactor;
				actor.skills.Learn(SkillDefOf.Medicine, num * num2, false);
				TendUtility.DoTend(actor, patient, medicine);
				if (medicine != null && medicine.Destroyed)
				{
					actor.CurJob.SetTarget(TargetIndex.B, LocalTargetInfo.Invalid);
				}
				if (toil.actor.CurJob.endAfterTendedOnce)
				{
					actor.jobs.EndCurrentJob(JobCondition.Succeeded, true, true);
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}

		// Token: 0x04001D8A RID: 7562
		public const int MaxMedicineReservations = 10;
	}
}
