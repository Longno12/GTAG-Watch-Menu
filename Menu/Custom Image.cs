




// Need To Fix






/*using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Watch_Menu.Menu
{
    internal class Custom_Image
    {
        public static Material enderpearl;
        private static bool isImageLoaded = false;

        public static void Initialize()
        {
            LoadCustomImage("https://github.com/Longno12/pics/blob/main/menubg.jpg?raw=true");
        }

        public static IEnumerator LoadCustomImage(string url)
        {
            using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(request);
                    enderpearl = new Material(Shader.Find("GorillaTag/UberShader"));
                    enderpearl.shaderKeywords = new string[] { "_USE_TEXTURE" };
                    enderpearl.mainTexture = texture;
                    isImageLoaded = true;

                    Debug.Log("Custom image loaded successfully!");
                }
                else
                {
                    Debug.LogError($"Failed to load image: {request.error}");
                    isImageLoaded = false;
                }
            }
        }

        public static bool IsImageLoaded()
        {
            return isImageLoaded;
        }
    }
}*/