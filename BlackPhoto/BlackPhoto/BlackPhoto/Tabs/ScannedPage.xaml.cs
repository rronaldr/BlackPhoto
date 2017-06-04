using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PCLStorage;
namespace BlackPhoto
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScannedPage : ContentPage
    {
        public ScannedPage()
        {
            InitializeComponent();
            ImgFromBase64(DependencyService.Get<ILoadImage>().GetImages());
        }

        public async void ImgFromBase64(List<ImageInfo> image)
        {
            ObservableCollection<SourceOfImage> sources = new ObservableCollection<SourceOfImage>();
            //List<SourceOfImage> sources = new List<SourceOfImage>();
            foreach (ImageInfo x in image)
            {
                bool scannedExists = await FolderExists("Scanned");
                bool deletedExists = await FolderExists("Deleted");

                if (scannedExists && deletedExists)
                {
                    IList<IFile> listFiles = new List<IFile>();
                    IFolder rootFolder = FileSystem.Current.LocalStorage;
                    IFolder scanned = await rootFolder.GetFolderAsync("Scanned");
                    listFiles = await scanned.GetFilesAsync();

                    //Saving image files to app local directory
                    IFile imgFile = await scanned.CreateFileAsync(x.ImageName, CreationCollisionOption.ReplaceExisting);
                    byte[] imageBytes = Convert.FromBase64String(x.Base64Code);
                    Stream s = await imgFile.OpenAsync(FileAccess.ReadAndWrite);
                    s.Write(imageBytes, 0, imageBytes.Length);
                    s.Dispose();

                    //Displaying images from local directory
                    IFile imgFileLoad = await scanned.GetFileAsync(x.ImageName);
                    Stream loadStream = await imgFileLoad.OpenAsync(FileAccess.Read);
                    sources.Add(new SourceOfImage() { ImageName = x.ImageName, SourceAttr = ImageSource.FromStream((() => loadStream)) });


                }
                else
                {
                    IFolder folder = FileSystem.Current.LocalStorage;
                    IFolder folder1 = FileSystem.Current.LocalStorage;
                    folder = await folder.CreateFolderAsync("Scanned", CreationCollisionOption.ReplaceExisting);
                    folder1 = await folder1.CreateFolderAsync("Deleted", CreationCollisionOption.ReplaceExisting);
                }
            }
            imageListView.ItemsSource = sources;
        }

        public async static Task<bool> FolderExists(string foldername)
        {
            IFolder folder = FileSystem.Current.LocalStorage;
            ExistenceCheckResult folderCheckResult = await folder.CheckExistsAsync(foldername);
            if (folderCheckResult == ExistenceCheckResult.FolderExists)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ImageListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var x = imageListView.SelectedItem as SourceOfImage;
            string imgToDelete = x.ImageName;
            testLabel.Text = "Image to delete: " + imgToDelete;
            DeleteImg(imgToDelete);
        }

        public async void DeleteImg(string imgName)
        {
            IFolder rootFolder = FileSystem.Current.LocalStorage;
            IFolder scanned = await rootFolder.GetFolderAsync("Scanned");
            IFile imgToDelete = await scanned.GetFileAsync(imgName);
            await imgToDelete.DeleteAsync();
        }
    }
}