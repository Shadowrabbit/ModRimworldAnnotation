using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012C0 RID: 4800
	public class Designator_Tame : Designator
	{
		// Token: 0x1700140A RID: 5130
		// (get) Token: 0x060072AC RID: 29356 RVA: 0x0009007E File Offset: 0x0008E27E
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x1700140B RID: 5131
		// (get) Token: 0x060072AD RID: 29357 RVA: 0x002643A7 File Offset: 0x002625A7
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Tame;
			}
		}

		// Token: 0x060072AE RID: 29358 RVA: 0x002643B0 File Offset: 0x002625B0
		public Designator_Tame()
		{
			this.defaultLabel = "DesignatorTame".Translate();
			this.defaultDesc = "DesignatorTameDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Tame", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Claim;
			this.hotKey = KeyBindingDefOf.Misc4;
			this.tutorTag = "Tame";
		}

		// Token: 0x060072AF RID: 29359 RVA: 0x00264447 File Offset: 0x00262647
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (!this.TameablesInCell(c).Any<Pawn>())
			{
				return "MessageMustDesignateTameable".Translate();
			}
			return true;
		}

		// Token: 0x060072B0 RID: 29360 RVA: 0x00264484 File Offset: 0x00262684
		public override void DesignateSingleCell(IntVec3 loc)
		{
			foreach (Pawn t in this.TameablesInCell(loc))
			{
				this.DesignateThing(t);
			}
		}

		// Token: 0x060072B1 RID: 29361 RVA: 0x002644D4 File Offset: 0x002626D4
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			Pawn pawn = t as Pawn;
			return pawn != null && TameUtility.CanTame(pawn) && base.Map.designationManager.DesignationOn(pawn, this.Designation) == null;
		}

		// Token: 0x060072B2 RID: 29362 RVA: 0x00264518 File Offset: 0x00262718
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			using (IEnumerator<PawnKindDef> enumerator = (from p in this.justDesignated
			select p.kindDef).Distinct<PawnKindDef>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PawnKindDef kind = enumerator.Current;
					TameUtility.ShowDesignationWarnings(this.justDesignated.First((Pawn x) => x.kindDef == kind), true);
				}
			}
			this.justDesignated.Clear();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.AnimalTaming, KnowledgeAmount.Total);
		}

		// Token: 0x060072B3 RID: 29363 RVA: 0x002645CC File Offset: 0x002627CC
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.RemoveAllDesignationsOn(t, false);
			base.Map.designationManager.AddDesignation(new Designation(t, this.Designation));
			this.justDesignated.Add((Pawn)t);
		}

		// Token: 0x060072B4 RID: 29364 RVA: 0x0026461D File Offset: 0x0026281D
		private IEnumerable<Pawn> TameablesInCell(IntVec3 c)
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
					yield return (Pawn)thingList[i];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x04003EC5 RID: 16069
		private List<Pawn> justDesignated = new List<Pawn>();
	}
}
