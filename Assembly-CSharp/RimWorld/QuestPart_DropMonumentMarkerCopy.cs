using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010C6 RID: 4294
	public class QuestPart_DropMonumentMarkerCopy : QuestPart
	{
		// Token: 0x17000E8D RID: 3725
		// (get) Token: 0x06005DA8 RID: 23976 RVA: 0x00040EA8 File Offset: 0x0003F0A8
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.mapParent != null)
				{
					yield return this.mapParent;
				}
				if (this.copy != null)
				{
					yield return this.copy;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x06005DA9 RID: 23977 RVA: 0x001DD3B8 File Offset: 0x001DB5B8
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				this.copy = null;
				MonumentMarker arg = signal.args.GetArg<MonumentMarker>("SUBJECT");
				if (arg != null && this.mapParent != null && this.mapParent.HasMap)
				{
					Map map = this.mapParent.Map;
					IntVec3 dropCenter = DropCellFinder.RandomDropSpot(map);
					this.copy = (MonumentMarker)ThingMaker.MakeThing(ThingDefOf.MonumentMarker, null);
					this.copy.sketch = arg.sketch.DeepCopy();
					if (!arg.questTags.NullOrEmpty<string>())
					{
						this.copy.questTags = new List<string>();
						this.copy.questTags.AddRange(arg.questTags);
					}
					DropPodUtility.DropThingsNear(dropCenter, map, Gen.YieldSingle<Thing>(this.copy.MakeMinified()), 110, false, false, true, false);
				}
				if (!this.outSignalResult.NullOrEmpty())
				{
					if (this.copy != null)
					{
						Find.SignalManager.SendSignal(new Signal(this.outSignalResult, this.copy.Named("SUBJECT")));
						return;
					}
					Find.SignalManager.SendSignal(new Signal(this.outSignalResult));
				}
			}
		}

		// Token: 0x06005DAA RID: 23978 RVA: 0x00040EB8 File Offset: 0x0003F0B8
		public override void Cleanup()
		{
			base.Cleanup();
			if (this.destroyOrPassToWorldOnCleanup && this.copy != null)
			{
				QuestPart_DestroyThingsOrPassToWorld.Destroy(this.copy);
			}
		}

		// Token: 0x06005DAB RID: 23979 RVA: 0x001DD4FC File Offset: 0x001DB6FC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.outSignalResult, "outSignalResult", null, false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_References.Look<MonumentMarker>(ref this.copy, "copy", false);
			Scribe_Values.Look<bool>(ref this.destroyOrPassToWorldOnCleanup, "destroyOrPassToWorldOnCleanup", false, false);
		}

		// Token: 0x06005DAC RID: 23980 RVA: 0x00040EDB File Offset: 0x0003F0DB
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			if (Find.AnyPlayerHomeMap != null)
			{
				this.mapParent = Find.RandomPlayerHomeMap.Parent;
			}
		}

		// Token: 0x04003EA2 RID: 16034
		public MapParent mapParent;

		// Token: 0x04003EA3 RID: 16035
		public string inSignal;

		// Token: 0x04003EA4 RID: 16036
		public string outSignalResult;

		// Token: 0x04003EA5 RID: 16037
		public bool destroyOrPassToWorldOnCleanup;

		// Token: 0x04003EA6 RID: 16038
		private MonumentMarker copy;
	}
}
