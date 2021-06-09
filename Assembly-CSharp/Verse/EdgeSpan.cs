using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020002D9 RID: 729
	public struct EdgeSpan
	{
		// Token: 0x1700036C RID: 876
		// (get) Token: 0x06001291 RID: 4753 RVA: 0x00013595 File Offset: 0x00011795
		public bool IsValid
		{
			get
			{
				return this.length > 0;
			}
		}

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x06001292 RID: 4754 RVA: 0x000135A0 File Offset: 0x000117A0
		public IEnumerable<IntVec3> Cells
		{
			get
			{
				int num;
				for (int i = 0; i < this.length; i = num + 1)
				{
					if (this.dir == SpanDirection.North)
					{
						yield return new IntVec3(this.root.x, 0, this.root.z + i);
					}
					else if (this.dir == SpanDirection.East)
					{
						yield return new IntVec3(this.root.x + i, 0, this.root.z);
					}
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x06001293 RID: 4755 RVA: 0x000C7658 File Offset: 0x000C5858
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(root=",
				this.root,
				", dir=",
				this.dir.ToString(),
				" + length=",
				this.length,
				")"
			});
		}

		// Token: 0x06001294 RID: 4756 RVA: 0x000135B5 File Offset: 0x000117B5
		public EdgeSpan(IntVec3 root, SpanDirection dir, int length)
		{
			this.root = root;
			this.dir = dir;
			this.length = length;
		}

		// Token: 0x06001295 RID: 4757 RVA: 0x000C76C0 File Offset: 0x000C58C0
		public ulong UniqueHashCode()
		{
			ulong num = this.root.UniqueHashCode();
			if (this.dir == SpanDirection.East)
			{
				num += 17592186044416UL;
			}
			return num + (ulong)(281474976710656L * (long)this.length);
		}

		// Token: 0x04000EE9 RID: 3817
		public IntVec3 root;

		// Token: 0x04000EEA RID: 3818
		public SpanDirection dir;

		// Token: 0x04000EEB RID: 3819
		public int length;
	}
}
