using UnityEditor;
using UnityEngine;
using System.Diagnostics;
using System.IO;

/// <summary>
/// 音频转换小工具
/// 需要用到 ffmpeg !!!!
/// 命令行安装: `brew install ffmpeg`
/// 安装后验证： `ffmpeg -version`
/// </summary>
public class AudioConverter : EditorWindow
{
    [MenuItem("Tools/Audio/Convert MP3 to OGG")]
    public static void ConvertMp3ToOgg()
    {
        string folder = EditorUtility.OpenFolderPanel("选择包含 MP3 的文件夹", "Assets", "");
        if (string.IsNullOrEmpty(folder)) return;

        string[] files = Directory.GetFiles(folder, "*.mp3", SearchOption.AllDirectories);
        if (files.Length == 0)
        {
            EditorUtility.DisplayDialog("提示", "该文件夹中没有 MP3 文件。", "确定");
            return;
        }

        foreach (var file in files)
        {
            string output = Path.ChangeExtension(file, ".ogg");

            Process ffmpeg = new Process();
            ffmpeg.StartInfo.FileName = "/opt/homebrew/bin/ffmpeg";
            ffmpeg.StartInfo.Arguments = $"-y -i \"{file}\" -c:a libvorbis \"{output}\"";
            ffmpeg.StartInfo.CreateNoWindow = true;
            ffmpeg.StartInfo.UseShellExecute = false;
            ffmpeg.StartInfo.RedirectStandardError = true;
            ffmpeg.Start();
            ffmpeg.WaitForExit();
        }

        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("完成", $"已转换 {files.Length} 个文件为 OGG 格式。", "确定");
    }
}