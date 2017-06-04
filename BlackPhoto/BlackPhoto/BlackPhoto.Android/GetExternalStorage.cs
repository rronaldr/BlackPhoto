using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BlackPhoto.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(GetExternalStorage))]
namespace BlackPhoto.Droid
{
    public class GetExternalStorage : IGetExternalStorage
    {
        public bool VerifyExternalStorage()
        {
            if (Android.OS.Environment.ExternalStorageState.Equals(Android.OS.Environment.MediaMounted))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}