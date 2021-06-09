using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using RimWorld;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x020008B8 RID: 2232
	public static class Workshop
	{
		// Token: 0x170008B8 RID: 2232
		// (get) Token: 0x06003784 RID: 14212 RVA: 0x0002AF59 File Offset: 0x00029159
		public static WorkshopInteractStage CurStage
		{
			get
			{
				return Workshop.curStage;
			}
		}

		// Token: 0x06003785 RID: 14213 RVA: 0x00160D88 File Offset: 0x0015EF88
		internal static void Init()
		{
			Workshop.subscribedCallback = Callback<RemoteStoragePublishedFileSubscribed_t>.Create(new Callback<RemoteStoragePublishedFileSubscribed_t>.DispatchDelegate(Workshop.OnItemSubscribed));
			Workshop.installedCallback = Callback<ItemInstalled_t>.Create(new Callback<ItemInstalled_t>.DispatchDelegate(Workshop.OnItemInstalled));
			Workshop.unsubscribedCallback = Callback<RemoteStoragePublishedFileUnsubscribed_t>.Create(new Callback<RemoteStoragePublishedFileUnsubscribed_t>.DispatchDelegate(Workshop.OnItemUnsubscribed));
		}

		// Token: 0x06003786 RID: 14214 RVA: 0x00160DD8 File Offset: 0x0015EFD8
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
					}), false);
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
					Log.Message("Workshop: Starting item creation for mod '" + Workshop.uploadingHook.Name + "'.", false);
				}
				Workshop.curStage = WorkshopInteractStage.CreatingItem;
				SteamAPICall_t hAPICall2 = SteamUGC.CreateItem(SteamUtils.GetAppID(), EWorkshopFileType.k_EWorkshopFileTypeFirst);
				Workshop.createResult = CallResult<CreateItemResult_t>.Create(new CallResult<CreateItemResult_t>.APIDispatchDelegate(Workshop.OnItemCreated));
				Workshop.createResult.Set(hAPICall2, null);
			}
			Find.WindowStack.Add(new Dialog_WorkshopOperationInProgress());
		}

		// Token: 0x06003787 RID: 14215 RVA: 0x0002AF60 File Offset: 0x00029160
		internal static void Unsubscribe(WorkshopUploadable item)
		{
			SteamUGC.UnsubscribeItem(item.GetPublishedFileId());
		}

		// Token: 0x06003788 RID: 14216 RVA: 0x00160F58 File Offset: 0x0015F158
		internal static void RequestItemsDetails(PublishedFileId_t[] publishedFileIds)
		{
			if (Workshop.detailsQueryCount >= 0)
			{
				Log.Error("Requested Workshop item details while a details request was already pending.", false);
				return;
			}
			Workshop.detailsQueryCount = publishedFileIds.Length;
			Workshop.detailsQueryHandle = SteamUGC.CreateQueryUGCDetailsRequest(publishedFileIds, (uint)Workshop.detailsQueryCount);
			SteamAPICall_t hAPICall = SteamUGC.SendQueryUGCRequest(Workshop.detailsQueryHandle);
			Workshop.requestDetailsResult = CallResult<SteamUGCRequestUGCDetailsResult_t>.Create(new CallResult<SteamUGCRequestUGCDetailsResult_t>.APIDispatchDelegate(Workshop.OnGotItemDetails));
			Workshop.requestDetailsResult.Set(hAPICall, null);
		}

		// Token: 0x06003789 RID: 14217 RVA: 0x0002AF6E File Offset: 0x0002916E
		internal static void OnItemSubscribed(RemoteStoragePublishedFileSubscribed_t result)
		{
			if (!Workshop.IsOurAppId(result.m_nAppID))
			{
				return;
			}
			if (Prefs.LogVerbose)
			{
				Log.Message("Workshop: Item subscribed: " + result.m_nPublishedFileId, false);
			}
			WorkshopItems.Notify_Subscribed(result.m_nPublishedFileId);
		}

		// Token: 0x0600378A RID: 14218 RVA: 0x0002AFAB File Offset: 0x000291AB
		internal static void OnItemInstalled(ItemInstalled_t result)
		{
			if (!Workshop.IsOurAppId(result.m_unAppID))
			{
				return;
			}
			if (Prefs.LogVerbose)
			{
				Log.Message("Workshop: Item installed: " + result.m_nPublishedFileId, false);
			}
			WorkshopItems.Notify_Installed(result.m_nPublishedFileId);
		}

		// Token: 0x0600378B RID: 14219 RVA: 0x00160FC0 File Offset: 0x0015F1C0
		internal static void OnItemUnsubscribed(RemoteStoragePublishedFileUnsubscribed_t result)
		{
			if (!Workshop.IsOurAppId(result.m_nAppID))
			{
				return;
			}
			if (Prefs.LogVerbose)
			{
				Log.Message("Workshop: Item unsubscribed: " + result.m_nPublishedFileId, false);
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

		// Token: 0x0600378C RID: 14220 RVA: 0x0016103C File Offset: 0x0015F23C
		private static void OnItemCreated(CreateItemResult_t result, bool IOFailure)
		{
			if (IOFailure || result.m_eResult != EResult.k_EResultOK)
			{
				Workshop.uploadingHook = null;
				Dialog_WorkshopOperationInProgress.CloseAll();
				Log.Error("Workshop: OnItemCreated failure. Result: " + result.m_eResult.GetLabel(), false);
				Find.WindowStack.Add(new Dialog_MessageBox("WorkshopSubmissionFailed".Translate(GenText.SplitCamelCase(result.m_eResult.GetLabel())), null, null, null, null, null, false, null, null));
				return;
			}
			Workshop.uploadingHook.PublishedFileId = result.m_nPublishedFileId;
			if (Prefs.LogVerbose)
			{
				Log.Message("Workshop: Item created. PublishedFileId: " + Workshop.uploadingHook.PublishedFileId, false);
			}
			Workshop.curUpdateHandle = SteamUGC.StartItemUpdate(SteamUtils.GetAppID(), Workshop.uploadingHook.PublishedFileId);
			Workshop.SetWorkshopItemDataFrom(Workshop.curUpdateHandle, Workshop.uploadingHook, true);
			Workshop.curStage = WorkshopInteractStage.SubmittingItem;
			if (Prefs.LogVerbose)
			{
				Log.Message("Workshop: Submitting item.", false);
			}
			SteamAPICall_t hAPICall = SteamUGC.SubmitItemUpdate(Workshop.curUpdateHandle, "[Auto-generated text]: Initial upload.");
			Workshop.submitResult = CallResult<SubmitItemUpdateResult_t>.Create(new CallResult<SubmitItemUpdateResult_t>.APIDispatchDelegate(Workshop.OnItemSubmitted));
			Workshop.submitResult.Set(hAPICall, null);
			Workshop.createResult = null;
		}

		// Token: 0x0600378D RID: 14221 RVA: 0x00161164 File Offset: 0x0015F364
		private static void OnItemSubmitted(SubmitItemUpdateResult_t result, bool IOFailure)
		{
			if (IOFailure || result.m_eResult != EResult.k_EResultOK)
			{
				Workshop.uploadingHook = null;
				Dialog_WorkshopOperationInProgress.CloseAll();
				Log.Error("Workshop: OnItemSubmitted failure. Result: " + result.m_eResult.GetLabel(), false);
				Find.WindowStack.Add(new Dialog_MessageBox("WorkshopSubmissionFailed".Translate(GenText.SplitCamelCase(result.m_eResult.GetLabel())), null, null, null, null, null, false, null, null));
			}
			else
			{
				SteamUtility.OpenWorkshopPage(Workshop.uploadingHook.PublishedFileId);
				Messages.Message("WorkshopUploadSucceeded".Translate(Workshop.uploadingHook.Name), MessageTypeDefOf.TaskCompletion, false);
				if (Prefs.LogVerbose)
				{
					Log.Message("Workshop: Item submit result: " + result.m_eResult, false);
				}
			}
			Workshop.curStage = WorkshopInteractStage.None;
			Workshop.submitResult = null;
		}

		// Token: 0x0600378E RID: 14222 RVA: 0x00161244 File Offset: 0x0015F444
		private static void OnGotItemDetails(SteamUGCRequestUGCDetailsResult_t result, bool IOFailure)
		{
			if (IOFailure)
			{
				Log.Error("Workshop: OnGotItemDetails IOFailure.", false);
				Workshop.detailsQueryCount = -1;
				return;
			}
			if (Workshop.detailsQueryCount < 0)
			{
				Log.Warning("Got unexpected Steam Workshop item details response.", false);
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
			Log.Message(text.TrimEndNewlines(), false);
			Workshop.detailsQueryCount = -1;
		}

		// Token: 0x0600378F RID: 14223 RVA: 0x0016142C File Offset: 0x0015F62C
		public static void GetUpdateStatus(out EItemUpdateStatus updateStatus, out float progPercent)
		{
			ulong num;
			ulong num2;
			updateStatus = SteamUGC.GetItemUpdateProgress(Workshop.curUpdateHandle, out num, out num2);
			progPercent = num / num2;
		}

		// Token: 0x06003790 RID: 14224 RVA: 0x0002AFE8 File Offset: 0x000291E8
		public static string UploadButtonLabel(PublishedFileId_t pfid)
		{
			return (pfid != PublishedFileId_t.Invalid) ? "UpdateOnSteamWorkshop".Translate() : "UploadToSteamWorkshop".Translate();
		}

		// Token: 0x06003791 RID: 14225 RVA: 0x00161454 File Offset: 0x0015F654
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
				Log.Warning("Missing preview file at " + hook.PreviewImagePath, false);
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

		// Token: 0x06003792 RID: 14226 RVA: 0x0002B012 File Offset: 0x00029212
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

		// Token: 0x06003793 RID: 14227 RVA: 0x00161538 File Offset: 0x0015F738
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
			Log.Message(stringBuilder.ToString(), false);
			List<PublishedFileId_t> list2 = Workshop.AllSubscribedItems().ToList<PublishedFileId_t>();
			PublishedFileId_t[] array = new PublishedFileId_t[list2.Count];
			for (int j = 0; j < list2.Count; j++)
			{
				array[j] = (PublishedFileId_t)list2[j].m_PublishedFileId;
			}
			Workshop.RequestItemsDetails(array);
		}

		// Token: 0x06003794 RID: 14228 RVA: 0x00161674 File Offset: 0x0015F874
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

		// Token: 0x06003795 RID: 14229 RVA: 0x0002B01B File Offset: 0x0002921B
		private static bool IsOurAppId(AppId_t appId)
		{
			return !(appId != SteamUtils.GetAppID());
		}

		// Token: 0x04002697 RID: 9879
		private static WorkshopItemHook uploadingHook;

		// Token: 0x04002698 RID: 9880
		private static UGCUpdateHandle_t curUpdateHandle;

		// Token: 0x04002699 RID: 9881
		private static WorkshopInteractStage curStage = WorkshopInteractStage.None;

		// Token: 0x0400269A RID: 9882
		private static Callback<RemoteStoragePublishedFileSubscribed_t> subscribedCallback;

		// Token: 0x0400269B RID: 9883
		private static Callback<RemoteStoragePublishedFileUnsubscribed_t> unsubscribedCallback;

		// Token: 0x0400269C RID: 9884
		private static Callback<ItemInstalled_t> installedCallback;

		// Token: 0x0400269D RID: 9885
		private static CallResult<SubmitItemUpdateResult_t> submitResult;

		// Token: 0x0400269E RID: 9886
		private static CallResult<CreateItemResult_t> createResult;

		// Token: 0x0400269F RID: 9887
		private static CallResult<SteamUGCRequestUGCDetailsResult_t> requestDetailsResult;

		// Token: 0x040026A0 RID: 9888
		private static UGCQueryHandle_t detailsQueryHandle;

		// Token: 0x040026A1 RID: 9889
		private static int detailsQueryCount = -1;

		// Token: 0x040026A2 RID: 9890
		public const uint InstallInfoFolderNameMaxLength = 257U;
	}
}
