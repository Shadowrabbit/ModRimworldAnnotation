using System;

namespace Verse
{
	// Token: 0x020001BD RID: 445
	public struct ThingRequest
	{
		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06000CC3 RID: 3267 RVA: 0x00043AF3 File Offset: 0x00041CF3
		public bool IsUndefined
		{
			get
			{
				return this.singleDef == null && this.group == ThingRequestGroup.Undefined;
			}
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x06000CC4 RID: 3268 RVA: 0x00043B08 File Offset: 0x00041D08
		public bool CanBeFoundInRegion
		{
			get
			{
				return !this.IsUndefined && (this.singleDef != null || this.group == ThingRequestGroup.Nothing || this.group.StoreInRegion());
			}
		}

		// Token: 0x06000CC5 RID: 3269 RVA: 0x00043B34 File Offset: 0x00041D34
		public static ThingRequest ForUndefined()
		{
			return new ThingRequest
			{
				singleDef = null,
				group = ThingRequestGroup.Undefined
			};
		}

		// Token: 0x06000CC6 RID: 3270 RVA: 0x00043B5C File Offset: 0x00041D5C
		public static ThingRequest ForDef(ThingDef singleDef)
		{
			return new ThingRequest
			{
				singleDef = singleDef,
				group = ThingRequestGroup.Undefined
			};
		}

		// Token: 0x06000CC7 RID: 3271 RVA: 0x00043B84 File Offset: 0x00041D84
		public static ThingRequest ForGroup(ThingRequestGroup group)
		{
			return new ThingRequest
			{
				singleDef = null,
				group = group
			};
		}

		// Token: 0x06000CC8 RID: 3272 RVA: 0x00043BAA File Offset: 0x00041DAA
		public bool Accepts(Thing t)
		{
			if (this.singleDef != null)
			{
				return t.def == this.singleDef;
			}
			return this.group == ThingRequestGroup.Everything || this.group.Includes(t.def);
		}

		// Token: 0x06000CC9 RID: 3273 RVA: 0x00043BE0 File Offset: 0x00041DE0
		public override string ToString()
		{
			string str;
			if (this.singleDef != null)
			{
				str = "singleDef " + this.singleDef.defName;
			}
			else
			{
				str = "group " + this.group.ToString();
			}
			return "ThingRequest(" + str + ")";
		}

		// Token: 0x04000A22 RID: 2594
		public ThingDef singleDef;

		// Token: 0x04000A23 RID: 2595
		public ThingRequestGroup group;
	}
}
