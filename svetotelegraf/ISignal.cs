using System.Threading.Tasks;

namespace svetotelegraf
{
    public interface ISignal
    {
        void ReportProgress(float progress);

        Task flash(int len, bool sound);
        
        void BeginService(string headl);
        void ShowServicePaused();
        void EndService();

        bool OtherMusicExists { get; }

        void ShowBottomPopup(string text);       
    }
}
