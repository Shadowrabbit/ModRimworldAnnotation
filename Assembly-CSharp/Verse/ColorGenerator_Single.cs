using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000F3 RID: 243
	public class ColorGenerator_Single : ColorGenerator
	{
		// Token: 0x0600070A RID: 1802 RVA: 0x0000BBCF File Offset: 0x00009DCF
		public override Color NewRandomizedColor()
		{
			return this.color;
		}

		// Token: 0x0400041C RID: 1052
		public Color color;
	}
}
