using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ArtInteraction : BaseInteraction
{
    [System.Serializable]
    private struct ArtworkSeedData
    {
        public string title;
        public string artist;
        public string year;
        public string style;
        public string intro;

        public ArtworkSeedData(string t, string a, string y, string s, string i)
        {
            title = t;
            artist = a;
            year = y;
            style = s;
            intro = i;
        }
    }

    private static readonly ArtworkSeedData[] SeedCatalog =
    {
        new ArtworkSeedData("\u661f\u591c", "\u6587\u68ee\u7279\u00b7\u51e1\u9ad8", "1889", "\u540e\u5370\u8c61\u4e3b\u4e49", "\u6f29\u6da1\u822c\u7684\u661f\u7a7a\u4e0e\u6751\u5e84\uff0c\u5f3a\u70c8\u8868\u8fbe\u5185\u5fc3\u60c5\u7eea\u3002"),
        new ArtworkSeedData("\u5411\u65e5\u8475", "\u6587\u68ee\u7279\u00b7\u51e1\u9ad8", "1888", "\u540e\u5370\u8c61\u4e3b\u4e49", "\u4ee5\u9ad8\u9971\u548c\u9ec4\u8272\u4e3a\u4e3b\u8c03\uff0c\u5c55\u73b0\u751f\u547d\u529b\u3002"),
        new ArtworkSeedData("\u8499\u5a1c\u4e3d\u838e", "\u8fbe\u82ac\u5947", "1503", "\u6587\u827a\u590d\u5174", "\u4ee5\u7ec6\u817b\u7b14\u89e6\u548c\u795e\u79d8\u5fae\u7b11\u8457\u79f0\u4e8e\u4e16\u3002"),
        new ArtworkSeedData("\u73cd\u73e0\u8033\u73af\u5c11\u5973", "\u7ea6\u7ff0\u5185\u65af\u00b7\u7ef4\u7c73\u5c14", "1665", "\u8377\u5170\u9ec4\u91d1\u65f6\u4ee3", "\u5149\u5f71\u4e0e\u6ce8\u89c6\u6784\u6210\u5b89\u9759\u53c8\u96bd\u6c38\u7684\u77ac\u95f4\u3002"),
        new ArtworkSeedData("\u5436\u558a", "\u7231\u5fb7\u534e\u00b7\u8499\u514b", "1893", "\u8868\u73b0\u4e3b\u4e49", "\u5f2f\u66f2\u7684\u5929\u9645\u7ebf\u4e0e\u4eba\u7269\u5f62\u8c61\u5f15\u53d1\u5f3a\u70c8\u60c5\u7eea\u5171\u9e23\u3002"),
        new ArtworkSeedData("\u65e5\u51fa\u00b7\u5370\u8c61", "\u514b\u52b3\u5fb7\u00b7\u83ab\u5948", "1872", "\u5370\u8c61\u4e3b\u4e49", "\u8f7b\u5feb\u7b14\u89e6\u6355\u6349\u6e2f\u53e3\u6668\u96fe\u4e0e\u5149\u8272\u3002"),
        new ArtworkSeedData("\u7761\u83b2", "\u514b\u52b3\u5fb7\u00b7\u83ab\u5948", "1906", "\u5370\u8c61\u4e3b\u4e49", "\u6c34\u9762\u53cd\u5c04\u4e0e\u82b1\u53f6\u5c42\u53e0\uff0c\u8425\u9020\u51a5\u60f3\u822c\u8282\u594f\u3002"),
        new ArtworkSeedData("\u62ff\u7834\u4ed1\u52a0\u5195", "\u96c5\u514b\u00b7\u8def\u6613\u00b7\u5927\u536b", "1807", "\u65b0\u53e4\u5178\u4e3b\u4e49", "\u5de8\u5e45\u5386\u53f2\u753b\uff0c\u7a81\u51fa\u6743\u529b\u4e0e\u79e9\u5e8f\u3002"),
        new ArtworkSeedData("\u683c\u5c14\u5c3c\u5361", "\u5df4\u52c3\u7f57\u00b7\u6bd5\u52a0\u7d22", "1937", "\u7acb\u4f53\u4e3b\u4e49", "\u4ee5\u5f3a\u70c8\u9ed1\u767d\u5bf9\u6bd4\u63ed\u793a\u6218\u4e89\u521b\u4f24\u3002"),
        new ArtworkSeedData("\u821e\u8e48", "\u4ea8\u5229\u00b7\u9a6c\u8482\u65af", "1910", "\u91ce\u517d\u4e3b\u4e49", "\u7b80\u7ec3\u8272\u9762\u4e0e\u97f5\u5f8b\u66f2\u7ebf\u8868\u8fbe\u751f\u547d\u6b22\u8fbe\u3002"),
        new ArtworkSeedData("\u81ea\u7531\u5f15\u5bfc\u4eba\u6c11", "\u6b27\u4e01\u00b7\u5fb7\u62c9\u514b\u7f57\u74e6", "1830", "\u6d6a\u6f2b\u4e3b\u4e49", "\u5973\u6027\u4eba\u7269\u5316\u8eab\u81ea\u7531\uff0c\u753b\u9762\u5145\u6ee1\u53f2\u8bd7\u6c14\u8d28\u3002"),
        new ArtworkSeedData("\u9752\u9a91\u58eb", "\u5eb7\u4e01\u65af\u57fa", "1903", "\u8868\u73b0\u4e3b\u4e49", "\u9a91\u58eb\u5728\u539f\u91ce\u4e2d\u98de\u9a70\uff0c\u8272\u5f69\u5bcc\u6709\u97f3\u4e50\u6027\u3002")
    };
    [Header("Artwork Metadata")]
    [SerializeField] private string artworkId;
    [SerializeField] private string artworkTitle = "\u672a\u547d\u540d\u4f5c\u54c1";
    [SerializeField] private string artistName = "\u672a\u77e5\u4f5c\u8005";
    [SerializeField] private string year = "\u672a\u77e5\u5e74\u4ee3";
    [SerializeField] private string style = "\u672a\u5206\u7c7b";
    [TextArea(2, 5)]
    [SerializeField] private string description = "\u6682\u65e0\u4f5c\u54c1\u4ecb\u7ecd\u3002";

    public Vector3 startPosition;
    public Vector3 startRotation;

    public bool isLooking=false;

    public float rotateSpeed;

    public Transform player;
    public Transform artParent;

    private float xRotation;
    private float yRotation;

    public float dis;

    public string ArtworkId
    {
        get
        {
            if (string.IsNullOrEmpty(artworkId))
            {
                artworkId = gameObject.name;
            }

            return artworkId;
        }
    }

    public string DisplayTitle => string.IsNullOrEmpty(artworkTitle) ? gameObject.name : artworkTitle;
    public string ArtistName => artistName;
    public string Year => year;
    public string Style => style;
    public string Description => description;

    protected override void BaseAwake()
    {
        base.BaseAwake();
        if (string.IsNullOrEmpty(artworkId))
        {
            artworkId = gameObject.name;
        }

        ApplyAutoMetadataIfNeeded();
    }

    protected override void BaseStart()
    {
        base.BaseStart();
        startPosition = transform.position;
        startRotation = transform.eulerAngles;
    }

    protected override void BaseOnEnable()
    {
        base.BaseOnEnable();
        ExhibitionRuntimeUI.Instance.RegisterArtwork(this);
    }

    protected override void BaseOnDisable()
    {
        base.BaseOnDisable();
        ExhibitionRuntimeUI.Instance.UnregisterArtwork(this);
    }

    protected override void BaseUpdate()
    {
        base.BaseUpdate();
        if (isLooking)
        {
            RatatePaint();
        }

        if (isLooking && Input.GetKeyDown(KeyCode.F))
        {
            ExhibitionProgressManager.Instance.ToggleFavorite(ArtworkId);
            ExhibitionRuntimeUI.Instance.RefreshFocusedArtwork(this);
        }
    }

    public override void Interact()
    {
        base.Interact();
        if (isLooking)
        {
            PutDownPaint();
            
        }
        else
        {
            PickUpPaint();
        }
    }

    protected virtual void PickUpPaint()
    {
        isLooking = true;
        PlayerControllerManager.Instance.isCanController = false;

        transform.parent = player;
        transform.localPosition = new Vector3(0, 0, dis);

        ExhibitionProgressManager.Instance.MarkVisited(ArtworkId);
        ExhibitionRuntimeUI.Instance.ShowArtworkDetail(this);
    }


    protected virtual void PutDownPaint()
    {
        isLooking = false;
        PlayerControllerManager.Instance.isCanController = true;
        
        transform.parent = artParent;
        transform.localPosition = startPosition;
        transform.eulerAngles = startRotation;
        xRotation = 0;
        yRotation = 0;
        ExhibitionRuntimeUI.Instance.HideArtworkDetail();
    }

    private void RatatePaint()
    {
        float horizontal = Input.GetAxis("Horizontal"); // A/D ?? ??????
        float vertical = Input.GetAxis("Vertical");     // W/S ?? ??????

        xRotation += horizontal * rotateSpeed * Time.deltaTime; // ?????????Y??
        yRotation += -vertical * rotateSpeed * Time.deltaTime; // ?????????X??

        transform.eulerAngles = new Vector3(yRotation, xRotation, 0);
    }

    public override void Select()
    {
        base.Select();
        ExhibitionRuntimeUI.Instance.ShowFocusedArtwork(this);
    }

    public override void CancelSelect()
    {
        base.CancelSelect();
        ExhibitionRuntimeUI.Instance.HideFocusedArtwork(this);
    }

    protected override void ChangeInformationText()
    {
        bool isFavorite = ExhibitionProgressManager.Instance.IsFavorite(ArtworkId);
        string favoriteText = isFavorite ? "\u5df2\u6536\u85cf" : "\u6309F\u6536\u85cf";
        InteractionButtonManager.Instance.ShowButton($"\u6309E\u67e5\u770b\u4f5c\u54c1 | {favoriteText}");
    }

    public string BuildDetailText()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine(DisplayTitle);
        sb.AppendLine($"\u4f5c\u8005: {ArtistName}");
        sb.AppendLine($"\u5e74\u4ee3: {Year}");
        sb.AppendLine($"\u98ce\u683c: {Style}");
        sb.AppendLine();
        sb.AppendLine(Description);
        sb.AppendLine();
        sb.AppendLine("\u64cd\u4f5c: WASD\u65cb\u8f6c | E\u653e\u4e0b | F\u6536\u85cf");
        return sb.ToString();
    }

    private void ApplyAutoMetadataIfNeeded()
    {
        bool needTitle = IsPlaceholder(artworkTitle);
        bool needArtist = IsPlaceholder(artistName);
        bool needYear = IsPlaceholder(year);
        bool needStyle = IsPlaceholder(style);
        bool needDesc = IsPlaceholder(description);

        if (!needTitle && !needArtist && !needYear && !needStyle && !needDesc)
        {
            return;
        }

        int seedIndex = Mathf.Abs(GetStableArtworkKey().GetHashCode()) % SeedCatalog.Length;
        ArtworkSeedData seed = SeedCatalog[seedIndex];

        if (needTitle) artworkTitle = seed.title;
        if (needArtist) artistName = seed.artist;
        if (needYear) year = seed.year;
        if (needStyle) style = seed.style;
        if (needDesc) description = seed.intro;
    }

    private string GetStableArtworkKey()
    {
        string objectName = string.IsNullOrEmpty(gameObject.name) ? "Artwork" : gameObject.name;
        string parentName = transform.parent == null ? "Root" : transform.parent.name;
        return $"{parentName}/{objectName}/{transform.GetSiblingIndex()}";
    }

    private static bool IsPlaceholder(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return true;
        }

        string text = value.Trim();
        if (text.Contains("?"))
        {
            return true;
        }

        return text == "\u672a\u547d\u540d\u4f5c\u54c1"
            || text == "\u672a\u77e5\u4f5c\u8005"
            || text == "\u672a\u77e5\u5e74\u4ee3"
            || text == "\u672a\u5206\u7c7b"
            || text == "\u6682\u65e0\u4f5c\u54c1\u4ecb\u7ecd\u3002";
    }
}
