using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace XamarinNetCore
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage(IServiceProvider serviceProvider)
        {
            InitializeComponent();
        }
    }
}
