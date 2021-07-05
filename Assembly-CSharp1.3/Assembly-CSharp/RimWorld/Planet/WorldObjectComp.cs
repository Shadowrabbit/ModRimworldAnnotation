using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017FE RID: 6142
	public abstract class WorldObjectComp
	{
		// Token: 0x1700177C RID: 6012
		// (get) Token: 0x06008F5F RID: 36703 RVA: 0x0033595D File Offset: 0x00333B5D
		public IThingHolder ParentHolder
		{
			get
			{
				return this.parent.ParentHolder;
			}
		}

		// Token: 0x1700177D RID: 6013
		// (get) Token: 0x06008F60 RID: 36704 RVA: 0x0033596C File Offset: 0x00333B6C
		public bool ParentHasMap
		{
			get
			{
				MapParent mapParent = this.parent as MapParent;
				return mapParent != null && mapParent.HasMap;
			}
		}

		// Token: 0x06008F61 RID: 36705 RVA: 0x00335990 File Offset: 0x00333B90
		public virtual void Initialize(WorldObjectCompProperties props)
		{
			this.props = props;
		}

		// Token: 0x06008F62 RID: 36706 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void CompTick()
		{
		}

		// Token: 0x06008F63 RID: 36707 RVA: 0x00335999 File Offset: 0x00333B99
		public virtual IEnumerable<Gizmo> GetGizmos()
		{
			yield break;
		}

		// Token: 0x06008F64 RID: 36708 RVA: 0x003359A2 File Offset: 0x00333BA2
		public virtual IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			yield break;
		}

		// Token: 0x06008F65 RID: 36709 RVA: 0x003359AB File Offset: 0x00333BAB
		public virtual IEnumerable<FloatMenuOption> GetTransportPodsFloatMenuOptions(IEnumerable<IThingHolder> pods, CompLaunchable representative)
		{
			yield break;
		}

		// Token: 0x06008F66 RID: 36710 RVA: 0x003359B4 File Offset: 0x00333BB4
		public virtual IEnumerable<Gizmo> GetCaravanGizmos(Caravan caravan)
		{
			yield break;
		}

		// Token: 0x06008F67 RID: 36711 RVA: 0x003359BD File Offset: 0x00333BBD
		public virtual IEnumerable<IncidentTargetTagDef> IncidentTargetTags()
		{
			yield break;
		}

		// Token: 0x06008F68 RID: 36712 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PostDrawExtraSelectionOverlays()
		{
		}

		// Token: 0x06008F69 RID: 36713 RVA: 0x00002688 File Offset: 0x00000888
		public virtual string CompInspectStringExtra()
		{
			return null;
		}

		// Token: 0x06008F6A RID: 36714 RVA: 0x00002688 File Offset: 0x00000888
		public virtual string GetDescriptionPart()
		{
			return null;
		}

		// Token: 0x06008F6B RID: 36715 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PostPostRemove()
		{
		}

		// Token: 0x06008F6C RID: 36716 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PostDestroy()
		{
		}

		// Token: 0x06008F6D RID: 36717 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PostMyMapRemoved()
		{
		}

		// Token: 0x06008F6E RID: 36718 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PostMapGenerate()
		{
		}

		// Token: 0x06008F6F RID: 36719 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PostCaravanFormed(Caravan caravan)
		{
		}

		// Token: 0x06008F70 RID: 36720 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PostExposeData()
		{
		}

		// Token: 0x06008F71 RID: 36721 RVA: 0x003359C8 File Offset: 0x00333BC8
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

		// Token: 0x04005A22 RID: 23074
		public WorldObject parent;

		// Token: 0x04005A23 RID: 23075
		public WorldObjectCompProperties props;
	}
}
