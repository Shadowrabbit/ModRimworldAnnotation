using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F2B RID: 3883
	public class CompProperties_Hibernatable : CompProperties
	{
		// Token: 0x06005594 RID: 21908 RVA: 0x0003B718 File Offset: 0x00039918
		public CompProperties_Hibernatable()
		{
			this.compClass = typeof(CompHibernatable);
		}

		// Token: 0x06005595 RID: 21909 RVA: 0x0003B73B File Offset: 0x0003993B
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (parentDef.tickerType != TickerType.Normal)
			{
				yield return string.Concat(new object[]
				{
					"CompHibernatable needs tickerType ",
					TickerType.Normal,
					", has ",
					parentDef.tickerType
				});
			}
			yield break;
			yield break;
		}

		// Token: 0x040036C4 RID: 14020
		public float startupDays = 14f;

		// Token: 0x040036C5 RID: 14021
		public IncidentTargetTagDef incidentTargetWhileStarting;

		// Token: 0x040036C6 RID: 14022
		public SoundDef sustainerActive;
	}
}
