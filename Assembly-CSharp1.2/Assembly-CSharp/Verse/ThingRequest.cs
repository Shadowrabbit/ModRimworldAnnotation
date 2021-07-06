using System;

namespace Verse
{
	// Token: 0x02000278 RID: 632
	public struct ThingRequest
	{
		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06001057 RID: 4183 RVA: 0x00012297 File Offset: 0x00010497
		public bool IsUndefined
		{
			get
			{
				return this.singleDef == null && this.group == ThingRequestGroup.Undefined;
			}
		}

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06001058 RID: 4184 RVA: 0x000122AC File Offset: 0x000104AC
		public bool CanBeFoundInRegion
		{
			get
			{
				return !this.IsUndefined && (this.singleDef != null || this.group == ThingRequestGroup.Nothing || this.group.StoreInRegion());
			}
		}

		// Token: 0x06001059 RID: 4185 RVA: 0x000B912C File Offset: 0x000B732C
		public static ThingRequest ForUndefined()
		{
			return new ThingRequest
			{
				singleDef = null,
				group = ThingRequestGroup.Undefined
			};
		}

		// Token: 0x0600105A RID: 4186 RVA: 0x000B9154 File Offset: 0x000B7354
		public static ThingRequest ForDef(ThingDef singleDef)
		{
			return new ThingRequest
			{
				singleDef = singleDef,
				group = ThingRequestGroup.Undefined
			};
		}

		// Token: 0x0600105B RID: 4187 RVA: 0x000B917C File Offset: 0x000B737C
		public static ThingRequest ForGroup(ThingRequestGroup group)
		{
			return new ThingRequest
			{
				singleDef = null,
				group = group
			};
		}

		// Token: 0x0600105C RID: 4188 RVA: 0x000122D6 File Offset: 0x000104D6
		public bool Accepts(Thing t)
		{
			if (this.singleDef != null)
			{
				return t.def == this.singleDef;
			}
			return this.group == ThingRequestGroup.Everything || this.group.Includes(t.def);
		}

		// Token: 0x0600105D RID: 4189 RVA: 0x000B91A4 File Offset: 0x000B73A4
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

		// Token: 0x04000CF9 RID: 3321
		public ThingDef singleDef;

		// Token: 0x04000CFA RID: 3322
		public ThingRequestGroup group;
	}
}
