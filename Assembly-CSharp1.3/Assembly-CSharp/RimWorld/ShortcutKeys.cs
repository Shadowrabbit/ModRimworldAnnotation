using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200148F RID: 5263
	public class ShortcutKeys
	{
		// Token: 0x06007DD0 RID: 32208 RVA: 0x002C9467 File Offset: 0x002C7667
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
