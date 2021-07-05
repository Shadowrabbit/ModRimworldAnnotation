using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001083 RID: 4227
	public class QuestPart_WorldObjectTimeout : QuestPart_Delay
	{
		// Token: 0x17000E4C RID: 3660
		// (get) Token: 0x06005C1B RID: 23579 RVA: 0x0003FE25 File Offset: 0x0003E025
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

		// Token: 0x06005C1C RID: 23580 RVA: 0x001D9B2C File Offset: 0x001D7D2C
		public override string ExtraInspectString(ISelectable target)
		{
			if (target == this.worldObject)
			{
				Site site = target as Site;
				if (site != null)
				{
					for (int i = 0; i < site.parts.Count; i++)
					{
						if (site.parts[i].def.handlesWorldObjectTimeoutInspectString)
						{
							return null;
						}
					}
				}
				return "WorldObjectTimeout".Translate(base.TicksLeft.ToStringTicksToPeriod(true, false, true, true));
			}
			return null;
		}

		// Token: 0x06005C1D RID: 23581 RVA: 0x0003FE35 File Offset: 0x0003E035
		protected override void DelayFinished()
		{
			QuestPart_DestroyWorldObject.TryRemove(this.worldObject);
			if (this.worldObject != null)
			{
				base.Complete(this.worldObject.Named("SUBJECT"));
				return;
			}
			base.Complete();
		}

		// Token: 0x06005C1E RID: 23582 RVA: 0x0003FE67 File Offset: 0x0003E067
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<WorldObject>(ref this.worldObject, "worldObject", false);
		}

		// Token: 0x06005C1F RID: 23583 RVA: 0x001D9BA4 File Offset: 0x001D7DA4
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			int tile;
			if (TileFinder.TryFindNewSiteTile(out tile, 7, 27, false, true, -1))
			{
				this.worldObject = SiteMaker.MakeSite(null, tile, null, true, null);
			}
		}

		// Token: 0x04003DB6 RID: 15798
		public WorldObject worldObject;
	}
}
