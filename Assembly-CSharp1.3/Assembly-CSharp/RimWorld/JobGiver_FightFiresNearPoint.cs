using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020007AA RID: 1962
	internal class JobGiver_FightFiresNearPoint : ThinkNode_JobGiver
	{
		// Token: 0x06003560 RID: 13664 RVA: 0x0012DE8C File Offset: 0x0012C08C
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_FightFiresNearPoint jobGiver_FightFiresNearPoint = (JobGiver_FightFiresNearPoint)base.DeepCopy(resolve);
			jobGiver_FightFiresNearPoint.maxDistFromPoint = this.maxDistFromPoint;
			return jobGiver_FightFiresNearPoint;
		}

		// Token: 0x06003561 RID: 13665 RVA: 0x0012DEA8 File Offset: 0x0012C0A8
		protected override Job TryGiveJob(Pawn pawn)
		{
			Thing thing = GenClosest.ClosestThingReachable(pawn.GetLord().CurLordToil.FlagLoc, pawn.Map, ThingRequest.ForDef(ThingDefOf.Fire), PathEndMode.Touch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false), this.maxDistFromPoint, JobGiver_FightFiresNearPoint.FireValidator(pawn), null, 0, -1, false, RegionType.Set_Passable, false);
			if (thing != null)
			{
				return JobMaker.MakeJob(JobDefOf.BeatFire, thing);
			}
			return null;
		}

		// Token: 0x06003562 RID: 13666 RVA: 0x0012DF0F File Offset: 0x0012C10F
		public static Predicate<Thing> FireValidator(Pawn pawn)
		{
			return (Thing t) => !(((AttachableThing)t).parent is Pawn) && pawn.CanReserve(t, 1, -1, null, false) && !pawn.WorkTagIsDisabled(WorkTags.Firefighting);
		}

		// Token: 0x04001E8F RID: 7823
		public float maxDistFromPoint = -1f;
	}
}
