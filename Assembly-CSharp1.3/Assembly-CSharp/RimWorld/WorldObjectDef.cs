using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AED RID: 2797
	public class WorldObjectDef : Def
	{
		// Token: 0x17000B8E RID: 2958
		// (get) Token: 0x060041C9 RID: 16841 RVA: 0x0016077A File Offset: 0x0015E97A
		public Material Material
		{
			get
			{
				if (this.texture.NullOrEmpty())
				{
					return null;
				}
				if (this.material == null)
				{
					this.material = MaterialPool.MatFrom(this.texture, ShaderDatabase.WorldOverlayTransparentLit, WorldMaterials.WorldObjectRenderQueue);
				}
				return this.material;
			}
		}

		// Token: 0x17000B8F RID: 2959
		// (get) Token: 0x060041CA RID: 16842 RVA: 0x001607BA File Offset: 0x0015E9BA
		public Texture2D ExpandingIconTexture
		{
			get
			{
				if (this.expandingIconTextureInt == null)
				{
					if (this.expandingIconTexture.NullOrEmpty())
					{
						return null;
					}
					this.expandingIconTextureInt = ContentFinder<Texture2D>.Get(this.expandingIconTexture, true);
				}
				return this.expandingIconTextureInt;
			}
		}

		// Token: 0x060041CB RID: 16843 RVA: 0x001607F4 File Offset: 0x0015E9F4
		public override void PostLoad()
		{
			base.PostLoad();
			if (this.inspectorTabs != null)
			{
				for (int i = 0; i < this.inspectorTabs.Count; i++)
				{
					if (this.inspectorTabsResolved == null)
					{
						this.inspectorTabsResolved = new List<InspectTabBase>();
					}
					try
					{
						this.inspectorTabsResolved.Add(InspectTabManager.GetSharedInstance(this.inspectorTabs[i]));
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Could not instantiate inspector tab of type ",
							this.inspectorTabs[i],
							": ",
							ex
						}));
					}
				}
			}
		}

		// Token: 0x060041CC RID: 16844 RVA: 0x001608A0 File Offset: 0x0015EAA0
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].ResolveReferences(this);
			}
		}

		// Token: 0x060041CD RID: 16845 RVA: 0x001608DB File Offset: 0x0015EADB
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			int num;
			for (int i = 0; i < this.comps.Count; i = num + 1)
			{
				foreach (string text2 in this.comps[i].ConfigErrors(this))
				{
					yield return text2;
				}
				enumerator = null;
				num = i;
			}
			if (this.expandMore && !this.expandingIcon)
			{
				yield return "has expandMore but doesn't have any expanding icon";
			}
			yield break;
			yield break;
		}

		// Token: 0x04002810 RID: 10256
		public Type worldObjectClass = typeof(WorldObject);

		// Token: 0x04002811 RID: 10257
		public bool canHaveFaction = true;

		// Token: 0x04002812 RID: 10258
		public bool saved = true;

		// Token: 0x04002813 RID: 10259
		public bool canBePlayerHome;

		// Token: 0x04002814 RID: 10260
		public List<WorldObjectCompProperties> comps = new List<WorldObjectCompProperties>();

		// Token: 0x04002815 RID: 10261
		public bool allowCaravanIncidentsWhichGenerateMap;

		// Token: 0x04002816 RID: 10262
		public bool isTempIncidentMapOwner;

		// Token: 0x04002817 RID: 10263
		public List<IncidentTargetTagDef> IncidentTargetTags;

		// Token: 0x04002818 RID: 10264
		public bool selectable = true;

		// Token: 0x04002819 RID: 10265
		public bool neverMultiSelect;

		// Token: 0x0400281A RID: 10266
		public MapGeneratorDef mapGenerator;

		// Token: 0x0400281B RID: 10267
		public List<Type> inspectorTabs;

		// Token: 0x0400281C RID: 10268
		[Unsaved(false)]
		public List<InspectTabBase> inspectorTabsResolved;

		// Token: 0x0400281D RID: 10269
		public bool useDynamicDrawer;

		// Token: 0x0400281E RID: 10270
		public bool expandingIcon;

		// Token: 0x0400281F RID: 10271
		[NoTranslate]
		public string expandingIconTexture;

		// Token: 0x04002820 RID: 10272
		public float expandingIconPriority;

		// Token: 0x04002821 RID: 10273
		[NoTranslate]
		public string texture;

		// Token: 0x04002822 RID: 10274
		[Unsaved(false)]
		private Material material;

		// Token: 0x04002823 RID: 10275
		[Unsaved(false)]
		private Texture2D expandingIconTextureInt;

		// Token: 0x04002824 RID: 10276
		public bool expandMore;

		// Token: 0x04002825 RID: 10277
		public bool rotateGraphicWhenTraveling;

		// Token: 0x04002826 RID: 10278
		public Color? expandingIconColor;

		// Token: 0x04002827 RID: 10279
		public float expandingIconDrawSize = 1f;

		// Token: 0x04002828 RID: 10280
		public bool blockExitGridUntilBattleIsWon;
	}
}
