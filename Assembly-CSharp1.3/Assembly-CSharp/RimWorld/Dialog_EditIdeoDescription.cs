using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020012E3 RID: 4835
	public class Dialog_EditIdeoDescription : Window
	{
		// Token: 0x17001446 RID: 5190
		// (get) Token: 0x060073C0 RID: 29632 RVA: 0x00270042 File Offset: 0x0026E242
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(700f, 400f);
			}
		}

		// Token: 0x060073C1 RID: 29633 RVA: 0x00270053 File Offset: 0x0026E253
		public Dialog_EditIdeoDescription(Ideo ideo)
		{
			this.ideo = ideo;
			this.newDescription = ideo.description;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x060073C2 RID: 29634 RVA: 0x00270075 File Offset: 0x0026E275
		public override void OnAcceptKeyPressed()
		{
			Event.current.Use();
		}

		// Token: 0x060073C3 RID: 29635 RVA: 0x00270084 File Offset: 0x0026E284
		public override void DoWindowContents(Rect rect)
		{
			Text.Font = GameFont.Medium;
			Widgets.Label(new Rect(rect.x, rect.y, rect.width, 35f), "EditNarrative".Translate());
			Text.Font = GameFont.Small;
			float num = rect.y + 35f + 10f;
			string a = Widgets.TextArea(new Rect(rect.x, num, rect.width, rect.height - Dialog_EditIdeoDescription.ButSize.y - 17f - num), this.newDescription, false);
			if (a != this.newDescription)
			{
				this.newDescription = a;
				this.newDescriptionTemplate = null;
			}
			if (Widgets.ButtonText(new Rect(0f, rect.height - Dialog_EditIdeoDescription.ButSize.y, Dialog_EditIdeoDescription.ButSize.x, Dialog_EditIdeoDescription.ButSize.y), "Cancel".Translate(), true, true, true))
			{
				this.Close(true);
			}
			if (Widgets.ButtonText(new Rect(rect.width / 2f - Dialog_EditIdeoDescription.ButSize.x / 2f, rect.height - Dialog_EditIdeoDescription.ButSize.y, Dialog_EditIdeoDescription.ButSize.x, Dialog_EditIdeoDescription.ButSize.y), "Randomize".Translate(), true, true, true))
			{
				IdeoDescriptionResult ideoDescriptionResult = this.ideo.GetNewDescription(true);
				this.newDescription = ideoDescriptionResult.text;
				this.newDescriptionTemplate = ideoDescriptionResult.template;
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
			}
			if (Widgets.ButtonText(new Rect(rect.width - Dialog_EditIdeoDescription.ButSize.x, rect.height - Dialog_EditIdeoDescription.ButSize.y, Dialog_EditIdeoDescription.ButSize.x, Dialog_EditIdeoDescription.ButSize.y), "DoneButton".Translate(), true, true, true))
			{
				this.ApplyChanges();
			}
		}

		// Token: 0x060073C4 RID: 29636 RVA: 0x00270274 File Offset: 0x0026E474
		private void ApplyChanges()
		{
			if (this.ideo.description != this.newDescription)
			{
				this.ideo.description = this.newDescription;
				this.ideo.descriptionTemplate = this.newDescriptionTemplate;
				this.ideo.descriptionLocked = true;
			}
			this.Close(true);
		}

		// Token: 0x04003F7D RID: 16253
		private Ideo ideo;

		// Token: 0x04003F7E RID: 16254
		private string newDescription;

		// Token: 0x04003F7F RID: 16255
		private string newDescriptionTemplate;

		// Token: 0x04003F80 RID: 16256
		private static readonly Vector2 ButSize = new Vector2(150f, 38f);

		// Token: 0x04003F81 RID: 16257
		private const float HeaderHeight = 35f;
	}
}
