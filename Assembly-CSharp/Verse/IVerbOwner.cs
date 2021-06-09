using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020008A5 RID: 2213
	public interface IVerbOwner
	{
		// Token: 0x17000887 RID: 2183
		// (get) Token: 0x060036E6 RID: 14054
		VerbTracker VerbTracker { get; }

		// Token: 0x17000888 RID: 2184
		// (get) Token: 0x060036E7 RID: 14055
		List<VerbProperties> VerbProperties { get; }

		// Token: 0x17000889 RID: 2185
		// (get) Token: 0x060036E8 RID: 14056
		List<Tool> Tools { get; }

		// Token: 0x1700088A RID: 2186
		// (get) Token: 0x060036E9 RID: 14057
		ImplementOwnerTypeDef ImplementOwnerTypeDef { get; }

		// Token: 0x060036EA RID: 14058
		string UniqueVerbOwnerID();

		// Token: 0x060036EB RID: 14059
		bool VerbsStillUsableBy(Pawn p);

		// Token: 0x1700088B RID: 2187
		// (get) Token: 0x060036EC RID: 14060
		Thing ConstantCaster { get; }
	}
}
