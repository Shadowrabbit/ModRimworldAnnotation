using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012BF RID: 4799
	public class Designator_Study : Designator
	{
		// Token: 0x17001408 RID: 5128
		// (get) Token: 0x060072A4 RID: 29348 RVA: 0x0009007E File Offset: 0x0008E27E
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17001409 RID: 5129
		// (get) Token: 0x060072A5 RID: 29349 RVA: 0x00264251 File Offset: 0x00262451
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Study;
			}
		}

		// Token: 0x060072A6 RID: 29350 RVA: 0x00264258 File Offset: 0x00262458
		public Designator_Study()
		{
			this.defaultLabel = "DesignatorStudy".Translate();
			this.defaultDesc = "DesignatorStudyDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Study", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Claim;
		}

		// Token: 0x060072A7 RID: 29351 RVA: 0x002642CE File Offset: 0x002624CE
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			return this.StudyThingsInCell(c).Any<Thing>();
		}

		// Token: 0x060072A8 RID: 29352 RVA: 0x002642F8 File Offset: 0x002624F8
		public override void DesignateSingleCell(IntVec3 c)
		{
			foreach (Thing t in this.StudyThingsInCell(c))
			{
				this.DesignateThing(t);
			}
		}

		// Token: 0x060072A9 RID: 29353 RVA: 0x00264348 File Offset: 0x00262548
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			if (base.Map.designationManager.DesignationOn(t, this.Designation) != null)
			{
				return false;
			}
			CompStudiable compStudiable = t.TryGetComp<CompStudiable>();
			return compStudiable != null && !compStudiable.Completed;
		}

		// Token: 0x060072AA RID: 29354 RVA: 0x00262A5F File Offset: 0x00260C5F
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.AddDesignation(new Designation(t, this.Designation));
		}

		// Token: 0x060072AB RID: 29355 RVA: 0x00264390 File Offset: 0x00262590
		private IEnumerable<Thing> StudyThingsInCell(IntVec3 c)
		{
			if (c.Fogged(base.Map))
			{
				yield break;
			}
			List<Thing> thingList = c.GetThingList(base.Map);
			int num;
			for (int i = 0; i < thingList.Count; i = num + 1)
			{
				if (this.CanDesignateThing(thingList[i]).Accepted)
				{
					yield return thingList[i];
				}
				num = i;
			}
			yield break;
		}
	}
}
