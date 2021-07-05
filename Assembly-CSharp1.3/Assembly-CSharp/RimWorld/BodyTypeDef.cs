using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A55 RID: 2645
	public class BodyTypeDef : Def
	{
		// Token: 0x04002357 RID: 9047
		[NoTranslate]
		public string bodyNakedGraphicPath;

		// Token: 0x04002358 RID: 9048
		[NoTranslate]
		public string bodyDessicatedGraphicPath;

		// Token: 0x04002359 RID: 9049
		public List<BodyTypeDef.WoundAnchor> woundAnchors;

		// Token: 0x0400235A RID: 9050
		public Vector2 headOffset;

		// Token: 0x02002007 RID: 8199
		public enum WoundLayer
		{
			// Token: 0x04007AB3 RID: 31411
			Body,
			// Token: 0x04007AB4 RID: 31412
			Head
		}

		// Token: 0x02002008 RID: 8200
		public class WoundAnchor
		{
			// Token: 0x04007AB5 RID: 31413
			[NoTranslate]
			public string tag;

			// Token: 0x04007AB6 RID: 31414
			public BodyPartGroupDef group;

			// Token: 0x04007AB7 RID: 31415
			public CrownType? crownType;

			// Token: 0x04007AB8 RID: 31416
			public Rot4? rotation;

			// Token: 0x04007AB9 RID: 31417
			public bool canMirror = true;

			// Token: 0x04007ABA RID: 31418
			public Vector3 offset;

			// Token: 0x04007ABB RID: 31419
			public BodyTypeDef.WoundLayer layer;

			// Token: 0x04007ABC RID: 31420
			public Color debugColor;

			// Token: 0x04007ABD RID: 31421
			public float range;
		}
	}
}
