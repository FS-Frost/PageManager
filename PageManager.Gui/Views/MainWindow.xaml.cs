using System.Threading.Tasks;
using System.Windows;
using PageManager.Gui.Classes;
using PageManager.Core;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.ComponentModel;
using PageManager.Gui.Views;

namespace PageManager.Gui {
    public partial class MainWindow : Window {
        readonly Config Config;
        readonly Connector Connector;
        readonly ObservableCollection<Post> Posts;

        public MainWindow() {
            InitializeComponent();
            Posts = new ObservableCollection<Post>();
            Config = Utils.DeserializeJsonFile<Config>("config.json");
            Connector = new Connector {
                AccessToken = Config.AccessToken,
                DataLimit = Config.DataLimit,
                PrettyJson = Config.PrettyJson
            };
            
            // Init data grid
            dgPosts.ItemsSource = Posts;
            dgPosts.HeadersVisibility = DataGridHeadersVisibility.Column;
            dgPosts.AutoGenerateColumns = true;

            // Context menu
            dgPosts.ContextMenu = new ContextMenu();

            var menuEdit = new MenuItem() { Header = "Editar" };
            menuEdit.Click += MenuEdit_Click;
            dgPosts.ContextMenu.Items.Add(menuEdit);

            var menuDelete = new MenuItem() { Header = "Eliminar" };
            menuDelete.Click += MenuDelete_Click;
            dgPosts.ContextMenu.Items.Add(menuDelete);

            // Events
            btnGetPosts.Click += BtnGetPosts_Click;
            btnCreate.Click += BtnCreate_Click;
            menuAbout.Click += MenuAbout_Click;

            dgPosts.AutoGeneratingColumn += DgPosts_AutoGeneratingColumn;
            dgPosts.ContextMenuOpening += DgPosts_ContextMenuOpening;

            if (Config.LoadDataAtStartup) {
                GetPosts();
            }
        }

        private Task GetPosts() {
            DisableGui();
            Posts.Clear();

            return Task.Run(async () => {
                GetPostsResponse response = await Connector.GetLatestPosts();
                response.SaveToJson("Latest posts.json");
                Dispatcher.Invoke(() => {
                    foreach (Post p in response.Posts) {
                        Posts.Add(p);
                    }

                    EnableGui();
                });
            });
        }

        private Task DeletePost(Post post) {
            DisableGui();

            return Task.Run(async () => {
                SuccessResponse response = await Connector.DeletePost(post.Id);
                Dispatcher.Invoke(() => {
                    if (response.Error == null) {
                        MessageBox.Show("Publicación eliminada.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                        Posts.Remove(post);
                    }
                    else {
                        MessageBox.Show(
                            $"Se produjo un error al eliminar:\n{response.Error}",
                            "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    EnableGui();
                });
            });
        }

        private void EnableGui() {
            dgPosts.IsEnabled = true;
            btnGetPosts.IsEnabled = true;
            btnCreate.IsEnabled = true;
        }

        private void DisableGui() {
            dgPosts.IsEnabled = false;
            btnGetPosts.IsEnabled = false;
            btnCreate.IsEnabled = false;
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e) {
            var window = new CreatePost(Connector);
            window.ShowDialog();

            if (window.NewPost != null) {
                Posts.Insert(0, window.NewPost);
            }
        }

        private void MenuEdit_Click(object sender, RoutedEventArgs e) {
            var menuItem = (MenuItem)sender;
            var contextMenu = (ContextMenu)menuItem.Parent;
            var item = (DataGrid)contextMenu.PlacementTarget;
            var selectedPost = (Post)item.SelectedCells[0].Item;

            var window = new EditPost(Connector, ref selectedPost);
            window.ShowDialog();
            dgPosts.Items.Refresh();
        }

        private async void MenuDelete_Click(object sender, RoutedEventArgs e) {
            var menuItem = (MenuItem)sender;
            var contextMenu = (ContextMenu)menuItem.Parent;
            var item = (DataGrid)contextMenu.PlacementTarget;
            var selectedPost = (Post)item.SelectedCells[0].Item;

            var shortLength = selectedPost.Message.Length > 50 ? 50 : selectedPost.Message.Length;
            MessageBoxResult result = MessageBox.Show(
                $"¿Eliminar esta publicación?\n" +
                $"«{selectedPost.Message.Substring(0, shortLength)}».",
                "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes) {
                return;
            }

            DisableGui();
            await DeletePost(selectedPost);
            EnableGui();
        }

        private void DgPosts_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e) {
            if (e.PropertyDescriptor is PropertyDescriptor descriptor) {
                e.Column.Header = descriptor.DisplayName ?? descriptor.Name;
            }
        }

        private void DgPosts_ContextMenuOpening(object sender, ContextMenuEventArgs e) {
            if (dgPosts.Items == null || dgPosts.Items.Count == 0 || dgPosts.SelectedItem == null) {
                e.Handled = true;
            }
        }

        private async void BtnGetPosts_Click(object sender, RoutedEventArgs e) {
            await GetPosts();
        }

        private void MenuAbout_Click(object sender, RoutedEventArgs e) {
            var repoUrl = "https://github.com/FS-Frost/PageManager";
            MessageBoxResult result = MessageBox.Show(
                "Desarrollado por Eduardo Hinojosa ([FS] Frost)\n" +
                $"Repositorio: {repoUrl}\n\n" +
                "¿Copiar URL?",
                "Acerca de",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.Yes) {
                Clipboard.SetText(repoUrl);
            }
        }
    }
}
