using System;
using System.Collections.Generic;
using System.IO;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x02000503 RID: 1283
	public interface WorkshopUploadable
	{
		// Token: 0x17000797 RID: 1943
		// (get) Token: 0x060026DD RID: 9949
		IEnumerable<System.Version> SupportedVersions { get; }

		// Token: 0x060026DE RID: 9950
		bool CanToUploadToWorkshop();

		// Token: 0x060026DF RID: 9951
		void PrepareForWorkshopUpload();

		// Token: 0x060026E0 RID: 9952
		PublishedFileId_t GetPublishedFileId();

		// Token: 0x060026E1 RID: 9953
		void SetPublishedFileId(PublishedFileId_t pfid);

		// Token: 0x060026E2 RID: 9954
		string GetWorkshopName();

		// Token: 0x060026E3 RID: 9955
		string GetWorkshopDescription();

		// Token: 0x060026E4 RID: 9956
		string GetWorkshopPreviewImagePath();

		// Token: 0x060026E5 RID: 9957
		IList<string> GetWorkshopTags();

		// Token: 0x060026E6 RID: 9958
		DirectoryInfo GetWorkshopUploadDirectory();

		// Token: 0x060026E7 RID: 9959
		WorkshopItemHook GetWorkshopItemHook();
	}
}
