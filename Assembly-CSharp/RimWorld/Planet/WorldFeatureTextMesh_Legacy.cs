using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200202E RID: 8238
	public class WorldFeatureTextMesh_Legacy : WorldFeatureTextMesh
	{
		// Token: 0x0600AE94 RID: 44692 RVA: 0x0007184E File Offset: 0x0006FA4E
		private static void TextScaleFactor_Changed()
		{
			Find.WorldFeatures.textsCreated = false;
		}

		// Token: 0x170019AC RID: 6572
		// (get) Token: 0x0600AE95 RID: 44693 RVA: 0x000718D2 File Offset: 0x0006FAD2
		public override bool Active
		{
			get
			{
				return this.textMesh.gameObject.activeInHierarchy;
			}
		}

		// Token: 0x170019AD RID: 6573
		// (get) Token: 0x0600AE96 RID: 44694 RVA: 0x000718E4 File Offset: 0x0006FAE4
		public override Vector3 Position
		{
			get
			{
				return this.textMesh.transform.position;
			}
		}

		// Token: 0x170019AE RID: 6574
		// (get) Token: 0x0600AE97 RID: 44695 RVA: 0x000718F6 File Offset: 0x0006FAF6
		// (set) Token: 0x0600AE98 RID: 44696 RVA: 0x00071903 File Offset: 0x0006FB03
		public override Color Color
		{
			get
			{
				return this.textMesh.color;
			}
			set
			{
				this.textMesh.color = value;
			}
		}

		// Token: 0x170019AF RID: 6575
		// (get) Token: 0x0600AE99 RID: 44697 RVA: 0x00071911 File Offset: 0x0006FB11
		// (set) Token: 0x0600AE9A RID: 44698 RVA: 0x0007191E File Offset: 0x0006FB1E
		public override string Text
		{
			get
			{
				return this.textMesh.text;
			}
			set
			{
				this.textMesh.text = value;
			}
		}

		// Token: 0x170019B0 RID: 6576
		// (set) Token: 0x0600AE9B RID: 44699 RVA: 0x0007192C File Offset: 0x0006FB2C
		public override float Size
		{
			set
			{
				this.textMesh.fontSize = Mathf.RoundToInt(value * WorldFeatureTextMesh_Legacy.TextScaleFactor);
			}
		}

		// Token: 0x170019B1 RID: 6577
		// (get) Token: 0x0600AE9C RID: 44700 RVA: 0x00071945 File Offset: 0x0006FB45
		// (set) Token: 0x0600AE9D RID: 44701 RVA: 0x00071957 File Offset: 0x0006FB57
		public override Quaternion Rotation
		{
			get
			{
				return this.textMesh.transform.rotation;
			}
			set
			{
				this.textMesh.transform.rotation = value;
			}
		}

		// Token: 0x170019B2 RID: 6578
		// (get) Token: 0x0600AE9E RID: 44702 RVA: 0x0007196A File Offset: 0x0006FB6A
		// (set) Token: 0x0600AE9F RID: 44703 RVA: 0x0007197C File Offset: 0x0006FB7C
		public override Vector3 LocalPosition
		{
			get
			{
				return this.textMesh.transform.localPosition;
			}
			set
			{
				this.textMesh.transform.localPosition = value;
			}
		}

		// Token: 0x0600AEA0 RID: 44704 RVA: 0x0007198F File Offset: 0x0006FB8F
		public override void SetActive(bool active)
		{
			this.textMesh.gameObject.SetActive(active);
		}

		// Token: 0x0600AEA1 RID: 44705 RVA: 0x000719A2 File Offset: 0x0006FBA2
		public override void Destroy()
		{
			UnityEngine.Object.Destroy(this.textMesh.gameObject);
		}

		// Token: 0x0600AEA2 RID: 44706 RVA: 0x0032C208 File Offset: 0x0032A408
		public override void Init()
		{
			GameObject gameObject = new GameObject("World feature name (legacy)");
			gameObject.layer = WorldCameraManager.WorldLayer;
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			this.textMesh = gameObject.AddComponent<TextMesh>();
			this.textMesh.color = new Color(1f, 1f, 1f, 0f);
			this.textMesh.anchor = TextAnchor.MiddleCenter;
			this.textMesh.alignment = TextAlignment.Center;
			this.textMesh.GetComponent<MeshRenderer>().sharedMaterial.renderQueue = WorldMaterials.FeatureNameRenderQueue;
			this.Color = new Color(1f, 1f, 1f, 0f);
			this.textMesh.transform.localScale = new Vector3(0.23f, 0.23f, 0.23f);
		}

		// Token: 0x0600AEA3 RID: 44707 RVA: 0x00006A05 File Offset: 0x00004C05
		public override void WrapAroundPlanetSurface()
		{
		}

		// Token: 0x040077D8 RID: 30680
		private TextMesh textMesh;

		// Token: 0x040077D9 RID: 30681
		private const float TextScale = 0.23f;

		// Token: 0x040077DA RID: 30682
		private const int MinFontSize = 13;

		// Token: 0x040077DB RID: 30683
		private const int MaxFontSize = 40;

		// Token: 0x040077DC RID: 30684
		[TweakValue("Interface.World", 0f, 10f)]
		private static float TextScaleFactor = 7.5f;
	}
}
