using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000170 RID: 368
	public class AnimalPenConnectedDistrictsCalculator : AnimalPenEnclosureCalculator
	{
		// Token: 0x06000A46 RID: 2630 RVA: 0x00038791 File Offset: 0x00036991
		protected override void VisitDirectlyConnectedRegion(Region r)
		{
			this.AddDistrict(r);
		}

		// Token: 0x06000A47 RID: 2631 RVA: 0x00038791 File Offset: 0x00036991
		protected override void VisitIndirectlyDirectlyConnectedRegion(Region r)
		{
			this.AddDistrict(r);
		}

		// Token: 0x06000A48 RID: 2632 RVA: 0x00038791 File Offset: 0x00036991
		protected override void VisitPassableDoorway(Region r)
		{
			this.AddDistrict(r);
		}

		// Token: 0x06000A49 RID: 2633 RVA: 0x0003879C File Offset: 0x0003699C
		private void AddDistrict(Region r)
		{
			District district = r.District;
			if (!this.districts.Contains(district))
			{
				this.districts.Add(district);
			}
		}

		// Token: 0x06000A4A RID: 2634 RVA: 0x000387CA File Offset: 0x000369CA
		public List<District> CalculateConnectedDistricts(IntVec3 position, Map map)
		{
			this.districts.Clear();
			if (!base.VisitPen(position, map))
			{
				this.districts.Clear();
			}
			return this.districts;
		}

		// Token: 0x06000A4B RID: 2635 RVA: 0x000387F2 File Offset: 0x000369F2
		public void Reset()
		{
			this.districts.Clear();
		}

		// Token: 0x040008D0 RID: 2256
		private readonly List<District> districts = new List<District>();
	}
}
