using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000385 RID: 901
	public class CompEquippable : ThingComp, IVerbOwner
	{
		// Token: 0x1700056E RID: 1390
		// (get) Token: 0x06001A61 RID: 6753 RVA: 0x000997D8 File Offset: 0x000979D8
		private Pawn Holder
		{
			get
			{
				return this.PrimaryVerb.CasterPawn;
			}
		}

		// Token: 0x1700056F RID: 1391
		// (get) Token: 0x06001A62 RID: 6754 RVA: 0x000997E5 File Offset: 0x000979E5
		public List<Verb> AllVerbs
		{
			get
			{
				return this.verbTracker.AllVerbs;
			}
		}

		// Token: 0x17000570 RID: 1392
		// (get) Token: 0x06001A63 RID: 6755 RVA: 0x000997F2 File Offset: 0x000979F2
		public Verb PrimaryVerb
		{
			get
			{
				return this.verbTracker.PrimaryVerb;
			}
		}

		// Token: 0x17000571 RID: 1393
		// (get) Token: 0x06001A64 RID: 6756 RVA: 0x000997FF File Offset: 0x000979FF
		public VerbTracker VerbTracker
		{
			get
			{
				return this.verbTracker;
			}
		}

		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x06001A65 RID: 6757 RVA: 0x00099807 File Offset: 0x00097A07
		public List<VerbProperties> VerbProperties
		{
			get
			{
				return this.parent.def.Verbs;
			}
		}

		// Token: 0x17000573 RID: 1395
		// (get) Token: 0x06001A66 RID: 6758 RVA: 0x00099819 File Offset: 0x00097A19
		public List<Tool> Tools
		{
			get
			{
				return this.parent.def.tools;
			}
		}

		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x06001A67 RID: 6759 RVA: 0x00002688 File Offset: 0x00000888
		Thing IVerbOwner.ConstantCaster
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x06001A68 RID: 6760 RVA: 0x0009982B File Offset: 0x00097A2B
		ImplementOwnerTypeDef IVerbOwner.ImplementOwnerTypeDef
		{
			get
			{
				return ImplementOwnerTypeDefOf.Weapon;
			}
		}

		// Token: 0x06001A69 RID: 6761 RVA: 0x00099832 File Offset: 0x00097A32
		public CompEquippable()
		{
			this.verbTracker = new VerbTracker(this);
		}

		// Token: 0x06001A6A RID: 6762 RVA: 0x00099846 File Offset: 0x00097A46
		public IEnumerable<Command> GetVerbsCommands()
		{
			return this.verbTracker.GetVerbsCommands(KeyCode.None);
		}

		// Token: 0x06001A6B RID: 6763 RVA: 0x00099854 File Offset: 0x00097A54
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			base.PostDestroy(mode, previousMap);
			if (this.Holder != null && this.Holder.equipment != null && this.Holder.equipment.Primary == this.parent)
			{
				this.Holder.equipment.Notify_PrimaryDestroyed();
			}
		}

		// Token: 0x06001A6C RID: 6764 RVA: 0x000998A6 File Offset: 0x00097AA6
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Deep.Look<VerbTracker>(ref this.verbTracker, "verbTracker", new object[]
			{
				this
			});
		}

		// Token: 0x06001A6D RID: 6765 RVA: 0x000998C8 File Offset: 0x00097AC8
		public override void CompTick()
		{
			base.CompTick();
			this.verbTracker.VerbsTick();
		}

		// Token: 0x06001A6E RID: 6766 RVA: 0x000998DC File Offset: 0x00097ADC
		public override void Notify_Unequipped(Pawn p)
		{
			List<Verb> allVerbs = this.AllVerbs;
			for (int i = 0; i < allVerbs.Count; i++)
			{
				allVerbs[i].Notify_EquipmentLost();
			}
		}

		// Token: 0x06001A6F RID: 6767 RVA: 0x0009990D File Offset: 0x00097B0D
		string IVerbOwner.UniqueVerbOwnerID()
		{
			return "CompEquippable_" + this.parent.ThingID;
		}

		// Token: 0x06001A70 RID: 6768 RVA: 0x00099924 File Offset: 0x00097B24
		bool IVerbOwner.VerbsStillUsableBy(Pawn p)
		{
			Apparel apparel = this.parent as Apparel;
			if (apparel != null)
			{
				return p.apparel.WornApparel.Contains(apparel);
			}
			return p.equipment.AllEquipmentListForReading.Contains(this.parent);
		}

		// Token: 0x04001132 RID: 4402
		public VerbTracker verbTracker;
	}
}
