using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000777 RID: 1911
	public abstract class JobGiver_AIDefendPawn : JobGiver_AIFightEnemy
	{
		// Token: 0x060034AC RID: 13484 RVA: 0x0012A938 File Offset: 0x00128B38
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_AIDefendPawn jobGiver_AIDefendPawn = (JobGiver_AIDefendPawn)base.DeepCopy(resolve);
			jobGiver_AIDefendPawn.attackMeleeThreatEvenIfNotHostile = this.attackMeleeThreatEvenIfNotHostile;
			return jobGiver_AIDefendPawn;
		}

		// Token: 0x060034AD RID: 13485
		protected abstract Pawn GetDefendee(Pawn pawn);

		// Token: 0x060034AE RID: 13486 RVA: 0x0012A954 File Offset: 0x00128B54
		protected override IntVec3 GetFlagPosition(Pawn pawn)
		{
			Pawn defendee = this.GetDefendee(pawn);
			if (defendee.Spawned || defendee.CarriedBy != null)
			{
				return defendee.PositionHeld;
			}
			return IntVec3.Invalid;
		}

		// Token: 0x060034AF RID: 13487 RVA: 0x0012A988 File Offset: 0x00128B88
		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn defendee = this.GetDefendee(pawn);
			if (defendee == null)
			{
				Log.Error(base.GetType() + " has null defendee. pawn=" + pawn.ToStringSafe<Pawn>());
				return null;
			}
			Pawn carriedBy = defendee.CarriedBy;
			if (carriedBy != null)
			{
				if (!pawn.CanReach(carriedBy, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					return null;
				}
			}
			else if (!defendee.Spawned || !pawn.CanReach(defendee, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return null;
			}
			return base.TryGiveJob(pawn);
		}

		// Token: 0x060034B0 RID: 13488 RVA: 0x0012AA00 File Offset: 0x00128C00
		protected override Thing FindAttackTarget(Pawn pawn)
		{
			if (this.attackMeleeThreatEvenIfNotHostile)
			{
				Pawn defendee = this.GetDefendee(pawn);
				if (defendee.Spawned && !defendee.InMentalState && defendee.mindState.meleeThreat != null && defendee.mindState.meleeThreat != pawn && pawn.CanReach(defendee.mindState.meleeThreat, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					return defendee.mindState.meleeThreat;
				}
			}
			return base.FindAttackTarget(pawn);
		}

		// Token: 0x060034B1 RID: 13489 RVA: 0x0012AA78 File Offset: 0x00128C78
		protected override bool TryFindShootingPosition(Pawn pawn, out IntVec3 dest)
		{
			Verb verb = pawn.TryGetAttackVerb(null, !pawn.IsColonist);
			if (verb == null)
			{
				dest = IntVec3.Invalid;
				return false;
			}
			return CastPositionFinder.TryFindCastPosition(new CastPositionRequest
			{
				caster = pawn,
				target = pawn.mindState.enemyTarget,
				verb = verb,
				maxRangeFromTarget = 9999f,
				locus = this.GetDefendee(pawn).PositionHeld,
				maxRangeFromLocus = this.GetFlagRadius(pawn),
				wantCoverFromTarget = (verb.verbProps.range > 7f),
				maxRegions = 50
			}, out dest);
		}

		// Token: 0x04001E5F RID: 7775
		private bool attackMeleeThreatEvenIfNotHostile;
	}
}
