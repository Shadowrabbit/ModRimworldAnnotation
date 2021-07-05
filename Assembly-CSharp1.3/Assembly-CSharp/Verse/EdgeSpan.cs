using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000203 RID: 515
	public struct EdgeSpan
	{
		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06000E96 RID: 3734 RVA: 0x00052E78 File Offset: 0x00051078
		public bool IsValid
		{
			get
			{
				return this.length > 0;
			}
		}

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06000E97 RID: 3735 RVA: 0x00052E83 File Offset: 0x00051083
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

		// Token: 0x06000E98 RID: 3736 RVA: 0x00052E98 File Offset: 0x00051098
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

		// Token: 0x06000E99 RID: 3737 RVA: 0x00052F00 File Offset: 0x00051100
		public EdgeSpan(IntVec3 root, SpanDirection dir, int length)
		{
			this.root = root;
			this.dir = dir;
			this.length = length;
		}

		// Token: 0x06000E9A RID: 3738 RVA: 0x00052F18 File Offset: 0x00051118
		public ulong UniqueHashCode()
		{
			ulong num = this.root.UniqueHashCode();
			if (this.dir == SpanDirection.East)
			{
				num += 17592186044416UL;
			}
			return num + (ulong)(281474976710656L * (long)this.length);
		}

		// Token: 0x04000BCA RID: 3018
		public IntVec3 root;

		// Token: 0x04000BCB RID: 3019
		public SpanDirection dir;

		// Token: 0x04000BCC RID: 3020
		public int length;
	}
}
