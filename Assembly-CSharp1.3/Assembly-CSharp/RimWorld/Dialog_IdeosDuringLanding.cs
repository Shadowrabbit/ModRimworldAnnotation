using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012EB RID: 4843
	public class Dialog_IdeosDuringLanding : Window
	{
		// Token: 0x1700145F RID: 5215
		// (get) Token: 0x06007439 RID: 29753 RVA: 0x002762EF File Offset: 0x002744EF
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1010f, Mathf.Min(1000f, (float)UI.screenHeight));
			}
		}

		// Token: 0x0600743A RID: 29754 RVA: 0x0027630B File Offset: 0x0027450B
		public Dialog_IdeosDuringLanding()
		{
			this.doCloseButton = true;
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x0600743B RID: 29755 RVA: 0x00276328 File Offset: 0x00274528
		public override void DoWindowContents(Rect inRect)
		{
			IdeoUIUtility.DoIdeoListAndDetails(new Rect(inRect.x, inRect.y, inRect.width, inRect.height - Window.CloseButSize.y), ref this.scrollPosition_ideoList, ref this.scrollViewHeight_ideoList, ref this.scrollPosition_ideoDetails, ref this.scrollViewHeight_ideoDetails, false, false, null, null);
		}

		// Token: 0x04003FF9 RID: 16377
		private Vector2 scrollPosition_ideoList;

		// Token: 0x04003FFA RID: 16378
		private float scrollViewHeight_ideoList;

		// Token: 0x04003FFB RID: 16379
		private Vector2 scrollPosition_ideoDetails;

		// Token: 0x04003FFC RID: 16380
		private float scrollViewHeight_ideoDetails;
	}
}
