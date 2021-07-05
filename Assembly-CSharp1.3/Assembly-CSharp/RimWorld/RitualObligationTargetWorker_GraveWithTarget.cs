using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F35 RID: 3893
	public class RitualObligationTargetWorker_GraveWithTarget : RitualObligationTargetFilter
	{
		// Token: 0x06005C94 RID: 23700 RVA: 0x001FE22F File Offset: 0x001FC42F
		public RitualObligationTargetWorker_GraveWithTarget()
		{
		}

		// Token: 0x06005C95 RID: 23701 RVA: 0x001FE237 File Offset: 0x001FC437
		public RitualObligationTargetWorker_GraveWithTarget(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x06005C96 RID: 23702 RVA: 0x001FE325 File Offset: 0x001FC525
		public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
		{
			Thing thing = map.listerThings.ThingsInGroup(ThingRequestGroup.Grave).FirstOrDefault((Thing t) => ((Building_Grave)t).Corpse == obligation.targetA.Thing);
			if (thing != null)
			{
				yield return thing;
			}
			yield break;
		}

		// Token: 0x06005C97 RID: 23703 RVA: 0x001FE33C File Offset: 0x001FC53C
		protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
		{
			Building_Grave building_Grave;
			return target.HasThing && (building_Grave = (target.Thing as Building_Grave)) != null && building_Grave.Corpse == obligation.targetA.Thing;
		}

		// Token: 0x06005C98 RID: 23704 RVA: 0x001FE37D File Offset: 0x001FC57D
		public override bool ObligationTargetsValid(RitualObligation obligation)
		{
			return obligation.targetA.HasThing && !obligation.targetA.ThingDestroyed;
		}

		// Token: 0x06005C99 RID: 23705 RVA: 0x001FE39C File Offset: 0x001FC59C
		public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
		{
			if (obligation == null)
			{
				yield return "RitualTargetGraveInfoAbstract".Translate(this.parent.ideo.Named("IDEO"));
				yield break;
			}
			bool flag = obligation.targetA.Thing.ParentHolder is Building_Grave;
			Pawn innerPawn = ((Corpse)obligation.targetA.Thing).InnerPawn;
			TaggedString taggedString = "RitualTargetGraveInfo".Translate(innerPawn.Named("PAWN"));
			if (!flag)
			{
				taggedString += " (" + "RitualTargetGraveInfoMustBeBuried".Translate(innerPawn.Named("PAWN")) + ")";
			}
			yield return taggedString;
			yield break;
		}

		// Token: 0x06005C9A RID: 23706 RVA: 0x001FE3B3 File Offset: 0x001FC5B3
		public override string LabelExtraPart(RitualObligation obligation)
		{
			return ((Corpse)obligation.targetA.Thing).InnerPawn.LabelShort;
		}
	}
}
