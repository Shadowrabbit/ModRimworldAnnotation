using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001174 RID: 4468
	public class CompProximityFuse : ThingComp
	{
		// Token: 0x1700127C RID: 4732
		// (get) Token: 0x06006B61 RID: 27489 RVA: 0x00240E67 File Offset: 0x0023F067
		public CompProperties_ProximityFuse Props
		{
			get
			{
				return (CompProperties_ProximityFuse)this.props;
			}
		}

		// Token: 0x06006B62 RID: 27490 RVA: 0x00240E74 File Offset: 0x0023F074
		public override void CompTick()
		{
			if (Find.TickManager.TicksGame % 250 == 0)
			{
				this.CompTickRare();
			}
		}

		// Token: 0x06006B63 RID: 27491 RVA: 0x00240E90 File Offset: 0x0023F090
		public override void CompTickRare()
		{
			if (GenClosest.ClosestThingReachable(this.parent.Position, this.parent.Map, ThingRequest.ForDef(this.Props.target), PathEndMode.OnCell, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false), this.Props.radius, null, null, 0, -1, false, RegionType.Set_Passable, false) != null)
			{
				this.parent.GetComp<CompExplosive>().StartWick(null);
			}
		}
	}
}
