using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200073F RID: 1855
	public static class Toils_Misc
	{
		// Token: 0x0600336E RID: 13166 RVA: 0x001252B4 File Offset: 0x001234B4
		public static Toil Learn(SkillDef skill, float xp)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				toil.actor.skills.Learn(skill, xp, false);
			};
			return toil;
		}

		// Token: 0x0600336F RID: 13167 RVA: 0x00125300 File Offset: 0x00123500
		public static Toil SetForbidden(TargetIndex ind, bool forbidden)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				toil.actor.CurJob.GetTarget(ind).Thing.SetForbidden(forbidden, true);
			};
			return toil;
		}

		// Token: 0x06003370 RID: 13168 RVA: 0x0012534C File Offset: 0x0012354C
		public static Toil TakeItemFromInventoryToCarrier(Pawn pawn, TargetIndex itemInd)
		{
			return new Toil
			{
				initAction = delegate()
				{
					Job curJob = pawn.CurJob;
					Thing thing = (Thing)curJob.GetTarget(itemInd);
					int count = Mathf.Min(thing.stackCount, curJob.count);
					pawn.inventory.innerContainer.TryTransferToContainer(thing, pawn.carryTracker.innerContainer, count, true);
					curJob.SetTarget(itemInd, pawn.carryTracker.CarriedThing);
				}
			};
		}

		// Token: 0x06003371 RID: 13169 RVA: 0x00125384 File Offset: 0x00123584
		public static Toil ThrowColonistAttackingMote(TargetIndex target)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.CurJob;
				if (actor.playerSettings != null && actor.playerSettings.UsesConfigurableHostilityResponse && !actor.Drafted && !actor.InMentalState && !curJob.playerForced && actor.HostileTo(curJob.GetTarget(target).Thing))
				{
					MoteMaker.MakeColonistActionOverlay(actor, ThingDefOf.Mote_ColonistAttacking);
				}
			};
			return toil;
		}

		// Token: 0x06003372 RID: 13170 RVA: 0x001253C8 File Offset: 0x001235C8
		public static Toil FindRandomAdjacentReachableCell(TargetIndex adjacentToInd, TargetIndex cellInd)
		{
			Toil findCell = new Toil();
			findCell.initAction = delegate()
			{
				Pawn actor = findCell.actor;
				Job curJob = actor.CurJob;
				LocalTargetInfo target = curJob.GetTarget(adjacentToInd);
				if (target.HasThing && (!target.Thing.Spawned || target.Thing.Map != actor.Map))
				{
					Log.Error(string.Concat(new object[]
					{
						actor,
						" could not find standable cell adjacent to ",
						target,
						" because this thing is either unspawned or spawned somewhere else."
					}));
					actor.jobs.curDriver.EndJobWith(JobCondition.Errored);
					return;
				}
				int num = 0;
				IntVec3 c;
				for (;;)
				{
					num++;
					if (num > 100)
					{
						break;
					}
					if (target.HasThing)
					{
						c = target.Thing.RandomAdjacentCell8Way();
					}
					else
					{
						c = target.Cell.RandomAdjacentCell8Way();
					}
					if (c.Standable(actor.Map) && actor.CanReserve(c, 1, -1, null, false) && actor.CanReach(c, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
					{
						goto Block_7;
					}
				}
				Log.Error(actor + " could not find standable cell adjacent to " + target);
				actor.jobs.curDriver.EndJobWith(JobCondition.Errored);
				return;
				Block_7:
				curJob.SetTarget(cellInd, c);
			};
			return findCell;
		}
	}
}
