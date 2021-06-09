using System;
using System.Linq;

namespace Verse
{
	// Token: 0x020002DB RID: 731
	public class RegionLink
	{
		// Token: 0x17000370 RID: 880
		// (get) Token: 0x0600129E RID: 4766 RVA: 0x00013603 File Offset: 0x00011803
		// (set) Token: 0x0600129F RID: 4767 RVA: 0x0001360D File Offset: 0x0001180D
		public Region RegionA
		{
			get
			{
				return this.regions[0];
			}
			set
			{
				this.regions[0] = value;
			}
		}

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x060012A0 RID: 4768 RVA: 0x00013618 File Offset: 0x00011818
		// (set) Token: 0x060012A1 RID: 4769 RVA: 0x00013622 File Offset: 0x00011822
		public Region RegionB
		{
			get
			{
				return this.regions[1];
			}
			set
			{
				this.regions[1] = value;
			}
		}

		// Token: 0x060012A2 RID: 4770 RVA: 0x000C7850 File Offset: 0x000C5A50
		public void Register(Region reg)
		{
			if (this.regions[0] == reg || this.regions[1] == reg)
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to double-register region ",
					reg.ToString(),
					" in ",
					this
				}), false);
				return;
			}
			if (this.RegionA == null || !this.RegionA.valid)
			{
				this.RegionA = reg;
				return;
			}
			if (this.RegionB == null || !this.RegionB.valid)
			{
				this.RegionB = reg;
				return;
			}
			Log.Error(string.Concat(new object[]
			{
				"Could not register region ",
				reg.ToString(),
				" in link ",
				this,
				": > 2 regions on link!\nRegionA: ",
				this.RegionA.DebugString,
				"\nRegionB: ",
				this.RegionB.DebugString
			}), false);
		}

		// Token: 0x060012A3 RID: 4771 RVA: 0x000C7938 File Offset: 0x000C5B38
		public void Deregister(Region reg)
		{
			if (this.RegionA == reg)
			{
				this.RegionA = null;
				if (this.RegionB == null)
				{
					reg.Map.regionLinkDatabase.Notify_LinkHasNoRegions(this);
					return;
				}
			}
			else if (this.RegionB == reg)
			{
				this.RegionB = null;
				if (this.RegionA == null)
				{
					reg.Map.regionLinkDatabase.Notify_LinkHasNoRegions(this);
				}
			}
		}

		// Token: 0x060012A4 RID: 4772 RVA: 0x0001362D File Offset: 0x0001182D
		public Region GetOtherRegion(Region reg)
		{
			if (reg != this.RegionA)
			{
				return this.RegionA;
			}
			return this.RegionB;
		}

		// Token: 0x060012A5 RID: 4773 RVA: 0x00013645 File Offset: 0x00011845
		public ulong UniqueHashCode()
		{
			return this.span.UniqueHashCode();
		}

		// Token: 0x060012A6 RID: 4774 RVA: 0x000C7998 File Offset: 0x000C5B98
		public override string ToString()
		{
			string text = (from r in this.regions
			where r != null
			select r.id.ToString()).ToCommaList(false);
			string text2 = string.Concat(new object[]
			{
				"span=",
				this.span.ToString(),
				" hash=",
				this.UniqueHashCode()
			});
			return string.Concat(new string[]
			{
				"(",
				text2,
				", regions=",
				text,
				")"
			});
		}

		// Token: 0x04000EF2 RID: 3826
		public Region[] regions = new Region[2];

		// Token: 0x04000EF3 RID: 3827
		public EdgeSpan span;
	}
}
