using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012B5 RID: 4789
	public class Designator_PlanRemove : Designator_Plan
	{
		// Token: 0x06007272 RID: 29298 RVA: 0x00263398 File Offset: 0x00261598
		public Designator_PlanRemove() : base(DesignateMode.Remove)
		{
			this.defaultLabel = "DesignatorPlanRemove".Translate();
			this.defaultDesc = "DesignatorPlanRemoveDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/PlanOff", true);
			this.soundSucceeded = SoundDefOf.Designate_PlanRemove;
			this.hotKey = KeyBindingDefOf.Misc8;
		}
	}
}
