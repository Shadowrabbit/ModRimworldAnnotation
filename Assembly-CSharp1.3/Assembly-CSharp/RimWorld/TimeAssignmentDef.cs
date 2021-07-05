using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ADC RID: 2780
	public class TimeAssignmentDef : Def
	{
		// Token: 0x17000B84 RID: 2948
		// (get) Token: 0x06004181 RID: 16769 RVA: 0x0015FAA7 File Offset: 0x0015DCA7
		public Texture2D ColorTexture
		{
			get
			{
				if (this.colorTextureInt == null)
				{
					this.colorTextureInt = SolidColorMaterials.NewSolidColorTexture(this.color);
				}
				return this.colorTextureInt;
			}
		}

		// Token: 0x06004182 RID: 16770 RVA: 0x0015FACE File Offset: 0x0015DCCE
		public override void PostLoad()
		{
			base.PostLoad();
			this.cachedHighlightNotSelectedTag = "TimeAssignmentButton-" + this.defName + "-NotSelected";
		}

		// Token: 0x04002787 RID: 10119
		public Color color;

		// Token: 0x04002788 RID: 10120
		public bool allowRest = true;

		// Token: 0x04002789 RID: 10121
		public bool allowJoy = true;

		// Token: 0x0400278A RID: 10122
		[Unsaved(false)]
		public string cachedHighlightNotSelectedTag;

		// Token: 0x0400278B RID: 10123
		private Texture2D colorTextureInt;
	}
}
