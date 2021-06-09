using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020015B0 RID: 5552
	public class UniqueIDsManager : IExposable
	{
		// Token: 0x06007887 RID: 30855 RVA: 0x00051306 File Offset: 0x0004F506
		public int GetNextThingID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextThingID);
		}

		// Token: 0x06007888 RID: 30856 RVA: 0x00051313 File Offset: 0x0004F513
		public int GetNextBillID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextBillID);
		}

		// Token: 0x06007889 RID: 30857 RVA: 0x00051320 File Offset: 0x0004F520
		public int GetNextFactionID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextFactionID);
		}

		// Token: 0x0600788A RID: 30858 RVA: 0x0005132D File Offset: 0x0004F52D
		public int GetNextLordID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextLordID);
		}

		// Token: 0x0600788B RID: 30859 RVA: 0x0005133A File Offset: 0x0004F53A
		public int GetNextTaleID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextTaleID);
		}

		// Token: 0x0600788C RID: 30860 RVA: 0x00051347 File Offset: 0x0004F547
		public int GetNextPassingShipID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextPassingShipID);
		}

		// Token: 0x0600788D RID: 30861 RVA: 0x00051354 File Offset: 0x0004F554
		public int GetNextWorldObjectID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextWorldObjectID);
		}

		// Token: 0x0600788E RID: 30862 RVA: 0x00051361 File Offset: 0x0004F561
		public int GetNextMapID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextMapID);
		}

		// Token: 0x0600788F RID: 30863 RVA: 0x0005136E File Offset: 0x0004F56E
		public int GetNextCaravanID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextCaravanID);
		}

		// Token: 0x06007890 RID: 30864 RVA: 0x0005137B File Offset: 0x0004F57B
		public int GetNextAreaID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextAreaID);
		}

		// Token: 0x06007891 RID: 30865 RVA: 0x00051388 File Offset: 0x0004F588
		public int GetNextTransporterGroupID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextTransporterGroupID);
		}

		// Token: 0x06007892 RID: 30866 RVA: 0x00051395 File Offset: 0x0004F595
		public int GetNextAncientCryptosleepCasketGroupID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextAncientCryptosleepCasketGroupID);
		}

		// Token: 0x06007893 RID: 30867 RVA: 0x000513A2 File Offset: 0x0004F5A2
		public int GetNextJobID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextJobID);
		}

		// Token: 0x06007894 RID: 30868 RVA: 0x000513AF File Offset: 0x0004F5AF
		public int GetNextSignalTagID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextSignalTagID);
		}

		// Token: 0x06007895 RID: 30869 RVA: 0x000513BC File Offset: 0x0004F5BC
		public int GetNextWorldFeatureID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextWorldFeatureID);
		}

		// Token: 0x06007896 RID: 30870 RVA: 0x000513C9 File Offset: 0x0004F5C9
		public int GetNextHediffID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextHediffID);
		}

		// Token: 0x06007897 RID: 30871 RVA: 0x000513D6 File Offset: 0x0004F5D6
		public int GetNextBattleID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextBattleID);
		}

		// Token: 0x06007898 RID: 30872 RVA: 0x000513E3 File Offset: 0x0004F5E3
		public int GetNextLogID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextLogID);
		}

		// Token: 0x06007899 RID: 30873 RVA: 0x000513F0 File Offset: 0x0004F5F0
		public int GetNextLetterID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextLetterID);
		}

		// Token: 0x0600789A RID: 30874 RVA: 0x000513FD File Offset: 0x0004F5FD
		public int GetNextArchivedDialogID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextArchivedDialogID);
		}

		// Token: 0x0600789B RID: 30875 RVA: 0x0005140A File Offset: 0x0004F60A
		public int GetNextMessageID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextMessageID);
		}

		// Token: 0x0600789C RID: 30876 RVA: 0x00051417 File Offset: 0x0004F617
		public int GetNextZoneID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextZoneID);
		}

		// Token: 0x0600789D RID: 30877 RVA: 0x00051424 File Offset: 0x0004F624
		public int GetNextQuestID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextQuestID);
		}

		// Token: 0x0600789E RID: 30878 RVA: 0x00051431 File Offset: 0x0004F631
		public int GetNextGameConditionID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextGameConditionID);
		}

		// Token: 0x0600789F RID: 30879 RVA: 0x0005143E File Offset: 0x0004F63E
		public UniqueIDsManager()
		{
			this.nextThingID = Rand.Range(0, 1000);
		}

		// Token: 0x060078A0 RID: 30880 RVA: 0x00051457 File Offset: 0x0004F657
		private static int GetNextID(ref int nextID)
		{
			if (Scribe.mode == LoadSaveMode.Saving || Scribe.mode == LoadSaveMode.LoadingVars)
			{
				Log.Warning("Getting next unique ID during saving or loading. This may cause bugs.", false);
			}
			int result = nextID;
			nextID++;
			if (nextID == 2147483647)
			{
				Log.Warning("Next ID is at max value. Resetting to 0. This may cause bugs.", false);
				nextID = 0;
			}
			return result;
		}

		// Token: 0x060078A1 RID: 30881 RVA: 0x0024A890 File Offset: 0x00248A90
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.nextThingID, "nextThingID", 0, false);
			Scribe_Values.Look<int>(ref this.nextBillID, "nextBillID", 0, false);
			Scribe_Values.Look<int>(ref this.nextFactionID, "nextFactionID", 0, false);
			Scribe_Values.Look<int>(ref this.nextLordID, "nextLordID", 0, false);
			Scribe_Values.Look<int>(ref this.nextTaleID, "nextTaleID", 0, false);
			Scribe_Values.Look<int>(ref this.nextPassingShipID, "nextPassingShipID", 0, false);
			Scribe_Values.Look<int>(ref this.nextWorldObjectID, "nextWorldObjectID", 0, false);
			Scribe_Values.Look<int>(ref this.nextMapID, "nextMapID", 0, false);
			Scribe_Values.Look<int>(ref this.nextCaravanID, "nextCaravanID", 0, false);
			Scribe_Values.Look<int>(ref this.nextAreaID, "nextAreaID", 0, false);
			Scribe_Values.Look<int>(ref this.nextTransporterGroupID, "nextTransporterGroupID", 0, false);
			Scribe_Values.Look<int>(ref this.nextAncientCryptosleepCasketGroupID, "nextAncientCryptosleepCasketGroupID", 0, false);
			Scribe_Values.Look<int>(ref this.nextJobID, "nextJobID", 0, false);
			Scribe_Values.Look<int>(ref this.nextSignalTagID, "nextSignalTagID", 0, false);
			Scribe_Values.Look<int>(ref this.nextWorldFeatureID, "nextWorldFeatureID", 0, false);
			Scribe_Values.Look<int>(ref this.nextHediffID, "nextHediffID", 0, false);
			Scribe_Values.Look<int>(ref this.nextBattleID, "nextBattleID", 0, false);
			Scribe_Values.Look<int>(ref this.nextLogID, "nextLogID", 0, false);
			Scribe_Values.Look<int>(ref this.nextLetterID, "nextLetterID", 0, false);
			Scribe_Values.Look<int>(ref this.nextArchivedDialogID, "nextArchivedDialogID", 0, false);
			Scribe_Values.Look<int>(ref this.nextMessageID, "nextMessageID", 0, false);
			Scribe_Values.Look<int>(ref this.nextZoneID, "nextZoneID", 0, false);
			Scribe_Values.Look<int>(ref this.nextQuestID, "nextQuestID", 0, false);
			Scribe_Values.Look<int>(ref this.nextGameConditionID, "nextGameConditionID", 0, false);
		}

		// Token: 0x04004F67 RID: 20327
		private int nextThingID;

		// Token: 0x04004F68 RID: 20328
		private int nextBillID;

		// Token: 0x04004F69 RID: 20329
		private int nextFactionID;

		// Token: 0x04004F6A RID: 20330
		private int nextLordID;

		// Token: 0x04004F6B RID: 20331
		private int nextTaleID;

		// Token: 0x04004F6C RID: 20332
		private int nextPassingShipID;

		// Token: 0x04004F6D RID: 20333
		private int nextWorldObjectID;

		// Token: 0x04004F6E RID: 20334
		private int nextMapID;

		// Token: 0x04004F6F RID: 20335
		private int nextCaravanID;

		// Token: 0x04004F70 RID: 20336
		private int nextAreaID;

		// Token: 0x04004F71 RID: 20337
		private int nextTransporterGroupID;

		// Token: 0x04004F72 RID: 20338
		private int nextAncientCryptosleepCasketGroupID;

		// Token: 0x04004F73 RID: 20339
		private int nextJobID;

		// Token: 0x04004F74 RID: 20340
		private int nextSignalTagID;

		// Token: 0x04004F75 RID: 20341
		private int nextWorldFeatureID;

		// Token: 0x04004F76 RID: 20342
		private int nextHediffID;

		// Token: 0x04004F77 RID: 20343
		private int nextBattleID;

		// Token: 0x04004F78 RID: 20344
		private int nextLogID;

		// Token: 0x04004F79 RID: 20345
		private int nextLetterID;

		// Token: 0x04004F7A RID: 20346
		private int nextArchivedDialogID;

		// Token: 0x04004F7B RID: 20347
		private int nextMessageID;

		// Token: 0x04004F7C RID: 20348
		private int nextZoneID;

		// Token: 0x04004F7D RID: 20349
		private int nextQuestID;

		// Token: 0x04004F7E RID: 20350
		private int nextGameConditionID;
	}
}
