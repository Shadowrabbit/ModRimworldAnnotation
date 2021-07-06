using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x0200054A RID: 1354
	public class ReverseDesignatorDatabase
	{
		// Token: 0x17000695 RID: 1685
		// (get) Token: 0x060022CF RID: 8911 RVA: 0x0001DD3E File Offset: 0x0001BF3E
		public List<Designator> AllDesignators
		{
			get
			{
				if (this.desList == null)
				{
					this.InitDesignators();
				}
				return this.desList;
			}
		}

		// Token: 0x060022D0 RID: 8912 RVA: 0x0001DD54 File Offset: 0x0001BF54
		public void Reinit()
		{
			this.desList = null;
		}

		// Token: 0x060022D1 RID: 8913 RVA: 0x0010B2A8 File Offset: 0x001094A8
		public T Get<T>() where T : Designator
		{
			if (this.desList == null)
			{
				this.InitDesignators();
			}
			for (int i = 0; i < this.desList.Count; i++)
			{
				T t = this.desList[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x060022D2 RID: 8914 RVA: 0x0010B304 File Offset: 0x00109504
		private void InitDesignators()
		{
			this.desList = new List<Designator>();
			this.desList.Add(new Designator_Cancel());
			this.desList.Add(new Designator_Claim());
			this.desList.Add(new Designator_Deconstruct());
			this.desList.Add(new Designator_Uninstall());
			this.desList.Add(new Designator_Haul());
			this.desList.Add(new Designator_Hunt());
			this.desList.Add(new Designator_Slaughter());
			this.desList.Add(new Designator_Tame());
			this.desList.Add(new Designator_PlantsCut());
			this.desList.Add(new Designator_PlantsHarvest());
			this.desList.Add(new Designator_PlantsHarvestWood());
			this.desList.Add(new Designator_Mine());
			this.desList.Add(new Designator_Strip());
			this.desList.Add(new Designator_Open());
			this.desList.Add(new Designator_SmoothSurface());
			this.desList.RemoveAll((Designator des) => !Current.Game.Rules.DesignatorAllowed(des));
		}

		// Token: 0x04001792 RID: 6034
		private List<Designator> desList;
	}
}
