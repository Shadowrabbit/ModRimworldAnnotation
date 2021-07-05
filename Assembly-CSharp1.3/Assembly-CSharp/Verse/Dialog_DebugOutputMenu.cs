using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003C8 RID: 968
	public class Dialog_DebugOutputMenu : Dialog_DebugOptionLister
	{
		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x06001DB1 RID: 7601 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x06001DB2 RID: 7602 RVA: 0x000B9848 File Offset: 0x000B7A48
		protected override int HighlightedIndex
		{
			get
			{
				if (base.FilterAllows(this.debugOutputs[this.prioritizedHighlightedIndex].label))
				{
					return this.prioritizedHighlightedIndex;
				}
				if (this.filter.NullOrEmpty())
				{
					return 0;
				}
				for (int i = 0; i < this.debugOutputs.Count; i++)
				{
					if (base.FilterAllows(this.debugOutputs[i].label))
					{
						this.currentHighlightIndex = i;
						break;
					}
				}
				return this.currentHighlightIndex;
			}
		}

		// Token: 0x06001DB3 RID: 7603 RVA: 0x000B98C8 File Offset: 0x000B7AC8
		public Dialog_DebugOutputMenu()
		{
			this.forcePause = true;
			foreach (Type type in GenTypes.AllTypes)
			{
				foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
				{
					DebugOutputAttribute attribute;
					if (methodInfo.TryGetAttribute(out attribute))
					{
						this.GenerateCacheForMethod(methodInfo, attribute);
					}
				}
			}
			this.debugOutputs = (from r in this.debugOutputs
			orderby r.category, r.label
			select r).ToList<Dialog_DebugOutputMenu.DebugOutputOption>();
		}

		// Token: 0x06001DB4 RID: 7604 RVA: 0x000B99AC File Offset: 0x000B7BAC
		private void GenerateCacheForMethod(MethodInfo method, DebugOutputAttribute attribute)
		{
			if (attribute.onlyWhenPlaying && Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			string label = attribute.name ?? GenText.SplitCamelCase(method.Name);
			Action action = Delegate.CreateDelegate(typeof(Action), method) as Action;
			string text = attribute.category;
			if (text == null)
			{
				text = "General";
			}
			this.debugOutputs.Add(new Dialog_DebugOutputMenu.DebugOutputOption
			{
				label = label,
				category = text,
				action = action
			});
		}

		// Token: 0x06001DB5 RID: 7605 RVA: 0x000B9A34 File Offset: 0x000B7C34
		protected override void DoListingItems()
		{
			base.DoListingItems();
			int highlightedIndex = this.HighlightedIndex;
			string b = null;
			for (int i = 0; i < this.debugOutputs.Count; i++)
			{
				Dialog_DebugOutputMenu.DebugOutputOption debugOutputOption = this.debugOutputs[i];
				if (debugOutputOption.category != b)
				{
					base.DoLabel(debugOutputOption.category);
					b = debugOutputOption.category;
				}
				Log.openOnMessage = true;
				try
				{
					base.DebugAction(debugOutputOption.label, debugOutputOption.action, highlightedIndex == i);
				}
				finally
				{
					Log.openOnMessage = false;
				}
			}
		}

		// Token: 0x06001DB6 RID: 7606 RVA: 0x000B9ACC File Offset: 0x000B7CCC
		protected override void ChangeHighlightedOption()
		{
			int highlightedIndex = this.HighlightedIndex;
			for (int i = 0; i < this.debugOutputs.Count; i++)
			{
				int num = (highlightedIndex + i + 1) % this.debugOutputs.Count;
				if (base.FilterAllows(this.debugOutputs[num].label))
				{
					this.prioritizedHighlightedIndex = num;
					return;
				}
			}
		}

		// Token: 0x06001DB7 RID: 7607 RVA: 0x000B9B2C File Offset: 0x000B7D2C
		public override void OnAcceptKeyPressed()
		{
			if (GUI.GetNameOfFocusedControl() == "DebugFilter")
			{
				int highlightedIndex = this.HighlightedIndex;
				if (highlightedIndex >= 0)
				{
					this.Close(true);
					this.debugOutputs[highlightedIndex].action();
				}
				Event.current.Use();
			}
		}

		// Token: 0x040011D9 RID: 4569
		private List<Dialog_DebugOutputMenu.DebugOutputOption> debugOutputs = new List<Dialog_DebugOutputMenu.DebugOutputOption>();

		// Token: 0x040011DA RID: 4570
		public const string DefaultCategory = "General";

		// Token: 0x02001C29 RID: 7209
		private struct DebugOutputOption
		{
			// Token: 0x04006D23 RID: 27939
			public string label;

			// Token: 0x04006D24 RID: 27940
			public string category;

			// Token: 0x04006D25 RID: 27941
			public Action action;
		}
	}
}
