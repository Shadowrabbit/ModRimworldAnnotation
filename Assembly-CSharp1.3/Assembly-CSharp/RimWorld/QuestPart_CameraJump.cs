using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B67 RID: 2919
	public class QuestPart_CameraJump : QuestPart
	{
		// Token: 0x17000BF9 RID: 3065
		// (get) Token: 0x06004443 RID: 17475 RVA: 0x0016A7A9 File Offset: 0x001689A9
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				GlobalTargetInfo globalTargetInfo2 = this.lookTargets.TryGetPrimaryTarget();
				if (globalTargetInfo2.IsValid)
				{
					yield return globalTargetInfo2;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x06004444 RID: 17476 RVA: 0x0016A7BC File Offset: 0x001689BC
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				LookTargets lookTargets = this.lookTargets;
				if (this.getLookTargetsFromSignal && !lookTargets.IsValid())
				{
					SignalArgsUtility.TryGetLookTargets(signal.args, "SUBJECT", out lookTargets);
				}
				if (lookTargets.IsValid())
				{
					if (this.select)
					{
						CameraJumper.TryJumpAndSelect(lookTargets.TryGetPrimaryTarget());
						return;
					}
					CameraJumper.TryJump(lookTargets.TryGetPrimaryTarget());
				}
			}
		}

		// Token: 0x06004445 RID: 17477 RVA: 0x0016A834 File Offset: 0x00168A34
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Deep.Look<LookTargets>(ref this.lookTargets, "lookTargets", Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.getLookTargetsFromSignal, "getLookTargetsFromSignal", true, false);
			Scribe_Values.Look<bool>(ref this.select, "select", true, false);
		}

		// Token: 0x06004446 RID: 17478 RVA: 0x0016A894 File Offset: 0x00168A94
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			this.lookTargets = Find.Maps.SelectMany((Map x) => x.mapPawns.FreeColonistsSpawned).RandomElementWithFallback(null);
		}

		// Token: 0x0400296D RID: 10605
		public string inSignal;

		// Token: 0x0400296E RID: 10606
		public LookTargets lookTargets;

		// Token: 0x0400296F RID: 10607
		public bool getLookTargetsFromSignal = true;

		// Token: 0x04002970 RID: 10608
		public bool select = true;
	}
}
