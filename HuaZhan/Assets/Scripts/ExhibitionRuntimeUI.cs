using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ExhibitionRuntimeUI : MonoBehaviour
{
    private static ExhibitionRuntimeUI _instance;
    public static ExhibitionRuntimeUI Instance
    {
        get
        {
            if (_instance == null)
            {
                #region agent log
                DebugSessionLogger.Log("run1", "H2", "ExhibitionRuntimeUI.cs:15", "Instance getter creating host", "runtimeUI instance was null");
                #endregion
                GameObject host = new GameObject("ExhibitionRuntimeUI");
                _instance = host.AddComponent<ExhibitionRuntimeUI>();
            }

            return _instance;
        }
    }

    private readonly List<ArtInteraction> _allArtworks = new List<ArtInteraction>();

    private Canvas _canvas;
    private Text _focusText;
    private Text _detailText;
    private Text _guideText;
    private Text _progressText;
    private Text _achievementText;
    private GameObject _guidePanel;

    private ArtInteraction _focusedArtwork;
    private int _guideIndex = -1;
    private bool _guideVisible;
    private bool _achievementShown;
    private bool _uiBuilt;
    private Font _runtimeFont;

    private void Awake()
    {
        #region agent log
        DebugSessionLogger.Log("run1", "H2", "ExhibitionRuntimeUI.cs:43", "Awake entered", $"self={name}, existingInstanceNull={(_instance == null)}");
        #endregion
        if (_instance != null && _instance != this)
        {
            #region agent log
            DebugSessionLogger.Log("run1", "H2", "ExhibitionRuntimeUI.cs:47", "Awake destroying duplicate instance", $"self={name}, instance={_instance.name}");
            #endregion
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
        TryBuildUI("awake");
        #region agent log
        DebugSessionLogger.Log("run1", "H1", "ExhibitionRuntimeUI.cs:57", "Awake finished BuildUI", $"progressTextNull={(_progressText == null)}, achievementTextNull={(_achievementText == null)}");
        #endregion
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            _guideVisible = !_guideVisible;
            _guidePanel.SetActive(_guideVisible);
            RefreshGuidePanel();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            CycleGuideTarget();
        }

        if (_guideVisible)
        {
            RefreshGuidePanel();
        }
    }

    public void RegisterArtwork(ArtInteraction artwork)
    {
        if (artwork == null || _allArtworks.Contains(artwork))
        {
            return;
        }

        _allArtworks.Add(artwork);
        RefreshProgress();
        RefreshGuidePanel();
    }

    public void UnregisterArtwork(ArtInteraction artwork)
    {
        if (artwork == null)
        {
            return;
        }

        _allArtworks.Remove(artwork);
        if (_focusedArtwork == artwork)
        {
            _focusedArtwork = null;
            _focusText.text = string.Empty;
        }

        RefreshProgress();
        RefreshGuidePanel();
    }

    public void ShowFocusedArtwork(ArtInteraction artwork)
    {
        _focusedArtwork = artwork;
        RefreshFocusedArtwork(artwork);
    }

    public void RefreshFocusedArtwork(ArtInteraction artwork)
    {
        if (artwork == null)
        {
            _focusText.text = string.Empty;
            return;
        }

        bool isFavorite = ExhibitionProgressManager.Instance.IsFavorite(artwork.ArtworkId);
        _focusText.text = $"{artwork.DisplayTitle} | {artwork.ArtistName} | {(isFavorite ? "已收藏" : "未收藏")}";
    }

    public void HideFocusedArtwork(ArtInteraction artwork)
    {
        if (_focusedArtwork != artwork)
        {
            return;
        }

        _focusedArtwork = null;
        _focusText.text = string.Empty;
    }

    public void ShowArtworkDetail(ArtInteraction artwork)
    {
        _detailText.text = artwork == null ? string.Empty : artwork.BuildDetailText();
    }

    public void HideArtworkDetail()
    {
        _detailText.text = string.Empty;
    }

    public void RefreshProgress()
    {
        #region agent log
        DebugSessionLogger.Log("run1", "H1", "ExhibitionRuntimeUI.cs:151", "RefreshProgress entered", $"progressTextNull={(_progressText == null)}, achievementTextNull={(_achievementText == null)}, artworkCount={_allArtworks.Count}");
        #endregion
        EnsureUIBuiltForRefresh();
        int total = _allArtworks.Count;
        int visited = ExhibitionProgressManager.Instance.GetVisitedCount();
        int favorite = ExhibitionProgressManager.Instance.GetFavoriteCount();
        _progressText.text = $"参观进度: {visited}/{total}  收藏: {favorite}";

        if (total > 0 && visited >= total)
        {
            _achievementText.text = "成就达成: 你已完成本次虚拟画展参观";
            _achievementShown = true;
        }
        else if (!_achievementShown)
        {
            _achievementText.text = string.Empty;
        }
    }

    private void BuildUI()
    {
        #region agent log
        DebugSessionLogger.Log("run2", "N1", "ExhibitionRuntimeUI.cs:174", "BuildUI start", "creating runtime canvas/texts");
        #endregion
        _canvas = new GameObject("ExhibitionCanvas").AddComponent<Canvas>();
        _canvas.transform.SetParent(transform);
        _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        _canvas.gameObject.AddComponent<CanvasScaler>();
        _canvas.gameObject.AddComponent<GraphicRaycaster>();

        _focusText = CreateText("FocusText", new Vector2(20, -20), TextAnchor.UpperLeft, 18);
        _detailText = CreateText("DetailText", new Vector2(20, -120), TextAnchor.UpperLeft, 18);
        _progressText = CreateText("ProgressText", new Vector2(-20, -20), TextAnchor.UpperRight, 18);
        _achievementText = CreateText("AchievementText", new Vector2(0, -20), TextAnchor.UpperCenter, 24);

        _guidePanel = new GameObject("GuidePanel");
        _guidePanel.transform.SetParent(_canvas.transform, false);
        RectTransform guideRect = _guidePanel.AddComponent<RectTransform>();
        guideRect.anchorMin = new Vector2(1, 0);
        guideRect.anchorMax = new Vector2(1, 0);
        guideRect.pivot = new Vector2(1, 0);
        guideRect.anchoredPosition = new Vector2(-20, 20);
        guideRect.sizeDelta = new Vector2(460, 260);
        _guideText = _guidePanel.AddComponent<Text>();
        _guideText.font = GetRuntimeFont();
        _guideText.fontSize = 16;
        _guideText.alignment = TextAnchor.LowerLeft;
        _guideText.color = Color.white;
        _guideText.horizontalOverflow = HorizontalWrapMode.Wrap;
        _guideText.verticalOverflow = VerticalWrapMode.Overflow;
        _guidePanel.SetActive(false);
        _uiBuilt = true;
        #region agent log
        DebugSessionLogger.Log("run2", "N1", "ExhibitionRuntimeUI.cs:203", "BuildUI complete", $"progressTextNull={(_progressText == null)}, achievementTextNull={(_achievementText == null)}");
        #endregion
    }

    private void TryBuildUI(string reason)
    {
        if (_uiBuilt)
        {
            return;
        }

        try
        {
            BuildUI();
        }
        catch (System.Exception ex)
        {
            #region agent log
            DebugSessionLogger.Log("run2", "N1", "ExhibitionRuntimeUI.cs:219", "BuildUI exception", $"reason={reason}, ex={ex.GetType().Name}:{ex.Message}");
            #endregion
            throw;
        }
    }

    private void EnsureUIBuiltForRefresh()
    {
        if (_progressText != null && _achievementText != null)
        {
            return;
        }

        #region agent log
        DebugSessionLogger.Log("run2", "N2", "ExhibitionRuntimeUI.cs:233", "Refresh detected null ui refs", $"uiBuilt={_uiBuilt}, progressNull={(_progressText == null)}, achievementNull={(_achievementText == null)}");
        #endregion
        TryBuildUI("refresh-progress");
    }

    private Text CreateText(string name, Vector2 anchoredPosition, TextAnchor anchor, int size)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(_canvas.transform, false);
        RectTransform rect = go.AddComponent<RectTransform>();

        if (anchor == TextAnchor.UpperRight)
        {
            rect.anchorMin = new Vector2(1, 1);
            rect.anchorMax = new Vector2(1, 1);
            rect.pivot = new Vector2(1, 1);
        }
        else if (anchor == TextAnchor.UpperCenter)
        {
            rect.anchorMin = new Vector2(0.5f, 1);
            rect.anchorMax = new Vector2(0.5f, 1);
            rect.pivot = new Vector2(0.5f, 1);
        }
        else
        {
            rect.anchorMin = new Vector2(0, 1);
            rect.anchorMax = new Vector2(0, 1);
            rect.pivot = new Vector2(0, 1);
        }

        rect.anchoredPosition = anchoredPosition;
        rect.sizeDelta = new Vector2(700, 300);

        Text text = go.AddComponent<Text>();
        text.font = GetRuntimeFont();
        text.fontSize = size;
        text.alignment = anchor;
        text.color = Color.white;
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.verticalOverflow = VerticalWrapMode.Overflow;
        return text;
    }

    private Font GetRuntimeFont()
    {
        if (_runtimeFont != null)
        {
            return _runtimeFont;
        }

        string[] preferredFonts =
        {
            "Microsoft YaHei",
            "SimHei",
            "SimSun",
            "Arial Unicode MS"
        };

        _runtimeFont = Font.CreateDynamicFontFromOSFont(preferredFonts, 18);
        if (_runtimeFont == null)
        {
            _runtimeFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        }

        return _runtimeFont;
    }

    private void CycleGuideTarget()
    {
        if (_allArtworks.Count == 0)
        {
            _guideIndex = -1;
            return;
        }

        _guideIndex++;
        if (_guideIndex >= _allArtworks.Count)
        {
            _guideIndex = 0;
        }
    }

    private void RefreshGuidePanel()
    {
        if (!_guideVisible)
        {
            return;
        }

        if (_allArtworks.Count == 0)
        {
            _guideText.text = "导览面板: 暂无作品";
            return;
        }

        if (_guideIndex < 0 || _guideIndex >= _allArtworks.Count)
        {
            _guideIndex = 0;
        }

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("导览面板 (M开关, G切换目标)");
        sb.AppendLine("--------------------------------");

        Transform viewer = Camera.main != null ? Camera.main.transform : null;
        for (int i = 0; i < _allArtworks.Count; i++)
        {
            ArtInteraction art = _allArtworks[i];
            string selected = i == _guideIndex ? ">" : " ";
            string visited = ExhibitionProgressManager.Instance.IsVisited(art.ArtworkId) ? "[已看]" : "[未看]";
            string favorite = ExhibitionProgressManager.Instance.IsFavorite(art.ArtworkId) ? "★" : " ";
            float distance = viewer == null ? 0f : Vector3.Distance(viewer.position, art.transform.position);
            sb.AppendLine($"{selected}{favorite} {art.DisplayTitle} {visited} 距离:{distance:F1}m");
        }

        ArtInteraction target = _allArtworks[_guideIndex];
        if (viewer != null && target != null)
        {
            Vector3 dir = target.transform.position - viewer.position;
            float angle = Vector3.SignedAngle(viewer.forward, dir, Vector3.up);
            sb.AppendLine("--------------------------------");
            sb.AppendLine($"当前目标: {target.DisplayTitle}");
            sb.AppendLine($"请转向角度: {angle:F0}°");
        }

        _guideText.text = sb.ToString();
    }
}
