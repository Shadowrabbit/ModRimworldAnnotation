using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E6F RID: 3695
	public class Pawn_Ownership : IExposable
	{
		// Token: 0x17000EDA RID: 3802
		// (get) Token: 0x060055DF RID: 21983 RVA: 0x001D1489 File Offset: 0x001CF689
		// (set) Token: 0x060055E0 RID: 21984 RVA: 0x001D1491 File Offset: 0x001CF691
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

		// Token: 0x17000EDB RID: 3803
		// (get) Token: 0x060055E1 RID: 21985 RVA: 0x001D14AE File Offset: 0x001CF6AE
		// (set) Token: 0x060055E2 RID: 21986 RVA: 0x001D14B6 File Offset: 0x001CF6B6
		public Building_Grave AssignedGrave { get; private set; }

		// Token: 0x17000EDC RID: 3804
		// (get) Token: 0x060055E3 RID: 21987 RVA: 0x001D14BF File Offset: 0x001CF6BF
		// (set) Token: 0x060055E4 RID: 21988 RVA: 0x001D14C7 File Offset: 0x001CF6C7
		public Building_Throne AssignedThrone { get; private set; }

		// Token: 0x17000EDD RID: 3805
		// (get) Token: 0x060055E5 RID: 21989 RVA: 0x001D14D0 File Offset: 0x001CF6D0
		// (set) Token: 0x060055E6 RID: 21990 RVA: 0x001D14D8 File Offset: 0x001CF6D8
		public Building AssignedMeditationSpot { get; private set; }

		// Token: 0x17000EDE RID: 3806
		// (get) Token: 0x060055E7 RID: 21991 RVA: 0x001D14E4 File Offset: 0x001CF6E4
		public Room OwnedRoom
		{
			get
			{
				if (this.OwnedBed == null)
				{
					return null;
				}
				Room room = this.OwnedBed.GetRoom(RegionType.Set_All);
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

		// Token: 0x060055E8 RID: 21992 RVA: 0x001D1524 File Offset: 0x001CF724
		public Pawn_Ownership(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060055E9 RID: 21993 RVA: 0x001D1534 File Offset: 0x001CF734
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

		// Token: 0x060055EA RID: 21994 RVA: 0x001D167C File Offset: 0x001CF87C
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
			if (newBed.CompAssignableToPawn.IdeoligionForbids(this.pawn))
			{
				Log.Error("Assigned " + this.pawn.GetUniqueLoadID() + " to a bed against their or occupants' ideo.");
			}
			if (newBed.Medical)
			{
				Log.Warning(this.pawn.LabelCap + " claimed medical bed.");
				this.UnclaimBed();
			}
			return true;
		}

		// Token: 0x060055EB RID: 21995 RVA: 0x001D1757 File Offset: 0x001CF957
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

		// Token: 0x060055EC RID: 21996 RVA: 0x001D1784 File Offset: 0x001CF984
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

		// Token: 0x060055ED RID: 21997 RVA: 0x001D17E6 File Offset: 0x001CF9E6
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

		// Token: 0x060055EE RID: 21998 RVA: 0x001D1824 File Offset: 0x001CFA24
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

		// Token: 0x060055EF RID: 21999 RVA: 0x001D187A File Offset: 0x001CFA7A
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

		// Token: 0x060055F0 RID: 22000 RVA: 0x001D18A4 File Offset: 0x001CFAA4
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

		// Token: 0x060055F1 RID: 22001 RVA: 0x001D18FA File Offset: 0x001CFAFA
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

		// Token: 0x060055F2 RID: 22002 RVA: 0x001D1924 File Offset: 0x001CFB24
		public void UnclaimAll()
		{
			this.UnclaimBed();
			this.UnclaimGrave();
			this.UnclaimThrone();
		}

		// Token: 0x060055F3 RID: 22003 RVA: 0x001D193C File Offset: 0x001CFB3C
		public void Notify_ChangedGuestStatus()
		{
			if (this.OwnedBed != null && ((this.OwnedBed.ForPrisoners && !this.pawn.IsPrisoner && !PawnUtility.IsBeingArrested(this.pawn)) || (!this.OwnedBed.ForPrisoners && this.pawn.IsPrisoner) || (this.OwnedBed.ForColonists && this.pawn.HostFaction == null)))
			{
				this.UnclaimBed();
			}
		}

		// Token: 0x040032C0 RID: 12992
		private Pawn pawn;

		// Token: 0x040032C1 RID: 12993
		private Building_Bed intOwnedBed;
	}
}
