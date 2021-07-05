using System;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x020004FA RID: 1274
	public class Verb_LaunchProjectileStatic : Verb_LaunchProjectile
	{
		// Token: 0x1700078D RID: 1933
		// (get) Token: 0x060026A9 RID: 9897 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool MultiSelect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700078E RID: 1934
		// (get) Token: 0x060026AA RID: 9898 RVA: 0x000EFE50 File Offset: 0x000EE050
		public override Texture2D UIIcon
		{
			get
			{
				return TexCommand.Attack;
			}
		}

		// Token: 0x060026AB RID: 9899 RVA: 0x000EFE57 File Offset: 0x000EE057
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			return base.ValidateTarget(target, true) && ReloadableUtility.CanUseConsideringQueuedJobs(this.CasterPawn, base.EquipmentSource, true);
		}

		// Token: 0x060026AC RID: 9900 RVA: 0x000EFE7C File Offset: 0x000EE07C
		public override void OrderForceTarget(LocalTargetInfo target)
		{
			Job job = JobMaker.MakeJob(JobDefOf.UseVerbOnThingStatic, target);
			job.verbToUse = this;
			this.CasterPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
		}
	}
}
