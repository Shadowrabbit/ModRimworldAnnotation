using System;

namespace Verse
{
	// Token: 0x020006EA RID: 1770
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
	public class LoadAliasAttribute : Attribute
	{
		// Token: 0x06002D18 RID: 11544 RVA: 0x00023A0E File Offset: 0x00021C0E
		public LoadAliasAttribute(string alias)
		{
			this.alias = alias;
		}

		// Token: 0x04001E84 RID: 7812
		public string alias;
	}
}
