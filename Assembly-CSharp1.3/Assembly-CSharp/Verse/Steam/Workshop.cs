using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using RimWorld;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x020004FE RID: 1278
	public static class Workshop
	{
		// Token: 0x17000793 RID: 1939
		// (get) Token: 0x060026BD RID: 9917 RVA: 0x000F014F File Offset: 0x000EE34F
		public static WorkshopInteractStage CurStage
		{
			get
			{
				return Workshop.curStage;
			}
		}

		// Token: 0x060026BE RID: 9918 RVA: 0x000F0158 File Offset: 0x000EE358
		internal static void Init()
		{
			Workshop.subscribedCallback = Callback<RemoteStoragePublishedFileSubscribed_t>.Create(new Callback<RemoteStoragePublishedFileSubscribed_t>.DispatchDelegate(Workshop.OnItemSubscribed));
			Workshop.installedCallback = Callback<ItemInstalled_t>.Create(new Callback<ItemInstalled_t>.DispatchDelegate(Workshop.OnItemInstalled));
			Workshop.unsubscribedCallback = Callback<RemoteStoragePublishedFileUnsubscribed_t>.Create(new Callback<RemoteStoragePublishedFileUnsubscribed_t>.DispatchDelegate(Workshop.OnItemUnsubscribed));
		}

		// Token: 0x060026BF RID: 9919 RVA: 0x000F01A8 File Offset: 0x000EE3A8
		internal static void Upload(WorkshopUploadable item)
		{
			if (Workshop.curStage != WorkshopInteractStage.None)
			{
				Messages.Message("UploadAlreadyInProgress".Translate(), MessageTypeDefOf.RejectInput, false);
				return;
			}
			Workshop.uploadingHook = item.GetWorkshopItemHook();
			if (Workshop.uploadingHook.PublishedFileId != PublishedFileId_t.Invalid)
			{
				if (Prefs.LogVerbose)
				{
					Log.Message(string.Concat(new object[]
					{
						"Workshop: Starting item update for mod '",
						Workshop.uploadingHook.Name,
						"' with PublishedFileId ",
						Workshop.uploadingHook.PublishedFileId
					}));
				}
				Workshop.curStage = WorkshopInteractStage.SubmittingItem;
				Workshop.curUpdateHandle = SteamUGC.StartItemUpdate(SteamUtils.GetAppID(), Workshop.uploadingHook.PublishedFileId);
				Workshop.SetWorkshopItemDataFrom(Workshop.curUpdateHandle, Workshop.uploadingHook, false);
				SteamAPICall_t hAPICall = SteamUGC.SubmitItemUpdate(Workshop.curUpdateHandle, "[Auto-generated text]: Update on " + DateTime.Now.ToString() + ".");
				Workshop.submitResult = CallResult<SubmitItemUpdateResult_t>.Create(new CallResult<SubmitItemUpdateResult_t>.APIDispatchDelegate(Workshop.OnItemSubmitted));
				Workshop.submitResult.Set(hAPICall, null);
			}
			else
			{
				if (Prefs.LogVerbose)
				{
					Log.Message("Workshop: Starting item creation for mod '" + Workshop.uploadingHook.Name + "'.");
				}
				Workshop.curStage = WorkshopInteractStage.CreatingItem;
				SteamAPICall_t hAPICall2 = SteamUGC.CreateItem(SteamUtils.GetAppID(), EWorkshopFileType.k_EWorkshopFileTypeFirst);
				Workshop.createResult = CallResult<CreateItemResult_t>.Create(new CallResult<CreateItemResult_t>.APIDispatchDelegate(Workshop.OnItemCreated));
				Workshop.createResult.Set(hAPICall2, null);
			}
			Find.WindowStack.Add(new Dialog_WorkshopOperationInProgress());
		}

		// Token: 0x060026C0 RID: 9920 RVA: 0x000F0323 File Offset: 0x000EE523
		internal static void Unsubscribe(WorkshopUploadable item)
		{
			SteamUGC.UnsubscribeItem(item.GetPublishedFileId());
		}

		// Token: 0x060026C1 RID: 9921 RVA: 0x000F0334 File Offset: 0x000EE534
		internal static void RequestItemsDetails(PublishedFileId_t[] publishedFileIds)
		{
			if (Workshop.detailsQueryCount >= 0)
			{
				Log.Error("Requested Workshop item details while a details request was already pending.");
				return;
			}
			Workshop.detailsQueryCount = publishedFileIds.Length;
			Workshop.detailsQueryHandle = SteamUGC.CreateQueryUGCDetailsRequest(publishedFileIds, (uint)Workshop.detailsQueryCount);
			SteamAPICall_t hAPICall = SteamUGC.SendQueryUGCRequest(Workshop.detailsQueryHandle);
			Workshop.requestDetailsResult = CallResult<SteamUGCRequestUGCDetailsResult_t>.Create(new CallResult<SteamUGCRequestUGCDetailsResult_t>.APIDispatchDelegate(Workshop.OnGotItemDetails));
			Workshop.requestDetailsResult.Set(hAPICall, null);
		}

		// Token: 0x060026C2 RID: 9922 RVA: 0x000F0399 File Offset: 0x000EE599
		internal static void OnItemSubscribed(RemoteStoragePublishedFileSubscribed_t result)
		{
			if (!Workshop.IsOurAppId(result.m_nAppID))
			{
				return;
			}
			if (Prefs.LogVerbose)
			{
				Log.Message("Workshop: Item subscribed: " + result.m_nPublishedFileId);
			}
			WorkshopItems.Notify_Subscribed(result.m_nPublishedFileId);
		}

		// Token: 0x060026C3 RID: 9923 RVA: 0x000F03D5 File Offset: 0x000EE5D5
		internal static void OnItemInstalled(ItemInstalled_t result)
		{
			if (!Workshop.IsOurAppId(result.m_unAppID))
			{
				return;
			}
			if (Prefs.LogVerbose)
			{
				Log.Message("Workshop: Item installed: " + result.m_nPublishedFileId);
			}
			WorkshopItems.Notify_Installed(result.m_nPublishedFileId);
		}

		// Token: 0x060026C4 RID: 9924 RVA: 0x000F0414 File Offset: 0x000EE614
		internal static void OnItemUnsubscribed(RemoteStoragePublishedFileUnsubscribed_t result)
		{
			if (!Workshop.IsOurAppId(result.m_nAppID))
			{
				return;
			}
			if (Prefs.LogVerbose)
			{
				Log.Message("Workshop: Item unsubscribed: " + result.m_nPublishedFileId);
			}
			Page_ModsConfig page_ModsConfig = Find.WindowStack.WindowOfType<Page_ModsConfig>();
			if (page_ModsConfig != null)
			{
				page_ModsConfig.Notify_SteamItemUnsubscribed(result.m_nPublishedFileId);
			}
			Page_SelectScenario page_SelectScenario = Find.WindowStack.WindowOfType<Page_SelectScenario>();
			if (page_SelectScenario != null)
			{
				page_SelectScenario.Notify_SteamItemUnsubscribed(result.m_nPublishedFileId);
			}
			WorkshopItems.Notify_Unsubscribed(result.m_nPublishedFileId);
		}

		// Token: 0x060026C5 RID: 9925 RVA: 0x000F0490 File Offset: 0x000EE690
		private static void OnItemCreated(CreateItemResult_t result, bool IOFailure)
		{
			if (IOFailure || result.m_eResult != EResult.k_EResultOK)
			{
				Workshop.uploadingHook = null;
				Dialog_WorkshopOperationInProgress.CloseAll();
				Log.Error("Workshop: OnItemCreated failure. Result: " + result.m_eResult.GetLabel());
				Find.WindowStack.Add(new Dialog_MessageBox("WorkshopSubmissionFailed".Translate(GenText.SplitCamelCase(result.m_eResult.GetLabel())), null, null, null, null, null, false, null, null));
				return;
			}
			Workshop.uploadingHook.PublishedFileId = result.m_nPublishedFileId;
			if (Prefs.LogVerbose)
			{
				Log.Message("Workshop: Item created. PublishedFileId: " + Workshop.uploadingHook.PublishedFileId);
			}
			Workshop.curUpdateHandle = SteamUGC.StartItemUpdate(SteamUtils.GetAppID(), Workshop.uploadingHook.PublishedFileId);
			Workshop.SetWorkshopItemDataFrom(Workshop.curUpdateHandle, Workshop.uploadingHook, true);
			Workshop.curStage = WorkshopInteractStage.SubmittingItem;
			if (Prefs.LogVerbose)
			{
				Log.Message("Workshop: Submitting item.");
			}
			SteamAPICall_t hAPICall = SteamUGC.SubmitItemUpdate(Workshop.curUpdateHandle, "[Auto-generated text]: Initial upload.");
			Workshop.submitResult = CallResult<SubmitItemUpdateResult_t>.Create(new CallResult<SubmitItemUpdateResult_t>.APIDispatchDelegate(Workshop.OnItemSubmitted));
			Workshop.submitResult.Set(hAPICall, null);
			Workshop.createResult = null;
		}

		// Token: 0x060026C6 RID: 9926 RVA: 0x000F05B4 File Offset: 0x000EE7B4
		private static void OnItemSubmitted(SubmitItemUpdateResult_t result, bool IOFailure)
		{
			if (IOFailure || result.m_eResult != EResult.k_EResultOK)
			{
				Workshop.uploadingHook = null;
				Dialog_WorkshopOperationInProgress.CloseAll();
				Log.Error("Workshop: OnItemSubmitted failure. Result: " + result.m_eResult.GetLabel());
				Find.WindowStack.Add(new Dialog_MessageBox("WorkshopSubmissionFailed".Translate(GenText.SplitCamelCase(result.m_eResult.GetLabel())), null, null, null, null, null, false, null, null));
			}
			else
			{
				SteamUtility.OpenWorkshopPage(Workshop.uploadingHook.PublishedFileId);
				Messages.Message("WorkshopUploadSucceeded".Translate(Workshop.uploadingHook.Name), MessageTypeDefOf.TaskCompletion, false);
				if (Prefs.LogVerbose)
				{
					Log.Message("Workshop: Item submit result: " + result.m_eResult);
				}
			}
			Workshop.curStage = WorkshopInteractStage.None;
			Workshop.submitResult = null;
		}

		// Token: 0x060026C7 RID: 9927 RVA: 0x000F0690 File Offset: 0x000EE890
		private static void OnGotItemDetails(SteamUGCRequestUGCDetailsResult_t result, bool IOFailure)
		{
			if (IOFailure)
			{
				Log.Error("Workshop: OnGotItemDetails IOFailure.");
				Workshop.detailsQueryCount = -1;
				return;
			}
			if (Workshop.detailsQueryCount < 0)
			{
				Log.Warning("Got unexpected Steam Workshop item details response.");
			}
			string text = "Steam Workshop Item details received:";
			for (int i = 0; i < Workshop.detailsQueryCount; i++)
			{
				SteamUGCDetails_t steamUGCDetails_t;
				SteamUGC.GetQueryUGCResult(Workshop.detailsQueryHandle, (uint)i, out steamUGCDetails_t);
				if (steamUGCDetails_t.m_eResult != EResult.k_EResultOK)
				{
					text = text + "\n  Query result: " + steamUGCDetails_t.m_eResult;
				}
				else
				{
					text = text + "\n  Title: " + steamUGCDetails_t.m_rgchTitle;
					text = text + "\n  PublishedFileId: " + steamUGCDetails_t.m_nPublishedFileId;
					text = text + "\n  Created: " + DateTime.FromFileTimeUtc((long)((ulong)steamUGCDetails_t.m_rtimeCreated)).ToString();
					text = text + "\n  Updated: " + DateTime.FromFileTimeUtc((long)((ulong)steamUGCDetails_t.m_rtimeUpdated)).ToString();
					text = text + "\n  Added to list: " + DateTime.FromFileTimeUtc((long)((ulong)steamUGCDetails_t.m_rtimeAddedToUserList)).ToString();
					text = text + "\n  File size: " + steamUGCDetails_t.m_nFileSize.ToStringKilobytes("F2");
					text = text + "\n  Preview size: " + steamUGCDetails_t.m_nPreviewFileSize.ToStringKilobytes("F2");
					text = text + "\n  File name: " + steamUGCDetails_t.m_pchFileName;
					text = text + "\n  CreatorAppID: " + steamUGCDetails_t.m_nCreatorAppID;
					text = text + "\n  ConsumerAppID: " + steamUGCDetails_t.m_nConsumerAppID;
					text = text + "\n  Visibiliy: " + steamUGCDetails_t.m_eVisibility;
					text = text + "\n  FileType: " + steamUGCDetails_t.m_eFileType;
					text = text + "\n  Owner: " + steamUGCDetails_t.m_ulSteamIDOwner;
				}
				text += "\n";
			}
			Log.Message(text.TrimEndNewlines());
			Workshop.detailsQueryCount = -1;
		}

		// Token: 0x060026C8 RID: 9928 RVA: 0x000F0878 File Offset: 0x000EEA78
		public static void GetUpdateStatus(out EItemUpdateStatus updateStatus, out float progPercent)
		{
			ulong num;
			ulong num2;
			updateStatus = SteamUGC.GetItemUpdateProgress(Workshop.curUpdateHandle, out num, out num2);
			progPercent = num / num2;
		}

		// Token: 0x060026C9 RID: 9929 RVA: 0x000F089E File Offset: 0x000EEA9E
		public static string UploadButtonLabel(PublishedFileId_t pfid)
		{
			return (pfid != PublishedFileId_t.Invalid) ? "UpdateOnSteamWorkshop".Translate() : "UploadToSteamWorkshop".Translate();
		}

		// Token: 0x060026CA RID: 9930 RVA: 0x000F08C8 File Offset: 0x000EEAC8
		private static void SetWorkshopItemDataFrom(UGCUpdateHandle_t updateHandle, WorkshopItemHook hook, bool creating)
		{
			hook.PrepareForWorkshopUpload();
			SteamUGC.SetItemTitle(updateHandle, hook.Name);
			if (creating)
			{
				SteamUGC.SetItemDescription(updateHandle, hook.Description);
			}
			if (!File.Exists(hook.PreviewImagePath))
			{
				Log.Warning("Missing preview file at " + hook.PreviewImagePath);
			}
			else
			{
				SteamUGC.SetItemPreview(updateHandle, hook.PreviewImagePath);
			}
			IList<string> tags = hook.Tags;
			foreach (System.Version version in hook.SupportedVersions)
			{
				tags.Add(version.Major + "." + version.Minor);
			}
			SteamUGC.SetItemTags(updateHandle, tags);
			SteamUGC.SetItemContent(updateHandle, hook.Directory.FullName);
		}

		// Token: 0x060026CB RID: 9931 RVA: 0x000F09AC File Offset: 0x000EEBAC
		internal static IEnumerable<PublishedFileId_t> AllSubscribedItems()
		{
			uint numSubscribedItems = SteamUGC.GetNumSubscribedItems();
			PublishedFileId_t[] subbedItems = new PublishedFileId_t[numSubscribedItems];
			uint count = SteamUGC.GetSubscribedItems(subbedItems, numSubscribedItems);
			int i = 0;
			while ((long)i < (long)((ulong)count))
			{
				PublishedFileId_t publishedFileId_t = subbedItems[i];
				yield return publishedFileId_t;
				int num = i;
				i = num + 1;
			}
			yield break;
		}

		// Token: 0x060026CC RID: 9932 RVA: 0x000F09B8 File Offset: 0x000EEBB8
		[DebugOutput("System", false)]
		internal static void SteamWorkshopStatus()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("All subscribed items (" + SteamUGC.GetNumSubscribedItems() + " total):");
			List<PublishedFileId_t> list = Workshop.AllSubscribedItems().ToList<PublishedFileId_t>();
			for (int i = 0; i < list.Count; i++)
			{
				stringBuilder.AppendLine("   " + Workshop.ItemStatusString(list[i]));
			}
			stringBuilder.AppendLine("All installed mods:");
			foreach (ModMetaData modMetaData in ModLister.AllInstalledMods)
			{
				stringBuilder.AppendLine("   " + modMetaData.PackageIdPlayerFacing + ": " + Workshop.ItemStatusString(modMetaData.GetPublishedFileId()));
			}
			Log.Message(stringBuilder.ToString());
			List<PublishedFileId_t> list2 = Workshop.AllSubscribedItems().ToList<PublishedFileId_t>();
			PublishedFileId_t[] array = new PublishedFileId_t[list2.Count];
			for (int j = 0; j < list2.Count; j++)
			{
				array[j] = (PublishedFileId_t)list2[j].m_PublishedFileId;
			}
			Workshop.RequestItemsDetails(array);
		}

		// Token: 0x060026CD RID: 9933 RVA: 0x000F0AF0 File Offset: 0x000EECF0
		private static string ItemStatusString(PublishedFileId_t pfid)
		{
			if (pfid == PublishedFileId_t.Invalid)
			{
				return "[unpublished]";
			}
			string text = "[" + pfid + "] ";
			ulong num;
			string str;
			uint num2;
			if (SteamUGC.GetItemInstallInfo(pfid, out num, out str, 257U, out num2))
			{
				text += "\n      installed";
				text = text + "\n      folder=" + str;
				text = text + "\n      sizeOnDisk=" + (num / 1024f).ToString("F2") + "Kb";
			}
			else
			{
				text += "\n      not installed";
			}
			return text;
		}

		// Token: 0x060026CE RID: 9934 RVA: 0x000F0B89 File Offset: 0x000EED89
		private static bool IsOurAppId(AppId_t appId)
		{
			return !(appId != SteamUtils.GetAppID());
		}

		// Token: 0x0400182E RID: 6190
		private static WorkshopItemHook uploadingHook;

		// Token: 0x0400182F RID: 6191
		private static UGCUpdateHandle_t curUpdateHandle;

		// Token: 0x04001830 RID: 6192
		private static WorkshopInteractStage curStage = WorkshopInteractStage.None;

		// Token: 0x04001831 RID: 6193
		private static Callback<RemoteStoragePublishedFileSubscribed_t> subscribedCallback;

		// Token: 0x04001832 RID: 6194
		private static Callback<RemoteStoragePublishedFileUnsubscribed_t> unsubscribedCallback;

		// Token: 0x04001833 RID: 6195
		private static Callback<ItemInstalled_t> installedCallback;

		// Token: 0x04001834 RID: 6196
		private static CallResult<SubmitItemUpdateResult_t> submitResult;

		// Token: 0x04001835 RID: 6197
		private static CallResult<CreateItemResult_t> createResult;

		// Token: 0x04001836 RID: 6198
		private static CallResult<SteamUGCRequestUGCDetailsResult_t> requestDetailsResult;

		// Token: 0x04001837 RID: 6199
		private static UGCQueryHandle_t detailsQueryHandle;

		// Token: 0x04001838 RID: 6200
		private static int detailsQueryCount = -1;

		// Token: 0x04001839 RID: 6201
		public const uint InstallInfoFolderNameMaxLength = 257U;
	}
}
