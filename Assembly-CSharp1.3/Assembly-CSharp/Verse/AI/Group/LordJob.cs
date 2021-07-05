using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x02000661 RID: 1633
	public abstract class LordJob : IExposable
	{
		// Token: 0x170008A7 RID: 2215
		// (get) Token: 0x06002E50 RID: 11856 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool LostImportantReferenceDuringLoading
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008A8 RID: 2216
		// (get) Token: 0x06002E51 RID: 11857 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool AllowStartNewGatherings
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170008A9 RID: 2217
		// (get) Token: 0x06002E52 RID: 11858 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool NeverInRestraints
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008AA RID: 2218
		// (get) Token: 0x06002E53 RID: 11859 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool GuiltyOnDowned
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008AB RID: 2219
		// (get) Token: 0x06002E54 RID: 11860 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool CanBlockHostileVisitors
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170008AC RID: 2220
		// (get) Token: 0x06002E55 RID: 11861 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool AddFleeToil
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170008AD RID: 2221
		// (get) Token: 0x06002E56 RID: 11862 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool OrganizerIsStartingPawn
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008AE RID: 2222
		// (get) Token: 0x06002E57 RID: 11863 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool KeepExistingWhileHasAnyBuilding
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008AF RID: 2223
		// (get) Token: 0x06002E58 RID: 11864 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool AlwaysShowWeapon
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008B0 RID: 2224
		// (get) Token: 0x06002E59 RID: 11865 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool IsCaravanSendable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008B1 RID: 2225
		// (get) Token: 0x06002E5A RID: 11866 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool ManagesRopableAnimals
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008B2 RID: 2226
		// (get) Token: 0x06002E5B RID: 11867 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool RemoveDownedPawns
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170008B3 RID: 2227
		// (get) Token: 0x06002E5C RID: 11868 RVA: 0x00116064 File Offset: 0x00114264
		public Map Map
		{
			get
			{
				return this.lord.lordManager.map;
			}
		}

		// Token: 0x06002E5D RID: 11869
		public abstract StateGraph CreateGraph();

		// Token: 0x06002E5E RID: 11870 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void LordJobTick()
		{
		}

		// Token: 0x06002E5F RID: 11871 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void ExposeData()
		{
		}

		// Token: 0x06002E60 RID: 11872 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Cleanup()
		{
		}

		// Token: 0x06002E61 RID: 11873 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PostCleanup()
		{
		}

		// Token: 0x06002E62 RID: 11874 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_AddedToLord()
		{
		}

		// Token: 0x06002E63 RID: 11875 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_PawnAdded(Pawn p)
		{
		}

		// Token: 0x06002E64 RID: 11876 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_PawnLost(Pawn p, PawnLostCondition condition)
		{
		}

		// Token: 0x06002E65 RID: 11877 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_InMentalState(Pawn pawn, MentalStateDef stateDef)
		{
		}

		// Token: 0x06002E66 RID: 11878 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_BuildingAdded(Building b)
		{
		}

		// Token: 0x06002E67 RID: 11879 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_BuildingLost(Building b)
		{
		}

		// Token: 0x06002E68 RID: 11880 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_LordDestroyed()
		{
		}

		// Token: 0x06002E69 RID: 11881 RVA: 0x00002688 File Offset: 0x00000888
		public virtual string GetReport(Pawn pawn)
		{
			return null;
		}

		// Token: 0x06002E6A RID: 11882 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool CanOpenAnyDoor(Pawn p)
		{
			return false;
		}

		// Token: 0x06002E6B RID: 11883 RVA: 0x00116076 File Offset: 0x00114276
		public virtual IEnumerable<Gizmo> GetPawnGizmos(Pawn p)
		{
			yield break;
		}

		// Token: 0x06002E6C RID: 11884 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool ValidateAttackTarget(Pawn searcher, Thing target)
		{
			return true;
		}

		// Token: 0x04001C83 RID: 7299
		public Lord lord;
	}
}
