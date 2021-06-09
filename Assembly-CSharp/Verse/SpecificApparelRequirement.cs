using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000160 RID: 352
	public class SpecificApparelRequirement
	{
		// Token: 0x1700019C RID: 412
		// (get) Token: 0x060008E5 RID: 2277 RVA: 0x0000D05C File Offset: 0x0000B25C
		public string RequiredTag
		{
			get
			{
				return this.requiredTag;
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x060008E6 RID: 2278 RVA: 0x0000D064 File Offset: 0x0000B264
		public List<SpecificApparelRequirement.TagChance> AlternateTagChoices
		{
			get
			{
				return this.alternateTagChoices;
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x060008E7 RID: 2279 RVA: 0x0000D06C File Offset: 0x0000B26C
		public ThingDef Stuff
		{
			get
			{
				return this.stuff;
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x060008E8 RID: 2280 RVA: 0x0000D074 File Offset: 0x0000B274
		public BodyPartGroupDef BodyPartGroup
		{
			get
			{
				return this.bodyPartGroup;
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x060008E9 RID: 2281 RVA: 0x0000D07C File Offset: 0x0000B27C
		public ApparelLayerDef ApparelLayer
		{
			get
			{
				return this.apparelLayer;
			}
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x060008EA RID: 2282 RVA: 0x0000D084 File Offset: 0x0000B284
		public Color Color
		{
			get
			{
				return this.color;
			}
		}

		// Token: 0x04000789 RID: 1929
		private string requiredTag;

		// Token: 0x0400078A RID: 1930
		private List<SpecificApparelRequirement.TagChance> alternateTagChoices;

		// Token: 0x0400078B RID: 1931
		private ThingDef stuff;

		// Token: 0x0400078C RID: 1932
		private BodyPartGroupDef bodyPartGroup;

		// Token: 0x0400078D RID: 1933
		private ApparelLayerDef apparelLayer;

		// Token: 0x0400078E RID: 1934
		private Color color;

		// Token: 0x02000161 RID: 353
		public struct TagChance
		{
			// Token: 0x0400078F RID: 1935
			public string tag;

			// Token: 0x04000790 RID: 1936
			public float chance;
		}
	}
}
