using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FFD RID: 4093
	public class TimeAssignmentDef : Def
	{
		// Token: 0x17000DCF RID: 3535
		// (get) Token: 0x06005943 RID: 22851 RVA: 0x0003DFBB File Offset: 0x0003C1BB
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

		// Token: 0x06005944 RID: 22852 RVA: 0x0003DFE2 File Offset: 0x0003C1E2
		public override void PostLoad()
		{
			base.PostLoad();
			this.cachedHighlightNotSelectedTag = "TimeAssignmentButton-" + this.defName + "-NotSelected";
		}

		// Token: 0x04003BD3 RID: 15315
		public Color color;

		// Token: 0x04003BD4 RID: 15316
		public bool allowRest = true;

		// Token: 0x04003BD5 RID: 15317
		public bool allowJoy = true;

		// Token: 0x04003BD6 RID: 15318
		[Unsaved(false)]
		public string cachedHighlightNotSelectedTag;

		// Token: 0x04003BD7 RID: 15319
		private Texture2D colorTextureInt;
	}
}
