using System;
using Steamworks;
using UnityEngine;

namespace Verse.Steam
{
	// Token: 0x020004FC RID: 1276
	public class Dialog_WorkshopOperationInProgress : Window
	{
		// Token: 0x17000790 RID: 1936
		// (get) Token: 0x060026B2 RID: 9906 RVA: 0x000BA2A6 File Offset: 0x000B84A6
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(600f, 400f);
			}
		}

		// Token: 0x060026B3 RID: 9907 RVA: 0x000EFF7F File Offset: 0x000EE17F
		public Dialog_WorkshopOperationInProgress()
		{
			this.forcePause = true;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.absorbInputAroundWindow = true;
			this.preventDrawTutor = true;
		}

		// Token: 0x060026B4 RID: 9908 RVA: 0x000EFFAC File Offset: 0x000EE1AC
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

		// Token: 0x060026B5 RID: 9909 RVA: 0x000F0040 File Offset: 0x000EE240
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
