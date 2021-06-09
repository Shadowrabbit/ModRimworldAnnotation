using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200023C RID: 572
	public interface ICellBoolGiver
	{
		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x06000EAD RID: 3757
		Color Color { get; }

		// Token: 0x06000EAE RID: 3758
		bool GetCellBool(int index);

		// Token: 0x06000EAF RID: 3759
		Color GetCellExtraColor(int index);
	}
}
