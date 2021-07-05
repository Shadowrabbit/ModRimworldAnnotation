using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000180 RID: 384
	public interface ICellBoolGiver
	{
		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000AF8 RID: 2808
		Color Color { get; }

		// Token: 0x06000AF9 RID: 2809
		bool GetCellBool(int index);

		// Token: 0x06000AFA RID: 2810
		Color GetCellExtraColor(int index);
	}
}
