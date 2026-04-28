using System;
using System.IO;
using UnityEngine;

public static class DebugSessionLogger
{
    private const string SessionId = "a876a4";
    private static string LogPath => Path.Combine(Directory.GetParent(Application.dataPath).FullName, "debug-a876a4.log");

    public static void Log(string runId, string hypothesisId, string location, string message, string data)
    {
        string safeData = (data ?? string.Empty).Replace("\\", "\\\\").Replace("\"", "\\\"");
        string safeMessage = (message ?? string.Empty).Replace("\\", "\\\\").Replace("\"", "\\\"");
        string safeLocation = (location ?? string.Empty).Replace("\\", "\\\\").Replace("\"", "\\\"");
        string safeRunId = (runId ?? string.Empty).Replace("\\", "\\\\").Replace("\"", "\\\"");
        string safeHypothesisId = (hypothesisId ?? string.Empty).Replace("\\", "\\\\").Replace("\"", "\\\"");
        long ts = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        string id = $"log_{ts}_{Guid.NewGuid().ToString("N").Substring(0, 8)}";

        string line =
            $"{{\"sessionId\":\"{SessionId}\",\"id\":\"{id}\",\"timestamp\":{ts},\"location\":\"{safeLocation}\",\"message\":\"{safeMessage}\",\"data\":{{\"text\":\"{safeData}\"}},\"runId\":\"{safeRunId}\",\"hypothesisId\":\"{safeHypothesisId}\"}}";

        try
        {
            File.AppendAllText(LogPath, line + Environment.NewLine);
        }
        catch
        {
            // ignore logging failures
        }
    }
}
