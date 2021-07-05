using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FAE RID: 4014
	public class RitualTargetFilter_SelectedThing : RitualTargetFilter
	{
		// Token: 0x06005EC8 RID: 24264 RVA: 0x00206E74 File Offset: 0x00205074
		public RitualTargetFilter_SelectedThing()
		{
		}

		// Token: 0x06005EC9 RID: 24265 RVA: 0x00206E7C File Offset: 0x0020507C
		public RitualTargetFilter_SelectedThing(RitualTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x06005ECA RID: 24266 RVA: 0x00207449 File Offset: 0x00205649
		public override bool CanStart(TargetInfo initiator, TargetInfo selectedTarget, out string rejectionReason)
		{
			rejectionReason = null;
			return true;
		}

		// Token: 0x06005ECB RID: 24267 RVA: 0x000D491C File Offset: 0x000D2B1C
		public override TargetInfo BestTarget(TargetInfo initiator, TargetInfo selectedTarget)
		{
			return selectedTarget;
		}

		// Token: 0x06005ECC RID: 24268 RVA: 0x0020744F File Offset: 0x0020564F
		public override IEnumerable<string> GetTargetInfos(TargetInfo initiator)
		{
			yield break;
		}
	}
}
