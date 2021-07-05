using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B6F RID: 2927
	public class QuestPart_DestroyWorldObject : QuestPart
	{
		// Token: 0x17000C02 RID: 3074
		// (get) Token: 0x06004477 RID: 17527 RVA: 0x0016B518 File Offset: 0x00169718
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.worldObject != null)
				{
					yield return this.worldObject;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x06004478 RID: 17528 RVA: 0x0016B528 File Offset: 0x00169728
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				QuestPart_DestroyWorldObject.TryRemove(this.worldObject);
			}
		}

		// Token: 0x06004479 RID: 17529 RVA: 0x0016B54F File Offset: 0x0016974F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<WorldObject>(ref this.worldObject, "worldObject", false);
		}

		// Token: 0x0600447A RID: 17530 RVA: 0x0016B57C File Offset: 0x0016977C
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			int tile;
			if (TileFinder.TryFindNewSiteTile(out tile, 7, 27, false, TileFinderMode.Near, -1, false))
			{
				this.worldObject = SiteMaker.MakeSite(null, tile, null, true, null);
			}
		}

		// Token: 0x0600447B RID: 17531 RVA: 0x0016B5D4 File Offset: 0x001697D4
		public static void TryRemove(WorldObject worldObject)
		{
			if (worldObject != null && worldObject.Spawned)
			{
				MapParent mapParent = worldObject as MapParent;
				if (mapParent != null && mapParent.HasMap)
				{
					mapParent.forceRemoveWorldObjectWhenMapRemoved = true;
					return;
				}
				worldObject.Destroy();
			}
		}

		// Token: 0x0400298B RID: 10635
		public string inSignal;

		// Token: 0x0400298C RID: 10636
		public WorldObject worldObject;
	}
}
