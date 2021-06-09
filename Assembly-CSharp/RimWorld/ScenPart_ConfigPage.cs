using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200161A RID: 5658
	public class ScenPart_ConfigPage : ScenPart
	{
		// Token: 0x06007B01 RID: 31489 RVA: 0x00052B77 File Offset: 0x00050D77
		public override IEnumerable<Page> GetConfigPages()
		{
			yield return (Page)Activator.CreateInstance(this.def.pageClass);
			yield break;
		}

		// Token: 0x06007B02 RID: 31490 RVA: 0x00006A05 File Offset: 0x00004C05
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
		}
	}
}
