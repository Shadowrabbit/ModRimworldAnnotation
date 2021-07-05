using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012E2 RID: 4834
	public class Dialog_EditDeity : Window
	{
		// Token: 0x17001445 RID: 5189
		// (get) Token: 0x060073BA RID: 29626 RVA: 0x0026FBF4 File Offset: 0x0026DDF4
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(700f, 250f);
			}
		}

		// Token: 0x060073BB RID: 29627 RVA: 0x0026FC08 File Offset: 0x0026DE08
		public Dialog_EditDeity(IdeoFoundation_Deity.Deity deity, Ideo ideo)
		{
			this.deity = deity;
			this.ideo = ideo;
			this.absorbInputAroundWindow = true;
			this.newDeityName = deity.name;
			this.newDeityTitle = deity.type;
			this.newDeityGender = deity.gender;
		}

		// Token: 0x060073BC RID: 29628 RVA: 0x0026FC54 File Offset: 0x0026DE54
		public override void OnAcceptKeyPressed()
		{
			this.ApplyChanges();
			Event.current.Use();
		}

		// Token: 0x060073BD RID: 29629 RVA: 0x0026FC68 File Offset: 0x0026DE68
		public override void DoWindowContents(Rect rect)
		{
			float num = rect.x + rect.width / 3f;
			float width = rect.xMax - num;
			Text.Font = GameFont.Medium;
			Widgets.Label(new Rect(rect.x, rect.y, rect.width, 35f), "EditDeity".Translate());
			Text.Font = GameFont.Small;
			float num2 = rect.y + 35f + 10f;
			Widgets.Label(new Rect(rect.x, num2, width, Dialog_EditDeity.EditFieldHeight), "DeityName".Translate());
			this.newDeityName = Widgets.TextField(new Rect(num, num2, width, Dialog_EditDeity.EditFieldHeight), this.newDeityName);
			num2 += Dialog_EditDeity.EditFieldHeight + 10f;
			Widgets.Label(new Rect(rect.x, num2, width, Dialog_EditDeity.EditFieldHeight), "DeityTitle".Translate());
			this.newDeityTitle = Widgets.TextField(new Rect(num, num2, width, Dialog_EditDeity.EditFieldHeight), this.newDeityTitle);
			num2 += Dialog_EditDeity.EditFieldHeight + 10f;
			Widgets.Label(new Rect(rect.x, num2, width, Dialog_EditDeity.EditFieldHeight), "DeityGender".Translate());
			Rect rect2 = new Rect(num, num2, Dialog_EditDeity.EditFieldHeight + 8f + Text.CalcSize(this.newDeityGender.GetLabel(false).CapitalizeFirst()).x, Dialog_EditDeity.EditFieldHeight);
			Rect rect3 = new Rect(rect2.x, num2, Dialog_EditDeity.EditFieldHeight, Dialog_EditDeity.EditFieldHeight);
			GUI.DrawTexture(rect3.ContractedBy(2f), this.newDeityGender.GetIcon());
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(new Rect(rect3.xMax + 4f, num2, rect2.width - rect3.width, Dialog_EditDeity.EditFieldHeight), this.newDeityGender.GetLabel(false).CapitalizeFirst());
			Text.Anchor = TextAnchor.UpperLeft;
			Widgets.DrawHighlightIfMouseover(rect2);
			if (Widgets.ButtonInvisible(rect2, true))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				Gender[] array = (Gender[])Enum.GetValues(typeof(Gender));
				for (int i = 0; i < array.Length; i++)
				{
					Gender g = array[i];
					list.Add(new FloatMenuOption(g.GetLabel(false).CapitalizeFirst(), delegate()
					{
						this.newDeityGender = g;
					}, g.GetIcon(), Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			num2 += Dialog_EditDeity.EditFieldHeight + 10f;
			if (Widgets.ButtonText(new Rect(0f, rect.height - Dialog_EditDeity.ButSize.y, Dialog_EditDeity.ButSize.x, Dialog_EditDeity.ButSize.y), "Back".Translate(), true, true, true))
			{
				this.Close(true);
			}
			if (Widgets.ButtonText(new Rect(rect.width - Dialog_EditDeity.ButSize.x, rect.height - Dialog_EditDeity.ButSize.y, Dialog_EditDeity.ButSize.x, Dialog_EditDeity.ButSize.y), "DoneButton".Translate(), true, true, true))
			{
				this.ApplyChanges();
			}
		}

		// Token: 0x060073BE RID: 29630 RVA: 0x0026FFC4 File Offset: 0x0026E1C4
		private void ApplyChanges()
		{
			this.deity.name = this.newDeityName;
			this.deity.type = this.newDeityTitle;
			this.deity.gender = this.newDeityGender;
			this.ideo.RegenerateAllPreceptNames();
			this.ideo.RegenerateDescription(false);
			this.Close(true);
		}

		// Token: 0x04003F76 RID: 16246
		private IdeoFoundation_Deity.Deity deity;

		// Token: 0x04003F77 RID: 16247
		private Ideo ideo;

		// Token: 0x04003F78 RID: 16248
		private string newDeityName;

		// Token: 0x04003F79 RID: 16249
		private string newDeityTitle;

		// Token: 0x04003F7A RID: 16250
		private Gender newDeityGender;

		// Token: 0x04003F7B RID: 16251
		private static readonly Vector2 ButSize = new Vector2(150f, 38f);

		// Token: 0x04003F7C RID: 16252
		private static readonly float EditFieldHeight = 30f;
	}
}
