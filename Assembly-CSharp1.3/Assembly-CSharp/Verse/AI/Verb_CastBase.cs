using System;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000588 RID: 1416
	public abstract class Verb_CastBase : Verb
	{
		// Token: 0x17000830 RID: 2096
		// (get) Token: 0x0600297E RID: 10622 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool MultiSelect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600297F RID: 10623 RVA: 0x000EFE57 File Offset: 0x000EE057
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			return base.ValidateTarget(target, true) && ReloadableUtility.CanUseConsideringQueuedJobs(this.CasterPawn, base.EquipmentSource, true);
		}

		// Token: 0x06002980 RID: 10624 RVA: 0x000FAE48 File Offset: 0x000F9048
		public override void OrderForceTarget(LocalTargetInfo target)
		{
			Job job = JobMaker.MakeJob(JobDefOf.UseVerbOnThingStatic, target);
			job.verbToUse = this;
			this.CasterPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
		}

		// Token: 0x06002981 RID: 10625 RVA: 0x000FAE84 File Offset: 0x000F9084
		public override void OnGUI(LocalTargetInfo target)
		{
			if (this.CanHitTarget(target) && this.verbProps.targetParams.CanTarget(target.ToTargetInfo(this.caster.Map), null))
			{
				base.OnGUI(target);
				return;
			}
			GenUI.DrawMouseAttachment(TexCommand.CannotShoot);
		}

		// Token: 0x06002982 RID: 10626 RVA: 0x000FAED4 File Offset: 0x000F90D4
		public override void DrawHighlight(LocalTargetInfo target)
		{
			if (target.IsValid && this.CanHitTarget(target))
			{
				GenDraw.DrawTargetHighlightWithLayer(target.CenterVector3, AltitudeLayer.MetaOverlays);
				base.DrawHighlightFieldRadiusAroundTarget(target);
			}
			if (this.verbProps.requireLineOfSight)
			{
				GenDraw.DrawRadiusRing(this.caster.Position, this.EffectiveRange, Color.white, new Func<IntVec3, bool>(this.<DrawHighlight>g__CanTarget|5_0));
				return;
			}
			GenDraw.DrawRadiusRing(this.caster.Position, this.EffectiveRange);
		}

		// Token: 0x06002984 RID: 10628 RVA: 0x000FAF53 File Offset: 0x000F9153
		[CompilerGenerated]
		private bool <DrawHighlight>g__CanTarget|5_0(IntVec3 c)
		{
			return GenSight.LineOfSight(this.caster.Position, c, this.caster.Map, false, null, 0, 0);
		}
	}
}
