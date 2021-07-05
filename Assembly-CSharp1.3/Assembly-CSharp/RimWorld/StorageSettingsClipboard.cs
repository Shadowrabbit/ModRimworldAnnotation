using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001061 RID: 4193
	public static class StorageSettingsClipboard
	{
		// Token: 0x170010F5 RID: 4341
		// (get) Token: 0x06006364 RID: 25444 RVA: 0x002198C2 File Offset: 0x00217AC2
		public static bool HasCopiedSettings
		{
			get
			{
				return StorageSettingsClipboard.copied;
			}
		}

		// Token: 0x06006365 RID: 25445 RVA: 0x002198C9 File Offset: 0x00217AC9
		public static void Copy(StorageSettings s)
		{
			StorageSettingsClipboard.clipboard.CopyFrom(s);
			StorageSettingsClipboard.copied = true;
		}

		// Token: 0x06006366 RID: 25446 RVA: 0x002198DC File Offset: 0x00217ADC
		public static void PasteInto(StorageSettings s)
		{
			s.CopyFrom(StorageSettingsClipboard.clipboard);
		}

		// Token: 0x06006367 RID: 25447 RVA: 0x002198E9 File Offset: 0x00217AE9
		public static IEnumerable<Gizmo> CopyPasteGizmosFor(StorageSettings s)
		{
			yield return new Command_Action
			{
				icon = ContentFinder<Texture2D>.Get("UI/Commands/CopySettings", true),
				defaultLabel = "CommandCopyZoneSettingsLabel".Translate(),
				defaultDesc = "CommandCopyZoneSettingsDesc".Translate(),
				action = delegate()
				{
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					StorageSettingsClipboard.Copy(s);
				},
				hotKey = KeyBindingDefOf.Misc4
			};
			Command_Action command_Action = new Command_Action();
			command_Action.icon = ContentFinder<Texture2D>.Get("UI/Commands/PasteSettings", true);
			command_Action.defaultLabel = "CommandPasteZoneSettingsLabel".Translate();
			command_Action.defaultDesc = "CommandPasteZoneSettingsDesc".Translate();
			command_Action.action = delegate()
			{
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				StorageSettingsClipboard.PasteInto(s);
			};
			command_Action.hotKey = KeyBindingDefOf.Misc5;
			if (!StorageSettingsClipboard.HasCopiedSettings)
			{
				command_Action.Disable(null);
			}
			yield return command_Action;
			yield break;
		}

		// Token: 0x04003850 RID: 14416
		private static StorageSettings clipboard = new StorageSettings();

		// Token: 0x04003851 RID: 14417
		private static bool copied = false;
	}
}
