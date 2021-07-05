using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020011FA RID: 4602
	public class CompProperties_Lightball : CompProperties
	{
		// Token: 0x06006EB4 RID: 28340 RVA: 0x00250C7E File Offset: 0x0024EE7E
		public CompProperties_Lightball()
		{
			this.compClass = typeof(CompLightball);
		}

		// Token: 0x06006EB5 RID: 28341 RVA: 0x00250C96 File Offset: 0x0024EE96
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (parentDef.comps.FirstOrDefault((CompProperties c) => c.compClass == typeof(CompPowerTrader)) == null)
			{
				yield return "Can't use CompLightball without CompPowerTrader.";
			}
			yield break;
			yield break;
		}

		// Token: 0x04003D4A RID: 15690
		public List<SoundDef> soundDefsPerSpeakerCount;

		// Token: 0x04003D4B RID: 15691
		public int maxSpeakerDistance;
	}
}
