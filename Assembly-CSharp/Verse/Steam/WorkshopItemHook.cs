using System;
using System.Collections.Generic;
using System.IO;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x020008C0 RID: 2240
	public class WorkshopItemHook
	{
		// Token: 0x170008BF RID: 2239
		// (get) Token: 0x060037BA RID: 14266 RVA: 0x0002B0FC File Offset: 0x000292FC
		// (set) Token: 0x060037BB RID: 14267 RVA: 0x0002B109 File Offset: 0x00029309
		public PublishedFileId_t PublishedFileId
		{
			get
			{
				return this.owner.GetPublishedFileId();
			}
			set
			{
				this.owner.SetPublishedFileId(value);
			}
		}

		// Token: 0x170008C0 RID: 2240
		// (get) Token: 0x060037BC RID: 14268 RVA: 0x0002B117 File Offset: 0x00029317
		public string Name
		{
			get
			{
				return this.owner.GetWorkshopName();
			}
		}

		// Token: 0x170008C1 RID: 2241
		// (get) Token: 0x060037BD RID: 14269 RVA: 0x0002B124 File Offset: 0x00029324
		public string Description
		{
			get
			{
				return this.owner.GetWorkshopDescription();
			}
		}

		// Token: 0x170008C2 RID: 2242
		// (get) Token: 0x060037BE RID: 14270 RVA: 0x0002B131 File Offset: 0x00029331
		public string PreviewImagePath
		{
			get
			{
				return this.owner.GetWorkshopPreviewImagePath();
			}
		}

		// Token: 0x170008C3 RID: 2243
		// (get) Token: 0x060037BF RID: 14271 RVA: 0x0002B13E File Offset: 0x0002933E
		public IList<string> Tags
		{
			get
			{
				return this.owner.GetWorkshopTags();
			}
		}

		// Token: 0x170008C4 RID: 2244
		// (get) Token: 0x060037C0 RID: 14272 RVA: 0x0002B14B File Offset: 0x0002934B
		public DirectoryInfo Directory
		{
			get
			{
				return this.owner.GetWorkshopUploadDirectory();
			}
		}

		// Token: 0x170008C5 RID: 2245
		// (get) Token: 0x060037C1 RID: 14273 RVA: 0x0002B158 File Offset: 0x00029358
		public IEnumerable<System.Version> SupportedVersions
		{
			get
			{
				return this.owner.SupportedVersions;
			}
		}

		// Token: 0x170008C6 RID: 2246
		// (get) Token: 0x060037C2 RID: 14274 RVA: 0x0002B165 File Offset: 0x00029365
		public bool MayHaveAuthorNotCurrentUser
		{
			get
			{
				return !(this.PublishedFileId == PublishedFileId_t.Invalid) && (this.steamAuthor == CSteamID.Nil || this.steamAuthor != SteamUser.GetSteamID());
			}
		}

		// Token: 0x060037C3 RID: 14275 RVA: 0x0002B19F File Offset: 0x0002939F
		public WorkshopItemHook(WorkshopUploadable owner)
		{
			this.owner = owner;
			if (owner.GetPublishedFileId() != PublishedFileId_t.Invalid)
			{
				this.SendSteamDetailsQuery();
			}
		}

		// Token: 0x060037C4 RID: 14276 RVA: 0x0002B1D1 File Offset: 0x000293D1
		public void PrepareForWorkshopUpload()
		{
			this.owner.PrepareForWorkshopUpload();
		}

		// Token: 0x060037C5 RID: 14277 RVA: 0x00161900 File Offset: 0x0015FB00
		private void SendSteamDetailsQuery()
		{
			SteamAPICall_t hAPICall = SteamUGC.RequestUGCDetails(this.PublishedFileId, 999999U);
			this.queryResult = CallResult<SteamUGCRequestUGCDetailsResult_t>.Create(new CallResult<SteamUGCRequestUGCDetailsResult_t>.APIDispatchDelegate(this.OnDetailsQueryReturned));
			this.queryResult.Set(hAPICall, null);
		}

		// Token: 0x060037C6 RID: 14278 RVA: 0x0002B1DE File Offset: 0x000293DE
		private void OnDetailsQueryReturned(SteamUGCRequestUGCDetailsResult_t result, bool IOFailure)
		{
			this.steamAuthor = (CSteamID)result.m_details.m_ulSteamIDOwner;
		}

		// Token: 0x040026AE RID: 9902
		private WorkshopUploadable owner;

		// Token: 0x040026AF RID: 9903
		private CSteamID steamAuthor = CSteamID.Nil;

		// Token: 0x040026B0 RID: 9904
		private CallResult<SteamUGCRequestUGCDetailsResult_t> queryResult;
	}
}
