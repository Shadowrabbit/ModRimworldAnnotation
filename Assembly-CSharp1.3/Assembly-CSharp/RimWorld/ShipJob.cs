using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020008EB RID: 2283
	public abstract class ShipJob : IExposable, ILoadReferenceable
	{
		// Token: 0x17000AB7 RID: 2743
		// (get) Token: 0x06003BD7 RID: 15319
		protected abstract bool ShouldEnd { get; }

		// Token: 0x17000AB8 RID: 2744
		// (get) Token: 0x06003BD8 RID: 15320 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool HasDestination
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000AB9 RID: 2745
		// (get) Token: 0x06003BD9 RID: 15321 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool Interruptible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000ABA RID: 2746
		// (get) Token: 0x06003BDA RID: 15322 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool ShowGizmos
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003BDB RID: 15323 RVA: 0x0014DDA9 File Offset: 0x0014BFA9
		public ShipJob()
		{
		}

		// Token: 0x06003BDC RID: 15324 RVA: 0x0014DDB8 File Offset: 0x0014BFB8
		public ShipJob(TransportShip transportShip)
		{
			this.loadID = Find.UniqueIDsManager.GetNextShipJobID();
			this.transportShip = transportShip;
		}

		// Token: 0x06003BDD RID: 15325 RVA: 0x0014DDDE File Offset: 0x0014BFDE
		public virtual bool TryStart()
		{
			if (this.transportShip == null)
			{
				Log.Error("Trying to start a ship job with a null ship object.");
				return false;
			}
			if (this.jobState == ShipJobState.Ended)
			{
				Log.Error("Trying to start an already ended ship job.");
				return false;
			}
			this.jobState = ShipJobState.Working;
			return true;
		}

		// Token: 0x06003BDE RID: 15326 RVA: 0x0014DE11 File Offset: 0x0014C011
		public virtual void End()
		{
			this.jobState = ShipJobState.Ended;
		}

		// Token: 0x06003BDF RID: 15327 RVA: 0x0014DE1A File Offset: 0x0014C01A
		public virtual void Tick()
		{
			if (this.jobState != ShipJobState.Working)
			{
				Log.Error("Trying to tick " + this.jobState.ToString() + " job.");
			}
			if (this.ShouldEnd)
			{
				this.End();
			}
		}

		// Token: 0x06003BE0 RID: 15328 RVA: 0x00002688 File Offset: 0x00000888
		public virtual IEnumerable<Gizmo> GetJobGizmos()
		{
			return null;
		}

		// Token: 0x06003BE1 RID: 15329 RVA: 0x0014DE58 File Offset: 0x0014C058
		public virtual string GetJobInfo()
		{
			return base.GetType().Name;
		}

		// Token: 0x06003BE2 RID: 15330 RVA: 0x00002688 File Offset: 0x00000888
		public virtual string ShipThingExtraInspectString()
		{
			return null;
		}

		// Token: 0x06003BE3 RID: 15331 RVA: 0x0014DE68 File Offset: 0x0014C068
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<ShipJobDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_Values.Look<ShipJobState>(ref this.jobState, "jobState", ShipJobState.Uninitialized, false);
			Scribe_References.Look<TransportShip>(ref this.transportShip, "transportShip", false);
		}

		// Token: 0x06003BE4 RID: 15332 RVA: 0x0014DEBA File Offset: 0x0014C0BA
		public string GetUniqueLoadID()
		{
			return "ShipJob_" + this.loadID;
		}

		// Token: 0x0400208A RID: 8330
		public ShipJobDef def;

		// Token: 0x0400208B RID: 8331
		public int loadID = -1;

		// Token: 0x0400208C RID: 8332
		public ShipJobState jobState;

		// Token: 0x0400208D RID: 8333
		public TransportShip transportShip;
	}
}
