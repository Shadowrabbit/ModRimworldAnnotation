using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x020006C4 RID: 1732
	public class Dialog_DebugActionsMenu : Dialog_DebugOptionLister
	{
		// Token: 0x170006A3 RID: 1699
		// (get) Token: 0x06002CA1 RID: 11425 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170006A4 RID: 1700
		// (get) Token: 0x06002CA2 RID: 11426 RVA: 0x0012FD50 File Offset: 0x0012DF50
		protected override int HighlightedIndex
		{
			get
			{
				if (base.FilterAllows(this.debugActions[this.prioritizedHighlightedIndex].label))
				{
					return this.prioritizedHighlightedIndex;
				}
				if (this.filter.NullOrEmpty())
				{
					return 0;
				}
				for (int i = 0; i < this.debugActions.Count; i++)
				{
					if (base.FilterAllows(this.debugActions[i].label))
					{
						this.currentHighlightIndex = i;
						break;
					}
				}
				return this.currentHighlightIndex;
			}
		}

		// Token: 0x06002CA3 RID: 11427 RVA: 0x0012FDD0 File Offset: 0x0012DFD0
		public Dialog_DebugActionsMenu()
		{
			this.forcePause = true;
			foreach (Type type in GenTypes.AllTypes)
			{
				foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
				{
					DebugActionAttribute attribute;
					if (methodInfo.TryGetAttribute(out attribute))
					{
						this.GenerateCacheForMethod(methodInfo, attribute);
					}
					DebugActionYielderAttribute debugActionYielderAttribute;
					if (methodInfo.TryGetAttribute(out debugActionYielderAttribute))
					{
						foreach (Dialog_DebugActionsMenu.DebugActionOption item in ((IEnumerable<Dialog_DebugActionsMenu.DebugActionOption>)methodInfo.Invoke(null, null)))
						{
							this.debugActions.Add(item);
						}
					}
				}
			}
			this.debugActions = (from r in this.debugActions
			orderby DebugActionCategories.GetOrderFor(r.category), r.category
			select r).ToList<Dialog_DebugActionsMenu.DebugActionOption>();
		}

		// Token: 0x06002CA4 RID: 11428 RVA: 0x0012FF14 File Offset: 0x0012E114
		private void GenerateCacheForMethod(MethodInfo method, DebugActionAttribute attribute)
		{
			if (!attribute.IsAllowedInCurrentGameState)
			{
				return;
			}
			string text = string.IsNullOrEmpty(attribute.name) ? GenText.SplitCamelCase(method.Name) : attribute.name;
			if (attribute.actionType == DebugActionType.ToolMap || attribute.actionType == DebugActionType.ToolMapForPawns || attribute.actionType == DebugActionType.ToolWorld)
			{
				text = "T: " + text;
			}
			string category = attribute.category;
			Dialog_DebugActionsMenu.DebugActionOption item = new Dialog_DebugActionsMenu.DebugActionOption
			{
				label = text,
				category = category,
				actionType = attribute.actionType
			};
			if (attribute.actionType == DebugActionType.ToolMapForPawns)
			{
				item.pawnAction = (Delegate.CreateDelegate(typeof(Action<Pawn>), method) as Action<Pawn>);
			}
			else
			{
				item.action = (Delegate.CreateDelegate(typeof(Action), method) as Action);
			}
			this.debugActions.Add(item);
		}

		// Token: 0x06002CA5 RID: 11429 RVA: 0x0012FFF0 File Offset: 0x0012E1F0
		protected override void DoListingItems()
		{
			base.DoListingItems();
			int highlightedIndex = this.HighlightedIndex;
			string b = null;
			for (int i = 0; i < this.debugActions.Count; i++)
			{
				Dialog_DebugActionsMenu.DebugActionOption debugActionOption = this.debugActions[i];
				bool highlight = highlightedIndex == i;
				if (debugActionOption.category != b)
				{
					base.DoGap();
					base.DoLabel(debugActionOption.category);
					b = debugActionOption.category;
				}
				Log.openOnMessage = true;
				try
				{
					switch (debugActionOption.actionType)
					{
					case DebugActionType.Action:
						base.DebugAction_NewTmp(debugActionOption.label, debugActionOption.action, highlight);
						break;
					case DebugActionType.ToolMap:
						base.DebugToolMap_NewTmp(debugActionOption.label, debugActionOption.action, highlight);
						break;
					case DebugActionType.ToolMapForPawns:
						base.DebugToolMapForPawns_NewTmp(debugActionOption.label, debugActionOption.pawnAction, highlight);
						break;
					case DebugActionType.ToolWorld:
						base.DebugToolWorld_NewTmp(debugActionOption.label, debugActionOption.action, highlight);
						break;
					}
				}
				finally
				{
					Log.openOnMessage = false;
				}
			}
		}

		// Token: 0x06002CA6 RID: 11430 RVA: 0x001300FC File Offset: 0x0012E2FC
		protected override void ChangeHighlightedOption()
		{
			int highlightedIndex = this.HighlightedIndex;
			for (int i = 0; i < this.debugActions.Count; i++)
			{
				int num = (highlightedIndex + i + 1) % this.debugActions.Count;
				if (base.FilterAllows(this.debugActions[num].label))
				{
					this.prioritizedHighlightedIndex = num;
					return;
				}
			}
		}

		// Token: 0x06002CA7 RID: 11431 RVA: 0x0013015C File Offset: 0x0012E35C
		public override void OnAcceptKeyPressed()
		{
			if (GUI.GetNameOfFocusedControl() == "DebugFilter")
			{
				int highlightedIndex = this.HighlightedIndex;
				if (highlightedIndex >= 0)
				{
					this.Close(true);
					this.DebugActionSelected(highlightedIndex);
				}
				Event.current.Use();
			}
		}

		// Token: 0x06002CA8 RID: 11432 RVA: 0x001301A0 File Offset: 0x0012E3A0
		private void DebugActionSelected(int index)
		{
			Dialog_DebugActionsMenu.DebugActionOption element = this.debugActions[index];
			switch (element.actionType)
			{
			case DebugActionType.Action:
				element.action();
				return;
			case DebugActionType.ToolMap:
				DebugTools.curTool = new DebugTool(element.label, element.action, null);
				return;
			case DebugActionType.ToolMapForPawns:
				DebugTools.curTool = new DebugTool(element.label, delegate()
				{
					if (UI.MouseCell().InBounds(Find.CurrentMap))
					{
						foreach (Pawn obj in (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
						where t is Pawn
						select t).Cast<Pawn>().ToList<Pawn>())
						{
							element.pawnAction(obj);
						}
					}
				}, null);
				return;
			case DebugActionType.ToolWorld:
				if (WorldRendererUtility.WorldRenderedNow)
				{
					DebugTools.curTool = new DebugTool(element.label, element.action, null);
				}
				return;
			default:
				return;
			}
		}

		// Token: 0x04001E40 RID: 7744
		private List<Dialog_DebugActionsMenu.DebugActionOption> debugActions = new List<Dialog_DebugActionsMenu.DebugActionOption>();

		// Token: 0x020006C5 RID: 1733
		public struct DebugActionOption
		{
			// Token: 0x04001E41 RID: 7745
			public DebugActionType actionType;

			// Token: 0x04001E42 RID: 7746
			public string label;

			// Token: 0x04001E43 RID: 7747
			public string category;

			// Token: 0x04001E44 RID: 7748
			public Action action;

			// Token: 0x04001E45 RID: 7749
			public Action<Pawn> pawnAction;
		}
	}
}
