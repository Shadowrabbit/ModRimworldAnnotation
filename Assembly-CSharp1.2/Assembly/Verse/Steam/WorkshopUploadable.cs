using System;
using System.Collections.Generic;
using System.IO;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x020008BF RID: 2239
	public interface WorkshopUploadable
	{
		// Token: 0x170008BE RID: 2238
		// (get) Token: 0x060037AF RID: 14255
		IEnumerable<System.Version> SupportedVersions { get; }

		// Token: 0x060037B0 RID: 14256
		bool CanToUploadToWorkshop();

		// Token: 0x060037B1 RID: 14257
		void PrepareForWorkshopUpload();

		// Token: 0x060037B2 RID: 14258
		PublishedFileId_t GetPublishedFileId();

		// Token: 0x060037B3 RID: 14259
		void SetPublishedFileId(PublishedFileId_t pfid);

		// Token: 0x060037B4 RID: 14260
		string GetWorkshopName();

		// Token: 0x060037B5 RID: 14261
		string GetWorkshopDescription();

		// Token: 0x060037B6 RID: 14262
		string GetWorkshopPreviewImagePath();

		// Token: 0x060037B7 RID: 14263
		IList<string> GetWorkshopTags();

		// Token: 0x060037B8 RID: 14264
		DirectoryInfo GetWorkshopUploadDirectory();

		// Token: 0x060037B9 RID: 14265
		WorkshopItemHook GetWorkshopItemHook();
	}
}
