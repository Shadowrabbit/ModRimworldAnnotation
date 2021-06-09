using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A9B RID: 6811
	public class Dialog_ConfirmModUpload : Dialog_MessageBox
	{
		// Token: 0x06009675 RID: 38517 RVA: 0x002BCAD0 File Offset: 0x002BACD0
		public Dialog_ConfirmModUpload(ModMetaData mod, Action acceptAction) : base("ConfirmSteamWorkshopUpload".Translate(), "Confirm".Translate(), acceptAction, "GoBack".Translate(), null, null, true, acceptAction, null)
		{
			this.mod = mod;
		}

		// Token: 0x06009676 RID: 38518 RVA: 0x002BCB18 File Offset: 0x002BAD18
		public override void DoWindowContents(Rect inRect)
		{
			base.DoWindowContents(inRect);
			Vector2 vector = new Vector2(inRect.x + 10f, inRect.height - 35f - 24f - 10f);
			Widgets.Checkbox(vector, ref this.mod.translationMod, 24f, false, false, null, null);
			Widgets.Label(new Rect(vector.x + 24f + 10f, vector.y + (24f - Text.LineHeight) / 2f, inRect.width / 2f, 24f), "TagAsTranslation".Translate());
		}

		// Token: 0x04006001 RID: 24577
		private ModMetaData mod;
	}
}
