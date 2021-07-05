using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000050 RID: 80
	public class PawnTags : IExposable
	{
		// Token: 0x060003CF RID: 975 RVA: 0x00014E44 File Offset: 0x00013044
		public bool Contains(string tag)
		{
			return this.tags.Contains(tag);
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x00014E52 File Offset: 0x00013052
		public void ExposeData()
		{
			Scribe_Collections.Look<string>(ref this.tags, "tags", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x0400011C RID: 284
		public List<string> tags;
	}
}
