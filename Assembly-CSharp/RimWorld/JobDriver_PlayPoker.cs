using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B97 RID: 2967
	public class JobDriver_PlayPoker : JobDriver_SitFacingBuilding
	{
		// Token: 0x060045A9 RID: 17833 RVA: 0x00033212 File Offset: 0x00031412
		protected override void ModifyPlayToil(Toil toil)
		{
			base.ModifyPlayToil(toil);
			toil.WithEffect(() => EffecterDefOf.PlayPoker, () => base.TargetA.Thing.OccupiedRect().ClosestCellTo(this.pawn.Position));
		}
	}
}
