using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020019E8 RID: 6632
	public class Dialog_DefineBinding : Window
	{
		// Token: 0x1700174E RID: 5966
		// (get) Token: 0x060092B3 RID: 37555 RVA: 0x00062413 File Offset: 0x00060613
		public override Vector2 InitialSize
		{
			get
			{
				return this.windowSize;
			}
		}

		// Token: 0x1700174F RID: 5967
		// (get) Token: 0x060092B4 RID: 37556 RVA: 0x00016647 File Offset: 0x00014847
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x060092B5 RID: 37557 RVA: 0x002A398C File Offset: 0x002A1B8C
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

		// Token: 0x060092B6 RID: 37558 RVA: 0x002A39EC File Offset: 0x002A1BEC
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

		// Token: 0x04005CDF RID: 23775
		protected Vector2 windowSize = new Vector2(400f, 200f);

		// Token: 0x04005CE0 RID: 23776
		protected KeyPrefsData keyPrefsData;

		// Token: 0x04005CE1 RID: 23777
		protected KeyBindingDef keyDef;

		// Token: 0x04005CE2 RID: 23778
		protected KeyPrefs.BindingSlot slot;
	}
}
