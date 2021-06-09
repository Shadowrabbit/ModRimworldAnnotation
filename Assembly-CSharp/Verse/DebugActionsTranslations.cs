using System;

namespace Verse
{
	// Token: 0x020005A5 RID: 1445
	public static class DebugActionsTranslations
	{
		// Token: 0x06002442 RID: 9282 RVA: 0x0001E643 File Offset: 0x0001C843
		[DebugAction("Translation", null, allowedGameStates = AllowedGameStates.Entry)]
		private static void WriteBackstoryTranslationFile()
		{
			LanguageDataWriter.WriteBackstoryFile();
		}

		// Token: 0x06002443 RID: 9283 RVA: 0x0001E64A File Offset: 0x0001C84A
		[DebugAction("Translation", null, allowedGameStates = AllowedGameStates.Entry)]
		private static void SaveTranslationReport()
		{
			LanguageReportGenerator.SaveTranslationReport();
		}
	}
}
