using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001014 RID: 4116
	public class ScenPart_ConfigPage : ScenPart
	{
		// Token: 0x06006111 RID: 24849 RVA: 0x0020FD26 File Offset: 0x0020DF26
		public override IEnumerable<Page> GetConfigPages()
		{
			yield return (Page)Activator.CreateInstance(this.def.pageClass);
			yield break;
		}

		// Token: 0x06006112 RID: 24850 RVA: 0x0000313F File Offset: 0x0000133F
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
		}
	}
}
