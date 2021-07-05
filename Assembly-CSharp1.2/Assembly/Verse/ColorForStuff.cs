using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000E8 RID: 232
	public class ColorForStuff
	{
		// Token: 0x17000133 RID: 307
		// (get) Token: 0x060006D0 RID: 1744 RVA: 0x0000B9C7 File Offset: 0x00009BC7
		public ThingDef Stuff
		{
			get
			{
				return this.stuff;
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060006D1 RID: 1745 RVA: 0x0000B9CF File Offset: 0x00009BCF
		public Color Color
		{
			get
			{
				return this.color;
			}
		}

		// Token: 0x040003A8 RID: 936
		private ThingDef stuff;

		// Token: 0x040003A9 RID: 937
		private Color color = Color.white;
	}
}
