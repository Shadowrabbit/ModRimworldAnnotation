using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020018E2 RID: 6370
	public class PassingShip : IExposable, ICommunicable, ILoadReferenceable
	{
		// Token: 0x17001634 RID: 5684
		// (get) Token: 0x06008D14 RID: 36116 RVA: 0x0005E8A3 File Offset: 0x0005CAA3
		public virtual string FullTitle
		{
			get
			{
				return "ErrorFullTitle";
			}
		}

		// Token: 0x17001635 RID: 5685
		// (get) Token: 0x06008D15 RID: 36117 RVA: 0x0005E8AA File Offset: 0x0005CAAA
		public bool Departed
		{
			get
			{
				return this.ticksUntilDeparture <= 0;
			}
		}

		// Token: 0x17001636 RID: 5686
		// (get) Token: 0x06008D16 RID: 36118 RVA: 0x0005E8B8 File Offset: 0x0005CAB8
		public Map Map
		{
			get
			{
				if (this.passingShipManager == null)
				{
					return null;
				}
				return this.passingShipManager.map;
			}
		}

		// Token: 0x17001637 RID: 5687
		// (get) Token: 0x06008D17 RID: 36119 RVA: 0x0005E8CF File Offset: 0x0005CACF
		public Faction Faction
		{
			get
			{
				return this.faction;
			}
		}

		// Token: 0x06008D18 RID: 36120 RVA: 0x0005E8D7 File Offset: 0x0005CAD7
		public PassingShip()
		{
		}

		// Token: 0x06008D19 RID: 36121 RVA: 0x0005E8FC File Offset: 0x0005CAFC
		public PassingShip(Faction faction)
		{
			this.faction = faction;
		}

		// Token: 0x06008D1A RID: 36122 RVA: 0x0028E7AC File Offset: 0x0028C9AC
		public virtual void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_Values.Look<int>(ref this.ticksUntilDeparture, "ticksUntilDeparture", 0, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
		}

		// Token: 0x06008D1B RID: 36123 RVA: 0x0005E928 File Offset: 0x0005CB28
		public virtual void PassingShipTick()
		{
			this.ticksUntilDeparture--;
			if (this.Departed)
			{
				this.Depart();
			}
		}

		// Token: 0x06008D1C RID: 36124 RVA: 0x0028E800 File Offset: 0x0028CA00
		public virtual void Depart()
		{
			if (this.Map.listerBuildings.ColonistsHaveBuilding((Thing b) => b.def.IsCommsConsole))
			{
				Messages.Message("MessageShipHasLeftCommsRange".Translate(this.FullTitle), MessageTypeDefOf.SituationResolved, true);
			}
			this.passingShipManager.RemoveShip(this);
		}

		// Token: 0x06008D1D RID: 36125 RVA: 0x0000FC33 File Offset: 0x0000DE33
		public virtual void TryOpenComms(Pawn negotiator)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06008D1E RID: 36126 RVA: 0x0005E946 File Offset: 0x0005CB46
		public virtual string GetCallLabel()
		{
			return this.name;
		}

		// Token: 0x06008D1F RID: 36127 RVA: 0x0005E94E File Offset: 0x0005CB4E
		public string GetInfoText()
		{
			return this.FullTitle;
		}

		// Token: 0x06008D20 RID: 36128 RVA: 0x0000C32E File Offset: 0x0000A52E
		Faction ICommunicable.GetFaction()
		{
			return null;
		}

		// Token: 0x06008D21 RID: 36129 RVA: 0x00012DFE File Offset: 0x00010FFE
		protected virtual AcceptanceReport CanCommunicateWith_NewTemp(Pawn negotiator)
		{
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x06008D22 RID: 36130 RVA: 0x0028E870 File Offset: 0x0028CA70
		protected virtual bool CanCommunicateWith(Pawn negotiator)
		{
			return this.CanCommunicateWith_NewTemp(negotiator).Accepted;
		}

		// Token: 0x06008D23 RID: 36131 RVA: 0x0028E88C File Offset: 0x0028CA8C
		public FloatMenuOption CommFloatMenuOption(Building_CommsConsole console, Pawn negotiator)
		{
			string label = "CallOnRadio".Translate(this.GetCallLabel());
			Action action = null;
			AcceptanceReport canCommunicate = this.CanCommunicateWith_NewTemp(negotiator);
			if (!canCommunicate.Accepted)
			{
				if (!canCommunicate.Reason.NullOrEmpty())
				{
					action = delegate()
					{
						Messages.Message(canCommunicate.Reason, console, MessageTypeDefOf.RejectInput, false);
					};
				}
			}
			else
			{
				action = delegate()
				{
					if (!Building_OrbitalTradeBeacon.AllPowered(this.Map).Any<Building_OrbitalTradeBeacon>())
					{
						Messages.Message("MessageNeedBeaconToTradeWithShip".Translate(), console, MessageTypeDefOf.RejectInput, false);
						return;
					}
					console.GiveUseCommsJob(negotiator, this);
				};
			}
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(label, action, MenuOptionPriority.InitiateSocial, null, null, 0f, null, null), negotiator, console, "ReservedBy");
		}

		// Token: 0x06008D24 RID: 36132 RVA: 0x0005E956 File Offset: 0x0005CB56
		public string GetUniqueLoadID()
		{
			return "PassingShip_" + this.loadID;
		}

		// Token: 0x04005A0B RID: 23051
		public PassingShipManager passingShipManager;

		// Token: 0x04005A0C RID: 23052
		private Faction faction;

		// Token: 0x04005A0D RID: 23053
		public string name = "Nameless";

		// Token: 0x04005A0E RID: 23054
		protected int loadID = -1;

		// Token: 0x04005A0F RID: 23055
		public int ticksUntilDeparture = 40000;
	}
}
