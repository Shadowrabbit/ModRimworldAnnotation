using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02001B3A RID: 6970
	public class MainTabWindow_Factions : MainTabWindow
	{
		// Token: 0x06009977 RID: 39287 RVA: 0x0006647F File Offset: 0x0006467F
		public override void PreOpen()
		{
			this.scrollToFaction = null;
		}

		// Token: 0x06009978 RID: 39288 RVA: 0x00066488 File Offset: 0x00064688
		public void ScrollToFaction(Faction faction)
		{
			this.scrollToFaction = faction;
		}

		// Token: 0x06009979 RID: 39289 RVA: 0x00066491 File Offset: 0x00064691
		public override void DoWindowContents(Rect fillRect)
		{
			base.DoWindowContents(fillRect);
			FactionUIUtility.DoWindowContents_NewTemp(fillRect, ref this.scrollPosition, ref this.scrollViewHeight, this.scrollToFaction);
			if (this.scrollToFaction != null)
			{
				this.scrollToFaction = null;
			}
		}

		// Token: 0x04006200 RID: 25088
		private Vector2 scrollPosition = Vector2.zero;

		// Token: 0x04006201 RID: 25089
		private float scrollViewHeight;

		// Token: 0x04006202 RID: 25090
		private Faction scrollToFaction;
	}
}
