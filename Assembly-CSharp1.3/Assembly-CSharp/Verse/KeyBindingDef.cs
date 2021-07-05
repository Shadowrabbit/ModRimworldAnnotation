using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000D8 RID: 216
	public class KeyBindingDef : Def
	{
		// Token: 0x17000108 RID: 264
		// (get) Token: 0x0600061B RID: 1563 RVA: 0x0001EA58 File Offset: 0x0001CC58
		public KeyCode MainKey
		{
			get
			{
				KeyBindingData keyBindingData;
				if (KeyPrefs.KeyPrefsData.keyPrefs.TryGetValue(this, out keyBindingData))
				{
					if (keyBindingData.keyBindingA != KeyCode.None)
					{
						return keyBindingData.keyBindingA;
					}
					if (keyBindingData.keyBindingB != KeyCode.None)
					{
						return keyBindingData.keyBindingB;
					}
				}
				return KeyCode.None;
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x0600061C RID: 1564 RVA: 0x0001EA98 File Offset: 0x0001CC98
		public string MainKeyLabel
		{
			get
			{
				return this.MainKey.ToStringReadable();
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x0600061D RID: 1565 RVA: 0x0001EAA8 File Offset: 0x0001CCA8
		public bool KeyDownEvent
		{
			get
			{
				KeyBindingData keyBindingData;
				return Event.current.type == EventType.KeyDown && Event.current.keyCode != KeyCode.None && KeyPrefs.KeyPrefsData.keyPrefs.TryGetValue(this, out keyBindingData) && (keyBindingData.keyBindingA == KeyCode.LeftCommand || keyBindingData.keyBindingA == KeyCode.RightCommand || keyBindingData.keyBindingB == KeyCode.LeftCommand || keyBindingData.keyBindingB == KeyCode.RightCommand || !Event.current.command) && (Event.current.keyCode == keyBindingData.keyBindingA || Event.current.keyCode == keyBindingData.keyBindingB);
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x0600061E RID: 1566 RVA: 0x0001EB50 File Offset: 0x0001CD50
		public bool IsDownEvent
		{
			get
			{
				KeyBindingData keyBindingData;
				return Event.current != null && KeyPrefs.KeyPrefsData.keyPrefs.TryGetValue(this, out keyBindingData) && (this.KeyDownEvent || (Event.current.shift && (keyBindingData.keyBindingA == KeyCode.LeftShift || keyBindingData.keyBindingA == KeyCode.RightShift || keyBindingData.keyBindingB == KeyCode.LeftShift || keyBindingData.keyBindingB == KeyCode.RightShift)) || (Event.current.control && (keyBindingData.keyBindingA == KeyCode.LeftControl || keyBindingData.keyBindingA == KeyCode.RightControl || keyBindingData.keyBindingB == KeyCode.LeftControl || keyBindingData.keyBindingB == KeyCode.RightControl)) || (Event.current.alt && (keyBindingData.keyBindingA == KeyCode.LeftAlt || keyBindingData.keyBindingA == KeyCode.RightAlt || keyBindingData.keyBindingB == KeyCode.LeftAlt || keyBindingData.keyBindingB == KeyCode.RightAlt)) || (Event.current.command && (keyBindingData.keyBindingA == KeyCode.LeftCommand || keyBindingData.keyBindingA == KeyCode.RightCommand || keyBindingData.keyBindingB == KeyCode.LeftCommand || keyBindingData.keyBindingB == KeyCode.RightCommand)) || this.IsDown);
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x0600061F RID: 1567 RVA: 0x0001EC94 File Offset: 0x0001CE94
		public bool JustPressed
		{
			get
			{
				KeyBindingData keyBindingData;
				return KeyPrefs.KeyPrefsData.keyPrefs.TryGetValue(this, out keyBindingData) && (Input.GetKeyDown(keyBindingData.keyBindingA) || Input.GetKeyDown(keyBindingData.keyBindingB));
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000620 RID: 1568 RVA: 0x0001ECD4 File Offset: 0x0001CED4
		public bool IsDown
		{
			get
			{
				KeyBindingData keyBindingData;
				return KeyPrefs.KeyPrefsData.keyPrefs.TryGetValue(this, out keyBindingData) && (Input.GetKey(keyBindingData.keyBindingA) || Input.GetKey(keyBindingData.keyBindingB));
			}
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x0001ED11 File Offset: 0x0001CF11
		public KeyCode GetDefaultKeyCode(KeyPrefs.BindingSlot slot)
		{
			if (slot == KeyPrefs.BindingSlot.A)
			{
				return this.defaultKeyCodeA;
			}
			if (slot == KeyPrefs.BindingSlot.B)
			{
				return this.defaultKeyCodeB;
			}
			throw new InvalidOperationException();
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x0001ED2D File Offset: 0x0001CF2D
		public static KeyBindingDef Named(string name)
		{
			return DefDatabase<KeyBindingDef>.GetNamedSilentFail(name);
		}

		// Token: 0x040004BA RID: 1210
		public KeyBindingCategoryDef category;

		// Token: 0x040004BB RID: 1211
		public KeyCode defaultKeyCodeA;

		// Token: 0x040004BC RID: 1212
		public KeyCode defaultKeyCodeB;

		// Token: 0x040004BD RID: 1213
		public bool devModeOnly;

		// Token: 0x040004BE RID: 1214
		[NoTranslate]
		public List<string> extraConflictTags;
	}
}
