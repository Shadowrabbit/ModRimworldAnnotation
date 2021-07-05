using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000A6F RID: 2671
	[StaticConstructorOnStartup]
	public sealed class ReservationManager : IExposable
	{
		// Token: 0x170009F1 RID: 2545
		// (get) Token: 0x06003FC0 RID: 16320 RVA: 0x0002FC2D File Offset: 0x0002DE2D
		public List<ReservationManager.Reservation> ReservationsReadOnly
		{
			get
			{
				return this.reservations;
			}
		}

		// Token: 0x06003FC1 RID: 16321 RVA: 0x0002FC35 File Offset: 0x0002DE35
		public ReservationManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x06003FC2 RID: 16322 RVA: 0x00180A68 File Offset: 0x0017EC68
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
						Log.Error("Loaded reservation with destroyed target: " + reservation + ". Deleting it...", false);
						this.reservations.Remove(reservation);
					}
					if (reservation.Claimant != null && reservation.Claimant.Destroyed)
					{
						Log.Error("Loaded reservation with destroyed claimant: " + reservation + ". Deleting it...", false);
						this.reservations.Remove(reservation);
					}
					if (reservation.Claimant == null)
					{
						Log.Error("Loaded reservation with null claimant: " + reservation + ". Deleting it...", false);
						this.reservations.Remove(reservation);
					}
					if (reservation.Job == null)
					{
						Log.Error("Loaded reservation with null job: " + reservation + ". Deleting it...", false);
						this.reservations.Remove(reservation);
					}
				}
			}
		}

		// Token: 0x06003FC3 RID: 16323 RVA: 0x00180B98 File Offset: 0x0017ED98
		public bool CanReserve(Pawn claimant, LocalTargetInfo target, int maxPawns = 1, int stackCount = -1, ReservationLayerDef layer = null, bool ignoreOtherReservations = false)
		{
			if (claimant == null)
			{
				Log.Error("CanReserve with null claimant", false);
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

		// Token: 0x06003FC4 RID: 16324 RVA: 0x00180D68 File Offset: 0x0017EF68
		public int CanReserveStack(Pawn claimant, LocalTargetInfo target, int maxPawns = 1, ReservationLayerDef layer = null, bool ignoreOtherReservations = false)
		{
			if (claimant == null)
			{
				Log.Error("CanReserve with null claimant", false);
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

		// Token: 0x06003FC5 RID: 16325 RVA: 0x00180EC4 File Offset: 0x0017F0C4
		public bool Reserve(Pawn claimant, Job job, LocalTargetInfo target, int maxPawns = 1, int stackCount = -1, ReservationLayerDef layer = null, bool errorOnFailed = true)
		{
			if (maxPawns > 1 && stackCount == -1)
			{
				Log.ErrorOnce("Reserving with maxPawns > 1 and stackCount = All; this will not have a useful effect (suppressing future warnings)", 83269, false);
			}
			if (job == null)
			{
				Log.Warning(claimant.ToStringSafe<Pawn>() + " tried to reserve thing " + target.ToStringSafe<LocalTargetInfo>() + " without a valid job", false);
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

		// Token: 0x06003FC6 RID: 16326 RVA: 0x001810C0 File Offset: 0x0017F2C0
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
				}), false);
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
				}), false);
				return;
			}
			this.reservations.Remove(reservation);
		}

		// Token: 0x06003FC7 RID: 16327 RVA: 0x001811A0 File Offset: 0x0017F3A0
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

		// Token: 0x06003FC8 RID: 16328 RVA: 0x001811F4 File Offset: 0x0017F3F4
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

		// Token: 0x06003FC9 RID: 16329 RVA: 0x00181250 File Offset: 0x0017F450
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

		// Token: 0x06003FCA RID: 16330 RVA: 0x0018129C File Offset: 0x0017F49C
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

		// Token: 0x06003FCB RID: 16331 RVA: 0x001812F4 File Offset: 0x0017F4F4
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

		// Token: 0x06003FCC RID: 16332 RVA: 0x0002FC4F File Offset: 0x0002DE4F
		public bool IsReservedAndRespected(LocalTargetInfo target, Pawn claimant)
		{
			return this.FirstRespectedReserver(target, claimant) != null;
		}

		// Token: 0x06003FCD RID: 16333 RVA: 0x00181350 File Offset: 0x0017F550
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

		// Token: 0x06003FCE RID: 16334 RVA: 0x001813B0 File Offset: 0x0017F5B0
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

		// Token: 0x06003FCF RID: 16335 RVA: 0x00181414 File Offset: 0x0017F614
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

		// Token: 0x06003FD0 RID: 16336 RVA: 0x0002FC5C File Offset: 0x0002DE5C
		public IEnumerable<Thing> AllReservedThings()
		{
			return from res in this.reservations
			select res.Target.Thing;
		}

		// Token: 0x06003FD1 RID: 16337 RVA: 0x00181530 File Offset: 0x0017F730
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

		// Token: 0x06003FD2 RID: 16338 RVA: 0x001815B8 File Offset: 0x0017F7B8
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

		// Token: 0x06003FD3 RID: 16339 RVA: 0x00181634 File Offset: 0x0017F834
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

		// Token: 0x06003FD4 RID: 16340 RVA: 0x001817D0 File Offset: 0x0017F9D0
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
			Log.Error(text3, false);
		}

		// Token: 0x04002C14 RID: 11284
		private Map map;

		// Token: 0x04002C15 RID: 11285
		private List<ReservationManager.Reservation> reservations = new List<ReservationManager.Reservation>();

		// Token: 0x04002C16 RID: 11286
		private static readonly Material DebugReservedThingIcon = MaterialPool.MatFrom("UI/Overlays/ReservedForWork", ShaderDatabase.Cutout);

		// Token: 0x04002C17 RID: 11287
		public const int StackCount_All = -1;

		// Token: 0x02000A70 RID: 2672
		public class Reservation : IExposable
		{
			// Token: 0x170009F2 RID: 2546
			// (get) Token: 0x06003FD6 RID: 16342 RVA: 0x0002FC9E File Offset: 0x0002DE9E
			public Pawn Claimant
			{
				get
				{
					return this.claimant;
				}
			}

			// Token: 0x170009F3 RID: 2547
			// (get) Token: 0x06003FD7 RID: 16343 RVA: 0x0002FCA6 File Offset: 0x0002DEA6
			public Job Job
			{
				get
				{
					return this.job;
				}
			}

			// Token: 0x170009F4 RID: 2548
			// (get) Token: 0x06003FD8 RID: 16344 RVA: 0x0002FCAE File Offset: 0x0002DEAE
			public LocalTargetInfo Target
			{
				get
				{
					return this.target;
				}
			}

			// Token: 0x170009F5 RID: 2549
			// (get) Token: 0x06003FD9 RID: 16345 RVA: 0x0002FCB6 File Offset: 0x0002DEB6
			public ReservationLayerDef Layer
			{
				get
				{
					return this.layer;
				}
			}

			// Token: 0x170009F6 RID: 2550
			// (get) Token: 0x06003FDA RID: 16346 RVA: 0x0002FCBE File Offset: 0x0002DEBE
			public int MaxPawns
			{
				get
				{
					return this.maxPawns;
				}
			}

			// Token: 0x170009F7 RID: 2551
			// (get) Token: 0x06003FDB RID: 16347 RVA: 0x0002FCC6 File Offset: 0x0002DEC6
			public int StackCount
			{
				get
				{
					return this.stackCount;
				}
			}

			// Token: 0x170009F8 RID: 2552
			// (get) Token: 0x06003FDC RID: 16348 RVA: 0x0002FCCE File Offset: 0x0002DECE
			public Faction Faction
			{
				get
				{
					return this.claimant.Faction;
				}
			}

			// Token: 0x06003FDD RID: 16349 RVA: 0x0002FCDB File Offset: 0x0002DEDB
			public Reservation()
			{
			}

			// Token: 0x06003FDE RID: 16350 RVA: 0x0002FCEA File Offset: 0x0002DEEA
			public Reservation(Pawn claimant, Job job, int maxPawns, int stackCount, LocalTargetInfo target, ReservationLayerDef layer)
			{
				this.claimant = claimant;
				this.job = job;
				this.maxPawns = maxPawns;
				this.stackCount = stackCount;
				this.target = target;
				this.layer = layer;
			}

			// Token: 0x06003FDF RID: 16351 RVA: 0x001819E4 File Offset: 0x0017FBE4
			public void ExposeData()
			{
				Scribe_References.Look<Pawn>(ref this.claimant, "claimant", false);
				Scribe_References.Look<Job>(ref this.job, "job", false);
				Scribe_TargetInfo.Look(ref this.target, "target");
				Scribe_Values.Look<int>(ref this.maxPawns, "maxPawns", 0, false);
				Scribe_Values.Look<int>(ref this.stackCount, "stackCount", 0, false);
				Scribe_Defs.Look<ReservationLayerDef>(ref this.layer, "layer");
			}

			// Token: 0x06003FE0 RID: 16352 RVA: 0x00181A58 File Offset: 0x0017FC58
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

			// Token: 0x04002C18 RID: 11288
			private Pawn claimant;

			// Token: 0x04002C19 RID: 11289
			private Job job;

			// Token: 0x04002C1A RID: 11290
			private LocalTargetInfo target;

			// Token: 0x04002C1B RID: 11291
			private ReservationLayerDef layer;

			// Token: 0x04002C1C RID: 11292
			private int maxPawns;

			// Token: 0x04002C1D RID: 11293
			private int stackCount = -1;
		}
	}
}
