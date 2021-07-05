using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AA0 RID: 2720
	public class PawnRelationWorker
	{
		// Token: 0x060040B9 RID: 16569 RVA: 0x0015DB31 File Offset: 0x0015BD31
		public virtual bool InRelation(Pawn me, Pawn other)
		{
			if (this.def.implied)
			{
				throw new NotImplementedException(this.def + " lacks InRelation implementation.");
			}
			return me.relations.DirectRelationExists(this.def, other);
		}

		// Token: 0x060040BA RID: 16570 RVA: 0x000682C5 File Offset: 0x000664C5
		public virtual float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			return 0f;
		}

		// Token: 0x060040BB RID: 16571 RVA: 0x0015DB68 File Offset: 0x0015BD68
		public virtual void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			if (!this.def.implied)
			{
				generated.relations.AddDirectRelation(this.def, other);
				return;
			}
			throw new NotImplementedException(this.def + " lacks CreateRelation implementation.");
		}

		// Token: 0x060040BC RID: 16572 RVA: 0x0015DBA0 File Offset: 0x0015BDA0
		public float BaseGenerationChanceFactor(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			float num = 1f;
			if (generated.Faction != other.Faction)
			{
				num *= 0.65f;
			}
			if (generated.HostileTo(other))
			{
				num *= 0.7f;
			}
			if (other.Faction != null && other.Faction.IsPlayer && (generated.Faction == null || !generated.Faction.IsPlayer))
			{
				num *= 0.5f;
			}
			if (other.Faction != null && other.Faction.IsPlayer)
			{
				num *= request.ColonistRelationChanceFactor;
			}
			if (other == request.ExtraPawnForExtraRelationChance)
			{
				num *= request.RelationWithExtraPawnChanceFactor;
			}
			TechLevel techLevel = (generated.Faction != null) ? generated.Faction.def.techLevel : TechLevel.Undefined;
			TechLevel techLevel2 = (other.Faction != null) ? other.Faction.def.techLevel : TechLevel.Undefined;
			if (techLevel != TechLevel.Undefined && techLevel2 != TechLevel.Undefined && techLevel != techLevel2)
			{
				num *= 0.85f;
			}
			if ((techLevel.IsNeolithicOrWorse() && !techLevel2.IsNeolithicOrWorse()) || (!techLevel.IsNeolithicOrWorse() && techLevel2.IsNeolithicOrWorse()))
			{
				num *= 0.03f;
			}
			return num;
		}

		// Token: 0x060040BD RID: 16573 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void OnRelationCreated(Pawn firstPawn, Pawn secondPawn)
		{
		}

		// Token: 0x040025A1 RID: 9633
		public PawnRelationDef def;
	}
}
