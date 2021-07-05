using System;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003C6 RID: 966
	public abstract class Dialog_DebugOptionLister : Dialog_OptionLister
	{
		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x06001DA3 RID: 7587 RVA: 0x000B955A File Offset: 0x000B775A
		protected virtual int HighlightedIndex
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x06001DA4 RID: 7588 RVA: 0x000B955D File Offset: 0x000B775D
		public Dialog_DebugOptionLister()
		{
			this.forcePause = true;
		}

		// Token: 0x06001DA5 RID: 7589 RVA: 0x000B956C File Offset: 0x000B776C
		protected bool DebugAction(string label, Action action, bool highlight)
		{
			bool result = false;
			if (!base.FilterAllows(label))
			{
				GUI.color = new Color(1f, 1f, 1f, 0.3f);
			}
			if (this.listing.ButtonDebug(label, highlight))
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

		// Token: 0x06001DA6 RID: 7590 RVA: 0x000B95EC File Offset: 0x000B77EC
		protected void DebugToolMap(string label, Action toolAction, bool highlight)
		{
			if (WorldRendererUtility.WorldRenderedNow)
			{
				return;
			}
			if (!base.FilterAllows(label))
			{
				GUI.color = new Color(1f, 1f, 1f, 0.3f);
			}
			if (this.listing.ButtonDebug(label, highlight))
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

		// Token: 0x06001DA7 RID: 7591 RVA: 0x000B9674 File Offset: 0x000B7874
		protected void DebugToolMapForPawns(string label, Action<Pawn> pawnAction, bool highlight)
		{
			this.DebugToolMap(label, delegate
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

		// Token: 0x06001DA8 RID: 7592 RVA: 0x000B96A4 File Offset: 0x000B78A4
		protected void DebugToolWorld(string label, Action toolAction, bool highlight)
		{
			if (!WorldRendererUtility.WorldRenderedNow)
			{
				return;
			}
			if (!base.FilterAllows(label))
			{
				GUI.color = new Color(1f, 1f, 1f, 0.3f);
			}
			if (this.listing.ButtonDebug(label, highlight))
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

		// Token: 0x06001DA9 RID: 7593 RVA: 0x000B972C File Offset: 0x000B792C
		protected override void DoListingItems()
		{
			if (KeyBindingDefOf.Dev_ChangeSelectedDebugAction.IsDownEvent)
			{
				this.ChangeHighlightedOption();
			}
		}

		// Token: 0x06001DAA RID: 7594 RVA: 0x0000313F File Offset: 0x0000133F
		protected virtual void ChangeHighlightedOption()
		{
		}

		// Token: 0x06001DAB RID: 7595 RVA: 0x000B9740 File Offset: 0x000B7940
		protected void CheckboxLabeledDebug(string label, ref bool checkOn, bool highlighted)
		{
			if (!base.FilterAllows(label))
			{
				GUI.color = new Color(1f, 1f, 1f, 0.3f);
			}
			this.listing.LabelCheckboxDebug(label, ref checkOn, highlighted);
			GUI.color = Color.white;
			if (Event.current.type == EventType.Layout)
			{
				this.totalOptionsHeight += 24f;
			}
		}

		// Token: 0x06001DAC RID: 7596 RVA: 0x000B97AB File Offset: 0x000B79AB
		protected void DoLabel(string label)
		{
			Text.Font = GameFont.Small;
			this.listing.Label(label, -1f, null);
			this.totalOptionsHeight += Text.CalcHeight(label, 300f) + 2f;
		}

		// Token: 0x06001DAD RID: 7597 RVA: 0x000B97E4 File Offset: 0x000B79E4
		protected void DoGap()
		{
			this.listing.Gap(7f);
			this.totalOptionsHeight += 7f;
		}

		// Token: 0x040011D3 RID: 4563
		protected int currentHighlightIndex;

		// Token: 0x040011D4 RID: 4564
		protected int prioritizedHighlightedIndex;

		// Token: 0x040011D5 RID: 4565
		private const float DebugOptionsGap = 7f;
	}
}
