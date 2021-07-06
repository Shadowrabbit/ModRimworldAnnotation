using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A01 RID: 6657
	public class Dialog_ModSettings : Window
	{
		// Token: 0x1700176B RID: 5995
		// (get) Token: 0x06009339 RID: 37689 RVA: 0x00062722 File Offset: 0x00060922
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(900f, 700f);
			}
		}

		// Token: 0x0600933A RID: 37690 RVA: 0x00062977 File Offset: 0x00060B77
		public Dialog_ModSettings()
		{
			this.forcePause = true;
			this.doCloseX = true;
			this.doCloseButton = true;
			this.closeOnClickedOutside = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x0600933B RID: 37691 RVA: 0x000629A2 File Offset: 0x00060BA2
		public override void PreClose()
		{
			base.PreClose();
			if (this.selMod != null)
			{
				this.selMod.WriteSettings();
			}
		}

		// Token: 0x0600933C RID: 37692 RVA: 0x002A6BB4 File Offset: 0x002A4DB4
		public override void DoWindowContents(Rect inRect)
		{
			if (Widgets.ButtonText(new Rect(0f, 0f, 150f, 35f), "SelectMod".Translate(), true, true, true))
			{
				if (Dialog_ModSettings.HasSettings())
				{
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					foreach (Mod mod2 in from mod in LoadedModManager.ModHandles
					where !mod.SettingsCategory().NullOrEmpty()
					orderby mod.SettingsCategory()
					select mod)
					{
						Mod localMod = mod2;
						list.Add(new FloatMenuOption(mod2.SettingsCategory(), delegate()
						{
							if (this.selMod != null)
							{
								this.selMod.WriteSettings();
							}
							this.selMod = localMod;
						}, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
					Find.WindowStack.Add(new FloatMenu(list));
				}
				else
				{
					List<FloatMenuOption> list2 = new List<FloatMenuOption>();
					list2.Add(new FloatMenuOption("NoConfigurableMods".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null));
					Find.WindowStack.Add(new FloatMenu(list2));
				}
			}
			if (this.selMod != null)
			{
				Text.Font = GameFont.Medium;
				Widgets.Label(new Rect(167f, 0f, inRect.width - 150f - 17f, 35f), this.selMod.SettingsCategory());
				Text.Font = GameFont.Small;
				Rect inRect2 = new Rect(0f, 40f, inRect.width, inRect.height - 40f - this.CloseButSize.y);
				this.selMod.DoSettingsWindowContents(inRect2);
			}
		}

		// Token: 0x0600933D RID: 37693 RVA: 0x000629BD File Offset: 0x00060BBD
		public static bool HasSettings()
		{
			return LoadedModManager.ModHandles.Any((Mod mod) => !mod.SettingsCategory().NullOrEmpty());
		}

		// Token: 0x04005D4B RID: 23883
		private Mod selMod;

		// Token: 0x04005D4C RID: 23884
		private const float TopAreaHeight = 40f;

		// Token: 0x04005D4D RID: 23885
		private const float TopButtonHeight = 35f;

		// Token: 0x04005D4E RID: 23886
		private const float TopButtonWidth = 150f;
	}
}
