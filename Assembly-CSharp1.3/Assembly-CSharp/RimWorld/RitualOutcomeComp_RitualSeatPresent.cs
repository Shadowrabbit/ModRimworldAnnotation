using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F60 RID: 3936
	public class RitualOutcomeComp_RitualSeatPresent : RitualOutcomeComp_BuildingsPresent
	{
		// Token: 0x1700102A RID: 4138
		// (get) Token: 0x06005D60 RID: 23904 RVA: 0x00200428 File Offset: 0x001FE628
		protected override string LabelForDesc
		{
			get
			{
				return "RitualSeat".Translate();
			}
		}

		// Token: 0x06005D61 RID: 23905 RVA: 0x0020043C File Offset: 0x001FE63C
		protected override int RequiredAmount(RitualRoleAssignments assignments)
		{
			int num = assignments.Ritual.ideo.RitualSeatDef.Size.x * assignments.Ritual.ideo.RitualSeatDef.Size.z;
			return Mathf.CeilToInt((float)assignments.SpectatorsForReading.Count / (float)num);
		}

		// Token: 0x06005D62 RID: 23906 RVA: 0x00200494 File Offset: 0x001FE694
		protected override string LabelForPredictedOutcomeDesc(Precept_Ritual ritual)
		{
			ThingDef ritualSeatDef = ritual.ideo.RitualSeatDef;
			if (ritualSeatDef == null)
			{
				return this.LabelForDesc;
			}
			return ritualSeatDef.LabelCap;
		}

		// Token: 0x06005D63 RID: 23907 RVA: 0x002004C4 File Offset: 0x001FE6C4
		protected override Thing LookForBuilding(IntVec3 cell, Map map, Precept_Ritual ritual)
		{
			ThingDef ritualSeatDef = ritual.ideo.RitualSeatDef;
			if (ritualSeatDef == null)
			{
				return null;
			}
			Thing firstThing = cell.GetFirstThing(map, ritualSeatDef);
			if (firstThing != null)
			{
				return firstThing;
			}
			return null;
		}
	}
}
