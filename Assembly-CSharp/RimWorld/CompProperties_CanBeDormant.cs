using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020017A9 RID: 6057
	public class CompProperties_CanBeDormant : CompProperties
	{
		// Token: 0x060085D7 RID: 34263 RVA: 0x00276FC4 File Offset: 0x002751C4
		public CompProperties_CanBeDormant()
		{
			this.compClass = typeof(CompCanBeDormant);
		}

		// Token: 0x060085D8 RID: 34264 RVA: 0x00059BE6 File Offset: 0x00057DE6
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (!parentDef.receivesSignals)
			{
				yield return "ThingDefs with CanBeDormant component must have receivesSignals set to true, otherwise wakeup won't work properly!";
			}
			yield break;
		}

		// Token: 0x0400564D RID: 22093
		public bool startsDormant;

		// Token: 0x0400564E RID: 22094
		public string wakeUpSignalTag = "CompCanBeDormant.WakeUp";

		// Token: 0x0400564F RID: 22095
		public float maxDistAwakenByOther = 40f;

		// Token: 0x04005650 RID: 22096
		public bool canWakeUpFogged = true;

		// Token: 0x04005651 RID: 22097
		public string awakeStateLabelKey = "AwokeDaysAgo";

		// Token: 0x04005652 RID: 22098
		public string dormantStateLabelKey = "DormantCompInactive";
	}
}
