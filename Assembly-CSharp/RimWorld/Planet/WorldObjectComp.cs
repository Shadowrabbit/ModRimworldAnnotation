using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020021AA RID: 8618
	public abstract class WorldObjectComp
	{
		// Token: 0x17001B4C RID: 6988
		// (get) Token: 0x0600B820 RID: 47136 RVA: 0x00077763 File Offset: 0x00075963
		public IThingHolder ParentHolder
		{
			get
			{
				return this.parent.ParentHolder;
			}
		}

		// Token: 0x17001B4D RID: 6989
		// (get) Token: 0x0600B821 RID: 47137 RVA: 0x00350010 File Offset: 0x0034E210
		public bool ParentHasMap
		{
			get
			{
				MapParent mapParent = this.parent as MapParent;
				return mapParent != null && mapParent.HasMap;
			}
		}

		// Token: 0x0600B822 RID: 47138 RVA: 0x00077770 File Offset: 0x00075970
		public virtual void Initialize(WorldObjectCompProperties props)
		{
			this.props = props;
		}

		// Token: 0x0600B823 RID: 47139 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void CompTick()
		{
		}

		// Token: 0x0600B824 RID: 47140 RVA: 0x00077779 File Offset: 0x00075979
		public virtual IEnumerable<Gizmo> GetGizmos()
		{
			yield break;
		}

		// Token: 0x0600B825 RID: 47141 RVA: 0x00077782 File Offset: 0x00075982
		public virtual IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			yield break;
		}

		// Token: 0x0600B826 RID: 47142 RVA: 0x0007778B File Offset: 0x0007598B
		public virtual IEnumerable<FloatMenuOption> GetTransportPodsFloatMenuOptions(IEnumerable<IThingHolder> pods, CompLaunchable representative)
		{
			yield break;
		}

		// Token: 0x0600B827 RID: 47143 RVA: 0x00077794 File Offset: 0x00075994
		public virtual IEnumerable<Gizmo> GetCaravanGizmos(Caravan caravan)
		{
			yield break;
		}

		// Token: 0x0600B828 RID: 47144 RVA: 0x0007779D File Offset: 0x0007599D
		public virtual IEnumerable<IncidentTargetTagDef> IncidentTargetTags()
		{
			yield break;
		}

		// Token: 0x0600B829 RID: 47145 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void PostDrawExtraSelectionOverlays()
		{
		}

		// Token: 0x0600B82A RID: 47146 RVA: 0x0000C32E File Offset: 0x0000A52E
		public virtual string CompInspectStringExtra()
		{
			return null;
		}

		// Token: 0x0600B82B RID: 47147 RVA: 0x0000C32E File Offset: 0x0000A52E
		public virtual string GetDescriptionPart()
		{
			return null;
		}

		// Token: 0x0600B82C RID: 47148 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void PostPostRemove()
		{
		}

		// Token: 0x0600B82D RID: 47149 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void PostDestroy()
		{
		}

		// Token: 0x0600B82E RID: 47150 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void PostMyMapRemoved()
		{
		}

		// Token: 0x0600B82F RID: 47151 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void PostMapGenerate()
		{
		}

		// Token: 0x0600B830 RID: 47152 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void PostCaravanFormed(Caravan caravan)
		{
		}

		// Token: 0x0600B831 RID: 47153 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void PostExposeData()
		{
		}

		// Token: 0x0600B832 RID: 47154 RVA: 0x00350034 File Offset: 0x0034E234
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				base.GetType().Name,
				"(parent=",
				this.parent,
				" at=",
				(this.parent != null) ? this.parent.Tile : -1,
				")"
			});
		}

		// Token: 0x04007DCA RID: 32202
		public WorldObject parent;

		// Token: 0x04007DCB RID: 32203
		public WorldObjectCompProperties props;
	}
}
