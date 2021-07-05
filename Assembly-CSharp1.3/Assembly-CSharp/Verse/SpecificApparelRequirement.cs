using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000EB RID: 235
	public class SpecificApparelRequirement
	{
		// Token: 0x17000115 RID: 277
		// (get) Token: 0x0600065B RID: 1627 RVA: 0x0001F6C5 File Offset: 0x0001D8C5
		public BodyPartGroupDef BodyPartGroup
		{
			get
			{
				return this.bodyPartGroup;
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x0600065C RID: 1628 RVA: 0x0001F6CD File Offset: 0x0001D8CD
		public ApparelLayerDef ApparelLayer
		{
			get
			{
				return this.apparelLayer;
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x0600065D RID: 1629 RVA: 0x0001F6D5 File Offset: 0x0001D8D5
		public ThingDef ApparelDef
		{
			get
			{
				return this.apparelDef;
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x0600065E RID: 1630 RVA: 0x0001F6DD File Offset: 0x0001D8DD
		public string RequiredTag
		{
			get
			{
				return this.requiredTag;
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x0600065F RID: 1631 RVA: 0x0001F6E5 File Offset: 0x0001D8E5
		public List<SpecificApparelRequirement.TagChance> AlternateTagChoices
		{
			get
			{
				return this.alternateTagChoices;
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000660 RID: 1632 RVA: 0x0001F6ED File Offset: 0x0001D8ED
		public ThingDef Stuff
		{
			get
			{
				return this.stuff;
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000661 RID: 1633 RVA: 0x0001F6F5 File Offset: 0x0001D8F5
		public Color Color
		{
			get
			{
				return this.color;
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000662 RID: 1634 RVA: 0x0001F6FD File Offset: 0x0001D8FD
		public ColorGenerator ColorGenerator
		{
			get
			{
				return this.ColorGenerator;
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000663 RID: 1635 RVA: 0x0001F705 File Offset: 0x0001D905
		public bool Locked
		{
			get
			{
				return this.locked;
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000664 RID: 1636 RVA: 0x0001F70D File Offset: 0x0001D90D
		public bool Biocode
		{
			get
			{
				return this.biocode;
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06000665 RID: 1637 RVA: 0x0001F715 File Offset: 0x0001D915
		public ThingStyleDef StyleDef
		{
			get
			{
				return this.styleDef;
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000666 RID: 1638 RVA: 0x0001F71D File Offset: 0x0001D91D
		public QualityCategory? Quality
		{
			get
			{
				return this.quality;
			}
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x0001F728 File Offset: 0x0001D928
		public Color GetColor()
		{
			if (this.color != default(Color))
			{
				return this.color;
			}
			if (this.colorGenerator != null)
			{
				return this.colorGenerator.NewRandomizedColor();
			}
			return default(Color);
		}

		// Token: 0x04000590 RID: 1424
		private BodyPartGroupDef bodyPartGroup;

		// Token: 0x04000591 RID: 1425
		private ApparelLayerDef apparelLayer;

		// Token: 0x04000592 RID: 1426
		private ThingDef apparelDef;

		// Token: 0x04000593 RID: 1427
		private string requiredTag;

		// Token: 0x04000594 RID: 1428
		private List<SpecificApparelRequirement.TagChance> alternateTagChoices;

		// Token: 0x04000595 RID: 1429
		private ThingDef stuff;

		// Token: 0x04000596 RID: 1430
		private ThingStyleDef styleDef;

		// Token: 0x04000597 RID: 1431
		private Color color;

		// Token: 0x04000598 RID: 1432
		private ColorGenerator colorGenerator;

		// Token: 0x04000599 RID: 1433
		private bool locked;

		// Token: 0x0400059A RID: 1434
		private bool biocode;

		// Token: 0x0400059B RID: 1435
		private QualityCategory? quality;

		// Token: 0x020018E6 RID: 6374
		public struct TagChance
		{
			// Token: 0x04005F6A RID: 24426
			public string tag;

			// Token: 0x04005F6B RID: 24427
			public float chance;
		}
	}
}
