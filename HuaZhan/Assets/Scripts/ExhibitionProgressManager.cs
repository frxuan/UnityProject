using System.Collections.Generic;
using UnityEngine;

public class ExhibitionProgressManager : MonoBehaviour
{
    private static ExhibitionProgressManager _instance;
    public static ExhibitionProgressManager Instance
    {
        get
        {
            if (_instance == null)
            {
                #region agent log
                DebugSessionLogger.Log("run1", "H3", "ExhibitionProgressManager.cs:14", "Instance getter creating host", "progress manager instance was null");
                #endregion
                GameObject host = new GameObject("ExhibitionProgressManager");
                _instance = host.AddComponent<ExhibitionProgressManager>();
            }

            return _instance;
        }
    }

    private readonly HashSet<string> _visitedIds = new HashSet<string>();
    private readonly HashSet<string> _favoriteIds = new HashSet<string>();

    private string VisitedKey => $"{UserSession.CurrentUser}_VisitedIds";
    private string FavoriteKey => $"{UserSession.CurrentUser}_FavoriteIds";

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
        LoadData();
    }

    public void MarkVisited(string artworkId)
    {
        if (string.IsNullOrEmpty(artworkId))
        {
            return;
        }

        if (_visitedIds.Add(artworkId))
        {
            SaveData();
        }

        ExhibitionRuntimeUI.Instance.RefreshProgress();
    }

    public bool IsVisited(string artworkId)
    {
        return !string.IsNullOrEmpty(artworkId) && _visitedIds.Contains(artworkId);
    }

    public void ToggleFavorite(string artworkId)
    {
        if (string.IsNullOrEmpty(artworkId))
        {
            return;
        }

        if (_favoriteIds.Contains(artworkId))
        {
            _favoriteIds.Remove(artworkId);
        }
        else
        {
            _favoriteIds.Add(artworkId);
        }

        SaveData();
        ExhibitionRuntimeUI.Instance.RefreshProgress();
    }

    public bool IsFavorite(string artworkId)
    {
        return !string.IsNullOrEmpty(artworkId) && _favoriteIds.Contains(artworkId);
    }

    public int GetVisitedCount()
    {
        return _visitedIds.Count;
    }

    public int GetFavoriteCount()
    {
        return _favoriteIds.Count;
    }

    public IReadOnlyCollection<string> GetFavorites()
    {
        return _favoriteIds;
    }

    public void ReloadForCurrentUser()
    {
        #region agent log
        DebugSessionLogger.Log("run1", "H3", "ExhibitionProgressManager.cs:106", "ReloadForCurrentUser entered", $"user={UserSession.CurrentUser}");
        #endregion
        _visitedIds.Clear();
        _favoriteIds.Clear();
        LoadData();
        #region agent log
        DebugSessionLogger.Log("run1", "H3", "ExhibitionProgressManager.cs:112", "About to call runtime UI refresh", $"runtimeUiNull={IsRuntimeUiNull()}");
        #endregion
        ExhibitionRuntimeUI.Instance.RefreshProgress();
    }

    private bool IsRuntimeUiNull()
    {
        return FindObjectOfType<ExhibitionRuntimeUI>() == null;
    }

    private void SaveData()
    {
        PlayerPrefs.SetString(VisitedKey, string.Join("|", _visitedIds));
        PlayerPrefs.SetString(FavoriteKey, string.Join("|", _favoriteIds));
        PlayerPrefs.Save();
    }

    private void LoadData()
    {
        LoadSetFromKey(VisitedKey, _visitedIds);
        LoadSetFromKey(FavoriteKey, _favoriteIds);
    }

    private static void LoadSetFromKey(string key, HashSet<string> set)
    {
        set.Clear();
        string raw = PlayerPrefs.GetString(key, string.Empty);
        if (string.IsNullOrEmpty(raw))
        {
            return;
        }

        string[] items = raw.Split('|');
        for (int i = 0; i < items.Length; i++)
        {
            string id = items[i].Trim();
            if (!string.IsNullOrEmpty(id))
            {
                set.Add(id);
            }
        }
    }
}
