using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001070 RID: 4208
	public class Building_Casket : Building, IThingHolder, IOpenable
	{
		// Token: 0x1700110C RID: 4364
		// (get) Token: 0x060063D1 RID: 25553 RVA: 0x001794E9 File Offset: 0x001776E9
		public virtual int OpenTicks
		{
			get
			{
				return 300;
			}
		}

		// Token: 0x060063D2 RID: 25554 RVA: 0x0021B753 File Offset: 0x00219953
		public Building_Casket()
		{
			this.innerContainer = new ThingOwner<Thing>(this, false, LookMode.Deep);
		}

		// Token: 0x1700110D RID: 4365
		// (get) Token: 0x060063D3 RID: 25555 RVA: 0x0021B769 File Offset: 0x00219969
		public bool HasAnyContents
		{
			get
			{
				return this.innerContainer.Count > 0;
			}
		}

		// Token: 0x1700110E RID: 4366
		// (get) Token: 0x060063D4 RID: 25556 RVA: 0x0021B779 File Offset: 0x00219979
		public Thing ContainedThing
		{
			get
			{
				if (this.innerContainer.Count != 0)
				{
					return this.innerContainer[0];
				}
				return null;
			}
		}

		// Token: 0x1700110F RID: 4367
		// (get) Token: 0x060063D5 RID: 25557 RVA: 0x0021B796 File Offset: 0x00219996
		public virtual bool CanOpen
		{
			get
			{
				return this.HasAnyContents;
			}
		}

		// Token: 0x060063D6 RID: 25558 RVA: 0x0021B79E File Offset: 0x0021999E
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x060063D7 RID: 25559 RVA: 0x0021B7A6 File Offset: 0x002199A6
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x060063D8 RID: 25560 RVA: 0x0021B7B4 File Offset: 0x002199B4
		public override void TickRare()
		{
			base.TickRare();
			this.innerContainer.ThingOwnerTickRare(true);
		}

		// Token: 0x060063D9 RID: 25561 RVA: 0x0021B7C8 File Offset: 0x002199C8
		public override void Tick()
		{
			base.Tick();
			this.innerContainer.ThingOwnerTick(true);
		}

		// Token: 0x060063DA RID: 25562 RVA: 0x0021B7DC File Offset: 0x002199DC
		public virtual void Open()
		{
			if (!this.HasAnyContents)
			{
				return;
			}
			this.EjectContents();
			if (!this.openedSignal.NullOrEmpty())
			{
				Find.SignalManager.SendSignal(new Signal(this.openedSignal, this.Named("SUBJECT")));
			}
		}

		// Token: 0x060063DB RID: 25563 RVA: 0x0021B81A File Offset: 0x00219A1A
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (Prefs.DevMode && this.CanOpen)
			{
				yield return new Command_Action
				{
					defaultLabel = "Dev: Open",
					action = delegate()
					{
						this.Open();
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x060063DC RID: 25564 RVA: 0x0021B82C File Offset: 0x00219A2C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_Values.Look<bool>(ref this.contentsKnown, "contentsKnown", false, false);
			Scribe_Values.Look<string>(ref this.openedSignal, "openedSignal", null, false);
		}

		// Token: 0x060063DD RID: 25565 RVA: 0x0021B87D File Offset: 0x00219A7D
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (base.Faction != null && base.Faction.IsPlayer)
			{
				this.contentsKnown = true;
			}
		}

		// Token: 0x060063DE RID: 25566 RVA: 0x0021B8A4 File Offset: 0x00219AA4
		public override bool ClaimableBy(Faction fac)
		{
			if (this.innerContainer.Any)
			{
				for (int i = 0; i < this.innerContainer.Count; i++)
				{
					if (this.innerContainer[i].Faction == fac)
					{
						return true;
					}
				}
				return false;
			}
			return base.ClaimableBy(fac);
		}

		// Token: 0x060063DF RID: 25567 RVA: 0x0021B8F3 File Offset: 0x00219AF3
		public virtual bool Accepts(Thing thing)
		{
			return this.innerContainer.CanAcceptAnyOf(thing, true);
		}

		// Token: 0x060063E0 RID: 25568 RVA: 0x0021B904 File Offset: 0x00219B04
		public virtual bool TryAcceptThing(Thing thing, bool allowSpecialEffects = true)
		{
			if (!this.Accepts(thing))
			{
				return false;
			}
			bool flag;
			if (thing.holdingOwner != null)
			{
				thing.holdingOwner.TryTransferToContainer(thing, this.innerContainer, thing.stackCount, true);
				flag = true;
			}
			else
			{
				flag = this.innerContainer.TryAdd(thing, true);
			}
			if (flag)
			{
				if (thing.Faction != null && thing.Faction.IsPlayer)
				{
					this.contentsKnown = true;
				}
				return true;
			}
			return false;
		}

		// Token: 0x060063E1 RID: 25569 RVA: 0x0021B974 File Offset: 0x00219B74
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			if (this.innerContainer.Count > 0 && (mode == DestroyMode.Deconstruct || mode == DestroyMode.KillFinalize))
			{
				if (mode != DestroyMode.Deconstruct)
				{
					List<Pawn> list = new List<Pawn>();
					foreach (Thing thing in ((IEnumerable<Thing>)this.innerContainer))
					{
						Pawn pawn = thing as Pawn;
						if (pawn != null)
						{
							list.Add(pawn);
						}
					}
					foreach (Pawn p in list)
					{
						HealthUtility.DamageUntilDowned(p, true);
					}
				}
				this.EjectContents();
			}
			this.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
			base.Destroy(mode);
		}

		// Token: 0x060063E2 RID: 25570 RVA: 0x0021BA40 File Offset: 0x00219C40
		public virtual void EjectContents()
		{
			this.innerContainer.TryDropAll(this.InteractionCell, base.Map, ThingPlaceMode.Near, null, null, true);
			this.contentsKnown = true;
		}

		// Token: 0x060063E3 RID: 25571 RVA: 0x0021BA68 File Offset: 0x00219C68
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			string str;
			if (!this.contentsKnown)
			{
				str = "UnknownLower".Translate();
			}
			else
			{
				str = this.innerContainer.ContentsString;
			}
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			return text + ("CasketContains".Translate() + ": " + str.CapitalizeFirst());
		}

		// Token: 0x04003879 RID: 14457
		protected ThingOwner innerContainer;

		// Token: 0x0400387A RID: 14458
		protected bool contentsKnown;

		// Token: 0x0400387B RID: 14459
		public string openedSignal;
	}
}
