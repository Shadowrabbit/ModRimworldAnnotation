using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012F4 RID: 4852
	public class Dialog_ModSettings : Window
	{
		// Token: 0x1700147B RID: 5243
		// (get) Token: 0x0600749B RID: 29851 RVA: 0x00278AAB File Offset: 0x00276CAB
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(900f, 700f);
			}
		}

		// Token: 0x0600749C RID: 29852 RVA: 0x00279E58 File Offset: 0x00278058
		public Dialog_ModSettings()
		{
			this.forcePause = true;
			this.doCloseX = true;
			this.doCloseButton = true;
			this.closeOnClickedOutside = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x0600749D RID: 29853 RVA: 0x0027A092 File Offset: 0x00278292
		public override void PreClose()
		{
			base.PreClose();
			if (this.selMod != null)
			{
				this.selMod.WriteSettings();
			}
		}

		// Token: 0x0600749E RID: 29854 RVA: 0x0027A0B0 File Offset: 0x002782B0
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
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
					Find.WindowStack.Add(new FloatMenu(list));
				}
				else
				{
					List<FloatMenuOption> list2 = new List<FloatMenuOption>();
					list2.Add(new FloatMenuOption("NoConfigurableMods".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					Find.WindowStack.Add(new FloatMenu(list2));
				}
			}
			if (this.selMod != null)
			{
				Text.Font = GameFont.Medium;
				Widgets.Label(new Rect(167f, 0f, inRect.width - 150f - 17f, 35f), this.selMod.SettingsCategory());
				Text.Font = GameFont.Small;
				Rect inRect2 = new Rect(0f, 40f, inRect.width, inRect.height - 40f - Window.CloseButSize.y);
				this.selMod.DoSettingsWindowContents(inRect2);
			}
		}

		// Token: 0x0600749F RID: 29855 RVA: 0x0027A29C File Offset: 0x0027849C
		public static bool HasSettings()
		{
			return LoadedModManager.ModHandles.Any((Mod mod) => !mod.SettingsCategory().NullOrEmpty());
		}

		// Token: 0x04004048 RID: 16456
		private Mod selMod;

		// Token: 0x04004049 RID: 16457
		private const float TopAreaHeight = 40f;

		// Token: 0x0400404A RID: 16458
		private const float TopButtonHeight = 35f;

		// Token: 0x0400404B RID: 16459
		private const float TopButtonWidth = 150f;
	}
}
