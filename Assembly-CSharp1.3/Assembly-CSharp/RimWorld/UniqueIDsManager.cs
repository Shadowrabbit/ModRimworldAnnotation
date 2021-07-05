using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FE1 RID: 4065
	public class UniqueIDsManager : IExposable
	{
		// Token: 0x06005FB8 RID: 24504 RVA: 0x0020B760 File Offset: 0x00209960
		public int GetNextThingID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextThingID);
		}

		// Token: 0x06005FB9 RID: 24505 RVA: 0x0020B76D File Offset: 0x0020996D
		public int GetNextBillID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextBillID);
		}

		// Token: 0x06005FBA RID: 24506 RVA: 0x0020B77A File Offset: 0x0020997A
		public int GetNextFactionID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextFactionID);
		}

		// Token: 0x06005FBB RID: 24507 RVA: 0x0020B787 File Offset: 0x00209987
		public int GetNextLordID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextLordID);
		}

		// Token: 0x06005FBC RID: 24508 RVA: 0x0020B794 File Offset: 0x00209994
		public int GetNextTaleID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextTaleID);
		}

		// Token: 0x06005FBD RID: 24509 RVA: 0x0020B7A1 File Offset: 0x002099A1
		public int GetNextPassingShipID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextPassingShipID);
		}

		// Token: 0x06005FBE RID: 24510 RVA: 0x0020B7AE File Offset: 0x002099AE
		public int GetNextWorldObjectID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextWorldObjectID);
		}

		// Token: 0x06005FBF RID: 24511 RVA: 0x0020B7BB File Offset: 0x002099BB
		public int GetNextMapID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextMapID);
		}

		// Token: 0x06005FC0 RID: 24512 RVA: 0x0020B7C8 File Offset: 0x002099C8
		public int GetNextCaravanID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextCaravanID);
		}

		// Token: 0x06005FC1 RID: 24513 RVA: 0x0020B7D5 File Offset: 0x002099D5
		public int GetNextAreaID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextAreaID);
		}

		// Token: 0x06005FC2 RID: 24514 RVA: 0x0020B7E2 File Offset: 0x002099E2
		public int GetNextAnimalPenID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextAnimalPenID);
		}

		// Token: 0x06005FC3 RID: 24515 RVA: 0x0020B7EF File Offset: 0x002099EF
		public int GetNextTransporterGroupID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextTransporterGroupID);
		}

		// Token: 0x06005FC4 RID: 24516 RVA: 0x0020B7FC File Offset: 0x002099FC
		public int GetNextAncientCryptosleepCasketGroupID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextAncientCryptosleepCasketGroupID);
		}

		// Token: 0x06005FC5 RID: 24517 RVA: 0x0020B809 File Offset: 0x00209A09
		public int GetNextJobID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextJobID);
		}

		// Token: 0x06005FC6 RID: 24518 RVA: 0x0020B816 File Offset: 0x00209A16
		public int GetNextSignalTagID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextSignalTagID);
		}

		// Token: 0x06005FC7 RID: 24519 RVA: 0x0020B823 File Offset: 0x00209A23
		public int GetNextWorldFeatureID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextWorldFeatureID);
		}

		// Token: 0x06005FC8 RID: 24520 RVA: 0x0020B830 File Offset: 0x00209A30
		public int GetNextHediffID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextHediffID);
		}

		// Token: 0x06005FC9 RID: 24521 RVA: 0x0020B83D File Offset: 0x00209A3D
		public int GetNextBattleID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextBattleID);
		}

		// Token: 0x06005FCA RID: 24522 RVA: 0x0020B84A File Offset: 0x00209A4A
		public int GetNextLogID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextLogID);
		}

		// Token: 0x06005FCB RID: 24523 RVA: 0x0020B857 File Offset: 0x00209A57
		public int GetNextLetterID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextLetterID);
		}

		// Token: 0x06005FCC RID: 24524 RVA: 0x0020B864 File Offset: 0x00209A64
		public int GetNextArchivedDialogID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextArchivedDialogID);
		}

		// Token: 0x06005FCD RID: 24525 RVA: 0x0020B871 File Offset: 0x00209A71
		public int GetNextMessageID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextMessageID);
		}

		// Token: 0x06005FCE RID: 24526 RVA: 0x0020B87E File Offset: 0x00209A7E
		public int GetNextZoneID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextZoneID);
		}

		// Token: 0x06005FCF RID: 24527 RVA: 0x0020B88B File Offset: 0x00209A8B
		public int GetNextQuestID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextQuestID);
		}

		// Token: 0x06005FD0 RID: 24528 RVA: 0x0020B898 File Offset: 0x00209A98
		public int GetNextGameConditionID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextGameConditionID);
		}

		// Token: 0x06005FD1 RID: 24529 RVA: 0x0020B8A5 File Offset: 0x00209AA5
		public int GetNextIdeoID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextIdeoID);
		}

		// Token: 0x06005FD2 RID: 24530 RVA: 0x0020B8B2 File Offset: 0x00209AB2
		public int GetNextPreceptID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextPreceptID);
		}

		// Token: 0x06005FD3 RID: 24531 RVA: 0x0020B8BF File Offset: 0x00209ABF
		public int GetNextRitualObligationID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextRitualObligationID);
		}

		// Token: 0x06005FD4 RID: 24532 RVA: 0x0020B8CC File Offset: 0x00209ACC
		public int GetNextPresenceDemandID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextPresenceDemandID);
		}

		// Token: 0x06005FD5 RID: 24533 RVA: 0x0020B8D9 File Offset: 0x00209AD9
		public int GetNextTransportShipID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextTransportShipID);
		}

		// Token: 0x06005FD6 RID: 24534 RVA: 0x0020B8E6 File Offset: 0x00209AE6
		public int GetNextShipJobID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextShipJobID);
		}

		// Token: 0x06005FD7 RID: 24535 RVA: 0x0020B8F3 File Offset: 0x00209AF3
		public int GetNextRitualRoleID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextRitualRoleID);
		}

		// Token: 0x06005FD8 RID: 24536 RVA: 0x0020B900 File Offset: 0x00209B00
		public int GetNextAbilityID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextAbilityID);
		}

		// Token: 0x06005FD9 RID: 24537 RVA: 0x0020B90D File Offset: 0x00209B0D
		public UniqueIDsManager()
		{
			this.nextThingID = Rand.Range(0, 1000);
		}

		// Token: 0x06005FDA RID: 24538 RVA: 0x0020B928 File Offset: 0x00209B28
		private static int GetNextID(ref int nextID)
		{
			if (Scribe.mode == LoadSaveMode.LoadingVars && !Find.UniqueIDsManager.wasLoaded)
			{
				Log.Warning("Getting next unique ID during LoadingVars before UniqueIDsManager was loaded. Assigning a random value.");
				return Rand.Int;
			}
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				Log.Warning("Getting next unique ID during saving This may cause bugs.");
			}
			int result = nextID;
			nextID++;
			if (nextID == 2147483647)
			{
				Log.Warning("Next ID is at max value. Resetting to 0. This may cause bugs.");
				nextID = 0;
			}
			return result;
		}

		// Token: 0x06005FDB RID: 24539 RVA: 0x0020B98C File Offset: 0x00209B8C
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
			Scribe_Values.Look<int>(ref this.nextAnimalPenID, "nextAnimalPenID", 0, false);
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
			Scribe_Values.Look<int>(ref this.nextIdeoID, "nextIdeoID", 0, false);
			Scribe_Values.Look<int>(ref this.nextPreceptID, "nextPreceptID", 0, false);
			Scribe_Values.Look<int>(ref this.nextRitualObligationID, "nextRitualObligationID", 0, false);
			Scribe_Values.Look<int>(ref this.nextPresenceDemandID, "nextPresenceDemandID", 0, false);
			Scribe_Values.Look<int>(ref this.nextTransportShipID, "nextTransportShipID", 0, false);
			Scribe_Values.Look<int>(ref this.nextShipJobID, "nextShipJobID", 0, false);
			Scribe_Values.Look<int>(ref this.nextRitualRoleID, "nextRitualRoleID", 0, false);
			Scribe_Values.Look<int>(ref this.nextAbilityID, "nextAbilityID", 0, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.wasLoaded = true;
			}
		}

		// Token: 0x040036F7 RID: 14071
		private int nextThingID;

		// Token: 0x040036F8 RID: 14072
		private int nextBillID;

		// Token: 0x040036F9 RID: 14073
		private int nextFactionID;

		// Token: 0x040036FA RID: 14074
		private int nextLordID;

		// Token: 0x040036FB RID: 14075
		private int nextTaleID;

		// Token: 0x040036FC RID: 14076
		private int nextPassingShipID;

		// Token: 0x040036FD RID: 14077
		private int nextWorldObjectID;

		// Token: 0x040036FE RID: 14078
		private int nextMapID;

		// Token: 0x040036FF RID: 14079
		private int nextCaravanID;

		// Token: 0x04003700 RID: 14080
		private int nextAreaID;

		// Token: 0x04003701 RID: 14081
		private int nextAnimalPenID;

		// Token: 0x04003702 RID: 14082
		private int nextTransporterGroupID;

		// Token: 0x04003703 RID: 14083
		private int nextAncientCryptosleepCasketGroupID;

		// Token: 0x04003704 RID: 14084
		private int nextJobID;

		// Token: 0x04003705 RID: 14085
		private int nextSignalTagID;

		// Token: 0x04003706 RID: 14086
		private int nextWorldFeatureID;

		// Token: 0x04003707 RID: 14087
		private int nextHediffID;

		// Token: 0x04003708 RID: 14088
		private int nextBattleID;

		// Token: 0x04003709 RID: 14089
		private int nextLogID;

		// Token: 0x0400370A RID: 14090
		private int nextLetterID;

		// Token: 0x0400370B RID: 14091
		private int nextArchivedDialogID;

		// Token: 0x0400370C RID: 14092
		private int nextMessageID;

		// Token: 0x0400370D RID: 14093
		private int nextZoneID;

		// Token: 0x0400370E RID: 14094
		private int nextQuestID;

		// Token: 0x0400370F RID: 14095
		private int nextGameConditionID;

		// Token: 0x04003710 RID: 14096
		private int nextIdeoID;

		// Token: 0x04003711 RID: 14097
		private int nextPreceptID;

		// Token: 0x04003712 RID: 14098
		private int nextRitualObligationID;

		// Token: 0x04003713 RID: 14099
		private int nextPresenceDemandID;

		// Token: 0x04003714 RID: 14100
		private int nextTransportShipID;

		// Token: 0x04003715 RID: 14101
		private int nextShipJobID;

		// Token: 0x04003716 RID: 14102
		private int nextRitualRoleID;

		// Token: 0x04003717 RID: 14103
		private int nextAbilityID;

		// Token: 0x04003718 RID: 14104
		private bool wasLoaded;
	}
}
