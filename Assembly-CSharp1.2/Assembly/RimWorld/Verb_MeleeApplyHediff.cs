using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D8A RID: 7562
	public class Verb_MeleeApplyHediff : Verb_MeleeAttack
	{
		// Token: 0x0600A44D RID: 42061 RVA: 0x002FD310 File Offset: 0x002FB510
		protected override DamageWorker.DamageResult ApplyMeleeDamageToTarget(LocalTargetInfo target)
		{
			DamageWorker.DamageResult damageResult = new DamageWorker.DamageResult();
			if (this.tool == null)
			{
				Log.ErrorOnce("Attempted to apply melee hediff without a tool", 38381735, false);
				return damageResult;
			}
			Pawn pawn = target.Thing as Pawn;
			if (pawn == null)
			{
				Log.ErrorOnce("Attempted to apply melee hediff without pawn target", 78330053, false);
				return damageResult;
			}
			foreach (BodyPartRecord part in pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, this.verbProps.bodypartTagTarget, null))
			{
				damageResult.AddHediff(pawn.health.AddHediff(this.tool.hediff, part, null, null));
				damageResult.AddPart(pawn, part);
				damageResult.wounded = true;
			}
			return damageResult;
		}

		// Token: 0x0600A44E RID: 42062 RVA: 0x0001D8FB File Offset: 0x0001BAFB
		public override bool IsUsableOn(Thing target)
		{
			return target is Pawn;
		}
	}
}
