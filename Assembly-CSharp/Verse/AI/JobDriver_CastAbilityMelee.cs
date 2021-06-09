using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000980 RID: 2432
	public class JobDriver_CastAbilityMelee : JobDriver_CastAbility
	{
		// Token: 0x06003B88 RID: 15240 RVA: 0x0002D97A File Offset: 0x0002BB7A
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			Ability ability = ((Verb_CastAbility)this.job.verbToUse).ability;
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOn(() => !ability.CanApplyOn(this.job.targetA));
			yield return Toils_Combat.CastVerb(TargetIndex.A, TargetIndex.B, false);
			yield break;
		}

		// Token: 0x06003B89 RID: 15241 RVA: 0x0002D98A File Offset: 0x0002BB8A
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
