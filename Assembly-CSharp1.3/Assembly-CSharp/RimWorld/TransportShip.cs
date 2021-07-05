using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FDE RID: 4062
	public class TransportShip : IExposable, ILoadReferenceable
	{
		// Token: 0x17001066 RID: 4198
		// (get) Token: 0x06005F96 RID: 24470 RVA: 0x0020AF64 File Offset: 0x00209164
		public bool ShipExistsAndIsSpawned
		{
			get
			{
				return this.shipThing != null && this.shipThing.Spawned;
			}
		}

		// Token: 0x17001067 RID: 4199
		// (get) Token: 0x06005F97 RID: 24471 RVA: 0x0020AF7B File Offset: 0x0020917B
		public bool Disposed
		{
			get
			{
				return this.disposed;
			}
		}

		// Token: 0x17001068 RID: 4200
		// (get) Token: 0x06005F98 RID: 24472 RVA: 0x0020AF83 File Offset: 0x00209183
		public bool Waiting
		{
			get
			{
				return this.ShipExistsAndIsSpawned && (this.curJob == null || typeof(ShipJob_Wait).IsAssignableFrom(this.curJob.GetType()));
			}
		}

		// Token: 0x17001069 RID: 4201
		// (get) Token: 0x06005F99 RID: 24473 RVA: 0x0020AFB3 File Offset: 0x002091B3
		public bool ShowGizmos
		{
			get
			{
				return this.curJob != null && this.curJob.ShowGizmos;
			}
		}

		// Token: 0x1700106A RID: 4202
		// (get) Token: 0x06005F9A RID: 24474 RVA: 0x0020AFCA File Offset: 0x002091CA
		private bool CanDispose
		{
			get
			{
				return this.started && this.curJob == null && !this.shipJobs.Any<ShipJob>() && !this.shipThing.Spawned;
			}
		}

		// Token: 0x1700106B RID: 4203
		// (get) Token: 0x06005F9B RID: 24475 RVA: 0x0020AFF9 File Offset: 0x002091F9
		public CompTransporter TransporterComp
		{
			get
			{
				if (this.cachedCompTransporter == null)
				{
					this.cachedCompTransporter = this.shipThing.TryGetComp<CompTransporter>();
				}
				return this.cachedCompTransporter;
			}
		}

		// Token: 0x1700106C RID: 4204
		// (get) Token: 0x06005F9C RID: 24476 RVA: 0x0020B01A File Offset: 0x0020921A
		public CompShuttle ShuttleComp
		{
			get
			{
				if (this.cachedCompShuttle == null)
				{
					this.cachedCompShuttle = this.shipThing.TryGetComp<CompShuttle>();
				}
				return this.cachedCompShuttle;
			}
		}

		// Token: 0x1700106D RID: 4205
		// (get) Token: 0x06005F9D RID: 24477 RVA: 0x0020B03C File Offset: 0x0020923C
		public bool LeavingSoonAutomatically
		{
			get
			{
				if (this.curJob != null && this.curJob.def == ShipJobDefOf.FlyAway)
				{
					return true;
				}
				foreach (ShipJob shipJob in this.shipJobs)
				{
					if (shipJob.Interruptible)
					{
						return false;
					}
					if (shipJob.def == ShipJobDefOf.FlyAway)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x1700106E RID: 4206
		// (get) Token: 0x06005F9E RID: 24478 RVA: 0x0020B0C4 File Offset: 0x002092C4
		public bool HasPredeterminedDestination
		{
			get
			{
				if (this.curJob != null && this.curJob.HasDestination)
				{
					return true;
				}
				using (List<ShipJob>.Enumerator enumerator = this.shipJobs.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.HasDestination)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x06005F9F RID: 24479 RVA: 0x0020B134 File Offset: 0x00209334
		public TransportShip()
		{
		}

		// Token: 0x06005FA0 RID: 24480 RVA: 0x0020B15C File Offset: 0x0020935C
		public TransportShip(TransportShipDef def)
		{
			this.def = def;
			this.loadID = Find.UniqueIDsManager.GetNextTransportShipID();
			Find.TransportShipManager.RegisterShipObject(this);
		}

		// Token: 0x06005FA1 RID: 24481 RVA: 0x0020B1AE File Offset: 0x002093AE
		public void Start()
		{
			if (this.started)
			{
				return;
			}
			this.started = true;
			this.TryGetNextJob();
		}

		// Token: 0x06005FA2 RID: 24482 RVA: 0x0020B1C8 File Offset: 0x002093C8
		public void Tick()
		{
			if (!this.started)
			{
				return;
			}
			if (this.curJob == null || this.curJob.jobState == ShipJobState.Ended)
			{
				this.TryGetNextJob();
			}
			if (this.curJob != null)
			{
				this.curJob.Tick();
				return;
			}
			if (this.CanDispose)
			{
				this.Dispose();
			}
		}

		// Token: 0x06005FA3 RID: 24483 RVA: 0x0020B21C File Offset: 0x0020941C
		public void ForceJob(ShipJob shipJob)
		{
			if (this.curJob != null && this.curJob.Interruptible)
			{
				this.EndCurrentJob();
			}
			this.shipJobs.Clear();
			this.AddJob(shipJob);
			this.TryGetNextJob();
		}

		// Token: 0x06005FA4 RID: 24484 RVA: 0x0020B251 File Offset: 0x00209451
		public void ForceJob(ShipJobDef def)
		{
			this.ForceJob(ShipJobMaker.MakeShipJob(def));
		}

		// Token: 0x06005FA5 RID: 24485 RVA: 0x0020B25F File Offset: 0x0020945F
		public void SetNextJob(ShipJob shipJob)
		{
			if (this.Disposed)
			{
				Log.Error("Trying to add a job to a disposed transport ship. id=" + this.GetUniqueLoadID());
				return;
			}
			shipJob.transportShip = this;
			this.shipJobs.Insert(0, shipJob);
		}

		// Token: 0x06005FA6 RID: 24486 RVA: 0x0020B293 File Offset: 0x00209493
		public void ForceJob_DelayCurrent(ShipJob shipJob)
		{
			if (this.curJob != null)
			{
				this.shipJobs.Insert(0, this.curJob);
				this.curJob = null;
			}
			this.SetNextJob(shipJob);
			this.TryGetNextJob();
		}

		// Token: 0x06005FA7 RID: 24487 RVA: 0x0020B2C3 File Offset: 0x002094C3
		public void AddJob(ShipJob shipJob)
		{
			if (this.Disposed)
			{
				Log.Error("Trying to add a job to a disposed transport ship. id=" + this.GetUniqueLoadID());
				return;
			}
			shipJob.transportShip = this;
			this.shipJobs.Add(shipJob);
		}

		// Token: 0x06005FA8 RID: 24488 RVA: 0x0020B2F6 File Offset: 0x002094F6
		public void AddJob(ShipJobDef def)
		{
			this.AddJob(ShipJobMaker.MakeShipJob(def));
		}

		// Token: 0x06005FA9 RID: 24489 RVA: 0x0020B304 File Offset: 0x00209504
		public void AddJobs(params ShipJobDef[] defs)
		{
			if (defs == null)
			{
				return;
			}
			for (int i = 0; i < defs.Length; i++)
			{
				this.AddJob(defs[i]);
			}
		}

		// Token: 0x06005FAA RID: 24490 RVA: 0x0020B32C File Offset: 0x0020952C
		public void EndCurrentJob()
		{
			if (this.curJob != null)
			{
				this.curJob.End();
			}
			this.curJob = null;
		}

		// Token: 0x06005FAB RID: 24491 RVA: 0x0020B348 File Offset: 0x00209548
		public void TryGetNextJob()
		{
			this.EndCurrentJob();
			if (this.shipJobs.NullOrEmpty<ShipJob>())
			{
				return;
			}
			ShipJob shipJob = this.shipJobs.First<ShipJob>();
			if (shipJob.TryStart())
			{
				this.curJob = shipJob;
				this.shipJobs.Remove(shipJob);
			}
		}

		// Token: 0x06005FAC RID: 24492 RVA: 0x0020B394 File Offset: 0x00209594
		public void ArriveAt(IntVec3 cell, MapParent mapParent)
		{
			if (this.curJob != null && this.curJob.Interruptible)
			{
				this.EndCurrentJob();
			}
			ShipJob_Arrive shipJob_Arrive = (ShipJob_Arrive)ShipJobMaker.MakeShipJob(ShipJobDefOf.Arrive);
			shipJob_Arrive.cell = cell;
			shipJob_Arrive.mapParent = mapParent;
			this.SetNextJob(shipJob_Arrive);
			this.Start();
			if (this.curJob == null || this.curJob.Interruptible)
			{
				this.TryGetNextJob();
			}
		}

		// Token: 0x06005FAD RID: 24493 RVA: 0x0020B404 File Offset: 0x00209604
		public void Dispose()
		{
			if (this.shipThing != null && !this.shipThing.Destroyed)
			{
				this.shipThing.Destroy(DestroyMode.QuestLogic);
			}
			QuestUtility.SendQuestTargetSignals(this.questTags, "Disposed", this.Named("SUBJECT"));
			Find.TransportShipManager.DeregisterShipObject(this);
		}

		// Token: 0x06005FAE RID: 24494 RVA: 0x0020B458 File Offset: 0x00209658
		public void LogJobs()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Jobs for " + this.GetUniqueLoadID() + ":");
			if (this.curJob == null)
			{
				stringBuilder.AppendLine("  - CurJob: null");
			}
			else
			{
				stringBuilder.AppendLine("  - CurJob: " + this.curJob.GetJobInfo());
			}
			foreach (ShipJob shipJob in this.shipJobs)
			{
				stringBuilder.AppendLine("  - " + shipJob.GetJobInfo());
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x06005FAF RID: 24495 RVA: 0x0020B51C File Offset: 0x0020971C
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_Values.Look<bool>(ref this.disposed, "disposed", false, false);
			Scribe_Values.Look<bool>(ref this.started, "started", false, false);
			Scribe_Defs.Look<TransportShipDef>(ref this.def, "def");
			Scribe_Deep.Look<ShipJob>(ref this.curJob, "curJob", Array.Empty<object>());
			Scribe_Collections.Look<ShipJob>(ref this.shipJobs, "shipJobs", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<string>(ref this.questTags, "questTags", LookMode.Value, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.shipThingSpawned = (this.shipThing != null && this.shipThing.SpawnedOrAnyParentSpawned);
			}
			Scribe_Values.Look<bool>(ref this.shipThingSpawned, "shipThingSpawned", false, false);
			if (this.shipThingSpawned)
			{
				Scribe_References.Look<Thing>(ref this.shipThing, "shipThing", false);
				return;
			}
			Scribe_Deep.Look<Thing>(ref this.shipThing, "shipThing", Array.Empty<object>());
		}

		// Token: 0x06005FB0 RID: 24496 RVA: 0x0020B615 File Offset: 0x00209815
		public string GetUniqueLoadID()
		{
			return "TransportShip_" + this.loadID;
		}

		// Token: 0x040036EB RID: 14059
		public int loadID = -1;

		// Token: 0x040036EC RID: 14060
		public TransportShipDef def;

		// Token: 0x040036ED RID: 14061
		public ShipJob curJob;

		// Token: 0x040036EE RID: 14062
		public Thing shipThing;

		// Token: 0x040036EF RID: 14063
		private List<ShipJob> shipJobs = new List<ShipJob>();

		// Token: 0x040036F0 RID: 14064
		public List<string> questTags = new List<string>();

		// Token: 0x040036F1 RID: 14065
		public bool started;

		// Token: 0x040036F2 RID: 14066
		private bool disposed;

		// Token: 0x040036F3 RID: 14067
		private bool shipThingSpawned;

		// Token: 0x040036F4 RID: 14068
		[Unsaved(false)]
		private CompShuttle cachedCompShuttle;

		// Token: 0x040036F5 RID: 14069
		[Unsaved(false)]
		private CompTransporter cachedCompTransporter;
	}
}
