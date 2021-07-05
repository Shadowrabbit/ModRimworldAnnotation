using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02001368 RID: 4968
	public class MainTabWindow_Factions : MainTabWindow
	{
		// Token: 0x0600787F RID: 30847 RVA: 0x002A796A File Offset: 0x002A5B6A
		public override void PreOpen()
		{
			base.PreOpen();
			this.scrollToFaction = null;
		}

		// Token: 0x06007880 RID: 30848 RVA: 0x002A7979 File Offset: 0x002A5B79
		public void ScrollToFaction(Faction faction)
		{
			this.scrollToFaction = faction;
		}

		// Token: 0x06007881 RID: 30849 RVA: 0x002A7982 File Offset: 0x002A5B82
		public override void DoWindowContents(Rect fillRect)
		{
			FactionUIUtility.DoWindowContents(fillRect, ref this.scrollPosition, ref this.scrollViewHeight, this.scrollToFaction);
			if (this.scrollToFaction != null)
			{
				this.scrollToFaction = null;
			}
		}

		// Token: 0x040042F0 RID: 17136
		private Vector2 scrollPosition;

		// Token: 0x040042F1 RID: 17137
		private float scrollViewHeight;

		// Token: 0x040042F2 RID: 17138
		private Faction scrollToFaction;
	}
}
