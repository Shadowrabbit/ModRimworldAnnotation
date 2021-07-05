using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012B4 RID: 4788
	public class Designator_PlanAdd : Designator_Plan
	{
		// Token: 0x06007271 RID: 29297 RVA: 0x00263330 File Offset: 0x00261530
		public Designator_PlanAdd() : base(DesignateMode.Add)
		{
			this.defaultLabel = "DesignatorPlan".Translate();
			this.defaultDesc = "DesignatorPlanDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/PlanOn", true);
			this.soundSucceeded = SoundDefOf.Designate_PlanAdd;
			this.hotKey = KeyBindingDefOf.Misc9;
		}
	}
}
