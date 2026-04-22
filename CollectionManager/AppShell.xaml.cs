namespace CollectionManager
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("ItemEditPage", typeof(Views.EditItemPage));
        }
    }
}
