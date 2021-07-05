using System;

namespace Verse
{
	// Token: 0x020003A0 RID: 928
	public static class DebugActionsTranslations
	{
		// Token: 0x06001BA0 RID: 7072 RVA: 0x000A1876 File Offset: 0x0009FA76
		[DebugAction("Translation", null, allowedGameStates = AllowedGameStates.Entry)]
		private static void WriteBackstoryTranslationFile()
		{
			LanguageDataWriter.WriteBackstoryFile();
		}

		// Token: 0x06001BA1 RID: 7073 RVA: 0x000A187D File Offset: 0x0009FA7D
		[DebugAction("Translation", null, allowedGameStates = AllowedGameStates.Entry)]
		private static void SaveTranslationReport()
		{
			LanguageReportGenerator.SaveTranslationReport();
		}
	}
}
