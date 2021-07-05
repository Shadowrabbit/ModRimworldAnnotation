using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020016AA RID: 5802
	public class Building_Casket : Building, IThingHolder, IOpenable
	{
		// Token: 0x06007F05 RID: 32517 RVA: 0x000554B6 File Offset: 0x000536B6
		public Building_Casket()
		{
			this.innerContainer = new ThingOwner<Thing>(this, false, LookMode.Deep);
		}

		// Token: 0x170013AB RID: 5035
		// (get) Token: 0x06007F06 RID: 32518 RVA: 0x000554CC File Offset: 0x000536CC
		public bool HasAnyContents
		{
			get
			{
				return this.innerContainer.Count > 0;
			}
		}

		// Token: 0x170013AC RID: 5036
		// (get) Token: 0x06007F07 RID: 32519 RVA: 0x000554DC File Offset: 0x000536DC
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

		// Token: 0x170013AD RID: 5037
		// (get) Token: 0x06007F08 RID: 32520 RVA: 0x000554F9 File Offset: 0x000536F9
		public bool CanOpen
		{
			get
			{
				return this.HasAnyContents;
			}
		}

		// Token: 0x06007F09 RID: 32521 RVA: 0x00055501 File Offset: 0x00053701
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x06007F0A RID: 32522 RVA: 0x00055509 File Offset: 0x00053709
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06007F0B RID: 32523 RVA: 0x00055517 File Offset: 0x00053717
		public override void TickRare()
		{
			base.TickRare();
			this.innerContainer.ThingOwnerTickRare(true);
		}

		// Token: 0x06007F0C RID: 32524 RVA: 0x0005552B File Offset: 0x0005372B
		public override void Tick()
		{
			base.Tick();
			this.innerContainer.ThingOwnerTick(true);
		}

		// Token: 0x06007F0D RID: 32525 RVA: 0x0005553F File Offset: 0x0005373F
		public virtual void Open()
		{
			if (!this.HasAnyContents)
			{
				return;
			}
			this.EjectContents();
		}

		// Token: 0x06007F0E RID: 32526 RVA: 0x00055550 File Offset: 0x00053750
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_Values.Look<bool>(ref this.contentsKnown, "contentsKnown", false, false);
		}

		// Token: 0x06007F0F RID: 32527 RVA: 0x00055584 File Offset: 0x00053784
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (base.Faction != null && base.Faction.IsPlayer)
			{
				this.contentsKnown = true;
			}
		}

		// Token: 0x06007F10 RID: 32528 RVA: 0x0025BF1C File Offset: 0x0025A11C
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

		// Token: 0x06007F11 RID: 32529 RVA: 0x000555AA File Offset: 0x000537AA
		public virtual bool Accepts(Thing thing)
		{
			return this.innerContainer.CanAcceptAnyOf(thing, true);
		}

		// Token: 0x06007F12 RID: 32530 RVA: 0x0025BF6C File Offset: 0x0025A16C
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

		// Token: 0x06007F13 RID: 32531 RVA: 0x0025BFDC File Offset: 0x0025A1DC
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

		// Token: 0x06007F14 RID: 32532 RVA: 0x000555B9 File Offset: 0x000537B9
		public virtual void EjectContents()
		{
			this.innerContainer.TryDropAll(this.InteractionCell, base.Map, ThingPlaceMode.Near, null, null);
			this.contentsKnown = true;
		}

		// Token: 0x06007F15 RID: 32533 RVA: 0x0025C0A8 File Offset: 0x0025A2A8
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

		// Token: 0x04005294 RID: 21140
		protected ThingOwner innerContainer;

		// Token: 0x04005295 RID: 21141
		protected bool contentsKnown;
	}
}
