using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A1B RID: 2587
	public class CompProperties_Hibernatable : CompProperties
	{
		// Token: 0x06003F14 RID: 16148 RVA: 0x001580D5 File Offset: 0x001562D5
		public CompProperties_Hibernatable()
		{
			this.compClass = typeof(CompHibernatable);
		}

		// Token: 0x06003F15 RID: 16149 RVA: 0x001580F8 File Offset: 0x001562F8
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

		// Token: 0x04002227 RID: 8743
		public float startupDays = 14f;

		// Token: 0x04002228 RID: 8744
		public IncidentTargetTagDef incidentTargetWhileStarting;

		// Token: 0x04002229 RID: 8745
		public SoundDef sustainerActive;
	}
}
