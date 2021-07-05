using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02002003 RID: 8195
	public interface ISlateRef
	{
		// Token: 0x17001987 RID: 6535
		// (get) Token: 0x0600AD91 RID: 44433
		// (set) Token: 0x0600AD92 RID: 44434
		string SlateRef { get; set; }

		// Token: 0x0600AD93 RID: 44435
		bool TryGetConvertedValue<T>(Slate slate, out T value);
	}
}
