using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02001727 RID: 5927
	public interface ISlateRef
	{
		// Token: 0x17001629 RID: 5673
		// (get) Token: 0x060088A7 RID: 34983
		// (set) Token: 0x060088A8 RID: 34984
		string SlateRef { get; set; }

		// Token: 0x060088A9 RID: 34985
		bool TryGetConvertedValue<T>(Slate slate, out T value);
	}
}
