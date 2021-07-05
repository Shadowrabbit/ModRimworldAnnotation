using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000598 RID: 1432
	public class JobDriver_CastAbility : JobDriver_CastVerbOnce
	{
		// Token: 0x060029DA RID: 10714 RVA: 0x000FCF61 File Offset: 0x000FB161
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return new Toil
			{
				initAction = delegate()
				{
					this.pawn.pather.StopDead();
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			Toil toil = Toils_Combat.CastVerb(TargetIndex.A, TargetIndex.B, false);
			if (this.job.ability != null && this.job.ability.def.showCastingProgressBar && this.job.verbToUse != null)
			{
				toil.WithProgressBar(TargetIndex.A, () => this.job.verbToUse.WarmupProgress, false, -0.5f, false);
			}
			yield return toil;
			yield break;
		}

		// Token: 0x060029DB RID: 10715 RVA: 0x00015D62 File Offset: 0x00013F62
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

		// Token: 0x060029DC RID: 10716 RVA: 0x000FCF74 File Offset: 0x000FB174
		public override string GetReport()
		{
			string text;
			if (this.job.ability != null && !this.job.ability.def.targetRequired)
			{
				text = "UsingVerbNoTarget".Translate(this.job.verbToUse.ReportLabel);
			}
			else
			{
				text = base.GetReport();
			}
			if (this.job.ability != null && this.job.ability.def.showCastingProgressBar)
			{
				text += " " + "DurationLeft".Translate(this.job.verbToUse.WarmupTicksLeft.ToStringSecondsFromTicks()) + ".";
			}
			return text;
		}
	}
}
