using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000063 RID: 99
	public class JobDriver_CastAbilityWorld : JobDriver
	{
		// Token: 0x0600041A RID: 1050 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x00015D12 File Offset: 0x00013F12
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

		// Token: 0x0600041C RID: 1052 RVA: 0x00015D22 File Offset: 0x00013F22
		public override string GetReport()
		{
			return "UsingVerb".Translate(this.job.ability.def.label, this.job.globalTarget.Label);
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x00015D62 File Offset: 0x00013F62
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
