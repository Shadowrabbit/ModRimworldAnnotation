using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B4A RID: 2890
	public class QuestPart_NoWorldObject : QuestPartActivable
	{
		// Token: 0x17000BDA RID: 3034
		// (get) Token: 0x0600439E RID: 17310 RVA: 0x001684FE File Offset: 0x001666FE
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

		// Token: 0x0600439F RID: 17311 RVA: 0x0016850E File Offset: 0x0016670E
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			if (this.worldObject == null || !this.worldObject.Spawned)
			{
				base.Complete();
			}
		}

		// Token: 0x060043A0 RID: 17312 RVA: 0x00168531 File Offset: 0x00166731
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<WorldObject>(ref this.worldObject, "worldObject", false);
		}

		// Token: 0x060043A1 RID: 17313 RVA: 0x0016854C File Offset: 0x0016674C
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

		// Token: 0x04002913 RID: 10515
		public WorldObject worldObject;
	}
}
