using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200132F RID: 4911
	public static class FloatMenuUtility
	{
		// Token: 0x060076CE RID: 30414 RVA: 0x0029ABC8 File Offset: 0x00298DC8
		public static void MakeMenu<T>(IEnumerable<T> objects, Func<T, string> labelGetter, Func<T, Action> actionGetter)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (T arg in objects)
			{
				list.Add(new FloatMenuOption(labelGetter(arg), actionGetter(arg), MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x060076CF RID: 30415 RVA: 0x0029AC44 File Offset: 0x00298E44
		public static Action GetRangedAttackAction(Pawn pawn, LocalTargetInfo target, out string failStr)
		{
			failStr = "";
			if (pawn.equipment.Primary == null)
			{
				return null;
			}
			Verb primaryVerb = pawn.equipment.PrimaryEq.PrimaryVerb;
			if (primaryVerb.verbProps.IsMeleeAttack)
			{
				return null;
			}
			Pawn target2;
			Pawn victim;
			if (!pawn.Drafted)
			{
				failStr = "IsNotDraftedLower".Translate(pawn.LabelShort, pawn);
			}
			else if (!pawn.IsColonistPlayerControlled)
			{
				failStr = "CannotOrderNonControlledLower".Translate();
			}
			else if (target.IsValid && !pawn.equipment.PrimaryEq.PrimaryVerb.CanHitTarget(target))
			{
				if (!pawn.Position.InHorDistOf(target.Cell, primaryVerb.verbProps.range))
				{
					failStr = "OutOfRange".Translate();
				}
				float num = primaryVerb.verbProps.EffectiveMinRange(target, pawn);
				if ((float)pawn.Position.DistanceToSquared(target.Cell) < num * num)
				{
					failStr = "TooClose".Translate();
				}
				else
				{
					failStr = "CannotHitTarget".Translate();
				}
			}
			else if (pawn.WorkTagIsDisabled(WorkTags.Violent))
			{
				failStr = "IsIncapableOfViolenceLower".Translate(pawn.LabelShort, pawn);
			}
			else if (pawn == target.Thing)
			{
				failStr = "CannotAttackSelf".Translate();
			}
			else if ((target2 = (target.Thing as Pawn)) != null && (pawn.InSameExtraFaction(target2, ExtraFactionType.HomeFaction, null) || pawn.InSameExtraFaction(target2, ExtraFactionType.MiniFaction, null)))
			{
				failStr = "CannotAttackSameFactionMember".Translate();
			}
			else if ((victim = (target.Thing as Pawn)) != null && HistoryEventUtility.IsKillingInnocentAnimal(pawn, victim) && !new HistoryEvent(HistoryEventDefOf.KilledInnocentAnimal, pawn.Named(HistoryEventArgsNames.Doer)).DoerWillingToDo())
			{
				failStr = "IdeoligionForbids".Translate();
			}
			else
			{
				Pawn pawn2;
				if ((pawn2 = (target.Thing as Pawn)) == null || pawn.Ideo == null || !pawn.Ideo.IsVeneratedAnimal(pawn2) || new HistoryEvent(HistoryEventDefOf.HuntedVeneratedAnimal, pawn.Named(HistoryEventArgsNames.Doer)).DoerWillingToDo())
				{
					return delegate()
					{
						Job job = JobMaker.MakeJob(JobDefOf.AttackStatic, target);
						pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
					};
				}
				failStr = "IdeoligionForbids".Translate();
			}
			failStr = failStr.CapitalizeFirst();
			return null;
		}

		// Token: 0x060076D0 RID: 30416 RVA: 0x0029AF6C File Offset: 0x0029916C
		public static Action GetMeleeAttackAction(Pawn pawn, LocalTargetInfo target, out string failStr)
		{
			failStr = "";
			Pawn target2;
			if (!pawn.Drafted)
			{
				failStr = "IsNotDraftedLower".Translate(pawn.LabelShort, pawn);
			}
			else if (!pawn.IsColonistPlayerControlled)
			{
				failStr = "CannotOrderNonControlledLower".Translate();
			}
			else if (target.IsValid && !pawn.CanReach(target, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				failStr = "NoPath".Translate();
			}
			else if (pawn.WorkTagIsDisabled(WorkTags.Violent))
			{
				failStr = "IsIncapableOfViolenceLower".Translate(pawn.LabelShort, pawn);
			}
			else if (pawn.meleeVerbs.TryGetMeleeVerb(target.Thing) == null)
			{
				failStr = "Incapable".Translate();
			}
			else if (pawn == target.Thing)
			{
				failStr = "CannotAttackSelf".Translate();
			}
			else if ((target2 = (target.Thing as Pawn)) != null && (pawn.InSameExtraFaction(target2, ExtraFactionType.HomeFaction, null) || pawn.InSameExtraFaction(target2, ExtraFactionType.MiniFaction, null)))
			{
				failStr = "CannotAttackSameFactionMember".Translate();
			}
			else
			{
				Pawn pawn2;
				if ((pawn2 = (target.Thing as Pawn)) == null || !pawn2.RaceProps.Animal || !HistoryEventUtility.IsKillingInnocentAnimal(pawn, pawn2) || new HistoryEvent(HistoryEventDefOf.KilledInnocentAnimal, pawn.Named(HistoryEventArgsNames.Doer)).DoerWillingToDo())
				{
					return delegate()
					{
						Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, target);
						Pawn pawn3 = target.Thing as Pawn;
						if (pawn3 != null)
						{
							job.killIncappedTarget = pawn3.Downed;
						}
						pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
					};
				}
				failStr = "IdeoligionForbids".Translate();
			}
			failStr = failStr.CapitalizeFirst();
			return null;
		}

		// Token: 0x060076D1 RID: 30417 RVA: 0x0029B18C File Offset: 0x0029938C
		public static Action GetAttackAction(Pawn pawn, LocalTargetInfo target, out string failStr)
		{
			if (pawn.equipment.Primary != null && !pawn.equipment.PrimaryEq.PrimaryVerb.verbProps.IsMeleeAttack)
			{
				return FloatMenuUtility.GetRangedAttackAction(pawn, target, out failStr);
			}
			return FloatMenuUtility.GetMeleeAttackAction(pawn, target, out failStr);
		}

		// Token: 0x060076D2 RID: 30418 RVA: 0x0029B1C8 File Offset: 0x002993C8
		public static FloatMenuOption DecoratePrioritizedTask(FloatMenuOption option, Pawn pawn, LocalTargetInfo target, string reservedText = "ReservedBy")
		{
			if (option.action == null)
			{
				return option;
			}
			if (pawn != null && !pawn.CanReserve(target, 1, -1, null, false) && pawn.CanReserve(target, 1, -1, null, true))
			{
				Pawn pawn2 = pawn.Map.reservationManager.FirstRespectedReserver(target, pawn);
				if (pawn2 == null)
				{
					pawn2 = pawn.Map.physicalInteractionReservationManager.FirstReserverOf(target);
				}
				if (pawn2 != null)
				{
					option.Label = option.Label + ": " + reservedText.Translate(pawn2.LabelShort, pawn2);
				}
			}
			if (option.revalidateClickTarget != null && option.revalidateClickTarget != target.Thing)
			{
				Log.ErrorOnce(string.Format("Click target mismatch; {0} vs {1} in {2}", option.revalidateClickTarget, target.Thing, option.Label), 52753118);
			}
			option.revalidateClickTarget = target.Thing;
			return option;
		}
	}
}
