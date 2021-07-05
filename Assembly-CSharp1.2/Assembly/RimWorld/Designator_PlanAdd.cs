using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020019AB RID: 6571
	public class Designator_PlanAdd : Designator_Plan
	{
		// Token: 0x0600914F RID: 37199 RVA: 0x0029C670 File Offset: 0x0029A870
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
