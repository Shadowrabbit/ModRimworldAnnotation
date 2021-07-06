using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200097E RID: 2430
	public class JobDriver_CastAbility : JobDriver_CastVerbOnce
	{
		// Token: 0x06003B7A RID: 15226 RVA: 0x0002D914 File Offset: 0x0002BB14
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
				toil.WithProgressBar(TargetIndex.A, () => this.job.verbToUse.WarmupProgress, false, -0.5f);
			}
			yield return toil;
			yield break;
		}

		// Token: 0x06003B7B RID: 15227 RVA: 0x0000AA0E File Offset: 0x00008C0E
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

		// Token: 0x06003B7C RID: 15228 RVA: 0x0016E190 File Offset: 0x0016C390
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
