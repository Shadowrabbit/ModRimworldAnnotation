using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200180D RID: 6157
	public class CompProximityFuse : ThingComp
	{
		// Token: 0x17001540 RID: 5440
		// (get) Token: 0x06008845 RID: 34885 RVA: 0x0005B814 File Offset: 0x00059A14
		public CompProperties_ProximityFuse Props
		{
			get
			{
				return (CompProperties_ProximityFuse)this.props;
			}
		}

		// Token: 0x06008846 RID: 34886 RVA: 0x0005B821 File Offset: 0x00059A21
		public override void CompTick()
		{
			if (Find.TickManager.TicksGame % 250 == 0)
			{
				this.CompTickRare();
			}
		}

		// Token: 0x06008847 RID: 34887 RVA: 0x0027E55C File Offset: 0x0027C75C
		public override void CompTickRare()
		{
			if (GenClosest.ClosestThingReachable(this.parent.Position, this.parent.Map, ThingRequest.ForDef(this.Props.target), PathEndMode.OnCell, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), this.Props.radius, null, null, 0, -1, false, RegionType.Set_Passable, false) != null)
			{
				this.parent.GetComp<CompExplosive>().StartWick(null);
			}
		}
	}
}
