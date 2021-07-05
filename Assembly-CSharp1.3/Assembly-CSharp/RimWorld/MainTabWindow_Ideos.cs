using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200136A RID: 4970
	public class MainTabWindow_Ideos : MainTabWindow
	{
		// Token: 0x17001536 RID: 5430
		// (get) Token: 0x0600788C RID: 30860 RVA: 0x002A89E9 File Offset: 0x002A6BE9
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(base.InitialSize.x, (float)(UI.screenHeight - 35));
			}
		}

		// Token: 0x0600788D RID: 30861 RVA: 0x002A8A04 File Offset: 0x002A6C04
		public override void PreOpen()
		{
			base.PreOpen();
			this.scrollPosition_ideoDetails = Vector2.zero;
		}

		// Token: 0x0600788E RID: 30862 RVA: 0x0026F782 File Offset: 0x0026D982
		public override void PostClose()
		{
			base.PostClose();
			IdeoUIUtility.UnselectCurrent();
		}

		// Token: 0x0600788F RID: 30863 RVA: 0x002A8A18 File Offset: 0x002A6C18
		public override void DoWindowContents(Rect rect)
		{
			IdeoUIUtility.DoIdeoListAndDetails(rect, ref this.scrollPosition_ideoList, ref this.scrollViewHeight_ideoList, ref this.scrollPosition_ideoDetails, ref this.scrollViewHeight_ideoDetails, false, false, null, null);
		}

		// Token: 0x04004307 RID: 17159
		private Vector2 scrollPosition_ideoList;

		// Token: 0x04004308 RID: 17160
		private float scrollViewHeight_ideoList;

		// Token: 0x04004309 RID: 17161
		private Vector2 scrollPosition_ideoDetails;

		// Token: 0x0400430A RID: 17162
		private float scrollViewHeight_ideoDetails;
	}
}
