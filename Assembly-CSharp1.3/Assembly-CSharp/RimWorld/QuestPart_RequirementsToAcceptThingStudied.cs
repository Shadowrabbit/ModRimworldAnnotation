using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B9F RID: 2975
	public class QuestPart_RequirementsToAcceptThingStudied : QuestPart_RequirementsToAccept
	{
		// Token: 0x17000C24 RID: 3108
		// (get) Token: 0x0600457A RID: 17786 RVA: 0x001706DC File Offset: 0x0016E8DC
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				if (this.thing != null)
				{
					yield return this.thing;
				}
				yield break;
			}
		}

		// Token: 0x0600457B RID: 17787 RVA: 0x001706EC File Offset: 0x0016E8EC
		public override AcceptanceReport CanAccept()
		{
			Thing thing = this.thing;
			CompStudiable compStudiable = (thing != null) ? thing.TryGetComp<CompStudiable>() : null;
			if (compStudiable != null && !compStudiable.Completed)
			{
				return new AcceptanceReport("QuestRequiredThingStudied".Translate(this.thing));
			}
			return true;
		}

		// Token: 0x0600457C RID: 17788 RVA: 0x0017073D File Offset: 0x0016E93D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.thing, "thing", false);
		}

		// Token: 0x04002A50 RID: 10832
		public Thing thing;
	}
}
