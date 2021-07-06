using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020019AC RID: 6572
	public class Designator_PlanRemove : Designator_Plan
	{
		// Token: 0x06009150 RID: 37200 RVA: 0x0029C6D8 File Offset: 0x0029A8D8
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
