using System;
using System.Collections.Generic;
using System.Reflection;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003C9 RID: 969
	public class Dialog_DebugSettingsMenu : Dialog_DebugOptionLister
	{
		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x06001DB8 RID: 7608 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x06001DB9 RID: 7609 RVA: 0x000B9B7C File Offset: 0x000B7D7C
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

		// Token: 0x06001DBA RID: 7610 RVA: 0x000B9C00 File Offset: 0x000B7E00
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

		// Token: 0x06001DBB RID: 7611 RVA: 0x000B9C94 File Offset: 0x000B7E94
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
					this.DoField(fieldInfo, highlightedIndex == num);
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
					this.DoField(fieldInfo2, highlightedIndex == num);
					num++;
				}
			}
		}

		// Token: 0x06001DBC RID: 7612 RVA: 0x000B9D90 File Offset: 0x000B7F90
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

		// Token: 0x06001DBD RID: 7613 RVA: 0x000B9DCC File Offset: 0x000B7FCC
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

		// Token: 0x06001DBE RID: 7614 RVA: 0x000B9E2C File Offset: 0x000B802C
		private void DoField(FieldInfo fi, bool highlight)
		{
			if (fi.IsLiteral)
			{
				return;
			}
			string label = GenText.SplitCamelCase(fi.Name).CapitalizeFirst();
			bool flag = (bool)fi.GetValue(null);
			bool flag2 = flag;
			base.CheckboxLabeledDebug(label, ref flag, highlight);
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

		// Token: 0x06001DBF RID: 7615 RVA: 0x000B9EB0 File Offset: 0x000B80B0
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

		// Token: 0x06001DC0 RID: 7616 RVA: 0x000B9F17 File Offset: 0x000B8117
		private string LegibleFieldName(FieldInfo fi)
		{
			return GenText.SplitCamelCase(fi.Name).CapitalizeFirst();
		}

		// Token: 0x040011DB RID: 4571
		private List<FieldInfo> settingsFields = new List<FieldInfo>();
	}
}
