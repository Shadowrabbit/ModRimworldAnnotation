using System;

namespace RimWorld
{
	// Token: 0x020015F2 RID: 5618
	public abstract class ScenPart_Rule : ScenPart
	{
		// Token: 0x06007A14 RID: 31252 RVA: 0x00052295 File Offset: 0x00050495
		public override void PostGameStart()
		{
			this.ApplyRule();
		}

		// Token: 0x06007A15 RID: 31253
		protected abstract void ApplyRule();
	}
}
