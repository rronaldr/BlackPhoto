using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BlackPhoto
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            ScanLocationLabel.Text = "Scanning: " + DependencyService.Get<ILoadImage>().GetScanPath();
        }

        private void Scan(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new TabPage());
        }
    }
}
