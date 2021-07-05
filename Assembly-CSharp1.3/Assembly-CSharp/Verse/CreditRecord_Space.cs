using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000472 RID: 1138
	public class CreditRecord_Space : CreditsEntry
	{
		// Token: 0x0600225F RID: 8799 RVA: 0x000DA5D6 File Offset: 0x000D87D6
		public CreditRecord_Space()
		{
		}

		// Token: 0x06002260 RID: 8800 RVA: 0x000DA5E9 File Offset: 0x000D87E9
		public CreditRecord_Space(float height)
		{
			this.height = height;
		}

		// Token: 0x06002261 RID: 8801 RVA: 0x000DA603 File Offset: 0x000D8803
		public override float DrawHeight(float width)
		{
			return this.height;
		}

		// Token: 0x06002262 RID: 8802 RVA: 0x0000313F File Offset: 0x0000133F
		public override void Draw(Rect rect)
		{
		}

		// Token: 0x040015B4 RID: 5556
		private float height = 10f;
	}
}
