using System;

namespace RimWorld
{
	// Token: 0x02001260 RID: 4704
	public static class StoryDangerUtility
	{
		// Token: 0x06006695 RID: 26261 RVA: 0x00046164 File Offset: 0x00044364
		public static float Scale(this StoryDanger d)
		{
			switch (d)
			{
			case StoryDanger.None:
				return 0f;
			case StoryDanger.Low:
				return 1f;
			case StoryDanger.High:
				return 2f;
			default:
				return 0f;
			}
		}
	}
}
