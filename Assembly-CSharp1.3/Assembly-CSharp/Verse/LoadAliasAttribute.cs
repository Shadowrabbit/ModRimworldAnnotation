using System;

namespace Verse
{
	// Token: 0x020003DA RID: 986
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
	public class LoadAliasAttribute : Attribute
	{
		// Token: 0x06001DE2 RID: 7650 RVA: 0x000BAE4A File Offset: 0x000B904A
		public LoadAliasAttribute(string alias)
		{
			this.alias = alias;
		}

		// Token: 0x040011ED RID: 4589
		public string alias;
	}
}
