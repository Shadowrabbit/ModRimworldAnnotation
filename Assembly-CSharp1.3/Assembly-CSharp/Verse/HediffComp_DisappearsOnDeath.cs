using System;

namespace Verse
{
	// Token: 0x0200028A RID: 650
	public class HediffComp_DisappearsOnDeath : HediffComp
	{
		// Token: 0x06001249 RID: 4681 RVA: 0x00069AC7 File Offset: 0x00067CC7
		public override void Notify_PawnDied()
		{
			base.Notify_PawnDied();
			base.Pawn.health.RemoveHediff(this.parent);
		}
	}
}
