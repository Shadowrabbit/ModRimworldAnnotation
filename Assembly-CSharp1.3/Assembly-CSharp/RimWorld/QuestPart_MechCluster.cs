using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B86 RID: 2950
	public class QuestPart_MechCluster : QuestPart
	{
		// Token: 0x17000C16 RID: 3094
		// (get) Token: 0x060044FE RID: 17662 RVA: 0x0016DBD1 File Offset: 0x0016BDD1
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

		// Token: 0x060044FF RID: 17663 RVA: 0x0016DBE4 File Offset: 0x0016BDE4
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.mapParent != null && this.mapParent.HasMap)
			{
				this.spawnedClusterPos = this.dropSpot;
				if (this.spawnedClusterPos == IntVec3.Invalid)
				{
					this.spawnedClusterPos = MechClusterUtility.FindClusterPosition(this.mapParent.Map, this.sketch, 100, 0.5f);
				}
				if (this.spawnedClusterPos == IntVec3.Invalid)
				{
					return;
				}
				MechClusterUtility.SpawnCluster(this.spawnedClusterPos, this.mapParent.Map, this.sketch, true, false, this.tag);
				Find.LetterStack.ReceiveLetter("LetterLabelMechClusterArrived".Translate(), "LetterMechClusterArrived".Translate(), LetterDefOf.ThreatBig, new TargetInfo(this.spawnedClusterPos, this.mapParent.Map, false), null, this.quest, null, null);
			}
		}

		// Token: 0x06004500 RID: 17664 RVA: 0x0016DCEC File Offset: 0x0016BEEC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.tag, "tag", null, false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Deep.Look<MechClusterSketch>(ref this.sketch, "sketch", Array.Empty<object>());
			Scribe_Values.Look<IntVec3>(ref this.dropSpot, "dropSpot", default(IntVec3), false);
			Scribe_Values.Look<IntVec3>(ref this.spawnedClusterPos, "spawnedClusterPos", default(IntVec3), false);
		}

		// Token: 0x040029E1 RID: 10721
		public MechClusterSketch sketch;

		// Token: 0x040029E2 RID: 10722
		public string inSignal;

		// Token: 0x040029E3 RID: 10723
		public string tag;

		// Token: 0x040029E4 RID: 10724
		public MapParent mapParent;

		// Token: 0x040029E5 RID: 10725
		public IntVec3 dropSpot = IntVec3.Invalid;

		// Token: 0x040029E6 RID: 10726
		private IntVec3 spawnedClusterPos = IntVec3.Invalid;
	}
}
