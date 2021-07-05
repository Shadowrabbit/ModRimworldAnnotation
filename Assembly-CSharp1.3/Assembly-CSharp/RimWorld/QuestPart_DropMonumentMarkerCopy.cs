using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B73 RID: 2931
	public class QuestPart_DropMonumentMarkerCopy : QuestPart
	{
		// Token: 0x17000C07 RID: 3079
		// (get) Token: 0x06004491 RID: 17553 RVA: 0x0016BB65 File Offset: 0x00169D65
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

		// Token: 0x06004492 RID: 17554 RVA: 0x0016BB78 File Offset: 0x00169D78
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
					IntVec3 dropCenter = DropCellFinder.RandomDropSpot(map, true);
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

		// Token: 0x06004493 RID: 17555 RVA: 0x0016BCBA File Offset: 0x00169EBA
		public override void Cleanup()
		{
			base.Cleanup();
			if (this.destroyOrPassToWorldOnCleanup && this.copy != null)
			{
				QuestPart_DestroyThingsOrPassToWorld.Destroy(this.copy);
			}
		}

		// Token: 0x06004494 RID: 17556 RVA: 0x0016BCE0 File Offset: 0x00169EE0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.outSignalResult, "outSignalResult", null, false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_References.Look<MonumentMarker>(ref this.copy, "copy", false);
			Scribe_Values.Look<bool>(ref this.destroyOrPassToWorldOnCleanup, "destroyOrPassToWorldOnCleanup", false, false);
		}

		// Token: 0x06004495 RID: 17557 RVA: 0x0016BD4B File Offset: 0x00169F4B
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			if (Find.AnyPlayerHomeMap != null)
			{
				this.mapParent = Find.RandomPlayerHomeMap.Parent;
			}
		}

		// Token: 0x04002999 RID: 10649
		public MapParent mapParent;

		// Token: 0x0400299A RID: 10650
		public string inSignal;

		// Token: 0x0400299B RID: 10651
		public string outSignalResult;

		// Token: 0x0400299C RID: 10652
		public bool destroyOrPassToWorldOnCleanup;

		// Token: 0x0400299D RID: 10653
		private MonumentMarker copy;
	}
}
