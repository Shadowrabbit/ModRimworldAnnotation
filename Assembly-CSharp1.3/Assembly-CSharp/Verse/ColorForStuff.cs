using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200008A RID: 138
	public class ColorForStuff
	{
		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060004F0 RID: 1264 RVA: 0x00019EE3 File Offset: 0x000180E3
		public ThingDef Stuff
		{
			get
			{
				return this.stuff;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060004F1 RID: 1265 RVA: 0x00019EEB File Offset: 0x000180EB
		public Color Color
		{
			get
			{
				return this.color;
			}
		}

		// Token: 0x040001E2 RID: 482
		private ThingDef stuff;

		// Token: 0x040001E3 RID: 483
		private Color color = Color.white;
	}
}
