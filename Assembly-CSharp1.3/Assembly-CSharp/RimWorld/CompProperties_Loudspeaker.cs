using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001202 RID: 4610
	public class CompProperties_Loudspeaker : CompProperties
	{
		// Token: 0x06006ED0 RID: 28368 RVA: 0x002511C1 File Offset: 0x0024F3C1
		public CompProperties_Loudspeaker()
		{
			this.compClass = typeof(CompLoudspeaker);
		}

		// Token: 0x06006ED1 RID: 28369 RVA: 0x002511D9 File Offset: 0x0024F3D9
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (parentDef.comps.FirstOrDefault((CompProperties c) => c.compClass == typeof(CompPowerTrader)) == null)
			{
				yield return "Can't use CompLoudspeaker without CompPowerTrader.";
			}
			yield break;
			yield break;
		}
	}
}
