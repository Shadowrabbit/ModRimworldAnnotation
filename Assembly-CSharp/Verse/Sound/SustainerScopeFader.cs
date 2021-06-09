using System;

namespace Verse.Sound
{
	// Token: 0x0200095C RID: 2396
	public class SustainerScopeFader
	{
		// Token: 0x06003AAC RID: 15020 RVA: 0x0016B0CC File Offset: 0x001692CC
		public void SustainerScopeUpdate()
		{
			if (this.inScope)
			{
				float num = this.inScopePercent + 0.05f;
				this.inScopePercent = num;
				if (this.inScopePercent > 1f)
				{
					this.inScopePercent = 1f;
					return;
				}
			}
			else
			{
				this.inScopePercent -= 0.03f;
				if (this.inScopePercent <= 0.001f)
				{
					this.inScopePercent = 0f;
				}
			}
		}

		// Token: 0x040028A7 RID: 10407
		public bool inScope = true;

		// Token: 0x040028A8 RID: 10408
		public float inScopePercent = 1f;

		// Token: 0x040028A9 RID: 10409
		private const float ScopeMatchFallRate = 0.03f;

		// Token: 0x040028AA RID: 10410
		private const float ScopeMatchRiseRate = 0.05f;
	}
}
