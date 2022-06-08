using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace svetotelegraf
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TbPage : TabbedPage
    {
        public TbPage()
        {
            InitializeComponent();
            main_page.Title = new StringBuilder(AppResources.main_tab, AppResources.main_tab.Length + 4)
                                .Insert(0, "📢 ").Append(" 🔦").ToString();
            settings_page.Title = new StringBuilder(AppResources.settings_tab, AppResources.settings_tab.Length + 2)
                                .Append(" 📻").ToString();
        }
    }
}