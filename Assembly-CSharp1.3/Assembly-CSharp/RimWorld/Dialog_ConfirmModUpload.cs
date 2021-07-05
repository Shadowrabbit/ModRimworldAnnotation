using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001323 RID: 4899
	public class Dialog_ConfirmModUpload : Dialog_MessageBox
	{
		// Token: 0x06007661 RID: 30305 RVA: 0x00290960 File Offset: 0x0028EB60
		public Dialog_ConfirmModUpload(ModMetaData mod, Action acceptAction) : base("ConfirmSteamWorkshopUpload".Translate(), "Confirm".Translate(), acceptAction, "GoBack".Translate(), null, null, true, acceptAction, null)
		{
			this.mod = mod;
		}

		// Token: 0x06007662 RID: 30306 RVA: 0x002909A8 File Offset: 0x0028EBA8
		public override void DoWindowContents(Rect inRect)
		{
			base.DoWindowContents(inRect);
			Vector2 vector = new Vector2(inRect.x + 10f, inRect.height - 35f - 24f - 10f);
			Widgets.Checkbox(vector, ref this.mod.translationMod, 24f, false, false, null, null);
			Widgets.Label(new Rect(vector.x + 24f + 10f, vector.y + (24f - Text.LineHeight) / 2f, inRect.width / 2f, 24f), "TagAsTranslation".Translate());
		}

		// Token: 0x040041C4 RID: 16836
		private ModMetaData mod;
	}
}
