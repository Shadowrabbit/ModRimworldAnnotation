using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B50 RID: 2896
	public class QuestPart_WorldObjectTimeout : QuestPart_Delay
	{
		// Token: 0x17000BE7 RID: 3047
		// (get) Token: 0x060043C9 RID: 17353 RVA: 0x00168C96 File Offset: 0x00166E96
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

		// Token: 0x060043CA RID: 17354 RVA: 0x00168CA8 File Offset: 0x00166EA8
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

		// Token: 0x060043CB RID: 17355 RVA: 0x00168D1D File Offset: 0x00166F1D
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

		// Token: 0x060043CC RID: 17356 RVA: 0x00168D4F File Offset: 0x00166F4F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<WorldObject>(ref this.worldObject, "worldObject", false);
		}

		// Token: 0x060043CD RID: 17357 RVA: 0x00168D68 File Offset: 0x00166F68
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			int tile;
			if (TileFinder.TryFindNewSiteTile(out tile, 7, 27, false, TileFinderMode.Near, -1, false))
			{
				this.worldObject = SiteMaker.MakeSite(null, tile, null, true, null);
			}
		}

		// Token: 0x04002923 RID: 10531
		public WorldObject worldObject;
	}
}
