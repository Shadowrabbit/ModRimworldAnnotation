using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Verse
{
	// Token: 0x020006D2 RID: 1746
	public class Dialog_DebugOutputMenu : Dialog_DebugOptionLister
	{
		// Token: 0x170006A7 RID: 1703
		// (get) Token: 0x06002CD3 RID: 11475 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x06002CD4 RID: 11476 RVA: 0x00130874 File Offset: 0x0012EA74
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

		// Token: 0x06002CD5 RID: 11477 RVA: 0x001308F4 File Offset: 0x0012EAF4
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

		// Token: 0x06002CD6 RID: 11478 RVA: 0x001309D8 File Offset: 0x0012EBD8
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

		// Token: 0x06002CD7 RID: 11479 RVA: 0x00130A60 File Offset: 0x0012EC60
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
					base.DebugAction_NewTmp(debugOutputOption.label, debugOutputOption.action, highlightedIndex == i);
				}
				finally
				{
					Log.openOnMessage = false;
				}
			}
		}

		// Token: 0x06002CD8 RID: 11480 RVA: 0x00130AF8 File Offset: 0x0012ECF8
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

		// Token: 0x06002CD9 RID: 11481 RVA: 0x00130B58 File Offset: 0x0012ED58
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

		// Token: 0x04001E5E RID: 7774
		private List<Dialog_DebugOutputMenu.DebugOutputOption> debugOutputs = new List<Dialog_DebugOutputMenu.DebugOutputOption>();

		// Token: 0x04001E5F RID: 7775
		public const string DefaultCategory = "General";

		// Token: 0x020006D3 RID: 1747
		private struct DebugOutputOption
		{
			// Token: 0x04001E60 RID: 7776
			public string label;

			// Token: 0x04001E61 RID: 7777
			public string category;

			// Token: 0x04001E62 RID: 7778
			public Action action;
		}
	}
}
