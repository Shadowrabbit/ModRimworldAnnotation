using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000715 RID: 1813
	public class Command_Target : Command
	{
		// Token: 0x06002DD8 RID: 11736 RVA: 0x000241CD File Offset: 0x000223CD
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
			Find.Targeter.BeginTargeting(this.targetingParams, delegate(LocalTargetInfo target)
			{
				this.action(target.Thing);
			}, null, null, null);
		}

		// Token: 0x06002DD9 RID: 11737 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool InheritInteractionsFrom(Gizmo other)
		{
			return false;
		}

		// Token: 0x04001F3D RID: 7997
		public Action<Thing> action;

		// Token: 0x04001F3E RID: 7998
		public TargetingParameters targetingParams;
	}
}
