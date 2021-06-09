using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000526 RID: 1318
	public class CompEquippable : ThingComp, IVerbOwner
	{
		// Token: 0x17000662 RID: 1634
		// (get) Token: 0x060021C7 RID: 8647 RVA: 0x0001D3B1 File Offset: 0x0001B5B1
		private Pawn Holder
		{
			get
			{
				return this.PrimaryVerb.CasterPawn;
			}
		}

		// Token: 0x17000663 RID: 1635
		// (get) Token: 0x060021C8 RID: 8648 RVA: 0x0001D3BE File Offset: 0x0001B5BE
		public List<Verb> AllVerbs
		{
			get
			{
				return this.verbTracker.AllVerbs;
			}
		}

		// Token: 0x17000664 RID: 1636
		// (get) Token: 0x060021C9 RID: 8649 RVA: 0x0001D3CB File Offset: 0x0001B5CB
		public Verb PrimaryVerb
		{
			get
			{
				return this.verbTracker.PrimaryVerb;
			}
		}

		// Token: 0x17000665 RID: 1637
		// (get) Token: 0x060021CA RID: 8650 RVA: 0x0001D3D8 File Offset: 0x0001B5D8
		public VerbTracker VerbTracker
		{
			get
			{
				return this.verbTracker;
			}
		}

		// Token: 0x17000666 RID: 1638
		// (get) Token: 0x060021CB RID: 8651 RVA: 0x0001D3E0 File Offset: 0x0001B5E0
		public List<VerbProperties> VerbProperties
		{
			get
			{
				return this.parent.def.Verbs;
			}
		}

		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x060021CC RID: 8652 RVA: 0x0001D3F2 File Offset: 0x0001B5F2
		public List<Tool> Tools
		{
			get
			{
				return this.parent.def.tools;
			}
		}

		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x060021CD RID: 8653 RVA: 0x0000C32E File Offset: 0x0000A52E
		Thing IVerbOwner.ConstantCaster
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x060021CE RID: 8654 RVA: 0x0001D404 File Offset: 0x0001B604
		ImplementOwnerTypeDef IVerbOwner.ImplementOwnerTypeDef
		{
			get
			{
				return ImplementOwnerTypeDefOf.Weapon;
			}
		}

		// Token: 0x060021CF RID: 8655 RVA: 0x0001D40B File Offset: 0x0001B60B
		public CompEquippable()
		{
			this.verbTracker = new VerbTracker(this);
		}

		// Token: 0x060021D0 RID: 8656 RVA: 0x0001D41F File Offset: 0x0001B61F
		public IEnumerable<Command> GetVerbsCommands()
		{
			return this.verbTracker.GetVerbsCommands(KeyCode.None);
		}

		// Token: 0x060021D1 RID: 8657 RVA: 0x00107A4C File Offset: 0x00105C4C
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			base.PostDestroy(mode, previousMap);
			if (this.Holder != null && this.Holder.equipment != null && this.Holder.equipment.Primary == this.parent)
			{
				this.Holder.equipment.Notify_PrimaryDestroyed();
			}
		}

		// Token: 0x060021D2 RID: 8658 RVA: 0x0001D42D File Offset: 0x0001B62D
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Deep.Look<VerbTracker>(ref this.verbTracker, "verbTracker", new object[]
			{
				this
			});
		}

		// Token: 0x060021D3 RID: 8659 RVA: 0x0001D44F File Offset: 0x0001B64F
		public override void CompTick()
		{
			base.CompTick();
			this.verbTracker.VerbsTick();
		}

		// Token: 0x060021D4 RID: 8660 RVA: 0x00107AA0 File Offset: 0x00105CA0
		public void Notify_EquipmentLost()
		{
			List<Verb> allVerbs = this.AllVerbs;
			for (int i = 0; i < allVerbs.Count; i++)
			{
				allVerbs[i].Notify_EquipmentLost();
			}
		}

		// Token: 0x060021D5 RID: 8661 RVA: 0x0001D462 File Offset: 0x0001B662
		string IVerbOwner.UniqueVerbOwnerID()
		{
			return "CompEquippable_" + this.parent.ThingID;
		}

		// Token: 0x060021D6 RID: 8662 RVA: 0x00107AD4 File Offset: 0x00105CD4
		bool IVerbOwner.VerbsStillUsableBy(Pawn p)
		{
			Apparel apparel = this.parent as Apparel;
			if (apparel != null)
			{
				return p.apparel.WornApparel.Contains(apparel);
			}
			return p.equipment.AllEquipmentListForReading.Contains(this.parent);
		}

		// Token: 0x040016FA RID: 5882
		public VerbTracker verbTracker;
	}
}
