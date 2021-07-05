using System;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x020008AB RID: 2219
	public class Verb_LaunchProjectileStatic : Verb_LaunchProjectile
	{
		// Token: 0x170008AB RID: 2219
		// (get) Token: 0x06003736 RID: 14134 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool MultiSelect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170008AC RID: 2220
		// (get) Token: 0x06003737 RID: 14135 RVA: 0x0002AC88 File Offset: 0x00028E88
		public override Texture2D UIIcon
		{
			get
			{
				return TexCommand.Attack;
			}
		}

		// Token: 0x06003738 RID: 14136 RVA: 0x0002AC8F File Offset: 0x00028E8F
		public override bool ValidateTarget(LocalTargetInfo target)
		{
			return base.ValidateTarget(target) && ReloadableUtility.CanUseConsideringQueuedJobs(this.CasterPawn, base.EquipmentSource, true);
		}

		// Token: 0x06003739 RID: 14137 RVA: 0x0015FF04 File Offset: 0x0015E104
		public override void OrderForceTarget(LocalTargetInfo target)
		{
			Job job = JobMaker.MakeJob(JobDefOf.UseVerbOnThingStatic, target);
			job.verbToUse = this;
			this.CasterPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		}
	}
}
