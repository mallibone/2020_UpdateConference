using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly:
    Xamarin.Forms.ExportRenderer(typeof(Xamarin.Forms.Page), typeof(SignalR.Local.Mobile.iOS.LifecyclePageRenderer))]

namespace SignalR.Local.Mobile.iOS
{
    // a hacky PageRenderer subclass that uses the correct hook (ViewWillAppear rather than ViewDidAppear) for the Page.Appearing event on iOS
    // TODO: remove this once XF life cycle is fixed (see https://forums.xamarin.com/discussion/84510/proposal-improved-life-cycle-support)
    public sealed class LifecyclePageRenderer : PageRenderer
    {
        private static readonly FieldInfo AppearedField = typeof(PageRenderer).GetField("_appeared", BindingFlags.NonPublic | BindingFlags.Instance);
        private static readonly FieldInfo DisposedField = typeof(PageRenderer).GetField("_disposed", BindingFlags.NonPublic | BindingFlags.Instance);

        private IPageController? PageController => this.Element as IPageController;

        private bool Appeared
        {
            get => (bool)AppearedField.GetValue(this);
            set => AppearedField.SetValue(this, value);
        }

        private bool Disposed
        {
            get => (bool)DisposedField.GetValue(this);
            set => DisposedField.SetValue(this, value);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (this.Appeared || this.Disposed)
            {
                return;
            }

            // by setting this to true, we also ensure that PageRenderer does not invoke SendAppearing a second time when ViewDidAppear fires
            this.Appeared = true;
            PageController?.SendAppearing();
        }
    }
}