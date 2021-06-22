using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
    // Token: 0x02000C1B RID: 3099
    public static class Toils_Misc
    {
        // Token: 0x060048F2 RID: 18674 RVA: 0x0019BB18 File Offset: 0x00199D18
        public static Toil Learn(SkillDef skill, float xp)
        {
            Toil toil = new Toil();
            toil.initAction = delegate()
            {
                toil.actor.skills.Learn(skill, xp, false);
            };
            return toil;
        }

        // Token: 0x060048F3 RID: 18675 RVA: 0x0019BB64 File Offset: 0x00199D64
        public static Toil SetForbidden(TargetIndex ind, bool forbidden)
        {
            Toil toil = new Toil();
            toil.initAction = delegate()
            {
                toil.actor.CurJob.GetTarget(ind).Thing.SetForbidden(forbidden, true);
            };
            return toil;
        }

        // Token: 0x060048F4 RID: 18676 RVA: 0x0019BBB0 File Offset: 0x00199DB0
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

        // Token: 0x060048F5 RID: 18677 RVA: 0x0019BBE8 File Offset: 0x00199DE8
        public static Toil ThrowColonistAttackingMote(TargetIndex target)
        {
            Toil toil = new Toil();
            toil.initAction = delegate()
            {
                Pawn actor = toil.actor;
                Job curJob = actor.CurJob;
                if (actor.playerSettings != null && actor.playerSettings.UsesConfigurableHostilityResponse && !actor.Drafted &&
                    !actor.InMentalState && !curJob.playerForced && actor.HostileTo(curJob.GetTarget(target).Thing))
                {
                    MoteMaker.MakeColonistActionOverlay(actor, ThingDefOf.Mote_ColonistAttacking);
                }
            };
            return toil;
        }

        // Token: 0x060048F6 RID: 18678 RVA: 0x0019BC2C File Offset: 0x00199E2C
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
                    }), false);
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
                    if (c.Standable(actor.Map) && actor.CanReserve(c, 1, -1, null, false) &&
                        actor.CanReach(c, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
                    {
                        goto Block_7;
                    }
                }
                Log.Error(actor + " could not find standable cell adjacent to " + target, false);
                actor.jobs.curDriver.EndJobWith(JobCondition.Errored);
                return;
                Block_7:
                curJob.SetTarget(cellInd, c);
            };
            return findCell;
        }
    }
}
