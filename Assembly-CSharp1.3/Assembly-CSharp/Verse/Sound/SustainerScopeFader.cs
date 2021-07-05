using System;

namespace Verse.Sound
{
	// Token: 0x02000580 RID: 1408
	public class SustainerScopeFader
	{
		// Token: 0x06002953 RID: 10579 RVA: 0x000FA214 File Offset: 0x000F8414
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

		// Token: 0x0400199E RID: 6558
		public bool inScope = true;

		// Token: 0x0400199F RID: 6559
		public float inScopePercent = 1f;

		// Token: 0x040019A0 RID: 6560
		private const float ScopeMatchFallRate = 0.03f;

		// Token: 0x040019A1 RID: 6561
		private const float ScopeMatchRiseRate = 0.05f;
	}
}
