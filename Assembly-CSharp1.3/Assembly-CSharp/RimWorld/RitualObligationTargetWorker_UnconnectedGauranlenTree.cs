using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F44 RID: 3908
	public class RitualObligationTargetWorker_UnconnectedGauranlenTree : RitualObligationTargetFilter
	{
		// Token: 0x06005CE2 RID: 23778 RVA: 0x001FE22F File Offset: 0x001FC42F
		public RitualObligationTargetWorker_UnconnectedGauranlenTree()
		{
		}

		// Token: 0x06005CE3 RID: 23779 RVA: 0x001FE237 File Offset: 0x001FC437
		public RitualObligationTargetWorker_UnconnectedGauranlenTree(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x06005CE4 RID: 23780 RVA: 0x001FEE70 File Offset: 0x001FD070
		public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
		{
			List<Thing> trees = map.listerThings.ThingsOfDef(ThingDefOf.Plant_TreeGauranlen);
			int num;
			for (int i = 0; i < trees.Count; i = num + 1)
			{
				CompTreeConnection compTreeConnection = trees[i].TryGetComp<CompTreeConnection>();
				if (compTreeConnection != null && !compTreeConnection.Connected && !compTreeConnection.ConnectionTorn)
				{
					yield return trees[i];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06005CE5 RID: 23781 RVA: 0x001FEE80 File Offset: 0x001FD080
		protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
		{
			Thing thing = target.Thing;
			CompTreeConnection compTreeConnection = thing.TryGetComp<CompTreeConnection>();
			if (compTreeConnection == null)
			{
				return false;
			}
			if (compTreeConnection.Connected)
			{
				return "RitualTargetConnectedGauranlenTree".Translate(thing.Named("TREE"), compTreeConnection.ConnectedPawn.Named("PAWN"));
			}
			if (compTreeConnection.ConnectionTorn)
			{
				return "RitualTargetConnectionTornGauranlenTree".Translate(thing.Named("TREE"), compTreeConnection.UntornInDurationTicks.ToStringTicksToPeriod(true, false, true, true));
			}
			return true;
		}

		// Token: 0x06005CE6 RID: 23782 RVA: 0x001FEF16 File Offset: 0x001FD116
		public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
		{
			yield return "RitualTargetUnconnectedGaruanlenTreeInfo".Translate();
			yield break;
		}
	}
}
