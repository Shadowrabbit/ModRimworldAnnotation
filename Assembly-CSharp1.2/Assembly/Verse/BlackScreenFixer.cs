using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020007C1 RID: 1985
	internal class BlackScreenFixer : MonoBehaviour
	{
		// Token: 0x06003201 RID: 12801 RVA: 0x00027422 File Offset: 0x00025622
		private void Start()
		{
			if (Screen.width != 0 && Screen.height != 0)
			{
				Screen.SetResolution(Screen.width, Screen.height, Screen.fullScreen);
			}
		}
	}
}
