using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001179 RID: 4473
	public class CompProperties_Rechargeable : CompProperties
	{
		// Token: 0x06006B7C RID: 27516 RVA: 0x0024179E File Offset: 0x0023F99E
		public CompProperties_Rechargeable()
		{
			this.compClass = typeof(CompRechargeable);
		}

		// Token: 0x06006B7D RID: 27517 RVA: 0x002417B6 File Offset: 0x0023F9B6
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (this.ticksToRecharge <= 0)
			{
				yield return "ticksToRecharge must be a positive value";
			}
			yield break;
		}

		// Token: 0x04003BCA RID: 15306
		public int ticksToRecharge;

		// Token: 0x04003BCB RID: 15307
		public SoundDef chargedSoundDef;

		// Token: 0x04003BCC RID: 15308
		public SoundDef dischargeSoundDef;
	}
}
