using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000467 RID: 1127
	internal class BlackScreenFixer : MonoBehaviour
	{
		// Token: 0x0600223A RID: 8762 RVA: 0x000D908C File Offset: 0x000D728C
		private void Start()
		{
			if (Screen.width != 0 && Screen.height != 0)
			{
				Screen.SetResolution(Screen.width, Screen.height, Screen.fullScreen);
			}
		}
	}
}
