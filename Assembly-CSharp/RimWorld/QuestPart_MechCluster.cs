using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010EA RID: 4330
	public class QuestPart_MechCluster : QuestPart
	{
		// Token: 0x17000EB0 RID: 3760
		// (get) Token: 0x06005E8E RID: 24206 RVA: 0x00041746 File Offset: 0x0003F946
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.spawnedClusterPos.IsValid && this.mapParent != null && this.mapParent.HasMap)
				{
					yield return new GlobalTargetInfo(this.spawnedClusterPos, this.mapParent.Map, false);
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x06005E8F RID: 24207 RVA: 0x001DFC8C File Offset: 0x001DDE8C
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.mapParent != null && this.mapParent.HasMap)
			{
				this.spawnedClusterPos = MechClusterUtility.FindClusterPosition(this.mapParent.Map, this.sketch, 100, 0.5f);
				if (this.spawnedClusterPos == IntVec3.Invalid)
				{
					return;
				}
				MechClusterUtility.SpawnCluster(this.spawnedClusterPos, this.mapParent.Map, this.sketch, true, false, this.tag);
				Find.LetterStack.ReceiveLetter("LetterLabelMechClusterArrived".Translate(), "LetterMechClusterArrived".Translate(), LetterDefOf.ThreatBig, new TargetInfo(this.spawnedClusterPos, this.mapParent.Map, false), null, this.quest, null, null);
			}
		}

		// Token: 0x06005E90 RID: 24208 RVA: 0x001DFD74 File Offset: 0x001DDF74
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.tag, "tag", null, false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Deep.Look<MechClusterSketch>(ref this.sketch, "sketch", Array.Empty<object>());
			Scribe_Values.Look<IntVec3>(ref this.spawnedClusterPos, "spawnedClusterPos", default(IntVec3), false);
		}

		// Token: 0x04003F34 RID: 16180
		public MechClusterSketch sketch;

		// Token: 0x04003F35 RID: 16181
		public string inSignal;

		// Token: 0x04003F36 RID: 16182
		public string tag;

		// Token: 0x04003F37 RID: 16183
		public MapParent mapParent;

		// Token: 0x04003F38 RID: 16184
		private IntVec3 spawnedClusterPos = IntVec3.Invalid;
	}
}
