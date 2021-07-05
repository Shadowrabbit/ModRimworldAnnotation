using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001117 RID: 4375
	public class CompProperties_CanBeDormant : CompProperties
	{
		// Token: 0x0600690E RID: 26894 RVA: 0x002370BC File Offset: 0x002352BC
		public CompProperties_CanBeDormant()
		{
			this.compClass = typeof(CompCanBeDormant);
		}

		// Token: 0x0600690F RID: 26895 RVA: 0x00237112 File Offset: 0x00235312
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (!parentDef.receivesSignals)
			{
				yield return "ThingDefs with CanBeDormant component must have receivesSignals set to true, otherwise wakeup won't work properly!";
			}
			yield break;
		}

		// Token: 0x04003ACF RID: 15055
		public bool startsDormant;

		// Token: 0x04003AD0 RID: 15056
		public string wakeUpSignalTag = "CompCanBeDormant.WakeUp";

		// Token: 0x04003AD1 RID: 15057
		public float maxDistAwakenByOther = 40f;

		// Token: 0x04003AD2 RID: 15058
		public bool canWakeUpFogged = true;

		// Token: 0x04003AD3 RID: 15059
		public string awakeStateLabelKey = "AwokeDaysAgo";

		// Token: 0x04003AD4 RID: 15060
		public string dormantStateLabelKey = "DormantCompInactive";
	}
}
