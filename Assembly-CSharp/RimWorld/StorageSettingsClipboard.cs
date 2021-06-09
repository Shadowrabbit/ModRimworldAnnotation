using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001697 RID: 5783
	public static class StorageSettingsClipboard
	{
		// Token: 0x17001391 RID: 5009
		// (get) Token: 0x06007E7E RID: 32382 RVA: 0x00055004 File Offset: 0x00053204
		public static bool HasCopiedSettings
		{
			get
			{
				return StorageSettingsClipboard.copied;
			}
		}

		// Token: 0x06007E7F RID: 32383 RVA: 0x0005500B File Offset: 0x0005320B
		public static void Copy(StorageSettings s)
		{
			StorageSettingsClipboard.clipboard.CopyFrom(s);
			StorageSettingsClipboard.copied = true;
		}

		// Token: 0x06007E80 RID: 32384 RVA: 0x0005501E File Offset: 0x0005321E
		public static void PasteInto(StorageSettings s)
		{
			s.CopyFrom(StorageSettingsClipboard.clipboard);
		}

		// Token: 0x06007E81 RID: 32385 RVA: 0x0005502B File Offset: 0x0005322B
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

		// Token: 0x04005253 RID: 21075
		private static StorageSettings clipboard = new StorageSettings();

		// Token: 0x04005254 RID: 21076
		private static bool copied = false;
	}
}
