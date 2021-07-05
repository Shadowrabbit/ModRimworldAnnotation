using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001213 RID: 4627
	public class PassingShip : IExposable, ICommunicable, ILoadReferenceable
	{
		// Token: 0x17001355 RID: 4949
		// (get) Token: 0x06006F13 RID: 28435 RVA: 0x00251B54 File Offset: 0x0024FD54
		public virtual string FullTitle
		{
			get
			{
				return "ErrorFullTitle";
			}
		}

		// Token: 0x17001356 RID: 4950
		// (get) Token: 0x06006F14 RID: 28436 RVA: 0x00251B5B File Offset: 0x0024FD5B
		public bool Departed
		{
			get
			{
				return this.ticksUntilDeparture <= 0;
			}
		}

		// Token: 0x17001357 RID: 4951
		// (get) Token: 0x06006F15 RID: 28437 RVA: 0x00251B69 File Offset: 0x0024FD69
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

		// Token: 0x17001358 RID: 4952
		// (get) Token: 0x06006F16 RID: 28438 RVA: 0x00251B80 File Offset: 0x0024FD80
		public Faction Faction
		{
			get
			{
				return this.faction;
			}
		}

		// Token: 0x06006F17 RID: 28439 RVA: 0x00251B88 File Offset: 0x0024FD88
		public PassingShip()
		{
		}

		// Token: 0x06006F18 RID: 28440 RVA: 0x00251BAD File Offset: 0x0024FDAD
		public PassingShip(Faction faction)
		{
			this.faction = faction;
		}

		// Token: 0x06006F19 RID: 28441 RVA: 0x00251BDC File Offset: 0x0024FDDC
		public virtual void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_Values.Look<int>(ref this.ticksUntilDeparture, "ticksUntilDeparture", 0, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
		}

		// Token: 0x06006F1A RID: 28442 RVA: 0x00251C30 File Offset: 0x0024FE30
		public virtual void PassingShipTick()
		{
			this.ticksUntilDeparture--;
			if (this.Departed)
			{
				this.Depart();
			}
		}

		// Token: 0x06006F1B RID: 28443 RVA: 0x00251C50 File Offset: 0x0024FE50
		public virtual void Depart()
		{
			if (this.Map.listerBuildings.ColonistsHaveBuilding((Thing b) => b.def.IsCommsConsole))
			{
				Messages.Message("MessageShipHasLeftCommsRange".Translate(this.FullTitle), MessageTypeDefOf.SituationResolved, true);
			}
			this.passingShipManager.RemoveShip(this);
		}

		// Token: 0x06006F1C RID: 28444 RVA: 0x0002974C File Offset: 0x0002794C
		public virtual void TryOpenComms(Pawn negotiator)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06006F1D RID: 28445 RVA: 0x00251CBF File Offset: 0x0024FEBF
		public virtual string GetCallLabel()
		{
			return this.name;
		}

		// Token: 0x06006F1E RID: 28446 RVA: 0x00251CC7 File Offset: 0x0024FEC7
		public string GetInfoText()
		{
			return this.FullTitle;
		}

		// Token: 0x06006F1F RID: 28447 RVA: 0x00002688 File Offset: 0x00000888
		Faction ICommunicable.GetFaction()
		{
			return null;
		}

		// Token: 0x06006F20 RID: 28448 RVA: 0x0004EE58 File Offset: 0x0004D058
		protected virtual AcceptanceReport CanCommunicateWith(Pawn negotiator)
		{
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x06006F21 RID: 28449 RVA: 0x00251CD0 File Offset: 0x0024FED0
		public FloatMenuOption CommFloatMenuOption(Building_CommsConsole console, Pawn negotiator)
		{
			string label = "CallOnRadio".Translate(this.GetCallLabel());
			Action action = null;
			AcceptanceReport canCommunicate = this.CanCommunicateWith(negotiator);
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
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(label, action, MenuOptionPriority.InitiateSocial, null, null, 0f, null, null, true, 0), negotiator, console, "ReservedBy");
		}

		// Token: 0x06006F22 RID: 28450 RVA: 0x00251D8E File Offset: 0x0024FF8E
		public string GetUniqueLoadID()
		{
			return "PassingShip_" + this.loadID;
		}

		// Token: 0x04003D64 RID: 15716
		public PassingShipManager passingShipManager;

		// Token: 0x04003D65 RID: 15717
		private Faction faction;

		// Token: 0x04003D66 RID: 15718
		public string name = "Nameless";

		// Token: 0x04003D67 RID: 15719
		protected int loadID = -1;

		// Token: 0x04003D68 RID: 15720
		public int ticksUntilDeparture = 40000;
	}
}
