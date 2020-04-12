using PageManager.Core;
using System;
using System.Threading.Tasks;
using System.Windows;
using PageManager.Gui.Classes;

namespace PageManager.Gui.Views {
    public partial class CreatePost : Window {
        readonly Connector Connector;
        public Post NewPost { get; set; }

        public CreatePost(Connector connector) {
            InitializeComponent();
            Connector = connector;
            this.SetCurrentSizeToMin();
            
            // Examples
            txtMessage.Text = "Publicación de prueba.";
            txtLink.Text = "http://www.syncrajo.net/2019/12/bd-clannad-serie-completa.html";

            // Events
            btnCancel.Click += BtnCancel_Click;
            btnPost.Click += BtnPost_Click;
        }

        private Task CreateNewPost(string message, string link) {
            DisableGui();
            
            return Task.Run(async () => {
                CreatePostResponse createPostReponse = await Connector.CreatePost(message, link);

                Dispatcher.Invoke(() => {
                    if (createPostReponse.Error == null) {
                        NewPost = createPostReponse.ToPost();
                        MessageBox.Show(
                            "Publicación creada.",
                            "Éxito",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information
                        );
                        Close();
                    }
                    else {
                        MessageBox.Show(
                            $"Se produjo un error al publicar:\n{createPostReponse.Error}",
                            "Error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error
                        );
                    }

                    EnableGui();
                });
            });
        }

        private void EnableGui() {
            btnPost.IsEnabled = true;
            btnCancel.IsEnabled = true;
            txtMessage.IsEnabled = true;
            txtLink.IsEnabled = true;
        }

        private void DisableGui() {
            btnPost.IsEnabled = false;
            btnCancel.IsEnabled = false;
            txtMessage.IsEnabled = false;
            txtLink.IsEnabled = false;
        }

        private async void BtnPost_Click(object sender, RoutedEventArgs e) {
            var message = txtMessage.Text;
            var link = string.IsNullOrEmpty(txtLink.Text) ? null : txtLink.Text;

            if (string.IsNullOrEmpty(message)) {
                MessageBox.Show("Se debe publicar un mensaje.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            try {
                await CreateNewPost(message, link);
            }
            catch (Exception) {
                MessageBox.Show(
                    $"Se produjo un error desconocido al publicar.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}
