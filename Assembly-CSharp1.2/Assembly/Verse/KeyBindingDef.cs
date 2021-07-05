using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000149 RID: 329
	public class KeyBindingDef : Def
	{
		// Token: 0x17000189 RID: 393
		// (get) Token: 0x06000888 RID: 2184 RVA: 0x000969D0 File Offset: 0x00094BD0
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

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06000889 RID: 2185 RVA: 0x0000CC26 File Offset: 0x0000AE26
		public string MainKeyLabel
		{
			get
			{
				return this.MainKey.ToStringReadable();
			}
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x0600088A RID: 2186 RVA: 0x00096A10 File Offset: 0x00094C10
		public bool KeyDownEvent
		{
			get
			{
				KeyBindingData keyBindingData;
				return Event.current.type == EventType.KeyDown && Event.current.keyCode != KeyCode.None && KeyPrefs.KeyPrefsData.keyPrefs.TryGetValue(this, out keyBindingData) && (keyBindingData.keyBindingA == KeyCode.LeftCommand || keyBindingData.keyBindingA == KeyCode.RightCommand || keyBindingData.keyBindingB == KeyCode.LeftCommand || keyBindingData.keyBindingB == KeyCode.RightCommand || !Event.current.command) && (Event.current.keyCode == keyBindingData.keyBindingA || Event.current.keyCode == keyBindingData.keyBindingB);
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x0600088B RID: 2187 RVA: 0x00096AB8 File Offset: 0x00094CB8
		public bool IsDownEvent
		{
			get
			{
				KeyBindingData keyBindingData;
				return Event.current != null && KeyPrefs.KeyPrefsData.keyPrefs.TryGetValue(this, out keyBindingData) && (this.KeyDownEvent || (Event.current.shift && (keyBindingData.keyBindingA == KeyCode.LeftShift || keyBindingData.keyBindingA == KeyCode.RightShift || keyBindingData.keyBindingB == KeyCode.LeftShift || keyBindingData.keyBindingB == KeyCode.RightShift)) || (Event.current.control && (keyBindingData.keyBindingA == KeyCode.LeftControl || keyBindingData.keyBindingA == KeyCode.RightControl || keyBindingData.keyBindingB == KeyCode.LeftControl || keyBindingData.keyBindingB == KeyCode.RightControl)) || (Event.current.alt && (keyBindingData.keyBindingA == KeyCode.LeftAlt || keyBindingData.keyBindingA == KeyCode.RightAlt || keyBindingData.keyBindingB == KeyCode.LeftAlt || keyBindingData.keyBindingB == KeyCode.RightAlt)) || (Event.current.command && (keyBindingData.keyBindingA == KeyCode.LeftCommand || keyBindingData.keyBindingA == KeyCode.RightCommand || keyBindingData.keyBindingB == KeyCode.LeftCommand || keyBindingData.keyBindingB == KeyCode.RightCommand)) || this.IsDown);
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x0600088C RID: 2188 RVA: 0x00096BFC File Offset: 0x00094DFC
		public bool JustPressed
		{
			get
			{
				KeyBindingData keyBindingData;
				return KeyPrefs.KeyPrefsData.keyPrefs.TryGetValue(this, out keyBindingData) && (Input.GetKeyDown(keyBindingData.keyBindingA) || Input.GetKeyDown(keyBindingData.keyBindingB));
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x0600088D RID: 2189 RVA: 0x00096C3C File Offset: 0x00094E3C
		public bool IsDown
		{
			get
			{
				KeyBindingData keyBindingData;
				return KeyPrefs.KeyPrefsData.keyPrefs.TryGetValue(this, out keyBindingData) && (Input.GetKey(keyBindingData.keyBindingA) || Input.GetKey(keyBindingData.keyBindingB));
			}
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x0000CC33 File Offset: 0x0000AE33
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

		// Token: 0x0600088F RID: 2191 RVA: 0x0000CC4F File Offset: 0x0000AE4F
		public static KeyBindingDef Named(string name)
		{
			return DefDatabase<KeyBindingDef>.GetNamedSilentFail(name);
		}

		// Token: 0x040006B6 RID: 1718
		public KeyBindingCategoryDef category;

		// Token: 0x040006B7 RID: 1719
		public KeyCode defaultKeyCodeA;

		// Token: 0x040006B8 RID: 1720
		public KeyCode defaultKeyCodeB;

		// Token: 0x040006B9 RID: 1721
		public bool devModeOnly;

		// Token: 0x040006BA RID: 1722
		[NoTranslate]
		public List<string> extraConflictTags;
	}
}
