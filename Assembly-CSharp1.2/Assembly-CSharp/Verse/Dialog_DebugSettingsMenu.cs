using System;
using System.Collections.Generic;
using System.Reflection;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020006D5 RID: 1749
	public class Dialog_DebugSettingsMenu : Dialog_DebugOptionLister
	{
		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x06002CDE RID: 11486 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170006AA RID: 1706
		// (get) Token: 0x06002CDF RID: 11487 RVA: 0x00130BA8 File Offset: 0x0012EDA8
		protected override int HighlightedIndex
		{
			get
			{
				if (base.FilterAllows(this.LegibleFieldName(this.settingsFields[this.prioritizedHighlightedIndex])))
				{
					return this.prioritizedHighlightedIndex;
				}
				if (this.filter.NullOrEmpty())
				{
					return 0;
				}
				for (int i = 0; i < this.settingsFields.Count; i++)
				{
					if (base.FilterAllows(this.LegibleFieldName(this.settingsFields[i])))
					{
						this.currentHighlightIndex = i;
						break;
					}
				}
				return this.currentHighlightIndex;
			}
		}

		// Token: 0x06002CE0 RID: 11488 RVA: 0x00130C2C File Offset: 0x0012EE2C
		public Dialog_DebugSettingsMenu()
		{
			this.forcePause = true;
			foreach (FieldInfo fieldInfo in typeof(DebugSettings).GetFields())
			{
				if (!fieldInfo.IsLiteral)
				{
					this.settingsFields.Add(fieldInfo);
				}
			}
			foreach (FieldInfo fieldInfo2 in typeof(DebugViewSettings).GetFields())
			{
				if (!fieldInfo2.IsLiteral)
				{
					this.settingsFields.Add(fieldInfo2);
				}
			}
		}

		// Token: 0x06002CE1 RID: 11489 RVA: 0x00130CC0 File Offset: 0x0012EEC0
		protected override void DoListingItems()
		{
			base.DoListingItems();
			if (KeyBindingDefOf.Dev_ToggleDebugSettingsMenu.KeyDownEvent)
			{
				Event.current.Use();
				this.Close(true);
			}
			Text.Font = GameFont.Small;
			this.listing.Label("Gameplay", -1f, null);
			int highlightedIndex = this.HighlightedIndex;
			int num = 0;
			foreach (FieldInfo fieldInfo in typeof(DebugSettings).GetFields())
			{
				if (!fieldInfo.IsLiteral)
				{
					this.DoField_NewTmp(fieldInfo, highlightedIndex == num);
					num++;
				}
			}
			this.listing.Gap(36f);
			Text.Font = GameFont.Small;
			this.listing.Label("View", -1f, null);
			foreach (FieldInfo fieldInfo2 in typeof(DebugViewSettings).GetFields())
			{
				if (!fieldInfo2.IsLiteral)
				{
					this.DoField_NewTmp(fieldInfo2, highlightedIndex == num);
					num++;
				}
			}
		}

		// Token: 0x06002CE2 RID: 11490 RVA: 0x00130DBC File Offset: 0x0012EFBC
		public override void OnAcceptKeyPressed()
		{
			if (GUI.GetNameOfFocusedControl() == "DebugFilter")
			{
				int highlightedIndex = this.HighlightedIndex;
				if (highlightedIndex >= 0)
				{
					this.Toggle(highlightedIndex);
				}
				Event.current.Use();
			}
		}

		// Token: 0x06002CE3 RID: 11491 RVA: 0x00130DF8 File Offset: 0x0012EFF8
		protected override void ChangeHighlightedOption()
		{
			int highlightedIndex = this.HighlightedIndex;
			for (int i = 0; i < this.settingsFields.Count; i++)
			{
				int num = (highlightedIndex + i + 1) % this.settingsFields.Count;
				if (base.FilterAllows(this.LegibleFieldName(this.settingsFields[num])))
				{
					this.prioritizedHighlightedIndex = num;
					return;
				}
			}
		}

		// Token: 0x06002CE4 RID: 11492 RVA: 0x0002382A File Offset: 0x00021A2A
		[Obsolete("Only used for mod compatibility.")]
		private void DoField(FieldInfo fi)
		{
			this.DoField_NewTmp(fi, false);
		}

		// Token: 0x06002CE5 RID: 11493 RVA: 0x00130E58 File Offset: 0x0012F058
		private void DoField_NewTmp(FieldInfo fi, bool highlight)
		{
			if (fi.IsLiteral)
			{
				return;
			}
			string label = GenText.SplitCamelCase(fi.Name).CapitalizeFirst();
			bool flag = (bool)fi.GetValue(null);
			bool flag2 = flag;
			base.CheckboxLabeledDebug_NewTmp(label, ref flag, highlight);
			if (flag != flag2)
			{
				fi.SetValue(null, flag);
				MethodInfo method = fi.DeclaringType.GetMethod(fi.Name + "Toggled", BindingFlags.Static | BindingFlags.Public);
				if (method != null)
				{
					method.Invoke(null, null);
				}
			}
		}

		// Token: 0x06002CE6 RID: 11494 RVA: 0x00130EDC File Offset: 0x0012F0DC
		private void Toggle(int index)
		{
			FieldInfo fieldInfo = this.settingsFields[index];
			bool flag = (bool)fieldInfo.GetValue(null);
			fieldInfo.SetValue(null, !flag);
			MethodInfo method = fieldInfo.DeclaringType.GetMethod(fieldInfo.Name + "Toggled", BindingFlags.Static | BindingFlags.Public);
			if (method != null)
			{
				method.Invoke(null, null);
			}
		}

		// Token: 0x06002CE7 RID: 11495 RVA: 0x00023834 File Offset: 0x00021A34
		private string LegibleFieldName(FieldInfo fi)
		{
			return GenText.SplitCamelCase(fi.Name).CapitalizeFirst();
		}

		// Token: 0x04001E66 RID: 7782
		private List<FieldInfo> settingsFields = new List<FieldInfo>();
	}
}
