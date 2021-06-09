using System;
using Steamworks;
using UnityEngine;

namespace Verse.Steam
{
	// Token: 0x020008B6 RID: 2230
	public class Dialog_WorkshopOperationInProgress : Window
	{
		// Token: 0x170008B5 RID: 2229
		// (get) Token: 0x06003779 RID: 14201 RVA: 0x000238B0 File Offset: 0x00021AB0
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(600f, 400f);
			}
		}

		// Token: 0x0600377A RID: 14202 RVA: 0x0002AEFB File Offset: 0x000290FB
		public Dialog_WorkshopOperationInProgress()
		{
			this.forcePause = true;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.absorbInputAroundWindow = true;
			this.preventDrawTutor = true;
		}

		// Token: 0x0600377B RID: 14203 RVA: 0x00160C14 File Offset: 0x0015EE14
		public override void DoWindowContents(Rect inRect)
		{
			EItemUpdateStatus eitemUpdateStatus;
			float num;
			Workshop.GetUpdateStatus(out eitemUpdateStatus, out num);
			WorkshopInteractStage curStage = Workshop.CurStage;
			if (curStage == WorkshopInteractStage.None && eitemUpdateStatus == EItemUpdateStatus.k_EItemUpdateStatusInvalid)
			{
				this.Close(true);
				return;
			}
			string text = "";
			if (curStage != WorkshopInteractStage.None)
			{
				text += curStage.GetLabel();
				text += "\n\n";
			}
			if (eitemUpdateStatus != EItemUpdateStatus.k_EItemUpdateStatusInvalid)
			{
				text += eitemUpdateStatus.GetLabel();
				if (num > 0f)
				{
					text = text + " (" + num.ToStringPercent() + ")";
				}
				text += GenText.MarchingEllipsis(0f);
			}
			Widgets.Label(inRect, text);
		}

		// Token: 0x0600377C RID: 14204 RVA: 0x00160CA8 File Offset: 0x0015EEA8
		public static void CloseAll()
		{
			Dialog_WorkshopOperationInProgress dialog_WorkshopOperationInProgress = Find.WindowStack.WindowOfType<Dialog_WorkshopOperationInProgress>();
			if (dialog_WorkshopOperationInProgress != null)
			{
				dialog_WorkshopOperationInProgress.Close(true);
			}
		}
	}
}
