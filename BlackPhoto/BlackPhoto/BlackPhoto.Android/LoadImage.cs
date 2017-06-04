using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using BlackPhoto.Droid;
using ColorThiefDotNet;
using Java.IO;
using Java.Util;
using Xamarin.Forms;
using Color = Android.Graphics.Color;
using Environment = Android.OS.Environment;
using File = Java.IO.File;
using ColorThiefDotNet.Forms;

[assembly: Dependency(typeof(LoadImage))]
namespace BlackPhoto.Droid
{
    public class LoadImage : ILoadImage
    {
        public List<ImageInfo> GetImages()
        {
            List<string> deletePaths = new List<string>();
            List<ImageInfo> classList = new List<ImageInfo>();
            File ExtCameraDirectory = new File(Environment.GetExternalStoragePublicDirectory(Environment.DirectoryDcim), "Camera");
            File Ext100ANDRODirectory = new File(Environment.GetExternalStoragePublicDirectory(Environment.DirectoryDcim), "100ANDRO");

            if (Ext100ANDRODirectory.ListFiles().Length == 0)
            {
                File[] files = ExtCameraDirectory.ListFiles();
                foreach (File file in files)
                {
                    if (file.IsFile)
                    {
                        if (file.Name.EndsWith(".jpg") || file.Name.EndsWith(".png"))
                        {
                            Bitmap imgBitmap = BitmapFactory.DecodeFile(ExtCameraDirectory + "/" + file.Name);
                            var colorThief = new ColorThief();
                            var imgColor = colorThief.GetColor(imgBitmap);
                            if (imgColor.IsDark)
                            {
                                MemoryStream ms = new MemoryStream();
                                imgBitmap.Compress(Bitmap.CompressFormat.Jpeg, 20, ms);
                                byte[] imgByte = ms.ToArray();
                                String imgString = Base64.EncodeToString(imgByte, Base64Flags.Default);

                                classList.Add(new ImageInfo() { ImageName = file.Name, ImagePath = file.AbsolutePath, Base64Code = imgString });

                                ms.Dispose();
                                deletePaths.Add(file.Path);
                            }
                        }
                    }
                }
            }
            else
            {
                File[] files = Ext100ANDRODirectory.ListFiles();
                foreach (File file in files)
                {
                    if (file.IsFile)
                    {
                        if (file.Name.EndsWith(".jpg") || file.Name.EndsWith(".png"))
                        {
                            Bitmap imgBitmap = BitmapFactory.DecodeFile(Ext100ANDRODirectory + "/" + file.Name);
                            var colorThief = new ColorThief();
                            var imgColor = colorThief.GetColor(imgBitmap);
                            if (imgColor.IsDark)
                            {
                                MemoryStream ms = new MemoryStream();
                                imgBitmap.Compress(Bitmap.CompressFormat.Jpeg, 20, ms);
                                byte[] imgByte = ms.ToArray();
                                String imgString = Base64.EncodeToString(imgByte, Base64Flags.Default);

                                classList.Add(new ImageInfo() { ImageName = file.Name, ImagePath = file.AbsolutePath, Base64Code = imgString });

                                ms.Dispose();
                                deletePaths.Add(file.Path);
                            }
                        }
                    }
                }
            }
            return classList;
        }

        public string GetScanPath()
        {
            File ExtCameraDirectory = new File(Environment.GetExternalStoragePublicDirectory(Environment.DirectoryDcim), "Camera");
            File Ext100ANDRODirectory = new File(Environment.GetExternalStoragePublicDirectory(Environment.DirectoryDcim), "100ANDRO");

            if (Ext100ANDRODirectory.ListFiles().Length == 0)
            {
                return ExtCameraDirectory.ToString();
            }
            else
            {
                return Ext100ANDRODirectory.ToString();
            }
        }
    }
}