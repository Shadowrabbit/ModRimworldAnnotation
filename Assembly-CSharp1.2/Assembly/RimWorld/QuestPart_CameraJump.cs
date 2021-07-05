using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010A7 RID: 4263
	public class QuestPart_CameraJump : QuestPart
	{
		// Token: 0x17000E6D RID: 3693
		// (get) Token: 0x06005CF0 RID: 23792 RVA: 0x00040794 File Offset: 0x0003E994
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

		// Token: 0x06005CF1 RID: 23793 RVA: 0x001DB9D0 File Offset: 0x001D9BD0
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

		// Token: 0x06005CF2 RID: 23794 RVA: 0x001DBA48 File Offset: 0x001D9C48
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Deep.Look<LookTargets>(ref this.lookTargets, "lookTargets", Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.getLookTargetsFromSignal, "getLookTargetsFromSignal", true, false);
			Scribe_Values.Look<bool>(ref this.select, "select", true, false);
		}

		// Token: 0x06005CF3 RID: 23795 RVA: 0x001DBAA8 File Offset: 0x001D9CA8
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			this.lookTargets = Find.Maps.SelectMany((Map x) => x.mapPawns.FreeColonistsSpawned).RandomElementWithFallback(null);
		}

		// Token: 0x04003E38 RID: 15928
		public string inSignal;

		// Token: 0x04003E39 RID: 15929
		public LookTargets lookTargets;

		// Token: 0x04003E3A RID: 15930
		public bool getLookTargetsFromSignal = true;

		// Token: 0x04003E3B RID: 15931
		public bool select = true;
	}
}
