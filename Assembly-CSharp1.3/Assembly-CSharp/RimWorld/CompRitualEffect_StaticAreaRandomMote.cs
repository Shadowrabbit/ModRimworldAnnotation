using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FBE RID: 4030
	public class CompRitualEffect_StaticAreaRandomMote : CompRitualEffect_Constant
	{
		// Token: 0x17001051 RID: 4177
		// (get) Token: 0x06005F03 RID: 24323 RVA: 0x0020829B File Offset: 0x0020649B
		protected CompProperties_RitualEffectStaticAreaRandomMote Props
		{
			get
			{
				return (CompProperties_RitualEffectStaticAreaRandomMote)this.props;
			}
		}

		// Token: 0x06005F04 RID: 24324 RVA: 0x000FE248 File Offset: 0x000FC448
		protected override Vector3 SpawnPos(LordJob_Ritual ritual)
		{
			return Vector3.zero;
		}

		// Token: 0x17001052 RID: 4178
		// (get) Token: 0x06005F05 RID: 24325 RVA: 0x002082A8 File Offset: 0x002064A8
		protected override ThingDef MoteDef
		{
			get
			{
				return this.Props.moteDefs.RandomElement<ThingDef>();
			}
		}

		// Token: 0x06005F06 RID: 24326 RVA: 0x002082BC File Offset: 0x002064BC
		public override void OnSetup(RitualVisualEffect parent, LordJob_Ritual ritual, bool loading)
		{
			this.parent = parent;
			CellRect cellRect = CellRect.CenteredOn(ritual.selectedTarget.Cell, this.Props.area.x / 2, this.Props.area.z / 2).ClipInsideMap(ritual.Map);
			List<IntVec3> list = new List<IntVec3>();
			for (int i = 0; i < this.Props.spawnCount; i++)
			{
				IntVec3 pos = IntVec3.Invalid;
				Predicate<IntVec3> <>9__0;
				for (int j = 0; j < 15; j++)
				{
					pos = cellRect.RandomCell;
					List<IntVec3> list2 = list;
					Predicate<IntVec3> predicate;
					if ((predicate = <>9__0) == null)
					{
						predicate = (<>9__0 = ((IntVec3 c) => c.InHorDistOf(pos, this.Props.minDist)));
					}
					if (!list2.Any(predicate))
					{
						break;
					}
				}
				if (pos.IsValid)
				{
					Mote mote = this.SpawnMote(ritual, new Vector3?(pos.ToVector3Shifted() + this.Props.offset));
					if (mote != null)
					{
						parent.AddMoteToMaintain(mote);
					}
					list.Add(pos);
				}
			}
		}
	}
}
