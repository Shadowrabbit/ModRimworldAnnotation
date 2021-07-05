using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003C2 RID: 962
	public class Dialog_DebugActionsMenu : Dialog_DebugOptionLister
	{
		// Token: 0x17000597 RID: 1431
		// (get) Token: 0x06001D94 RID: 7572 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000598 RID: 1432
		// (get) Token: 0x06001D95 RID: 7573 RVA: 0x000B8D8C File Offset: 0x000B6F8C
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

		// Token: 0x06001D96 RID: 7574 RVA: 0x000B8E0C File Offset: 0x000B700C
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

		// Token: 0x06001D97 RID: 7575 RVA: 0x000B8F50 File Offset: 0x000B7150
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

		// Token: 0x06001D98 RID: 7576 RVA: 0x000B902C File Offset: 0x000B722C
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
						base.DebugAction(debugActionOption.label, debugActionOption.action, highlight);
						break;
					case DebugActionType.ToolMap:
						base.DebugToolMap(debugActionOption.label, debugActionOption.action, highlight);
						break;
					case DebugActionType.ToolMapForPawns:
						base.DebugToolMapForPawns(debugActionOption.label, debugActionOption.pawnAction, highlight);
						break;
					case DebugActionType.ToolWorld:
						base.DebugToolWorld(debugActionOption.label, debugActionOption.action, highlight);
						break;
					}
				}
				finally
				{
					Log.openOnMessage = false;
				}
			}
		}

		// Token: 0x06001D99 RID: 7577 RVA: 0x000B9138 File Offset: 0x000B7338
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

		// Token: 0x06001D9A RID: 7578 RVA: 0x000B9198 File Offset: 0x000B7398
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

		// Token: 0x06001D9B RID: 7579 RVA: 0x000B91DC File Offset: 0x000B73DC
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

		// Token: 0x040011CB RID: 4555
		private List<Dialog_DebugActionsMenu.DebugActionOption> debugActions = new List<Dialog_DebugActionsMenu.DebugActionOption>();

		// Token: 0x02001C22 RID: 7202
		public struct DebugActionOption
		{
			// Token: 0x04006D13 RID: 27923
			public DebugActionType actionType;

			// Token: 0x04006D14 RID: 27924
			public string label;

			// Token: 0x04006D15 RID: 27925
			public string category;

			// Token: 0x04006D16 RID: 27926
			public Action action;

			// Token: 0x04006D17 RID: 27927
			public Action<Pawn> pawnAction;
		}
	}
}
