using System;

namespace Verse
{
	// Token: 0x020003CC RID: 972
	public class HediffComp_DisappearsOnDeath : HediffComp
	{
		// Token: 0x06001816 RID: 6166 RVA: 0x00016EBD File Offset: 0x000150BD
		public override void Notify_PawnDied()
		{
			base.Notify_PawnDied();
			base.Pawn.health.RemoveHediff(this.parent);
		}
	}
}
