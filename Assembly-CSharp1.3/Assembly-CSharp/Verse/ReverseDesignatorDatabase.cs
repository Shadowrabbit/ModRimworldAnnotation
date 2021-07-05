using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000398 RID: 920
	public class ReverseDesignatorDatabase
	{
		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x06001B22 RID: 6946 RVA: 0x0009D30E File Offset: 0x0009B50E
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

		// Token: 0x06001B23 RID: 6947 RVA: 0x0009D324 File Offset: 0x0009B524
		public void Reinit()
		{
			this.desList = null;
		}

		// Token: 0x06001B24 RID: 6948 RVA: 0x0009D330 File Offset: 0x0009B530
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

		// Token: 0x06001B25 RID: 6949 RVA: 0x0009D38C File Offset: 0x0009B58C
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
			this.desList.Add(new Designator_ReleaseAnimalToWild());
			this.desList.Add(new Designator_Study());
			if (ModsConfig.IdeologyActive)
			{
				this.desList.Add(new Designator_ExtractSkull());
			}
			this.desList.RemoveAll((Designator des) => !Current.Game.Rules.DesignatorAllowed(des));
		}

		// Token: 0x0400119C RID: 4508
		private List<Designator> desList;
	}
}
