using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000A2 RID: 162
	public class AsymmetricLinkData
	{
		// Token: 0x040002A5 RID: 677
		public LinkFlags linkFlags;

		// Token: 0x040002A6 RID: 678
		public bool linkToDoors;

		// Token: 0x040002A7 RID: 679
		public AsymmetricLinkData.BorderData drawDoorBorderEast;

		// Token: 0x040002A8 RID: 680
		public AsymmetricLinkData.BorderData drawDoorBorderWest;

		// Token: 0x020018C7 RID: 6343
		public class BorderData
		{
			// Token: 0x170018A1 RID: 6305
			// (get) Token: 0x060094E9 RID: 38121 RVA: 0x003509A2 File Offset: 0x0034EBA2
			public Material Mat
			{
				get
				{
					if (this.colorMat == null)
					{
						this.colorMat = SolidColorMaterials.SimpleSolidColorMaterial(this.color, false);
					}
					return this.colorMat;
				}
			}

			// Token: 0x04005ECE RID: 24270
			public Color color = Color.black;

			// Token: 0x04005ECF RID: 24271
			public Vector2 size;

			// Token: 0x04005ED0 RID: 24272
			public Vector3 offset;

			// Token: 0x04005ED1 RID: 24273
			private Material colorMat;
		}
	}
}
