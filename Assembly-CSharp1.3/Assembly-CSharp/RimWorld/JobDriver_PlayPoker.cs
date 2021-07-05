using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006FC RID: 1788
	public class JobDriver_PlayPoker : JobDriver_SitFacingBuilding
	{
		// Token: 0x060031B1 RID: 12721 RVA: 0x00120E7C File Offset: 0x0011F07C
		protected override void ModifyPlayToil(Toil toil)
		{
			base.ModifyPlayToil(toil);
			toil.WithEffect(() => EffecterDefOf.PlayPoker, () => base.TargetA.Thing.OccupiedRect().ClosestCellTo(this.pawn.Position), null);
		}
	}
}
