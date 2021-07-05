using System;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006EE RID: 1774
	public class JobDriver_PrepareSkylantern : JobDriver_GotoAndStandSociallyActive
	{
		// Token: 0x1700093A RID: 2362
		// (get) Token: 0x0600316E RID: 12654 RVA: 0x0011FE90 File Offset: 0x0011E090
		public override Toil StandToil
		{
			get
			{
				if (!ModLister.CheckIdeology("Skylantern job"))
				{
					return null;
				}
				Toil toil = base.StandToil.WithEffect(EffecterDefOf.MakingSkylantern, () => this.pawn.Position + this.pawn.Rotation.FacingCell, null);
				toil.AddPreInitAction(delegate
				{
					Thing thing = this.pawn.inventory.innerContainer.FirstOrDefault((Thing t) => t.def == this.job.thingDefToCarry && t.stackCount >= this.job.count);
					if (thing != null)
					{
						this.pawn.carryTracker.TryStartCarry(thing, this.job.count, true);
					}
				});
				return toil;
			}
		}
	}
}
