using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020003F9 RID: 1017
	public class Command_Target : Command
	{
		// Token: 0x06001E96 RID: 7830 RVA: 0x000BF477 File Offset: 0x000BD677
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
			Find.Targeter.BeginTargeting(this.targetingParams, delegate(LocalTargetInfo target)
			{
				this.action(target);
			}, null, null, null);
		}

		// Token: 0x06001E97 RID: 7831 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool InheritInteractionsFrom(Gizmo other)
		{
			return false;
		}

		// Token: 0x0400129D RID: 4765
		public Action<LocalTargetInfo> action;

		// Token: 0x0400129E RID: 4766
		public TargetingParameters targetingParams;
	}
}
