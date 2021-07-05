using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x0200043E RID: 1086
	[StaticConstructorOnStartup]
	public sealed class PawnDestinationReservationManager : IExposable
	{
		// Token: 0x06001AF7 RID: 6903 RVA: 0x00018D13 File Offset: 0x00016F13
		public PawnDestinationReservationManager.PawnDestinationSet GetPawnDestinationSetFor(Faction faction)
		{
			if (!this.reservedDestinations.ContainsKey(faction))
			{
				this.reservedDestinations.Add(faction, new PawnDestinationReservationManager.PawnDestinationSet());
			}
			return this.reservedDestinations[faction];
		}

		// Token: 0x06001AF8 RID: 6904 RVA: 0x000E9AF0 File Offset: 0x000E7CF0
		public void Reserve(Pawn p, Job job, IntVec3 loc)
		{
			if (p.Faction == null)
			{
				return;
			}
			Pawn pawn;
			if (p.Drafted && p.Faction == Faction.OfPlayer && this.IsReserved(loc, out pawn) && pawn != p && !pawn.HostileTo(p) && pawn.Faction != p.Faction && (pawn.mindState == null || pawn.mindState.mentalStateHandler == null || !pawn.mindState.mentalStateHandler.InMentalState || (pawn.mindState.mentalStateHandler.CurStateDef.category != MentalStateCategory.Aggro && pawn.mindState.mentalStateHandler.CurStateDef.category != MentalStateCategory.Malicious)))
			{
				pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
			this.ObsoleteAllClaimedBy(p);
			this.GetPawnDestinationSetFor(p.Faction).list.Add(new PawnDestinationReservationManager.PawnDestinationReservation
			{
				target = loc,
				claimant = p,
				job = job
			});
		}

		// Token: 0x06001AF9 RID: 6905 RVA: 0x000E9BE8 File Offset: 0x000E7DE8
		public PawnDestinationReservationManager.PawnDestinationReservation MostRecentReservationFor(Pawn p)
		{
			if (p.Faction == null)
			{
				return null;
			}
			List<PawnDestinationReservationManager.PawnDestinationReservation> list = this.GetPawnDestinationSetFor(p.Faction).list;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].claimant == p && !list[i].obsolete)
				{
					return list[i];
				}
			}
			return null;
		}

		// Token: 0x06001AFA RID: 6906 RVA: 0x000E9C48 File Offset: 0x000E7E48
		public IntVec3 FirstObsoleteReservationFor(Pawn p)
		{
			if (p.Faction == null)
			{
				return IntVec3.Invalid;
			}
			List<PawnDestinationReservationManager.PawnDestinationReservation> list = this.GetPawnDestinationSetFor(p.Faction).list;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].claimant == p && list[i].obsolete)
				{
					return list[i].target;
				}
			}
			return IntVec3.Invalid;
		}

		// Token: 0x06001AFB RID: 6907 RVA: 0x000E9CB8 File Offset: 0x000E7EB8
		public Job FirstObsoleteReservationJobFor(Pawn p)
		{
			if (p.Faction == null)
			{
				return null;
			}
			List<PawnDestinationReservationManager.PawnDestinationReservation> list = this.GetPawnDestinationSetFor(p.Faction).list;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].claimant == p && list[i].obsolete)
				{
					return list[i].job;
				}
			}
			return null;
		}

		// Token: 0x06001AFC RID: 6908 RVA: 0x000E9D20 File Offset: 0x000E7F20
		public bool IsReserved(IntVec3 loc)
		{
			Pawn pawn;
			return this.IsReserved(loc, out pawn);
		}

		// Token: 0x06001AFD RID: 6909 RVA: 0x000E9D38 File Offset: 0x000E7F38
		public bool IsReserved(IntVec3 loc, out Pawn claimant)
		{
			foreach (KeyValuePair<Faction, PawnDestinationReservationManager.PawnDestinationSet> keyValuePair in this.reservedDestinations)
			{
				List<PawnDestinationReservationManager.PawnDestinationReservation> list = keyValuePair.Value.list;
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].target == loc)
					{
						claimant = list[i].claimant;
						return true;
					}
				}
			}
			claimant = null;
			return false;
		}

		// Token: 0x06001AFE RID: 6910 RVA: 0x000E9DD0 File Offset: 0x000E7FD0
		public bool CanReserve(IntVec3 c, Pawn searcher, bool draftedOnly = false)
		{
			if (searcher.Faction == null)
			{
				return true;
			}
			if (searcher.Faction == Faction.OfPlayer)
			{
				return this.CanReserveInt(c, searcher.Faction, searcher, draftedOnly);
			}
			foreach (Faction faction in Find.FactionManager.AllFactionsListForReading)
			{
				if (!faction.HostileTo(searcher.Faction) && !this.CanReserveInt(c, faction, searcher, draftedOnly))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001AFF RID: 6911 RVA: 0x000E9E68 File Offset: 0x000E8068
		private bool CanReserveInt(IntVec3 c, Faction faction, Pawn ignoreClaimant = null, bool draftedOnly = false)
		{
			if (faction == null)
			{
				return true;
			}
			List<PawnDestinationReservationManager.PawnDestinationReservation> list = this.GetPawnDestinationSetFor(faction).list;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].target == c && (ignoreClaimant == null || list[i].claimant != ignoreClaimant) && (!draftedOnly || list[i].claimant.Drafted))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001B00 RID: 6912 RVA: 0x000E9ED8 File Offset: 0x000E80D8
		public Pawn FirstReserverOf(IntVec3 c, Faction faction)
		{
			if (faction == null)
			{
				return null;
			}
			List<PawnDestinationReservationManager.PawnDestinationReservation> list = this.GetPawnDestinationSetFor(faction).list;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].target == c)
				{
					return list[i].claimant;
				}
			}
			return null;
		}

		// Token: 0x06001B01 RID: 6913 RVA: 0x000E9F2C File Offset: 0x000E812C
		public void ReleaseAllObsoleteClaimedBy(Pawn p)
		{
			if (p.Faction == null)
			{
				return;
			}
			List<PawnDestinationReservationManager.PawnDestinationReservation> list = this.GetPawnDestinationSetFor(p.Faction).list;
			int i = 0;
			while (i < list.Count)
			{
				if (list[i].claimant == p && list[i].obsolete)
				{
					list[i] = list[list.Count - 1];
					list.RemoveLast<PawnDestinationReservationManager.PawnDestinationReservation>();
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x06001B02 RID: 6914 RVA: 0x000E9FA0 File Offset: 0x000E81A0
		public void ReleaseAllClaimedBy(Pawn p)
		{
			if (p.Faction == null)
			{
				return;
			}
			List<PawnDestinationReservationManager.PawnDestinationReservation> list = this.GetPawnDestinationSetFor(p.Faction).list;
			int i = 0;
			while (i < list.Count)
			{
				if (list[i].claimant == p)
				{
					list[i] = list[list.Count - 1];
					list.RemoveLast<PawnDestinationReservationManager.PawnDestinationReservation>();
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x06001B03 RID: 6915 RVA: 0x000EA008 File Offset: 0x000E8208
		public void ReleaseClaimedBy(Pawn p, Job job)
		{
			if (p.Faction == null)
			{
				return;
			}
			List<PawnDestinationReservationManager.PawnDestinationReservation> list = this.GetPawnDestinationSetFor(p.Faction).list;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].claimant == p && list[i].job == job)
				{
					list[i].job = null;
					if (list[i].obsolete)
					{
						list[i] = list[list.Count - 1];
						list.RemoveLast<PawnDestinationReservationManager.PawnDestinationReservation>();
						i--;
					}
				}
			}
		}

		// Token: 0x06001B04 RID: 6916 RVA: 0x00018D40 File Offset: 0x00016F40
		public void Notify_FactionRemoved(Faction faction)
		{
			if (this.reservedDestinations.ContainsKey(faction))
			{
				this.reservedDestinations.Remove(faction);
			}
		}

		// Token: 0x06001B05 RID: 6917 RVA: 0x000EA09C File Offset: 0x000E829C
		public void ObsoleteAllClaimedBy(Pawn p)
		{
			if (p.Faction == null)
			{
				return;
			}
			List<PawnDestinationReservationManager.PawnDestinationReservation> list = this.GetPawnDestinationSetFor(p.Faction).list;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].claimant == p)
				{
					list[i].obsolete = true;
					if (list[i].job == null)
					{
						list[i] = list[list.Count - 1];
						list.RemoveLast<PawnDestinationReservationManager.PawnDestinationReservation>();
						i--;
					}
				}
			}
		}

		// Token: 0x06001B06 RID: 6918 RVA: 0x000EA120 File Offset: 0x000E8320
		public void DebugDrawDestinations()
		{
			foreach (PawnDestinationReservationManager.PawnDestinationReservation pawnDestinationReservation in this.GetPawnDestinationSetFor(Faction.OfPlayer).list)
			{
				if (!(pawnDestinationReservation.claimant.Position == pawnDestinationReservation.target))
				{
					IntVec3 target = pawnDestinationReservation.target;
					Vector3 s = new Vector3(1f, 1f, 1f);
					Matrix4x4 matrix = default(Matrix4x4);
					matrix.SetTRS(target.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays), Quaternion.identity, s);
					Graphics.DrawMesh(MeshPool.plane10, matrix, PawnDestinationReservationManager.DestinationMat, 0);
					if (Find.Selector.IsSelected(pawnDestinationReservation.claimant))
					{
						Graphics.DrawMesh(MeshPool.plane10, matrix, PawnDestinationReservationManager.DestinationSelectionMat, 0);
					}
				}
			}
		}

		// Token: 0x06001B07 RID: 6919 RVA: 0x000EA208 File Offset: 0x000E8408
		public void DebugDrawReservations()
		{
			foreach (KeyValuePair<Faction, PawnDestinationReservationManager.PawnDestinationSet> keyValuePair in this.reservedDestinations)
			{
				foreach (PawnDestinationReservationManager.PawnDestinationReservation pawnDestinationReservation in keyValuePair.Value.list)
				{
					IntVec3 target = pawnDestinationReservation.target;
					MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
					materialPropertyBlock.SetColor("_Color", keyValuePair.Key.Color);
					Vector3 s = new Vector3(1f, 1f, 1f);
					Matrix4x4 matrix = default(Matrix4x4);
					matrix.SetTRS(target.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays), Quaternion.identity, s);
					Graphics.DrawMesh(MeshPool.plane10, matrix, PawnDestinationReservationManager.DestinationMat, 0, Camera.main, 0, materialPropertyBlock);
					if (Find.Selector.IsSelected(pawnDestinationReservation.claimant))
					{
						Graphics.DrawMesh(MeshPool.plane10, matrix, PawnDestinationReservationManager.DestinationSelectionMat, 0);
					}
				}
			}
		}

		// Token: 0x06001B08 RID: 6920 RVA: 0x00018D5D File Offset: 0x00016F5D
		public void ExposeData()
		{
			Scribe_Collections.Look<Faction, PawnDestinationReservationManager.PawnDestinationSet>(ref this.reservedDestinations, "reservedDestinations", LookMode.Reference, LookMode.Deep, ref this.reservedDestinationsKeysWorkingList, ref this.reservedDestinationsValuesWorkingList);
		}

		// Token: 0x040013BD RID: 5053
		private Dictionary<Faction, PawnDestinationReservationManager.PawnDestinationSet> reservedDestinations = new Dictionary<Faction, PawnDestinationReservationManager.PawnDestinationSet>();

		// Token: 0x040013BE RID: 5054
		private static readonly Material DestinationMat = MaterialPool.MatFrom("UI/Overlays/ReservedDestination");

		// Token: 0x040013BF RID: 5055
		private static readonly Material DestinationSelectionMat = MaterialPool.MatFrom("UI/Overlays/ReservedDestinationSelection");

		// Token: 0x040013C0 RID: 5056
		private List<Faction> reservedDestinationsKeysWorkingList;

		// Token: 0x040013C1 RID: 5057
		private List<PawnDestinationReservationManager.PawnDestinationSet> reservedDestinationsValuesWorkingList;

		// Token: 0x0200043F RID: 1087
		public class PawnDestinationReservation : IExposable
		{
			// Token: 0x06001B0B RID: 6923 RVA: 0x000EA33C File Offset: 0x000E853C
			public void ExposeData()
			{
				Scribe_Values.Look<IntVec3>(ref this.target, "target", default(IntVec3), false);
				Scribe_References.Look<Pawn>(ref this.claimant, "claimant", false);
				Scribe_References.Look<Job>(ref this.job, "job", false);
				Scribe_Values.Look<bool>(ref this.obsolete, "obsolete", false, false);
			}

			// Token: 0x040013C2 RID: 5058
			public IntVec3 target;

			// Token: 0x040013C3 RID: 5059
			public Pawn claimant;

			// Token: 0x040013C4 RID: 5060
			public Job job;

			// Token: 0x040013C5 RID: 5061
			public bool obsolete;
		}

		// Token: 0x02000440 RID: 1088
		public class PawnDestinationSet : IExposable
		{
			// Token: 0x06001B0D RID: 6925 RVA: 0x000EA398 File Offset: 0x000E8598
			public void ExposeData()
			{
				Scribe_Collections.Look<PawnDestinationReservationManager.PawnDestinationReservation>(ref this.list, "list", LookMode.Deep, Array.Empty<object>());
				if (Scribe.mode == LoadSaveMode.PostLoadInit)
				{
					if (this.list.RemoveAll((PawnDestinationReservationManager.PawnDestinationReservation x) => x.claimant.DestroyedOrNull()) != 0)
					{
						Log.Warning("Some destination reservations had null or destroyed claimant.", false);
					}
				}
			}

			// Token: 0x040013C6 RID: 5062
			public List<PawnDestinationReservationManager.PawnDestinationReservation> list = new List<PawnDestinationReservationManager.PawnDestinationReservation>();
		}
	}
}
