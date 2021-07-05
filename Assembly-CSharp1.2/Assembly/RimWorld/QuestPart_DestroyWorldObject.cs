using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010B8 RID: 4280
	public class QuestPart_DestroyWorldObject : QuestPart
	{
		// Token: 0x17000E7E RID: 3710
		// (get) Token: 0x06005D55 RID: 23893 RVA: 0x00040B98 File Offset: 0x0003ED98
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

		// Token: 0x06005D56 RID: 23894 RVA: 0x00040BA8 File Offset: 0x0003EDA8
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				QuestPart_DestroyWorldObject.TryRemove(this.worldObject);
			}
		}

		// Token: 0x06005D57 RID: 23895 RVA: 0x00040BCF File Offset: 0x0003EDCF
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<WorldObject>(ref this.worldObject, "worldObject", false);
		}

		// Token: 0x06005D58 RID: 23896 RVA: 0x001DC790 File Offset: 0x001DA990
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			int tile;
			if (TileFinder.TryFindNewSiteTile(out tile, 7, 27, false, true, -1))
			{
				this.worldObject = SiteMaker.MakeSite(null, tile, null, true, null);
			}
		}

		// Token: 0x06005D59 RID: 23897 RVA: 0x001DC7E4 File Offset: 0x001DA9E4
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

		// Token: 0x04003E71 RID: 15985
		public string inSignal;

		// Token: 0x04003E72 RID: 15986
		public WorldObject worldObject;
	}
}
