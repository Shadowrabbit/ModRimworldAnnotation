using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000611 RID: 1553
	[StaticConstructorOnStartup]
	public sealed class ReservationManager : IExposable
	{
		// Token: 0x17000886 RID: 2182
		// (get) Token: 0x06002CE9 RID: 11497 RVA: 0x0010DD00 File Offset: 0x0010BF00
		public List<ReservationManager.Reservation> ReservationsReadOnly
		{
			get
			{
				return this.reservations;
			}
		}

		// Token: 0x06002CEA RID: 11498 RVA: 0x0010DD08 File Offset: 0x0010BF08
		public ReservationManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x06002CEB RID: 11499 RVA: 0x0010DD24 File Offset: 0x0010BF24
		public void ExposeData()
		{
			Scribe_Collections.Look<ReservationManager.Reservation>(ref this.reservations, "reservations", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				for (int i = this.reservations.Count - 1; i >= 0; i--)
				{
					ReservationManager.Reservation reservation = this.reservations[i];
					if (reservation.Target.Thing != null && reservation.Target.Thing.Destroyed)
					{
						Log.Error("Loaded reservation with destroyed target: " + reservation + ". Deleting it...");
						this.reservations.Remove(reservation);
					}
					if (reservation.Claimant != null && reservation.Claimant.Destroyed)
					{
						Log.Error("Loaded reservation with destroyed claimant: " + reservation + ". Deleting it...");
						this.reservations.Remove(reservation);
					}
					if (reservation.Claimant == null)
					{
						Log.Error("Loaded reservation with null claimant: " + reservation + ". Deleting it...");
						this.reservations.Remove(reservation);
					}
					if (reservation.Job == null)
					{
						Log.Error("Loaded reservation with null job: " + reservation + ". Deleting it...");
						this.reservations.Remove(reservation);
					}
				}
			}
		}

		// Token: 0x06002CEC RID: 11500 RVA: 0x0010DE50 File Offset: 0x0010C050
		public bool CanReserve(Pawn claimant, LocalTargetInfo target, int maxPawns = 1, int stackCount = -1, ReservationLayerDef layer = null, bool ignoreOtherReservations = false)
		{
			if (claimant == null)
			{
				Log.Error("CanReserve with null claimant");
				return false;
			}
			if (!claimant.Spawned || claimant.Map != this.map)
			{
				return false;
			}
			if (!target.IsValid || target.ThingDestroyed)
			{
				return false;
			}
			if (target.HasThing && target.Thing.SpawnedOrAnyParentSpawned && target.Thing.MapHeld != this.map)
			{
				return false;
			}
			int num = target.HasThing ? target.Thing.stackCount : 1;
			int num2 = (stackCount == -1) ? num : stackCount;
			if (num2 > num)
			{
				return false;
			}
			if (!ignoreOtherReservations)
			{
				if (this.map.physicalInteractionReservationManager.IsReserved(target) && !this.map.physicalInteractionReservationManager.IsReservedBy(claimant, target))
				{
					return false;
				}
				for (int i = 0; i < this.reservations.Count; i++)
				{
					ReservationManager.Reservation reservation = this.reservations[i];
					if (reservation.Target == target && reservation.Layer == layer && reservation.Claimant == claimant && (reservation.StackCount == -1 || reservation.StackCount >= num2))
					{
						return true;
					}
				}
				int num3 = 0;
				int num4 = 0;
				for (int j = 0; j < this.reservations.Count; j++)
				{
					ReservationManager.Reservation reservation2 = this.reservations[j];
					if (!(reservation2.Target != target) && reservation2.Layer == layer && reservation2.Claimant != claimant && ReservationManager.RespectsReservationsOf(claimant, reservation2.Claimant))
					{
						if (reservation2.MaxPawns != maxPawns)
						{
							return false;
						}
						num3++;
						if (reservation2.StackCount == -1)
						{
							num4 += num;
						}
						else
						{
							num4 += reservation2.StackCount;
						}
						if (num3 >= maxPawns || num2 + num4 > num)
						{
							return false;
						}
					}
				}
			}
			return true;
		}

		// Token: 0x06002CED RID: 11501 RVA: 0x0010E01C File Offset: 0x0010C21C
		public int CanReserveStack(Pawn claimant, LocalTargetInfo target, int maxPawns = 1, ReservationLayerDef layer = null, bool ignoreOtherReservations = false)
		{
			if (claimant == null)
			{
				Log.Error("CanReserve with null claimant");
				return 0;
			}
			if (!claimant.Spawned || claimant.Map != this.map)
			{
				return 0;
			}
			if (!target.IsValid || target.ThingDestroyed)
			{
				return 0;
			}
			if (target.HasThing && target.Thing.SpawnedOrAnyParentSpawned && target.Thing.MapHeld != this.map)
			{
				return 0;
			}
			int num = target.HasThing ? target.Thing.stackCount : 1;
			int num2 = 0;
			if (!ignoreOtherReservations)
			{
				if (this.map.physicalInteractionReservationManager.IsReserved(target) && !this.map.physicalInteractionReservationManager.IsReservedBy(claimant, target))
				{
					return 0;
				}
				int num3 = 0;
				for (int i = 0; i < this.reservations.Count; i++)
				{
					ReservationManager.Reservation reservation = this.reservations[i];
					if (!(reservation.Target != target) && reservation.Layer == layer && reservation.Claimant != claimant && ReservationManager.RespectsReservationsOf(claimant, reservation.Claimant))
					{
						if (reservation.MaxPawns != maxPawns)
						{
							return 0;
						}
						num3++;
						if (reservation.StackCount == -1)
						{
							num2 += num;
						}
						else
						{
							num2 += reservation.StackCount;
						}
						if (num3 >= maxPawns || num2 >= num)
						{
							return 0;
						}
					}
				}
			}
			return Mathf.Max(num - num2, 0);
		}

		// Token: 0x06002CEE RID: 11502 RVA: 0x0010E178 File Offset: 0x0010C378
		public bool Reserve(Pawn claimant, Job job, LocalTargetInfo target, int maxPawns = 1, int stackCount = -1, ReservationLayerDef layer = null, bool errorOnFailed = true)
		{
			if (maxPawns > 1 && stackCount == -1)
			{
				Log.ErrorOnce("Reserving with maxPawns > 1 and stackCount = All; this will not have a useful effect (suppressing future warnings)", 83269);
			}
			if (job == null)
			{
				Log.Warning(claimant.ToStringSafe<Pawn>() + " tried to reserve thing " + target.ToStringSafe<LocalTargetInfo>() + " without a valid job");
				return false;
			}
			int num = target.HasThing ? target.Thing.stackCount : 1;
			int num2 = (stackCount == -1) ? num : stackCount;
			for (int i = 0; i < this.reservations.Count; i++)
			{
				ReservationManager.Reservation reservation = this.reservations[i];
				if (reservation.Target == target && reservation.Claimant == claimant && reservation.Job == job && reservation.Layer == layer && (reservation.StackCount == -1 || reservation.StackCount >= num2))
				{
					return true;
				}
			}
			if (!target.IsValid || target.ThingDestroyed)
			{
				return false;
			}
			if (this.CanReserve(claimant, target, maxPawns, stackCount, layer, false))
			{
				this.reservations.Add(new ReservationManager.Reservation(claimant, job, maxPawns, stackCount, target, layer));
				return true;
			}
			if (job != null && job.playerForced && this.CanReserve(claimant, target, maxPawns, stackCount, layer, true))
			{
				this.reservations.Add(new ReservationManager.Reservation(claimant, job, maxPawns, stackCount, target, layer));
				foreach (ReservationManager.Reservation reservation2 in this.reservations.ToList<ReservationManager.Reservation>())
				{
					if (reservation2.Target == target && reservation2.Claimant != claimant && reservation2.Layer == layer && ReservationManager.RespectsReservationsOf(claimant, reservation2.Claimant))
					{
						reservation2.Claimant.jobs.EndCurrentOrQueuedJob(reservation2.Job, JobCondition.InterruptForced, true);
					}
				}
				return true;
			}
			if (errorOnFailed)
			{
				this.LogCouldNotReserveError(claimant, job, target, maxPawns, stackCount, layer);
			}
			return false;
		}

		// Token: 0x06002CEF RID: 11503 RVA: 0x0010E374 File Offset: 0x0010C574
		public void Release(LocalTargetInfo target, Pawn claimant, Job job)
		{
			if (target.ThingDestroyed)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Releasing destroyed thing ",
					target,
					" for ",
					claimant
				}));
			}
			ReservationManager.Reservation reservation = null;
			for (int i = 0; i < this.reservations.Count; i++)
			{
				ReservationManager.Reservation reservation2 = this.reservations[i];
				if (reservation2.Target == target && reservation2.Claimant == claimant && reservation2.Job == job)
				{
					reservation = reservation2;
					break;
				}
			}
			if (reservation == null && !target.ThingDestroyed)
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to release ",
					target,
					" that wasn't reserved by ",
					claimant,
					"."
				}));
				return;
			}
			this.reservations.Remove(reservation);
		}

		// Token: 0x06002CF0 RID: 11504 RVA: 0x0010E450 File Offset: 0x0010C650
		public void ReleaseAllForTarget(Thing t)
		{
			if (t == null)
			{
				return;
			}
			for (int i = this.reservations.Count - 1; i >= 0; i--)
			{
				if (this.reservations[i].Target.Thing == t)
				{
					this.reservations.RemoveAt(i);
				}
			}
		}

		// Token: 0x06002CF1 RID: 11505 RVA: 0x0010E4A4 File Offset: 0x0010C6A4
		public void ReleaseClaimedBy(Pawn claimant, Job job)
		{
			for (int i = this.reservations.Count - 1; i >= 0; i--)
			{
				if (this.reservations[i].Claimant == claimant && this.reservations[i].Job == job)
				{
					this.reservations.RemoveAt(i);
				}
			}
		}

		// Token: 0x06002CF2 RID: 11506 RVA: 0x0010E500 File Offset: 0x0010C700
		public void ReleaseAllClaimedBy(Pawn claimant)
		{
			if (claimant == null)
			{
				return;
			}
			for (int i = this.reservations.Count - 1; i >= 0; i--)
			{
				if (this.reservations[i].Claimant == claimant)
				{
					this.reservations.RemoveAt(i);
				}
			}
		}

		// Token: 0x06002CF3 RID: 11507 RVA: 0x0010E54C File Offset: 0x0010C74C
		public LocalTargetInfo FirstReservationFor(Pawn claimant)
		{
			if (claimant == null)
			{
				return LocalTargetInfo.Invalid;
			}
			for (int i = 0; i < this.reservations.Count; i++)
			{
				if (this.reservations[i].Claimant == claimant)
				{
					return this.reservations[i].Target;
				}
			}
			return LocalTargetInfo.Invalid;
		}

		// Token: 0x06002CF4 RID: 11508 RVA: 0x0010E5A4 File Offset: 0x0010C7A4
		public bool IsReservedByAnyoneOf(LocalTargetInfo target, Faction faction)
		{
			if (!target.IsValid)
			{
				return false;
			}
			for (int i = 0; i < this.reservations.Count; i++)
			{
				ReservationManager.Reservation reservation = this.reservations[i];
				if (reservation.Target == target && reservation.Claimant.Faction == faction)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002CF5 RID: 11509 RVA: 0x0010E5FE File Offset: 0x0010C7FE
		public bool IsReservedAndRespected(LocalTargetInfo target, Pawn claimant)
		{
			return this.FirstRespectedReserver(target, claimant) != null;
		}

		// Token: 0x06002CF6 RID: 11510 RVA: 0x0010E60C File Offset: 0x0010C80C
		public Pawn FirstRespectedReserver(LocalTargetInfo target, Pawn claimant)
		{
			if (!target.IsValid)
			{
				return null;
			}
			for (int i = 0; i < this.reservations.Count; i++)
			{
				ReservationManager.Reservation reservation = this.reservations[i];
				if (reservation.Target == target && ReservationManager.RespectsReservationsOf(claimant, reservation.Claimant))
				{
					return reservation.Claimant;
				}
			}
			return null;
		}

		// Token: 0x06002CF7 RID: 11511 RVA: 0x0010E66C File Offset: 0x0010C86C
		public bool ReservedBy(LocalTargetInfo target, Pawn claimant, Job job = null)
		{
			if (!target.IsValid)
			{
				return false;
			}
			for (int i = 0; i < this.reservations.Count; i++)
			{
				ReservationManager.Reservation reservation = this.reservations[i];
				if (reservation.Target == target && reservation.Claimant == claimant && (job == null || reservation.Job == job))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002CF8 RID: 11512 RVA: 0x0010E6D0 File Offset: 0x0010C8D0
		public bool ReservedBy<TDriver>(LocalTargetInfo target, Pawn claimant, LocalTargetInfo? targetAIsNot = null, LocalTargetInfo? targetBIsNot = null, LocalTargetInfo? targetCIsNot = null)
		{
			if (!target.IsValid)
			{
				return false;
			}
			for (int i = 0; i < this.reservations.Count; i++)
			{
				ReservationManager.Reservation reservation = this.reservations[i];
				if (reservation.Target == target && reservation.Claimant == claimant && reservation.Job != null && reservation.Job.GetCachedDriver(claimant) is TDriver && (targetAIsNot == null || reservation.Job.targetA != targetAIsNot) && (targetBIsNot == null || reservation.Job.targetB != targetBIsNot) && (targetCIsNot == null || reservation.Job.targetC != targetCIsNot))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002CF9 RID: 11513 RVA: 0x0010E7EA File Offset: 0x0010C9EA
		public IEnumerable<Thing> AllReservedThings()
		{
			return from res in this.reservations
			select res.Target.Thing;
		}

		// Token: 0x06002CFA RID: 11514 RVA: 0x0010E818 File Offset: 0x0010CA18
		private static bool RespectsReservationsOf(Pawn newClaimant, Pawn oldClaimant)
		{
			if (newClaimant == oldClaimant)
			{
				return true;
			}
			if (newClaimant.Faction == null || oldClaimant.Faction == null)
			{
				return false;
			}
			if (newClaimant.Faction == oldClaimant.Faction)
			{
				return true;
			}
			if (!newClaimant.Faction.HostileTo(oldClaimant.Faction))
			{
				return true;
			}
			if (oldClaimant.HostFaction != null && oldClaimant.HostFaction == newClaimant.HostFaction)
			{
				return true;
			}
			if (newClaimant.HostFaction != null)
			{
				if (oldClaimant.HostFaction != null)
				{
					return true;
				}
				if (newClaimant.HostFaction == oldClaimant.Faction)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002CFB RID: 11515 RVA: 0x0010E8A0 File Offset: 0x0010CAA0
		internal string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("All reservation in ReservationManager:");
			for (int i = 0; i < this.reservations.Count; i++)
			{
				stringBuilder.AppendLine(string.Concat(new object[]
				{
					"[",
					i,
					"] ",
					this.reservations[i].ToString()
				}));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002CFC RID: 11516 RVA: 0x0010E91C File Offset: 0x0010CB1C
		internal void DebugDrawReservations()
		{
			for (int i = 0; i < this.reservations.Count; i++)
			{
				ReservationManager.Reservation reservation = this.reservations[i];
				if (reservation.Target.Thing != null)
				{
					if (reservation.Target.Thing.Spawned)
					{
						Thing thing = reservation.Target.Thing;
						Vector3 s = new Vector3((float)thing.RotatedSize.x, 1f, (float)thing.RotatedSize.z);
						Matrix4x4 matrix = default(Matrix4x4);
						matrix.SetTRS(thing.DrawPos + Vector3.up * 0.1f, Quaternion.identity, s);
						Graphics.DrawMesh(MeshPool.plane10, matrix, ReservationManager.DebugReservedThingIcon, 0);
						GenDraw.DrawLineBetween(reservation.Claimant.DrawPos, reservation.Target.Thing.DrawPos);
					}
					else
					{
						Graphics.DrawMesh(MeshPool.plane03, reservation.Claimant.DrawPos + Vector3.up + new Vector3(0.5f, 0f, 0.5f), Quaternion.identity, ReservationManager.DebugReservedThingIcon, 0);
					}
				}
				else
				{
					Graphics.DrawMesh(MeshPool.plane10, reservation.Target.Cell.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays), Quaternion.identity, ReservationManager.DebugReservedThingIcon, 0);
					GenDraw.DrawLineBetween(reservation.Claimant.DrawPos, reservation.Target.Cell.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays));
				}
			}
		}

		// Token: 0x06002CFD RID: 11517 RVA: 0x0010EAB8 File Offset: 0x0010CCB8
		private void LogCouldNotReserveError(Pawn claimant, Job job, LocalTargetInfo target, int maxPawns, int stackCount, ReservationLayerDef layer)
		{
			Job curJob = claimant.CurJob;
			string text = "null";
			int num = -1;
			if (curJob != null)
			{
				text = curJob.ToString();
				if (claimant.jobs.curDriver != null)
				{
					num = claimant.jobs.curDriver.CurToilIndex;
				}
			}
			string text2;
			if (target.HasThing && target.Thing.def.stackLimit != 1)
			{
				text2 = "(current stack count: " + target.Thing.stackCount + ")";
			}
			else
			{
				text2 = "";
			}
			string text3 = string.Concat(new object[]
			{
				"Could not reserve ",
				target.ToStringSafe<LocalTargetInfo>(),
				text2,
				" (layer: ",
				layer.ToStringSafe<ReservationLayerDef>(),
				") for ",
				claimant.ToStringSafe<Pawn>(),
				" for job ",
				job.ToStringSafe<Job>(),
				" (now doing job ",
				text,
				"(curToil=",
				num,
				")) for maxPawns ",
				maxPawns,
				" and stackCount ",
				stackCount,
				"."
			});
			Pawn pawn = this.FirstRespectedReserver(target, claimant);
			if (pawn != null)
			{
				string text4 = "null";
				int num2 = -1;
				Job curJob2 = pawn.CurJob;
				if (curJob2 != null)
				{
					text4 = curJob2.ToStringSafe<Job>();
					if (pawn.jobs.curDriver != null)
					{
						num2 = pawn.jobs.curDriver.CurToilIndex;
					}
				}
				text3 = string.Concat(new object[]
				{
					text3,
					" Existing reserver: ",
					pawn.ToStringSafe<Pawn>(),
					" doing job ",
					text4,
					" (toilIndex=",
					num2,
					")"
				});
			}
			else
			{
				text3 += " No existing reserver.";
			}
			Pawn pawn2 = this.map.physicalInteractionReservationManager.FirstReserverOf(target);
			if (pawn2 != null)
			{
				text3 = text3 + " Physical interaction reserver: " + pawn2.ToStringSafe<Pawn>();
			}
			Log.Error(text3);
		}

		// Token: 0x04001BA5 RID: 7077
		private Map map;

		// Token: 0x04001BA6 RID: 7078
		private List<ReservationManager.Reservation> reservations = new List<ReservationManager.Reservation>();

		// Token: 0x04001BA7 RID: 7079
		private static readonly Material DebugReservedThingIcon = MaterialPool.MatFrom("UI/Overlays/ReservedForWork", ShaderDatabase.Cutout);

		// Token: 0x04001BA8 RID: 7080
		public const int StackCount_All = -1;

		// Token: 0x02001DBA RID: 7610
		public class Reservation : IExposable
		{
			// Token: 0x17001A7E RID: 6782
			// (get) Token: 0x0600AB90 RID: 43920 RVA: 0x00391604 File Offset: 0x0038F804
			public Pawn Claimant
			{
				get
				{
					return this.claimant;
				}
			}

			// Token: 0x17001A7F RID: 6783
			// (get) Token: 0x0600AB91 RID: 43921 RVA: 0x0039160C File Offset: 0x0038F80C
			public Job Job
			{
				get
				{
					return this.job;
				}
			}

			// Token: 0x17001A80 RID: 6784
			// (get) Token: 0x0600AB92 RID: 43922 RVA: 0x00391614 File Offset: 0x0038F814
			public LocalTargetInfo Target
			{
				get
				{
					return this.target;
				}
			}

			// Token: 0x17001A81 RID: 6785
			// (get) Token: 0x0600AB93 RID: 43923 RVA: 0x0039161C File Offset: 0x0038F81C
			public ReservationLayerDef Layer
			{
				get
				{
					return this.layer;
				}
			}

			// Token: 0x17001A82 RID: 6786
			// (get) Token: 0x0600AB94 RID: 43924 RVA: 0x00391624 File Offset: 0x0038F824
			public int MaxPawns
			{
				get
				{
					return this.maxPawns;
				}
			}

			// Token: 0x17001A83 RID: 6787
			// (get) Token: 0x0600AB95 RID: 43925 RVA: 0x0039162C File Offset: 0x0038F82C
			public int StackCount
			{
				get
				{
					return this.stackCount;
				}
			}

			// Token: 0x17001A84 RID: 6788
			// (get) Token: 0x0600AB96 RID: 43926 RVA: 0x00391634 File Offset: 0x0038F834
			public Faction Faction
			{
				get
				{
					return this.claimant.Faction;
				}
			}

			// Token: 0x0600AB97 RID: 43927 RVA: 0x00391641 File Offset: 0x0038F841
			public Reservation()
			{
			}

			// Token: 0x0600AB98 RID: 43928 RVA: 0x00391650 File Offset: 0x0038F850
			public Reservation(Pawn claimant, Job job, int maxPawns, int stackCount, LocalTargetInfo target, ReservationLayerDef layer)
			{
				this.claimant = claimant;
				this.job = job;
				this.maxPawns = maxPawns;
				this.stackCount = stackCount;
				this.target = target;
				this.layer = layer;
			}

			// Token: 0x0600AB99 RID: 43929 RVA: 0x0039168C File Offset: 0x0038F88C
			public void ExposeData()
			{
				Scribe_References.Look<Pawn>(ref this.claimant, "claimant", false);
				Scribe_References.Look<Job>(ref this.job, "job", false);
				Scribe_TargetInfo.Look(ref this.target, "target");
				Scribe_Values.Look<int>(ref this.maxPawns, "maxPawns", 0, false);
				Scribe_Values.Look<int>(ref this.stackCount, "stackCount", 0, false);
				Scribe_Defs.Look<ReservationLayerDef>(ref this.layer, "layer");
			}

			// Token: 0x0600AB9A RID: 43930 RVA: 0x00391700 File Offset: 0x0038F900
			public override string ToString()
			{
				return string.Concat(new object[]
				{
					(this.claimant != null) ? this.claimant.LabelShort : "null",
					":",
					this.job.ToStringSafe<Job>(),
					", ",
					this.target.ToStringSafe<LocalTargetInfo>(),
					", ",
					this.layer.ToStringSafe<ReservationLayerDef>(),
					", ",
					this.maxPawns,
					", ",
					this.stackCount
				});
			}

			// Token: 0x04007234 RID: 29236
			private Pawn claimant;

			// Token: 0x04007235 RID: 29237
			private Job job;

			// Token: 0x04007236 RID: 29238
			private LocalTargetInfo target;

			// Token: 0x04007237 RID: 29239
			private ReservationLayerDef layer;

			// Token: 0x04007238 RID: 29240
			private int maxPawns;

			// Token: 0x04007239 RID: 29241
			private int stackCount = -1;
		}
	}
}
