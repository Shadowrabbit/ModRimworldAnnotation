using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000091 RID: 145
	public abstract class ColorGenerator
	{
		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000513 RID: 1299 RVA: 0x0001A4B0 File Offset: 0x000186B0
		public virtual Color ExemplaryColor
		{
			get
			{
				Rand.PushState(764543439);
				Color result = this.NewRandomizedColor();
				Rand.PopState();
				return result;
			}
		}

		// Token: 0x06000514 RID: 1300
		public abstract Color NewRandomizedColor();
	}
}
