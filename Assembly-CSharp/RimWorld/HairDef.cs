using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F9D RID: 3997
	public class HairDef : Def
	{
		// Token: 0x0400394A RID: 14666
		[NoTranslate]
		public string texPath;

		// Token: 0x0400394B RID: 14667
		public HairGender hairGender = HairGender.Any;

		// Token: 0x0400394C RID: 14668
		[NoTranslate]
		public List<string> hairTags = new List<string>();
	}
}
