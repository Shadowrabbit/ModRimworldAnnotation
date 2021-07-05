using System;

namespace RimWorld
{
	// Token: 0x02000C76 RID: 3190
	public static class StoryDangerUtility
	{
		// Token: 0x06004A66 RID: 19046 RVA: 0x00189C68 File Offset: 0x00187E68
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
