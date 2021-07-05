using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000093 RID: 147
	public class ColorGenerator_Single : ColorGenerator
	{
		// Token: 0x06000518 RID: 1304 RVA: 0x0001A4D6 File Offset: 0x000186D6
		public override Color NewRandomizedColor()
		{
			return this.color;
		}

		// Token: 0x04000253 RID: 595
		public Color color;
	}
}
