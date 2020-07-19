using UnityEngine;
using UnityEngine.UI;

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

using AssetPackage;

public class ButtonScript : MonoBehaviour
public RawImage rawimage;
public Text emotions;
public Text msg;
WebCamTexture webcam;
Texture2D output;
Color32[] data;
void Start()
{
    WebCamDevice[] devices = WebCamTexture.devices;
    for (int i = 0; i < devices.Length; i++)
    {
      print("Webcam available: " + devices[i].name);
    }
     webcam = new WebCamTexture(640, 480);
     rawimage.texture = webcam;
     rawimage.material.mainTexture = webcam;
     data = new Color32[webcam.width * webcam.height];
     eda = new EmotionDetectionAsset();
     eda.Bridge = new dlib_csharp.Bridge();
     eda.Initialize(@"Assets\", database);
     String[] lines = File.ReadAllLines(furia);
     eda.ParseRules(lines);
     Debug.Log("Emotion detection Ready for Use");
    }
Int32 frames = 0;
void Update()
 {
 if (data != null && eda != null && (++frames) % 5 > 0)
 {
 
 webcam.GetPixels32(data);
 ProcessColor32(data, webcam.width, webcam.height);
 frames = 0;
 }
 }
 const String face3 = @"Assets\Kiavash1.jpg";
 const String furia = @"Assets\FURIA Fuzzy Logic Rules.txt";
 const String database = @"Assets\shape_predictor_68_face_landmarks.dat";
 EmotionDetectionAsset eda;
 public static Texture2D LoadPNG(string filePath)
 {
 Texture2D tex = null;
 byte[] fileData;

 if (File.Exists(filePath))
 {
 fileData = File.ReadAllBytes(filePath);
 tex = new Texture2D(640, 864, TextureFormat.RGBA32, false);
 tex.LoadImage(fileData); 
 }
 return tex;
 }
 private static byte[] Color32ArrayToByteArray(UnityEngine.Color32[] colors)
 {
 if (colors == null || colors.Length == 0)
 return null;

 int lengthOfColor32 = Marshal.SizeOf(typeof(UnityEngine.Color32));
 int length = lengthOfColor32 * colors.Length;
 byte[] bytes = new byte[length];

 GCHandle handle = default(GCHandle);
 try
 {
 handle = GCHandle.Alloc(colors, GCHandleType.Pinned);
 IntPtr ptr = handle.AddrOfPinnedObject();
 Marshal.Copy(ptr, bytes, 0, length);
 }
 finally
 {
 if (handle != default(GCHandle))
 handle.Free();
 }

 return bytes;
 }
 public void onClick()
 {
 onClickBitmap();
 }
 public void onClickTexture()
{
 Int32 avg = (eda.Settings as EmotionDetectionAssetSettings).Average;
 Boolean spike = (eda.Settings as EmotionDetectionAssetSettings).
 SuppressSpikes;
 Texture2D texture = LoadPNG(face3);
 ProcessTexture(texture);
 (eda.Settings as EmotionDetectionAssetSettings).Average = avg;
 (eda.Settings as EmotionDetectionAssetSettings).SuppressSpikes = spike;
 }
 public void onClickBitmap()
 {
 Int32 avg = (eda.Settings as EmotionDetectionAssetSettings).Average;
 Boolean spike = (eda.Settings as EmotionDetectionAssetSettings).
 SuppressSpikes;
 System.Drawing.Image img = System.Drawing.Image.
 FromFile(@"Assets\dump.bmp");
 ProcessImage(img);
 (eda.Settings as EmotionDetectionAssetSettings).Average = avg;
 (eda.Settings as EmotionDetectionAssetSettings).SuppressSpikes = spike;
 }
 private void ProcessTexture(Texture2D texture)
 {
 Rect r = new Rect(0, 0, texture.width, texture.height);
 Sprite sprite = Sprite.Create(texture, r, Vector2.zero);
 GetComponent<UnityEngine.UI.Image>().sprite = sprite;
 (UnityEngine.UI.Image)(GameObject.Find("anmimage"));
 ProcessColor32(texture.GetPixels32(), texture.width, texture.height);
 }
 private void ProcessColor32(Color32[] pixels, Int32 width, Int32 height)
 {
 byte[] raw = Color32ArrayToByteArray(pixels);
 (eda.Settings as EmotionDetectionAssetSettings).Average = 1;
 (eda.Settings as EmotionDetectionAssetSettings).SuppressSpikes = false;
 if (eda.ProcessImage(raw, width, height, true))
 {
 msg.text = String.Format("{0} Face(s detected.", eda.Faces.Count);
 if (eda.ProcessFaces())
 {
 if(eda.ProcessLandmarks())
 {
 Dictionary<string, double> emos = new Dictionary<string, double>();
 foreach (String emo in eda.Emotions)
 {
 emos[emo] = eda[0, emo];
 }
 emotions.text = String.Join("\r\n", emos.OrderBy(p => p.Key).
 Select(p => String.Format("{0}={1:0.00}", p.Key, p.Value)).ToArray());
 }
 else
 {
 emotions.text = "No emotions detected";
 }
 }
 else
 {
 emotions.text = "No landmarks detected";
 }
 }
 else
 {
 msg.text = "No Face(s) detected";
 }
 }
 public void onClickImage()
 {
 System.Drawing.Image img = System.Drawing.Image.FromFile(face3);

 ProcessImage(img);
 }
 private void ProcessImage(System.Drawing.Image img)
 {
 Rect r = new Rect(0, 0, img.Width, img.Height);

 (eda.Settings as EmotionDetectionAssetSettings).Average = 1;
 (eda.Settings as EmotionDetectionAssetSettings).SuppressSpikes = false;

 if (eda.ProcessImage(img))
 {
 msg.text = String.Format("{0} Face(s detected.", eda.Faces.Count);

 if (eda.ProcessFaces())
 {
 if (eda.ProcessLandmarks())
 {
 Dictionary<string, double> emos = new Dictionary<string, double>();

 foreach (String emo in eda.Emotions)
 {
 emos[emo] = eda[0, emo];
 }

 emotions.text = String.Join("\r\n", emos.OrderBy(p => p.Key).
 Select(p => String.Format("{0}={1:0.00}", p.Key, p.Value)).ToArray());
 }
 else
 {
 msg.text = "No emotions detected";
 }
 }
 else
 {
 msg.text = "No landmarks detected";
 }
 }
 else
 {
 msg.text = "No Face(s) detected";
 }}}