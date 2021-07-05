using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F34 RID: 3892
	public class RitualObligationTargetWorker_Thing : RitualObligationTargetFilter
	{
		// Token: 0x06005C8F RID: 23695 RVA: 0x001FE22F File Offset: 0x001FC42F
		public RitualObligationTargetWorker_Thing()
		{
		}

		// Token: 0x06005C90 RID: 23696 RVA: 0x001FE237 File Offset: 0x001FC437
		public RitualObligationTargetWorker_Thing(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x06005C91 RID: 23697 RVA: 0x001FE2DE File Offset: 0x001FC4DE
		public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
		{
			if (!obligation.targetA.HasThing)
			{
				yield break;
			}
			yield return obligation.targetA.Thing;
			yield break;
		}

		// Token: 0x06005C92 RID: 23698 RVA: 0x001FE2EE File Offset: 0x001FC4EE
		protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
		{
			return target.HasThing && target.Thing == obligation.targetA.Thing;
		}

		// Token: 0x06005C93 RID: 23699 RVA: 0x001FE315 File Offset: 0x001FC515
		public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
		{
			yield return obligation.targetA.Thing.LabelShortCap;
			yield break;
		}
	}
}
