using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001078 RID: 4216
	public class QuestPart_NoWorldObject : QuestPartActivable
	{
		// Token: 0x17000E35 RID: 3637
		// (get) Token: 0x06005BC2 RID: 23490 RVA: 0x0003FA0B File Offset: 0x0003DC0B
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

		// Token: 0x06005BC3 RID: 23491 RVA: 0x0003FA1B File Offset: 0x0003DC1B
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			if (this.worldObject == null || !this.worldObject.Spawned)
			{
				base.Complete();
			}
		}

		// Token: 0x06005BC4 RID: 23492 RVA: 0x0003FA3E File Offset: 0x0003DC3E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<WorldObject>(ref this.worldObject, "worldObject", false);
		}

		// Token: 0x06005BC5 RID: 23493 RVA: 0x001D9014 File Offset: 0x001D7214
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			Site site = Find.WorldObjects.Sites.FirstOrDefault<Site>();
			if (site != null)
			{
				this.worldObject = site;
				return;
			}
			Map randomPlayerHomeMap = Find.RandomPlayerHomeMap;
			if (randomPlayerHomeMap != null)
			{
				this.worldObject = randomPlayerHomeMap.Parent;
			}
		}

		// Token: 0x04003D8D RID: 15757
		public WorldObject worldObject;
	}
}
