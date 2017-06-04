using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BlackPhoto
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeletedPage : ContentPage
    {
        public DeletedPage()
        {
            InitializeComponent();
            if (DependencyService.Get<IGetExternalStorage>().VerifyExternalStorage())
            {
                TestLabel.Text = "SDcard mounted";
            }
            else
            {
                TestLabel.Text = "SDcard not mounted";
            }
        }
    }
}