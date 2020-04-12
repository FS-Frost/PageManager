using PageManager.Core;
using PageManager.Gui.Classes;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace PageManager.Gui.Views {
    public partial class EditPost : Window {
        readonly Connector Connector;
        readonly Post Post;

        public EditPost(Connector connector, ref Post post) {
            InitializeComponent();
            Connector = connector;
            this.SetCurrentSizeToMin();

            // Set current post data
            Post = post;
            txtMessage.Text = post.Message;
            txtLink.Text = post.AttachedLink;

            // Events
            btnCancel.Click += BtnCancel_Click;
            btnUpdate.Click += BtnUpdate_Click;
        }

        private Task UpdatePost(string message) {
            DisableGui();
            var id = Post.Id;

            return Task.Run(async () => {
                SuccessResponse response = await Connector.UpdatePost(id, message);
                
                Dispatcher.Invoke(() => {
                    if (response.Success) {
                        MessageBox.Show("Publicación actualizada.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                        Close();
                    }
                    else {
                        MessageBox.Show(
                            $"Se produjo un error al actualizar:\n{response.Error}",
                            "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    Post.Message = message;
                    EnableGui();
                });
            });
        }

        private void EnableGui() {
            btnUpdate.IsEnabled = true;
            btnCancel.IsEnabled = true;
            txtMessage.IsEnabled = true;
            txtLink.IsEnabled = true;
        }

        private void DisableGui() {
            btnUpdate.IsEnabled = false;
            btnCancel.IsEnabled = false;
            txtMessage.IsEnabled = false;
            txtLink.IsEnabled = false;
        }

        private async void BtnUpdate_Click(object sender, RoutedEventArgs e) {
            var message = txtMessage.Text;

            if (string.IsNullOrEmpty(message)) {
                MessageBox.Show("Se debe publicar un mensaje.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            try {
                await UpdatePost(message);
            }
            catch (Exception) {
                MessageBox.Show(
                    $"Se produjo un error desconocido al actualizar.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}
