using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace svetotelegraf
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new TbPage();
        }

        protected override void OnStart()
        {
            LetterSettings.WeightedAvg();
        }
    }
}
