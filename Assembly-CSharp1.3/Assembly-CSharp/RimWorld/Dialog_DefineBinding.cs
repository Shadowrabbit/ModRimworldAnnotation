using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012ED RID: 4845
	public class Dialog_DefineBinding : Window
	{
		// Token: 0x17001462 RID: 5218
		// (get) Token: 0x06007444 RID: 29764 RVA: 0x00276B27 File Offset: 0x00274D27
		public override Vector2 InitialSize
		{
			get
			{
				return this.windowSize;
			}
		}

		// Token: 0x17001463 RID: 5219
		// (get) Token: 0x06007445 RID: 29765 RVA: 0x000682C5 File Offset: 0x000664C5
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x06007446 RID: 29766 RVA: 0x00276B30 File Offset: 0x00274D30
		public Dialog_DefineBinding(KeyPrefsData keyPrefsData, KeyBindingDef keyDef, KeyPrefs.BindingSlot slot)
		{
			this.keyDef = keyDef;
			this.slot = slot;
			this.keyPrefsData = keyPrefsData;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.forcePause = true;
			this.onlyOneOfTypeAllowed = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x06007447 RID: 29767 RVA: 0x00276B90 File Offset: 0x00274D90
		public override void DoWindowContents(Rect inRect)
		{
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(inRect, "PressAnyKeyOrEsc".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
			if (Event.current.isKey && Event.current.type == EventType.KeyDown && Event.current.keyCode != KeyCode.None)
			{
				if (Event.current.keyCode != KeyCode.Escape)
				{
					this.keyPrefsData.EraseConflictingBindingsForKeyCode(this.keyDef, Event.current.keyCode, delegate(KeyBindingDef oldDef)
					{
						Messages.Message("KeyBindingOverwritten".Translate(oldDef.LabelCap), MessageTypeDefOf.TaskCompletion, false);
					});
					this.keyPrefsData.SetBinding(this.keyDef, this.slot, Event.current.keyCode);
				}
				this.Close(true);
				Event.current.Use();
			}
		}

		// Token: 0x04004004 RID: 16388
		protected Vector2 windowSize = new Vector2(400f, 200f);

		// Token: 0x04004005 RID: 16389
		protected KeyPrefsData keyPrefsData;

		// Token: 0x04004006 RID: 16390
		protected KeyBindingDef keyDef;

		// Token: 0x04004007 RID: 16391
		protected KeyPrefs.BindingSlot slot;
	}
}
