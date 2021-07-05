using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F58 RID: 3928
	public class RitualOutcomeComp_BuildingOfDefUsed : RitualOutcomeComp_QualitySingleOffset
	{
		// Token: 0x17001021 RID: 4129
		// (get) Token: 0x06005D36 RID: 23862 RVA: 0x001FFAEA File Offset: 0x001FDCEA
		protected override string LabelForDesc
		{
			get
			{
				return "Used".Translate(this.def.LabelCap);
			}
		}

		// Token: 0x06005D37 RID: 23863 RVA: 0x001FFB0B File Offset: 0x001FDD0B
		public override bool Applies(LordJob_Ritual ritual)
		{
			return ritual.usedThings.Any((Thing t) => t.def == this.def);
		}

		// Token: 0x06005D38 RID: 23864 RVA: 0x001FFB24 File Offset: 0x001FDD24
		public override ExpectedOutcomeDesc GetExpectedOutcomeDesc(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments)
		{
			bool flag = false;
			foreach (Thing thing in ritualTarget.Map.listerThings.ThingsOfDef(this.def))
			{
				if (GatheringsUtility.InGatheringArea(thing.Position, ritualTarget.Cell, ritualTarget.Map) && (this.maxHorDistFromTarget == 0 || thing.Position.InHorDistOf(ritualTarget.Cell, (float)this.maxHorDistFromTarget)))
				{
					flag = true;
					break;
				}
			}
			return new ExpectedOutcomeDesc
			{
				label = this.def.LabelCap,
				present = flag,
				effect = this.ExpectedOffsetDesc(flag, -1f),
				quality = (flag ? this.qualityOffset : 0f),
				positive = flag,
				priority = 1f
			};
		}

		// Token: 0x040035F8 RID: 13816
		public ThingDef def;

		// Token: 0x040035F9 RID: 13817
		public int maxHorDistFromTarget;
	}
}
