using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x02000B03 RID: 2819
	public class Trigger_OnHumanlikeHarmAnyThing : Trigger
	{
		// Token: 0x06004221 RID: 16929 RVA: 0x00031483 File Offset: 0x0002F683
		public Trigger_OnHumanlikeHarmAnyThing(List<Thing> things)
		{
			this.things = things;
		}

		// Token: 0x06004222 RID: 16930 RVA: 0x00189048 File Offset: 0x00187248
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			Pawn pawn;
			return signal.dinfo.Def != null && signal.dinfo.Def.ExternalViolenceFor(signal.thing) && signal.dinfo.Instigator != null && (pawn = (signal.dinfo.Instigator as Pawn)) != null && pawn.RaceProps.Humanlike && this.things.Contains(signal.thing);
		}

		// Token: 0x04002D64 RID: 11620
		private List<Thing> things;
	}
}
