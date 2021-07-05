using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000FAA RID: 4010
	public class RitualTargetFilter_UsableThrone : RitualTargetFilter
	{
		// Token: 0x06005EB3 RID: 24243 RVA: 0x00206E74 File Offset: 0x00205074
		public RitualTargetFilter_UsableThrone()
		{
		}

		// Token: 0x06005EB4 RID: 24244 RVA: 0x00206E7C File Offset: 0x0020507C
		public RitualTargetFilter_UsableThrone(RitualTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x06005EB5 RID: 24245 RVA: 0x00206E88 File Offset: 0x00205088
		public override bool CanStart(TargetInfo initiator, TargetInfo selectedTarget, out string rejectionReason)
		{
			Pawn pawn = initiator.Thing as Pawn;
			rejectionReason = "";
			if (pawn == null)
			{
				return false;
			}
			Building_Throne building_Throne = RoyalTitleUtility.FindBestUsableThrone(pawn) ?? pawn.ownership.AssignedThrone;
			if (building_Throne == null)
			{
				rejectionReason = "AbilitySpeechDisabledNoThroneAssigned".Translate();
				return false;
			}
			if (!pawn.CanReserveAndReach(building_Throne, PathEndMode.InteractionCell, pawn.NormalMaxDanger(), 1, -1, null, false))
			{
				rejectionReason = "AbilitySpeechDisabledNoThroneIsNotAccessible".Translate();
				return false;
			}
			if (pawn.royalty.GetUnmetThroneroomRequirements(true, false).Any<string>())
			{
				rejectionReason = "AbilitySpeechDisabledNoThroneUndignified".Translate();
				return false;
			}
			return true;
		}

		// Token: 0x06005EB6 RID: 24246 RVA: 0x00206F30 File Offset: 0x00205130
		public override TargetInfo BestTarget(TargetInfo initiator, TargetInfo selectedTarget)
		{
			Building_Throne building_Throne = RoyalTitleUtility.FindBestUsableThrone((Pawn)initiator.Thing);
			if (building_Throne == null)
			{
				return TargetInfo.Invalid;
			}
			return new TargetInfo(building_Throne.InteractionCell, building_Throne.Map, false);
		}

		// Token: 0x06005EB7 RID: 24247 RVA: 0x00206F6A File Offset: 0x0020516A
		public override IEnumerable<string> GetTargetInfos(TargetInfo initiator)
		{
			yield return "AbilitySpeechTargetDescThrone".Translate();
			yield break;
		}
	}
}
