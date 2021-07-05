using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F36 RID: 3894
	public class RitualObligationTargetWorker_AnyEmptyGrave : RitualObligationTargetFilter
	{
		// Token: 0x06005C9B RID: 23707 RVA: 0x001FE22F File Offset: 0x001FC42F
		public RitualObligationTargetWorker_AnyEmptyGrave()
		{
		}

		// Token: 0x06005C9C RID: 23708 RVA: 0x001FE237 File Offset: 0x001FC437
		public RitualObligationTargetWorker_AnyEmptyGrave(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x06005C9D RID: 23709 RVA: 0x001FE3CF File Offset: 0x001FC5CF
		public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
		{
			Thing thing = map.listerThings.ThingsInGroup(ThingRequestGroup.Grave).FirstOrDefault((Thing t) => ((Building_Grave)t).Corpse == null);
			if (thing != null)
			{
				yield return thing;
			}
			yield break;
		}

		// Token: 0x06005C9E RID: 23710 RVA: 0x001FE3E0 File Offset: 0x001FC5E0
		protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
		{
			Building_Grave building_Grave;
			return target.HasThing && (building_Grave = (target.Thing as Building_Grave)) != null && building_Grave.Corpse == null;
		}

		// Token: 0x06005C9F RID: 23711 RVA: 0x001FE417 File Offset: 0x001FC617
		public override bool ObligationTargetsValid(RitualObligation obligation)
		{
			return obligation.targetA.HasThing && obligation.targetA.ThingDestroyed;
		}

		// Token: 0x06005CA0 RID: 23712 RVA: 0x001FE433 File Offset: 0x001FC633
		public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
		{
			if (obligation == null)
			{
				yield return "RitualTargetEmptyGraveInfoAbstract".Translate(this.parent.ideo.Named("IDEO"));
				yield break;
			}
			Pawn arg = (Pawn)obligation.targetA.Thing;
			TaggedString taggedString = "RitualTargetEmptyGraveInfo".Translate(arg.Named("PAWN"));
			yield return taggedString;
			yield break;
		}

		// Token: 0x06005CA1 RID: 23713 RVA: 0x001FE44A File Offset: 0x001FC64A
		public override string LabelExtraPart(RitualObligation obligation)
		{
			return ((Pawn)obligation.targetA.Thing).LabelShort;
		}
	}
}
