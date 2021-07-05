using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000171 RID: 369
	public class AnimalPenEnclosureStateCalculator : AnimalPenEnclosureCalculator
	{
		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06000A4D RID: 2637 RVA: 0x00038812 File Offset: 0x00036A12
		public bool Enclosed
		{
			get
			{
				return this.enclosed;
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06000A4E RID: 2638 RVA: 0x0003881A File Offset: 0x00036A1A
		public bool IndirectlyConnected
		{
			get
			{
				return this.indirectlyConnected;
			}
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x06000A4F RID: 2639 RVA: 0x00038822 File Offset: 0x00036A22
		public bool PassableDoors
		{
			get
			{
				return this.passableDoors.Any<Building_Door>();
			}
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06000A50 RID: 2640 RVA: 0x0003882F File Offset: 0x00036A2F
		public List<Region> DirectlyConnectedRegions
		{
			get
			{
				return this.directlyConnectedRegions;
			}
		}

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06000A51 RID: 2641 RVA: 0x00038837 File Offset: 0x00036A37
		public HashSet<Region> ConnectedRegions
		{
			get
			{
				return this.connectedRegions;
			}
		}

		// Token: 0x06000A52 RID: 2642 RVA: 0x0003883F File Offset: 0x00036A3F
		public bool ContainsConnectedRegion(Region r)
		{
			return this.connectedRegions.Contains(r);
		}

		// Token: 0x06000A53 RID: 2643 RVA: 0x00038850 File Offset: 0x00036A50
		public bool NeedsRecalculation()
		{
			using (List<Building_Door>.Enumerator enumerator = this.passableDoors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!AnimalPenEnclosureCalculator.RoamerCanPass(enumerator.Current))
					{
						return true;
					}
				}
			}
			using (List<Building_Door>.Enumerator enumerator = this.impassableDoors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (AnimalPenEnclosureCalculator.RoamerCanPass(enumerator.Current))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000A54 RID: 2644 RVA: 0x000388F0 File Offset: 0x00036AF0
		public void Recalulate(IntVec3 position, Map map)
		{
			this.indirectlyConnected = false;
			this.passableDoors.Clear();
			this.impassableDoors.Clear();
			this.connectedRegions.Clear();
			this.directlyConnectedRegions.Clear();
			this.enclosed = base.VisitPen(position, map);
		}

		// Token: 0x06000A55 RID: 2645 RVA: 0x0003893E File Offset: 0x00036B3E
		protected override void VisitDirectlyConnectedRegion(Region r)
		{
			this.connectedRegions.Add(r);
			this.directlyConnectedRegions.Add(r);
		}

		// Token: 0x06000A56 RID: 2646 RVA: 0x00038959 File Offset: 0x00036B59
		protected override void VisitIndirectlyDirectlyConnectedRegion(Region r)
		{
			this.indirectlyConnected = true;
			this.connectedRegions.Add(r);
		}

		// Token: 0x06000A57 RID: 2647 RVA: 0x0003896F File Offset: 0x00036B6F
		protected override void VisitPassableDoorway(Region r)
		{
			this.connectedRegions.Add(r);
			this.passableDoors.Add(r.door);
		}

		// Token: 0x06000A58 RID: 2648 RVA: 0x0003898F File Offset: 0x00036B8F
		protected override void VisitImpassableDoorway(Region r)
		{
			this.impassableDoors.Add(r.door);
		}

		// Token: 0x040008D1 RID: 2257
		private bool enclosed;

		// Token: 0x040008D2 RID: 2258
		private bool indirectlyConnected;

		// Token: 0x040008D3 RID: 2259
		private List<Building_Door> passableDoors = new List<Building_Door>();

		// Token: 0x040008D4 RID: 2260
		private List<Building_Door> impassableDoors = new List<Building_Door>();

		// Token: 0x040008D5 RID: 2261
		private List<Region> directlyConnectedRegions = new List<Region>();

		// Token: 0x040008D6 RID: 2262
		private HashSet<Region> connectedRegions = new HashSet<Region>();
	}
}
