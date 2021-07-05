using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000173 RID: 371
	public class PenMarkerState
	{
		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000A5F RID: 2655 RVA: 0x00038BE8 File Offset: 0x00036DE8
		public bool Enclosed
		{
			get
			{
				return this.Calc().Enclosed;
			}
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06000A60 RID: 2656 RVA: 0x00038BF5 File Offset: 0x00036DF5
		public bool Unenclosed
		{
			get
			{
				return !this.Enclosed;
			}
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000A61 RID: 2657 RVA: 0x00038C00 File Offset: 0x00036E00
		public bool PassableDoors
		{
			get
			{
				return this.Calc().PassableDoors;
			}
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000A62 RID: 2658 RVA: 0x00038C0D File Offset: 0x00036E0D
		public List<Region> DirectlyConnectedRegions
		{
			get
			{
				return this.Calc().DirectlyConnectedRegions;
			}
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06000A63 RID: 2659 RVA: 0x00038C1A File Offset: 0x00036E1A
		public HashSet<Region> ConnectedRegions
		{
			get
			{
				return this.Calc().ConnectedRegions;
			}
		}

		// Token: 0x06000A64 RID: 2660 RVA: 0x00038C27 File Offset: 0x00036E27
		public bool ContainsConnectedRegion(Region r)
		{
			return this.Calc().ContainsConnectedRegion(r);
		}

		// Token: 0x06000A65 RID: 2661 RVA: 0x00038C35 File Offset: 0x00036E35
		public PenMarkerState(CompAnimalPenMarker marker)
		{
			this.marker = marker;
		}

		// Token: 0x06000A66 RID: 2662 RVA: 0x00038C44 File Offset: 0x00036E44
		private AnimalPenEnclosureStateCalculator Calc()
		{
			if (this.state == null)
			{
				this.state = new AnimalPenEnclosureStateCalculator();
				this.state.Recalulate(this.marker.parent.Position, this.marker.parent.Map);
			}
			else if (this.state.NeedsRecalculation())
			{
				this.state.Recalulate(this.marker.parent.Position, this.marker.parent.Map);
			}
			return this.state;
		}

		// Token: 0x040008DE RID: 2270
		private readonly CompAnimalPenMarker marker;

		// Token: 0x040008DF RID: 2271
		private AnimalPenEnclosureStateCalculator state;
	}
}
