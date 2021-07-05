using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200034F RID: 847
	public struct FleckDrawData
	{
		// Token: 0x04001082 RID: 4226
		public Vector3 pos;

		// Token: 0x04001083 RID: 4227
		public float rotation;

		// Token: 0x04001084 RID: 4228
		public Vector3 scale;

		// Token: 0x04001085 RID: 4229
		public float alpha;

		// Token: 0x04001086 RID: 4230
		public Color color;

		// Token: 0x04001087 RID: 4231
		public int drawLayer;

		// Token: 0x04001088 RID: 4232
		public Color? overrideColor;

		// Token: 0x04001089 RID: 4233
		public DrawBatchPropertyBlock propertyBlock;

		// Token: 0x0400108A RID: 4234
		public float ageSecs;

		// Token: 0x0400108B RID: 4235
		public float calculatedShockwaveSpan;
	}
}
