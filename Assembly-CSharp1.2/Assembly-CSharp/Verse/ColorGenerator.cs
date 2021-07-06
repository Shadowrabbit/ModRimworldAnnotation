using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000F1 RID: 241
	public abstract class ColorGenerator
	{
		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000705 RID: 1797 RVA: 0x0000BBA9 File Offset: 0x00009DA9
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

		// Token: 0x06000706 RID: 1798
		public abstract Color NewRandomizedColor();
	}
}
