using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001740 RID: 5952
	public class WorldFeatureTextMesh_Legacy : WorldFeatureTextMesh
	{
		// Token: 0x0600895E RID: 35166 RVA: 0x003157AF File Offset: 0x003139AF
		private static void TextScaleFactor_Changed()
		{
			Find.WorldFeatures.textsCreated = false;
		}

		// Token: 0x1700164C RID: 5708
		// (get) Token: 0x0600895F RID: 35167 RVA: 0x003157BC File Offset: 0x003139BC
		public override bool Active
		{
			get
			{
				return this.textMesh.gameObject.activeInHierarchy;
			}
		}

		// Token: 0x1700164D RID: 5709
		// (get) Token: 0x06008960 RID: 35168 RVA: 0x003157CE File Offset: 0x003139CE
		public override Vector3 Position
		{
			get
			{
				return this.textMesh.transform.position;
			}
		}

		// Token: 0x1700164E RID: 5710
		// (get) Token: 0x06008961 RID: 35169 RVA: 0x003157E0 File Offset: 0x003139E0
		// (set) Token: 0x06008962 RID: 35170 RVA: 0x003157ED File Offset: 0x003139ED
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

		// Token: 0x1700164F RID: 5711
		// (get) Token: 0x06008963 RID: 35171 RVA: 0x003157FB File Offset: 0x003139FB
		// (set) Token: 0x06008964 RID: 35172 RVA: 0x00315808 File Offset: 0x00313A08
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

		// Token: 0x17001650 RID: 5712
		// (set) Token: 0x06008965 RID: 35173 RVA: 0x00315816 File Offset: 0x00313A16
		public override float Size
		{
			set
			{
				this.textMesh.fontSize = Mathf.RoundToInt(value * WorldFeatureTextMesh_Legacy.TextScaleFactor);
			}
		}

		// Token: 0x17001651 RID: 5713
		// (get) Token: 0x06008966 RID: 35174 RVA: 0x0031582F File Offset: 0x00313A2F
		// (set) Token: 0x06008967 RID: 35175 RVA: 0x00315841 File Offset: 0x00313A41
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

		// Token: 0x17001652 RID: 5714
		// (get) Token: 0x06008968 RID: 35176 RVA: 0x00315854 File Offset: 0x00313A54
		// (set) Token: 0x06008969 RID: 35177 RVA: 0x00315866 File Offset: 0x00313A66
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

		// Token: 0x0600896A RID: 35178 RVA: 0x00315879 File Offset: 0x00313A79
		public override void SetActive(bool active)
		{
			this.textMesh.gameObject.SetActive(active);
		}

		// Token: 0x0600896B RID: 35179 RVA: 0x0031588C File Offset: 0x00313A8C
		public override void Destroy()
		{
			UnityEngine.Object.Destroy(this.textMesh.gameObject);
		}

		// Token: 0x0600896C RID: 35180 RVA: 0x003158A0 File Offset: 0x00313AA0
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

		// Token: 0x0600896D RID: 35181 RVA: 0x0000313F File Offset: 0x0000133F
		public override void WrapAroundPlanetSurface()
		{
		}

		// Token: 0x04005721 RID: 22305
		private TextMesh textMesh;

		// Token: 0x04005722 RID: 22306
		private const float TextScale = 0.23f;

		// Token: 0x04005723 RID: 22307
		private const int MinFontSize = 13;

		// Token: 0x04005724 RID: 22308
		private const int MaxFontSize = 40;

		// Token: 0x04005725 RID: 22309
		[TweakValue("Interface.World", 0f, 10f)]
		private static float TextScaleFactor = 7.5f;
	}
}
