using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CCE RID: 7374
	public class ShortcutKeys
	{
		// Token: 0x0600A04C RID: 41036 RVA: 0x0006AD4E File Offset: 0x00068F4E
		public void ShortcutKeysOnGUI()
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				if (KeyBindingDefOf.NextColonist.KeyDownEvent)
				{
					ThingSelectionUtility.SelectNextColonist();
					Event.current.Use();
				}
				if (KeyBindingDefOf.PreviousColonist.KeyDownEvent)
				{
					ThingSelectionUtility.SelectPreviousColonist();
					Event.current.Use();
				}
			}
		}
	}
}
