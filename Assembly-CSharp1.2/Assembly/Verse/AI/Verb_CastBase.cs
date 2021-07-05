using System;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000964 RID: 2404
	public abstract class Verb_CastBase : Verb
	{
		// Token: 0x17000974 RID: 2420
		// (get) Token: 0x06003AD7 RID: 15063 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool MultiSelect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003AD8 RID: 15064 RVA: 0x0002AC8F File Offset: 0x00028E8F
		public override bool ValidateTarget(LocalTargetInfo target)
		{
			return base.ValidateTarget(target) && ReloadableUtility.CanUseConsideringQueuedJobs(this.CasterPawn, base.EquipmentSource, true);
		}

		// Token: 0x06003AD9 RID: 15065 RVA: 0x0015FF04 File Offset: 0x0015E104
		public override void OrderForceTarget(LocalTargetInfo target)
		{
			Job job = JobMaker.MakeJob(JobDefOf.UseVerbOnThingStatic, target);
			job.verbToUse = this;
			this.CasterPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		}

		// Token: 0x06003ADA RID: 15066 RVA: 0x0016BBF0 File Offset: 0x00169DF0
		public override void OnGUI(LocalTargetInfo target)
		{
			if (this.CanHitTarget(target) && this.verbProps.targetParams.CanTarget(target.ToTargetInfo(this.caster.Map)))
			{
				base.OnGUI(target);
				return;
			}
			GenUI.DrawMouseAttachment(TexCommand.CannotShoot);
		}

		// Token: 0x06003ADB RID: 15067 RVA: 0x0016BC3C File Offset: 0x00169E3C
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

		// Token: 0x06003ADD RID: 15069 RVA: 0x0002D3CC File Offset: 0x0002B5CC
		[CompilerGenerated]
		private bool <DrawHighlight>g__CanTarget|5_0(IntVec3 c)
		{
			return GenSight.LineOfSight(this.caster.Position, c, this.caster.Map, false, null, 0, 0);
		}
	}
}
