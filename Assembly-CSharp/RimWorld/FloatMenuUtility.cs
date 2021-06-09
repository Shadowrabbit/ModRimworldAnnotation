﻿using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001AD4 RID: 6868
	public static class FloatMenuUtility
	{
		// Token: 0x0600974C RID: 38732 RVA: 0x002C6418 File Offset: 0x002C4618
		public static void MakeMenu<T>(IEnumerable<T> objects, Func<T, string> labelGetter, Func<T, Action> actionGetter)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (T arg in objects)
			{
				list.Add(new FloatMenuOption(labelGetter(arg), actionGetter(arg), MenuOptionPriority.Default, null, null, 0f, null, null));
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x0600974D RID: 38733 RVA: 0x002C6494 File Offset: 0x002C4694
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
			else
			{
				Pawn target2;
				if ((target2 = (target.Thing as Pawn)) == null || (!pawn.InSameExtraFaction(target2, ExtraFactionType.HomeFaction, null) && !pawn.InSameExtraFaction(target2, ExtraFactionType.MiniFaction, null)))
				{
					return delegate()
					{
						Job job = JobMaker.MakeJob(JobDefOf.AttackStatic, target);
						pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
					};
				}
				failStr = "CannotAttackSameFactionMember".Translate();
			}
			failStr = failStr.CapitalizeFirst();
			return null;
		}

		// Token: 0x0600974E RID: 38734 RVA: 0x002C66F0 File Offset: 0x002C48F0
		public static Action GetMeleeAttackAction(Pawn pawn, LocalTargetInfo target, out string failStr)
		{
			failStr = "";
			if (!pawn.Drafted)
			{
				failStr = "IsNotDraftedLower".Translate(pawn.LabelShort, pawn);
			}
			else if (!pawn.IsColonistPlayerControlled)
			{
				failStr = "CannotOrderNonControlledLower".Translate();
			}
			else if (target.IsValid && !pawn.CanReach(target, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
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
			else
			{
				Pawn target2;
				if ((target2 = (target.Thing as Pawn)) == null || (!pawn.InSameExtraFaction(target2, ExtraFactionType.HomeFaction, null) && !pawn.InSameExtraFaction(target2, ExtraFactionType.MiniFaction, null)))
				{
					return delegate()
					{
						Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, target);
						Pawn pawn2 = target.Thing as Pawn;
						if (pawn2 != null)
						{
							job.killIncappedTarget = pawn2.Downed;
						}
						pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
					};
				}
				failStr = "CannotAttackSameFactionMember".Translate();
			}
			failStr = failStr.CapitalizeFirst();
			return null;
		}

		// Token: 0x0600974F RID: 38735 RVA: 0x00064DDD File Offset: 0x00062FDD
		public static Action GetAttackAction(Pawn pawn, LocalTargetInfo target, out string failStr)
		{
			if (pawn.equipment.Primary != null && !pawn.equipment.PrimaryEq.PrimaryVerb.verbProps.IsMeleeAttack)
			{
				return FloatMenuUtility.GetRangedAttackAction(pawn, target, out failStr);
			}
			return FloatMenuUtility.GetMeleeAttackAction(pawn, target, out failStr);
		}

		// Token: 0x06009750 RID: 38736 RVA: 0x002C68A8 File Offset: 0x002C4AA8
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
				Log.ErrorOnce(string.Format("Click target mismatch; {0} vs {1} in {2}", option.revalidateClickTarget, target.Thing, option.Label), 52753118, false);
			}
			option.revalidateClickTarget = target.Thing;
			return option;
		}
	}
}
