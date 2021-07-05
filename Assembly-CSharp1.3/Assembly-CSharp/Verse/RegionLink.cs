using System;
using System.Linq;

namespace Verse
{
	// Token: 0x02000204 RID: 516
	public class RegionLink
	{
		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06000E9B RID: 3739 RVA: 0x00052F5B File Offset: 0x0005115B
		// (set) Token: 0x06000E9C RID: 3740 RVA: 0x00052F65 File Offset: 0x00051165
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

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06000E9D RID: 3741 RVA: 0x00052F70 File Offset: 0x00051170
		// (set) Token: 0x06000E9E RID: 3742 RVA: 0x00052F7A File Offset: 0x0005117A
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

		// Token: 0x06000E9F RID: 3743 RVA: 0x00052F88 File Offset: 0x00051188
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
				}));
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
			}));
		}

		// Token: 0x06000EA0 RID: 3744 RVA: 0x0005306C File Offset: 0x0005126C
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

		// Token: 0x06000EA1 RID: 3745 RVA: 0x000530CC File Offset: 0x000512CC
		public Region GetOtherRegion(Region reg)
		{
			if (reg != this.RegionA)
			{
				return this.RegionA;
			}
			return this.RegionB;
		}

		// Token: 0x06000EA2 RID: 3746 RVA: 0x000530E4 File Offset: 0x000512E4
		public ulong UniqueHashCode()
		{
			return this.span.UniqueHashCode();
		}

		// Token: 0x06000EA3 RID: 3747 RVA: 0x000530F4 File Offset: 0x000512F4
		public override string ToString()
		{
			string text = (from r in this.regions
			where r != null
			select r.id.ToString()).ToCommaList(false, false);
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

		// Token: 0x04000BCD RID: 3021
		public Region[] regions = new Region[2];

		// Token: 0x04000BCE RID: 3022
		public EdgeSpan span;
	}
}
