using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F2B RID: 3883
	public class RitualBehaviorWorker_Funeral : RitualBehaviorWorker
	{
		// Token: 0x06005C5D RID: 23645 RVA: 0x001FD352 File Offset: 0x001FB552
		public RitualBehaviorWorker_Funeral()
		{
		}

		// Token: 0x06005C5E RID: 23646 RVA: 0x001FD35A File Offset: 0x001FB55A
		public RitualBehaviorWorker_Funeral(RitualBehaviorDef def) : base(def)
		{
		}

		// Token: 0x06005C5F RID: 23647 RVA: 0x001FDC78 File Offset: 0x001FBE78
		public override string CanStartRitualNow(TargetInfo target, Precept_Ritual ritual, Pawn selectedPawn = null, Dictionary<string, Pawn> forcedForRole = null)
		{
			Building_Grave building_Grave;
			if (target.HasThing && (building_Grave = (target.Thing as Building_Grave)) != null && building_Grave.Corpse != null && building_Grave.Corpse.InnerPawn.IsSlave)
			{
				return "CantStartFuneralForSlave".Translate(building_Grave.Corpse.InnerPawn);
			}
			return base.CanStartRitualNow(target, ritual, selectedPawn, forcedForRole);
		}
	}
}
