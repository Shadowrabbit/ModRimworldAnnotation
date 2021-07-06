using System;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x020006C8 RID: 1736
	public abstract class Dialog_DebugOptionLister : Dialog_OptionLister
	{
		// Token: 0x170006A5 RID: 1701
		// (get) Token: 0x06002CB0 RID: 11440 RVA: 0x000236C9 File Offset: 0x000218C9
		protected virtual int HighlightedIndex
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x06002CB1 RID: 11441 RVA: 0x000236CC File Offset: 0x000218CC
		public Dialog_DebugOptionLister()
		{
			this.forcePause = true;
		}

		// Token: 0x06002CB2 RID: 11442 RVA: 0x00130314 File Offset: 0x0012E514
		protected bool DebugAction_NewTmp(string label, Action action, bool highlight)
		{
			bool result = false;
			if (!base.FilterAllows(label))
			{
				GUI.color = new Color(1f, 1f, 1f, 0.3f);
			}
			if (this.listing.ButtonDebug_NewTmp(label, highlight))
			{
				this.Close(true);
				action();
				result = true;
			}
			GUI.color = Color.white;
			if (Event.current.type == EventType.Layout)
			{
				this.totalOptionsHeight += 24f;
			}
			return result;
		}

		// Token: 0x06002CB3 RID: 11443 RVA: 0x00130394 File Offset: 0x0012E594
		protected void DebugToolMap_NewTmp(string label, Action toolAction, bool highlight)
		{
			if (WorldRendererUtility.WorldRenderedNow)
			{
				return;
			}
			if (!base.FilterAllows(label))
			{
				GUI.color = new Color(1f, 1f, 1f, 0.3f);
			}
			if (this.listing.ButtonDebug_NewTmp(label, highlight))
			{
				this.Close(true);
				DebugTools.curTool = new DebugTool(label, toolAction, null);
			}
			GUI.color = Color.white;
			if (Event.current.type == EventType.Layout)
			{
				this.totalOptionsHeight += 24f;
			}
		}

		// Token: 0x06002CB4 RID: 11444 RVA: 0x0013041C File Offset: 0x0012E61C
		protected void DebugToolMapForPawns_NewTmp(string label, Action<Pawn> pawnAction, bool highlight)
		{
			this.DebugToolMap_NewTmp(label, delegate
			{
				if (UI.MouseCell().InBounds(Find.CurrentMap))
				{
					foreach (Pawn obj in (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
					where t is Pawn
					select t).Cast<Pawn>().ToList<Pawn>())
					{
						pawnAction(obj);
					}
				}
			}, highlight);
		}

		// Token: 0x06002CB5 RID: 11445 RVA: 0x0013044C File Offset: 0x0012E64C
		protected void DebugToolWorld_NewTmp(string label, Action toolAction, bool highlight)
		{
			if (!WorldRendererUtility.WorldRenderedNow)
			{
				return;
			}
			if (!base.FilterAllows(label))
			{
				GUI.color = new Color(1f, 1f, 1f, 0.3f);
			}
			if (this.listing.ButtonDebug_NewTmp(label, highlight))
			{
				this.Close(true);
				DebugTools.curTool = new DebugTool(label, toolAction, null);
			}
			GUI.color = Color.white;
			if (Event.current.type == EventType.Layout)
			{
				this.totalOptionsHeight += 24f;
			}
		}

		// Token: 0x06002CB6 RID: 11446 RVA: 0x000236DB File Offset: 0x000218DB
		protected override void DoListingItems()
		{
			if (KeyBindingDefOf.Dev_ChangeSelectedDebugAction.IsDownEvent)
			{
				this.ChangeHighlightedOption();
			}
		}

		// Token: 0x06002CB7 RID: 11447 RVA: 0x00006A05 File Offset: 0x00004C05
		protected virtual void ChangeHighlightedOption()
		{
		}

		// Token: 0x06002CB8 RID: 11448 RVA: 0x000236EF File Offset: 0x000218EF
		[Obsolete("Only used for mod compatibility")]
		protected void CheckboxLabeledDebug(string label, ref bool checkOn)
		{
			this.CheckboxLabeledDebug(label, ref checkOn);
		}

		// Token: 0x06002CB9 RID: 11449 RVA: 0x001304D4 File Offset: 0x0012E6D4
		protected void CheckboxLabeledDebug_NewTmp(string label, ref bool checkOn, bool highlighted)
		{
			if (!base.FilterAllows(label))
			{
				GUI.color = new Color(1f, 1f, 1f, 0.3f);
			}
			this.listing.LabelCheckboxDebug_NewTmp(label, ref checkOn, highlighted);
			GUI.color = Color.white;
			if (Event.current.type == EventType.Layout)
			{
				this.totalOptionsHeight += 24f;
			}
		}

		// Token: 0x06002CBA RID: 11450 RVA: 0x000236F9 File Offset: 0x000218F9
		protected void DoLabel(string label)
		{
			Text.Font = GameFont.Small;
			this.listing.Label(label, -1f, null);
			this.totalOptionsHeight += Text.CalcHeight(label, 300f) + 2f;
		}

		// Token: 0x06002CBB RID: 11451 RVA: 0x00023732 File Offset: 0x00021932
		protected void DoGap()
		{
			this.listing.Gap(7f);
			this.totalOptionsHeight += 7f;
		}

		// Token: 0x06002CBC RID: 11452 RVA: 0x00023756 File Offset: 0x00021956
		[Obsolete("Only used for mod compatibility")]
		protected bool DebugAction(string label, Action action)
		{
			return this.DebugAction_NewTmp(label, action, false);
		}

		// Token: 0x06002CBD RID: 11453 RVA: 0x00023761 File Offset: 0x00021961
		[Obsolete("Only used for mod compatibility")]
		protected void DebugToolMap(string label, Action toolAction)
		{
			this.DebugToolMap_NewTmp(label, toolAction, false);
		}

		// Token: 0x06002CBE RID: 11454 RVA: 0x0002376C File Offset: 0x0002196C
		[Obsolete("Only used for mod compatibility")]
		protected void DebugToolMapForPawns(string label, Action<Pawn> pawnAction)
		{
			this.DebugToolMapForPawns_NewTmp(label, pawnAction, false);
		}

		// Token: 0x06002CBF RID: 11455 RVA: 0x00023777 File Offset: 0x00021977
		[Obsolete("Only used for mod compatibility")]
		protected void DebugToolWorld(string label, Action toolAction)
		{
			this.DebugToolWorld_NewTmp(label, toolAction, false);
		}

		// Token: 0x04001E4B RID: 7755
		protected int currentHighlightIndex;

		// Token: 0x04001E4C RID: 7756
		protected int prioritizedHighlightedIndex;

		// Token: 0x04001E4D RID: 7757
		private const float DebugOptionsGap = 7f;
	}
}
