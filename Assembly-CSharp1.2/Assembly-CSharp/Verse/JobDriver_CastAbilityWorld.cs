using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x020000AB RID: 171
	public class JobDriver_CastAbilityWorld : JobDriver
	{
		// Token: 0x06000573 RID: 1395 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x0000A9BE File Offset: 0x00008BBE
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_General.Do(delegate
			{
				this.pawn.stances.SetStance(new Stance_Warmup(this.job.ability.def.verbProperties.warmupTime.SecondsToTicks(), null, this.job.ability.verb));
			});
			yield return Toils_General.Do(delegate
			{
				this.job.ability.Activate(this.job.globalTarget);
			});
			yield break;
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x0000A9CE File Offset: 0x00008BCE
		public override string GetReport()
		{
			return "UsingVerb".Translate(this.job.ability.def.label, this.job.globalTarget.Label);
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x0000AA0E File Offset: 0x00008C0E
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
