using System;
using System.Collections.Generic;
using System.IO;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x02000504 RID: 1284
	public class WorkshopItemHook
	{
		// Token: 0x17000798 RID: 1944
		// (get) Token: 0x060026E8 RID: 9960 RVA: 0x000F0D42 File Offset: 0x000EEF42
		// (set) Token: 0x060026E9 RID: 9961 RVA: 0x000F0D4F File Offset: 0x000EEF4F
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

		// Token: 0x17000799 RID: 1945
		// (get) Token: 0x060026EA RID: 9962 RVA: 0x000F0D5D File Offset: 0x000EEF5D
		public string Name
		{
			get
			{
				return this.owner.GetWorkshopName();
			}
		}

		// Token: 0x1700079A RID: 1946
		// (get) Token: 0x060026EB RID: 9963 RVA: 0x000F0D6A File Offset: 0x000EEF6A
		public string Description
		{
			get
			{
				return this.owner.GetWorkshopDescription();
			}
		}

		// Token: 0x1700079B RID: 1947
		// (get) Token: 0x060026EC RID: 9964 RVA: 0x000F0D77 File Offset: 0x000EEF77
		public string PreviewImagePath
		{
			get
			{
				return this.owner.GetWorkshopPreviewImagePath();
			}
		}

		// Token: 0x1700079C RID: 1948
		// (get) Token: 0x060026ED RID: 9965 RVA: 0x000F0D84 File Offset: 0x000EEF84
		public IList<string> Tags
		{
			get
			{
				return this.owner.GetWorkshopTags();
			}
		}

		// Token: 0x1700079D RID: 1949
		// (get) Token: 0x060026EE RID: 9966 RVA: 0x000F0D91 File Offset: 0x000EEF91
		public DirectoryInfo Directory
		{
			get
			{
				return this.owner.GetWorkshopUploadDirectory();
			}
		}

		// Token: 0x1700079E RID: 1950
		// (get) Token: 0x060026EF RID: 9967 RVA: 0x000F0D9E File Offset: 0x000EEF9E
		public IEnumerable<System.Version> SupportedVersions
		{
			get
			{
				return this.owner.SupportedVersions;
			}
		}

		// Token: 0x1700079F RID: 1951
		// (get) Token: 0x060026F0 RID: 9968 RVA: 0x000F0DAB File Offset: 0x000EEFAB
		public bool MayHaveAuthorNotCurrentUser
		{
			get
			{
				return !(this.PublishedFileId == PublishedFileId_t.Invalid) && (this.steamAuthor == CSteamID.Nil || this.steamAuthor != SteamUser.GetSteamID());
			}
		}

		// Token: 0x060026F1 RID: 9969 RVA: 0x000F0DE5 File Offset: 0x000EEFE5
		public WorkshopItemHook(WorkshopUploadable owner)
		{
			this.owner = owner;
			if (owner.GetPublishedFileId() != PublishedFileId_t.Invalid)
			{
				this.SendSteamDetailsQuery();
			}
		}

		// Token: 0x060026F2 RID: 9970 RVA: 0x000F0E17 File Offset: 0x000EF017
		public void PrepareForWorkshopUpload()
		{
			this.owner.PrepareForWorkshopUpload();
		}

		// Token: 0x060026F3 RID: 9971 RVA: 0x000F0E24 File Offset: 0x000EF024
		private void SendSteamDetailsQuery()
		{
			SteamAPICall_t hAPICall = SteamUGC.RequestUGCDetails(this.PublishedFileId, 999999U);
			this.queryResult = CallResult<SteamUGCRequestUGCDetailsResult_t>.Create(new CallResult<SteamUGCRequestUGCDetailsResult_t>.APIDispatchDelegate(this.OnDetailsQueryReturned));
			this.queryResult.Set(hAPICall, null);
		}

		// Token: 0x060026F4 RID: 9972 RVA: 0x000F0E66 File Offset: 0x000EF066
		private void OnDetailsQueryReturned(SteamUGCRequestUGCDetailsResult_t result, bool IOFailure)
		{
			this.steamAuthor = (CSteamID)result.m_details.m_ulSteamIDOwner;
		}

		// Token: 0x0400183D RID: 6205
		private WorkshopUploadable owner;

		// Token: 0x0400183E RID: 6206
		private CSteamID steamAuthor = CSteamID.Nil;

		// Token: 0x0400183F RID: 6207
		private CallResult<SteamUGCRequestUGCDetailsResult_t> queryResult;
	}
}
