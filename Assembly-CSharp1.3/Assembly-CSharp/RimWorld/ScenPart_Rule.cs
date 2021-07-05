using System;

namespace RimWorld
{
	// Token: 0x02001005 RID: 4101
	public abstract class ScenPart_Rule : ScenPart
	{
		// Token: 0x0600609C RID: 24732 RVA: 0x0020E640 File Offset: 0x0020C840
		public override void PostGameStart()
		{
			this.ApplyRule();
		}

		// Token: 0x0600609D RID: 24733
		protected abstract void ApplyRule();
	}
}
