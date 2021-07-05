using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001105 RID: 4357
	public class CompProperties_BiosculpterPod : CompProperties
	{
		// Token: 0x060068A1 RID: 26785 RVA: 0x00235088 File Offset: 0x00233288
		public CompProperties_BiosculpterPod()
		{
			this.compClass = typeof(CompBiosculpterPod);
		}

		// Token: 0x060068A2 RID: 26786 RVA: 0x002350A0 File Offset: 0x002332A0
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (parentDef.tickerType != TickerType.Normal)
			{
				yield return base.GetType().Name + " requires parent ticker type Normal";
			}
			yield break;
			yield break;
		}

		// Token: 0x04003A9C RID: 15004
		public SoundDef enterSound;

		// Token: 0x04003A9D RID: 15005
		public SoundDef exitSound;

		// Token: 0x04003A9E RID: 15006
		public EffecterDef operatingEffecter;

		// Token: 0x04003A9F RID: 15007
		public EffecterDef readyEffecter;
	}
}
