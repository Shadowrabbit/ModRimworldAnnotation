using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F66 RID: 3942
	public class RitualOutcomeComp_RitualTargetDefs : RitualOutcomeComp_QualitySingleOffset
	{
		// Token: 0x06005D7C RID: 23932 RVA: 0x00200E24 File Offset: 0x001FF024
		public override bool Applies(LordJob_Ritual ritual)
		{
			if (this.allowAltars && ritual.selectedTarget.Thing != null && ritual.selectedTarget.Thing.def.isAltar)
			{
				return true;
			}
			if (!this.defs.NullOrEmpty<ThingDef>())
			{
				List<ThingDef> list = this.defs;
				Thing thing = ritual.selectedTarget.Thing;
				return list.Contains((thing != null) ? thing.def : null);
			}
			return false;
		}

		// Token: 0x06005D7D RID: 23933 RVA: 0x00200E90 File Offset: 0x001FF090
		public override ExpectedOutcomeDesc GetExpectedOutcomeDesc(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments)
		{
			Thing thing = ritualTarget.Thing;
			if (thing == null)
			{
				return null;
			}
			bool flag = (this.allowAltars && thing.def.isAltar) || (!this.defs.NullOrEmpty<ThingDef>() && this.defs.Contains(ritualTarget.Thing.def));
			TaggedString taggedString = "RitualOutcomeCompTip_RitualTargetDefsPart1".Translate(ritual.LabelCap, this.expectedThingLabelTip);
			if (!flag)
			{
				taggedString += "\n\n" + "RitualOutcomeCompTip_RitualTargetDefsPart2".Translate(ritual.LabelCap, thing);
			}
			return new ExpectedOutcomeDesc
			{
				label = this.label.CapitalizeFirst(),
				effect = this.ExpectedOffsetDesc(flag, -1f),
				positive = flag,
				present = flag,
				quality = (flag ? this.qualityOffset : 0f),
				priority = 2f,
				tip = taggedString
			};
		}

		// Token: 0x06005D7E RID: 23934 RVA: 0x00200F9D File Offset: 0x001FF19D
		protected override string ExpectedOffsetDesc(bool positive, float quality = -1f)
		{
			quality = ((quality == -1f) ? this.qualityOffset : quality);
			if (!positive)
			{
				return "";
			}
			return quality.ToStringWithSign("0.#%");
		}

		// Token: 0x04003605 RID: 13829
		[MustTranslate]
		public string expectedThingLabelTip;

		// Token: 0x04003606 RID: 13830
		public List<ThingDef> defs;

		// Token: 0x04003607 RID: 13831
		public bool allowAltars;
	}
}
