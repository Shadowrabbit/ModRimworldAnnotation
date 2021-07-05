using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020007CD RID: 1997
	public class CreditRecord_Space : CreditsEntry
	{
		// Token: 0x06003227 RID: 12839 RVA: 0x0002761A File Offset: 0x0002581A
		public CreditRecord_Space()
		{
		}

		// Token: 0x06003228 RID: 12840 RVA: 0x0002762D File Offset: 0x0002582D
		public CreditRecord_Space(float height)
		{
			this.height = height;
		}

		// Token: 0x06003229 RID: 12841 RVA: 0x00027647 File Offset: 0x00025847
		public override float DrawHeight(float width)
		{
			return this.height;
		}

		// Token: 0x0600322A RID: 12842 RVA: 0x00006A05 File Offset: 0x00004C05
		public override void Draw(Rect rect)
		{
		}

		// Token: 0x040022DB RID: 8923
		private float height = 10f;
	}
}
