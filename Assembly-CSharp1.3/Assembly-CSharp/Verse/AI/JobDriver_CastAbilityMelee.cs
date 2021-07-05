using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200059A RID: 1434
	public class JobDriver_CastAbilityMelee : JobDriver_CastAbility
	{
		// Token: 0x060029E2 RID: 10722 RVA: 0x000FD07E File Offset: 0x000FB27E
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			Ability ability = ((Verb_CastAbility)this.job.verbToUse).ability;
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOn(() => !ability.CanApplyOn(this.job.targetA));
			yield return Toils_Combat.CastVerb(TargetIndex.A, TargetIndex.B, false);
			yield break;
		}

		// Token: 0x060029E3 RID: 10723 RVA: 0x000FD08E File Offset: 0x000FB28E
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			Ability ability = this.job.ability;
			if (ability == null)
			{
				return;
			}
			ability.Notify_StartedCasting();
		}
	}
}
