using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x020006A1 RID: 1697
	public class Trigger_OnHumanlikeHarmAnyThing : Trigger
	{
		// Token: 0x06002F4E RID: 12110 RVA: 0x00118982 File Offset: 0x00116B82
		public Trigger_OnHumanlikeHarmAnyThing(List<Thing> things)
		{
			this.things = things;
		}

		// Token: 0x06002F4F RID: 12111 RVA: 0x00118994 File Offset: 0x00116B94
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			Pawn pawn;
			return signal.dinfo.Def != null && signal.dinfo.Def.ExternalViolenceFor(signal.thing) && signal.dinfo.Instigator != null && (pawn = (signal.dinfo.Instigator as Pawn)) != null && pawn.RaceProps.Humanlike && this.things.Contains(signal.thing);
		}

		// Token: 0x04001CEE RID: 7406
		private List<Thing> things;
	}
}
