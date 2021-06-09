using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001514 RID: 5396
	public class Pawn_Ownership : IExposable
	{
		// Token: 0x170011FB RID: 4603
		// (get) Token: 0x06007467 RID: 29799 RVA: 0x0004E868 File Offset: 0x0004CA68
		// (set) Token: 0x06007468 RID: 29800 RVA: 0x0004E870 File Offset: 0x0004CA70
		public Building_Bed OwnedBed
		{
			get
			{
				return this.intOwnedBed;
			}
			private set
			{
				if (this.intOwnedBed != value)
				{
					this.intOwnedBed = value;
					ThoughtUtility.RemovePositiveBedroomThoughts(this.pawn);
				}
			}
		}

		// Token: 0x170011FC RID: 4604
		// (get) Token: 0x06007469 RID: 29801 RVA: 0x0004E88D File Offset: 0x0004CA8D
		// (set) Token: 0x0600746A RID: 29802 RVA: 0x0004E895 File Offset: 0x0004CA95
		public Building_Grave AssignedGrave { get; private set; }

		// Token: 0x170011FD RID: 4605
		// (get) Token: 0x0600746B RID: 29803 RVA: 0x0004E89E File Offset: 0x0004CA9E
		// (set) Token: 0x0600746C RID: 29804 RVA: 0x0004E8A6 File Offset: 0x0004CAA6
		public Building_Throne AssignedThrone { get; private set; }

		// Token: 0x170011FE RID: 4606
		// (get) Token: 0x0600746D RID: 29805 RVA: 0x0004E8AF File Offset: 0x0004CAAF
		// (set) Token: 0x0600746E RID: 29806 RVA: 0x0004E8B7 File Offset: 0x0004CAB7
		public Building AssignedMeditationSpot { get; private set; }

		// Token: 0x170011FF RID: 4607
		// (get) Token: 0x0600746F RID: 29807 RVA: 0x00237020 File Offset: 0x00235220
		public Room OwnedRoom
		{
			get
			{
				if (this.OwnedBed == null)
				{
					return null;
				}
				Room room = this.OwnedBed.GetRoom(RegionType.Set_Passable);
				if (room == null)
				{
					return null;
				}
				if (room.Owners.Contains(this.pawn))
				{
					return room;
				}
				return null;
			}
		}

		// Token: 0x06007470 RID: 29808 RVA: 0x0004E8C0 File Offset: 0x0004CAC0
		public Pawn_Ownership(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06007471 RID: 29809 RVA: 0x00237060 File Offset: 0x00235260
		public void ExposeData()
		{
			Building_Grave assignedGrave = this.AssignedGrave;
			Building_Throne assignedThrone = this.AssignedThrone;
			Building assignedMeditationSpot = this.AssignedMeditationSpot;
			Scribe_References.Look<Building_Bed>(ref this.intOwnedBed, "ownedBed", false);
			Scribe_References.Look<Building>(ref assignedMeditationSpot, "assignedMeditationSpot", false);
			Scribe_References.Look<Building_Grave>(ref assignedGrave, "assignedGrave", false);
			Scribe_References.Look<Building_Throne>(ref assignedThrone, "assignedThrone", false);
			this.AssignedGrave = assignedGrave;
			this.AssignedThrone = assignedThrone;
			this.AssignedMeditationSpot = assignedMeditationSpot;
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.intOwnedBed != null)
				{
					CompAssignableToPawn compAssignableToPawn = this.intOwnedBed.TryGetComp<CompAssignableToPawn>();
					if (compAssignableToPawn != null && !compAssignableToPawn.AssignedPawns.Contains(this.pawn))
					{
						Building_Bed newBed = this.intOwnedBed;
						this.UnclaimBed();
						this.ClaimBedIfNonMedical(newBed);
					}
				}
				if (this.AssignedGrave != null)
				{
					CompAssignableToPawn compAssignableToPawn2 = this.AssignedGrave.TryGetComp<CompAssignableToPawn>();
					if (compAssignableToPawn2 != null && !compAssignableToPawn2.AssignedPawns.Contains(this.pawn))
					{
						Building_Grave assignedGrave2 = this.AssignedGrave;
						this.UnclaimGrave();
						this.ClaimGrave(assignedGrave2);
					}
				}
				if (this.AssignedThrone != null)
				{
					CompAssignableToPawn compAssignableToPawn3 = this.AssignedThrone.TryGetComp<CompAssignableToPawn>();
					if (compAssignableToPawn3 != null && !compAssignableToPawn3.AssignedPawns.Contains(this.pawn))
					{
						Building_Throne assignedThrone2 = this.AssignedThrone;
						this.UnclaimThrone();
						this.ClaimThrone(assignedThrone2);
					}
				}
			}
		}

		// Token: 0x06007472 RID: 29810 RVA: 0x002371A8 File Offset: 0x002353A8
		public bool ClaimBedIfNonMedical(Building_Bed newBed)
		{
			if (newBed.OwnersForReading.Contains(this.pawn) || newBed.Medical)
			{
				return false;
			}
			this.UnclaimBed();
			if (newBed.OwnersForReading.Count == newBed.SleepingSlotsCount)
			{
				newBed.OwnersForReading[newBed.OwnersForReading.Count - 1].ownership.UnclaimBed();
			}
			newBed.CompAssignableToPawn.ForceAddPawn(this.pawn);
			this.OwnedBed = newBed;
			if (newBed.Medical)
			{
				Log.Warning(this.pawn.LabelCap + " claimed medical bed.", false);
				this.UnclaimBed();
			}
			return true;
		}

		// Token: 0x06007473 RID: 29811 RVA: 0x0004E8CF File Offset: 0x0004CACF
		public bool UnclaimBed()
		{
			if (this.OwnedBed == null)
			{
				return false;
			}
			this.OwnedBed.CompAssignableToPawn.ForceRemovePawn(this.pawn);
			this.OwnedBed = null;
			return true;
		}

		// Token: 0x06007474 RID: 29812 RVA: 0x00237254 File Offset: 0x00235454
		public bool ClaimGrave(Building_Grave newGrave)
		{
			if (newGrave.AssignedPawn == this.pawn)
			{
				return false;
			}
			this.UnclaimGrave();
			if (newGrave.AssignedPawn != null)
			{
				newGrave.AssignedPawn.ownership.UnclaimGrave();
			}
			newGrave.CompAssignableToPawn.ForceAddPawn(this.pawn);
			newGrave.GetStoreSettings().Priority = StoragePriority.Critical;
			this.AssignedGrave = newGrave;
			return true;
		}

		// Token: 0x06007475 RID: 29813 RVA: 0x0004E8F9 File Offset: 0x0004CAF9
		public bool UnclaimGrave()
		{
			if (this.AssignedGrave == null)
			{
				return false;
			}
			this.AssignedGrave.CompAssignableToPawn.ForceRemovePawn(this.pawn);
			this.AssignedGrave.GetStoreSettings().Priority = StoragePriority.Important;
			this.AssignedGrave = null;
			return true;
		}

		// Token: 0x06007476 RID: 29814 RVA: 0x002372B8 File Offset: 0x002354B8
		public bool ClaimThrone(Building_Throne newThrone)
		{
			if (newThrone.AssignedPawn == this.pawn)
			{
				return false;
			}
			this.UnclaimThrone();
			if (newThrone.AssignedPawn != null)
			{
				newThrone.AssignedPawn.ownership.UnclaimThrone();
			}
			newThrone.CompAssignableToPawn.ForceAddPawn(this.pawn);
			this.AssignedThrone = newThrone;
			return true;
		}

		// Token: 0x06007477 RID: 29815 RVA: 0x0004E934 File Offset: 0x0004CB34
		public bool UnclaimThrone()
		{
			if (this.AssignedThrone == null)
			{
				return false;
			}
			this.AssignedThrone.CompAssignableToPawn.ForceRemovePawn(this.pawn);
			this.AssignedThrone = null;
			return true;
		}

		// Token: 0x06007478 RID: 29816 RVA: 0x00237310 File Offset: 0x00235510
		public bool ClaimMeditationSpot(Building newSpot)
		{
			if (newSpot.GetAssignedPawn() == this.pawn)
			{
				return false;
			}
			this.UnclaimMeditationSpot();
			if (newSpot.GetAssignedPawn() != null)
			{
				newSpot.GetAssignedPawn().ownership.UnclaimMeditationSpot();
			}
			newSpot.TryGetComp<CompAssignableToPawn>().ForceAddPawn(this.pawn);
			this.AssignedMeditationSpot = newSpot;
			return true;
		}

		// Token: 0x06007479 RID: 29817 RVA: 0x0004E95E File Offset: 0x0004CB5E
		public bool UnclaimMeditationSpot()
		{
			if (this.AssignedMeditationSpot == null)
			{
				return false;
			}
			this.AssignedMeditationSpot.TryGetComp<CompAssignableToPawn>().ForceRemovePawn(this.pawn);
			this.AssignedMeditationSpot = null;
			return true;
		}

		// Token: 0x0600747A RID: 29818 RVA: 0x0004E988 File Offset: 0x0004CB88
		public void UnclaimAll()
		{
			this.UnclaimBed();
			this.UnclaimGrave();
			this.UnclaimThrone();
		}

		// Token: 0x0600747B RID: 29819 RVA: 0x00237368 File Offset: 0x00235568
		public void Notify_ChangedGuestStatus()
		{
			if (this.OwnedBed != null && ((this.OwnedBed.ForPrisoners && !this.pawn.IsPrisoner) || (!this.OwnedBed.ForPrisoners && this.pawn.IsPrisoner)))
			{
				this.UnclaimBed();
			}
		}

		// Token: 0x04004CC5 RID: 19653
		private Pawn pawn;

		// Token: 0x04004CC6 RID: 19654
		private Building_Bed intOwnedBed;
	}
}
